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
using System.Collections.Generic;
//using System.Collections.Generic;
//using System.Linq;

//using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;

namespace SilverlightControlsLibrary
{
    [Description("Базовый элемент мнемосхемы")]
    public class CBaseControl : Button
    {
        #region Свойства элемента
        [Category("Свойства элемента мнемосхемы"), Description("Ширина текстового поля диспетчерского наименования."), Browsable(true)]
        public double ASUTextContentWidth
        {
            get { return (double)GetValue(ASUTextContentWidthProperty); }
            set { SetValue(ASUTextContentWidthProperty, value); }
        }
        public static DependencyProperty ASUTextContentWidthProperty = DependencyProperty.Register("ASUTextContentWidth", typeof(double), typeof(CBaseControl), new PropertyMetadata((double)90));
         //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Размер шрифта."), Browsable(true)]
        public double ASUTextContentFontSize
        {
            get { return (double)GetValue(ASUTextContentFontSizeProperty); }
            set { SetValue(ASUTextContentFontSizeProperty, value); }
        }
        public static DependencyProperty ASUTextContentFontSizeProperty = DependencyProperty.Register("ASUTextContentFontSize", typeof(double), typeof(CBaseControl), new PropertyMetadata((double)10));
        
        //==============================================
       
        [Category("Свойства элемента мнемосхемы"), Description("Отступ диспетчерского наименования в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху."), Browsable(true)]      
        public Thickness ASUMarginText
        {
            get { return (Thickness)GetValue(ASUMarginTextProperty); }
            set { SetValue(ASUMarginTextProperty, value); }
        }
        public static DependencyProperty ASUMarginTextProperty = DependencyProperty.Register("ASUMarginText", typeof(Thickness), typeof(CBaseControl), new PropertyMetadata(new Thickness(-30, 7, 0, 0)));

        //==============================================
         [Category("Свойства элемента мнемосхемы"), Description("Отступ содержимого в формате (0,0,0,0). Через запятую отступ слева, сверху, справа, снизу. Отступ может быть отрицательным (-10). Имеет значение только отступ слева и сверху. Содержимое - это, например, символ 'Замок', отображающий состояние оперативной блокировки для данного элемента."), Browsable(true)]
        public Thickness ASUMarginContent
        {
            get { return (Thickness)GetValue(ASUMarginContentProperty); }
            set { SetValue(ASUMarginContentProperty, value); }
        }
         public static DependencyProperty ASUMarginContentProperty = DependencyProperty.Register("ASUMarginContent", typeof(Thickness), typeof(CBaseControl), new PropertyMetadata(new Thickness(-50, 0, 0, -10)));
        
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]  
        public Visibility ASUControlSelectedVisibility
        {
            get { return (Visibility)GetValue(ASUControlSelectedVisibilityProperty); }
            set { SetValue(ASUControlSelectedVisibilityProperty, value); }
        }
        public static DependencyProperty ASUControlSelectedVisibilityProperty = DependencyProperty.Register("ASUControlSelectedVisibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

       
        [Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]
        public bool ASUControlISSelected
        {
            get { return (bool)GetValue(ASUControlISSelectedProperty); }
            set { SetValue(ASUControlISSelectedProperty, value); }
        }
        public static DependencyProperty ASUControlISSelectedProperty = DependencyProperty.Register("ASUControlISSelected", typeof(bool), typeof(CBaseControl), new PropertyMetadata(false, OnASUControlISSelectedChanged));
        private static void OnASUControlISSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBaseControl cis = d as CBaseControl;
            cis.ASUControlSelectedVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        //==============================================
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость первого плаката."), Browsable(false)]
        public Visibility ASUBanner1Visibility
        {
            get { return (Visibility)GetValue(ASUBanner1VisibilityProperty); }
            set { SetValue(ASUBanner1VisibilityProperty, value); }
        }
        public static DependencyProperty ASUBanner1VisibilityProperty = DependencyProperty.Register("ASUBanner1Visibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость второго плаката."), Browsable(false)]
        public Visibility ASUBanner2Visibility
        {
            get { return (Visibility)GetValue(ASUBanner2VisibilityProperty); }
            set { SetValue(ASUBanner2VisibilityProperty, value); }
        }
        public static DependencyProperty ASUBanner2VisibilityProperty = DependencyProperty.Register("ASUBanner2Visibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость третьего плаката."), Browsable(false)]
        public Visibility ASUBanner3Visibility
        {
            get { return (Visibility)GetValue(ASUBanner3VisibilityProperty); }
            set { SetValue(ASUBanner3VisibilityProperty, value); }
        }
        public static DependencyProperty ASUBanner3VisibilityProperty = DependencyProperty.Register("ASUBanner3Visibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость четвертого плаката."), Browsable(false)]
        public Visibility ASUBanner4Visibility
        {
            get { return (Visibility)GetValue(ASUBanner4VisibilityProperty); }
            set { SetValue(ASUBanner4VisibilityProperty, value); }
        }
        public static DependencyProperty ASUBanner4VisibilityProperty = DependencyProperty.Register("ASUBanner4Visibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость пятого плаката."), Browsable(false)]
        public Visibility ASUBanner5Visibility
        {
            get { return (Visibility)GetValue(ASUBanner5VisibilityProperty); }
            set { SetValue(ASUBanner5VisibilityProperty, value); }
        }
        public static DependencyProperty ASUBanner5VisibilityProperty = DependencyProperty.Register("ASUBanner5Visibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость всех 5 плакатов. Задается числом до 31, где степени двойки - видимость отдельных плакатов. "), Browsable(false)]
        public Single ASUBannersState
        {
            get { return (Single)GetValue(ASUBannersStateProperty); }
            set { SetValue(ASUBannersStateProperty, value); }
        }
        public static DependencyProperty ASUBannersStateProperty = DependencyProperty.Register("ASUBannersState", typeof(Single), typeof(CBaseControl), new PropertyMetadata((Single)0, OnASUBannersStateChanged));
        private static void OnASUBannersStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBaseControl cis = d as CBaseControl;
            cis.ASUBanner1Visibility = (Convert.ToBoolean(Convert.ToInt32(e.NewValue) & 1)) ? Visibility.Visible : Visibility.Collapsed;
            cis.ASUBanner2Visibility = (Convert.ToBoolean(Convert.ToInt32(e.NewValue) & 2)) ? Visibility.Visible : Visibility.Collapsed;
            cis.ASUBanner3Visibility = (Convert.ToBoolean(Convert.ToInt32(e.NewValue) & 4)) ? Visibility.Visible : Visibility.Collapsed;
            cis.ASUBanner4Visibility = (Convert.ToBoolean(Convert.ToInt32(e.NewValue) & 8)) ? Visibility.Visible : Visibility.Collapsed;
            cis.ASUBanner5Visibility = (Convert.ToBoolean(Convert.ToInt32(e.NewValue) & 16)) ? Visibility.Visible : Visibility.Collapsed;
        }
        //==============================================
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость ручной установки состояния."), Browsable(false)]
        public Visibility ASUManualSetVisibility
        {
            get { return (Visibility)GetValue(ASUManualSetVisibilityProperty); }
            set { SetValue(ASUManualSetVisibilityProperty, value); }
        }
        public static DependencyProperty ASUManualSetVisibilityProperty = DependencyProperty.Register("ASUManualSetVisibility", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Collapsed));
        
        [Category("Свойства элемента мнемосхемы"), Description("Подсветка текущего выбранного элемента."), Browsable(false)]
        public bool ASUControlISManualSet
        {
            get { return (bool)GetValue(ASUControlISManualSetProperty); }
            set { SetValue(ASUControlISManualSetProperty, value); }
        }
        public static DependencyProperty ASUControlISManualSetProperty = DependencyProperty.Register("ASUControlISManualSet", typeof(bool), typeof(CBaseControl), new PropertyMetadata(false, OnASUControlISManualSetChanged));
        private static void OnASUControlISManualSetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBaseControl cis = d as CBaseControl;
            cis.ASUManualSetVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Уникальный общий идентификатор КА в системе (например, для оперативных блокировок). Предназначен для однозначной идентификации коммутационного аппарата в цепочках ОБ при изменении состава проекта."), Browsable(true)]  
        public string ASUCommonKAID
        {
            get { return (string)GetValue(ASUCommonKAIDProperty); }
            set { SetValue(ASUCommonKAIDProperty, value); }
        }
        public static DependencyProperty ASUCommonKAIDProperty = DependencyProperty.Register("ASUCommonKAID", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
       
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Кисть диспетчерского наименования."), Browsable(false)]
        public SolidColorBrush ASUTextColorBrush
        {
            get { return (SolidColorBrush)GetValue(ASUTextColorBrushProperty); }
            set { SetValue(ASUTextColorBrushProperty, value); }
        }
        public static DependencyProperty ASUTextColorBrushProperty = DependencyProperty.Register("ASUTextColorBrush", typeof(SolidColorBrush), typeof(CBaseControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 190, 0))));//, OnASUTextColorBrushChanged));

        [Category("Свойства элемента мнемосхемы"), Description("Цвет текста диспетчерского наименования."), Browsable(true)]
        public Color ASUTextColor
        {
            get { return (Color)GetValue(ASUTextColorProperty); }
            set { SetValue(ASUTextColorProperty, value); }
        }
        public static DependencyProperty ASUTextColorProperty = DependencyProperty.Register("ASUTextColor", typeof(Color), typeof(CBaseControl), new PropertyMetadata( Color.FromArgb(255, 255, 190, 0), OnASUTextColorChanged));
        private static void OnASUTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {           
            CBaseControl cis = d as CBaseControl;
            cis.ASUTextColorBrush = new SolidColorBrush((Color)e.NewValue);
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Цвет элемента в соответствии с классом напряжения."), Browsable(false)]
        //[DisplayName("ffffНеопределенное")]        
        public SolidColorBrush ASUVoltageColor
        {
            get { return (SolidColorBrush)GetValue(ASUVoltageColorProperty); }
            set { SetValue(ASUVoltageColorProperty, value); }
        }
        public static DependencyProperty ASUVoltageColorProperty = DependencyProperty.Register("ASUVoltageColor", typeof(SolidColorBrush), typeof(CBaseControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 180, 200))));
        
        //==============================================
         [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения элемента."), Browsable(true)]
        public ASUCommutationDeviceVoltageClasses ASUCommutationDeviceVoltage
        {
            get { return (ASUCommutationDeviceVoltageClasses)GetValue(ASUCommutationDeviceVoltageProperty); }
            set
            {
                SetValue(ASUCommutationDeviceVoltageProperty, value);
            }
        }
        public static DependencyProperty ASUCommutationDeviceVoltageProperty = DependencyProperty.Register("ASUCommutationDeviceVoltage", typeof(ASUCommutationDeviceVoltageClasses), typeof(CBaseControl), new PropertyMetadata(ASUCommutationDeviceVoltageClasses.kV110, OnASUCommutationDeviceVoltagePropertyChanged));
        private static void OnASUCommutationDeviceVoltagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region класс напряжения
            CBaseControl cis = d as CBaseControl;
            cis.ASUCommutationDeviceVoltage = (ASUCommutationDeviceVoltageClasses)e.NewValue;
            switch (cis.ASUCommutationDeviceVoltage)
            {

                case ASUCommutationDeviceVoltageClasses.kV1150:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 205, 138, 255));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV750:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV500:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 165, 15, 10));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV400:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 240, 150, 30));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV330:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 140, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV220:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 200, 200, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV150:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 170, 150, 0));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV110:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV35:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 130, 100, 50));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV10:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV6:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 200, 150, 100));
                    break;
                case ASUCommutationDeviceVoltageClasses.kV04:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVGenerator:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 230, 70, 230));
                    break;
                case ASUCommutationDeviceVoltageClasses.kVRepair:
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 205, 255, 155));
                    break;
                default://ASUCommutationDeviceVoltageClasses.kVEmpty
                    cis.ASUVoltageColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;

            }
            #endregion состояние ВВ
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Текущее состояние элемента."), Browsable(false)]
        public ASUCommutationDeviceStates ASUCommutationDeviceState
        {
            get { return (ASUCommutationDeviceStates)GetValue(ASUCommutationDeviceStateProperty); }
            set
            {
                SetValue(ASUCommutationDeviceStateProperty, value);
                OnRaiseASUCommutationDeviceStateEvent(new EventArgsASUCommutationDeviceState(value));
            }
        }
        public static DependencyProperty ASUCommutationDeviceStateProperty = DependencyProperty.Register("ASUCommutationDeviceState", typeof(ASUCommutationDeviceStates), typeof(CBaseControl), new PropertyMetadata(ASUCommutationDeviceStates.Off, OnASUCommutationDeviceStatePropertyChanged));
        private static void OnASUCommutationDeviceStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние 
            CBaseControl cis = d as CBaseControl;
            cis.ASUCommutationDeviceState = (ASUCommutationDeviceStates)e.NewValue;
            switch (cis.ASUCommutationDeviceState)
            {
                case ASUCommutationDeviceStates.On:
                    VisualStateManager.GoToState(cis, "VisualStateOn", false);
                    break;
                case ASUCommutationDeviceStates.Off:
                    VisualStateManager.GoToState(cis, "VisualStateOff", false);
                    break;
                case ASUCommutationDeviceStates.Broken:
                    VisualStateManager.GoToState(cis, "VisualStateBroken", false);
                    break;
                case ASUCommutationDeviceStates.OutOfWork:
                    VisualStateManager.GoToState(cis, "VisualStateOutOfWork", false);
                    break;
                 case ASUCommutationDeviceStates.TurningOn:
                    VisualStateManager.GoToState(cis, "VisualStateTurningOn", false);
                    break;
                case ASUCommutationDeviceStates.TurningOff:
                    VisualStateManager.GoToState(cis, "VisualStateTurningOff", false);
                    break;
                       case ASUCommutationDeviceStates.ManualPE:
                    VisualStateManager.GoToState(cis, "VisualStateManualPE", false);
                    break;

                default://ASUCommutationDeviceStates.Undefined
                    VisualStateManager.GoToState(cis, "VisualStateUnDefined", false);
                    break;
                    
            }
            #endregion состояние 
        }

        public class EventArgsASUCommutationDeviceState : EventArgs
        {
            #region Класс, аргумент события, содержащий состояние 
            private ASUCommutationDeviceStates сommutationDeviceState;
            public ASUCommutationDeviceStates ASUCommutationDeviceState
            {
                get { return сommutationDeviceState; }
                set { сommutationDeviceState = value; }
            }
            public EventArgsASUCommutationDeviceState(ASUCommutationDeviceStates AСommutationDeviceState)
            {
                сommutationDeviceState = AСommutationDeviceState;
            }
            #endregion Класс, аргумент события, содержащий состояние 
        }
        public event EventHandler<EventArgsASUCommutationDeviceState> OnASUCommutationDeviceStateChange;
        protected virtual void OnRaiseASUCommutationDeviceStateEvent(EventArgsASUCommutationDeviceState e)
        {
            #region Событие при изменении состояния
            //Упаковываем вызов события в protected virtual метод чтобы позволить наследникам перекрывать его
            // Создаем временную копию события, чтобы корректно обработать ситуацию, когда последний подписчик события отписался от него сразу после 
            // проверки if (handler != null), но еще до того как событие наступило.
            EventHandler<EventArgsASUCommutationDeviceState> handler = OnASUCommutationDeviceStateChange;

            // Если нет ни одного подписчика, handler == null
            if (handler != null)
            {
                // Можно еще что-нибудь сделать с аргументами  
                //e.Message = "Скачан файл:     " + e.Message;

                // Запускаем событие
                handler(this, e);
            }
            #endregion Событие при изменении состояния
        }

        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Текущий режим управления элемента."), Browsable(false)]  
        public ASUCommutationDeviceControlModes ASUCommutationDeviceControlMode
        {
            get { return (ASUCommutationDeviceControlModes)GetValue(ASUCommutationDeviceControlModeProperty); }
            set
            {
                SetValue(ASUCommutationDeviceControlModeProperty, value);
                OnRaiseASUCommutationDeviceControlModeEvent(new EventArgsASUCommutationDeviceControlMode(value));
            }
        }
        public static DependencyProperty ASUCommutationDeviceControlModeProperty = DependencyProperty.Register("ASUCommutationDeviceControlMode", typeof(ASUCommutationDeviceControlModes), typeof(CBaseControl), new PropertyMetadata(ASUCommutationDeviceControlModes.Distance, OnASUCommutationDeviceControlModePropertyChanged));
        private static void OnASUCommutationDeviceControlModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region режим управления 
            CBaseControl cis = d as CBaseControl;
            cis.ASUCommutationDeviceControlMode = (ASUCommutationDeviceControlModes)e.NewValue;
            switch (cis.ASUCommutationDeviceControlMode)
            {
                case ASUCommutationDeviceControlModes.LocalWithBlocks:
                    VisualStateManager.GoToState(cis, "LocalControlBlock", false);
                    break;
                case ASUCommutationDeviceControlModes.LocalWithoutBlocks:
                    VisualStateManager.GoToState(cis, "LocalControlUnBlock", false);
                    break;

                default://ASUCommutationDeviceControlModes.Undefined или Distance
                    VisualStateManager.GoToState(cis, "DistanceControl", false);
                    break;

            }
            #endregion режим управления 
        }

        public class EventArgsASUCommutationDeviceControlMode : EventArgs
        {
            #region Класс, аргумент события, содержащий режим управления 
            private ASUCommutationDeviceControlModes сommutationDeviceControlMode;
            public ASUCommutationDeviceControlModes СommutationDeviceControlMode
            {
                get { return сommutationDeviceControlMode; }
                set { сommutationDeviceControlMode = value; }
            }
            public EventArgsASUCommutationDeviceControlMode(ASUCommutationDeviceControlModes AСommutationDeviceControlMode)
            {
                сommutationDeviceControlMode = AСommutationDeviceControlMode;
            }
            #endregion Класс, аргумент события, содержащий режим управления 
        }
        public event EventHandler<EventArgsASUCommutationDeviceControlMode> OnASUCommutationDeviceControlModeChange;
        protected virtual void OnRaiseASUCommutationDeviceControlModeEvent(EventArgsASUCommutationDeviceControlMode e)
        {
            #region Событие при изменении режима управления 
            //Упаковываем вызов события в protected virtual метод чтобы позволить наследникам перекрывать его
            // Создаем временную копию события, чтобы корректно обработать ситуацию, когда последний подписчик события отписался от него сразу после 
            // проверки if (handler != null), но еще до того как событие наступило.
            EventHandler<EventArgsASUCommutationDeviceControlMode> handler = OnASUCommutationDeviceControlModeChange;

            // Если нет ни одного подписчика, handler == null
            if (handler != null)
            {
                // Можно еще что-нибудь сделать с аргументами  
                //e.Message = "Скачан файл:     " + e.Message;

                // Запускаем событие
                handler(this, e);
            }
            #endregion Событие при изменении режима управления 
        }

        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Строка сетки элемента, в которой расположено его содержимое."), Browsable(false)]  
        public int ASUGridRowContent
        {
            get { return (int)GetValue(ASUGridRowContentProperty); }
            set { SetValue(ASUGridRowContentProperty, value); }
        }
        public static readonly DependencyProperty ASUGridRowContentProperty = DependencyProperty.Register("ASUGridRowContent", typeof(int), typeof(CBaseControl), new PropertyMetadata(0));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Столбец сетки элемента, в которой расположено его содержимое."), Browsable(false)]  
        public int ASUGridColumnContent
        {
            get { return (int)GetValue(ASUGridColumnContentProperty); }
            set { SetValue(ASUGridColumnContentProperty, value); }
        }
        public static readonly DependencyProperty ASUGridColumnContentProperty = DependencyProperty.Register("ASUGridColumnContent", typeof(int), typeof(CBaseControl), new PropertyMetadata(2));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Положение содержимого элемента относительно него. Содержимое - это, например, символ 'Замок', отображающий состояние оперативной блокировки для данного элемента."), Browsable(true)]  
        public ASUContentRelativelyPositions ASUContentRelativelyPosition
        {
            get { return (ASUContentRelativelyPositions)GetValue(ASUContentRelativelyPositionProperty); }
            set { SetValue (ASUContentRelativelyPositionProperty, value);  }
        }
        public static DependencyProperty ASUContentRelativelyPositionProperty = DependencyProperty.Register("ASUContentRelativelyPosition", typeof(ASUContentRelativelyPositions), typeof(CBaseControl), new PropertyMetadata(ASUContentRelativelyPositions.TopRight, OnASUContentRelativelyPositionPropertyChanged));
        private static void OnASUContentRelativelyPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region положение контента относительно элемента
            CBaseControl cis = d as CBaseControl;
            cis.ASUContentRelativelyPosition = (ASUContentRelativelyPositions)e.NewValue;
            switch (cis.ASUContentRelativelyPosition)
            {
                case ASUContentRelativelyPositions.Left:
                    cis.ASUGridRowContent = 1;
                    cis.ASUGridColumnContent = 0;
                    break;

                case ASUContentRelativelyPositions.TopLeft:
                    cis.ASUGridRowContent = 0;
                    cis.ASUGridColumnContent = 0;
                    break;
                
                case ASUContentRelativelyPositions.Top:
                    cis.ASUGridRowContent = 0;
                    cis.ASUGridColumnContent = 1;
                    break;

                case ASUContentRelativelyPositions.TopRight:
                    cis.ASUGridRowContent = 0;
                    cis.ASUGridColumnContent = 2;
                    break;

                case ASUContentRelativelyPositions.Right:
                    cis.ASUGridRowContent = 1;
                    cis.ASUGridColumnContent = 2;
                    break;

                case ASUContentRelativelyPositions.BottomRight:
                    cis.ASUGridRowContent = 2;
                    cis.ASUGridColumnContent = 2;
                    break;

                case ASUContentRelativelyPositions.Bottom:
                    cis.ASUGridRowContent = 2;
                    cis.ASUGridColumnContent = 1;
                    break;
                                   
               
                case ASUContentRelativelyPositions.BottomLeft:
                    cis.ASUGridRowContent = 2;
                    cis.ASUGridColumnContent = 0;
                    break;
              

                default://ASUContentRelativelyPositions.Center
                    cis.ASUGridRowContent = 1;
                    cis.ASUGridColumnContent = 1;
                    break;

            }
            #endregion положение контента относительно элемента
        }
        //==============================================
        [Category("Свойства элемента мнемосхемы"), Description("Строка сетки элемента, в которой расположено его диспетчерское наименование."), Browsable(false)]
        public int ASUGridRowASUTextContent
        {
            get { return (int)GetValue(ASUGridRowASUTextContentProperty); }
            set { SetValue(ASUGridRowASUTextContentProperty, value); }
        }
        public static readonly DependencyProperty ASUGridRowASUTextContentProperty = DependencyProperty.Register("ASUGridRowASUTextContent", typeof(int), typeof(CBaseControl), new PropertyMetadata(1));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Столбец сетки элемента, в которой расположено его диспетчерское наименование."), Browsable(false)]
        public int ASUGridColumnASUTextContent
        {
            get { return (int)GetValue(ASUGridColumnASUTextContentProperty); }
            set { SetValue(ASUGridColumnASUTextContentProperty, value); }
        }
        public static readonly DependencyProperty ASUGridColumnASUTextContentProperty = DependencyProperty.Register("ASUGridColumnASUTextContent", typeof(int), typeof(CBaseControl), new PropertyMetadata(2));// { AffectsRender = true });

        [Category("Свойства элемента мнемосхемы"), Description("Положение диспетчерского наименования элемента относительно него."), Browsable(true)]
        public ASUContentRelativelyPositions ASUTextContentRelativelyPosition
        {
            get { return (ASUContentRelativelyPositions)GetValue(ASUTextContentRelativelyPositionProperty); }
            set { SetValue(ASUTextContentRelativelyPositionProperty, value); }
        }
        public static DependencyProperty ASUTextContentRelativelyPositionProperty = DependencyProperty.Register("ASUTextContentRelativelyPosition", typeof(ASUContentRelativelyPositions), typeof(CBaseControl), new PropertyMetadata(ASUContentRelativelyPositions.Right, OnASUTextContentRelativelyPositionPropertyChanged));
        private static void OnASUTextContentRelativelyPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region положение текста относительно элемента
            CBaseControl cis = d as CBaseControl;
            cis.ASUTextContentRelativelyPosition = (ASUContentRelativelyPositions)e.NewValue;
            switch (cis.ASUTextContentRelativelyPosition)
            {
                case ASUContentRelativelyPositions.Left:
                    cis.ASUGridRowASUTextContent = 1;
                    cis.ASUGridColumnASUTextContent = 0;
                    break;

                case ASUContentRelativelyPositions.TopLeft:
                    cis.ASUGridRowASUTextContent = 0;
                    cis.ASUGridColumnASUTextContent = 0;
                    break;

                case ASUContentRelativelyPositions.Top:
                    cis.ASUGridRowASUTextContent = 0;
                    cis.ASUGridColumnASUTextContent = 1;
                    break;

                case ASUContentRelativelyPositions.TopRight:
                    cis.ASUGridRowASUTextContent = 0;
                    cis.ASUGridColumnASUTextContent = 2;
                    break;

                case ASUContentRelativelyPositions.Right:
                    cis.ASUGridRowASUTextContent = 1;
                    cis.ASUGridColumnASUTextContent = 2;
                    break;

                case ASUContentRelativelyPositions.BottomRight:
                    cis.ASUGridRowASUTextContent = 2;
                    cis.ASUGridColumnASUTextContent = 2;
                    break;

                case ASUContentRelativelyPositions.Bottom:
                    cis.ASUGridRowASUTextContent = 2;
                    cis.ASUGridColumnASUTextContent = 1;
                    break;


                case ASUContentRelativelyPositions.BottomLeft:
                    cis.ASUGridRowASUTextContent = 2;
                    cis.ASUGridColumnASUTextContent = 0;
                    break;


                default://ASUContentRelativelyPositions.Center
                    cis.ASUGridRowASUTextContent = 1;
                    cis.ASUGridColumnASUTextContent = 1;
                    break;

            }
            #endregion положение контента относительно элемента
        }

        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Дискретное значение тега элемента."), Browsable(false)]
        public bool? ASUDiscretContentValue
        {
            get { return (bool?)GetValue(ASUDiscretContentValueProperty); }
            set { SetValue(ASUDiscretContentValueProperty, value); }
        }
        public static readonly DependencyProperty ASUDiscretContentValueProperty = DependencyProperty.Register("ASUDiscretContentValue", typeof(bool?), typeof(CBaseControl), new PropertyMetadata(null));
        
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Не дискретное значение тега элемента."), Browsable(false)]
        public string ASUContentValue
        {
            get { return (string)GetValue(ASUContentValueProperty); }
            set { SetValue(ASUContentValueProperty, value); }
        }
        public static readonly DependencyProperty ASUContentValueProperty = DependencyProperty.Register("ASUContentValue", typeof(string), typeof(CBaseControl), new PropertyMetadata("Значение", OnASUContentValuePropertyChanged));
        private static void OnASUContentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBaseControl isw = d as CBaseControl;
        }
           
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Диспетчерское наименование элемента."), Browsable(true)]
        public string ASUTextContent
        {
            get { return (string)GetValue(ASUTextContentProperty); }
            set { SetValue(ASUTextContentProperty, value); }
        }
        public static readonly DependencyProperty ASUTextContentProperty = DependencyProperty.Register("ASUTextContent", typeof(string), typeof(CBaseControl), new PropertyMetadata("", OnASUTextContentPropertyChanged));
        private static void OnASUTextContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CBaseControl isw = d as CBaseControl;
            if (((string)e.NewValue).Length > 0)
                isw.ASUTextContentVisible = Visibility.Visible;
            else
                isw.ASUTextContentVisible = Visibility.Collapsed;
        }
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Видимость диспетчерского наименования элемента."), Browsable(false)]
        public Visibility ASUTextContentVisible
        {
            get { return (Visibility)GetValue(ASUTextContentVisibleProperty); }
            set { SetValue(ASUTextContentVisibleProperty, value); }
        }
        public static readonly DependencyProperty ASUTextContentVisibleProperty = DependencyProperty.Register("ASUTextContentVisible", typeof(Visibility), typeof(CBaseControl), new PropertyMetadata(Visibility.Visible));
        
        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Угол поворота элемента."), Browsable(true)]
        public double ASUAngle
        {
            get { return (double)GetValue(ASUAngleProperty); }
            set { SetValue(ASUAngleProperty, value); }
        }
        public static readonly DependencyProperty ASUAngleProperty = DependencyProperty.Register("ASUAngle", typeof(double), typeof(CBaseControl), new PropertyMetadata(0.0));// { AffectsRender = true });

        //=======================================================================
        // Приходится извращаться с AttachedProperty потому что RotateTransform, с помощью которого мы хотим поворачивать Grid на какой-нибудь угол,  
        // не является FrameworkElement и нельзя его DependencyProperty "Угол" просто привязать к нашему "ASUAngle"
        //[AttachedPropertyBrowsableForType(typeof(Grid))]
        public static double GetAttachedASUAngle(DependencyObject d)
        {
            return (double)d.GetValue(AttachedASUAngleProperty);
        }
        public static void SetAttachedASUAngle(DependencyObject d, double value)
        {
            d.SetValue(AttachedASUAngleProperty, value);
        }

        public static readonly DependencyProperty AttachedASUAngleProperty = DependencyProperty.RegisterAttached("AttachedASUAngle", typeof(double), typeof(CBaseControl), new PropertyMetadata(0.0, OnASUAngleChanged));

        private static void OnASUAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region OnASUAngleChanged
            if (d is UIElement)
            {
                UIElement b = d as UIElement;
                if (e.NewValue is double)
                {
                    double c = (double)e.NewValue;
                    if (!double.IsNaN(c))
                    {
                        if (b.RenderTransform is RotateTransform)
                            (b.RenderTransform as RotateTransform).Angle = c;
                        else
                            b.RenderTransform = new RotateTransform() { Angle = c };
                    }
                }
            }
            #endregion OnASUAngleChanged
        }


         //=======================================================================
        //[Category("Свойства элемента мнемосхемы"), Description("Режим привязки данных состояния элемента. Один дискретный тег РПВ, два тега РПВ и РПО, один комплексный расчетный тег состояния (число от 0 до 7)."), Browsable(true)]
        //public ASUCommutationDeviceStateModes ASUCommutationDeviceStateMode
        //{
        //    get { return (ASUCommutationDeviceStateModes)GetValue(ASUCommutationDeviceStateModeProperty); }
        //    set { SetValue(ASUCommutationDeviceStateModeProperty, value); }
        //}
        //public static readonly DependencyProperty ASUCommutationDeviceStateModeProperty = DependencyProperty.Register("ASUCommutationDeviceStateMode", typeof(ASUCommutationDeviceStateModes), typeof(CBaseControl), new PropertyMetadata(ASUCommutationDeviceStateModes.TwoTagsModePRO_RPV));

        //=======================================================================
        [Category("Свойства элемента мнемосхемы"), Description("Ручной ввод данных состояния элемента разрешен."), Browsable(true)]
        public bool ASUCommutationDeviceStateManualSetEnabled
        {
            get { return (bool)GetValue(ASUCommutationDeviceStateManualSetEnabledProperty); }
            set { SetValue(ASUCommutationDeviceStateManualSetEnabledProperty, value); }
        }
        public static readonly DependencyProperty ASUCommutationDeviceStateManualSetEnabledProperty = DependencyProperty.Register("ASUCommutationDeviceStateManualSetEnabled", typeof(bool), typeof(CBaseControl), new PropertyMetadata(false));
        
        #endregion Свойства элемента

        //=======================================================================
        //=======================================================================
        //=======================================================================

        #region Привязки 

        //[Category("Привязки данных"), Description("ID тега РПВ для состояния элемента. Для привязки достаточно перетащить тег на элемент из дерева проекта слева."), Browsable(true)]
        //public string ASUTagIDStateRPV
        //{
        //    get { return (string)GetValue(ASUTagIDStateRPVProperty); }
        //    set { SetValue(ASUTagIDStateRPVProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDStateRPVProperty = DependencyProperty.Register("ASUTagIDStateRPV", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки данных"), Description("ID тега РПО для состояния элемента. Для привязки достаточно перетащить тег на элемент из дерева проекта слева."), Browsable(true)]
        //public string ASUTagIDStateRPO
        //{
        //    get { return (string)GetValue(ASUTagIDStateRPOProperty); }
        //    set { SetValue(ASUTagIDStateRPOProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDStateRPOProperty = DependencyProperty.Register("ASUTagIDStateRPO", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================
        [Category("Привязки данных"), Description("ID тега состояния элемента (рассчетное значение состояния коммутационного аппарата). Для привязки достаточно перетащить тег на элемент из дерева проекта слева."), Browsable(true)]
        public string ASUTagIDState
        {
            get { return (string)GetValue(ASUTagIDStateProperty); }
            set { SetValue(ASUTagIDStateProperty, value); }
        }
        public static DependencyProperty ASUTagIDStateProperty = DependencyProperty.Register("ASUTagIDState", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================

        [Category("Привязки данных"), Description("ID тега режима управления элемента. Например, местное/дистанционное управление обозначается рамкой вокруг элемента. Видимость рамки можно привязтаь к соответствующему тегу."), Browsable(true)]
        public string ASUTagIDControlMode
        {
            get { return (string)GetValue(ASUTagIDControlModeProperty); }
            set { SetValue(ASUTagIDControlModeProperty, value); }
        }
        public static DependencyProperty ASUTagIDControlModeProperty = DependencyProperty.Register("ASUTagIDControlMode", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================

        [Category("Привязки данных"), Description("ID тега видимости плакатов. Тег в виртуальном устройстве, значение задается только вручную с мнемосхемы. Видимость всех 5 плакатов. Задается числом до 31, где степени двойки - видимость отдельных плакатов."), Browsable(true)]
        public string ASUTagIDBanners
        {
            get { return (string)GetValue(ASUTagIDBannersProperty); }
            set { SetValue(ASUTagIDBannersProperty, value); }
        }
        public static DependencyProperty ASUTagIDBannersProperty = DependencyProperty.Register("ASUTagIDBanners", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================

        [Category("Привязки данных"), Description("ID привязанных устройств. Для привязки достаточно перетащить устройство на элемент из дерева проекта слева."), Browsable(true)]
        public string ASUDeviceIDs
        {
            get { return (string)GetValue(ASUDeviceIDsProperty); }
            set { SetValue(ASUDeviceIDsProperty, value); }
        }
        public static DependencyProperty ASUDeviceIDsProperty = DependencyProperty.Register("ASUDeviceIDs", typeof(string), typeof(CBaseControl), new PropertyMetadata( "-1" ));
        //=======================================================================

        [Category("Привязки данных"), Description("ID привязанных тегов для всплывающих подсказок. Например, токи фаз. Для привязки достаточно перетащить тег на элемент из дерева проекта слева."), Browsable(true)]
        public string ASUToolTipsTagIDs
        {
            get { return (string)GetValue(ASUToolTipsTagIDsProperty); }
            set { SetValue(ASUToolTipsTagIDsProperty, value); }
        }
        public static DependencyProperty ASUToolTipsTagIDsProperty = DependencyProperty.Register("ASUToolTipsTagIDs", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================
        //[Category("Привязки данных"), Description("ID тега дискретного значения."), Browsable(true)]
        //public string ASUTagIDDisctretValue
        //{
        //    get { return (string)GetValue(ASUTagIDDisctretValueProperty); }
        //    set { SetValue(ASUTagIDDisctretValueProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDDisctretValueProperty = DependencyProperty.Register("ASUTagIDDisctretValue", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================
        [Category("Привязки данных"), Description("ID тега значения элемента. Например, ID тега 'ток Ia' для отображения тока на мнемосхеме. Привязывается автоматически при перетаскивании на схему тега из дерева проекта слева, но можно изменить. Если через ';' добавить еще несколько - они будут отображаться в всплывающей подсказке."), Browsable(true)]
        public string ASUTagIDAnyValue
        {
            get { return (string)GetValue(ASUTagIDAnyValueProperty); }
            set { SetValue(ASUTagIDAnyValueProperty, value); }
        }
        public static DependencyProperty ASUTagIDAnyValueProperty = DependencyProperty.Register("ASUTagIDAnyValue", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================
        //=======================================================================
        [Category("Привязки команд"), Description("ID тега команды включения."), Browsable(true)]
        public string ASUTagIDCommandOn
        {
            get { return (string)GetValue(ASUTagIDCommandOnProperty); }
            set { SetValue(ASUTagIDCommandOnProperty, value); }
        }
        public static DependencyProperty ASUTagIDCommandOnProperty = DependencyProperty.Register("ASUTagIDCommandOn", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        //=======================================================================
        [Category("Привязки команд"), Description("ID тега команды отключения."), Browsable(true)]
        public string ASUTagIDCommandOff
        {
            get { return (string)GetValue(ASUTagIDCommandOffProperty); }
            set { SetValue(ASUTagIDCommandOffProperty, value); }
        }
        public static DependencyProperty ASUTagIDCommandOffProperty = DependencyProperty.Register("ASUTagIDCommandOff", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        ////=======================================================================
        [Category("Привязки команд"), Description("ID тега команды выдачи готовности управления КА."), Browsable(true)]
        public string ASUTagIDCommandReady
        {
            get { return (string)GetValue(ASUTagIDCommandReadyProperty); }
            set { SetValue(ASUTagIDCommandReadyProperty, value); }
        }
        public static DependencyProperty ASUTagIDCommandReadyProperty = DependencyProperty.Register("ASUTagIDCommandReady", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды сброса ресурса КА."), Browsable(true)]
        //public string ASUTagIDCommandClearResource
        //{
        //    get { return (string)GetValue(ASUTagIDCommandClearResourceProperty); }
        //    set { SetValue(ASUTagIDCommandClearResourceProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandClearResourceProperty = DependencyProperty.Register("ASUTagIDCommandClearResource", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        ////=======================================================================
        [Category("Привязки команд"), Description("ID тега команды квитирования."), Browsable(true)]
        public string ASUTagIDCommandReceipt
        {
            get { return (string)GetValue(ASUTagIDCommandReceiptProperty); }
            set { SetValue(ASUTagIDCommandReceiptProperty, value); }
        }
        public static DependencyProperty ASUTagIDCommandReceiptProperty = DependencyProperty.Register("ASUTagIDCommandReceipt", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
               
        ////=======================================================================
        //[Category("Привязки команд"), Description("ID тега команды записи уставок."), Browsable(true)]
        //public string ASUTagIDCommandWriteSettings
        //{
        //    get { return (string)GetValue(ASUTagIDCommandWriteSettingsProperty); }
        //    set { SetValue(ASUTagIDCommandWriteSettingsProperty, value); }
        //}
        //public static DependencyProperty ASUTagIDCommandWriteSettingsProperty = DependencyProperty.Register("ASUTagIDCommandWriteSettings", typeof(string), typeof(CBaseControl), new PropertyMetadata("-1"));
        
        //=======================================================================
        //=======================================================================

        #endregion Привязки

        public CBaseControl()
        {
            this.DefaultStyleKey = typeof(CBaseControl);
        }

        //protected override void OnRender()
        //{          
        //    switch (this.ASUCommutationDeviceState)
        //    {
        //        case ASUCommutationDeviceStates.On:
        //            VisualStateManager.GoToState(this, "VisualStateOn", false);
        //            break;
        //        case ASUCommutationDeviceStates.Off:
        //            VisualStateManager.GoToState(this, "VisualStateOff", false);
        //            break;
        //        case ASUCommutationDeviceStates.TurningOn:
        //            VisualStateManager.GoToState(this, "VisualStateTurningOn", false);
        //            break;
        //        case ASUCommutationDeviceStates.TurningOff:
        //            VisualStateManager.GoToState(this, "VisualStateTurningOff", false);
        //            break;
        //        case ASUCommutationDeviceStates.Broken:
        //            VisualStateManager.GoToState(this, "VisualStateBroken", false);
        //            break;
        //        case ASUCommutationDeviceStates.ManualPE:
        //            VisualStateManager.GoToState(this, "VisualStateManualPE", false);
        //            break;
        //        case ASUCommutationDeviceStates.OutOfWork:
        //            VisualStateManager.GoToState(this, "VisualStateOutOfWork", false);
        //            break;
        //        default://CommutationDeviceStates.Undefined
        //            VisualStateManager.GoToState(this, "VisualStateUnDefined", false);
        //            break;

        //    }
        //} 
        //=======================================================================
        //=======================================================================
        
    }


}



