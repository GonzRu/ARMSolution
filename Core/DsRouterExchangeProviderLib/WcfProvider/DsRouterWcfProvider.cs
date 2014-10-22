using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using CoreLib.Models.Common.Reports;
using CoreLib.Models.Configuration;
using DsRouterExchangeProviderLib.DSRouterService;

namespace DsRouterExchangeProviderLib.WcfProvider
{
    public class DsRouterWcfProvider : IExchangeProvider
    {
        #region Events

        /// <summary>
        /// Событие об изменении состояния канала связи
        /// </summary>
        public event Action<bool> ConnectionStateChanged;

        /// <summary>
        /// Событие изменения значений тегов
        /// </summary>
        public event Action<Dictionary<string, TagValue>> TagsValuesUpdated;

        #endregion

        #region Private fields

        /// <summary>
        /// Прокси WCF-сервиса роутера
        /// </summary>
        private DSRouterClient _dsRouterProxy;

        /// <summary>
        /// IP адресс сервиса
        /// </summary>
        private string _ipAddress;

        /// <summary>
        /// Порт сервиса
        /// </summary>
        private string _port;

        /// <summary>
        /// Таймер для установления связи
        /// </summary>
        private Timer OpenConnectionTimer;

        /// <summary>
        /// Таймер для запроса обновлений от роутера
        /// </summary>
        private Timer GetTagsValuesUpdatedTimer;

        /// <summary>
        /// Таймер для периодического пингования роутера
        /// </summary>
        private Timer PingPongTimer;

        /// <summary>
        /// Список тегов, на которые подписан клиент
        /// </summary>
        private Dictionary<string, int> _subscribedTags;

        #endregion

        #region Constructor

        public DsRouterWcfProvider(string ipAddress, string port)
        {
            _ipAddress = ipAddress;
            _port = port;

            OpenConnectionTimer = new Timer();
            OpenConnectionTimer.Interval = 2000;
            OpenConnectionTimer.Elapsed += OpenConnectionTimerOnElapsed;

            PingPongTimer = new Timer();
            PingPongTimer.Interval = 3000;
            PingPongTimer.Elapsed += PingPongTimerOnElapsed;

            GetTagsValuesUpdatedTimer = new Timer();
            GetTagsValuesUpdatedTimer.Interval = 1000;
            GetTagsValuesUpdatedTimer.Elapsed += GetTagsValuesUpdatedTimerOnElapsed;

            _subscribedTags = new Dictionary<string, int>();

            InitProxy();
        }

        #endregion

        #region Implemenation IExchangeProvider

        #region Public properties

        /// <summary>
        /// Состояние связи с роутером
        /// </summary>
        bool IExchangeProvider.IsConnectionStateOpened { get; set; }

        #endregion

        #region Работы с пользователями

        /// <summary>
        /// Авторизация
        /// </summary>
        public void Authorization(string userName, string userPass, bool isFirstLogin)
        {
            try
            {
                _dsRouterProxy.Authorization(userName, userPass, isFirstLogin);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        public List<User> GetUsersList()
        {
            List<User> result = null;

            try
            {
                var dsRouterUsers = _dsRouterProxy.GetUsersList();

                result = dsRouterUsers.ConvertAll(input => DsRouterUserToUser(input));
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Получить список групп пользователей
        /// </summary>
        public List<UserGroup> GetUserGroupsList()
        {
            List<UserGroup> result = null;

            try
            {
                var dsRouterUserGroups = _dsRouterProxy.GetUserGroupsList();

                result = dsRouterUserGroups.ConvertAll(input => DsRouterUserGroupToUserGroup(input));
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Создание группы пользователей
        /// </summary>
        public Boolean CreateUserGroup(string groupName, string groupComment, string groupRight)
        {
            var result = false;

            try
            {
                result = _dsRouterProxy.CreateUserGroup(groupName, groupComment, groupRight);
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        public Boolean CreateUser(string userName, string userPassword, string userComment, Int32 userGroupId)
        {
            var result = false;

            try
            {
                result = _dsRouterProxy.CreateUser(userName, userPassword, userComment, userGroupId);
            }
            catch (Exception)
            {
            }

            return result;
        }

        #endregion

        #region Текущие данные

        void IExchangeProvider.SubscribeToTagsValuesUpdate(List<string> tagsListToSubscribe)
        {
            tagsListToSubscribe.ForEach(s =>
            {
                if (_subscribedTags.ContainsKey(s))
                    _subscribedTags[s]++;
                else
                    _subscribedTags.Add(s, 1);
            });

            DoFullRequest();
        }

        void IExchangeProvider.UnSubscribeToTagsValuesUpdate(List<string> tagsListToUnSubscribe)
        {
            tagsListToUnSubscribe.ForEach(s =>
            {
                if (_subscribedTags.ContainsKey(s))
                    if (_subscribedTags[s] > 1)
                        _subscribedTags[s]--;
                    else
                        _subscribedTags.Remove(s);
            });

            DoFullRequest();
        }

        #endregion

        #region Работа с событиями

        #region Запрос событий

        /// <summary>
        /// Получение событий
        /// </summary>
        List<EventValue> IExchangeProvider.GetEvents(DateTime dateTimeFrom, DateTime dateTimeTo, bool needSystemEvents, bool needUserEvents, bool needTerminalEvents, List<Tuple<UInt16, UInt32>> requestDevicesList)
        {
            try
            {
                var dsRouterEvents = _dsRouterProxy.GetEvents(dateTimeFrom, dateTimeTo, needSystemEvents, needUserEvents, needTerminalEvents, requestDevicesList);

                return (from dsRouterEvent in dsRouterEvents
                    select
                        new EventValue
                        {
                            DsGuid = dsRouterEvent.DsGuid,
                            DevGuid = dsRouterEvent.DevGuid,
                            EventDataID = dsRouterEvent.EventDataID,
                            EventID = dsRouterEvent.EventID,
                            EventSourceComment = dsRouterEvent.EventSourceComment,
                            EventSourceName = dsRouterEvent.EventSourceName,
                            EventText = dsRouterEvent.EventText,
                            EventTime = dsRouterEvent.EventTime,
                            IsNeedReceipt = dsRouterEvent.IsNeedReceipt,
                            IsReceipted = dsRouterEvent.IsReceipted,
                        }).ToList();
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion

        #region Получение данных событий

        /// <summary>
        /// Получить ссылку на осциллограмму по её номеру
        /// </summary>
        string IExchangeProvider.GetOscillogramAsUrlByID(UInt16 dsGuid, Int32 eventDataID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить zip архив с осциллограммами как кортеж массива байтов и имени архива
        /// </summary>
        Tuple<byte[], string> IExchangeProvider.GetOscillogramAsByteArray(UInt16 dsGuid, Int32 eventDataID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить архивную информацию (аварии, уставки и т.д.) как словарь значений
        /// </summary>
        Dictionary<string, TagValue> IExchangeProvider.GetHistoricalDataByID(UInt16 dsGuid, Int32 dataID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Работа с квитированием

        #region По всем DS

        /// <summary>
        /// Проверяет есть ли не квитированные сообщения
        /// </summary>
        Boolean IExchangeProvider.IsNotReceiptedEventsExist()
        {
            try
            {
                return _dsRouterProxy.IsNotReceiptedEventsExist();
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Получить все не квитированные сообщения
        /// </summary>
        List<EventValue> IExchangeProvider.GetNotReceiptedEvents()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Квитировать все собщения
        /// </summary>
        void IExchangeProvider.ReceiptAllEvents(String receiptComment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Квитировать сообщения
        /// </summary>
        void IExchangeProvider.ReceiptEvents(List<EventValue> eventValues, String receiptComment)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #endregion

        #region Ручной ввод данных

        #region Ручной ввод значений тегов

        /// <summary>
        /// Установить значение тега 
        /// с уровня HMI через тип object
        /// (качество тега vqHandled)
        /// </summary>
        void IExchangeProvider.SetTagValueFromHMI(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, object valinobject)
        {
            try
            {
                _dsRouterProxy.SetTagValueFromHMI(dsGuid, devGuid, tagGuid, valinobject);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// восстановить процесс естесвенного обновления тега
        /// (качество тега vqGood или по факту)
        /// </summary>
        void IExchangeProvider.ReSetTagValueFromHMI(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            try
            {
                _dsRouterProxy.ReSetTagValueFromHMI(dsGuid, devGuid, tagGuid);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Ручной ввод преобразовывающих коэффициентов

        /// <summary>
        /// Получить коэффициент преобразования для тега
        /// </summary>
        Object IExchangeProvider.GetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            try
            {
                return _dsRouterProxy.GetTagAnalogTransformationRatio(dsGuid, devGuid, tagGuid);
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        Task<Object> IExchangeProvider.GetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            return _dsRouterProxy.GetTagAnalogTransformationRatioAsync(dsGuid, devGuid, tagGuid);
        }

        /// <summary>
        /// Установить коэффициент преобразования
        /// </summary>
        void IExchangeProvider.SetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid,
            Object transformationRatio)
        {
            try
            {
                _dsRouterProxy.SetTagAnalogTransformationRatio(dsGuid, devGuid, tagGuid, transformationRatio);
            }
            catch (Exception)
            {
            }
        }

        Task IExchangeProvider.SetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, Object transformationRatio)
        {
            return _dsRouterProxy.SetTagAnalogTransformationRatioAsync(dsGuid, devGuid, tagGuid, transformationRatio);
        }

        /// <summary>
        /// Сбросить коэффициент преобразования
        /// </summary>
        void IExchangeProvider.ReSetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            try
            {
                _dsRouterProxy.ReSetTagAnalogTransformationRatio(dsGuid, devGuid, tagGuid);
            }
            catch (Exception)
            {
            }
        }

        Task IExchangeProvider.ReSetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            return _dsRouterProxy.ReSetTagAnalogTransformationRatioAsync(dsGuid, devGuid, tagGuid);
        }

        /// <summary>
        /// Возвращает true, если значение дискретного тега инвертируется
        /// </summary>
        bool? IExchangeProvider.IsInverseTagDiscret(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            try
            {
                return _dsRouterProxy.IsInverseDiscretTag(dsGuid, devGuid, tagGuid);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Инвертирует значение дискретного тега
        /// </summary>
        void IExchangeProvider.InverseTagDiscret(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, Boolean newInverseValue)
        {
            try
            {
                _dsRouterProxy.InverseDiscretTag(dsGuid, devGuid, tagGuid, newInverseValue);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #endregion

        #region Тренды

        /// <summary>
        /// Получить список тегов, у которых включена запись значений
        /// </summary>
        public List<string> GetTagsListWithEnabledTrendSave()
        {
            try
            {
                return _dsRouterProxy.GetTagsListWithEnabledTrendSave();
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Получить доступные диапозоны значений тренда
        /// </summary>
        public List<Tuple<DateTime, DateTime>> GetTrendDateTimeRanges(ushort dsGuid, uint devGuid, uint tagGuid)
        {
            try
            {
                return _dsRouterProxy.GetTrendDateTimeRanges(dsGuid, devGuid, tagGuid);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Получить тренд единым списком
        /// </summary>
        public List<Tuple<DateTime, object>> GetTagTrend(ushort dsGuid, uint devGuid, uint tagGuid, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                return _dsRouterProxy.GetTagTrend(dsGuid, devGuid, tagGuid, startDateTime, endDateTime);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Получить список обособленных трендов
        /// </summary>
        public List<List<Tuple<DateTime, object>>> GetTagTrendsList(ushort dsGuid, uint devGuid, uint tagGuid, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                return _dsRouterProxy.GetTagTrendsList(dsGuid, devGuid, tagGuid, startDateTime, endDateTime);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Получить настройки режима работы записи тренда
        /// </summary>
        public TrendSettings GetTrendSettings(ushort dsGuid, uint devGuid, uint tagGuid)
        {
            try
            {
                var result = _dsRouterProxy.GetTrendSettings(dsGuid, devGuid, tagGuid);

                if (result != null)
                return new TrendSettings
                {
                    Enable = result.Enable,
                    Sample = result.Sample,
                    AbsoluteError = result.AbsoluteError,
                    RelativeError = result.RelativeError,
                    MaxCacheMinutes = result.MaxCacheMinutes,
                    MaxCacheValuesCount = result.MaxCacheValuesCount
                };
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Установить настройки режима работы записи тренда
        /// </summary>
        public void SetTrendSettings(ushort dsGuid, uint devGuid, uint tagGuid, TrendSettings trendSettings)
        {
            try
            {
                var dsRouterTrendSettings = new DSRouterTrendSettings
                {
                    Enable = trendSettings.Enable,
                    Sample = trendSettings.Sample,
                    AbsoluteError = trendSettings.AbsoluteError,
                    RelativeError = trendSettings.RelativeError,
                    MaxCacheMinutes = trendSettings.MaxCacheMinutes,
                    MaxCacheValuesCount = trendSettings.MaxCacheValuesCount
                };

                _dsRouterProxy.SetTrendSettings(dsGuid, devGuid, tagGuid, dsRouterTrendSettings);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Отчеты

        /// <summary>
        /// Получить отчет
        /// </summary>
        public string GetReport(BaseReportSettings reportSettings)
        {
            string result = null;

            try
            {
                if (reportSettings is EventsReportSettings)
                {
                    var eventsReportSettings = reportSettings as EventsReportSettings;

                    var dsRouterEventsReportSettings = new DSRouterEventsReportSettings()
                    {
                        ReportExtension = (DSRouterReportExtension)eventsReportSettings.ReportExtension,
                        StartDateTime = eventsReportSettings.StartDateTime,
                        EndDateTime = eventsReportSettings.EndDateTime,
                        DeviceGuid = eventsReportSettings.DeviceGuid,
                        DsGuid = eventsReportSettings.DsGuid
                    };

                    result = _dsRouterProxy.GetEventsReport(dsRouterEventsReportSettings);
                }

                if (reportSettings is TagsReportSettings)
                {
                    var tagsReportSettings = reportSettings as TagsReportSettings;

                    var dsRouterTagsReportSettings = new DSRouterTagsReportSettings
                    {
                        ReportTamplateName = tagsReportSettings.ReportTamplateName,
                        ReportExtension = (DSRouterReportExtension)tagsReportSettings.ReportExtension,
                        StartDateTime = tagsReportSettings.StartDateTime,
                        EndDateTime = tagsReportSettings.EndDateTime,
                        Tags = tagsReportSettings.Tags,
                        Interval = tagsReportSettings.Interval
                    };

                    result = _dsRouterProxy.GetTagsReport(dsRouterTagsReportSettings);
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// Получить отчет как байтовый массив
        /// </summary>
        public byte[] GetReportAsByteArray(BaseReportSettings reportSettings)
        {
            byte[] result = null;

            try
            {
                if (reportSettings is EventsReportSettings)
                {
                    var eventsReportSettings = reportSettings as EventsReportSettings;

                    var dsRouterEventsReportSettings = new DSRouterEventsReportSettings()
                    {
                        ReportExtension = (DSRouterReportExtension)eventsReportSettings.ReportExtension,
                        StartDateTime = eventsReportSettings.StartDateTime,
                        EndDateTime = eventsReportSettings.EndDateTime,
                        DeviceGuid = eventsReportSettings.DeviceGuid,
                        DsGuid = eventsReportSettings.DsGuid
                    };

                    result = _dsRouterProxy.GetEventsReportAsByteArray(dsRouterEventsReportSettings);
                }

                if (reportSettings is TagsReportSettings)
                {
                    var tagsReportSettings = reportSettings as TagsReportSettings;

                    var dsRouterTagsReportSettings = new DSRouterTagsReportSettings
                    {
                        ReportTamplateName = tagsReportSettings.ReportTamplateName,
                        ReportExtension = (DSRouterReportExtension)tagsReportSettings.ReportExtension,
                        StartDateTime = tagsReportSettings.StartDateTime,
                        EndDateTime = tagsReportSettings.EndDateTime,
                        Tags = tagsReportSettings.Tags,
                        Interval = tagsReportSettings.Interval
                    };

                    result = _dsRouterProxy.GetTagsReportAsByteArray(dsRouterTagsReportSettings);
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        #endregion

        #endregion

        #region Private metods

        #region Методы для иницилизации класса

        /// <summary>
        /// запускает процесс иницилизации соединения
        /// </summary>
        private void InitProxy()
        {
            try
            {
                OpenProxy();
            }
            catch (Exception)
            {
            }

            PingPongTimer.Start();
        }

        private void OpenProxy()
        {
            var netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;
            netTcpBinding.MaxReceivedMessageSize = int.MaxValue;
            netTcpBinding.MaxBufferSize = int.MaxValue;
            netTcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;

            var instanceContext = new InstanceContext(new DsRouterServiceCallback());
            var endPointAddress = new EndpointAddress(String.Format("net.tcp://{0}:{1}/DSRouter.DSRouterService/DSRouterService.svc", _ipAddress, _port));

            _dsRouterProxy = new DSRouterClient(instanceContext, netTcpBinding, endPointAddress);
            _dsRouterProxy.Open();

#warning Не забыть убрать!
            var res = _dsRouterProxy.Authorization("alex", "s", false);

            // Оповещаем подписчиков, что канал открыт
            (this as IExchangeProvider).IsConnectionStateOpened = true;
            OnConnectionStateChanged(true);

            // Делаем полный запрос, если есть что запрашивать
            DoFullRequest();

            // Запускаем запрос обновлений
            GetTagsValuesUpdatedTimer.Start();
        }

        #endregion

        #region Вспомогательные методы для работы класса

        /// <summary>
        /// Вызывает событие изменения состояния канала связи
        /// </summary>
        private void OnConnectionStateChanged(bool isConnectionStateOk)
        {
            if (ConnectionStateChanged != null)
                ConnectionStateChanged(isConnectionStateOk);
        }

        private void OnTagsValuesUpdated(Dictionary<string, TagValue> tagsValues)
        {
            if (TagsValuesUpdated != null)
                TagsValuesUpdated(tagsValues);
        }

        /// <summary>
        /// Делает полный запрос тегов к роутеру
        /// </summary>
        private void DoFullRequest()
        {
            if (_subscribedTags.Count == 0)
                return;

            GetTagsValuesUpdatedTimer.Stop();

            try
            {
                var result = _dsRouterProxy.GetTagsValue(_subscribedTags.Keys.ToList());
                var r = result.ToDictionary(pair => pair.Key,
                    pair =>
                        new TagValue
                        {
                            TagValueAsObject = pair.Value.VarValueAsObject,
                            TagValueQuality = (TagValueQuality)pair.Value.VarQuality
                        });
                 
                OnTagsValuesUpdated(r);
            }
            catch (Exception ex)
            {
            }

            GetTagsValuesUpdatedTimer.Start();
        }

        #endregion

        #region Вспомогательные методы для преобразования данных роутера к нужным данным

        #region Данные пользователя

        /// <summary>
        /// Преобразует класс DSRouterUserGroup к UserGroup
        /// </summary>
        private UserGroup DsRouterUserGroupToUserGroup(DSRouterUserGroup dsRouterUserGroup)
        {
            return new UserGroup
            {
                GroupId = dsRouterUserGroup.GroupID,
                GroupName = dsRouterUserGroup.GroupName,
                GroupComment = dsRouterUserGroup.GroupComment,
                CreateDateTime = dsRouterUserGroup.CreateDateTime,
                EditDateTime = dsRouterUserGroup.EditDateTime,
                GroupRight = dsRouterUserGroup.GroupRight
            };
        }

        /// <summary>
        /// Преобразует класс DSRouterUser к User
        /// </summary>
        private User DsRouterUserToUser(DSRouterUser dsRouterUser)
        {
            return new User
            {
                UserId = dsRouterUser.UserID,
                UserName = dsRouterUser.UserName,
                UserComment = "",
                CreateDateTime = dsRouterUser.CreateDateTime,
                EditDateTime = dsRouterUser.EditDateTime,
                UserGroup = DsRouterUserGroupToUserGroup(dsRouterUser.DsRouterUserGroup)
            };
        }

        #endregion

        #endregion

        #endregion

        #region Handlers

        /// <summary>
        /// Периодические попытки установить подключение с роутером
        /// </summary>
        private void OpenConnectionTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            OpenConnectionTimer.Stop();

            try
            {
                OpenProxy();
            }
            catch (Exception ex)
            {
                OpenConnectionTimer.Start();
                return;
            }

            PingPongTimer.Start();
        }

        /// <summary>
        /// Периодическое пингование роутера для проверки связи с ним
        /// </summary>
        private void PingPongTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            PingPongTimer.Stop();

            try
            {
                _dsRouterProxy.Ping();
            }
            catch (Exception ex)
            {
                (this as IExchangeProvider).IsConnectionStateOpened = false;
                OnConnectionStateChanged(false);

                GetTagsValuesUpdatedTimer.Stop();
                OpenConnectionTimer.Start();

                return;
            }

            PingPongTimer.Start();
        }

        /// <summary>
        /// Периодический запрос подключений
        /// </summary>
        private void GetTagsValuesUpdatedTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_subscribedTags.Count == 0)
                return;

            GetTagsValuesUpdatedTimer.Stop();

            Dictionary<string, DSRouterTagValue> result;
            try
            {
                result = _dsRouterProxy.GetTagsValuesUpdated();
                if (result.Count == 0)
                {
                    GetTagsValuesUpdatedTimer.Start();
                    return;
                }

                var r = result.ToDictionary(
                    pair => pair.Key,
                    pair =>
                        new TagValue
                        {
                            TagValueAsObject = pair.Value.VarValueAsObject,
                            TagValueQuality = (TagValueQuality)pair.Value.VarQuality
                        });

                OnTagsValuesUpdated(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DsRouterWcfProvider:GetTagsValuesUpdatedTimerOnElapsed - исключение " + ex.Message);
            }

            GetTagsValuesUpdatedTimer.Start();
        }

        #endregion
    }
}
