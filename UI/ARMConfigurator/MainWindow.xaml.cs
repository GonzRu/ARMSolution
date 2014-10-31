using System;
using ARMConfigurator.ViewModels;
using ARMConfigurator.Views;
using ConfigurationParsersLib;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ARMConfigurator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ConfigurationViewModel ConfigurationViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var pathToParrentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            var pathToConfigurationDirectory = Path.Combine(pathToParrentDirectory, "Project");

            var configurationProvider = ConfigurationProvidersFactory.CreateConfigurationProvider(pathToConfigurationDirectory, false);

            ConfigurationViewModel = new ConfigurationViewModel(configurationProvider);            

            DataContext = ConfigurationViewModel;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
                PropertyGrid.SelectedObject = e.AddedItems[0];

            //List<BaseTagViewModel> selectedCollection;

            //if (PropertyGrid.SelectedObjects is BaseTagViewModel[])
            //    selectedCollection = (PropertyGrid.SelectedObjects as BaseTagViewModel[]).ToList<BaseTagViewModel>();
            //else
            //    selectedCollection = new List<BaseTagViewModel>();

            //if (e.AddedItems.Count > 0)
            //    selectedCollection.AddRange(e.AddedItems.Cast<BaseTagViewModel>().ToList<BaseTagViewModel>());

            //if (e.RemovedItems.Count > 0)
            //    e.RemovedItems.Cast<BaseTagViewModel>().ToList<BaseTagViewModel>().ForEach((i) => selectedCollection.Remove(i));

            //PropertyGrid.SelectedObjects = selectedCollection.ToArray();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PropertyGrid.SelectedObject = e.NewValue;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportsView();
            reportWindow.ShowDialog();
        }
    }
}
