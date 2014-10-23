using System.Globalization;
using CoreLib.Models.Configuration;
using SilverlightControlsLibrary;
using System;
using System.Windows.Data;
using UICore.Converters;

namespace ArmWpfUI.Converters
{
    [ValueConversion(typeof(Tuple<object, TagValueQuality>), typeof(ASUCommutationDeviceStates))]
    class AnalogTagValueToDeviceStateConverter : ConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = ASUCommutationDeviceStates.UnDefined;

            if (value is Tuple<object, TagValueQuality>)
            {
                var tagValue = value as Tuple<object, TagValueQuality>;

                if (tagValue.Item2 != TagValueQuality.vqGood || tagValue.Item2 != TagValueQuality.vqHandled)
                    switch ((int)(float)tagValue.Item1)
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
    }
}
