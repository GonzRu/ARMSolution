using System;

namespace CoreLib.Models.Configuration
{
    public class TagString : Tag
    {
        #region Public Properties

        /// <summary>
        /// Значение по-умолчанию
        /// </summary>
        public override object DefaultValueAsObject
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public override string TagValueAsString
        {
            get { return TagValueAsObject as String; }
        }

        #endregion

        #region Overrided

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public override void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            if (!(newTagValueAsObject is String))
                return;

            base.SetTagValue(newTagValueAsObject, newTagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public override string GetTypeAsString()
        {
            return "String";
        }

        #endregion
    }
}
