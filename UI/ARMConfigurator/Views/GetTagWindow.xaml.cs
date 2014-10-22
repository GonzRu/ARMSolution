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

namespace ARMConfigurator.Views
{
    /// <summary>
    /// Логика взаимодействия для GetTagWindow.xaml
    /// </summary>
    public partial class GetTagWindow : Window
    {
        public GetTagWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Tag = TextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
