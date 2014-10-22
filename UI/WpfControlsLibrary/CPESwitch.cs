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
     [Description("Заземляющий нож")]
    public class CPESwitch : CBaseControl
    {
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Номер переносного заземления."), Browsable(true)]
        public string ASUManualPENumber
        {
            get { return (string)GetValue(ASUManualPENumberProperty); }
            set { SetValue(ASUManualPENumberProperty, value); }
        }
        public static DependencyProperty ASUManualPENumberProperty = DependencyProperty.Register("ASUManualPENumber", typeof(string), typeof(CPESwitch), new PropertyMetadata("№1"));
        //=======================================================================


        public CPESwitch()
        {
            this.DefaultStyleKey = typeof(CPESwitch);
        }
    }
}
