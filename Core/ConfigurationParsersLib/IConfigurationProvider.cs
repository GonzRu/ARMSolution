using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Models.Configuration;

namespace ConfigurationParsersLib
{
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Загрузить конфигурацию
        /// </summary>
        void LoadConfiguration();

        /// <summary>
        /// Сохранить конфигурацию
        /// </summary>
        void SaveConfiguration();

        /// <summary>
        /// Получить конфигурацию
        /// </summary>
        Configuration GetConfiguration();

        /// <summary>
        /// Сделать резервную копию
        /// </summary>
        void BackUp(string backUpName);

        /// <summary>
        /// Получить список доступных резервных копий данной конфигурации
        /// </summary>
        Dictionary<DateTime, string> GetBuckUpList();

        /// <summary>
        /// Восстановить резервную копию
        /// </summary>
        Configuration RestoreBackUp(string backUpDateTime);
    }
}
