using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightControlsLibrary
{
    [Description("Область ссылка")]
    public class CMnemoLinkArea : CBaseControl
    {
        [Category("Свойства элемента мнемосхемы"), Description("X-координата конца линии."), Browsable(true)]
        public double ASUCoordinateX2
        {
            get { return (double)GetValue(ASUCoordinateX2Property); }
            set { SetValue(ASUCoordinateX2Property, value); }
        }
        public static readonly DependencyProperty ASUCoordinateX2Property = DependencyProperty.Register("ASUCoordinateX2", typeof(double), typeof(CMnemoLinkArea), new PropertyMetadata(0.0));// { AffectsRender = true });


        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Кисть элемента."), Browsable(false)]
        public SolidColorBrush ASUBorderColorBrush
        {
            get { return (SolidColorBrush)GetValue(ASUBorderColorBrushProperty); }
            set { SetValue(ASUBorderColorBrushProperty, value); }
        }
        public static DependencyProperty ASUBorderColorBrushProperty = DependencyProperty.Register("ASUBorderColorBrush", typeof(SolidColorBrush), typeof(CMnemoLinkArea), new PropertyMetadata(new SolidColorBrush(Colors.Green)));//, OnASUTextColorBrushChanged));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет элемента."), Browsable(true)]
        public Color ASUElementColor
        {
            get { return (Color)GetValue(ASUElementColorProperty); }
            set { SetValue(ASUElementColorProperty, value); }
        }
        public static DependencyProperty ASUElementColorProperty = DependencyProperty.Register("ASUElementColor", typeof(Color), typeof(CMnemoLinkArea), new PropertyMetadata(Colors.Green, ASUElementColorPropertyChanged));
        private static void ASUElementColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CMnemoLinkArea dev = d as CMnemoLinkArea;
            dev.ASUBorderColorBrush = new SolidColorBrush((Color)e.NewValue);
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Y-координата конца линии."), Browsable(true)]
        public double ASUCoordinateY2
        {
            get { return (double)GetValue(ASUCoordinateY2Property); }
            set { SetValue(ASUCoordinateY2Property, value); }
        }
        public static readonly DependencyProperty ASUCoordinateY2Property = DependencyProperty.Register("ASUCoordinateY2", typeof(double), typeof(CMnemoLinkArea), new PropertyMetadata(0.0));// { AffectsRender = true });
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Толщина линии."), Browsable(true)]
        public double ASULineThickness
        {
            get { return (double)GetValue(ASULineThicknessProperty); }
            set { SetValue(ASULineThicknessProperty, value); }
        }
        public static readonly DependencyProperty ASULineThicknessProperty = DependencyProperty.Register("ASULineThickness", typeof(double), typeof(CMnemoLinkArea), new PropertyMetadata(1.0));// { AffectsRender = true });
        //==============================================
        [Category("Навигация"), Description("Имя файла"), Browsable(true)]
        public string ASUMnemoLinkFileName
        {
            get { return (string)GetValue(ASUMnemoLinkFileNameProperty); }
            set
            {
                SetValue(ASUMnemoLinkFileNameProperty, value);
                CommandParameter = value;
            }
        }
        public static readonly DependencyProperty ASUMnemoLinkFileNameProperty = DependencyProperty.Register("ASUMnemoLinkFileName", typeof(string), typeof(CMnemoLinkArea), new PropertyMetadata(String.Empty));// { AffectsRender = true });
        //==============================================
        [Category("Навигация"), Description("Название вложенной мнемосхемы"), Browsable(true)]
        public string ASUMnemoLinkName
        {
            get { return (string)GetValue(ASUMnemoLinkNameProperty); }
            set
            {
                SetValue(ASUMnemoLinkNameProperty, value);
            }
        }
        public static readonly DependencyProperty ASUMnemoLinkNameProperty = DependencyProperty.Register("ASUMnemoLinkName", typeof(string), typeof(CMnemoLinkArea), new PropertyMetadata("Вложенная мнемосхема"));// { AffectsRender = true });
        //==============================================
        //==============================================
        public CMnemoLinkArea()
        {
            this.DefaultStyleKey = typeof(CMnemoLinkArea);
        }
    }
}
