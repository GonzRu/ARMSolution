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

        /// <summary>
        /// События устройства
        /// </summary>
        public DeviceEventsViewModel DeviceEventsViewModel { get; set; }

        #endregion

        #endregion

        #region Commands



        #endregion

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider) : base(device, exchangeProvider)
        {

            CurrentDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.CurrentData).ToList(), exchangeProvider);
            ServiceDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Service).ToList(), exchangeProvider);
            SpecificDataViewModel = new DeviceDataViewModel(Groups.Where(model => model.GroupCategory == GroupCategory.Specific).ToList(), exchangeProvider);
            DeviceDocumentsViewModel = new DeviceDocumentsViewModel(Device.DataServer.DsGuid, DeviceGuid, exchangeProvider);
            DeviceEventsViewModel = new DeviceEventsViewModel(Device, exchangeProvider);
        }

        #endregion

        #region Private metods

        #region Implementation commands



        #endregion

        #endregion
    }
}
