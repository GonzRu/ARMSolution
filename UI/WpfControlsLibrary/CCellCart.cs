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
     [Description("Выкатной элемент ячейки")]
    public class CCellCart : CBaseControl
    {
       
        //[Category("Свойства элемента мнемосхемы"), Description("Состояние тележки."), Browsable(true)]
        //public ASUCellCartStates ASUCellCartState
        //{
        //    get { return (ASUCellCartStates)GetValue(ASUCellCartStateProperty); }
        //    set { SetValue(ASUCellCartStateProperty, value); }
        //}
        //public static DependencyProperty ASUCellCartStateProperty = DependencyProperty.Register("ASUCellCartState", typeof(ASUCellCartStates), typeof(CCellCart), new PropertyMetadata(ASUCellCartStates.On, OnASUCellCartStateChanged));
        //private static void OnASUCellCartStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CCellCart ctc = d as CCellCart;
        //    ctc.ASUCellCartState = (ASUCellCartStates)e.NewValue;
        //    #region Установка состояния тележки
        //    switch (ctc.ASUCellCartState)
        //    {
        //        case ASUCellCartStates.Off:
        //            ctc.ASUCellCartStateOnVisibility = Visibility.Collapsed;
        //            ctc.ASUCellCartStateOffVisibility = Visibility.Visible;
        //            ctc.ASUCellCartStateOutVisibility = Visibility.Collapsed;
        //            break;
        //        case ASUCellCartStates.On:
        //            ctc.ASUCellCartStateOnVisibility = Visibility.Visible;
        //            ctc.ASUCellCartStateOffVisibility = Visibility.Collapsed;
        //            ctc.ASUCellCartStateOutVisibility = Visibility.Collapsed;
        //            break;
        //        default:// ASUCellCartStates.OutOfWork
        //            ctc.ASUCellCartStateOnVisibility = Visibility.Collapsed;
        //            ctc.ASUCellCartStateOffVisibility = Visibility.Collapsed;
        //            ctc.ASUCellCartStateOutVisibility = Visibility.Visible;
        //            break;
        //    }
        //    #endregion Установка состояния тележки
        //}


        //[Category("Свойства элемента мнемосхемы"), Description("Тележка в рабочем положении."), Browsable(false)]
        //public Visibility ASUCellCartStateOnVisibility
        //{
        //    get { return (Visibility)GetValue(ASUCellCartStateOnVisibilityProperty); }
        //    set { SetValue(ASUCellCartStateOnVisibilityProperty, value); }
        //}
        //public static DependencyProperty ASUCellCartStateOnVisibilityProperty = DependencyProperty.Register("ASUCellCartStateOnVisibility", typeof(Visibility), typeof(CCellCart), new PropertyMetadata(Visibility.Visible));

        //[Category("Свойства элемента мнемосхемы"), Description("Тележка в контрольном положении."), Browsable(false)]
        //public Visibility ASUCellCartStateOffVisibility
        //{
        //    get { return (Visibility)GetValue(ASUCellCartStateOffVisibilityProperty); }
        //    set { SetValue(ASUCellCartStateOffVisibilityProperty, value); }
        //}
        //public static DependencyProperty ASUCellCartStateOffVisibilityProperty = DependencyProperty.Register("ASUCellCartStateOffVisibility", typeof(Visibility), typeof(CCellCart), new PropertyMetadata(Visibility.Visible));

        //[Category("Свойства элемента мнемосхемы"), Description("Тележка в контрольном положении."), Browsable(false)]
        //public Visibility ASUCellCartStateOutVisibility
        //{
        //    get { return (Visibility)GetValue(ASUCellCartStateOutVisibilityProperty); }
        //    set { SetValue(ASUCellCartStateOutVisibilityProperty, value); }
        //}
        //public static DependencyProperty ASUCellCartStateOutVisibilityProperty = DependencyProperty.Register("ASUCellCartStateOutVisibility", typeof(Visibility), typeof(CCellCart), new PropertyMetadata(Visibility.Visible));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего соединителя."), Browsable(true)]
        public bool ASUConnectorIsExist
        {
            get { return (bool)GetValue(ASUConnectorIsExistProperty); }
            set { SetValue(ASUConnectorIsExistProperty, value); }
        }
        public static DependencyProperty ASUConnectorIsExistProperty = DependencyProperty.Register("ASUConnectorIsExist", typeof(bool), typeof(CCellCart), new PropertyMetadata(true, OnASUConnectorIsExistChanged));
        private static void OnASUConnectorIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CCellCart ctc = d as CCellCart;
            ctc.ASUConnectorVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость верхнего соединителя."), Browsable(false)]
        public Visibility ASUConnectorVisibility
        {
            get { return (Visibility)GetValue(ASUConnectorVisibilityProperty); }
            set { SetValue(ASUConnectorVisibilityProperty, value); }
        }
        public static DependencyProperty ASUConnectorVisibilityProperty = DependencyProperty.Register("ASUConnectorVisibility", typeof(Visibility), typeof(CCellCart), new PropertyMetadata(Visibility.Visible));
        

        public CCellCart()
        {
            this.DefaultStyleKey = typeof(CCellCart);
        }
    }
}
