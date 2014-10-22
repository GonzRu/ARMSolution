using System;
using System.Collections.Generic;

namespace CoreLib.Models.Configuration
{
    public class Device
    {
        #region Public-Properties

        /// <summary>
        /// Номер устройства
        /// </summary>
        public UInt32 DeviceGuid { get; set; }

        /// <summary>
        /// Название присоединения устройства
        /// </summary>
        public string DeviceDescription { get; set; }

        /// <summary>
        /// Активировано ли устройство в конфигурации
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Теги устройства
        /// </summary>
        public Dictionary<UInt32, Tag> Tags { get; set; }

        /// <summary>
        /// Список групп устройства
        /// </summary>
        public List<Group> Groups;

        /// <summary>
        /// Тип устройства
        /// </summary>
        public string DeviceTypeName { get; set; }

        /// <summary>
        /// Ссылка на класс DS, которому принадлежит данное устройство
        /// </summary>
        public DataServer DataServer { get; set; }

        #endregion
    }
}
