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
    public class CToolTipSwitch : CheckBox
    {
        //==============================================
        //public bool? IsPassed
        //{
        //    get { return (bool?)GetValue(IsPassedProperty); }
        //    set { SetValue(IsPassedProperty, value); }
        //}
        //public static DependencyProperty IsPassedProperty = DependencyProperty.Register("IsPassed", typeof(bool?), typeof(CToolTip), new PropertyMetadata(false, OnIsPassedPropertyChanged));
        //private static void OnIsPassedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CToolTip tt = d as CToolTip;
        //    tt.IsChecked = (bool?)e.NewValue;
        //}
        //=======================================================================
        public string ASUTagIDState
        {
            get { return (string)GetValue(ASUTagIDStateProperty); }
            set { SetValue(ASUTagIDStateProperty, value); }
        }
        public static DependencyProperty ASUTagIDStateProperty = DependencyProperty.Register("ASUTagIDState", typeof(string), typeof(CToolTipSwitch), new PropertyMetadata("-1"));
        //=======================================================================

        public string ASUCommonKAID
        {
            get { return (string)GetValue(ASUCommonKAIDProperty); }
            set { SetValue(ASUCommonKAIDProperty, value); }
        }
        public static DependencyProperty ASUCommonKAIDProperty = DependencyProperty.Register("ASUCommonKAID", typeof(string), typeof(CToolTipSwitch), new PropertyMetadata("-1"));
        //=======================================================================
         

        public CToolTipSwitch()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CToolTip), new FrameworkPropertyMetadata(typeof(CToolTip)));
            this.DefaultStyleKey = typeof(CToolTipSwitch);
            this.IsChecked = null;
        }

    }
}
