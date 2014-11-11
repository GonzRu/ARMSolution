using System;

namespace CoreLib.Models.Common
{
    /// <summary>
    /// Описывает архивный набор уставок
    /// </summary>
    public class SettingsSet
    {
        /// <summary>
        /// Идентификатор набора уставок
        /// </summary>
        public Int32 SettingsSetId { get; set; }

        /// <summary>
        /// Комментарий к набору уставок
        /// </summary>
        public String SettingsSetComment { get; set; }

        /// <summary>
        /// Дата записи набора уставок
        /// </summary>
        public DateTime SettingsSetDateTime { get; set; }
    }
}
