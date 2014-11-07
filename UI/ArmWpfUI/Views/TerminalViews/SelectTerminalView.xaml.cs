using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ArmWpfUI.Views.DeviceViews;
using CoreLib.Models.Configuration;

namespace ArmWpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectTerminalView.xaml
    /// </summary>
    public partial class SelectTerminalView : Window
    {
        public SelectTerminalView(string terminals)
        {
            InitializeComponent();

            #region Кнопки - привязанные устройства
            if (terminals.Equals("-1"))
            {
                TextBlockTerminalsComment.Text = "К данному элементу не привязано ни одного терминала";
            }
            else
            {
                TextBlockTerminalsComment.Text = "К данному элементу привязаны следующие терминалы:";
                string[] devices = terminals.Split(';');
                foreach (string deviceID in devices)
                {
                    if (!string.IsNullOrWhiteSpace(deviceID))
                    {
                        string deviceName = "";
                        string[] addressParts = deviceID.Split('.');

                        var dsGuid = UInt16.Parse(addressParts[0]);
                        var devGuid = UInt32.Parse(addressParts[1]);

                        var device = App.Configuration.GetDevice(dsGuid, devGuid);
                        if (device != null)
                        {
                            deviceName = device.DeviceTypeName;

                            Button btn = new Button();
                            btn.Tag = device;
                            btn.Height = 30;

                            btn.Content = deviceName;
                            btn.Margin = new Thickness(5);
                            btn.Click += new RoutedEventHandler(btn_Click);
                            StackPanelButtonsTerminals.Children.Add(btn);
                        }
                    }
                }
            }
            #endregion Кнопки - привязанные устройства
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            #region Выбор терминала
            var device = (Device)((Button)sender).Tag;
            try
            {
                Tag = new TerminalView();
            }
            catch (KeyNotFoundException)
            {
            }
            #endregion Выбор терминала
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
