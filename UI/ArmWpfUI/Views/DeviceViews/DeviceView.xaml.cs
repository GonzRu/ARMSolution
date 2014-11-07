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
