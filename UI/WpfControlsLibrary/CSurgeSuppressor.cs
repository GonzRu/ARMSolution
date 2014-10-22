using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightControlsLibrary
{
     [Description("Ограничитель перенапряжений")]
    public class CSurgeSuppressor : CBaseControl
    {
        public CSurgeSuppressor()
        {
            this.DefaultStyleKey = typeof(CSurgeSuppressor);
        }
    }
}
