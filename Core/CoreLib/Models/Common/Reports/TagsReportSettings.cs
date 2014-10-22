using System;
using System.Collections.Generic;

namespace CoreLib.Models.Common.Reports
{
    public class TagsReportSettings : BaseReportSettings
    {
        /// <summary>
        /// Список тегов, по которым необходимо получить отчёт
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Начало отсчета для тегов
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Конец отсчета для тегов
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Частота вывода значений в отчете
        /// </summary>
        public uint Interval { get; set; }
    }
}
