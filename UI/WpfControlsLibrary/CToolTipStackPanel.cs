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
    public class CToolTipStackPanel : StackPanel
    {        
        //=======================================================================
        public string ASUTagIDState
        {
            get { return (string)GetValue(ASUTagIDStateProperty); }
            set { SetValue(ASUTagIDStateProperty, value); }
        }
        public static DependencyProperty ASUTagIDStateProperty = DependencyProperty.Register("ASUTagIDState", typeof(string), typeof(CToolTipStackPanel), new PropertyMetadata("-1"));
        //=======================================================================

        public CToolTipStackPanel()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CToolTip), new FrameworkPropertyMetadata(typeof(CToolTip)));
            //this.DefaultStyleKey = typeof(CToolTip);
        }

    }
}
