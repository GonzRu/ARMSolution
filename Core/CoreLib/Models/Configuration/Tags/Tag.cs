using System;
using System.Collections.Generic;

namespace CoreLib.Models.Configuration
{
    public enum TagValueQuality
    {
        vqUndefined = 0,        // Не определено (не производилось ни одного чтения, нет связи)
        vqGood = 1,             // Хорошее качество
        vqArhiv = 2,            // архивная переменная (из БД)
        vqRangeError = 3,       // Выход за пределы диапазона
        vqHandled = 4,          // Ручной ввод данных
        vqUknownTag = 5,           // несуществующий тег (? что значит не существующий тег - м.б. это может исп. в ответах на запросы когда запрашивается тег кот. нет, тогда возвращ его ид и это знач качества)
        vqErrorConverted = 6,   // ошибка преобразования в целевой тип
        vqNonExistDevice = 7,   // несуществующее устройство
        vqTagLengthIs0 = 8,      // длина запрашиваемого тега нулевая
        vqUknownError = 9,       // неизвестная ошибка при попытке получения значения тега
        /*
         * тег неактуален из-за
         * нарушения связи между
         * Dsr и Ds 
         * (это качество формируется на роутере)
         */
        vqDsr2DsBadConnection = 10,
        /*
         * ручной вычисляемый тег -
         * это качество устанавливается 
         * для расчетного тега если все составляющие его теги имеют хорошее качество
         * а хотя бы один (или все) - ручное
         */
        vqCalculatedHanle = 11
    }

    public abstract class Tag
    {
        #region Events

        /// <summary>
        /// Событие изменения значения тега
        /// </summary>
        public Action<object, string, TagValueQuality, DateTime> TagValueChanged = delegate { };

        #endregion

        #region Public-properties

        /// <summary>
        /// Активирован ли тег
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// имя тега
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Идентификатор тега
        /// </summary>
        public UInt32 TagGuid { get; set; }

        /// <summary>
        /// Качество значения тега
        /// </summary>
        public TagValueQuality TagValueQuality { get; set; }

        /// <summary>
        /// Время изменения тега
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Только ли для чтения
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Ссылка на устройство, которому принадлежит данный тег
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Значение тега в виде объекта
        /// </summary>
        public object TagValueAsObject { get; set; }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public abstract string TagValueAsString { get; }

        /// <summary>
        /// Значение по-умолчанию
        /// </summary>
        public abstract object DefaultValueAsObject { get; }

        #endregion

        #region Constructor

        protected Tag()
        {
            SetDefaultValue();
        }

        #endregion

        #region Public metods

        /// <summary>
        /// Устанавливает тегу значение по-умолчанию, качество - в неизвестное.
        /// </summary>
        public void SetDefaultValue()
        {
            SetTagValue(DefaultValueAsObject, TagValueQuality.vqUndefined, DateTime.MinValue);
        }

        /// <summary>
        /// Устанавливает тегу значение по-умолчанию с указанным качеством
        /// </summary>
        public void SetDefaultValue(TagValueQuality tagValueQuality)
        {
            SetTagValue(DefaultValueAsObject, tagValueQuality, DateTime.MinValue);
        }

        /// <summary>
        /// Устанавливает тегу значение по-умолчанию с указанным качеством и временем
        /// </summary>
        public void SetDefaultValue(TagValueQuality tagValueQuality, DateTime tagValueChangeDateTime)
        {
            SetTagValue(DefaultValueAsObject, tagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public virtual void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            TagValueAsObject = newTagValueAsObject;
            TagValueQuality = newTagValueQuality;
            TimeStamp = tagValueChangeDateTime;

            OnTagValueChanged();
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public abstract string GetTypeAsString();

        #endregion

        #region Private-metods

        /// <summary>
        /// Вызывает событие изменения значения тега
        /// </summary>
        private void OnTagValueChanged()
        {
            // Так как событие проиницилизировано пустым делегатом, то мы обезопасили себя от NullReferenceException
            TagValueChanged(TagValueAsObject, TagValueAsString, TagValueQuality, TimeStamp);
        }

        #endregion
    }
}
