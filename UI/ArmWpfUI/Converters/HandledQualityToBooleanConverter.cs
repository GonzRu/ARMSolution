using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CoreLib.Models.Configuration;

namespace ArmWpfUI.Converters
{
    class HandledQualityToBooleanConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Tuple<object, TagValueQuality>))
                return false;

            var tagValueQuality = (value as Tuple<object, TagValueQuality>).Item2;

            bool invert = false;
            if (parameter is bool)
                if ((bool) parameter)
                    invert = true;

            return tagValueQuality == TagValueQuality.vqHandled || tagValueQuality == TagValueQuality.vqCalculatedHanle ? !invert : invert;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
