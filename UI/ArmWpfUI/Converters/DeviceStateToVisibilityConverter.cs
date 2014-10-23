using System.Globalization;
using System.Windows;
using CoreLib.Models.Configuration;
using System;
using System.Windows.Data;
using UICore.Converters;

namespace ArmWpfUI.Converters
{
    [ValueConversion(typeof(Tuple<object, TagValueQuality>), typeof(Visibility))]
    class DeviceStateToVisibilityConverter : ConverterBase
    {
        public int DeviceState { get; set; }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deviceState = (int) (float) ((value as Tuple<object, TagValueQuality>).Item1);

            return deviceState == DeviceState ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
