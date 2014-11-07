using ArmWpfUI.ViewModels;
using ArmWpfUI.ViewModels.DeviceViewModels;
using ArmWpfUI.Views.TerminalViews;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace ArmWpfUI.Views.DeviceViews
{
    /// <summary>
    /// Логика взаимодействия для DeviceView.xaml
    /// </summary>
    public partial class DeviceView : Page
    {
        private DeviceViewModel DeviceViewModel { get; set; }

        #region Constructors

        public DeviceView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
