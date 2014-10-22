using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public class DWT2D : BaseDWT
    {
        #region Constructors

        public DWT2D(double error)
            : base(new List<float> { 1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2) }, error)
        {
        }

        #endregion
    }
}
