using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CoreLib.Models.Configuration;
using UICore.Converters;

namespace ArmWpfUI.Converters
{
    [ValueConversion(typeof(Tuple<object, TagValueQuality>), typeof(bool))]
    class AnalogTagValueToIsBannerSetOnBoolean : ConverterBase
    {
        public int BannerId { get; set; }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value is Tuple<object, TagValueQuality>)
            {
                var tagValue = value as Tuple<object, TagValueQuality>;

                var val = (int)(float)tagValue.Item1;
                var quality = tagValue.Item2;

                if (quality != TagValueQuality.vqHandled && quality != TagValueQuality.vqCalculatedHanle &&
                    quality != TagValueQuality.vqGood)
                    return false;

                result = (val & (1 << (BannerId - 1))) > 0;
            }

            return result;
        }
    }
}
