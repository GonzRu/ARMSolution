using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UICore.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : ConverterBase
    {
        public bool Invert { get; set; }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (value is bool && (bool)value);
            return (flag ^ Invert) ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is Visibility && (Visibility)value == Visibility.Visible;
            return result ^ Invert;
        }
    }
}