using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    class EventsPageViewModel : ViewModelBase
    {
        #region Private fields

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructors

        public EventsPageViewModel(IExchangeProvider exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;

            LoadEventsAsyncCommands = new AsyncCommand(LoadEvents);
        }

        #endregion

        #region Public properties

        #region Properties

        /// <summary>
        /// Начало выборки событий
        /// </summary>
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                _startDateTime = value;
                NotifyPropertyChanged("StartDateTime");
            }
        }
        private DateTime _startDateTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// Конец выборки событий
        /// </summary>
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set
            {
                _endDateTime = value;
                NotifyPropertyChanged("EndDateTime");
            }
        }
        private DateTime _endDateTime = DateTime.Now;

        /// <summary>
        /// Список событий
        /// </summary>
        public List<EventValue> EventsSource
        {
            get { return _eventsSource; }
            set
            {
                _eventsSource = value;
                NotifyPropertyChanged("EventsSource");
            }
        }
        private List<EventValue> _eventsSource; 

        #endregion

        #region Commands

        /// <summary>
        /// Загрузить события
        /// </summary>
        public ICommand LoadEventsAsyncCommands { get; set; }

        #endregion

        #endregion

        #region Private metods

        #region Implementation commands

        private void LoadEvents()
        {
            var events = _exchangeProvider.GetEvents(StartDateTime, EndDateTime, true, true, true, null);

            EventsSource = events;
        }

        #endregion

        #endregion
    }
}
