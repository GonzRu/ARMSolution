using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationParsersLib
{
    public static class ConfigurationProvidersFactory
    {
        public static IConfigurationProvider CreateConfigurationProvider(string pathToProjectDirectory, bool loadDisabledElements = false)
        {
            if (!Directory.Exists(pathToProjectDirectory))
                throw new ArgumentException("По указанному пути не найдена папка.");

            var dsConfigPath = Path.Combine(pathToProjectDirectory, "DSConfig.cfg");
            if (File.Exists(dsConfigPath))
                return new ASULightConfigurationProvider(pathToProjectDirectory, loadDisabledElements);

            return new WinFormArmConfigurationProvider(pathToProjectDirectory);
        }
    }
}
