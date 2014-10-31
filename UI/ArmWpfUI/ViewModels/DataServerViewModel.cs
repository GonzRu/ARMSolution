using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Collections.Generic;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    public class DataServerViewModel : UICore.ViewModels.DataServerViewModel
    {
        #region Constructors

        public DataServerViewModel(DataServer dataServer, IExchangeProvider exchangeProvider)
        {
            DataServer = dataServer;

            Devices = new List<BaseDeviceViewModel>();
            foreach (var device in DataServer.Devices.Values)
                Devices.Add(new DeviceViewModel(device, exchangeProvider));
        }

        #endregion
    }
}
