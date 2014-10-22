
using System.ComponentModel;

namespace CoreLib.Models.Common.Reports
{
    public enum ReportExtension
    {
        [Description("Формат Excel (xls)")]
        xls,
        [Description("Формат Excel (xlsx)")]
        xlsx,
        [Description("Формат Word (doc)")]
        doc,
        [Description("Формат Pdf")]
        pdf
    }

    public class BaseReportSettings
    {
        /// <summary>
        /// Формат отчета
        /// </summary>
        public ReportExtension ReportExtension { get; set; }

        /// <summary>
        /// Имя шаблона отчета
        /// </summary>
        public string ReportTamplateName { get; set; }
    }
}
