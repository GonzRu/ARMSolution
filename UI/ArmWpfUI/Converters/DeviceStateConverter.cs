using CoreLib.Models.Configuration;
using SilverlightControlsLibrary;
using System;
using System.Windows.Data;

namespace ArmWpfUI.Converters
{
    class DeviceStateConverter : IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            var result = ASUCommutationDeviceStates.UnDefined;

            if (value is Tuple<object, TagValueQuality>)
            {
                var tagValue = value as Tuple<object, TagValueQuality>;

                if (tagValue.Item2 != TagValueQuality.vqGood || tagValue.Item2 != TagValueQuality.vqHandled)
                    switch ((int) (float) tagValue.Item1)
                    {
                        case 1:
                            result = ASUCommutationDeviceStates.On;
                            break;
                        case 2:
                            result = ASUCommutationDeviceStates.Off;
                            break;
                        case 3:
                            result = ASUCommutationDeviceStates.Broken;
                            break;
                    }
            }

            return result;
        }
        public object ConvertBack(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            return new Tuple<object, TagValueQuality>(0, TagValueQuality.vqUndefined);
        }
    }
}
