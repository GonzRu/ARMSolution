
namespace CoreLib.Models.Common
{
    public class TrendSettings
    {
        /// <summary>
        /// Включена ли запись тренда
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Интервал записи значений. 0 - запись по факту изменения
        /// </summary>
        public uint Sample { get; set; }

        /// <summary>
        /// Относительная погрешность изменения.
        /// Допустимый диапозон значений (0,1]
        /// </summary>
        public float? RelativeError { get; set; }

        /// <summary>
        /// Абсолютная погрешность изменения.
        /// </summary>
        public float? AbsoluteError { get; set; }

        /// <summary>
        /// Максимальное число значений, которое будет кешироваться до записи в БД
        /// </summary>
        public uint MaxCacheValuesCount { get; set; }

        /// <summary>
        /// Максимальное число минут для хранения закешированных данных
        /// </summary>
        public uint MaxCacheMinutes { get; set; }
    }
}
