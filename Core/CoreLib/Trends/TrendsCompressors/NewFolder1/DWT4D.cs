using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public class DWT4D : BaseDWT
    {
        #region Constructors

        public DWT4D(double error)
            : base(
            new List<float> {
                (float)( (1 + Math.Sqrt(3) ) / ( 4 * Math.Sqrt(2) )),
                (float)( (3 + Math.Sqrt(3) ) / ( 4 * Math.Sqrt(2) )),
                (float)( (3 - Math.Sqrt(3) ) / ( 4 * Math.Sqrt(2) )),
                (float)( (1 - Math.Sqrt(3) ) / ( 4 * Math.Sqrt(2) )),
            }, error)
        {
        }

        #endregion
    }
}
