using ARMConfigurator.Views;
using CoreLib.Models.Common.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UICore.Commands;
using UICore.ViewModels;

namespace ARMConfigurator.ViewModels
{
    internal sealed class ReportsViewModel : ViewModelBase
    {
        #region Costructor

        public ReportsViewModel()
        {
            EventsReport = new EventsReportSettings
            {
                StartDateTime = DateTime.Now.AddDays(-1),
                EndDateTime = DateTime.Now
            };
            TagsReport = new TagsReportSettings
            {
                StartDateTime = DateTime.Now.AddMinutes(-10),
                EndDateTime = DateTime.Now
            };

            Tags = new ObservableCollection<string> { "0.263.510", "0.263.511" };

            AddTagCommand = new Command(AddTag);
            DeleteTagsCommand = new Command(DeleteTags);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Формат отчета
        /// </summary>
        public ReportExtension ReportExtension { get; set; }

        /// <summary>
        /// Параметры отчета по событиям
        /// </summary>
        public EventsReportSettings EventsReport { get; set; }

        /// <summary>
        /// Параметры отчета по тегам
        /// </summary>
        public TagsReportSettings TagsReport { get; set; }

        public ObservableCollection<string> Tags { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Добавить тег
        /// </summary>
        public ICommand AddTagCommand { get; set; }

        /// <summary>
        /// Удалить теги
        /// </summary>
        public ICommand DeleteTagsCommand { get; set; }

        #endregion

        #region Private metods

        #region Metods for commands

        private void AddTag()
        {
            var window = new GetTagWindow();
            window.ShowDialog();

            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                var tag = window.Tag as string;

                Tags.Add(tag);
            }
        }

        private void DeleteTags(object param)
        {
            var tags = param as IEnumerable<object>;
            if (tags == null)
                return;

            foreach (var tag in (from tag in tags select tag.ToString()).ToArray())
            {
                Tags.Remove(tag);
            }
        }

        #endregion

        #endregion
    }
}
