using ArmWpfUI.ViewModels.DeviceViewModels;
using ArmWpfUI.Views.TerminalViews;
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

            var uploadDocumentProgressView = new UpLoadDocumentView(DataContext as DeviceViewModel);
            uploadDocumentProgressView.Loaded += (o, args) =>
            {
                var deviceViewModel = DataContext as DeviceViewModel;
                deviceViewModel.UploadDocumentAsyncCommand.DoExecute(openFileDialog.FileName);
            };

            uploadDocumentProgressView.ShowDialog();
        }

        #endregion
    }
}
