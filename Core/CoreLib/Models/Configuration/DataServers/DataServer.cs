using System;
using System.Collections.Generic;

namespace CoreLib.Models.Configuration
{
    public class DataServer
    {
        #region Public-properties

        /// <summary>
        /// Название проекта, который обслуживает данный DS
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Идентификатор DS
        /// </summary>
        public UInt16 DsGuid { get; set; }

        /// <summary>
        /// Список устройств DS
        /// </summary>
        public Dictionary<UInt32, Device> Devices { get; set; }

        //// <summary>
        //// Состояние связи с DS
        //// </summary>
        //public DsConnectionState ConnectionState { get; set; }

        #endregion
    }
}
