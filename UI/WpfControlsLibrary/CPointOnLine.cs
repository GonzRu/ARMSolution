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
     [Description("Точка соединения")]
    public class CPointOnLine : CBaseControl
    {
        //[Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]
        //public Visibility ASUControlSelectedVisibility
        //{
        //    get { return (Visibility)GetValue(ASUControlSelectedVisibilityProperty); }
        //    set { SetValue(ASUControlSelectedVisibilityProperty, value); }
        //}
        //public static DependencyProperty ASUControlSelectedVisibilityProperty = DependencyProperty.Register("ASUControlSelectedVisibility", typeof(Visibility), typeof(CPointOnLine), new PropertyMetadata(Visibility.Collapsed));


        //[Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(true)]
        //public bool ASUControlISSelected
        //{
        //    get { return (bool)GetValue(ASUControlISSelectedProperty); }
        //    set { SetValue(ASUControlISSelectedProperty, value); }
        //}
        //public static DependencyProperty ASUControlISSelectedProperty = DependencyProperty.Register("ASUControlISSelected", typeof(bool), typeof(CPointOnLine), new PropertyMetadata(false, OnASUControlISSelectedChanged));
        //private static void OnASUControlISSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CPointOnLine cis = d as CPointOnLine;
        //    cis.ASUControlSelectedVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        //}

        public CPointOnLine()
        {
            this.DefaultStyleKey = typeof(CPointOnLine);
        }
    }
}
