using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    public class DeviceSettingValueViewModel : ViewModelBase
    {
        #region Public properties

        /// <summary>
        /// Имя тега
        /// </summary>
        public string TagName
        {
            get { return _tagViewModel.TagName; }
        }

        /// <summary>
        /// Реальное значение уставок
        /// </summary>
        public object RealSettingsValue
        {
            get { return _tagViewModel.TagValueAsObject; }
        }
        private object _realSettingsValue;

        /// <summary>
        /// Новое значение уставок
        /// </summary>
        public object NewSettingsValue
        {
            get { return _newSettingsValue; }
            set
            {
                _newSettingsValue = value;
                NotifyPropertyChanged("NewSettingsValue");
                IsValueChanged = true;
            }
        }
        private object _newSettingsValue;

        public bool IsValueChanged
        {
            get { return _isValueChanged; }
            set
            {
                _isValueChanged = !_tagViewModel.TagValueAsObject.Equals(NewSettingsValue);
                NotifyPropertyChanged("IsValueChanged");
            }
        }
        private bool _isValueChanged = false;

        #endregion

        #region Private fields

        private TagViewModel _tagViewModel;

        #endregion

        #region Constructors

        public DeviceSettingValueViewModel(TagViewModel tagViewModel)
        {
            _tagViewModel = tagViewModel;
        }

        #endregion

        #region Public metods

        public void SetOnEditSettingsMode()
        {
            NewSettingsValue = _tagViewModel.TagValueAsObject;
        }

        #endregion
    }
}
