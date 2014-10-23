using System.Windows;
using System.Windows.Data;
using CoreLib.Models.Configuration;
using System;
using System.Globalization;
using UICore.Converters;

namespace ArmWpfUI.Converters
{
    [ValueConversion(typeof(Tuple<object, TagValueQuality>), typeof(bool))]
    class HandledQualityToBooleanConverter : ConverterBase
    {
        public bool Invert { get; set; }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tagValueQuality = (value as Tuple<object, TagValueQuality>).Item2;

            return (tagValueQuality == TagValueQuality.vqHandled || tagValueQuality == TagValueQuality.vqCalculatedHanle) ^ Invert;
        }
    }
}
