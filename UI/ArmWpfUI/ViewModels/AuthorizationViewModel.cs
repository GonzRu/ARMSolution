using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.ExchangeProviders;

namespace ArmWpfUI.ViewModels
{
    internal sealed class AuthorizationViewModel
    {
        #region Private fields

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructor

        public AuthorizationViewModel(IExchangeProvider exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;
        }

        #endregion
    }
}
