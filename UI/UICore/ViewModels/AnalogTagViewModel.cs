using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UICore.ViewModels
{
    public class AnalogTagViewModel : TagViewModel
    {
        #region Public properties

        /// <summary>
        /// Размерность аналоговой величины
        /// </summary>
        [Category("Специфическое")]
        [DisplayName("Размерность")]
        [Description("Размерность аналоговой величины")]
        public string Dim
        {
            get { return (Tag as TagAnalog).Dim; }
            set
            {
                (Tag as TagAnalog).Dim = value;
                NotifyPropertyChanged("Dim");
            }
        }

        /// <summary>
        /// Количество знаков после запятой в строковм представлении
        /// </summary>
        [Category("Специфическое")]
        [DisplayName("Точность")]
        [Description("Отображаемое количество знаков после запятой")]
        public UInt16 Precision
        {
            get { return (Tag as TagAnalog).Precision; }
            set { (Tag as TagAnalog).Precision = value; NotifyPropertyChanged("Precision"); NotifyPropertyChanged("TagValueAsString"); }
        }

        /// <summary>
        /// Коэффициент преобразования тега
        /// </summary>
        [Category("Специфическое")]
        [DisplayName("Коэффициент преобразования")]
        [Description("Коэффициент преобразования")]
        public object TransformationRatio
        {
            get
            {
                if (_transformationRatio == null)
                    GetTransformationRatio();

                return _transformationRatio;
            }
            set
            {
                if (value == null)
                    return;

                Single r;
                if (Single.TryParse(value.ToString().Replace(".", ","), out r))
                {
                    SetTransformationRatio(r);
                }
            }
        }
        private object _transformationRatio = null;

        #endregion

        #region Constructor

        public AnalogTagViewModel(TagAnalog tag, IExchangeProvider exchangeProvider)
            : base(tag, exchangeProvider)
        {
        }

        #endregion

        #region Private fields

        private async void GetTransformationRatio()
        {
            _transformationRatio = await Task<object>.Run(() => ExchangeProvider.GetTagAnalogTransformationRatio(DsGuid, DeviceGuid, TagGuid));

            if (_transformationRatio != null)
                NotifyPropertyChanged("TransformationRatio");
        }

        private async void SetTransformationRatio(Single r)
        {
            await Task.Run(() => ExchangeProvider.SetTagAnalogTransformationRatio(DsGuid, DeviceGuid, TagGuid, r));
            _transformationRatio = r;

            GetTransformationRatio();
        }

        //private void GetTransformationRatio()
        //{
        //    _transformationRatio = Task<object>.Run(() => ExchangeProvider.GetTagAnalogTransformationRatio(DsGuid, DeviceGuid, TagGuid)).Result;

        //    if (_transformationRatio != null)
        //        NotifyPropertyChanged("TransformationRatio");
        //}

        //private void SetTransformationRatio(Single r)
        //{
        //    Task.Run(() => ExchangeProvider.SetTagAnalogTransformationRatio(DsGuid, DeviceGuid, TagGuid, r)).Wait();
        //    _transformationRatio = r;

        //    GetTransformationRatio();
        //}

        #endregion
    }
}
