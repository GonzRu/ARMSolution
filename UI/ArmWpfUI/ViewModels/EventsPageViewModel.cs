using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    class EventsPageViewModel : ViewModelBase
    {
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

        /// <summary>
        /// Флаг, показывающий нужно ли отображать системные сообщения
        /// </summary>
        public bool IsShowSystemEvents
        {
            get { return _isShowSystemEvents; }
            set { _isShowSystemEvents = value; NotifyPropertyChanged("IsShowSystemEvents"); }
        }
        private bool _isShowSystemEvents;

        /// <summary>
        /// Флаг, показывающий нужно ли отображать терминальные сообщения
        /// </summary>
        public bool IsShowTerminalEvents
        {
            get { return _isShowTerminalEvents; }
            set { _isShowTerminalEvents = value; NotifyPropertyChanged("IsShowTerminalEvents"); }
        }
        private bool _isShowTerminalEvents;

        /// <summary>
        /// Флаг, показывающий нужно ли отображать пользовательские сообщения сообщения
        /// </summary>
        public bool IsShowUserEvents
        {
            get { return _isShowUserEvents; }
            set { _isShowUserEvents = value; NotifyPropertyChanged("IsShowUserEvents"); }
        }
        private bool _isShowUserEvents;

        #endregion

        #region Commands

        /// <summary>
        /// Загрузить события
        /// </summary>
        public AsyncCommand LoadEventsAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructors

        public EventsPageViewModel(IExchangeProvider exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;

            LoadEventsAsyncCommand = new AsyncCommand(LoadEvents);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        private void LoadEvents()
        {
            var events = _exchangeProvider.GetEvents(StartDateTime, EndDateTime, IsShowSystemEvents, IsShowTerminalEvents, IsShowUserEvents, null);

            EventsSource = events;
        }

        #endregion

        #endregion
    }
}
