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
     [Description("Предохранитель")]
    public class CFuse : CBaseControl
    {
        [Category("Свойства элемента мнемосхемы"), Description("Видимость левого соединителя."), Browsable(true)]
        public bool ASUConnectorLeftIsExist
        {
            get { return (bool)GetValue(ASUConnectorLeftIsExistProperty); }
            set { SetValue(ASUConnectorLeftIsExistProperty, value); }
        }
        public static DependencyProperty ASUConnectorLeftIsExistProperty = DependencyProperty.Register("ASUConnectorLeftIsExist", typeof(bool), typeof(CFuse), new PropertyMetadata(true, OnASUConnectorLeftIsExistChanged));
        private static void OnASUConnectorLeftIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CFuse ctc = d as CFuse;
            ctc.ASUConnectorLeftVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителей."), Browsable(false)]
        public Visibility ASUConnectorLeftVisibility
        {
            get { return (Visibility)GetValue(ASUConnectorLeftVisibilityProperty); }
            set { SetValue(ASUConnectorLeftVisibilityProperty, value); }
        }
        public static DependencyProperty ASUConnectorLeftVisibilityProperty = DependencyProperty.Register("ASUConnectorLeftVisibility", typeof(Visibility), typeof(CFuse), new PropertyMetadata(Visibility.Visible));
        //===========================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость правого соединителя."), Browsable(true)]
        public bool ASUConnectorRightIsExist
        {
            get { return (bool)GetValue(ASUConnectorRightIsExistProperty); }
            set { SetValue(ASUConnectorRightIsExistProperty, value); }
        }
        public static DependencyProperty ASUConnectorRightIsExistProperty = DependencyProperty.Register("ASUConnectorRightIsExist", typeof(bool), typeof(CFuse), new PropertyMetadata(true, OnASUConnectorRightIsExistChanged));
        private static void OnASUConnectorRightIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CFuse ctc = d as CFuse;
            ctc.ASUConnectorRightVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителей."), Browsable(false)]
        public Visibility ASUConnectorRightVisibility
        {
            get { return (Visibility)GetValue(ASUConnectorRightVisibilityProperty); }
            set { SetValue(ASUConnectorRightVisibilityProperty, value); }
        }
        public static DependencyProperty ASUConnectorRightVisibilityProperty = DependencyProperty.Register("ASUConnectorRightVisibility", typeof(Visibility), typeof(CFuse), new PropertyMetadata(Visibility.Visible));
              

        public CFuse()
        {
            this.DefaultStyleKey = typeof(CFuse);
        }
    }
}
