using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    class BaseCompressor
    {
        #region Constructors

        #endregion

        #region Public metods

        /// <summary>
        /// Сжимает последовательность чисел
        /// </summary>
        public byte[] Compress(List<Tuple<DateTime, object>> originalTrend)
        {
            return ZipByteArray(PackTrendToByteArray(originalTrend));
        }

        /// <summary>
        /// Восстанавливает исходный сигнал
        /// </summary>
        public List<Tuple<DateTime, object>> Decompress(byte[] compressedTrend)
        {
            return UnPackTrendFromByteArray(UnZipByteArray(compressedTrend));
        }

        #endregion

        #region Protected metods

        /// <summary>
        /// Переводит сигнал в массив байтов
        /// </summary>
        protected virtual byte[] PackTrendToByteArray(List<Tuple<DateTime, object>> trend)
        {
            var outstream = new MemoryStream();

            using (var stream = new BinaryWriter(outstream))
            {
                stream.Write(trend.Count);

                foreach (var s in trend.ConvertAll(input => (float)input.Item2))
                {
                    stream.Write(s);
                }

                stream.Write(CompressDateTimeList(trend.ConvertAll(input => input.Item1)));
            }

            return outstream.ToArray();
        }

        /// <summary>
        /// Переводит сигнал из массива байтов
        /// </summary>
        protected virtual List<Tuple<DateTime, object>> UnPackTrendFromByteArray(byte[] data)
        {
            var result = new List<Tuple<DateTime, object>>();

            using (var memoryStream = new MemoryStream(data))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    int count = binaryReader.ReadInt32() - 1;
                    var firstValue = binaryReader.ReadSingle();

                    var values = new List<float>();
                    while (count-- != 0)
                    {
                        values.Add(binaryReader.ReadSingle());
                    }

                    var startTime = new DateTime(binaryReader.ReadInt64());
                    values.ForEach(f => result.Add(new Tuple<DateTime, object>(startTime.AddMilliseconds(binaryReader.ReadInt32()), f)));
                    result.Insert(0, new Tuple<DateTime, object>(startTime, firstValue));
                }
            }

            return result;
        }

        /// <summary>
        /// Сжимает массив данных с помощью zip-архивации
        /// </summary>
        protected virtual byte[] ZipByteArray(byte[] data)
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
        protected virtual byte[] UnZipByteArray(byte[] data)
        {
            var inStream = new MemoryStream(data);
            var outStream = new MemoryStream();

            using (var gZipStream = new GZipStream(inStream, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(outStream);
            }

            return outStream.ToArray();
        }

        protected virtual byte[] CompressDateTimeList(List<DateTime> dateTimes)
        {
            var result = new List<byte>();
            var startTime = dateTimes[0];

            result.AddRange(BitConverter.GetBytes(startTime.Ticks));

            dateTimes.RemoveAt(0);
            foreach (var time in dateTimes)
            {
                result.AddRange(BitConverter.GetBytes((int)(time - startTime).TotalMilliseconds));
            }

            return result.ToArray();
        }

        protected virtual List<DateTime> DecompressDateTimeList(byte[] compressedDateTimes)
        {
            var result = new List<DateTime>();

            using (var memoryStream = new MemoryStream(compressedDateTimes))
            {
                using (var binaryStream = new BinaryReader(memoryStream))
                {
                    var startTime = new DateTime(binaryStream.ReadInt64());
                    result.Add(startTime);

                    while (binaryStream.PeekChar() != -1)
                    {
                        result.Add((startTime.AddMilliseconds(binaryStream.ReadInt32())));
                    }
                }
            }

            return result;
        }

        #endregion

        #region Private metods

        #endregion

    }
}
