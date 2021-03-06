﻿using ConfigurationParsersLib;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UICore.Commands;
using UICore.ViewModels;

namespace ARMConfigurator.ViewModels
{
    internal sealed class ConfigurationViewModel : UICore.ViewModels.ConfigurationViewModel
    {
        #region Public properties

        #region Properties

        /// <summary>
        /// Флаг, показывающий активность какой-либо блокирующей команды
        /// </summary>
        public bool IsBlockCommandActive
        {
            get { return _isBlockCommandActive; }
            set
            {
                _isBlockCommandActive = value;
                NotifyPropertyChanged("IsBlockCommandActive");
            }
        }
        private bool _isBlockCommandActive;

        #endregion

        #region Commands

        public ICommand SubscribeToTagsValueUpdateAsyncCommand { get; set; }
        public ICommand UnSubscribeToTagsValueUpdateAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        /// <summary>
        /// Последняя группа, на которую была организована подиска
        /// </summary>
        private GroupViewModel _lastSubscribedBaseGroup;

        #endregion

        #region Constructor

        public ConfigurationViewModel(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
            ConfigurationProvider = configurationProvider;

            SubscribeToTagsValueUpdateAsyncCommand = new AsyncCommand(SubscribeToTagsValueUpdate);
            UnSubscribeToTagsValueUpdateAsyncCommand = new AsyncCommand(UnSubscribeToTagsValueUpdate);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        private void SubscribeToTagsValueUpdate(object param)
        {
            if (param is GroupViewModel)
            {
                var group = param as GroupViewModel;

                if (group.Tags == null) return;

                if (_lastSubscribedBaseGroup != null)
                    UnSubscribeToTagsValueUpdate(_lastSubscribedBaseGroup);
                _lastSubscribedBaseGroup = group;

                var tagsToRequest = new List<string>();
                foreach (var tagViewModel in group.Tags)
                {
                    tagsToRequest.Add(tagViewModel.TagFullGuid);
                }

                Configuration.DsRouterProvider.SubscribeToTagsValuesUpdate(tagsToRequest);
            }
        }

        private void UnSubscribeToTagsValueUpdate(object param)
        {
            if (param is GroupViewModel)
            {
                var group = param as GroupViewModel;

                if (group.Tags == null) return;

                var tagsToRequest = new List<string>();
                foreach (var tagViewModel in group.Tags)
                {
                    tagsToRequest.Add(tagViewModel.TagFullGuid);
                }

                Configuration.DsRouterProvider.UnSubscribeToTagsValuesUpdate(tagsToRequest);
            }
        }

        protected override void SaveConfiguration()
        {
            IsBlockCommandActive = true;

            base.SaveConfiguration();

            IsBlockCommandActive = false;
        }

        protected override void LoadConfiguration()
        {
            IsBlockCommandActive = true;

            base.LoadConfiguration();

            var tmp = new List<DataServerViewModel>();
            foreach (var ds in Configuration.DataServers.Values)
            {
                tmp.Add(new DataServerViewModel(ds, Configuration.DsRouterProvider));
            }
            DataServers = tmp;

            IsBlockCommandActive = false;

            App.Configuration = Configuration;
        }

        //private void DoBackUp()
        //{
        //    IsBlockCommandActive = true;
        //    NotifyPropertyChanged("IsBlockCommandActive");

        //    ConfigurationProvider.BackUp("Резервная копия");

        //    IsBlockCommandActive = false;
        //    NotifyPropertyChanged("IsBlockCommandActive");
        //}

        protected override void Authorization()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion


    }
}
