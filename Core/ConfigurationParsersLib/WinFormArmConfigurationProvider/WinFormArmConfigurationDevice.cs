using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CoreLib.Models.Configuration;

namespace ConfigurationParsersLib
{
    class WinFormArmConfigurationDevice : Device
    {
        #region Public metods

        public void LoadDeviceInfo(XElement deviceXElement)
        {
            Enable = Boolean.Parse(deviceXElement.Attribute("enable").Value);
            DeviceGuid = UInt32.Parse(deviceXElement.Attribute("objectGUID").Value);
            DeviceDescription = deviceXElement.Element("DescDev").Element("DescDev").Value;
            DeviceTypeName = deviceXElement.Attribute("TypeName").Value;
        }

        public void LoadDeviceFile(string pathToDeviceCfgFile)
        {
            var deviceXDocument = XDocument.Load(pathToDeviceCfgFile);
            Tags = ParseTags(deviceXDocument.Element("Device").Element("Tags"));

            ParseGroups(deviceXDocument.Element("Device").Element("Groups"));
        }

        public void Save(string pathToDeviceCfgFile)
        {
            var deviceXDocument = XDocument.Load(pathToDeviceCfgFile);

            var groupsXElement = deviceXDocument.Element("Device").Element("Groups");
            SaveGroups(groupsXElement);

            deviceXDocument.Save(pathToDeviceCfgFile);
        }

        #endregion

        #region Private metods

        private Dictionary<uint, Tag> ParseTags(XElement tagsXElement)
        {
            var result = new Dictionary<uint, Tag>();

            foreach (var tagXElement in tagsXElement.Elements("Tag"))
            {
                var tag = ParseTag(tagXElement);

                if (tag != null)
                    result.Add(tag.TagGuid, tag);
            }

            return result;
        }

        private Tag ParseTag(XElement tagXElement)
        {
            if (tagXElement.Attribute("TagEnable") != null)
            if (tagXElement.Attribute("TagEnable").Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                return null;

            var configurationLevelDescribeXElement = tagXElement.Element("Configurator_level_Describe");
            var tagType = configurationLevelDescribeXElement.Element("Type").Value;

            Tag tag = null;
            switch (tagType.ToLower())
            {
                case "analog":
                    tag = new TagAnalog();

                    var minValue = configurationLevelDescribeXElement.Element("MinValue").Value;
                    float tmpMinValue;
                    if (!String.IsNullOrWhiteSpace(minValue))
                        if (float.TryParse(minValue, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpMinValue))
                            (tag as TagAnalog).MinValue = tmpMinValue;

                    var maxValue = configurationLevelDescribeXElement.Element("MaxValue").Value;
                    float tmpMaxValue;
                    if (!String.IsNullOrWhiteSpace(maxValue))
                        if (float.TryParse(maxValue, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpMaxValue))
                            (tag as TagAnalog).MaxValue = tmpMaxValue;

                    break;
                case "discret":
                    tag = new TagDiscret();
                    break;
                case "datetime":
                    tag = new TagDateTime();
                    break;
                case "enum":
                    tag = new TagEnum();

                    var cbItemListXElement = configurationLevelDescribeXElement.Element("CBItemsList");
                    foreach (var cbItemXElement in cbItemListXElement.Elements("CBItem"))
                    {
                        var value = UInt16.Parse(cbItemXElement.Attribute("intvalue").Value);
                        var stringValue = cbItemXElement.Value;

                        (tag as TagEnum).EnumsStringList.Add(value, stringValue);
                    }

                    break;
                case "string":
                    tag = new TagString();
                    break;
                default:
                    Console.WriteLine("WinFormArmConfigurationDevice:ParseTags() : неизвестный тип тега - " + tagType);
                    return null;
            }

            tag.TagGuid = UInt32.Parse(tagXElement.Attribute("TagGUID").Value);
            tag.TagName = configurationLevelDescribeXElement.Element("TagName").Value;
            tag.ReadOnly = configurationLevelDescribeXElement.Element("AccessToValue").Value.Equals("r", StringComparison.OrdinalIgnoreCase);
            tag.Device = this;

            return tag;
        }

        private void ParseGroups(XElement groupsXelement)
        {
            Groups = new List<Group>();

            foreach (var groupXElement in groupsXelement.Elements("Group"))
                Groups.Add(CreateGroup(groupXElement));
        }

        private Group CreateGroup(XElement groupXElement)
        {
            var group = new Group();
            group.SubGroups = new List<Group>();
            group.Tags = new List<Tag>();

            group.Enable = bool.Parse(groupXElement.Attribute("enable").Value);
            group.GroupGuid = groupXElement.Attribute("GroupGUID").Value;
            group.GroupName = groupXElement.Attribute("Name").Value;
            group.GroupCategory = GetGroupCategory(groupXElement.Attribute("category"));

            foreach (var subGroupXElement in groupXElement.Elements("Group"))
                group.SubGroups.Add(CreateGroup(subGroupXElement));

            if (groupXElement.Element("Tags") != null)
                foreach (var tagXElement in groupXElement.Element("Tags").Elements("TagGuid"))
                {
                    var tag = CreateTag(tagXElement);

                    if (tag != null)
                        group.Tags.Add(tag);
                }

            return group;
        }

        private Tag CreateTag(XElement tagXElement)
        {
            var tagGuid = UInt32.Parse(tagXElement.Attribute("value").Value);
            Tag tag;
            if (!Tags.TryGetValue(tagGuid, out tag))
                return null;

            tag.Enable = bool.Parse(tagXElement.Attribute("enable").Value);

            var guiVariableDescribeXElement = tagXElement.Element("gui_variables_describe");
            if (guiVariableDescribeXElement != null)
            {
                tag.TagName = guiVariableDescribeXElement.Element("var_title").Value;

                if (tag is TagAnalog)
                {
                    (tag as TagAnalog).Dim = guiVariableDescribeXElement.Element("UOM").Value;
                }
            }

            // Количество знаков после запятой для аналогового тега
            if (tag is TagAnalog)
            {
                var hmiFormatDescribeXElement = tagXElement.Element("HMI_Format_describe");
                if (hmiFormatDescribeXElement != null)
                {
                    (tag as TagAnalog).Precision = UInt16.Parse(hmiFormatDescribeXElement.Element("HMIPosPoint").Value);
                }
            }

            return tag;
        }

        private GroupCategory GetGroupCategory(XAttribute xAttribute)
        {
            if (xAttribute == null)
                return GroupCategory.None;

            switch (xAttribute.Value)
            {
                case "Crush":
                    return GroupCategory.Crush;
                case "MaxMeter":
                    return GroupCategory.MaxMeter;
                case "StorageDevice":
                    return GroupCategory.StorageDevice;
                case "Ustavki":
                    return GroupCategory.Ustavki;
                default:
                    Console.WriteLine("WinFormArmConfigurationDevice:GetGroupCategory : неизвестный тип группы. DevGuid = " + DeviceGuid + ". Category = " + xAttribute.Value);
                    return GroupCategory.None;
            }
        }

        private void SaveGroups(XElement groupsXElement)
        {
            var groupEnumenator = Groups.GetEnumerator();
            foreach (var groupXElement in groupsXElement.Elements("Group"))
            {
                groupEnumenator.MoveNext();
                SaveGroup(groupXElement, groupEnumenator.Current);
            }
        }

        private void SaveGroup(XElement groupXElement, Group group)
        {
            groupXElement.Attribute("Name").Value = group.GroupName;

            SaveGroupCategory(groupXElement, group);

            var groupEnumenator = group.SubGroups.GetEnumerator();
            foreach (var subGroupXElement in groupXElement.Elements("Group"))
            {
                groupEnumenator.MoveNext();
                SaveGroup(subGroupXElement, groupEnumenator.Current);                
            }

            if (groupXElement.Element("Tags") != null)
            foreach (var tagXElement in groupXElement.Element("Tags").Elements("TagGuid"))
                SaveTag(tagXElement);
        }

        private void SaveTag(XElement tagXElement)
        {
            var tagGuid = uint.Parse(tagXElement.Attribute("value").Value);
            var tag = Tags[tagGuid];

            var guiVariablesDescriptionXElement = tagXElement.Element("gui_variables_describe");
            if (guiVariablesDescriptionXElement == null)
            {
                guiVariablesDescriptionXElement = 
                    new XElement(
                        "gui_variables_describe",
                        new XElement("var_title"),
                        new XElement("UOM")
                        );

                tagXElement.Add(guiVariablesDescriptionXElement);
            }

            guiVariablesDescriptionXElement.Element("var_title").Value = tag.TagName;
            if (tag is TagAnalog)
                guiVariablesDescriptionXElement.Element("UOM").Value = (tag as TagAnalog).Dim;
        }

        private void SaveGroupCategory(XElement groupXElement, Group group)
        {
            var groupCategoryXAttribute = groupXElement.Attribute("category");
            if (group.GroupCategory == GroupCategory.None)
            {
                if (groupCategoryXAttribute != null)
                    groupCategoryXAttribute.Remove();
                return;
            }

            if (groupCategoryXAttribute == null)
            {
                groupCategoryXAttribute = new XAttribute("category", GroupCategoryToString(group.GroupCategory));
                groupXElement.Add(groupCategoryXAttribute);
            }

            groupCategoryXAttribute.Value = GroupCategoryToString(group.GroupCategory);
        }

        private string GroupCategoryToString(GroupCategory groupCategory)
        {
            switch (groupCategory)
            {
                case GroupCategory.Crush:
                    return "Crush";
                case GroupCategory.MaxMeter:
                    return "MaxMeter";
                case GroupCategory.StorageDevice:
                    return "StorageDevice";
                case GroupCategory.Ustavki:
                    return "Ustavki";
            }

            return String.Empty;
        }

        #endregion
    }
}
