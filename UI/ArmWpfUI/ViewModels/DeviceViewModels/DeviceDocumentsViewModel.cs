using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using CoreLib.ExchangeProviders;
using CoreLib.Models.Common;
using UICore.Commands;
using UICore.ViewModels;

namespace ArmWpfUI.ViewModels.DeviceViewModels
{
    public class DeviceDocumentsViewModel : ViewModelBase
    {
        #region CONSTS

        /// <summary>
        /// Размер части файла, которая за раз загружается на сервер
        /// </summary>
        private const int UPLOAD_FILE_CHUNK_LENGTH = 20480;

        #endregion

        #region Public Properties

        #region Properties

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

        #region Commands

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

        #region Private fields

        private ushort _dsGuid;

        private uint _deviceGuid;

        private IExchangeProvider _exchangeProvider;

        #endregion

        #region Constructors

        public DeviceDocumentsViewModel(ushort dsGuid, uint deviceGuid, IExchangeProvider exchangeProvider)
        {
            _dsGuid = dsGuid;
            _deviceGuid = deviceGuid;
            _exchangeProvider = exchangeProvider;

            LoadDocumentsListCommand = new AsyncCommand(LoadDocumentsList);
            UploadDocumentAsyncCommand = new AsyncCommand(UploadDocument);
        }

        #endregion

        #region Private metods

        #region Implementation commands

        /// <summary>
        /// Загружает список документов, привязанных к данному устройству
        /// </summary>
        private void LoadDocumentsList()
        {
            Documents = _exchangeProvider.GetDocumentsList(_dsGuid, _deviceGuid);
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
            if (!_exchangeProvider.InitUploadFileSession(_dsGuid, _deviceGuid, Path.GetFileName(pathToFile), "sdfsdf"))
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
                        _exchangeProvider.TerminateUploadFileSession();
                        return;
                    }

                    _exchangeProvider.UploadFileChunk(fileChunlBuffer);

                    UploadDocumentProgress += progressStep;
                    chunkCount++;
                }

                // Загружаем последний кусок
                if (fileStream.Length > UPLOAD_FILE_CHUNK_LENGTH * chunkCount)
                {
                    fileChunlBuffer = new byte[fileStream.Length - UPLOAD_FILE_CHUNK_LENGTH * chunkCount];
                    fileStream.Read(fileChunlBuffer, 0, fileChunlBuffer.Length);

                    _exchangeProvider.UploadFileChunk(fileChunlBuffer);
                }

                // Проверяем - не отменил ли пользователь загрузку файла
                if (UploadDocumentAsyncCommand.IsCancellationRequested)
                {
                    _exchangeProvider.TerminateUploadFileSession();
                    return;
                }

                _exchangeProvider.SaveUploadedFile();

                LoadDocumentsList();
            }

        }

        #endregion

        #endregion
    }
}
