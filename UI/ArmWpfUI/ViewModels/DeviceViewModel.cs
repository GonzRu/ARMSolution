using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using CoreLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels
{
    public class DeviceViewModel : UICore.ViewModels.DeviceViewModel
    {
        #region CONSTS

        /// <summary>
        /// Размер части файла, которая за раз загружается на сервер
        /// </summary>
        private const int UPLOAD_FILE_CHUNK_LENGTH = 20480;

        #endregion

        #region Public properties

        #region Properties

        #region Группы

        /// <summary>
        /// Список групп, содержащих текущие данные
        /// </summary>
        [Browsable(false)]
        public List<GroupViewModel> CurrentDataGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.CurrentData).ToList(); }
        }

        /// <summary>
        /// Список групп, содержащих уставки устройства
        /// </summary>
        [Browsable(false)]
        public List<GroupViewModel> SettingsGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.Ustavki).ToList(); }
        }

        /// <summary>
        /// Список всех групп
        /// </summary>
        [Browsable(false)]
        public List<GroupViewModel> SpecificGroups
        {
            get { return Groups.Where(model => model.GroupCategory == GroupCategory.Specific).ToList(); }
        }

        #endregion

        #region События устройства

        /// <summary>
        /// Список событий данного устройства
        /// </summary>
        [Browsable(false)]
        public List<EventValue> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                NotifyPropertyChanged("Events");
            }
        }
        private List<EventValue> _events;

        /// <summary>
        /// Начало отсчета показываемых событий устройства
        /// </summary>
        public DateTime EventsStartDateTime { get; set; }

        /// <summary>
        /// Конец отсчета показываемых событий устройства
        /// </summary>
        public DateTime EventsEndDateTime { get; set; }

        #endregion

        #region Документы устройства

        /// <summary>
        /// Список документов, соответствующих данному устройству
        /// </summary>
        [Browsable(false)]
        public List<Document> Documents
        {
            get { return _documents; }
            set
            {
                _documents = value;
                NotifyPropertyChanged("Documents");
            }
        }
        private List<Document> _documents;

        /// <summary>
        /// Прогресс загрузки документа
        /// </summary>
        public float UploadDocumentProgress
        {
            get { return _uploadDocumentProgress; }
            set
            {
                _uploadDocumentProgress = value;
                NotifyPropertyChanged("UploadDocumentProgress");
            }
        }
        private float _uploadDocumentProgress;

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Загружает события данного устройства
        /// </summary>
        public ICommand LoadEventsCommand { get; set; }

        /// <summary>
        /// Загружает список документов данного устройства
        /// </summary>
        public ICommand LoadDocumentsListCommand { get; set; }

        /// <summary>
        /// Загружает документ устройства
        /// </summary>
        public AsyncCommand UploadDocumentAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Constructor

        public DeviceViewModel(Device device, IExchangeProvider exchangeProvider) : base(device, exchangeProvider)
        {
            EventsStartDateTime = DateTime.Now.AddDays(-1).Date;
            EventsEndDateTime = DateTime.Now.Date;

            LoadEventsCommand = new AsyncCommand(LoadEvents);
            LoadDocumentsListCommand = new AsyncCommand(LoadDocumentsList);
            UploadDocumentAsyncCommand = new AsyncCommand(UploadDocument);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        /// <summary>
        /// Загружает события, соответствующие данному устройству
        /// </summary>
        private void LoadEvents()
        {
            Events = ExchangeProvider.GetEvents(EventsStartDateTime, EventsEndDateTime, false, false, true,
                new List<Tuple<ushort, uint>> {new Tuple<ushort, uint>(Device.DataServer.DsGuid, Device.DeviceGuid)});            
        }

        /// <summary>
        /// Загружает список документов, привязанных к данному устройству
        /// </summary>
        private void LoadDocumentsList()
        {
            Documents = ExchangeProvider.GetDocumentsList(Device.DataServer.DsGuid, Device.DeviceGuid);
        }

        /// <summary>
        /// Загружает конкретный документ
        /// </summary>
        private void UploadDocument(object param)
        {
            if (!(param is string))
                return;

            var pathToFile = param.ToString();

            // Проверяем - можно ли инициировать загрузку файла
            if (!ExchangeProvider.InitUploadFileSession(Device.DataServer.DsGuid, DeviceGuid, Path.GetFileName(pathToFile), "sdfsdf"))
                return;

            UploadDocumentProgress = 0;

            using (var fileStream = File.OpenRead(pathToFile))
            {
                int chunkCount = 0;
                byte[] fileChunlBuffer;

                float progressStep = (float)UPLOAD_FILE_CHUNK_LENGTH / fileStream.Length;

                while (fileStream.Length > UPLOAD_FILE_CHUNK_LENGTH * (chunkCount + 1))
                {
                    fileChunlBuffer = new byte[UPLOAD_FILE_CHUNK_LENGTH];
                    fileStream.Read(fileChunlBuffer, 0, UPLOAD_FILE_CHUNK_LENGTH);

                    // Проверяем - не отменил ли пользователь загрузку файла
                    if (UploadDocumentAsyncCommand.IsCancellationRequested)
                    {
                        ExchangeProvider.TerminateUploadFileSession();
                        return;
                    }

                    ExchangeProvider.UploadFileChunk(fileChunlBuffer);

                    UploadDocumentProgress += progressStep;
                    chunkCount++;
                }

                // Загружаем последний кусок
                if (fileStream.Length > UPLOAD_FILE_CHUNK_LENGTH*chunkCount)
                {
                    fileChunlBuffer = new byte[fileStream.Length - UPLOAD_FILE_CHUNK_LENGTH * chunkCount];
                    fileStream.Read(fileChunlBuffer, 0, fileChunlBuffer.Length);

                    ExchangeProvider.UploadFileChunk(fileChunlBuffer);
                }

                // Проверяем - не отменил ли пользователь загрузку файла
                if (UploadDocumentAsyncCommand.IsCancellationRequested)
                {
                    ExchangeProvider.TerminateUploadFileSession();
                    return;
                }

                ExchangeProvider.SaveUploadedFile();

                LoadDocumentsList();
            }

        }

        #endregion

        #endregion
    }
}
