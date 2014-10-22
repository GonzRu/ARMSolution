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
    [Description("Замок - блокировка при местном управлении")]
    public class CBlockCommonAccess : CBlock 
    {
       
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость текста состояния."), Browsable(false)]
        public Visibility ASUContentVisibility
        {
            get { return (Visibility)GetValue(ASUContentVisibilityProperty); }
            set { SetValue(ASUContentVisibilityProperty, value); }
        }
        public static DependencyProperty ASUContentVisibilityProperty = DependencyProperty.Register("ASUContentVisibility", typeof(Visibility), typeof(CBlockCommonAccess), new PropertyMetadata(Visibility.Collapsed));

        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость 'замочка'."), Browsable(false)]
        public Visibility ASULockerVisibility
        {
            get { return (Visibility)GetValue(ASULockerVisibilityProperty); }
            set { SetValue(ASULockerVisibilityProperty, value); }
        }
        public static DependencyProperty ASULockerVisibilityProperty = DependencyProperty.Register("ASULockerVisibility", typeof(Visibility), typeof(CBlockCommonAccess), new PropertyMetadata(Visibility.Visible));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость 'замочка'."), Browsable(true)]
        public bool ASULockerIsVisible
        {
            get { return (bool)GetValue(ASULockerIsVisibleProperty); }
            set { SetValue(ASULockerIsVisibleProperty, value); }
        }
        public static DependencyProperty ASULockerIsVisibleProperty = DependencyProperty.Register("ASULockerIsVisible", typeof(bool), typeof(CBlockCommonAccess), new PropertyMetadata(true, OnASULockerIsVisibleChanged));
        private static void OnASULockerIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBlockCommonAccess cis = d as CBlockCommonAccess;
            cis.ASULockerVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        //==============================================
        

        public CBlockCommonAccess()
        {
            this.DefaultStyleKey = typeof( CBlockCommonAccess);            
        }
                   

    }


}



