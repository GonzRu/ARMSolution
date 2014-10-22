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
    [Description("Двухобмоточный трансформатор")]
    public class CTransformer2Coils : CTransformerCoil
    {

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Тип соединения вторых обмоток трансформатора."), Browsable(true)]
        public ASUCoilsConnectionTypes ASUCoilsConnectionType2
        {
            get { return (ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionType2Property); }
            set { SetValue(ASUCoilsConnectionType2Property, value); }
        }
        public static DependencyProperty ASUCoilsConnectionType2Property = DependencyProperty.Register("ASUCoilsConnectionType2", typeof(ASUCoilsConnectionTypes), typeof(CTransformer2Coils), new PropertyMetadata(ASUCoilsConnectionTypes.DeltaConnection, OnASUCoilsConnectionType2Changed));
        private static void OnASUCoilsConnectionType2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer2Coils ctc = d as CTransformer2Coils;
            ctc.ASUCoilsConnectionType2 = (ASUCoilsConnectionTypes)e.NewValue;
            #region Установка видимости типов соединения
            switch (ctc.ASUCoilsConnectionType2)
            {
                case ASUCoilsConnectionTypes.DeltaConnection:
                    ctc.ASUDeltaConnectionVisibility2 = Visibility.Visible;
                    ctc.ASUVConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility2 = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.VConnection:
                    ctc.ASUDeltaConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility2 = Visibility.Visible;
                    ctc.ASUStarConnectionVisibility2 = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.StarConnection:
                    ctc.ASUDeltaConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility2 = Visibility.Visible;
                    break;
                default:// ASUCoilsConnectionTypes.NoCoils
                    ctc.ASUDeltaConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility2 = Visibility.Collapsed;
                    break;
            }
            #endregion Установка видимости типов соединения
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость  треугольника 2."), Browsable(false)]
        public Visibility ASUDeltaConnectionVisibility2
        {
            get { return (Visibility)GetValue(ASUDeltaConnectionVisibility2Property); }
            set { SetValue(ASUDeltaConnectionVisibility2Property, value); }
        }
        public static DependencyProperty ASUDeltaConnectionVisibility2Property = DependencyProperty.Register("ASUDeltaConnectionVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Visible));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость разомкнутого треугольника 2."), Browsable(false)]
        public Visibility ASUVConnectionVisibility2
        {
            get { return (Visibility)GetValue(ASUVConnectionVisibility2Property); }
            set { SetValue(ASUVConnectionVisibility2Property, value); }
        }
        public static DependencyProperty ASUVConnectionVisibility2Property = DependencyProperty.Register("ASUVConnectionVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость звезды2."), Browsable(false)]
        public Visibility ASUStarConnectionVisibility2
        {
            get { return (Visibility)GetValue(ASUStarConnectionVisibility2Property); }
            set { SetValue(ASUStarConnectionVisibility2Property, value); }
        }
        public static DependencyProperty ASUStarConnectionVisibility2Property = DependencyProperty.Register("ASUStarConnectionVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Наличие вывода заземления вторичной обмотки."), Browsable(true)]
        public bool ASUPEConnectionIsExist2
        {
            get { return (bool)GetValue(ASUPEConnectionIsExist2Property); }
            set
            {
                if (!value)
                    SetValue(ASUPEConnectionIsExist2Property, value);
                else
                {
                    if ((ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionType2Property) == ASUCoilsConnectionTypes.StarConnection)
                        SetValue(ASUPEConnectionIsExist2Property, value);
                    else
                    {
                        SetValue(ASUPEConnectionIsExist2Property, !value);
                        //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                    }
                }

            }
        }
        public static DependencyProperty ASUPEConnectionIsExist2Property = DependencyProperty.Register("ASUPEConnectionIsExist2", typeof(bool), typeof(CTransformer2Coils), new PropertyMetadata(false, OnASUPEConnectionIsExist2Changed));
        private static void OnASUPEConnectionIsExist2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Что происходит раньше?
            CTransformer2Coils ctc = d as CTransformer2Coils;
            if (!(bool)e.NewValue)
                ctc.ASUPEConnectionVisibility2 = Visibility.Collapsed;
            else
            {
                if (ctc.ASUCoilsConnectionType2 == ASUCoilsConnectionTypes.StarConnection)
                    ctc.ASUPEConnectionVisibility2 = Visibility.Visible;
                else
                {
                    ctc.ASUPEConnectionVisibility2 = Visibility.Collapsed;
                    ctc.ASUPEConnectionIsExist2 = false;
                    //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                }
            }

        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость вывода вторичной обмотки."), Browsable(false)]
        public Visibility ASUPEConnectionVisibility2
        {
            get { return (Visibility)GetValue(ASUPEConnectionVisibility2Property); }
            set { SetValue(ASUPEConnectionVisibility2Property, value); }
        }
        public static DependencyProperty ASUPEConnectionVisibility2Property = DependencyProperty.Register("ASUPEConnectionVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Цвет вторичной обмотки в соответствии с классом напряжения."), Browsable(false)]
        public SolidColorBrush ASUVoltageColor2
        {
            get { return (SolidColorBrush)GetValue(ASUVoltageColor2Property); }
            set { SetValue(ASUVoltageColor2Property, value); }
        }
        public static DependencyProperty ASUVoltageColor2Property = DependencyProperty.Register("ASUVoltageColor2", typeof(SolidColorBrush), typeof(CTransformer2Coils), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 180, 200))));


        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения вторичной обмотки."), Browsable(true)]
        public ASUCommutationDeviceVoltageClasses ASUVoltage2
        {
            get { return (ASUCommutationDeviceVoltageClasses)GetValue(ASUVoltage2Property); }
            set { SetValue(ASUVoltage2Property, value); }
        }
        public static DependencyProperty ASUVoltage2Property = DependencyProperty.Register("ASUVoltage2", typeof(ASUCommutationDeviceVoltageClasses), typeof(CTransformer2Coils), new PropertyMetadata(ASUCommutationDeviceVoltageClasses.kV110, OnASUVoltage2PropertyChanged));
        private static void OnASUVoltage2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region класс напряжения вторичной обмотки
            CTransformer2Coils cis = d as CTransformer2Coils;
            cis.ASUVoltage2 = (ASUCommutationDeviceVoltageClasses)e.NewValue;
            switch (cis.ASUVoltage2)
            {
                case ASUCommutationDeviceVoltageClasses.kV1150:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 205, 138, 255));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV750:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 0, 0, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV500:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 165, 15, 10));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV400:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 240, 150, 30));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV330:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 0, 140, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV220:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 200, 200, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV150:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 170, 150, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV110:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV35:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 130, 100, 50));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV10:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV6:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 200, 150, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV04:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVGenerator:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 230, 70, 230));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVRepair:
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 205, 255, 155));
                    break;
                default://ASUCommutationDeviceVoltageClasses.kVEmpty
                    cis.ASUVoltageColor2 = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;

            }
            #endregion класс напряжения вторичной обмотки
        }

        //=======================================================================
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода вторичной обмотки."), Browsable(true)]
        public bool ASUCoilLeftExitIsExist2
        {
            get { return (bool)GetValue(ASUCoilLeftExitIsExist2Property); }
            set { SetValue(ASUCoilLeftExitIsExist2Property, value); }
        }
        public static DependencyProperty ASUCoilLeftExitIsExist2Property = DependencyProperty.Register("ASUCoilLeftExitIsExist2", typeof(bool), typeof(CTransformer2Coils), new PropertyMetadata(false, OnASUCoilLeftExitIsExist2Changed));
        private static void OnASUCoilLeftExitIsExist2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer2Coils ctc = d as CTransformer2Coils;
            ctc.ASUCoilLeftExitVisibility2 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода вторичной обмотки."), Browsable(false)]
        public Visibility ASUCoilLeftExitVisibility2
        {
            get { return (Visibility)GetValue(ASUCoilLeftExitVisibility2Property); }
            set { SetValue(ASUCoilLeftExitVisibility2Property, value); }
        }
        public static DependencyProperty ASUCoilLeftExitVisibility2Property = DependencyProperty.Register("ASUCoilLeftExitVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода вторичной обмотки."), Browsable(true)]
        public bool ASUCoilTopExitIsExist2
        {
            get { return (bool)GetValue(ASUCoilTopExitIsExist2Property); }
            set { SetValue(ASUCoilTopExitIsExist2Property, value); }
        }
        public static DependencyProperty ASUCoilTopExitIsExist2Property = DependencyProperty.Register("ASUCoilTopExitIsExist2", typeof(bool), typeof(CTransformer2Coils), new PropertyMetadata(false, OnASUCoilTopExitIsExist2Changed));
        private static void OnASUCoilTopExitIsExist2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer2Coils ctc = d as CTransformer2Coils;
            ctc.ASUCoilTopExitVisibility2 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода вторичной обмотки."), Browsable(false)]
        public Visibility ASUCoilTopExitVisibility2
        {
            get { return (Visibility)GetValue(ASUCoilTopExitVisibility2Property); }
            set { SetValue(ASUCoilTopExitVisibility2Property, value); }
        }
        public static DependencyProperty ASUCoilTopExitVisibility2Property = DependencyProperty.Register("ASUCoilTopExitVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода вторичной обмотки."), Browsable(true)]
        public bool ASUCoilRightExitIsExist2
        {
            get { return (bool)GetValue(ASUCoilRightExitIsExist2Property); }
            set { SetValue(ASUCoilRightExitIsExist2Property, value); }
        }
        public static DependencyProperty ASUCoilRightExitIsExist2Property = DependencyProperty.Register("ASUCoilRightExitIsExist2", typeof(bool), typeof(CTransformer2Coils), new PropertyMetadata(false, OnASUCoilRightExitIsExist2Changed));
        private static void OnASUCoilRightExitIsExist2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer2Coils ctc = d as CTransformer2Coils;
            ctc.ASUCoilRightExitVisibility2 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода вторичной обмотки."), Browsable(false)]
        public Visibility ASUCoilRightExitVisibility2
        {
            get { return (Visibility)GetValue(ASUCoilRightExitVisibility2Property); }
            set { SetValue(ASUCoilRightExitVisibility2Property, value); }
        }
        public static DependencyProperty ASUCoilRightExitVisibility2Property = DependencyProperty.Register("ASUCoilRightExitVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода вторичной обмотки."), Browsable(true)]
        public bool ASUCoilBottomExitIsExist2
        {
            get { return (bool)GetValue(ASUCoilBottomExitIsExist2Property); }
            set { SetValue(ASUCoilBottomExitIsExist2Property, value); }
        }
        public static DependencyProperty ASUCoilBottomExitIsExist2Property = DependencyProperty.Register("ASUCoilBottomExitIsExist2", typeof(bool), typeof(CTransformer2Coils), new PropertyMetadata(false, OnASUCoilBottomExitIsExist2Changed));
        private static void OnASUCoilBottomExitIsExist2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer2Coils ctc = d as CTransformer2Coils;
            ctc.ASUCoilBottomExitVisibility2 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода вторичной обмотки."), Browsable(false)]
        public Visibility ASUCoilBottomExitVisibility2
        {
            get { return (Visibility)GetValue(ASUCoilBottomExitVisibility2Property); }
            set { SetValue(ASUCoilBottomExitVisibility2Property, value); }
        }
        public static DependencyProperty ASUCoilBottomExitVisibility2Property = DependencyProperty.Register("ASUCoilBottomExitVisibility2", typeof(Visibility), typeof(CTransformer2Coils), new PropertyMetadata(Visibility.Collapsed));


        //=======================================================================
        //=======================================================================
        //[Category("Свойства элемента мнемосхемы"), Description("Положение вывода вторичной обмотки."), Browsable(true)]
        //public ASUCoilExitPositions ASUCoilExitPosition2
        //{
        //    get { return (ASUCoilExitPositions)GetValue(ASUCoilExitPosition2Property); }
        //    set { SetValue(ASUCoilExitPosition2Property, value); }
        //}
        //public static DependencyProperty ASUCoilExitPosition2Property = DependencyProperty.Register("ASUCoilExitPosition2", typeof(ASUCoilExitPositions), typeof(CTransformer2Coils), new PropertyMetadata(ASUCoilExitPositions.No, OnASUCoilExitPosition2PropertyChanged));
        //private static void OnASUCoilExitPosition2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    #region положение вывода обмотки
        //    CTransformer2Coils cis = d as CTransformer2Coils;
        //    cis.ASUCoilExitPosition2 = (ASUCoilExitPositions)e.NewValue;
        //    switch (cis.ASUCoilExitPosition2)
        //    {
        //        case ASUCoilExitPositions.Left:
        //            cis.ASUCoilLeftExitVisibility2 = Visibility.Visible;
        //            cis.ASUCoilTopExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility2 = Visibility.Collapsed;
        //            break;

        //        case ASUCoilExitPositions.Top:
        //            cis.ASUCoilLeftExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility2 = Visibility.Visible;
        //            cis.ASUCoilRightExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility2 = Visibility.Collapsed;
        //            break;
                    
        //        case ASUCoilExitPositions.Right:
        //            cis.ASUCoilLeftExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility2 = Visibility.Visible;
        //            cis.ASUCoilBottomExitVisibility2 = Visibility.Collapsed;
        //            break;
        //        case ASUCoilExitPositions.Bottom:
        //            cis.ASUCoilLeftExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility2 = Visibility.Visible;
        //            break;

        //        default://нет
        //            cis.ASUCoilLeftExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility2 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility2 = Visibility.Collapsed;
        //            break;
        //    }
        //    #endregion положение вывода обмотки
        //}

        //=======================================================================
        //=======================================================================

        public CTransformer2Coils()
        {
            this.DefaultStyleKey = typeof(CTransformer2Coils);
        }
    }
}
