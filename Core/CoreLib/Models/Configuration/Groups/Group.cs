using System.Collections.Generic;
using System.ComponentModel;

namespace CoreLib.Models.Configuration
{
    public enum GroupCategory
    {
        [Description("")]
        None,
        [Description("Текущие")]
        CurrentData,
        [Description("Аварии")]
        Crush,
        [Description("Уставки")]
        Ustavki,
        [Description("Накопительная информация")]
        StorageDevice,
        [Description("Максметр")]
        MaxMeter,
        [Description("Идентификация")]
        Identification,
        [Description("Служебные")]
        Service,
        [Description("Специфические")]
        Specific,
        [Description("Команды")]
        Commands
    }

    public class Group
    {
        #region Public-properties

        /// <summary>
        /// Имя группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Guid группы
        /// </summary>
        public string GroupGuid { get; set; }

        /// <summary>
        /// Список подгрупп
        /// </summary>
        public List<Group> SubGroups { get; set; } 

        /// <summary>
        /// Список тегов группы
        /// </summary>
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Активирована ли группа
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Категория группы
        /// </summary>
        public GroupCategory GroupCategory { get; set; }

        #endregion
    }
}
