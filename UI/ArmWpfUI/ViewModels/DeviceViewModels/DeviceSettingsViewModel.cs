using System.Linq;
using CoreLib.ExchangeProviders;
using System.Collections.Generic;
using CoreLib.Models.Common;
using CoreLib.Models.Configuration;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    public class DeviceSettingsViewModel : ViewModelBase
    {
        #region Public properties

        #region properties

        /// <summary>
        /// Список групп. На данный момент не предполагает динамического измененияs
        /// </summary>
        public List<GroupViewModel> Groups { get; set; }

        /// <summary>
        /// Содержит значения уставок текущей выделенной группы
        /// </summary>
        public List<DeviceSettingValueViewModel> CurrentGroupSettingsValues
        {
            get { return _currentGroupSettingsValues; }
            set
            {
                _currentGroupSettingsValues = value;
                NotifyPropertyChanged("CurrentGroupSettingsValues");
            }
        }
        private List<DeviceSettingValueViewModel> _currentGroupSettingsValues;

        /// <summary>
        /// Флаг включенного режима редактирования уставок
        /// </summary>
        public bool IsEditSettingsModeEnable
        {
            get { return _isEditSettingsModeEnable; }
            set
            {
                _isEditSettingsModeEnable = value;
                NotifyPropertyChanged("IsEditSettingsModeEnable");
            }
        }
        private bool _isEditSettingsModeEnable = false;

        #endregion

        #region Commands

        /// <summary>
        /// Подготавливает данные указанной группы для отображения
        /// </summary>
        public AsyncCommand PrepareSettingsAsyncCommand { get; set; }

        /// <summary>
        /// Включить режим редактирования уставок
        /// </summary>
        public Command SetOnEditSettingsModeCommand { get; set; }

        /// <summary>
        /// Выключить режим правки уставок
        /// </summary>
        public Command SetOffEditSettingsModeCommand { get; set; }

        /// <summary>
        /// Выполняет сохранение уставок устройства
        /// </summary>
        public AsyncCommand SaveSettingsSetAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        private IExchangeProvider _exchangeProvider;

        private Dictionary<string, DeviceSettingValueViewModel> SettingsValues;

        private Device _device;

        #endregion

        #region Constructors

        public DeviceSettingsViewModel(Device device, List<GroupViewModel> groups, IExchangeProvider exchangeProvider)
        {
            Groups = groups;
            _exchangeProvider = exchangeProvider;
            _device = device;

            CreateDeviceSettingsValues();
            //SettingsValues = new Dictionary<string, DeviceSettingValueViewModel>();

            PrepareSettingsAsyncCommand = new AsyncCommand(PrepareSettings);
            SetOnEditSettingsModeCommand = new Command(SetOnEditSettingsMode);
            SetOffEditSettingsModeCommand = new Command(SetOffEditSettingsMode);
            SaveSettingsSetAsyncCommand = new AsyncCommand(SaveSettingsSet);
        }

        #endregion

        #region Private metods

        #region Commands implementation

        private void PrepareSettings(object param)
        {
            if (!(param is GroupViewModel))
                return;

            var groupViewModel = param as GroupViewModel;
            if (groupViewModel.Tags == null)
                return;

            CurrentGroupSettingsValues = new List<DeviceSettingValueViewModel>(groupViewModel.Tags.Count);
            foreach (var tagViewModel in groupViewModel.Tags)
            {
                if (!SettingsValues.ContainsKey(tagViewModel.TagFullGuid))
                    SettingsValues.Add(tagViewModel.TagFullGuid, new DeviceSettingValueViewModel(tagViewModel));

                CurrentGroupSettingsValues.Add(SettingsValues[tagViewModel.TagFullGuid]);
            }
        }

        private void SetOnEditSettingsMode()
        {
            IsEditSettingsModeEnable = true;

            foreach (var deviceSettingValueViewModel in SettingsValues.Values)
            {
                deviceSettingValueViewModel.SetOnEditSettingsMode();
            }
        }

        private void SetOffEditSettingsMode()
        {
            IsEditSettingsModeEnable = false;

            foreach (var deviceSettingValueViewModel in SettingsValues.Values)
            {
                deviceSettingValueViewModel.NewSettingsValue = null;
            }
        }

        private void SaveSettingsSet()
        {
            var changedSettings = SettingsValues.Where(pair => pair.Value.IsValueChanged).ToDictionary(pair => pair.Key, pair => new TagValue {TagValueAsObject = pair.Value.NewSettingsValue});

            _exchangeProvider.SaveSettingsToDevice(_device.DataServer.DsGuid, _device.DeviceGuid, changedSettings);
            IsEditSettingsModeEnable = false;
        }

        #endregion

        #region Подготовка 

        /// <summary>
        ///
        /// </summary>
        private void CreateDeviceSettingsValues()
        {
            var tags = new List<TagViewModel>();

            foreach (var groupViewModel in Groups)
            {
                tags.AddRange(GetAllGroupTags(groupViewModel));
            }

            SettingsValues = tags.ToDictionary(
                tagViewModel => tagViewModel.TagFullGuid,
                tagViewModel => new DeviceSettingValueViewModel(tagViewModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupViewModel"></param>
        /// <returns></returns>
        private List<TagViewModel> GetAllGroupTags(GroupViewModel groupViewModel)
        {
            var tags = new List<TagViewModel>();

            if (groupViewModel.SubGroups != null)
                foreach (var subGroup in groupViewModel.SubGroups)
                {
                    tags.AddRange(GetAllGroupTags(subGroup));
                }

            if (groupViewModel.Tags != null)
                tags.AddRange(groupViewModel.Tags);

            return tags;
        }

        #endregion

        #endregion
    }
}
