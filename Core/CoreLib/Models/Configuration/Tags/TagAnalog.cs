using System;

namespace CoreLib.Models.Configuration
{
    public class TagAnalog : Tag
    {
        #region Public Properties

        /// <summary>
        /// Размерность тега
        /// </summary>
        public string Dim { get; set; }

        /// <summary>
        /// Количество знаков после запятой в строковом представлении
        /// </summary>
        public UInt16 Precision { get; set; }

        public Single? MinValue { get; set; }
        public Single? MaxValue { get; set; }

        /// <summary>
        /// Значчение по-умолчанию
        /// </summary>
        public override object DefaultValueAsObject
        {
            get { return (Single)0; }
        }

        /// <summary>
        /// Значения тега в строковом представлении
        /// </summary>
        public override string TagValueAsString
        {
            get { return ((Single) TagValueAsObject).ToString(String.Format("F{0}", Precision)); }
        }

        #endregion

        #region Constructor

        public TagAnalog()
        {
            Dim = String.Empty;
            Precision = 2;
            MinValue = null;
            MaxValue = null;
        }

        #endregion

        #region Public metods

        /// <summary>
        /// Устанавливает значение тега
        /// </summary>
        public override void SetTagValue(object newTagValueAsObject, TagValueQuality newTagValueQuality, DateTime tagValueChangeDateTime)
        {
            if (!(newTagValueAsObject is Single))
                try
                {
                    newTagValueAsObject = Convert.ToSingle(newTagValueAsObject);
                }
                catch (Exception)
                {
                    return;
                }

            base.SetTagValue(newTagValueAsObject, newTagValueQuality, tagValueChangeDateTime);
        }

        /// <summary>
        /// Получить строковое представление типа тега
        /// </summary>
        public override string GetTypeAsString()
        {
            return "Analog";
        }

        #endregion
    }
}
