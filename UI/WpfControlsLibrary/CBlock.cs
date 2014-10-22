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
    [Description("Замок - блокировка при местном управлении")]
    public class CBlock: CBaseControl
    {

        //=======================================================================
        //=======================================================================

        [Category("Свойства элемента мнемосхемы"), Description("Текущее состояние блокировки."), Browsable(false)]
        public ASUBlockStates ASUBlockState
        {
            get { return (ASUBlockStates)GetValue(ASUBlockStateProperty); }
            set
            {
                SetValue(ASUBlockStateProperty, value);
                OnRaiseASUBlockStateEvent(new EventArgsASUBlockState(value));
            }
        }
        public static DependencyProperty ASUBlockStateProperty = DependencyProperty.Register("ASUBlockState", typeof(ASUBlockStates), typeof(CBlock), new PropertyMetadata(ASUBlockStates.UnDefined, OnASUBlockStatePropertyChanged));
        private static void OnASUBlockStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region режим управления
            CBlock cis = d as CBlock;
            if (cis != null)
            {
                cis.ASUBlockState = (ASUBlockStates)e.NewValue;
                switch (cis.ASUBlockState)
                {
                    case ASUBlockStates.Locked:
                        VisualStateManager.GoToState(cis, "Locked", false);
                        break;
                    case ASUBlockStates.UnLocked:
                        VisualStateManager.GoToState(cis, "UnLocked", false);
                        break;

                    default://ASUBlockStates.Undefined или Distance
                        VisualStateManager.GoToState(cis, "Undefined", false);
                        break;
                }
            }
            #endregion режим управления
        }

        public class EventArgsASUBlockState : EventArgs
        {
            #region Класс, аргумент события, содержащий состояние блокировки
            private ASUBlockStates asuBlockState;
            public ASUBlockStates ASUBlockState
            {
                get { return asuBlockState; }
                set { asuBlockState = value; }
            }
            public EventArgsASUBlockState(ASUBlockStates ABlockState)
            {
                asuBlockState = ABlockState;
            }
            #endregion Класс, аргумент события, содержащий состояние блокировки
        }
        public event EventHandler<EventArgsASUBlockState> OnASUBlockStateChange;
        protected virtual void OnRaiseASUBlockStateEvent(EventArgsASUBlockState e)
        {
            #region Событие при изменении состояния блокировки
            //Упаковываем вызов события в protected virtual метод чтобы позволить наследникам перекрывать его
            // Создаем временную копию события, чтобы корректно обработать ситуацию, когда последний подписчик события отписался от него сразу после 
            // проверки if (handler != null), но еще до того как событие наступило.
            EventHandler<EventArgsASUBlockState> handler = OnASUBlockStateChange;

            // Если нет ни одного подписчика, handler == null
            if (handler != null)
            {
                // Можно еще что-нибудь сделать с аргументами  
                //e.Message = "Скачан файл:     " + e.Message;

                // Запускаем событие
                handler(this, e);
            }
            #endregion Событие при изменении состояния блокировки
        }
        //==============================================
       
        public CBlock()
        {
            this.DefaultStyleKey = typeof(CBlock);
        }


    }


}



