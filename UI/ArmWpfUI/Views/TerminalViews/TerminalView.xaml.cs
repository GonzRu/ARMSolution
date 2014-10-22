using System;
using System.Windows;
using System.Windows.Controls;
using ArmWpfUI.ViewModels;
using CoreLib.Models.Configuration;
using UICore.ViewModels;

namespace ArmWpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для TerminalView.xaml
    /// </summary>
    public partial class TerminalView : Page
    {
        private DeviceViewModel DeviceViewModel { get; set; }

        #region Constructors

        public TerminalView()
        {
            InitializeComponent();
        }

        #endregion

        #region Handlers

        private void DataGridDataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldDataContext = e.OldValue as BaseGroupViewModel;
            //if (oldDataContext != null)
            //    oldDataContext.UnSubscribeToTagsValuesUpdateCommand.Execute(null);

            var newDataContext = e.NewValue as BaseGroupViewModel;
            //if (newDataContext != null)
            //    newDataContext.SubscribeToTagsValuesUpdateCommand.Execute(null);
        }

        #endregion
    }
}
