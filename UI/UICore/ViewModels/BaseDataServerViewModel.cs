using CoreLib.ExchangeProviders;
using System.Collections.Generic;
using System.ComponentModel;
using CoreLib.Models.Configuration;

namespace UICore.ViewModels
{
    public class BaseDataServerViewModel : BaseViewModel
    {
        #region Public properties

        [Category("Общее")]
        [DisplayName("Название проекта")]
        [Description("Название проекта")]
        public string ObjectName
        {
            get { return DataServer.ObjectName; }
            set { DataServer.ObjectName = value; }
        }

        [Category("Общее")]
        [DisplayName("Номер сервера данных")]
        [Description("Номер сервера данных")]
        public ushort DsGuid
        {
            get { return DataServer.DsGuid; }
            set { DataServer.DsGuid = value; }
        }

        /// <summary>
        /// Список устройств
        /// </summary>
        [Browsable(false)]
        public List<BaseDeviceViewModel> Devices { get; set; }

        #endregion

        #region Private fields

        protected DataServer DataServer;

        #endregion

        #region Constructors

        protected BaseDataServerViewModel()
        {
            
        }

        public BaseDataServerViewModel(DataServer dataServer, IExchangeProvider exchangeProvider)
        {
            DataServer = dataServer;

            Devices = new List<BaseDeviceViewModel>();
            foreach (var device in DataServer.Devices.Values)
                Devices.Add(new BaseDeviceViewModel(device, exchangeProvider));
        }

        #endregion
    }
}
