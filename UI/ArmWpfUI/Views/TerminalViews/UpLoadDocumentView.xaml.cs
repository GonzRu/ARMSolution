using ArmWpfUI.ViewModels;
using UICore.Commands;

namespace ArmWpfUI.Views.TerminalViews
{
    /// <summary>
    /// Логика взаимодействия для UpLoadDocumentView.xaml
    /// </summary>
    public partial class UpLoadDocumentView
    {
        private DeviceViewModel _deviceViewModel;

        public UpLoadDocumentView(DeviceViewModel deviceViewModel)
        {
            InitializeComponent();

            _deviceViewModel = deviceViewModel;
            DataContext = _deviceViewModel;
            _deviceViewModel.UploadDocumentAsyncCommand.Executed += OnExecuted;
            _deviceViewModel.UploadDocumentAsyncCommand.Cancelled += OnExecuted;
        }

        private void OnExecuted(object sender, CommandEventArgs args)
        {
            _deviceViewModel.UploadDocumentAsyncCommand.Executed -= OnExecuted;
            _deviceViewModel.UploadDocumentAsyncCommand.Cancelled -= OnExecuted;
            Close();
        }
    }
}
