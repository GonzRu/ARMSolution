using System;

namespace CoreLib.Models.Common.Reports
{
    public class EventsReportSettings : BaseReportSettings
    {
        public ushort DsGuid { get; set; }

        public uint DeviceGuid { get; set; }

        /// <summary>
        /// Начало отсчета для событий
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Конец отсчета для событий
        /// </summary>
        public DateTime EndDateTime { get; set; }
    }
}
