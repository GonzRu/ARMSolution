using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    internal sealed class DeviceEventsViewModel : ViewModelBase
    {
        #region Public Properties

        #region Properties

        public Device Device { get; protected set; }

        /// <summary>
        /// Список событий данного устройства
        /// </summary>
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

        #region Commands

        /// <summary>
        /// Загружает события данного устройства
        /// </summary>
        public AsyncCommand LoadEventsAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructors

        public DeviceEventsViewModel(Device device, IExchangeProvider exchangeProvider)
        {
            Device = device;
            _exchangeProvider = exchangeProvider;

            EventsStartDateTime = DateTime.Now.AddDays(-1).Date;
            EventsEndDateTime = DateTime.Now.Date;

            LoadEventsAsyncCommand = new AsyncCommand(LoadEvents);

        }

        #endregion

        #region Private metods

        #region Implementation commands

        /// <summary>
        /// Загружает события, соответствующие данному устройству
        /// </summary>
        private void LoadEvents()
        {
            Events = _exchangeProvider.GetEvents(EventsStartDateTime, EventsEndDateTime, false, false, true,
                new List<Tuple<ushort, uint>> { new Tuple<ushort, uint>(Device.DataServer.DsGuid, Device.DeviceGuid) });
        }

        #endregion

        #endregion
    }
}
