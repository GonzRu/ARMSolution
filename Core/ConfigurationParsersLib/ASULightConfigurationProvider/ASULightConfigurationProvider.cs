﻿using CoreLib.Models.Configuration;
using DsRouterExchangeProviderLib.WcfProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurationParsersLib
{
    public class ASULightConfigurationProvider : IConfigurationProvider
    {
        #region CONSTS

        private string DEFAULT_DSROUTER_SERVICE_PORT = "3332";

        #endregion

        #region Private fields

        /// <summary>
        /// Путь до папки с конфигурацией
        /// </summary>
        private readonly string PathToConfigurationDirectory;
        private readonly string PathToDSConfig;
        private readonly string PathMainMnemo;

        private Configuration _configuration;

        /// <summary>
        /// Загружать в конфигурацию отключенные устройства, группы и теги
        /// </summary>
        private bool _loadDisabledElements;

        /// <summary>
        /// Нужно для присваивания тегам ссылки на устройство
        /// </summary>
        private Device _currentParsingDevice;

        /// <summary>
        /// Нужно для присваивания устройствам ссылки на DS
        /// </summary>
        private DataServer _currentDataServer;

        #endregion

        #region Constructor

        public ASULightConfigurationProvider(string pathToConfigurationDirectory, bool loadDisabledElements = false)
        {
            PathToConfigurationDirectory = pathToConfigurationDirectory;
            PathToDSConfig = Path.Combine(PathToConfigurationDirectory, "DSConfig.cfg");
            PathMainMnemo = Path.Combine(PathToConfigurationDirectory, "Mnemo", "Mnemo.xml");

            _loadDisabledElements = loadDisabledElements;

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

            if (!File.Exists(PathToDSConfig))
                return false;

            if (!File.Exists(PathMainMnemo))
                return false;

            return true;
        }

        /// <summary>
        /// Открывает соединение с DS
        /// </summary>
        private void OpenConnection()
        {
            _configuration.DsRouterProvider = new DsRouterWcfProvider(_configuration.DsRouterIpAddress, _configuration.DsRouterWcfServicePort);
        }

        #region Load

        private void LoadConfiguration()
        {
            _configuration = new Configuration();
            _configuration.MainMnemoPath = PathMainMnemo;
            _configuration.DataServers = new Dictionary<ushort, DataServer>();

            ParseDSConfig();
        }

        private void ParseDSConfig()
        {
            var dsConfigXDocument = XDocument.Load(PathToDSConfig);

            ParseProjectInfo(dsConfigXDocument.Element("MTRA").Element("ProjectInfo"));

            ParseConfiguration(dsConfigXDocument.Element("MTRA").Element("Configuration"));
        }

        /// <summary>
        /// Загружает информацию из секции ProjectInfo
        /// </summary>
        private void ParseProjectInfo(XElement projectInfoXElement)
        {
            _configuration.ProjectName = projectInfoXElement.Element("NamePTK").Value;

            var dsRouterServiceUrlStr = projectInfoXElement.Element("DSRouterServiceAddress").Value;
            var dsRouterServiceUrl = new Uri(dsRouterServiceUrlStr);

            _configuration.DsRouterIpAddress = dsRouterServiceUrl.Host;
            _configuration.DsRouterWcfServicePort = DEFAULT_DSROUTER_SERVICE_PORT;
        }

        /// <summary>
        /// Загружает информацию из секции Configuration
        /// </summary>
        private void ParseConfiguration(XElement configurationXElement)
        {
            foreach (var objectXElement in configurationXElement.Elements("Object"))
            {
                var ds = ParseDataServer(objectXElement);
                if (ds != null)
                    _configuration.DataServers.Add(ds.DsGuid, ds);
            }
        }

        /// <summary>
        /// Загружает DataServer из секции Object
        /// </summary>
        private DataServer ParseDataServer(XElement objectXElement)
        {
            var ds = new DataServer();
            ds.ObjectName = objectXElement.Attribute("name").Value;
            ds.DsGuid = ushort.Parse(objectXElement.Attribute("UniDS_GUID").Value);            
            ds.Devices = new Dictionary<uint, Device>();

            _currentDataServer = ds;

            foreach (var deviceXElement in objectXElement.Elements("Device"))
            {
                var device = ParseDevice(deviceXElement);
                if (device != null)
                    ds.Devices.Add(device.DeviceGuid, device);
            }

            return ds;
        }

        /// <summary>
        /// Загружает Device из секции Device
        /// </summary>
        private Device ParseDevice(XElement deviceXElement)
        {
            if (!_loadDisabledElements)
                if (deviceXElement.Attribute("enable").Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    return null;

            var device = new Device();
            device.Tags = new Dictionary<uint, Tag>();
            device.Groups = new List<Group>();
            device.DataServer = _currentDataServer;
            _currentParsingDevice = device;

            device.DeviceGuid = UInt16.Parse(deviceXElement.Attribute("objectGUID").Value);

            var deviceDescriptionXElement = deviceXElement.Element("DescriptInfo");
            device.DeviceTypeName = deviceDescriptionXElement.Element("DeviceType").Value;
            device.DeviceDescription = deviceDescriptionXElement.Element("DeviceAttachmentDescribe").Value;

            foreach (var groupXElement in deviceXElement.Element("Groups").Elements("Group"))
            {
                var group = ParseGroup(groupXElement, device);
                if (group != null)
                    device.Groups.Add(group);
            }

            return device;
        }

        /// <summary>
        /// Загружает Group из секции Group
        /// </summary>
        private Group ParseGroup(XElement groupXElement, Device device)
        {
            if (!_loadDisabledElements)
                if (groupXElement.Attribute("enbl").Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    return null;

            var group = new Group();
            group.Enable = bool.Parse(groupXElement.Attribute("enbl").Value);
            group.GroupName = groupXElement.Attribute("nm").Value;
            group.GroupGuid = groupXElement.Attribute("GroupGUID").Value;
            group.GroupCategory = GetGroupCategory(groupXElement.Attribute("category"));

            group.SubGroups = new List<Group>();
            group.Tags = new List<Tag>();

            // Загрузка подгрупп
            foreach (var subGroupXElement in groupXElement.Elements("Group"))
            {
                var subGroup = ParseGroup(subGroupXElement, device);
                if (subGroup != null)
                    group.SubGroups.Add(subGroup);
            }

            // Загрузка тегов
            if (groupXElement.Element("Tags") != null)
            foreach (var tagXElement in groupXElement.Element("Tags").Elements("Tag"))
            {
                var tag = ParseTag(tagXElement);
                if (tag != null)
                {
                    device.Tags.Add(tag.TagGuid, tag);
                    group.Tags.Add(tag);
                }             
            }

            return group;
        }

        private Tag ParseTag(XElement tagXElement)
        {
            if (!_loadDisabledElements)
                if (tagXElement.Attribute("enbl").Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    return null;

            Tag tag = null;
            var tagType = tagXElement.Attribute("tp").Value;
            switch (tagType)
            {
                case "Analog":
                    #region Init TagAnalog's properties

                    tag = new TagAnalog();
                    (tag as TagAnalog).Dim = tagXElement.Attribute("uom").Value;

                    // Min value
                    var minValue = tagXElement.Attribute("min").Value;
                    float tmpMinValue;
                    if (!String.IsNullOrWhiteSpace(minValue))
                        if (float.TryParse(minValue, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpMinValue))
                            (tag as TagAnalog).MinValue = tmpMinValue;

                    // Max Value
                    var maxValue = tagXElement.Attribute("max").Value;
                    float tmpMaxValue;
                    if (!String.IsNullOrWhiteSpace(maxValue))
                        if (float.TryParse(maxValue, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpMaxValue))
                            (tag as TagAnalog).MaxValue = tmpMaxValue;

                    #endregion
                    break;
                case "Discret":
                    #region Init TagDiscret's properties

                    tag = new TagDiscret();

                    #endregion
                    break;
                case "Enum":
                    #region Init TagEnum's properties

                    tag = new TagEnum();
                    var tagEnum = tag as TagEnum;

                    if (tagXElement.Element("CBItemsList") != null)
                    foreach (var cbItem in tagXElement.Element("CBItemsList").Elements("CBItem"))
                        tagEnum.EnumsStringList.Add(ushort.Parse(cbItem.Attribute("intvalue").Value), cbItem.Value);

                    #warning не загружена информация о min max value.
                    // Хотя по сути для enum бесполезная информация, так как можно ограничить по доступным текстовым представлениям

                    #endregion
                    break;
                case "DateTime":
                    #region Init TagDateTime's properties

                    tag = new TagDateTime();

                    #endregion
                    break;
                case "String":
                    #region Init TagString's properties

                    tag = new TagString();

                    #endregion
                    break;
                case "":
#warning не обрабатываются команды
                    return null;
                default:
                    break;
            }

            // Общая часть
            tag.Enable = bool.Parse(tagXElement.Attribute("enbl").Value);
            tag.ReadOnly = tagXElement.Attribute("acc").Value.Equals("r", StringComparison.OrdinalIgnoreCase);
            tag.TagGuid = uint.Parse(tagXElement.Attribute("tg").Value);
            tag.TagName = tagXElement.Attribute("nm").Value;
            tag.Device = _currentParsingDevice;

            return tag;
        }

        private GroupCategory GetGroupCategory(XAttribute categoryXAttribute)
        {
            if (categoryXAttribute == null)
                return GroupCategory.None;

            switch (categoryXAttribute.Value)
            {
                case "1":
                    return GroupCategory.Identification;
                case "2":
                    return GroupCategory.CurrentData;
                case "3":
                    return GroupCategory.Crush;
                case "4":
                    return GroupCategory.Ustavki;
                case "5":
                    return GroupCategory.Service;
                case "6":
                    return GroupCategory.Specific;
                case "7":
                    return GroupCategory.Commands;
                default:
                    throw new ArgumentException("ASULightConfigurationProvider:GetGroupCategory() : неизвестная категория группы - " + categoryXAttribute.Value);
            }
        }

        #endregion

        #region Save

        private void SaveConfiguration()
        {
            var dsConfigXDocument = XDocument.Load(PathToDSConfig);

            SaveProjectInfo(dsConfigXDocument.Element("MTRA").Element("ProjectInfo"));

            SaveProjectConfiguration(dsConfigXDocument.Element("MTRA").Element("Configuration"));

            dsConfigXDocument.Save(PathToDSConfig);
        }

        private void SaveProjectInfo(XElement projectInfoXElement)
        {
            projectInfoXElement.Element("NamePTK").Value = _configuration.ProjectName;
        }

        private void SaveProjectConfiguration(XElement configurationXElement)
        {
            foreach (var objectXElement in configurationXElement.Elements("Object"))
            {
                var dsGuid = UInt16.Parse(objectXElement.Attribute("UniDS_GUID").Value);
                var ds = _configuration.DataServers[dsGuid];

                objectXElement.Attribute("name").Value = ds.ObjectName;

                foreach (var deviceXElement in objectXElement.Elements("Device"))
                {
                    var devGuid = uint.Parse(deviceXElement.Attribute("objectGUID").Value);
                    var device = ds.Devices[devGuid];

                    SaveDevice(deviceXElement, device);
                }
            }
        }

        private void SaveDevice(XElement deviceXElement, Device device)
        {
            deviceXElement.Element("DescriptInfo").Element("DeviceAttachmentDescribe").Value = device.DeviceDescription;

            SaveGroups(deviceXElement.Element("Groups").Elements("Group"), device.Groups);
        }

        private void SaveGroups(IEnumerable<XElement> groupsXElements, List<Group> groups)
        {
            if (groupsXElements == null || !groupsXElements.Any())
                return;

            foreach (var groupXElement in groupsXElements)
            {
                var groupId = groupXElement.Attribute("GroupGUID").Value;
                var group = (from gr in groups where gr.GroupGuid.Equals(groupId) select gr).FirstOrDefault();

                SaveGroup(groupXElement, group);
            }
        }

        private void SaveGroup(XElement groupXElement, Group group)
        {
            if (groupXElement.Attribute("category") != null && groupXElement.Attribute("category").Value.Equals("7"))
                return;

            groupXElement.Attribute("enbl").Value = group.Enable.ToString(CultureInfo.InvariantCulture);
            groupXElement.Attribute("nm").Value = group.GroupName;

            SaveGroups(groupXElement.Elements("Group"), group.SubGroups);

            var tagsXElement = groupXElement.Element("Tags");
            if (tagsXElement != null)
                foreach (var tagXElement in tagsXElement.Elements("Tag"))
                {
                    var tagGuid = uint.Parse(tagXElement.Attribute("tg").Value);
                    var tag = group.Tags.FirstOrDefault(tg => tg.TagGuid == tagGuid);

                    SaveTag(tagXElement, tag);
                }
        }

        private void SaveTag(XElement tagXElement, Tag tag)
        {
            // Общая часть
            tagXElement.Attribute("enbl").Value = tag.Enable.ToString(CultureInfo.InvariantCulture);
            tagXElement.Attribute("acc").Value = tag.ReadOnly.ToString(CultureInfo.InvariantCulture);
            tagXElement.Attribute("tg").Value = tag.TagGuid.ToString(CultureInfo.InvariantCulture);
            tagXElement.Attribute("nm").Value = tag.TagName;

            var tagType = tagXElement.Attribute("tp").Value;
            switch (tagType)
            {
                case "Analog":
                    #region Init TagAnalog's properties

                    var analogTag = (tag as TagAnalog);

                    tagXElement.Attribute("uom").Value = analogTag.Dim;

                    // Min value
                    tagXElement.Attribute("min").Value = analogTag.MinValue.HasValue
                        ? analogTag.MinValue.Value.ToString(CultureInfo.InvariantCulture)
                        : String.Empty;

                    // Max Value
                    tagXElement.Attribute("max").Value = analogTag.MinValue.HasValue
                        ? analogTag.MinValue.Value.ToString(CultureInfo.InvariantCulture)
                        : String.Empty;

                    #endregion
                    break;
                case "Discret":
                    #region Init TagDiscret's properties
                    #endregion
                    break;
                case "Enum":
                    #region Init TagEnum's properties
                    #endregion
                    break;
                case "DateTime":
                    #region Init TagDateTime's properties
                    #endregion
                    break;
                case "String":
                    #region Init TagString's properties
                    #endregion
                    break;
                default:
                    break;
            }
        }

        string GroupCategoryToString(GroupCategory groupCategory)
        {
            switch (groupCategory)
            {
                case GroupCategory.Identification:
                    return "1";
                case GroupCategory.CurrentData:
                    return "2";
                case GroupCategory.Crush:
                    return "3";
                case GroupCategory.Ustavki:
                    return "4";
                case GroupCategory.Service:
                    return "5";
                case GroupCategory.Specific:
                    return "6";
                case GroupCategory.Commands:
                    return "7";
                case GroupCategory.None:
                default:
                    return String.Empty;
            }
        }

        #endregion

        #endregion
    }
}
