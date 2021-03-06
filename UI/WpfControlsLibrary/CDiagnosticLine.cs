﻿using System;
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
using System.Windows.Data;

namespace SilverlightControlsLibrary
{
     [Description("Линия")]
    public class CDiagnosticLine : CDiagnosticDevice
    {
        [Category("Свойства элемента мнемосхемы"), Description("X-координата конца линии."), Browsable(true)]
        public double ASUCoordinateX2
        {
            get { return (double)GetValue(ASUCoordinateX2Property); }
            set { SetValue(ASUCoordinateX2Property, value); }
        }
        public static readonly DependencyProperty ASUCoordinateX2Property = DependencyProperty.Register("ASUCoordinateX2", typeof(double), typeof(CDiagnosticLine), new PropertyMetadata(0.0));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Y-координата конца линии."), Browsable(true)]
        public double ASUCoordinateY2
        {
            get { return (double)GetValue(ASUCoordinateY2Property); }
            set { SetValue(ASUCoordinateY2Property, value); }
        }
        public static readonly DependencyProperty ASUCoordinateY2Property = DependencyProperty.Register("ASUCoordinateY2", typeof(double), typeof(CDiagnosticLine), new PropertyMetadata(0.0));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Толщина линии."), Browsable(true)]
        public double ASULineThickness
        {
            get { return (double)GetValue(ASULineThicknessProperty); }
            set { SetValue(ASULineThicknessProperty, value); }
        }
        public static readonly DependencyProperty ASULineThicknessProperty = DependencyProperty.Register("ASULineThickness", typeof(double), typeof(CDiagnosticLine), new PropertyMetadata(1.0));// { AffectsRender = true });


        public CDiagnosticLine()
        {
            this.DefaultStyleKey = typeof(CDiagnosticLine);
        }
        
    }

   
}
