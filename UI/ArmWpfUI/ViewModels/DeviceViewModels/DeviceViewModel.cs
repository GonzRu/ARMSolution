using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    public class DeviceViewModel : UICore.ViewModels.DeviceViewModel
    {
        #region Public properties

        #region Properties

        #region Подразделы устройства

        /// <summary>
        /// Текущие данные
        /// </summary>
        public DeviceDataViewModel CurrentDataViewModel { get; set; }

        /// <summary>
        /// Служебные данные
        /// </summary>
        public DeviceDataViewModel ServiceDataViewModel { get; set; }

        /// <summary>
        /// Специфические данные
        /// </summary>
        public DeviceDataViewModel SpecificDataViewModel { get; set; }

        /// <summary>
        /// Документы устройства
        /// </summary>
        public DeviceDocumentsViewModel DeviceDocumentsViewModel { get; set; }

        #endregion

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

        #endregion

        #region Commands

        /// <summary>
        /// Загружает события данного устройства
        /// </summary>
        public ICommand LoadEventsCommand { get; set; }

        #endregion

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider) : base(device, exchangeProvider)
        {
            EventsStartDateTime = DateTime.Now.AddDays(-1).Date;
            EventsEndDateTime = DateTime.Now.Date;

            LoadEventsCommand = new AsyncCommand(LoadEvents);

            CurrentDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.CurrentData).ToList(), exchangeProvider);
            ServiceDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Service).ToList(), exchangeProvider);
            SpecificDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Specific).ToList(), exchangeProvider);
            DeviceDocumentsViewModel = new DeviceDocumentsViewModel(Device.DataServer.DsGuid, DeviceGuid, exchangeProvider);
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

        #endregion

        #endregion
    }
}
