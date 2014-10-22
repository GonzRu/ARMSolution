using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public abstract class BaseFloatCompressor
    {
        #region Private and protected fields

        /// <summary>
        /// Число квантирования
        /// </summary>
        private float _error;

        #endregion

        #region Constructors

        protected BaseFloatCompressor(double error)
        {
            _error = (float)error;
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Делает преобразование над сигналом, которое приводит к большому количеству 0
        /// </summary>
        protected abstract List<float> DoSignalTransformation(List<float> originalSignal);

        /// <summary>
        /// Делает обратное преобразование над сигналом
        /// </summary>
        protected abstract List<float> DoInverseSignalTransformation(List<float> transformedSignal);

        #endregion

        #region Public metods

        /// <summary>
        /// Сжимает последовательность чисел
        /// </summary>
        public byte[] Compress(List<float> originalSignal)
        {
            var transformedSignal = DoSignalTransformation(originalSignal);

            if (_error != 0)
            {
                var count = 0;
                for (int i = 0; i < transformedSignal.Count; i++)
                    if (Math.Abs(transformedSignal[i]) < _error)
                    {
                        transformedSignal[i] = 0;
                        count++;
                    }
                Console.WriteLine("Обнулилось " + (count * 100 / (double)transformedSignal.Count).ToString("F2") + "% коэффициентов");
            }

            return ZipByteArray(PackSignalToByteArray(transformedSignal));
        }

        /// <summary>
        /// Восстанавливает исходный сигнал
        /// </summary>
        public List<float> UnCompress(byte[] compressedSignal)
        {
            var transformedSignal = UnPackSignalFromByteArray(UnZipByteArray(compressedSignal));

            return DoInverseSignalTransformation(transformedSignal);
        }

        /// <summary>
        /// Тестирование алгоритма сжатия
        /// </summary>
        public void TestCompress(List<float> originalSignal)
        {
            var compressedData = Compress(originalSignal);
            var restoredSignal = UnCompress(compressedData);

            int N = originalSignal.Count;

            float mes = 0;
            float maxDelta = 0;
            float minDelta = Math.Abs(originalSignal[0] - restoredSignal[0]); ;
            float temp;
            for (int i = 0; i < N; i++)
            {
                temp = Math.Abs(originalSignal[i] - restoredSignal[i]);
                mes += temp;

                if (maxDelta < temp) maxDelta = temp;
                if (minDelta > temp) minDelta = temp;
            }
            mes /= (float)N;
            Console.WriteLine("MES = " + mes.ToString());
            Console.WriteLine("Максимальное отклонение: " + maxDelta.ToString());

            float max = originalSignal.Max();
            Console.WriteLine("PSNR = " + 20 * Math.Log10(max / Math.Sqrt(mes)));

            var originalSize = GetSizeOfPackedSignal(originalSignal);
            var newSize = compressedData.LongLength;
            Console.WriteLine("Коэффициент сжатия: " + ((originalSize - newSize) / (double)originalSize).ToString());
        }

        #endregion

        #region Protected metods

        /// <summary>
        /// Переводит сигнал в массив байтов, используя нотацию (Int32, Single), где
        /// Int32 - число 0, а Single - число, идущее за этими нулями
        /// </summary>
        protected byte[] PackSignalToByteArray(List<float> signal)
        {
            var outstream = new MemoryStream();

            using (var stream = new BinaryWriter(outstream))
            {
                foreach (var s in signal)
                {
                    stream.Write(s);
                }
            }

            return outstream.ToArray();
        }

        /// <summary>
        /// Переводит сигнал из массива байтов в нотации (Int32, Single)
        /// </summary>
        protected List<float> UnPackSignalFromByteArray(byte[] data)
        {
            var signal = new List<float>();

            using (var memoryStream = new MemoryStream(data))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        signal.Add(binaryReader.ReadSingle());
                    }
                }
            }

            return signal;
        }

        /// <summary>
        /// Сжимает массив данных с помощью zip-архивации
        /// </summary>
        protected byte[] ZipByteArray(byte[] data)
        {
            var inStream = new MemoryStream(data);
            var outStream = new MemoryStream();

            using (var gZipStream = new GZipStream(outStream, CompressionMode.Compress))
            {
                inStream.CopyTo(gZipStream);
            }

            return outStream.ToArray();
        }

        /// <summary>
        /// Декодирует массив данных с помощью zip-архивации
        /// </summary>
        protected byte[] UnZipByteArray(byte[] data)
        {
            var inStream = new MemoryStream(data);
            var outStream = new MemoryStream();

            using (var gZipStream = new GZipStream(inStream, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(outStream);
            }

            return outStream.ToArray();
        }

        #endregion

        #region Private metods

        /// <summary>
        /// Переводит сигнал в массив байтов как есть
        /// </summary>
        private byte[] SimplePackSignalToByteArray(List<float> signal)
        {
            var data = new List<byte>();

            signal.ForEach((s) =>
            {
                data.AddRange(BitConverter.GetBytes(s));
            });

            return data.ToArray();
        }

        /// <summary>
        /// Возвращает размер упакованных данных без сжатия с потерями
        /// </summary>
        private long GetSizeOfPackedSignal(List<float> signal)
        {
            return ZipByteArray(SimplePackSignalToByteArray(signal)).LongLength;
        }

        #endregion
    }
}
