using ArmWpfUI.ViewModels.DeviceViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace ArmWpfUI.Views.DeviceViews
{
    /// <summary>
    /// Логика взаимодействия для DeviceDocumentsControl.xaml
    /// </summary>
    public partial class DeviceDocumentsControl : UserControl
    {
        public DeviceDocumentsControl()
        {
            InitializeComponent();
        }

        #region Handlers

        private void UploadDocumentButtonOnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (!openFileDialog.ShowDialog(Application.Current.MainWindow).Value)
                return;

            var deviceViewModel = DataContext as DeviceDocumentsViewModel;
            deviceViewModel.UploadDocumentAsyncCommand.DoExecute(openFileDialog.FileName);
        }

        #endregion
    }
}
