using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Collections.Generic;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    internal sealed class DeviceDataViewModel : ViewModelBase
    {
        #region Public properties

        /// <summary>
        /// Список групп. На данный момент не предполагает динамического измененияs
        /// </summary>
        public List<GroupViewModel> Groups { get; set; } 

        #endregion

        #region Private fields

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructors

        public DeviceDataViewModel(List<GroupViewModel> groups, IExchangeProvider exchangeProvider)
        {
            Groups = groups;
            _exchangeProvider = exchangeProvider;
        }

        #endregion
    }
}
