using System;
using System.Windows;
using System.Windows.Data;
using CoreLib.Models.Configuration;

namespace ARMConfigurator.Converters
{
    //[ValueConversion(typeof(GroupCategory), typeof(string))]
    public class GroupCategoryToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var groupCategory = (GroupCategory)value;

            switch (groupCategory)
            {
                case GroupCategory.None:
                    return String.Empty;
                case GroupCategory.Crush:
                    return "Авария";
                case GroupCategory.MaxMeter:
                    return "Максметер";
                case GroupCategory.StorageDevice:
                    return "Накопительная информация";
                case GroupCategory.Ustavki:
                    return "Уставки";
            }

            return String.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
