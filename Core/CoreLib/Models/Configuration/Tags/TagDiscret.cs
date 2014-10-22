using System;

namespace CoreLib.Models.Configuration
{
    public class TagDiscret : Tag
    {
        #region Public Properties

        /// <summary>
        /// Инвертировать ли значение со стороны АРМа
        /// </summary>
        public bool IsInverse { get; set; }

        /// <summary>
        /// Значение по-умолчанию
        /// </summary>
        public override object DefaultValueAsObject
        {
            get { return false; }
        }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public override string TagValueAsString
        {
            get { return TagValueAsObject.ToString(); }
        }

        #endregion

        #region Constructor

        public TagDiscret()
        {
            IsInverse = false;
        }

        #endregion

        #region Overrided

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public override void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            if (!(newTagValueAsObject is Boolean))
                return;

            if (IsInverse)
                newTagValueAsObject = !((bool)newTagValueAsObject);

            base.SetTagValue(newTagValueAsObject, newTagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public override string GetTypeAsString()
        {
            return "Discret";
        }

        #endregion
    }
}
