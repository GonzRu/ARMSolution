using System;
using System.Collections.Generic;
using System.Linq;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;

namespace CoreLib.Models.Configuration
{
    public class Configuration
    {
        #region Events

        /// <summary>
        /// Событие изменения связи с роутером
        /// </summary>
        public Action<bool> DsRouterConnectionStateChanged;

        #endregion

        #region Public-properties

        /// <summary>
        /// Провайдер связи с роутером
        /// </summary>
        private IExchangeProvider _dsRouterProvider;
        public IExchangeProvider DsRouterProvider
        {
            get { return _dsRouterProvider; }
            set
            {
                if (_dsRouterProvider != null)
                {
                    _dsRouterProvider.ConnectionStateChanged -= DsRouterProviderConnectionStateChangedHandler;
                    //_dsRouterProvider.ConnectionStateChanged -= DsRouterConnectionStateChanged;

                    _dsRouterProvider.TagsValuesUpdated -= DsRouterProviderTagsValuesUpdatedHandler;
                }

                _dsRouterProvider = value;
                IsConnectionStateOpened = _dsRouterProvider.IsConnectionStateOpened;
                _dsRouterProvider.ConnectionStateChanged += DsRouterProviderConnectionStateChangedHandler;
                //_dsRouterProvider.ConnectionStateChanged += DsRouterConnectionStateChanged;

                _dsRouterProvider.TagsValuesUpdated += DsRouterProviderTagsValuesUpdatedHandler;
            }
        }

        /// <summary>
        /// Название ПТК в документации
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Состояние связи с роутером
        /// </summary>
        public bool IsConnectionStateOpened { get; set; }

        /// <summary>
        /// Список DS
        /// </summary>
        public Dictionary<UInt16, DataServer> DataServers { get; set; }

        /// <summary>
        /// IP адрес роутера
        /// </summary>
        public string DsRouterIpAddress { get; set; }

        /// <summary>
        /// Порт подключения к WCF-сервису роутера
        /// </summary>
        public string DsRouterWcfServicePort { get; set; }

        /// <summary>
        /// Путь до главной мнемохсемы
        /// </summary>
        public string MainMnemoPath { get; set; }

        #endregion

        #region Constructor

        public Configuration()
        {
        }

        #endregion

        #region Public metods

        /// <summary>
        /// Получить ссылку на устройство
        /// </summary>
        public Device GetDevice(UInt16 dsGuid, UInt32 devGuid)
        {
            if (!DataServers.ContainsKey(dsGuid))
                return null;

            var ds = DataServers[dsGuid];
            if (!ds.Devices.ContainsKey(devGuid))
                return null;

            return ds.Devices[devGuid];
        }

        /// <summary>
        /// Получить ссылку на тег
        /// </summary>
        public Tag GetTag(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid)
        {
            var device = GetDevice(dsGuid, devGuid);
            if (device == null)
                return null;

            if (!device.Tags.ContainsKey(tagGuid))
                return null;

            return device.Tags[tagGuid];
        }

        /// <summary>
        /// Получить ссылку на устройство
        /// </summary>
        public Device GetDevice(string deviceGuidAsStr)
        {
            try
            {
                var c = deviceGuidAsStr.Split('.');

                var dsGuid = ushort.Parse(c[0]);
                var devGuid = uint.Parse(c[1]);

                return GetDevice(dsGuid, devGuid);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Получить ссылку на тег
        /// </summary>
        public Tag GetTag(string tagGuidAsStr)
        {
            try
            {
                var c = tagGuidAsStr.Split('.');

                var dsGuid = ushort.Parse(c[0]);
                var devGuid = uint.Parse(c[1]);
                var tagGuid = uint.Parse(c[2]);

                return GetTag(dsGuid, devGuid, tagGuid);
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion

        #region Private metods

        /// <summary>
        /// Сбросить значение всех тегов на знчение по-умолчанию
        /// </summary>
        void ResetAllTags()
        {
            foreach (var dataServer in DataServers.Values)
            {
                foreach (var device in dataServer.Devices.Values)
                {
                    foreach (var tag in device.Tags.Values)
                    {
                        tag.SetDefaultValue();
                    }
                }
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Обработчик события изменения состояния связи с роутером
        /// </summary>
        private void DsRouterProviderConnectionStateChangedHandler(bool b)
        {
            IsConnectionStateOpened = b;

            var dsRouterConnectionStateChanged = DsRouterConnectionStateChanged;
            if (dsRouterConnectionStateChanged != null)
                dsRouterConnectionStateChanged(b);

            ResetAllTags();
        }

        /// <summary>
        /// Обработчик события обновления значений тегов
        /// </summary>
        private void DsRouterProviderTagsValuesUpdatedHandler(Dictionary<string, TagValue> tagValues)
        {
            foreach (var tagFullGuid in tagValues.Keys)
            {
                var c = tagFullGuid.Split('.');

                var dsGuid = UInt16.Parse(c[0]);
                var devGuid = UInt32.Parse(c[1]);
                var tagGuid = UInt32.Parse(c[2]);

                var tag = GetTag(dsGuid, devGuid, tagGuid);
                if (tag != null)
                {
                    var tagValueQuality = tagValues[tagFullGuid].TagValueQuality;
                    var tagValueAsObject = tagValues[tagFullGuid].TagValueAsObject;

                    if (tagValueQuality == TagValueQuality.vqDsr2DsBadConnection)
                    {
                        if (tag.TagValueQuality != TagValueQuality.vqDsr2DsBadConnection)
                            tag.SetDefaultValue(tagValueQuality);
                        continue;
                    }

                    if (tagValueAsObject == null)
                    {
                        tag.SetDefaultValue();
                        continue;
                    }

                    tag.SetTagValue(tagValueAsObject, tagValueQuality, DateTime.Now);
                }
            }
        }

        #endregion
    }
}
