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

     [Description("Режим работы")]
    public class CSystemWorkModeSL : Button
    {
        //=======================================================================
        //=======================================================================
        public bool TagDuty
        {
            get { return (bool)GetValue(TagDutyProperty); }
            set { SetValue(TagDutyProperty, value); CheckState(); }
        }
        public static DependencyProperty TagDutyProperty = DependencyProperty.Register("TagDuty", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagDutyPropertyChanged));

        private static void OnTagDutyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }
        //=======================================================================
        public bool TagPE
        {
            get { return (bool)GetValue(TagPEProperty); }
            set { SetValue(TagPEProperty, value); CheckState(); }
        }
        public static DependencyProperty TagPEProperty = DependencyProperty.Register("TagPE", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagPEPropertyChanged));

        private static void OnTagPEPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }
        //=======================================================================
        public bool TagFindPE
        {
            get { return (bool)GetValue(TagFindPEProperty); }
            set { SetValue(TagFindPEProperty, value); CheckState(); }
        }
        public static DependencyProperty TagFindPEProperty = DependencyProperty.Register("TagFindPE", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagFindPEPropertyChanged));

        private static void OnTagFindPEPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }
        //=======================================================================
        public bool TagControl
        {
            get { return (bool)GetValue(TagControlProperty); }
            set { SetValue(TagControlProperty, value); CheckState(); }
        }
        public static DependencyProperty TagControlProperty = DependencyProperty.Register("TagControl", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagControlPropertyChanged));

        private static void OnTagControlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }
        //=======================================================================
        public bool TagLocalWithBlock
        {
            get { return (bool)GetValue(TagLocalWithBlockProperty); }
            set { SetValue(TagLocalWithBlockProperty, value); CheckState(); }
        }
        public static DependencyProperty TagLocalWithBlockProperty = DependencyProperty.Register("TagLocalWithBlock", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagLocalWithBlockPropertyChanged));

        private static void OnTagLocalWithBlockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }
        //=======================================================================
        public bool TagLocalWithoutBlock
        {
            get { return (bool)GetValue(TagLocalWithoutBlockProperty); }
            set { SetValue(TagLocalWithoutBlockProperty, value); CheckState(); }
        }
        public static DependencyProperty TagLocalWithoutBlockProperty = DependencyProperty.Register("TagLocalWithoutBlock", typeof(bool), typeof(CSystemWorkModeSL), new PropertyMetadata(false, OnTagLocalWithoutBlockPropertyChanged));

        private static void OnTagLocalWithoutBlockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region состояние
            CSystemWorkModeSL csw = d as CSystemWorkModeSL;
            csw.TagDuty = (bool)e.NewValue;
            csw.CheckState();
            #endregion состояние
        }

        //=======================================================================       
        //=======================================================================
        public object RealTag
        {
            get { return (object)GetValue(RealTagProperty); }
            set { SetValue(RealTagProperty, value); }
        }
        public static DependencyProperty RealTagProperty = DependencyProperty.Register("RealTag", typeof(object), typeof(CSystemWorkModeSL), new PropertyMetadata(false));
        //=======================================================================
        public ASUSystemWorkStates SystemWorkState
        {
            get { return (ASUSystemWorkStates)GetValue(SystemWorkStateProperty); }
            set
            {
                SetValue(SystemWorkStateProperty, value);
                OnRaiseSystemWorkStateChangEvent(new EventArgsSystemWorkState(value));
            }
        }
        public static DependencyProperty SystemWorkStateProperty = DependencyProperty.Register("SystemWorkState", typeof(ASUSystemWorkStates), typeof(CSystemWorkModeSL), new PropertyMetadata(ASUSystemWorkStates.UnDefined, OnSystemWorkStatePropertyChanged));
        private static void OnSystemWorkStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region Режим системы
            CSystemWorkModeSL wm = d as CSystemWorkModeSL;
            wm.SystemWorkState = (ASUSystemWorkStates)e.NewValue;
            switch (wm.SystemWorkState)
            {
                case ASUSystemWorkStates.Duty:
                    VisualStateManager.GoToState(wm, "VisualStateDuty", false);
                    break;
                case ASUSystemWorkStates.PE:
                    VisualStateManager.GoToState(wm, "VisualStatePE", false);
                    break;
                case ASUSystemWorkStates.FindPE:
                    VisualStateManager.GoToState(wm, "VisualStateFindPE", false);
                    break;
                case ASUSystemWorkStates.Control:
                    VisualStateManager.GoToState(wm, "VisualStateDistControl", false);
                    break;
                case ASUSystemWorkStates.LocalWithBlock:
                    VisualStateManager.GoToState(wm, "VisualStateLocalWithBlock", false);
                    break;
                case ASUSystemWorkStates.LocalWithoutBlock:
                    VisualStateManager.GoToState(wm, "VisualStateLocalWithoutBlock", false);
                    break;
                default://ASUSystemWorkStates.UnDefined
                    VisualStateManager.GoToState(wm, "VisualStateUnDefined", false);
                    break;

            }
            #endregion Режим системы
        }

        public class EventArgsSystemWorkState : EventArgs
        {
            #region Класс, аргумент события, содержащий состояние КА
            private ASUSystemWorkStates systemWorkState;
            public ASUSystemWorkStates SystemWorkState
            {
                get { return systemWorkState; }
                set { systemWorkState = value; }
            }
            public EventArgsSystemWorkState(ASUSystemWorkStates ASystemWorkState)
            {
                systemWorkState = ASystemWorkState;
            }
            #endregion Класс, аргумент события, содержащий состояние КА
        }
        public event EventHandler<EventArgsSystemWorkState> OnSystemWorkStateChange;
        protected virtual void OnRaiseSystemWorkStateChangEvent(EventArgsSystemWorkState e)
        {
            #region Событие при изменении состояния
            //Упаковываем вызов события в protected virtual метод чтобы позволить наследникам перекрывать его
            // Создаем временную копию события, чтобы корректно обработать ситуацию, когда последний подписчик события отписался от него сразу после 
            // проверки if (handler != null), но еще до того как событие наступило.
            EventHandler<EventArgsSystemWorkState> handler = OnSystemWorkStateChange;

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
        //=======================================================================
        private void CheckState()
        {
            if (TagFindPE)
                SystemWorkState = ASUSystemWorkStates.FindPE;

            else if (TagPE)
                SystemWorkState = ASUSystemWorkStates.PE;

            else if (TagLocalWithoutBlock)
                SystemWorkState = ASUSystemWorkStates.LocalWithoutBlock;

            else if (TagLocalWithBlock)
                SystemWorkState = ASUSystemWorkStates.LocalWithBlock;

            else if (TagControl)
                SystemWorkState = ASUSystemWorkStates.Control;

            else
                SystemWorkState = ASUSystemWorkStates.Duty;
            //else if (TagDuty)
            //    SystemWorkState = ASUSystemWorkStates.Duty;            

            //else
            //    SystemWorkState = ASUSystemWorkStates.UnDefined;

        }
        //=======================================================================
        public CSystemWorkModeSL()
        {
            this.DefaultStyleKey = typeof(CSystemWorkModeSL);
        }
    }
}

