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
    [Description("Обмотка трансформатора")]
    public class CTransformerCoil : CBaseControl
    {
        //=======================================================================
        
        [Category("Свойства элемента мнемосхемы"), Description("Наличие РПН."), Browsable(true)]
        public bool ASURPNIsExist
        {
            get { return (bool)GetValue(ASURPNIsExistProperty); }
            set { SetValue(ASURPNIsExistProperty, value); }
        }
        public static DependencyProperty ASURPNIsExistProperty = DependencyProperty.Register("ASURPNIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASURPNIsExistChanged));
        private static void OnASURPNIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASURPNVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Наличие РПН."), Browsable(false)]
        public Visibility ASURPNVisibility
        {
            get { return (Visibility)GetValue(ASURPNVisibilityProperty); }
            set { SetValue(ASURPNVisibilityProperty, value); }
        }
        public static DependencyProperty ASURPNVisibilityProperty = DependencyProperty.Register("ASURPNVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));


        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Тип соединения обмоток трансформатора."), Browsable(true)]
        public ASUCoilsConnectionTypes ASUCoilsConnectionType
        {
            get { return (ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionTypeProperty); }
            set { SetValue(ASUCoilsConnectionTypeProperty, value); }
        }
        public static DependencyProperty ASUCoilsConnectionTypeProperty = DependencyProperty.Register("ASUCoilsConnectionType", typeof(ASUCoilsConnectionTypes), typeof(CTransformerCoil), new PropertyMetadata(ASUCoilsConnectionTypes.StarConnection, OnASUCoilsConnectionTypeChanged));
        private static void OnASUCoilsConnectionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUCoilsConnectionType = (ASUCoilsConnectionTypes)e.NewValue;
            #region Установка видимости типов соединения
            switch (ctc.ASUCoilsConnectionType)
            {
                case ASUCoilsConnectionTypes.DeltaConnection:
                    ctc.ASUDeltaConnectionVisibility = Visibility.Visible;
                    ctc.ASUVConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.VConnection:
                    ctc.ASUDeltaConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility = Visibility.Visible;
                    ctc.ASUStarConnectionVisibility = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.StarConnection:
                    ctc.ASUDeltaConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility = Visibility.Visible;
                    break;
                default:// ASUCoilsConnectionTypes.NoCoils
                    ctc.ASUDeltaConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility = Visibility.Collapsed;
                    break;
            }
            #endregion Установка видимости типов соединения
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость треугольника."), Browsable(false)]
        public Visibility ASUDeltaConnectionVisibility
        {
            get { return (Visibility)GetValue(ASUDeltaConnectionVisibilityProperty); }
            set { SetValue(ASUDeltaConnectionVisibilityProperty, value); }
        }
        public static DependencyProperty ASUDeltaConnectionVisibilityProperty = DependencyProperty.Register("ASUDeltaConnectionVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость разомкнутого треугольника."), Browsable(false)]
        public Visibility ASUVConnectionVisibility
        {
            get { return (Visibility)GetValue(ASUVConnectionVisibilityProperty); }
            set { SetValue(ASUVConnectionVisibilityProperty, value); }
        }
        public static DependencyProperty ASUVConnectionVisibilityProperty = DependencyProperty.Register("ASUVConnectionVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость звезды."), Browsable(false)]
        public Visibility ASUStarConnectionVisibility
        {
            get { return (Visibility)GetValue(ASUStarConnectionVisibilityProperty); }
            set { SetValue(ASUStarConnectionVisibilityProperty, value); }
        }
        public static DependencyProperty ASUStarConnectionVisibilityProperty = DependencyProperty.Register("ASUStarConnectionVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Visible));

        //=======================================================================
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Наличие вывода заземления."), Browsable(true)]
        public bool ASUPEConnectionIsExist
        {
            get { return (bool)GetValue(ASUPEConnectionIsExistProperty); }
            set 
            {
                if (!value)
                    SetValue(ASUPEConnectionIsExistProperty, value); 
                else
                {
                    if ((ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionTypeProperty) == ASUCoilsConnectionTypes.StarConnection)
                        SetValue(ASUPEConnectionIsExistProperty, value);
                    else
                    {
                        SetValue(ASUPEConnectionIsExistProperty, !value);
                        //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                    }
                }
                
            }
        }
        public static DependencyProperty ASUPEConnectionIsExistProperty = DependencyProperty.Register("ASUPEConnectionIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUPEConnectionIsExistChanged));
        private static void OnASUPEConnectionIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Что происходит раньше?
            CTransformerCoil ctc = d as CTransformerCoil;
            if( !(bool)e.NewValue )
                ctc.ASUPEConnectionVisibility = Visibility.Collapsed;
            else
            {
                if (ctc.ASUCoilsConnectionType == ASUCoilsConnectionTypes.StarConnection)
                    ctc.ASUPEConnectionVisibility = Visibility.Visible;
                else
                {
                    ctc.ASUPEConnectionVisibility = Visibility.Collapsed;
                    ctc.ASUPEConnectionIsExist = false;
                    //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                }
            }
            
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость вывода заземления."), Browsable(false)]
        public Visibility ASUPEConnectionVisibility
        {
            get { return (Visibility)GetValue(ASUPEConnectionVisibilityProperty); }
            set { SetValue(ASUPEConnectionVisibilityProperty, value); }
        }
        public static DependencyProperty ASUPEConnectionVisibilityProperty = DependencyProperty.Register("ASUPEConnectionVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Автотрансформатор."), Browsable(true)]
        public bool ASUAutoIsExist
        {
            get { return (bool)GetValue(ASUAutoIsExistProperty); }
            set { SetValue(ASUAutoIsExistProperty, value); }
        }
        public static DependencyProperty ASUAutoIsExistProperty = DependencyProperty.Register("ASUAutoIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUAutoIsExistChanged));
        private static void OnASUAutoIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUAutoVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость автотрансформатора."), Browsable(false)]
        public Visibility ASUAutoVisibility
        {
            get { return (Visibility)GetValue(ASUAutoVisibilityProperty); }
            set { SetValue(ASUAutoVisibilityProperty, value); }
        }
        public static DependencyProperty ASUAutoVisibilityProperty = DependencyProperty.Register("ASUAutoVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));


        [Category("Свойства элемента мнемосхемы"), Description("Цвет автотрансформатора в соответствии с классом напряжения."), Browsable(false)]
        //[DisplayName("ffffНеопределенное")]        
        public SolidColorBrush ASUAutoVoltageColor
        {
            get { return (SolidColorBrush)GetValue(ASUAutoVoltageColorProperty); }
            set { SetValue(ASUAutoVoltageColorProperty, value); }
        }
        public static DependencyProperty ASUAutoVoltageColorProperty = DependencyProperty.Register("ASUAutoVoltageColor", typeof(SolidColorBrush), typeof(CTransformerCoil), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 180, 200))));

        
        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения автотрансформатора."), Browsable(true)]
        public ASUCommutationDeviceVoltageClasses ASUAutoVoltage
        {
            get { return (ASUCommutationDeviceVoltageClasses)GetValue(ASUAutoVoltageProperty); }
            set
            {
                SetValue(ASUAutoVoltageProperty, value);
            }
        }
        public static DependencyProperty ASUAutoVoltageProperty = DependencyProperty.Register("ASUAutoVoltage", typeof(ASUCommutationDeviceVoltageClasses), typeof(CTransformerCoil), new PropertyMetadata(ASUCommutationDeviceVoltageClasses.kV110, OnASUAutoVoltagePropertyChanged));
        private static void OnASUAutoVoltagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region класс напряжения автотрансформатора
            CTransformerCoil cis = d as CTransformerCoil;
            cis.ASUAutoVoltage = (ASUCommutationDeviceVoltageClasses)e.NewValue;
            switch (cis.ASUAutoVoltage)
            {
                case ASUCommutationDeviceVoltageClasses.kV1150:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 205, 138, 255));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV750:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV500:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 165, 15, 10));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV400:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 240, 150, 30));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV330:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 140, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV220:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 200, 200, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV150:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 170, 150, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV110:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV35:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 130, 100, 50));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV10:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV6:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 200, 150, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV04:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVGenerator:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 230, 70, 230));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVRepair:
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 205, 255, 155));
                    break;
                default://ASUCommutationDeviceVoltageClasses.kVEmpty
                    cis.ASUAutoVoltageColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;

            }
            #endregion класс напряжения автотрансформатора
        }
       
        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода обмотки."), Browsable(true)]
        public bool ASUCoilLeftExitIsExist
        {
            get { return (bool)GetValue(ASUCoilLeftExitIsExistProperty); }
            set { SetValue(ASUCoilLeftExitIsExistProperty, value); }
        }
        public static DependencyProperty ASUCoilLeftExitIsExistProperty = DependencyProperty.Register("ASUCoilLeftExitIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUCoilLeftExitIsExistChanged));
        private static void OnASUCoilLeftExitIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUCoilLeftExitVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода обмотки."), Browsable(false)]
        public Visibility ASUCoilLeftExitVisibility
        {
            get { return (Visibility)GetValue(ASUCoilLeftExitVisibilityProperty); }
            set { SetValue(ASUCoilLeftExitVisibilityProperty, value); }
        }
        public static DependencyProperty ASUCoilLeftExitVisibilityProperty = DependencyProperty.Register("ASUCoilLeftExitVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));
        
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода обмотки."), Browsable(true)]
        public bool ASUCoilTopExitIsExist
        {
            get { return (bool)GetValue(ASUCoilTopExitIsExistProperty); }
            set { SetValue(ASUCoilTopExitIsExistProperty, value); }
        }
        public static DependencyProperty ASUCoilTopExitIsExistProperty = DependencyProperty.Register("ASUCoilTopExitIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUCoilTopExitIsExistChanged));
        private static void OnASUCoilTopExitIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUCoilTopExitVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода обмотки."), Browsable(false)]
        public Visibility ASUCoilTopExitVisibility
        {
            get { return (Visibility)GetValue(ASUCoilTopExitVisibilityProperty); }
            set { SetValue(ASUCoilTopExitVisibilityProperty, value); }
        }
        public static DependencyProperty ASUCoilTopExitVisibilityProperty = DependencyProperty.Register("ASUCoilTopExitVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода обмотки."), Browsable(true)]
        public bool ASUCoilRightExitIsExist
        {
            get { return (bool)GetValue(ASUCoilRightExitIsExistProperty); }
            set { SetValue(ASUCoilRightExitIsExistProperty, value); }
        }
        public static DependencyProperty ASUCoilRightExitIsExistProperty = DependencyProperty.Register("ASUCoilRightExitIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUCoilRightExitIsExistChanged));
        private static void OnASUCoilRightExitIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUCoilRightExitVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода обмотки."), Browsable(false)]
        public Visibility ASUCoilRightExitVisibility
        {
            get { return (Visibility)GetValue(ASUCoilRightExitVisibilityProperty); }
            set { SetValue(ASUCoilRightExitVisibilityProperty, value); }
        }
        public static DependencyProperty ASUCoilRightExitVisibilityProperty = DependencyProperty.Register("ASUCoilRightExitVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода обмотки."), Browsable(true)]
        public bool ASUCoilBottomExitIsExist
        {
            get { return (bool)GetValue(ASUCoilBottomExitIsExistProperty); }
            set { SetValue(ASUCoilBottomExitIsExistProperty, value); }
        }
        public static DependencyProperty ASUCoilBottomExitIsExistProperty = DependencyProperty.Register("ASUCoilBottomExitIsExist", typeof(bool), typeof(CTransformerCoil), new PropertyMetadata(false, OnASUCoilBottomExitIsExistChanged));
        private static void OnASUCoilBottomExitIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformerCoil ctc = d as CTransformerCoil;
            ctc.ASUCoilBottomExitVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода обмотки."), Browsable(false)]
        public Visibility ASUCoilBottomExitVisibility
        {
            get { return (Visibility)GetValue(ASUCoilBottomExitVisibilityProperty); }
            set { SetValue(ASUCoilBottomExitVisibilityProperty, value); }
        }
        public static DependencyProperty ASUCoilBottomExitVisibilityProperty = DependencyProperty.Register("ASUCoilBottomExitVisibility", typeof(Visibility), typeof(CTransformerCoil), new PropertyMetadata(Visibility.Collapsed));

        
        //=======================================================================
        //=======================================================================
        // Иногда бывает нужно показать более одного выведа (например, для заземления)
        //[Category("Свойства элемента мнемосхемы"), Description("Положение вывода обмотки."), Browsable(true)]
        //public ASUCoilExitPositions ASUCoilExitPosition
        //{
        //    get { return (ASUCoilExitPositions)GetValue(ASUCoilExitPositionProperty); }
        //    set { SetValue(ASUCoilExitPositionProperty, value); }
        //}
        //public static DependencyProperty ASUCoilExitPositionProperty = DependencyProperty.Register("ASUCoilExitPosition", typeof(ASUCoilExitPositions), typeof(CTransformerCoil), new PropertyMetadata(ASUCoilExitPositions.No, OnASUCoilExitPositionPropertyChanged));
        //private static void OnASUCoilExitPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    #region положение вывода обмотки
        //    CTransformerCoil cis = d as CTransformerCoil;
        //    cis.ASUCoilExitPosition = (ASUCoilExitPositions)e.NewValue;
        //    switch (cis.ASUCoilExitPosition)
        //    {
        //        case ASUCoilExitPositions.Left:
        //            cis.ASUCoilLeftExitVisibility = Visibility.Visible;
        //            cis.ASUCoilTopExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility = Visibility.Collapsed;
        //            break;

        //        case ASUCoilExitPositions.Top:
        //            cis.ASUCoilLeftExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility = Visibility.Visible;
        //            cis.ASUCoilRightExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility = Visibility.Collapsed;
        //            break;


        //        case ASUCoilExitPositions.Right:
        //            cis.ASUCoilLeftExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility = Visibility.Visible;
        //            cis.ASUCoilBottomExitVisibility = Visibility.Collapsed;
        //            break;
        //        case ASUCoilExitPositions.Bottom:
        //            cis.ASUCoilLeftExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility = Visibility.Visible;
        //            break;

        //        default://нет
        //            cis.ASUCoilLeftExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility = Visibility.Collapsed;
        //            break;
        //    }
        //    #endregion положение вывода обмотки
        //}
               
        //=======================================================================
        //=======================================================================

        public CTransformerCoil()
        {
            this.DefaultStyleKey = typeof(CTransformerCoil);
        }
    }
}
