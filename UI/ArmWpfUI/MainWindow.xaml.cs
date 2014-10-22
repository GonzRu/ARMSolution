using System.Windows.Controls;
using ArmWpfUI.ViewModels;
using ConfigurationParsersLib;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace ArmWpfUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var configurationProvider = ConfigurationProvidersFactory.CreateConfigurationProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project"), false);

            ConfigurationViewModel configurationViewModel = new ConfigurationViewModel(configurationProvider);

            DataContext = configurationViewModel;
        }
    }
}
