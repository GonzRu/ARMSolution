using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Models.Configuration;
using CoreLib.Trends.TrendsCompressors;

namespace CoreLib.Trends.TrendsManager
{
    public class TrendManager : ITrendManager
    {
        #region CONSTS

        private const int VALUE_COUNT_TO_SAVE = 2048;

        #endregion

        #region Private fields

        private Dictionary<UInt32, List<Tuple<DateTime, object>>>  _devicesTrends = new Dictionary<uint, List<Tuple<DateTime, object>>>();

        #endregion

        #region Implement ITrendManager

        /// <summary>
        /// Сохранить
        /// </summary>
        public void SaveTrendToArchiv(UInt32 devGuid, UInt32 tagGuid, List<Tuple<DateTime, object>> trend)
        {
            if (!_devicesTrends.ContainsKey(devGuid))
                _devicesTrends.Add(devGuid, new List<Tuple<DateTime, object>>());

            var deviceTrend = _devicesTrends[devGuid];

            deviceTrend.AddRange(trend);

            if (deviceTrend.Count > VALUE_COUNT_TO_SAVE)
            {
                SaveTrendToDbAsync(devGuid, tagGuid, deviceTrend.GetRange(0, VALUE_COUNT_TO_SAVE));
                deviceTrend.RemoveRange(0, VALUE_COUNT_TO_SAVE);
            }
        }

        /// <summary>
        /// Получить данные из архива
        /// </summary>
        public List<Tuple<DateTime, object>> GetTrendFromArhiv(UInt32 devGuid, UInt32 tagGuid, DateTime startDateTime, DateTime endDateTime)
        {
            var result = ConvertDataToTrend(GetTrendsDataFromDb(devGuid, tagGuid, startDateTime, endDateTime));

            return result.Where(tuple => tuple.Item1 >= startDateTime && tuple.Item1 <= endDateTime).ToList();
        }

        #endregion

        #region Private metods

        #region Save trend metods

        private void SaveTrendToDbAsync(UInt32 devGuid, UInt32 tagGuid, List<Tuple<DateTime, object>> trend)
        {
            Task.Run(() => SaveTrendToDB(devGuid, tagGuid, trend[0].Item1, trend[trend.Count - 1].Item1, CompressTrend(trend)));
        }

        private byte[] CompressTrend(List<Tuple<DateTime, object>> trend)
        {
            var compressor = new BaseCompressor();

            return compressor.Compress(trend);
        }

        private void SaveTrendToDB(UInt32 devGuid, UInt32 tagGuid, DateTime startTime, DateTime endTime, byte[] data)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=ASU-SMIRNOV; Initial Catalog=ptk143; Persist Security Info=True; User ID=asu; Password=12345;");
            sqlConnection.Open();

            SqlCommand putTrendSqlCommand = new SqlCommand("INSERT INTO Trends VALUES (@BlockID, @TagID, @StartTime, @EndTime, @Data)", sqlConnection);
            putTrendSqlCommand.Parameters.AddWithValue("@BlockID", (int)devGuid);
            putTrendSqlCommand.Parameters.AddWithValue("@TagID", (int)tagGuid);
            putTrendSqlCommand.Parameters.AddWithValue("@StartTime", startTime);
            putTrendSqlCommand.Parameters.AddWithValue("@EndTime", endTime);
            putTrendSqlCommand.Parameters.AddWithValue("@Data", data);

            putTrendSqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

        #endregion

        #region Load trend metods

        private List<byte[]> GetTrendsDataFromDb(UInt32 devGuid, UInt32 tagGuid, DateTime startTime, DateTime endTime)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=ASU-SMIRNOV; Initial Catalog=ptk143; Persist Security Info=True; User ID=asu; Password=12345;");
            sqlConnection.Open();

            SqlCommand putTrendSqlCommand = new SqlCommand("SELECT Data FROM Trends  WHERE BlockID = @BlockID AND TagID = @TagID AND ((StartTime > @StartTime AND EndTime < @EndTime) OR (StartTime < @StartTime AND EndTime > @StartTime) OR (StartTime < @EndTime AND EndTime > @EndTime))", sqlConnection);
            putTrendSqlCommand.Parameters.AddWithValue("@BlockID", (int)devGuid);
            putTrendSqlCommand.Parameters.AddWithValue("@TagID", (int)tagGuid);
            putTrendSqlCommand.Parameters.AddWithValue("@StartTime", startTime);
            putTrendSqlCommand.Parameters.AddWithValue("@EndTime", endTime);

            var result = new List<byte[]>();

            using (var sqlReader = putTrendSqlCommand.ExecuteReader())
            {
                if (!sqlReader.HasRows)
                    return result;

                while (sqlReader.Read())
                {
                    result.Add((byte[])sqlReader["Data"]);
                }
            }

            sqlConnection.Close();

            return result;
        }

        private List<Tuple<DateTime, object>>  ConvertDataToTrend(List<byte[]> data)
        {
            var result = new List<Tuple<DateTime, object>>();

            var compressor = new BaseCompressor();
            foreach (var d in data)
            {
                result.AddRange(compressor.Decompress(d));
            }

            return result;
        }

        #endregion

        #endregion
    }
}
