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
        public MainWindow()
        {
            InitializeComponent();

            var pathToParrentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            var pathToConfigurationDirectory = Path.Combine(pathToParrentDirectory, "Project");

            var configurationProvider = ConfigurationProvidersFactory.CreateConfigurationProvider(pathToConfigurationDirectory, false);

            var configurationViewModel = new ConfigurationViewModel(configurationProvider);

            DataContext = configurationViewModel;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
                PropertyGrid.SelectedObject = e.AddedItems[0];

            //List<TagViewModel> selectedCollection;

            //if (PropertyGrid.SelectedObjects is TagViewModel[])
            //    selectedCollection = (PropertyGrid.SelectedObjects as TagViewModel[]).ToList<TagViewModel>();
            //else
            //    selectedCollection = new List<TagViewModel>();

            //if (e.AddedItems.Count > 0)
            //    selectedCollection.AddRange(e.AddedItems.Cast<TagViewModel>().ToList<TagViewModel>());

            //if (e.RemovedItems.Count > 0)
            //    e.RemovedItems.Cast<TagViewModel>().ToList<TagViewModel>().ForEach((i) => selectedCollection.Remove(i));

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
