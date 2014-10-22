using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsCompressors
{
    public static class DateTimeCompressor
    {
        public static byte[] Compress(List<DateTime> times)
        {
            var result = new List<byte>();
            var startTime = times[0];

            result.AddRange(BitConverter.GetBytes(startTime.Ticks));

            times.RemoveAt(0);
            foreach (var time in times)
            {
                result.AddRange(BitConverter.GetBytes((int)(time - startTime).TotalMilliseconds));
            }

            return result.ToArray();
        }

        public static List<DateTime> UnCompress(byte[] compressedTimes)
        {
            var result = new List<DateTime>();

            using (var memoryStream = new MemoryStream(compressedTimes))
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
    }
}
