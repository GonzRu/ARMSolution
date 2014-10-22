using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UICore.ViewModels
{
    public class BaseTagDiscretViewModel : BaseTagViewModel
    {
        #region Public properties

        /// <summary>
        /// Инвертируется ли тег на стороне DS
        /// </summary>
        private bool? _isInverse = null;
        [Category("Специфическое")]
        [DisplayName("Инвертирован")]
        [Description("Инвертируется ли тег на стороне сервера данных")]
        public bool? IsInverse
        {
            get
            {
                if (_isInverse == null)
                    IsTagInverse();

                return _isInverse;
            }
            set
            {
                InverseTag();
            }
        }

        #endregion

        #region Constructor

        public BaseTagDiscretViewModel(TagDiscret tag, IExchangeProvider exchangeProvider)
            : base(tag, exchangeProvider)
        {
        }

        #endregion

        #region Private metods

        private async void IsTagInverse()
        {
            _isInverse = await Task<bool?>.Run(() => ExchangeProvider.IsInverseTagDiscret(DsGuid, DeviceGuid, TagGuid));

            if (_isInverse != null)
                NotifyPropertyChanged("IsInverse");
        }

        private async void InverseTag()
        {
            if (_isInverse != null)
                await Task.Run(() => ExchangeProvider.InverseTagDiscret(DsGuid, DeviceGuid, TagGuid, !(bool)_isInverse));

            IsTagInverse();
        }

        #endregion
    }
}
