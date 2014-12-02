using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Linq;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    internal sealed class DeviceViewModel : UICore.ViewModels.DeviceViewModel
    {
        #region Public properties

        #region Properties

        #region Подразделы устройства

        /// <summary>
        /// Текущие данные
        /// </summary>
        public DeviceDataViewModel CurrentDataViewModel {
            get
            {
                if (_currentDataViewModel == null)
                    _currentDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.CurrentData).ToList(), ExchangeProvider);
                return _currentDataViewModel;
            }
        }
        private DeviceDataViewModel _currentDataViewModel;

        /// <summary>
        /// Уставки
        /// </summary>
        public DeviceSettingsViewModel DeviceSettingsViewModel
        {
            get
            {
                if (_deviceSettingsViewModel == null)
                    _deviceSettingsViewModel = new DeviceSettingsViewModel(Device, Groups.Where(model => model.GroupCategory == GroupCategory.Ustavki).ToList(), ExchangeProvider);
                return _deviceSettingsViewModel;
            }
        }
        private DeviceSettingsViewModel _deviceSettingsViewModel;

        /// <summary>
        /// Служебные данные
        /// </summary>
        public DeviceDataViewModel ServiceDataViewModel
        {
            get
            {
                if (_serviceDataViewModel == null)
                    _serviceDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Service).ToList(), ExchangeProvider);
                return _serviceDataViewModel;
            }
        }
        private DeviceDataViewModel _serviceDataViewModel;

        /// <summary>
        /// Специфические данные
        /// </summary>
        public DeviceDataViewModel SpecificDataViewModel
        {
            get
            {
                if (_specificDataViewModel == null)
                    _specificDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Specific).ToList(), ExchangeProvider);
                return _specificDataViewModel;
            }
        }
        private DeviceDataViewModel _specificDataViewModel;

        /// <summary>
        /// Документы устройства
        /// </summary>
        public DeviceDocumentsViewModel DeviceDocumentsViewModel
        {
            get
            {
                if (_deviceDocumentsViewModel == null)
                    _deviceDocumentsViewModel = new DeviceDocumentsViewModel(Device.DataServer.DsGuid, DeviceGuid, ExchangeProvider);
                return _deviceDocumentsViewModel;
            }
        }
        private DeviceDocumentsViewModel _deviceDocumentsViewModel;

        /// <summary>
        /// События устройства
        /// </summary>
        public DeviceEventsViewModel DeviceEventsViewModel
        {
            get
            {
                if (_deviceEventsViewModel == null)
                    _deviceEventsViewModel = new DeviceEventsViewModel(Device, ExchangeProvider);
                return _deviceEventsViewModel;
            }
        }
        private DeviceEventsViewModel _deviceEventsViewModel;

        #endregion

        #endregion

        #region Commands



        #endregion

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider) : base(device, exchangeProvider)
        {
        }

        #endregion

        #region Private metods

        #region Implementation commands



        #endregion

        #endregion
    }
}
