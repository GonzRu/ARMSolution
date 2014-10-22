using System;

namespace CoreLib.Models.Common.Reports
{
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Int32 UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Комментарий к пользователю
        /// </summary>
        public string UserComment { get; set; }

        /// <summary>
        /// Группа, в которой состоит пользователь
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Дата изменения пользователя
        /// </summary>
        public DateTime EditDateTime { get; set; }
    }
}
