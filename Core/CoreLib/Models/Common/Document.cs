using System;

namespace CoreLib.Models.Common
{
    public class Document
    {
        /// <summary>
        /// Идентификатор документа в системе
        /// </summary>
        public int DocumentId { get; set; }

        /// <summary>
        /// Дата добавления документа
        /// </summary>
        public DateTime DocumentAddDate { get; set; }

        /// <summary>
        /// Имя пользователя, добавившего документ
        /// </summary>
        public string DocumentUserName { get; set; }

        /// <summary>
        /// Имя документа
        /// </summary>
        public string DocumentFileName { get; set; }

        /// <summary>
        /// Комментарий к документу
        /// </summary>
        public string DocumentComment { get; set; }
    }
}
