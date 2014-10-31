using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UICore.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        #region Public properties

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
        }

        #endregion

        #region Private metods

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
    }
}
