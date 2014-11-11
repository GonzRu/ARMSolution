using ArmWpfUI.ViewModels.DeviceViewModels;
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
