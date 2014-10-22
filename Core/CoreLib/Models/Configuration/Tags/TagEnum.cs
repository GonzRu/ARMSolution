using System;
using System.Collections.Generic;

namespace CoreLib.Models.Configuration
{
    public class TagEnum : Tag
    {
        #region Public Properties

        /// <summary>
        /// Список текстовых представлений значений тега, соответствующих числовому
        /// </summary>
        public SortedList<UInt16, string> EnumsStringList { get; set; }

        /// <summary>
        /// Значение по-умолчанию
        /// </summary>
        public override object DefaultValueAsObject
        {
            get { return (UInt16)0; }
        }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public override string TagValueAsString
        {
            get
            {
                return EnumsStringList.ContainsKey((UInt16) TagValueAsObject)
                    ? EnumsStringList[(UInt16) TagValueAsObject]
                    : String.Empty;
            }
        }

        #endregion

        #region Constructor

        public TagEnum()
        {
            EnumsStringList = new SortedList<ushort, string>();
        }

        #endregion

        #region Public metods

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public override void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            if (!(newTagValueAsObject is Single))
                return;

            base.SetTagValue(Convert.ToUInt16(newTagValueAsObject), newTagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public override string GetTypeAsString()
        {
            return "Enum";
        }

        #endregion
    }
}
