using System;

namespace CoreLib.Models.Common.Reports
{
    public class UserGroup
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public Int32 GroupId { get; set; }

        /// <summary>
        /// Имя группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Комментарий группы
        /// </summary>
        public string GroupComment { get; set; }

        /// <summary>
        /// Права группы
        /// </summary>
        public string GroupRight { get; set; }

        /// <summary>
        /// Дата создания группы
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Дата последнего изменения группы
        /// </summary>
        public DateTime EditDateTime { get; set; }
    }
}
