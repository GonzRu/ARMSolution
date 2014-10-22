using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SilverlightControlsLibrary
{
    /// <summary>
    /// Базовый класс всех контролов схемы диагностики.
    /// </summary>
    [Description("Базовый элемент диагностической схемы")]
    public class CDiagnosticDevice : Button
    {
        #region Свойства элемента
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Размер шрифта."), Browsable(true)]
        public double ASUTextContentFontSize
        {
            get { return (double)GetValue(ASUTextContentFontSizeProperty); }
            set { SetValue(ASUTextContentFontSizeProperty, value); }
        }
        public static DependencyProperty ASUTextContentFontSizeProperty = DependencyProperty.Register("ASUTextContentFontSize", typeof(double), typeof(CDiagnosticDevice), new PropertyMetadata((double)10));
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]
        public Visibility ASUControlSelectedVisibility
        {
            get { return (Visibility)GetValue(ASUControlSelectedVisibilityProperty); }
            set { SetValue(ASUControlSelectedVisibilityProperty, value); }
        }
        public static DependencyProperty ASUControlSelectedVisibilityProperty = DependencyProperty.Register("ASUControlSelectedVisibility", typeof(Visibility), typeof(CDiagnosticDevice), new PropertyMetadata(Visibility.Collapsed));


        [Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]
        public bool ASUControlISSelected
        {
            get { return (bool)GetValue(ASUControlISSelectedProperty); }
            set { SetValue(ASUControlISSelectedProperty, value); }
        }
        public static DependencyProperty ASUControlISSelectedProperty = DependencyProperty.Register("ASUControlISSelected", typeof(bool), typeof(CDiagnosticDevice), new PropertyMetadata(false, OnASUControlISSelectedChanged));
        private static void OnASUControlISSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CDiagnosticDevice dev = d as CDiagnosticDevice;
            dev.ASUControlSelectedVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Кисть диспетчерского наименования."), Browsable(false)]
        public SolidColorBrush ASUTextColorBrush
        {
            get { return (SolidColorBrush)GetValue(ASUTextColorBrushProperty); }
            set { SetValue(ASUTextColorBrushProperty, value); }
        }
        public static DependencyProperty ASUTextColorBrushProperty = DependencyProperty.Register("ASUTextColorBrush", typeof(SolidColorBrush), typeof(CDiagnosticDevice), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 190, 0))));//, OnASUTextColorBrushChanged));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет текста диспетчерского наименования."), Browsable(true)]
        public Color ASUTextColor
        {
            get { return (Color)GetValue(ASUTextColorProperty); }
            set { SetValue(ASUTextColorProperty, value); }
        }
        public static DependencyProperty ASUTextColorProperty = DependencyProperty.Register("ASUTextColor", typeof(Color), typeof(CDiagnosticDevice), new PropertyMetadata(Color.FromArgb(255, 255, 190, 0), OnASUTextColorChanged));
        private static void OnASUTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CDiagnosticDevice dev = d as CDiagnosticDevice;
            dev.ASUTextColorBrush = new SolidColorBrush((Color)e.NewValue);
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Кисть элемента."), Browsable(false)]
        public SolidColorBrush ASUElementColorBrush
        {
            get { return (SolidColorBrush)GetValue(ASUElementColorBrushProperty); }
            set { SetValue(ASUElementColorBrushProperty, value); }
        }
        public static DependencyProperty ASUElementColorBrushProperty = DependencyProperty.Register("ASUElementColorBrush", typeof(SolidColorBrush), typeof(CDiagnosticDevice), new PropertyMetadata(new SolidColorBrush(Colors.Green)));//, OnASUTextColorBrushChanged));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет элемента."), Browsable(true)]
        public Color ASUElementColor
        {
            get { return (Color)GetValue(ASUElementColorProperty); }
            set { SetValue(ASUElementColorProperty, value); }
        }
        public static DependencyProperty ASUElementColorProperty = DependencyProperty.Register("ASUElementColor", typeof(Color), typeof(CDiagnosticDevice), new PropertyMetadata(Colors.Green, ASUElementColorPropertyChanged));
        private static void ASUElementColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CDiagnosticDevice dev = d as CDiagnosticDevice;
            dev.ASUElementColorBrush = new SolidColorBrush((Color)e.NewValue);
        }

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Подсветка ошибки элемента."), Browsable(false)]
        public Visibility ASUControlErrorVisibility
        {
            get { return (Visibility)GetValue(ASUControlErrorVisibilityProperty); }
            set { SetValue(ASUControlErrorVisibilityProperty, value); }
        }
        public static DependencyProperty ASUControlErrorVisibilityProperty = DependencyProperty.Register("ASUControlErrorVisibility", typeof(Visibility), typeof(CDiagnosticDevice), new PropertyMetadata(Visibility.Visible));


        [Category("Свойства элемента мнемосхемы"), Description("Подсветка ошибки элемента."), Browsable(false)]
        public bool ASUControlError
        {
            get { return (bool)GetValue(ASUControlErrorProperty); }
            set { SetValue(ASUControlErrorProperty, value); }
        }
        public static DependencyProperty ASUControlErrorProperty = DependencyProperty.Register("ASUControlError", typeof(bool), typeof(CDiagnosticDevice), new PropertyMetadata(true, OnASUControlErrorPropertyChanged));
        private static void OnASUControlErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CDiagnosticDevice dev = d as CDiagnosticDevice;
            dev.ASUControlErrorVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Диспетчерское наименование элемента."), Browsable(true)]
        public string ASUTextContent
        {
            get { return (string)GetValue(ASUTextContentProperty); }
            set { SetValue(ASUTextContentProperty, value); }
        }
        public static readonly DependencyProperty ASUTextContentProperty = DependencyProperty.Register("ASUTextContent", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("Терминал РЗА"));
       
        #endregion Свойства элемента

        //=======================================================================
       
        #region Привязки 

        [Category("Привязки данных"), Description("ID тега состояния элемента. Например, рассчетное значение состояния связи с устройством. Для привязки достаточно перетащить тег из дерева проекта слева на элемент."), Browsable(true)]
        public string ASUTagIDState
        {
            get { return (string)GetValue(ASUTagIDStateProperty); }
            set { SetValue(ASUTagIDStateProperty, value); }
        }
        public static DependencyProperty ASUTagIDStateProperty = DependencyProperty.Register("ASUTagIDState", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        //=======================================================================

        [Category("Привязки данных"), Description("ID привязанного устройства. Привязывается автоматически при перетаскивании на схему устройства из дерева проекта слева, но можно изменить"), Browsable(true)]
        public string ASUDeviceIDs
        {
            get { return (string)GetValue(ASUDeviceIDsProperty); }
            set { SetValue(ASUDeviceIDsProperty, value); }
        }
        public static DependencyProperty ASUDeviceIDsProperty = DependencyProperty.Register("ASUDeviceIDs", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata( "-1" ));
        
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды включения."), Browsable(true)]
        //public string ASUTagIDCommandOn
        //{
        //    get { return (string)GetValue(ASUTagIDCommandOnProperty); }
        //    set { SetValue(ASUTagIDCommandOnProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandOnProperty = DependencyProperty.Register("ASUTagIDCommandOn", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды отключения."), Browsable(true)]
        //public string ASUTagIDCommandOff
        //{
        //    get { return (string)GetValue(ASUTagIDCommandOffProperty); }
        //    set { SetValue(ASUTagIDCommandOffProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandOffProperty = DependencyProperty.Register("ASUTagIDCommandOff", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды выдачи готовности управления КА."), Browsable(true)]
        //public string ASUTagIDCommandReady
        //{
        //    get { return (string)GetValue(ASUTagIDCommandReadyProperty); }
        //    set { SetValue(ASUTagIDCommandReadyProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandReadyProperty = DependencyProperty.Register("ASUTagIDCommandReady", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды сброса ресурса КА."), Browsable(true)]
        //public string ASUTagIDCommandClearResource
        //{
        //    get { return (string)GetValue(ASUTagIDCommandClearResourceProperty); }
        //    set { SetValue(ASUTagIDCommandClearResourceProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandClearResourceProperty = DependencyProperty.Register("ASUTagIDCommandClearResource", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды квитирования."), Browsable(true)]
        //public string ASUTagIDCommandAccept
        //{
        //    get { return (string)GetValue(ASUTagIDCommandAcceptProperty); }
        //    set { SetValue(ASUTagIDCommandAcceptProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandAcceptProperty = DependencyProperty.Register("ASUTagIDCommandAccept", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
       
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды записи уставок."), Browsable(true)]
        //public string ASUTagIDCommandWriteSettings
        //{
        //    get { return (string)GetValue(ASUTagIDCommandWriteSettingsProperty); }
        //    set { SetValue(ASUTagIDCommandWriteSettingsProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandWriteSettingsProperty = DependencyProperty.Register("ASUTagIDCommandWriteSettings", typeof(string), typeof(CDiagnosticDevice), new PropertyMetadata("-1"));
        
        //=======================================================================
        //=======================================================================

        #endregion Привязки

        public CDiagnosticDevice()
        {
            this.DefaultStyleKey = typeof(CDiagnosticDevice);
        }
    }
}
