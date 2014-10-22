using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using CoreLib.Models.Configuration;
using System.ComponentModel;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    class DeviceViewModel : BaseDeviceViewModel
    {
        #region Public properties

        #region Properties

        /// <summary>
        /// Список групп, содержащих текущие данные
        /// </summary>
        [Browsable(false)]
        public List<BaseGroupViewModel> CurrentDataGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.CurrentData).ToList(); }
        }

        /// <summary>
        /// Список групп, содержащих уставки устройства
        /// </summary>
        [Browsable(false)]
        public List<BaseGroupViewModel> SettingsGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.Ustavki).ToList(); }
        }

        /// <summary>
        /// Список всех групп
        /// </summary>
        [Browsable(false)]
        public List<BaseGroupViewModel> SpecificGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.Specific).ToList(); }
        }

        #region События устройства

        /// <summary>
        /// Список событий данного устройства
        /// </summary>
        [Browsable(false)]
        public List<EventValue> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                NotifyPropertyChanged("Events");
            }
        }
        private List<EventValue> _events;

        /// <summary>
        /// Начало отсчета показываемых событий устройства
        /// </summary>
        public DateTime EventsStartDateTime { get; set; }

        /// <summary>
        /// Конец отсчета показываемых событий устройства
        /// </summary>
        public DateTime EventsEndDateTime { get; set; }

        #endregion

        #region Документы устройства



        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Загружает события данного устройства
        /// </summary>
        public ICommand LoadEventsCommand { get; set; }

        /// <summary>
        /// Загружает список документов данного устройства
        /// </summary>
        public ICommand LoadDocumentsListCommand { get; set; }

        /// <summary>
        /// Загружает документ устройства
        /// </summary>
        public ICommand LoadDocumentCommand { get; set; }

        #endregion

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider) : base(device, exchangeProvider)
        {
            EventsStartDateTime = DateTime.Now.AddDays(-1).Date;
            EventsEndDateTime = DateTime.Now.Date;

            LoadEventsCommand = new AsyncCommand(LoadEvents);
            LoadDocumentsListCommand = new AsyncCommand(LoadDocumentsList);
            LoadDocumentCommand = new Command(LoadDocument);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        /// <summary>
        /// Загружает события, соответствующие данному устройству
        /// </summary>
        private void LoadEvents()
        {
            Events = ExchangeProvider.GetEvents(EventsStartDateTime, EventsEndDateTime, false, false, true,
                new List<Tuple<ushort, uint>> {new Tuple<ushort, uint>(Device.DataServer.DsGuid, Device.DeviceGuid)});            
        }

        /// <summary>
        /// Загружает список документов, привязанных к данному устройству
        /// </summary>
        private void LoadDocumentsList()
        {
            
        }

        /// <summary>
        /// Загружает конкретный документ
        /// </summary>
        private void LoadDocument(object param)
        {
            if (param == null || !(param is EventValue))
                return;

            var eventValue = param as EventValue;
            if (eventValue.EventDataID == -1)
                return;
        }

        #endregion

        #endregion
    }
}
