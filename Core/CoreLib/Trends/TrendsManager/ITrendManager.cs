using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Trends.TrendsManager
{
    interface ITrendManager
    {
        /// <summary>
        /// Сохранить
        /// </summary>
        void SaveTrendToArchiv(UInt32 devGuid, UInt32 tagGuid, List<Tuple<DateTime, object>> trend);

        /// <summary>
        /// Получить данные из архива
        /// </summary>
        List<Tuple<DateTime, object>> GetTrendFromArhiv(UInt32 devGuid, UInt32 tagGuid, DateTime startDateTime, DateTime endDateTime);
    }
}
