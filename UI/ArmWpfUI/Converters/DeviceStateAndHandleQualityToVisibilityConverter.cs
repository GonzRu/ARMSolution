using CoreLib.Models.Configuration;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using UICore.Converters;

namespace ArmWpfUI.Converters
{
    /// <summary>
    /// Конвертер со сложной логикой.
    /// Если качество Handle, то всегда возвращает Visible
    /// (если InvertHandleQuality = true, то при любом качестве кроме Handle возвращает true)
    /// Затем, если состояние устройства совпадает со значением DeviceState, то возвращает Visible,
    /// иначе Collapsed. Флаг InvertDeviceState инвертирует это поведение
    /// </summary>
    [ValueConversion(typeof(Tuple<object, TagValueQuality>), typeof(Visibility))]
    class DeviceStateAndHandleQualityToVisibilityConverter : ConverterBase
    {
        public int DeviceState { get; set; }

        public bool InvertDeviceState { get; set; }

        public bool InvertHandleQuality { get; set; }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tagValue = value as Tuple<object, TagValueQuality>;

            var deviceState = (int) (float) tagValue.Item1;
            var isHandleQuality = tagValue.Item2 == TagValueQuality.vqHandled ^ InvertHandleQuality;

            if (isHandleQuality)
                return Visibility.Visible;

            return (deviceState == DeviceState) ^ InvertDeviceState ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
