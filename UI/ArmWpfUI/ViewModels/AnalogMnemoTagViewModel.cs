using CoreLib.Models.Configuration;
using System;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    class AnalogMnemoTagViewModel : ViewModelBase
    {
        private Tag _tag;

        public AnalogMnemoTagViewModel(Tag tag)
        {
            _tag = tag;
            _tag.TagValueChanged +=
                (o, s, arg3, arg4) => TagValue = ConvertToEnum(o, arg3);

        }

        private object _tagValue = SilverlightControlsLibrary.ASUCommutationDeviceStates.UnDefined;
        public object TagValue
        {
            get { return _tagValue; }
            set
            {
                _tagValue = value;
                NotifyPropertyChanged("TagValue");
            }
        }

        private SilverlightControlsLibrary.ASUCommutationDeviceStates ConvertToEnum(object value, TagValueQuality valueQuality)
        {
            if (value != null && (valueQuality == TagValueQuality.vqGood || valueQuality == TagValueQuality.vqHandled))
            {
                if ((Single)value == 1)
                    return SilverlightControlsLibrary.ASUCommutationDeviceStates.On;
                if ((Single)value == 0)
                    return SilverlightControlsLibrary.ASUCommutationDeviceStates.Off;
                if ((Single)value == 3)
                    return SilverlightControlsLibrary.ASUCommutationDeviceStates.Broken;
            }

            return SilverlightControlsLibrary.ASUCommutationDeviceStates.UnDefined;
        }
    }
}
