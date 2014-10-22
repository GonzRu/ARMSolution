using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public abstract class BaseDWT : BaseFloatCompressor
    {
        #region Private and protected fields

        protected List<float> CL;
        protected List<float> CH;
        protected List<float> iCL;
        protected List<float> iCH;

        #endregion

        #region Constructors

        public BaseDWT(List<float> CL, double error)
            : base(error)
        {
            this.CL = CL;
            this.CH = GetCH(CL);
        }

        #endregion

        #region Implement BaseCompressor

        /// <summary>
        /// Сжимает последовательность чисел
        /// </summary>
        protected override List<float> DoSignalTransformation(List<float> originalSignal)
        {
            return DWT(originalSignal);
        }

        /// <summary>
        /// Восстанавливает исходный сигнал
        /// </summary>
        protected override List<float> DoInverseSignalTransformation(List<float> transformedSignal)
        {
            iCL = GetiCL(CL, CH);
            iCH = GetiCH(CL, CH);

            return InverseDWT(transformedSignal);
        }

        #endregion

        #region Private metods

        /// <summary>
        /// Сформировать CH коэффициенты
        /// </summary>
        private List<float> GetCH(List<Single> CL)
        {
            var CH = new List<Single>();

            for (int k = 0; k < CL.Count; k++)
                CH.Add((Single)Math.Pow(-1, k) * CL[CL.Count - k - 1]);

            return CH;
        }

        /// <summary>
        /// Сформировать iCL коэффициенты
        /// </summary>
        private List<float> GetiCL(List<float> CL, List<float> CH)
        {
            var iCL = new List<Single>();

            iCL.Add(CL[CL.Count - 2]);
            iCL.Add(CH[CH.Count - 2]);

            for (int i = 2; i < CL.Count; i += 2)
            {
                iCL.Add(CL[i - 2]);
                iCL.Add(CH[i - 2]);
            }

            return iCL;
        }

        /// <summary>
        /// Сформировать iCH коэффициенты
        /// </summary>
        private List<float> GetiCH(List<float> CL, List<float> CH)
        {
            var iCH = new List<Single>();

            iCH.Add(CL[CL.Count - 1]);
            iCH.Add(CH[CH.Count - 1]);

            for (int i = 2; i < CL.Count; i += 2)
            {
                iCH.Add(CL[i - 1]);
                iCH.Add(CH[i - 1]);
            }

            return iCH;
        }

        private List<Single> pconv(List<Single> signal, List<Single> CL, List<Single> CH, int delta = 0)
        {
            List<Single> RetVal = new List<Single>();

            int M = signal.Count;
            for (int j = 0; j < M; j += 2)
            {
                float sL = 0;
                float sH = 0;

                for (int i = 0; i < CL.Count; i++)
                {
                    var index = (i + j - delta) % M;

                    if (index < 0) index = M + index;

                    sL += signal[index] * CL[i];
                    sH += signal[index] * CH[i];
                }

                RetVal.Add(sL);
                RetVal.Add(sH);
            }

            return RetVal;
        }

        /// <summary>
        /// Выполняет ДВП рекурсивно до тех пор пока есть возможность
        /// </summary>
        private List<float> DWT(List<float> signal)
        {
            if (signal.Count == CL.Count / 2)
                return signal;

            var transformedSignal = pconv(signal, CL, CH);

            var secondPart = transformedSignal.Where((s, i) => i % 2 == 0).ToList<float>();
            var firstPart = transformedSignal.Where((s, i) => i % 2 != 0).ToList<float>();

            firstPart.AddRange(DWT(secondPart));

            return firstPart;
        }

        /// <summary>
        /// Выполняет обратное ДВП рекурсивно до тех пор пока есть возможность
        /// </summary>
        private List<float> InverseDWT(List<float> signal)
        {
            if (signal.Count == CL.Count / 2)
                return signal;

            var restoredSignal = signal.GetRange(0, signal.Count / 2);
            restoredSignal.AddRange(InverseDWT(signal.GetRange(signal.Count / 2, signal.Count / 2)));


            var result = new float[restoredSignal.Count];
            for (int i = 0; i < result.Length; i++)
                if (i % 2 == 0)
                    result[i] = restoredSignal[(result.Length + i) / 2];
                else
                    result[i] = restoredSignal[i / 2];

            return pconv(result.ToList<float>(), iCL, iCH, iCL.Count - 2);
        }

        #endregion
    }
}
