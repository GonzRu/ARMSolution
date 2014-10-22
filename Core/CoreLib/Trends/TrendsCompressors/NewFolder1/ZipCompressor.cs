using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    class ZipCompressor : BaseFloatCompressor
    {
        public ZipCompressor() : base(0)
        {
        }

        protected override List<float> DoSignalTransformation(List<float> originalSignal)
        {
            return originalSignal;
        }

        protected override List<float> DoInverseSignalTransformation(List<float> transformedSignal)
        {
            return transformedSignal;
        }
    }
}
