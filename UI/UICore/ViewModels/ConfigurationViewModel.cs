using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using ConfigurationParsersLib;
using CoreLib.Models.Configuration;
using UICore.Commands;

namespace UICore.ViewModels
{
    public abstract class ConfigurationViewModel : ViewModelBase
    {
        #region CONSTS

        #region Константы состояния выключателя для ручного режима

        /// <summary>
        /// Значение тега, соответствующее включенному положению
        /// </summary>
        protected float DEVICE_STATE_ON = 1;

        /// <summary>
        /// Значение тега, соответствующее отключенному положению
        /// </summary>
        protected float DEVICE_STATE_OFF = 2;

        #endregion

        #endregion

        #region Public properties

        #region Properties

        /// <summary>
        /// Конфигурация
        /// </summary>
        public Configuration Configuration { get; protected set; }

        /// <summary>
        /// Список моделей-представлний DS
        /// </summary>
        public List<DataServerViewModel> DataServers
        {
            get { return _dataServers; }
            set
            {
                _dataServers = value;
                NotifyPropertyChanged("DataServers");
            }
        }
        private List<DataServerViewModel> _dataServers;

        /// <summary>
        /// Индикатор показывающий - есть ли в системе неквитированные сообщения
        /// </summary>
        public bool IsNotReciptedEventsExist
        {
            get { return _isNotReciptedEventsExist; }
            set
            {
                _isNotReciptedEventsExist = value;
                NotifyPropertyChanged("IsNotReciptedEventsExist");
            }
        }
        private bool _isNotReciptedEventsExist;

        /// <summary>
        /// Состояние связи с роутером
        /// </summary>
        public bool IsConnectionStateOpened
        {
            get { return Configuration == null ? false : Configuration.DsRouterProvider.IsConnectionStateOpened; }
        }

        /// <summary>
        /// ip-адресс роутера
        /// </summary>
        public string IpAddress { get { return Configuration.DsRouterIpAddress; } }

        #endregion

        #region Commands

        /// <summary>
        /// Загурзка и сохранение конфигурации
        /// </summary>
        public Command LoadConfigurationCommand { get; private set; }
        public Command SaveConfigurationCommand { get; private set; }

        /// <summary>
        /// Ручная установка значений тегов
        /// </summary>
        public ICommand HandleSetOnDeviceStateCommand { get; private set; }
        public ICommand HandleSetOffDeviceStateCommand { get; private set; }
        public ICommand ReSetHandleDeviceStateCommand { get; private set; }

        public ICommand AuthorizationCommand { get; private set; }

        #endregion

        #endregion

        #region Private fields

        /// <summary>
        /// Источник конфигурации
        /// </summary>
        protected IConfigurationProvider ConfigurationProvider;

        #endregion

        #region Constructors

        protected ConfigurationViewModel(IConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;

            LoadConfigurationCommand = new AsyncCommand(LoadConfiguration);
            SaveConfigurationCommand = new AsyncCommand(SaveConfiguration);

            HandleSetOnDeviceStateCommand = new AsyncCommand(HandleSetOnDeviceState);
            HandleSetOffDeviceStateCommand = new AsyncCommand(HandleSetOffDeviceState);
            ReSetHandleDeviceStateCommand = new AsyncCommand(ReSetHandleDeviceState);

            AuthorizationCommand = new AsyncCommand(Authorization);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        #region Работа с конфигурацией

        /// <summary>
        /// Загружает конфигурацию
        /// </summary>
        protected virtual void LoadConfiguration()
        {
            ConfigurationProvider.LoadConfiguration();
            Configuration = ConfigurationProvider.GetConfiguration();

            Configuration.DsRouterConnectionStateChanged += b => NotifyPropertyChanged("IsConnectionStateOpened");

            NotifyPropertyChanged("IsConnectionStateOpened");
            NotifyPropertyChanged("DsRouterIpAddress");
        }

        /// <summary>
        /// Сохраняет изменения в конфигурации
        /// </summary>
        protected virtual void SaveConfiguration()
        {
            ConfigurationProvider.SaveConfiguration();
        }

        #endregion

        #region Ручная установка тега состояния выключателя

        /// <summary>
        /// Перевести выключатель в ручное состояние Вкл.
        /// </summary>
        /// <param name="param">Guid тега, отвечающего за состояние выключателя</param>
        private void HandleSetOnDeviceState(object param)
        {
            if (!(param is string) || String.IsNullOrWhiteSpace(param as string))
                return;

            var tag = Configuration.GetTag(param as string);
            if (tag == null)
                return;

            Configuration.DsRouterProvider.SetTagValueFromHMI(tag.Device.DataServer.DsGuid, tag.Device.DeviceGuid, tag.TagGuid, DEVICE_STATE_ON);
        }

        /// <summary>
        /// Перевести выключатель в ручное состояние Выкл.
        /// </summary>
        /// <param name="param">Guid тега, отвечающего за состояние выключателя</param>
        private void HandleSetOffDeviceState(object param)
        {
            if (!(param is string) || String.IsNullOrWhiteSpace(param as string))
                return;

            var tag = Configuration.GetTag(param as string);
            if (tag == null)
                return;

            Configuration.DsRouterProvider.SetTagValueFromHMI(tag.Device.DataServer.DsGuid, tag.Device.DeviceGuid, tag.TagGuid, DEVICE_STATE_OFF);
        }

        /// <summary>
        /// Сбросить ручной ввод состояния выключателя
        /// </summary>
        /// <param name="param">Guid тега, отвечающего за состояние выключателя</param>
        private void ReSetHandleDeviceState(object param)
        {
            if (!(param is string) || String.IsNullOrWhiteSpace(param as string))
                return;

            var tag = Configuration.GetTag(param as string);
            if (tag == null)
                return;

            Configuration.DsRouterProvider.ReSetTagValueFromHMI(tag.Device.DataServer.DsGuid, tag.Device.DeviceGuid, tag.TagGuid);
        }

        #endregion

        #region Авторизация пользователя

        /// <summary>
        /// Авторизирует пользователя по введенному логину и паролю
        /// </summary>
        protected abstract void Authorization();

        #endregion

        #endregion

        #region Вспомогательные методы для получение VM

        protected BaseDeviceViewModel GetDeviceViewModel(string deviceGuidAsStr)
        {
            var c = deviceGuidAsStr.Split('.');

            var dsGuid = UInt16.Parse(c[0]);
            var devGuid = UInt32.Parse(c[1]);

            var dataServerViewModel = (from ds in DataServers where ds.DsGuid == dsGuid select ds).FirstOrDefault();
            if (dataServerViewModel == null)
                return null;

            return
                (from deviceViewModel in dataServerViewModel.Devices
                    where deviceViewModel.DeviceGuid == devGuid
                    select deviceViewModel).FirstOrDefault();
        }

        protected BaseTagViewModel GetTagViewModel(string tagGuidAsStr)
        {
            var deviceViewModel = GetDeviceViewModel(tagGuidAsStr);
            if (deviceViewModel == null)
                return null;

            var c = tagGuidAsStr.Split('.');
            var tagGuid = UInt32.Parse(c[2]);

            return
                (from tagViewModel in deviceViewModel.Tags where tagViewModel.TagGuid == tagGuid select tagViewModel).FirstOrDefault();
        }

        #endregion

        #endregion
    }
}
