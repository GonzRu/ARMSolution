using CoreLib.Models.Common;
using CoreLib.Models.Common.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLib.ExchangeProviders
{
    public interface IExchangeProvider
    {
        #region Events

        /// <summary>
        /// Событие об изменении состояния канала связи
        /// </summary>
        event Action<bool> ConnectionStateChanged;

        /// <summary>
        /// Событие изменения значений тегов
        /// </summary>
        event Action<Dictionary<string, TagValue>> TagsValuesUpdated;

        #endregion

        #region Public properties

        /// <summary>
        /// Состояние связи с роутером
        /// </summary>
        bool IsConnectionStateOpened { get; set; }

        #endregion

        #region Работы с пользователями

        /// <summary>
        /// Авторизация
        /// </summary>
        void Authorization(string userName, string userPass, bool isFirstLogin);

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        List<User> GetUsersList();

        /// <summary>
        /// Получить список групп пользователей
        /// </summary>
        List<UserGroup> GetUserGroupsList();

        /// <summary>
        /// Создание группы пользователей
        /// </summary>
        Boolean CreateUserGroup(string groupName, string groupComment, string groupRight);

        /// <summary>
        /// Создание пользователя
        /// </summary>
        Boolean CreateUser(string userName, string userPassword, string userComment, Int32 userGroupId);

        #endregion

        #region Текущие данные

        /// <summary>
        /// Подписатсья на обновление тегов
        /// </summary>
        void SubscribeToTagsValuesUpdate(List<string> tagsListToSubscribe);

        /// <summary>
        /// Отписаться от обновления тегов
        /// </summary>
        void UnSubscribeToTagsValuesUpdate(List<string> tagsListToUnSubscribe);

        #endregion

        #region Работа с событиями

        #region Запрос событий

        /// <summary>
        /// Получение событий
        /// </summary>
        List<EventValue> GetEvents(DateTime dateTimeFrom, DateTime dateTimeTo, bool needSystemEvents, bool needUserEvents, bool needTerminalEvents, List<Tuple<UInt16, UInt32>> requestDevicesList);

        #endregion

        #region Получение данных событий

        /// <summary>
        /// Получить ссылку на осциллограмму по её номеру
        /// </summary>
        string GetOscillogramAsUrlByID(UInt16 dsGuid, Int32 eventDataID);

        /// <summary>
        /// Получить zip архив с осциллограммами как кортеж массива байтов и имени архива
        /// </summary>
        Tuple<byte[], string> GetOscillogramAsByteArray(UInt16 dsGuid, Int32 eventDataID);

        /// <summary>
        /// Получить архивную информацию (аварии, уставки и т.д.) как словарь значений
        /// </summary>
        Dictionary<string, TagValue> GetHistoricalDataByID(UInt16 dsGuid, Int32 dataID);

        #endregion

        #region Работа с квитированием

        #region По всем DS

        /// <summary>
        /// Проверяет есть ли не квитированные сообщения
        /// </summary>
        Boolean IsNotReceiptedEventsExist();

        /// <summary>
        /// Получить все не квитированные сообщения
        /// </summary>
        List<EventValue> GetNotReceiptedEvents();

        /// <summary>
        /// Квитировать все собщения
        /// </summary>
        void ReceiptAllEvents(String receiptComment);

        /// <summary>
        /// Квитировать сообщения
        /// </summary>
        void ReceiptEvents(List<EventValue> eventValues, String receiptComment);

        #endregion

        #endregion

        #endregion

        #region Работа с документами

        #region Методы для работы с существующими документами

        /// <summary>
        /// Получить список документов терминала
        /// </summary>
        List<Document> GetDocumentsList(UInt16 dsGuid, Int32 devGuid);

        /// <summary>
        /// Получить содержимое документа и его имя
        /// </summary>
        Tuple<byte[], string> GetDocument(UInt16 dsGuid, Int32 documentId);

        #endregion

        #region Методы для загрузки документов

        /// <summary>
        /// Загрузить файла на сервер
        /// </summary>
        bool LoadFile(UInt16 dsGuid, Int32 devGuid, string fileName, string comment);

        #endregion

        #endregion

        #region Ручной ввод данных

        #region Ручной ввод значений тегов

        /// <summary>
        /// Установить значение тега 
        /// с уровня HMI через тип object
        /// (качество тега vqHandled)
        /// </summary>
        void SetTagValueFromHMI(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, object valinobject);

        /// <summary>
        /// восстановить процесс естесвенного обновления тега
        /// (качество тега vqGood или по факту)
        /// </summary>
        void ReSetTagValueFromHMI(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);

        #endregion

        #region Ручной ввод преобразовывающих коэффициентов

        /// <summary>
        /// Получить коэффициент преобразования для тега
        /// </summary>
        Object GetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);
        Task<Object> GetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);

        /// <summary>
        /// Установить коэффициент преобразования
        /// </summary>
        void SetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, Object transformationRatio);
        Task SetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, Object transformationRatio);

        /// <summary>
        /// Сбросить коэффициент преобразования
        /// </summary>
        void ReSetTagAnalogTransformationRatio(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);
        Task ReSetTagAnalogTransformationRatioAsync(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);

        /// <summary>
        /// Возвращает true, если значение дискретного тега инвертируется
        /// </summary>
        bool? IsInverseTagDiscret(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid);

        /// <summary>
        /// Инвертирует значение дискретного тега
        /// </summary>
        void InverseTagDiscret(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, Boolean newInverseValue);

        #endregion

        #endregion

        #region Тренды

        /// <summary>
        /// Получить список тегов, у которых включена запись значений
        /// </summary>
        List<string> GetTagsListWithEnabledTrendSave();

        /// <summary>
        /// Получить доступные диапозоны значений тренда
        /// </summary>
        List<Tuple<DateTime, DateTime>> GetTrendDateTimeRanges(ushort dsGuid, uint devGuid, uint tagGuid);

        /// <summary>
        /// Получить тренд единым списком
        /// </summary>
        List<Tuple<DateTime, object>> GetTagTrend(ushort dsGuid, uint devGuid, uint tagGuid, DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Получить список обособленных трендов
        /// </summary>
        List<List<Tuple<DateTime, object>>> GetTagTrendsList(ushort dsGuid, uint devGuid, uint tagGuid, DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Получить настройки режима работы записи тренда
        /// </summary>
        TrendSettings GetTrendSettings(ushort dsGuid, uint devGuid, uint tagGuid);

        /// <summary>
        /// Установить настройки режима работы записи тренда
        /// </summary>
        void SetTrendSettings(ushort dsGuid, uint devGuid, uint tagGuid, TrendSettings trendSettings);

        #endregion

        #region Отчеты

        /// <summary>
        /// Получить отчет
        /// </summary>
        string GetReport(BaseReportSettings reportSettings);

        /// <summary>
        /// Получить отчет как байтовый массив
        /// </summary>
        byte[] GetReportAsByteArray(BaseReportSettings reportSettings);

        #endregion
    }
}
