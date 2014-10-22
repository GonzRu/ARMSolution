using System;

namespace CoreLib.Models.Configuration
{
    public class TagDateTime : Tag
    {
        #region Public Properties

        /// <summary>
        /// Значение по-умолчанию
        /// </summary>
        public override object DefaultValueAsObject
        {
            get { return DateTime.MinValue; }
        }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public override string TagValueAsString
        {
            get { return ((DateTime)TagValueAsObject).ToString("u"); }
        }

        #endregion

        #region Public-metods

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public override void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            if (!(newTagValueAsObject is DateTime))
                return;

            base.SetTagValue(newTagValueAsObject, newTagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public override string GetTypeAsString()
        {
            return "DateTime";
        }

        #endregion
    }
}
