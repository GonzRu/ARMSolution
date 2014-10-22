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
     [Description("Элемент отображения текущих данных")]
    public class CCurrentDataDisplay : CBaseControl
    {
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Кисть текста значения тега."), Browsable(false)]
        public SolidColorBrush ASUTextValueColorBrush
        {
            get { return (SolidColorBrush)GetValue(ASUTextValueColorBrushProperty); }
            set { SetValue(ASUTextValueColorBrushProperty, value); }
        }
        public static DependencyProperty ASUTextValueColorBrushProperty = DependencyProperty.Register("ASUTextValueColorBrush", typeof(SolidColorBrush), typeof(CCurrentDataDisplay), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 255, 0))));
        
        [Category("Свойства элемента мнемосхемы"), Description("Цвет текста значения тега."), Browsable(true)]
        public Color ASUTextValueColor
        {
            get { return (Color)GetValue(ASUTextValueColorProperty); }
            set { SetValue(ASUTextValueColorProperty, value); }
        }
        public static DependencyProperty ASUTextValueColorProperty = DependencyProperty.Register("ASUTextValueColor", typeof(Color), typeof(CCurrentDataDisplay), new PropertyMetadata(Color.FromArgb(255, 255, 190, 0), OnASUTextValueColorChanged));
        private static void OnASUTextValueColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CCurrentDataDisplay cis = d as CCurrentDataDisplay;
            cis.ASUTextValueColorBrush = new SolidColorBrush((Color)e.NewValue);
        }
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Название тега."), Browsable(true)]
        public string ASUTagName
        {
            get { return (string)GetValue(ASUTagNameProperty); }
            set { SetValue(ASUTagNameProperty, value); }
        }
        public static readonly DependencyProperty ASUTagNameProperty = DependencyProperty.Register("ASUTagName", typeof(string), typeof(CCurrentDataDisplay), new PropertyMetadata("Название тега"));
        
        //=======================================================================
        //[Category("Свойства элемента мнемосхемы"), Description("ID тега."), Browsable(true)]
        //public string ASUTagID
        //{
        //    get { return (string)GetValue(ASUTagIDProperty); }
        //    set { SetValue(ASUTagIDProperty, value); }
        //}
        //public static readonly DependencyProperty ASUTagIDProperty = DependencyProperty.Register("ASUTagID", typeof(string), typeof(CCurrentDataDisplay), new PropertyMetadata("Идентификатор тега"));
        
        //=======================================================================               
        [Category("Свойства элемента мнемосхемы"), Description("Тип тега."), Browsable(false)]
        public string ASUTagType
        {
            get { return (string)GetValue(ASUTagTypeProperty); }
            set { SetValue(ASUTagTypeProperty, value); }
        }
        public static DependencyProperty ASUTagTypeProperty = DependencyProperty.Register("ASUTagType", typeof(string), typeof(CCurrentDataDisplay), new PropertyMetadata("String", OnASUTagTypeChanged));
        private static void OnASUTagTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CCurrentDataDisplay ctc = d as CCurrentDataDisplay;
            if (ctc.ASUTagType.Equals("Discret", StringComparison.InvariantCultureIgnoreCase))
            {
                ctc.ASUCheckBoxVisibility = Visibility.Visible;
                ctc.ASULabelVisibility = Visibility.Collapsed;
            }
            else
            {
                ctc.ASUCheckBoxVisibility = Visibility.Collapsed;
                ctc.ASULabelVisibility = Visibility.Visible;
            }          
        }

        [Category("Свойства элемента мнемосхемы"), Description("Видимость чекбокса для отображения дискретых сигналов."), Browsable(false)]
        public Visibility ASUCheckBoxVisibility
        {
            get { return (Visibility)GetValue(ASUCheckBoxVisibilityProperty); }
            set { SetValue(ASUCheckBoxVisibilityProperty, value); }
        }
        public static DependencyProperty ASUCheckBoxVisibilityProperty = DependencyProperty.Register("ASUCheckBoxVisibility", typeof(Visibility), typeof(CCurrentDataDisplay), new PropertyMetadata(Visibility.Collapsed));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость текстового значения для отображения аналоговых сигналов."), Browsable(false)]
        public Visibility ASULabelVisibility
        {
            get { return (Visibility)GetValue(ASULabelVisibilityProperty); }
            set { SetValue(ASULabelVisibilityProperty, value); }
        }
        public static DependencyProperty ASULabelVisibilityProperty = DependencyProperty.Register("ASULabelVisibility", typeof(Visibility), typeof(CCurrentDataDisplay), new PropertyMetadata(Visibility.Visible));
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Единицы измерения значения тега."), Browsable(true)]
        public string ASUTagUOM
        {
            get { return (string)GetValue(ASUTagUOMProperty); }
            set { SetValue(ASUTagUOMProperty, value); }
        }
        public static readonly DependencyProperty ASUTagUOMProperty = DependencyProperty.Register("ASUTagUOM", typeof(string), typeof(CCurrentDataDisplay), new PropertyMetadata(""));
        
       
        //=======================================================================
        //=======================================================================
        public CCurrentDataDisplay()
        {
            this.DefaultStyleKey = typeof(CCurrentDataDisplay);
        }


        //=======================================================================
        //=======================================================================
        
    }


}



