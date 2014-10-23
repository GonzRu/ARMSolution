using CoreLib.Models.Configuration;
using System;
using System.Globalization;
using System.Windows.Data;
using UICore.Converters;

namespace ARMConfigurator.Converters
{
    [ValueConversion(typeof(GroupCategory), typeof(string))]
    public class GroupCategoryToStringConverter : ConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
    }
}
