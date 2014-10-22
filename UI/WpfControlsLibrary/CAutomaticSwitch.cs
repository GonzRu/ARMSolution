using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SilverlightControlsLibrary
{
    [Description("Автоматический выключатель")]
    public class CAutomaticSwitch : CBaseControl
    {
        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителей"), Browsable(true)]
        public bool ASUConnectorIsExist
        {
            get { return (bool)GetValue(ASUConnectorIsExistProperty); }
            set { SetValue(ASUConnectorIsExistProperty, value); }
        }
        public static DependencyProperty ASUConnectorIsExistProperty = DependencyProperty.Register("ASUConnectorIsExist", typeof(bool), typeof(CAutomaticSwitch), new PropertyMetadata(true, OnASUConnectorIsExistChanged));
        private static void OnASUConnectorIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CAutomaticSwitch ctc = d as CAutomaticSwitch;
            ctc.ASUConnectorVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителей"), Browsable(false)]
        public Visibility ASUConnectorVisibility
        {
            get { return (Visibility)GetValue(ASUConnectorVisibilityProperty); }
            set { SetValue(ASUConnectorVisibilityProperty, value); }
        }
        public static DependencyProperty ASUConnectorVisibilityProperty = DependencyProperty.Register("ASUConnectorVisibility", typeof(Visibility), typeof(CAutomaticSwitch), new PropertyMetadata(Visibility.Visible));

       
        //=======================================================================
        //=======================================================================
        public CAutomaticSwitch()
        {
            this.DefaultStyleKey = typeof(CAutomaticSwitch);
        }
        //=======================================================================
        //=======================================================================
       
    }


}



