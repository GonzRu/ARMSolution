using System.Windows;
using CoreLib.Models.Configuration;
using System;
using System.Windows.Data;

namespace ArmWpfUI.Converters
{
    class DeviceStateToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Tuple<object, TagValueQuality>) || parameter == null)
                return false;

            var deviceState = (int)(float)((Tuple<object, TagValueQuality>) value).Item1;

            return deviceState == (int) parameter ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
