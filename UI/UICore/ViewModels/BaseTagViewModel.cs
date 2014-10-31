using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System;
using System.ComponentModel;

namespace UICore.ViewModels
{
    public class BaseTagViewModel : ViewModelBase
    {
        #region Public properties

        /// <summary>
        /// Имя тега
        /// </summary>
        [Category("Общее")]
        [DisplayName("Название тега")]
        [Description("Отображаемое название тега")]
        public string TagName
        {
            get { return Tag.TagName; }
            set
            {
                Tag.TagName = value;
                NotifyPropertyChanged("TagName");
            }
        }

        /// <summary>
        /// Активирован ли тег
        /// </summary>
        [Category("Общее")]
        [DisplayName("Enable")]
        [Description("Показывать ли тег в группе")]
        public bool Enable
        {
            get { return Tag.Enable; }
            set
            {
                Tag.Enable = value;
                NotifyPropertyChanged("Enable");
            }
        }

        /// <summary>
        /// Идентификатор тега
        /// </summary>
        [Category("Общее")]
        [DisplayName("GuID тега")]
        [Description("Уникальный идентификатор тега в устройстве")]
        public UInt32 TagGuid
        {
            get { return Tag.TagGuid; }
        }

        /// <summary>
        /// Значение тега в виде объекта
        /// </summary>
        [Category("Значение")]
        [DisplayName("Raw-значение")]
        [Description("Исходное значение тега")]
        public virtual object TagValueAsObject
        {
            get { return Tag.TagValueAsObject; }
        }

        [Category("Значение")]
        [DisplayName("String-значение")]
        [Description("Строковое представление значения тега")]
        public string TagValueAsString
        {
            get { return Tag.TagValueAsString; }
        }

        /// <summary>
        /// Качество значения тега
        /// </summary>
        [Category("Значение")]
        [DisplayName("Качество")]
        [Description("Качество значения тега")]
        public TagValueQuality TagValueQuality
        {
            get { return Tag.TagValueQuality; }
        }

        /// <summary>
        /// Время изменения тега
        /// </summary>
        [Category("Значение")]
        [DisplayName("Время изменения")]
        [Description("Время последнего изменения значения тега")]
        public DateTime TimeStamp
        {
            get { return Tag.TimeStamp; }
        }

        /// <summary>
        /// Только ли для чтения
        /// </summary>
        [Category("Общее")]
        [DisplayName("Только для чтения")]
        [Description("Сигнал только для чтения (актуально только для уставок)")]
        public bool ReadOnly
        {
            get { return Tag.ReadOnly; }
        }

        [Browsable(false)]
        public UInt32 DeviceGuid { get { return Tag.Device.DeviceGuid; } }

        [Browsable(false)]
        public UInt16 DsGuid { get { return Tag.Device.DataServer.DsGuid; } }

        [Browsable(false)]
        public Tuple<object, TagValueQuality> TagValue
        {
            get { return new Tuple<object, TagValueQuality>(TagValueAsObject, TagValueQuality); }
        }

        #endregion

        #region Private fields

        protected readonly Tag Tag;

        protected readonly IExchangeProvider ExchangeProvider;

        #endregion

        #region Constructor

        public BaseTagViewModel(Tag tag, IExchangeProvider exchangeProvider)
        {
            Tag = tag;
            ExchangeProvider = exchangeProvider;

            Tag.TagValueChanged += (o, s, arg3, arg4) =>
            {
                NotifyPropertyChanged("TagValueAsString");
                NotifyPropertyChanged("TagValueAsObject");
                NotifyPropertyChanged("TagValueQuality");
                NotifyPropertyChanged("TimeStamp");
                NotifyPropertyChanged("TagValue");
            };
        }

        #endregion
    }
}
