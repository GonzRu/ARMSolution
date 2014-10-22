using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CoreLib.Models.Configuration;
using DsRouterExchangeProviderLib.WcfProvider;

namespace ConfigurationParsersLib
{
    public class WinFormArmConfigurationProvider : IConfigurationProvider
    {
        #region Private fields

        /// <summary>
        /// Путь до папки с конфигурацией
        /// </summary>
        private readonly string PathToConfigurationDirectory;
        private readonly string PathToConfigurationCfgFile;
        private readonly string PathToDevicesDirectory;
        private readonly string PathToPrgDevCfgFile;

        private Configuration _configuration;

        #endregion

        #region Constructor

        public WinFormArmConfigurationProvider(string pathToConfigurationDirectory)
        {
            PathToConfigurationDirectory = pathToConfigurationDirectory;
            PathToDevicesDirectory = Path.Combine(PathToConfigurationDirectory, "Configuration", "0#Dataserver", "Devices");
            PathToPrgDevCfgFile = Path.Combine(PathToConfigurationDirectory, "Configuration", "0#Dataserver", "Sources", "MOA_ECU", "PrgDevCFG.cdp");
            PathToConfigurationCfgFile = Path.Combine(PathToConfigurationDirectory, "Configuration.cfg");

            if (!CheckConfigurationPaths())
                throw new Exception("Неверный формат конфигурации");
        }

        #endregion

        #region IConfigurationProvider

        void IConfigurationProvider.LoadConfiguration()
        {
            LoadConfiguration();

            OpenConnection();
        }

        void IConfigurationProvider.SaveConfiguration()
        {
            if (_configuration == null)
                return;

            SaveConfiguration();
        }

        Configuration IConfigurationProvider.GetConfiguration()
        {
            return _configuration;
        }

        void IConfigurationProvider.BackUp(string backUpName)
        {
            throw new NotImplementedException();
        }

        Dictionary<DateTime, string> IConfigurationProvider.GetBuckUpList()
        {
            throw new NotImplementedException();
        }

        Configuration IConfigurationProvider.RestoreBackUp(string backUpDateTime)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private metods

        /// <summary>
        ///  Проверяет сущестовование нобходимых папок и файлов
        /// </summary>
        /// <returns></returns>
        private bool CheckConfigurationPaths()
        {
            if (!Directory.Exists(PathToConfigurationDirectory))
                return false;

            if (!Directory.Exists(PathToDevicesDirectory))
                return false;

            if (!File.Exists(PathToPrgDevCfgFile))
                return false;

            return true;
        }

        #region Load

        /// <summary>
        /// Загружает конфигурацию
        /// </summary>
        private void LoadConfiguration()
        {
            _configuration = new WinFormArmConfiguration();
            _configuration.DataServers = new Dictionary<ushort, DataServer>();
            _configuration.DataServers.Add(0, new DataServer());

            LoadConfigurationCfgFile();

            LoadDevicesInfo();
        }

        /// <summary>
        /// Загружает Configuration.cfg файл
        /// </summary>
        private void LoadConfigurationCfgFile()
        {
            var configurationcfgXDocument = XDocument.Load(PathToConfigurationCfgFile);
            var objectXElement = configurationcfgXDocument.Element("Project").Element("Configuration").Element("Object");

            _configuration.DataServers[0].ObjectName = objectXElement.Attribute("name").Value;

            var dsAccessInfoXElement = objectXElement.Element("DSAccessInfo").Element("CustomiseDriverInfo");
            _configuration.DsRouterIpAddress = dsAccessInfoXElement.Element("IPAddress").Attribute("value").Value;
            _configuration.DsRouterWcfServicePort = dsAccessInfoXElement.Element("Port").Attribute("value").Value;
        }

        /// <summary>
        /// Загружает информацию об устройствах
        /// </summary>
        private void LoadDevicesInfo()
        {
            var prgDevCfgXDocument = XDocument.Load(PathToPrgDevCfgFile);

            _configuration.DataServers[0].Devices = new Dictionary<uint, Device>();

            var dsSourcesXElements = prgDevCfgXDocument.Element("MTRA").Element("Configuration").Elements("SourceECU");
            foreach (var dsSourceXElement in dsSourcesXElements)
            {
                foreach (var deviceXElement in dsSourceXElement.Element("ECUDevices").Elements("Device"))
                {
                    var device = new WinFormArmConfigurationDevice();
                    device.LoadDeviceInfo(deviceXElement);
                    device.LoadDeviceFile(Path.Combine(PathToDevicesDirectory, String.Format("{0}@{1}.cfg", device.DeviceGuid, device.DeviceTypeName)));
                    device.DataServer = _configuration.DataServers[0];

                    _configuration.DataServers[0].Devices.Add(device.DeviceGuid, device);
                }
            }
        }

        #endregion

        #region Save

        private void SaveConfiguration()
        {
            SaveDevicesInfo();
        }

        private void SaveDevicesInfo()
        {
            var prgDevCfgXDocument = XDocument.Load(PathToPrgDevCfgFile);

            var ds = _configuration.DataServers[0];

            var dsSourcesXElements = prgDevCfgXDocument.Element("MTRA").Element("Configuration").Elements("SourceECU");
            foreach (var dsSourceXElement in dsSourcesXElements)
            {
                foreach (var deviceXElement in dsSourceXElement.Element("ECUDevices").Elements("Device"))
                {
                    var devGuid = UInt32.Parse(deviceXElement.Attribute("objectGUID").Value);
                    var device = ds.Devices[devGuid];

                    deviceXElement.Element("DescDev").Element("DescDev").Value = device.DeviceDescription;

                    (device as WinFormArmConfigurationDevice).Save(Path.Combine(PathToDevicesDirectory, String.Format("{0}@{1}.cfg", device.DeviceGuid, device.DeviceTypeName)));
                }
            }

            prgDevCfgXDocument.Save(PathToPrgDevCfgFile);
        }

        #endregion

        /// <summary>
        /// Открывает соединение с DS
        /// </summary>
        private void OpenConnection()
        {
            _configuration.DsRouterProvider = new DsRouterWcfProvider(_configuration.DsRouterIpAddress, _configuration.DsRouterWcfServicePort);
        }


        #endregion
    }
}
