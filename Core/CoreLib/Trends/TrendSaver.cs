using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CoreLib.Trends.TrendsManager;

namespace CoreLib.Trends
{
    public class TrendSaver
    {
        #region Public properties

        public Tag Tag { get; private set; }

        /// <summary>
        /// Дискретизация в мс. 0 - запись по факту обновления
        /// </summary>
        public uint Sample { get; set; }

        /// <summary>
        /// Относительная граница изменения значения в %. 
        /// </summary>
        public float RelativeValueError { get; set; }

        /// <summary>
        /// Абсолютная граница изменения значения
        /// </summary>
        public float AbsoluteValueError { get; set; }

        /// <summary>
        /// Верхняя граница, при пересечении которой значение будет занесено в тренд. Не для режима "По факту обновления"
        /// </summary>
        public float? UpAlarmBound { get; set; }

        /// <summary>
        /// Нижняя граница, при пересечении которой значение будет занесено в тренд. Не для режима "По факту обновления"
        /// </summary>
        public float? BottomAlarmBound { get; set; }

        #endregion

        #region Private fields

        /// <summary>
        /// Словарь значений
        /// </summary>
        private List<Tuple<DateTime, object>> _trend = new List<Tuple<DateTime, object>>();

        /// <summary>
        /// Управление записью и чтением трендов
        /// </summary>
        private ITrendManager _trendManager = new TrendManager();

        private object _lockObject = new object();

        /// <summary>
        /// Таймер для периодического добавления в тренд значений
        /// </summary>
        Timer periodicGetValuesTimer = new Timer();

        /// <summary>
        /// Таймер для периодической отправки значений на сохранение
        /// </summary>
        Timer periodicSaveTrendTimer = new Timer();

        #endregion

        #region Constructor

        /// <summary>
        /// Запускает запись значений по факту изменений
        /// </summary>
        /// <param name="tag">Тег - источник значений для тренда</param>
        /// <param name="relativeValueError">Граница изменений, ниже которой изменение не считается изменением (в %)</param>
        public TrendSaver(Tag tag, float relativeValueError = 0)
        {
            Tag = tag;
            RelativeValueError = relativeValueError;

            periodicSaveTrendTimer.Interval = 60000;
            periodicSaveTrendTimer.Elapsed += PeriodicSaveTrendTimerOnElapsed;
            periodicSaveTrendTimer.Start();

            Tag.TagValueChanged += TagValueChanged;
        }

        /// <summary>
        /// Запускает запись значений в тренд с некоторой периодичностью
        /// </summary>
        /// <param name="tag">Тег - источник значений для тренда</param>
        /// <param name="sample">Дискретизация записи значений</param>
        /// <param name="relativeValueError">Граница изменений, ниже которой изменение не считается изменением (в %)</param>
        /// <param name="upAlarmBound">Верхняя граница, при пересечении значением которой будет принудительно добавлено в тренд</param>
        /// <param name="bottomAlarmBound">Нижняя граница, при пересечении значением которой будет принудительно добавлено в тренд</param>
        public TrendSaver(Tag tag, uint sample, float relativeValueError = 0, float? upAlarmBound = null, float? bottomAlarmBound = null)
        {
            if (sample == 0)
                throw new ArgumentException("Для режима ");

            periodicSaveTrendTimer.Interval = 60000;
            periodicSaveTrendTimer.Elapsed += PeriodicSaveTrendTimerOnElapsed;
            periodicSaveTrendTimer.Start();

            periodicGetValuesTimer.Interval = sample;
            periodicGetValuesTimer.Elapsed += PeriodicGetValuesTimerOnElapsed;
            periodicGetValuesTimer.Start();

            Tag = tag;
            Sample = sample;
        }

        private void PeriodicSaveTrendTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (_lockObject)
            {
                if (_trend.Count > 0)
                {
                    _trendManager.SaveTrendToArchiv(Tag.Device.DeviceGuid, Tag.TagGuid, _trend);
                    _trend = new List<Tuple<DateTime, object>>();
                }
            }
        }

        #endregion

        #region Private metods

        /// <summary>
        /// Добавляет значение в тренд
        /// </summary>
        private void AddTagValueToTrend(object tagValueAsObject, DateTime tagValueChangeTime, TagValueQuality tagValueQuality)
        {
            lock (_lockObject)
            {
                //if (RelativeValueError != 0)
                //    if (Math.Abs((float) _trend.Last().Value - (float) tagValueAsObject) > RelativeValueError)
                //        return;
                if (tagValueQuality == TagValueQuality.vqGood)
                {
                    _trend.Add(new Tuple<DateTime, object>(tagValueChangeTime, tagValueAsObject));
                }
            }
            
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Добавляет значение тега 
        /// </summary>
        private void TagValueChanged(object tagValueAsObject, string tagValueAsString, TagValueQuality tagValueQuality, DateTime tagValueChangeTime)
        {
            AddTagValueToTrend((float)tagValueAsObject, tagValueChangeTime, tagValueQuality);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PeriodicGetValuesTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            AddTagValueToTrend((float)Tag.TagValueAsObject, DateTime.Now, Tag.TagValueQuality);
        }

        #endregion
    }
}
