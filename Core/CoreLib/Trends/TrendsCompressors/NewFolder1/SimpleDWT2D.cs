using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public class SimpleDWT2D : BaseFloatCompressor
    {
        #region Constructors

        public SimpleDWT2D(double error)
            : base(error)
        {
        }

        #endregion

        #region Implement BaseCompressor

        /// <summary>
        /// Сжимает последовательность чисел
        /// </summary>
        protected override List<float> DoSignalTransformation(List<float> originalSignal)
        {
            return DWTD2(originalSignal);
        }

        /// <summary>
        /// Восстанавливает исходный сигнал
        /// </summary>
        protected override List<float> DoInverseSignalTransformation(List<float> transformedSignal)
        {
            return InverseDWT2(transformedSignal);
        }

        #endregion

        #region Private metods

        private List<Single> DWTD2(List<Single> SourceList)
        {
            if (SourceList.Count == 1)
                return SourceList;

            List<Single> RetVal = new List<Single>();
            List<Single> TmpArr = new List<Single>();

            for (int j = 0; j < SourceList.Count; j += 2)
            {
                RetVal.Add((SourceList[j] - SourceList[j + 1]) / (Single)2.0);
                TmpArr.Add((SourceList[j] + SourceList[j + 1]) / (Single)2.0);
            }

            RetVal.AddRange(DWTD2(TmpArr));

            return RetVal;
        }

        private List<Single> InverseDWT2(List<Single> SourceList)
        {
            if (SourceList.Count == 1)
                return SourceList;

            List<Single> RetVal = new List<Single>();
            List<Single> TmpPart = new List<Single>();

            for (int i = SourceList.Count / 2; i < SourceList.Count; i++)
                TmpPart.Add(SourceList[i]);

            List<Single> SecondPart = InverseDWT2(TmpPart);

            for (int i = 0; i < SourceList.Count / 2; i++)
            {
                RetVal.Add(SecondPart[i] + SourceList[i]);
                RetVal.Add(SecondPart[i] - SourceList[i]);
            }

            return RetVal;
        }

        #endregion
    }
}
