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
    [Description("Реактор")]
    public class CReactor : CBaseControl
    {
        [Category("Свойства элемента мнемосхемы"), Description("Наличие регулировки."), Browsable(true)]
        public bool ASURegulatorIsExist
        {
            get { return (bool)GetValue(ASURegulatorIsExistProperty); }
            set { SetValue(ASURegulatorIsExistProperty, value); }
        }
        public static DependencyProperty ASURegulatorIsExistProperty = DependencyProperty.Register("ASURegulatorIsExist", typeof(bool), typeof(CReactor), new PropertyMetadata(false, OnASURegulatorIsExistChanged));
        private static void OnASURegulatorIsExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CReactor ctc = d as CReactor;
            ctc.ASURegulatorVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Наличие регулировки."), Browsable(false)]
        public Visibility ASURegulatorVisibility
        {
            get { return (Visibility)GetValue(ASURegulatorVisibilityProperty); }
            set { SetValue(ASURegulatorVisibilityProperty, value); }
        }
        public static DependencyProperty ASURegulatorVisibilityProperty = DependencyProperty.Register("ASURegulatorVisibility", typeof(Visibility), typeof(CReactor), new PropertyMetadata(Visibility.Collapsed));


        public CReactor()
        {
            this.DefaultStyleKey = typeof(CReactor);
        }
    }
}
