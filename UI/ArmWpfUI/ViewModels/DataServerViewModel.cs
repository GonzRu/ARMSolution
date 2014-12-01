using ArmWpfUI.ViewModels.DeviceViewModels;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Collections.Generic;

namespace ArmWpfUI.ViewModels
{
    internal sealed class DataServerViewModel : UICore.ViewModels.DataServerViewModel
    {
        #region Constructors

        public DataServerViewModel(DataServer dataServer, IExchangeProvider exchangeProvider)
        {
            DataServer = dataServer;

            Devices = new List<UICore.ViewModels.DeviceViewModel>();
            foreach (var device in DataServer.Devices.Values)
                Devices.Add(new DeviceViewModel(device, exchangeProvider));
        }

        #endregion
    }
}
