using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace UICore.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConverterBase : MarkupExtension, IValueConverter
    {
        #region Implementation MarkupExtension

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion

        #region Implementation IValueConverter

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        #endregion

        #region Protected metods

        protected abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        protected virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}