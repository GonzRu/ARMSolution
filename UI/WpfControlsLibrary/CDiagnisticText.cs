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
     [Description("Текстовый элемент")]
    public class CDiagnosticText : CDiagnosticDevice
    {
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Угол поворота элемента."), Browsable(true)]
        public double ASUAngle
        {
            get { return (double)GetValue(ASUAngleProperty); }
            set { SetValue(ASUAngleProperty, value); }
        }
        public static readonly DependencyProperty ASUAngleProperty = DependencyProperty.Register("ASUAngle", typeof(double), typeof(CDiagnosticText), new PropertyMetadata(0.0));// { AffectsRender = true });
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Ширина текстового поля."), Browsable(true)]
        public double ASUTextContentWidth
        {
            get { return (double)GetValue(ASUTextContentWidthProperty); }
            set { SetValue(ASUTextContentWidthProperty, value); }
        }
        public static DependencyProperty ASUTextContentWidthProperty = DependencyProperty.Register("ASUTextContentWidth", typeof(double), typeof(CDiagnosticText), new PropertyMetadata((double)90));
        
        //=======================================================================

        public CDiagnosticText()
        {
            this.DefaultStyleKey = typeof(CDiagnosticText);
        }
    }
}
