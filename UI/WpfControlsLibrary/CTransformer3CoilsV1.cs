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
    [Description("Трехобмоточный трансформатор Версия 1 - Првый")]
    public class CTransformer3CoilsV1 : CTransformer2Coils
    {
        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Тип соединения третьих обмоток трансформатора."), Browsable(true)]
        public ASUCoilsConnectionTypes ASUCoilsConnectionType3
        {
            get { return (ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionType3Property); }
            set { SetValue(ASUCoilsConnectionType3Property, value); }
        }
        public static DependencyProperty ASUCoilsConnectionType3Property = DependencyProperty.Register("ASUCoilsConnectionType3", typeof(ASUCoilsConnectionTypes), typeof(CTransformer3CoilsV1), new PropertyMetadata(ASUCoilsConnectionTypes.DeltaConnection, OnASUCoilsConnectionType3Changed));
        private static void OnASUCoilsConnectionType3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            ctc.ASUCoilsConnectionType3 = (ASUCoilsConnectionTypes)e.NewValue;
            #region Установка видимости типов соединения
            switch (ctc.ASUCoilsConnectionType3)
            {
                case ASUCoilsConnectionTypes.DeltaConnection:
                    ctc.ASUDeltaConnectionVisibility3 = Visibility.Visible;
                    ctc.ASUVConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility3 = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.VConnection:
                    ctc.ASUDeltaConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility3 = Visibility.Visible;
                    ctc.ASUStarConnectionVisibility3 = Visibility.Collapsed;
                    break;
                case ASUCoilsConnectionTypes.StarConnection:
                    ctc.ASUDeltaConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility3 = Visibility.Visible;
                    break;
                default:// ASUCoilsConnectionTypes.None
                    ctc.ASUDeltaConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUVConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUStarConnectionVisibility3 = Visibility.Collapsed;
                    break;
            }
            #endregion Установка видимости типов соединения
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость треугольника 3."), Browsable(false)]
        public Visibility ASUDeltaConnectionVisibility3
        {
            get { return (Visibility)GetValue(ASUDeltaConnectionVisibility3Property); }
            set { SetValue(ASUDeltaConnectionVisibility3Property, value); }
        }
        public static DependencyProperty ASUDeltaConnectionVisibility3Property = DependencyProperty.Register("ASUDeltaConnectionVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Visible));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость разомкнутого треугольника 3."), Browsable(false)]
        public Visibility ASUVConnectionVisibility3
        {
            get { return (Visibility)GetValue(ASUVConnectionVisibility3Property); }
            set { SetValue(ASUVConnectionVisibility3Property, value); }
        }
        public static DependencyProperty ASUVConnectionVisibility3Property = DependencyProperty.Register("ASUVConnectionVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость звезды3."), Browsable(false)]
        public Visibility ASUStarConnectionVisibility3
        {
            get { return (Visibility)GetValue(ASUStarConnectionVisibility3Property); }
            set { SetValue(ASUStarConnectionVisibility3Property, value); }
        }
        public static DependencyProperty ASUStarConnectionVisibility3Property = DependencyProperty.Register("ASUStarConnectionVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Наличие вывода заземления третьей обмотки."), Browsable(true)]
        public bool ASUPEConnectionIsExist3
        {
            get { return (bool)GetValue(ASUPEConnectionIsExist3Property); }
            set
            {
                if (!value)
                    SetValue(ASUPEConnectionIsExist3Property, value);
                else
                {
                    if ((ASUCoilsConnectionTypes)GetValue(ASUCoilsConnectionType3Property) == ASUCoilsConnectionTypes.StarConnection)
                        SetValue(ASUPEConnectionIsExist3Property, value);
                    else
                    {
                        SetValue(ASUPEConnectionIsExist3Property, !value);
                        //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                    }
                }

            }
        }
        public static DependencyProperty ASUPEConnectionIsExist3Property = DependencyProperty.Register("ASUPEConnectionIsExist3", typeof(bool), typeof(CTransformer3CoilsV1), new PropertyMetadata(false, OnASUPEConnectionIsExist3Changed));
        private static void OnASUPEConnectionIsExist3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Что происходит раньше?
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            if (!(bool)e.NewValue)
                ctc.ASUPEConnectionVisibility3 = Visibility.Collapsed;
            else
            {
                if (ctc.ASUCoilsConnectionType3 == ASUCoilsConnectionTypes.StarConnection)
                    ctc.ASUPEConnectionVisibility3 = Visibility.Visible;
                else
                {
                    ctc.ASUPEConnectionVisibility3 = Visibility.Collapsed;
                    ctc.ASUPEConnectionIsExist3 = false;
                    //MessageBox.Show("Заземлить нейтраль можно только если обмотки соединены звездой.", "Внимание", MessageBoxButton.OK);
                }
            }

        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость вывода третьей обмотки."), Browsable(false)]
        public Visibility ASUPEConnectionVisibility3
        {
            get { return (Visibility)GetValue(ASUPEConnectionVisibility3Property); }
            set { SetValue(ASUPEConnectionVisibility3Property, value); }
        }
        public static DependencyProperty ASUPEConnectionVisibility3Property = DependencyProperty.Register("ASUPEConnectionVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Цвет третьей обмотки в соответствии с классом напряжения."), Browsable(false)]
        public SolidColorBrush ASUVoltageColor3
        {
            get { return (SolidColorBrush)GetValue(ASUVoltageColor3Property); }
            set { SetValue(ASUVoltageColor3Property, value); }
        }
        public static DependencyProperty ASUVoltageColor3Property = DependencyProperty.Register("ASUVoltageColor3", typeof(SolidColorBrush), typeof(CTransformer3CoilsV1), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 180, 200))));


        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения третьей обмотки."), Browsable(true)]
        public ASUCommutationDeviceVoltageClasses ASUVoltage3
        {
            get { return (ASUCommutationDeviceVoltageClasses)GetValue(ASUVoltage3Property); }
            set { SetValue(ASUVoltage3Property, value); }
        }
        public static DependencyProperty ASUVoltage3Property = DependencyProperty.Register("ASUVoltage3", typeof(ASUCommutationDeviceVoltageClasses), typeof(CTransformer3CoilsV1), new PropertyMetadata(ASUCommutationDeviceVoltageClasses.kV110, OnASUVoltage3PropertyChanged));
        private static void OnASUVoltage3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region класс напряжения третьей обмотки
            CTransformer3CoilsV1 cis = d as CTransformer3CoilsV1;
            cis.ASUVoltage3 = (ASUCommutationDeviceVoltageClasses)e.NewValue;
            switch (cis.ASUVoltage3)
            {
                case ASUCommutationDeviceVoltageClasses.kV1150:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 205, 138, 255));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV750:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 0, 0, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV500:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 165, 15, 10));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV400:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 240, 150, 30));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV330:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 0, 140, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV220:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 200, 200, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV150:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 170, 150, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV110:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV35:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 130, 100, 50));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV10:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV6:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 200, 150, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV04:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVGenerator:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 230, 70, 230));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVRepair:
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 205, 255, 155));
                    break;
                default://ASUCommutationDeviceVoltageClasses.kVEmpty
                    cis.ASUVoltageColor3 = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;

            }
            #endregion класс напряжения третьей обмотки
        }

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода третьей обмотки."), Browsable(true)]
        public bool ASUCoilLeftExitIsExist3
        {
            get { return (bool)GetValue(ASUCoilLeftExitIsExist3Property); }
            set { SetValue(ASUCoilLeftExitIsExist3Property, value); }
        }
        public static DependencyProperty ASUCoilLeftExitIsExist3Property = DependencyProperty.Register("ASUCoilLeftExitIsExist3", typeof(bool), typeof(CTransformer3CoilsV1), new PropertyMetadata(false, OnASUCoilLeftExitIsExist3Changed));
        private static void OnASUCoilLeftExitIsExist3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            ctc.ASUCoilLeftExitVisibility3 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого вывода третьей обмотки."), Browsable(false)]
        public Visibility ASUCoilLeftExitVisibility3
        {
            get { return (Visibility)GetValue(ASUCoilLeftExitVisibility3Property); }
            set { SetValue(ASUCoilLeftExitVisibility3Property, value); }
        }
        public static DependencyProperty ASUCoilLeftExitVisibility3Property = DependencyProperty.Register("ASUCoilLeftExitVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода третьей обмотки."), Browsable(true)]
        public bool ASUCoilTopExitIsExist3
        {
            get { return (bool)GetValue(ASUCoilTopExitIsExist3Property); }
            set { SetValue(ASUCoilTopExitIsExist3Property, value); }
        }
        public static DependencyProperty ASUCoilTopExitIsExist3Property = DependencyProperty.Register("ASUCoilTopExitIsExist3", typeof(bool), typeof(CTransformer3CoilsV1), new PropertyMetadata(false, OnASUCoilTopExitIsExist3Changed));
        private static void OnASUCoilTopExitIsExist3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            ctc.ASUCoilTopExitVisibility3 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего вывода третьей обмотки."), Browsable(false)]
        public Visibility ASUCoilTopExitVisibility3
        {
            get { return (Visibility)GetValue(ASUCoilTopExitVisibility3Property); }
            set { SetValue(ASUCoilTopExitVisibility3Property, value); }
        }
        public static DependencyProperty ASUCoilTopExitVisibility3Property = DependencyProperty.Register("ASUCoilTopExitVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода третьей обмотки."), Browsable(true)]
        public bool ASUCoilRightExitIsExist3
        {
            get { return (bool)GetValue(ASUCoilRightExitIsExist3Property); }
            set { SetValue(ASUCoilRightExitIsExist3Property, value); }
        }
        public static DependencyProperty ASUCoilRightExitIsExist3Property = DependencyProperty.Register("ASUCoilRightExitIsExist3", typeof(bool), typeof(CTransformer3CoilsV1), new PropertyMetadata(false, OnASUCoilRightExitIsExist3Changed));
        private static void OnASUCoilRightExitIsExist3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            ctc.ASUCoilRightExitVisibility3 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого вывода третьей обмотки."), Browsable(false)]
        public Visibility ASUCoilRightExitVisibility3
        {
            get { return (Visibility)GetValue(ASUCoilRightExitVisibility3Property); }
            set { SetValue(ASUCoilRightExitVisibility3Property, value); }
        }
        public static DependencyProperty ASUCoilRightExitVisibility3Property = DependencyProperty.Register("ASUCoilRightExitVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));
        //===============================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода третьей обмотки."), Browsable(true)]
        public bool ASUCoilBottomExitIsExist3
        {
            get { return (bool)GetValue(ASUCoilBottomExitIsExist3Property); }
            set { SetValue(ASUCoilBottomExitIsExist3Property, value); }
        }
        public static DependencyProperty ASUCoilBottomExitIsExist3Property = DependencyProperty.Register("ASUCoilBottomExitIsExist3", typeof(bool), typeof(CTransformer3CoilsV1), new PropertyMetadata(false, OnASUCoilBottomExitIsExist3Changed));
        private static void OnASUCoilBottomExitIsExist3Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CTransformer3CoilsV1 ctc = d as CTransformer3CoilsV1;
            ctc.ASUCoilBottomExitVisibility3 = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        [Category("Свойства элемента мнемосхемы"), Description("Видимость нижнего вывода третьей обмотки."), Browsable(false)]
        public Visibility ASUCoilBottomExitVisibility3
        {
            get { return (Visibility)GetValue(ASUCoilBottomExitVisibility3Property); }
            set { SetValue(ASUCoilBottomExitVisibility3Property, value); }
        }
        public static DependencyProperty ASUCoilBottomExitVisibility3Property = DependencyProperty.Register("ASUCoilBottomExitVisibility3", typeof(Visibility), typeof(CTransformer3CoilsV1), new PropertyMetadata(Visibility.Collapsed));


        //=======================================================================
        //=======================================================================
        //[Category("Свойства элемента мнемосхемы"), Description("Положение вывода третьей обмотки."), Browsable(true)]
        //public ASUCoilExitPositions ASUCoilExitPosition3
        //{
        //    get { return (ASUCoilExitPositions)GetValue(ASUCoilExitPosition3Property); }
        //    set { SetValue(ASUCoilExitPosition3Property, value); }
        //}
        //public static DependencyProperty ASUCoilExitPosition3Property = DependencyProperty.Register("ASUCoilExitPosition3", typeof(ASUCoilExitPositions), typeof(CTransformer3CoilsV1), new PropertyMetadata(ASUCoilExitPositions.No, OnASUCoilExitPosition3PropertyChanged));
        //private static void OnASUCoilExitPosition3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    #region положение вывода обмотки
        //    CTransformer3CoilsV1 cis = d as CTransformer3CoilsV1;
        //    cis.ASUCoilExitPosition3 = (ASUCoilExitPositions)e.NewValue;
        //    switch (cis.ASUCoilExitPosition3)
        //    {
        //        case ASUCoilExitPositions.Left:
        //            cis.ASUCoilLeftExitVisibility3 = Visibility.Visible;
        //            cis.ASUCoilTopExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility3 = Visibility.Collapsed;
        //            break;

        //        case ASUCoilExitPositions.Top:
        //            cis.ASUCoilLeftExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility3 = Visibility.Visible;
        //            cis.ASUCoilRightExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility3 = Visibility.Collapsed;
        //            break;

        //        case ASUCoilExitPositions.Right:
        //            cis.ASUCoilLeftExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility3 = Visibility.Visible;
        //            cis.ASUCoilBottomExitVisibility3 = Visibility.Collapsed;
        //            break;
        //        case ASUCoilExitPositions.Bottom:
        //            cis.ASUCoilLeftExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility3 = Visibility.Visible;
        //            break;

        //        default://нет
        //            cis.ASUCoilLeftExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilTopExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilRightExitVisibility3 = Visibility.Collapsed;
        //            cis.ASUCoilBottomExitVisibility3 = Visibility.Collapsed;
        //            break;
        //    }
        //    #endregion положение вывода обмотки
        //}

        //=======================================================================
        //=======================================================================

        public CTransformer3CoilsV1()
        {
            this.DefaultStyleKey = typeof(CTransformer3CoilsV1);
        }
    }
}
