using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using ARMConfigurator.ViewModels;
using System.Windows;
using CoreLib.Models.Common.Reports;
using Microsoft.Win32;

namespace ARMConfigurator.Views
{
    /// <summary>
    /// Логика взаимодействия для ReportsView.xaml
    /// </summary>
    public partial class ReportsView : Window
    {
        public ReportsView()
        {
            InitializeComponent();

            DataContext = new ReportsViewModel();
        }

        private void GetReportButtonClickHandler(object sender, RoutedEventArgs e)
        {
            var reportsViewModel = DataContext as ReportsViewModel;
            if (reportsViewModel == null)
                return;

            byte[] reportContent = null;

            if (DailyReportRadioButton.IsChecked.Value)
            {
            }

            if (EventsReportRadioButton.IsChecked.Value)
            {
                reportsViewModel.EventsReport.ReportExtension = reportsViewModel.ReportExtension;
                reportContent = App.Configuration.DsRouterProvider.GetReportAsByteArray(reportsViewModel.EventsReport);
            }

            if (TagsReportRadioButton.IsChecked.Value)
            {
                if (ComboBox.SelectedValue == null)
                    return;

                if (reportsViewModel.Tags.Count == 0)
                    return;

                if ((ComboBox.SelectedValue as ComboBoxItem).Content.Equals("Обычный отчет по тегам"))
                    reportsViewModel.TagsReport.ReportTamplateName = String.Empty;
                else
                    reportsViewModel.TagsReport.ReportTamplateName = "Tags4Report";

                reportsViewModel.TagsReport.ReportExtension = reportsViewModel.ReportExtension;
                reportsViewModel.TagsReport.Tags = new List<string>(reportsViewModel.Tags);

                reportContent = App.Configuration.DsRouterProvider.GetReportAsByteArray(reportsViewModel.TagsReport);
            }

            if (reportContent == null)
                return;

            var saveFileDialog = new SaveFileDialog();

            var dialogFilter = String.Empty;
            switch (reportsViewModel.ReportExtension)
            {
                case ReportExtension.doc:
                    dialogFilter = "MS Word (*.doc)|*.doc";
                    saveFileDialog.DefaultExt = ".doc";
                    break;
                case ReportExtension.pdf:
                    dialogFilter = "Portable Doc File (*.pdf)|*.pdf";
                    saveFileDialog.DefaultExt = ".pdf";
                    break;
                case ReportExtension.xls:
                    dialogFilter = "MS Excel (*.xls)|*.xls";
                    saveFileDialog.DefaultExt = ".xls";
                    break;
                case ReportExtension.xlsx:
                    dialogFilter = "MS Excel (*.xlsx)|*.xlsx";
                    saveFileDialog.DefaultExt = ".xlsx";
                    break;
            }
            saveFileDialog.Filter = dialogFilter;
            saveFileDialog.AddExtension = true;
            //saveFileDialog.CheckFileExists = true;
            saveFileDialog.FileName = "Отчет";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fileStream =  saveFileDialog.OpenFile())
                {
                    fileStream.Write(reportContent, 0, reportContent.Length);
                    fileStream.Close();
                }
            }
        }
    }
}
