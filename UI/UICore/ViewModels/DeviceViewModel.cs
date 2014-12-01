using System.Linq;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UICore.Commands;

namespace UICore.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        #region Public properties

        #region Properties

        /// <summary>
        /// Номер устройства
        /// </summary>
        [Category("Общее")]
        [DisplayName("Номер устройства")]
        [Description("Номер устройства")]
        public UInt32 DeviceGuid
        {
            get { return Device.DeviceGuid; }
        }

        /// <summary>
        /// Наименование устройства
        /// </summary>
        [Category("Общее")]
        [DisplayName("Имя устройства")]
        [Description("Качество значения тега")]
        public string DeviceName
        {
            get { return Device.DeviceGuid + "@" + Device.DeviceTypeName; }
        }

        /// <summary>
        /// Наименование присоединения устройства
        /// </summary>
        [Category("Общее")]
        [DisplayName("Присоединение")]
        [Description("Описание присоединения устройства")]
        public string DeviceDescription
        {
            get { return Device.DeviceDescription; }
            set { Device.DeviceDescription = value; }
        }

        /// <summary>
        /// Список всех групп
        /// </summary>
        [Browsable(false)]
        public List<GroupViewModel> Groups { get; set; }

        /// <summary>
        /// Список всех тегов устройства
        /// </summary>
        [Browsable(false)]
        public List<TagViewModel> Tags { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Подписаться на обновление всех тегов данного устройства
        /// </summary>
        public AsyncCommand SubscribeToAllTagsAsyncCommand { get; set; }

        /// <summary>
        /// Отписаться от обновления всех тегов
        /// </summary>
        public AsyncCommand UnSubscribeFromAllTagsAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        /// <summary>
        /// Ссылка на модель данных устройства
        /// </summary>
        protected readonly Device Device;

        /// <summary>
        /// Провайдер обмена с сервером данных
        /// </summary>
        protected readonly IExchangeProvider ExchangeProvider;

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider)
        {
            Device = device;
            ExchangeProvider = exchangeProvider;

            Groups = new List<GroupViewModel>();
            Tags = new List<TagViewModel>();
            foreach (var group in Device.Groups)
            {
                var groupViewModel = new GroupViewModel(group, exchangeProvider);
                Groups.Add(groupViewModel);

                Tags.AddRange(GetGroupTags(groupViewModel));
            }

            SubscribeToAllTagsAsyncCommand = new AsyncCommand(SubscribeToAllTags);
            UnSubscribeFromAllTagsAsyncCommand = new AsyncCommand(UnSubscribeFromAllTags);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        private void SubscribeToAllTags()
        {
            ExchangeProvider.SubscribeToTagsValuesUpdate(Tags.Select(model => model.TagFullGuid).ToList());
        }

        private void UnSubscribeFromAllTags()
        {
            ExchangeProvider.UnSubscribeToTagsValuesUpdate(Tags.Select(model => model.TagFullGuid).ToList());
        }

        #endregion

        #region Вспомогательные методы

        private List<TagViewModel> GetGroupTags(GroupViewModel groupViewModel)
        {
            var result = new List<TagViewModel>();

            foreach (var subGroup in groupViewModel.SubGroups)
            {
                result.AddRange(GetGroupTags(subGroup));
            }

            if (groupViewModel.Tags != null)
                result.AddRange(groupViewModel.Tags);

            return result;
        }

        #endregion

        #endregion
    }
}
