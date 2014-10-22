using System;

namespace CoreLib.Models.Common
{
    public class EventValue
    {
        #region Public properties

        /// <summary>
        /// Номер DS, которому принадлежит данное событие
        /// </summary>
        public UInt16 DsGuid { get; set; }

        /// <summary>
        /// Номер устройства, которому принадлежит событие. Для не терминальных событий = -1
        /// </summary>
        public UInt32 DevGuid { get; set; }

        /// <summary>
        /// Идентификатор события
        /// </summary>
        public Int32 EventID { get; set; }

        /// <summary>
        /// Имя источника события
        /// </summary>
        public String EventSourceName { get; set; }

        /// <summary>
        /// Комментарий источника события
        /// </summary>
        public String EventSourceComment { get; set; }

        /// <summary>
        /// Текст события
        /// </summary>
        public String EventText { get; set; }

        /// <summary>
        /// Время события
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// Идентификатор данных, привязанных к событию
        /// </summary>
        public Int32 EventDataID { get; set; }

        /// <summary>
        /// Нужно ли квитирование событие
        /// </summary>
        public Boolean IsNeedReceipt { get; set; }

        /// <summary>
        /// Квитировано ли событие
        /// </summary>
        public Boolean IsReceipted { get; set; }

        /// <summary>
        /// Сообщение квитирования
        /// </summary>
        public String ReceiptMessage { get; set; }

        /// <summary>
        /// Имя пользователя, квитировавшего сообщение
        /// </summary>
        public String ReceiptUser { get; set; }

        /// <summary>
        /// Время квитирования сообщения
        /// </summary>
        public DateTime ReceiptTime { get; set; }

        #endregion
    }
}
