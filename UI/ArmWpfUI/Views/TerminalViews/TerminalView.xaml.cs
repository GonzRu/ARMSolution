using ArmWpfUI.ViewModels;
using ArmWpfUI.Views.TerminalViews;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

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
            var oldDataContext = e.OldValue as UICore.ViewModels.GroupViewModel;
            //if (oldDataContext != null)
            //    oldDataContext.UnSubscribeToTagsValuesUpdateCommand.Execute(null);

            var newDataContext = e.NewValue as UICore.ViewModels.GroupViewModel;
            //if (newDataContext != null)
            //    newDataContext.SubscribeToTagsValuesUpdateCommand.Execute(null);
        }

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
