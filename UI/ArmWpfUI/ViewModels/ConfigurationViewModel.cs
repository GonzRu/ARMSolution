using ArmWpfUI.Converters;
using ArmWpfUI.Views;
using ArmWpfUI.Views.DeviceViews;
using ConfigurationParsersLib;
using CoreLib.Models.Configuration;
using SilverlightControlsLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using UICore.Commands;

namespace ArmWpfUI.ViewModels
{
    internal sealed class ConfigurationViewModel : UICore.ViewModels.ConfigurationViewModel
    {
        #region Public properties

        #region Properties

        /// <summary>
        /// Содержимое главного фрейма
        /// </summary>
        public object MainFrameContent
        {
            get { return _mainFrameContent; }
            set
            {
                _mainFrameContent = value;
                NotifyPropertyChanged("MainFrameContent");
            }
        }
        private object _mainFrameContent;

        /// <summary>
        /// Заголовок текущей страницы
        /// </summary>
        public string PageHeader
        {
            get { return _pageHeader; }
            set
            {
                _pageHeader = value;
                NotifyPropertyChanged("PageHeader");
            }
        }
        private string _pageHeader;

        #endregion

        #region Commands

        #region Команды навигации

        /// <summary>
        /// Переход назад
        /// </summary>
        public Command GoBackCommand { get; set; }

        /// <summary>
        /// Переход к первой(главной) странице
        /// </summary>
        public Command GotoFirstPageCommand { get; set; }

        /// <summary>
        /// Перейти на другую мнемосхему
        /// </summary>
        public Command GotoMnemoCommand { get; set; }

        /// <summary>
        /// Перейти на страницу событий
        /// </summary>
        public Command GotoEventsViewCommand { get; set; }

        /// <summary>
        /// Перейти на страницу блока
        /// </summary>
        public Command GotoTerminalViewCommand { get; set; }

        #endregion

        /// <summary>
        /// Меняет состояние указанного баннера
        /// </summary>
        public AsyncCommand ChangeBannerStateAsyncCommand { get; set; }

        #endregion

        #endregion

        #region Private fields

        /// <summary>
        /// Стек страниц для обратной навигации
        /// </summary>
        private readonly Stack<Tuple<string, object>> _backNavigationPagesStack = new Stack<Tuple<string, object>>(); 

        /// <summary>
        /// Кешированные страницы мнемосхем
        /// </summary>
        private readonly Dictionary<string, object> _mnemoPagesCache = new Dictionary<string, object>();

        #endregion

        #region Constructor

        public ConfigurationViewModel(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
            LoadConfiguration();

            #region Иницилизация комманд

            GoBackCommand = new Command(GoBack, false);
            GotoFirstPageCommand = new Command(GotoFirstPage, false);

            GotoMnemoCommand = new Command(GotoMnemo);
            GotoEventsViewCommand = new Command(GotoEventsView);
            GotoTerminalViewCommand = new Command(GotoTerminalView);

            ChangeBannerStateAsyncCommand = new AsyncCommand(ChangeBannerState);

            #endregion

            Configuration.DsRouterProvider.ConnectionStateChanged += b => NotifyPropertyChanged("IsConnectionStateOpened");

            //GotoMnemo("Mnemo.xml");

            LoadMainMnemo();
        }

        #endregion

        #region Private metods

        #region Commands Implementations

        #region Работа с представлением главного окна

        /// <summary>
        /// Переводит состояние фрейма на одну страницу назад
        /// </summary>
        private void GoBack()
        {
            if (_backNavigationPagesStack.Count == 0)
                return;

            var prevPage = _backNavigationPagesStack.Pop();

            SetFrameContent(prevPage.Item1, prevPage.Item2, true);
        }

        /// <summary>
        /// Переводит состояние фрейма на главную страницу
        /// </summary>
        private void GotoFirstPage()
        {
            if (_backNavigationPagesStack.Count == 0)
                return;

            var prevPage = _backNavigationPagesStack.ToArray()[_backNavigationPagesStack.Count - 1];
            _backNavigationPagesStack.Clear();

            SetFrameContent(prevPage.Item1, prevPage.Item2, true);
        }

        /// <summary>
        /// Загружает указанную мнемосхему и переходит на нее
        /// </summary>
        private void GotoMnemo(object param)
        {
            if (!(param is string) || String.IsNullOrWhiteSpace(param as string))
                return;

            var xamlFileName = param as string;
            var frameContent = LoadXaml(xamlFileName);

            SetFrameContent(xamlFileName, frameContent);
        }

        /// <summary>
        /// Переходит к окну событий
        /// </summary>
        private void GotoEventsView()
        {
            var eventsViewModel = new EventsView();
            eventsViewModel.DataContext = new EventsPageViewModel(Configuration.DsRouterProvider);

            SetFrameContent("События", eventsViewModel);
        }

        /// <summary>
        /// Переходит у окну терминала
        /// </summary>
        private void GotoTerminalView(object param)
        {
            var terminalsId = param as string;
            if (terminalsId == null)
                return;

            var terminalViewContent = new DeviceView();
            var terminals = terminalsId.Split(';');

            if (terminals.Length == 1)
            {
                var deviceViewModel = GetDeviceViewModel(terminals[0]);
                terminalViewContent.DataContext = deviceViewModel;

                SetFrameContent(deviceViewModel.DeviceDescription, terminalViewContent);
            }
            //else
            //{
            //    var selectTerminalWindow = new SelectTerminalView(terminalsId);
            //    selectTerminalWindow.ShowDialog();
            //}
        }

        #endregion

        #region Работа с конфигурацией

        /// <summary>
        /// Загружает конфигурацию
        /// </summary>
        protected override void LoadConfiguration()
        {
            base.LoadConfiguration();

            DataServers = new List<UICore.ViewModels.DataServerViewModel>();
            foreach (var ds in Configuration.DataServers.Values)
            {
                DataServers.Add(new DataServerViewModel(ds, Configuration.DsRouterProvider));
            }

            App.Configuration = Configuration;
        }

        #endregion

        private void ChangeBannerState(object param)
        {
            if (!(param is Tuple<string, int>))
                return;

            var p = param as Tuple<string, int>;
            var bannerTagId = p.Item1;
            var bannerId = p.Item2;

            var tagViewModel = GetTagViewModel(bannerTagId);
            if (tagViewModel == null)
                return;

            var newBannerTagValue = (int)(float)tagViewModel.TagValueAsObject ^ (1 << (bannerId - 1));
            Configuration.DsRouterProvider.SetTagValueFromHMI(tagViewModel.DsGuid, tagViewModel.DeviceGuid, tagViewModel.TagGuid, (float)newBannerTagValue);
        }

        protected override void Authorization()
        {
            Configuration.DsRouterProvider.Authorization("alex", "s", false);
        }

        #endregion

        #region Загрузка xaml

        /// <summary>
        /// Загружает мнемосхему и привязывает все эелементы
        /// </summary>
        private object LoadXaml(string xamlFileName)
        {
            if (_mnemoPagesCache.ContainsKey(xamlFileName))
                return _mnemoPagesCache[xamlFileName];

            var pathTomainMnemo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project", "Mnemo", xamlFileName);
            string str1 = OpenData(pathTomainMnemo);// При сохранении Было переименовано для WEB, для редактирования - обратно
            string str = str1.Replace("clr-namespace:SilverlightControlsLibrary;assembly=SilverlightControlsLibrary", "clr-namespace:SilverlightControlsLibrary;assembly=WpfControlsLibrary");// При сохранении Было переименовано для WEB, для редактирования - обратно

            var grid = XamlReader.Parse(str) as Grid;
            grid.LayoutTransform = new ScaleTransform(1, 1);

            var tagsToSubscribe = new List<string>();

            foreach (var child in grid.Children)
            {
                var cc = child as CBaseControl;
                if (cc == null)
                    continue;

                cc.ASUControlISSelected = false;

                MenuItem handleDeviceStateMenuItem = null;
                MenuItem deviceCommandsMenuItem = null;
                MenuItem bannersMenuItem = null;

                #region Привязанные устройства - ASUDeviceIDs

                if (!cc.ASUDeviceIDs.Equals("-1"))
                {
                    cc.Command = GotoTerminalViewCommand;
                    cc.CommandParameter = cc.ASUDeviceIDs;
                }

                #endregion

                #region DeviceState - ASUCommutationDeviceState

                if (!cc.ASUTagIDState.Equals("-1"))
                {
                    CreateBinding(cc, CBaseControl.ASUCommutationDeviceStateProperty, cc.ASUTagIDState, "TagValue", new AnalogTagValueToDeviceStateConverter());
                    if (!tagsToSubscribe.Contains(cc.ASUTagIDState))
                        tagsToSubscribe.Add(cc.ASUTagIDState);

                    #region Ручное управление - ASUCommutationDeviceStateManualSetEnabled

                    if (cc.ASUCommutationDeviceStateManualSetEnabled)
                    {
                        handleDeviceStateMenuItem = new MenuItem();
                        handleDeviceStateMenuItem.Header = "Ручное управление";

                        #region Включить

                        var setHandleStateOnMenuItem = new MenuItem
                        {
                            Header = "Ручной режим: Вкл",
                            Command = HandleSetOnDeviceStateCommand,
                            CommandParameter = cc.ASUTagIDState
                        };
                        CreateBinding(setHandleStateOnMenuItem, MenuItem.VisibilityProperty, cc.ASUTagIDState, "TagValue", new DeviceStateAndHandleQualityToVisibilityConverter { DeviceState = 2, InvertHandleQuality = true }, null);

                        #endregion

                        #region Отключить

                        var setHandleStateOffMenuItem = new MenuItem
                        {
                            Header = "Ручной режим: Выкл",
                            Command = HandleSetOffDeviceStateCommand,
                            CommandParameter = cc.ASUTagIDState
                        };
                        CreateBinding(setHandleStateOffMenuItem, MenuItem.VisibilityProperty, cc.ASUTagIDState, "TagValue", new DeviceStateAndHandleQualityToVisibilityConverter { DeviceState = 1, InvertHandleQuality = true });

                        #endregion

                        #region Сбросить

                        var resetHandleStatefMenuItem = new MenuItem
                        {
                            Header = "Сброс ручного ввода",
                            Command = ReSetHandleDeviceStateCommand,
                            CommandParameter = cc.ASUTagIDState
                        };
                        CreateBinding(resetHandleStatefMenuItem, MenuItem.IsEnabledProperty, cc.ASUTagIDState, "TagValue", new HandledQualityToBooleanConverter());

                        #endregion

                        handleDeviceStateMenuItem.Items.Add(setHandleStateOnMenuItem);
                        handleDeviceStateMenuItem.Items.Add(setHandleStateOffMenuItem);
                        handleDeviceStateMenuItem.Items.Add(resetHandleStatefMenuItem);
                    }

                    #endregion

                    #region Управление выключателем - комманды устройства

                    deviceCommandsMenuItem = new MenuItem();
                    deviceCommandsMenuItem.Header = "Комманды";

                    if (!cc.ASUTagIDCommandOn.Equals("-1"))
                    {
                        var deviceCommandOnMenuItem = new MenuItem();
                        deviceCommandOnMenuItem.Header = "Включить выключатель";
                        CreateBinding(deviceCommandOnMenuItem, MenuItem.IsEnabledProperty, cc.ASUTagIDState, "TagValue", new HandledQualityToBooleanConverter { Invert = true });
                        CreateBinding(deviceCommandOnMenuItem, MenuItem.VisibilityProperty, cc.ASUTagIDState, "TagValue", new DeviceStateAndHandleQualityToVisibilityConverter { DeviceState = 2 });

                        deviceCommandsMenuItem.Items.Add(deviceCommandOnMenuItem);
                    }

                    if (!cc.ASUTagIDCommandOff.Equals("-1"))
                    {
                        var deviceCommandOffMenuItem = new MenuItem();
                        deviceCommandOffMenuItem.Header = "Отключить выключатель";
                        CreateBinding(deviceCommandOffMenuItem, MenuItem.IsEnabledProperty, cc.ASUTagIDState, "TagValue", new HandledQualityToBooleanConverter { Invert = true });
                        CreateBinding(deviceCommandOffMenuItem, MenuItem.VisibilityProperty, cc.ASUTagIDState, "TagValue", new DeviceStateAndHandleQualityToVisibilityConverter { DeviceState = 1 });

                        deviceCommandsMenuItem.Items.Add(deviceCommandOffMenuItem);
                    }

                    if (!cc.ASUTagIDCommandReady.Equals("-1"))
                    {

                    }

                    if (!cc.ASUTagIDCommandReceipt.Equals("-1"))
                    {

                    }

                    if (deviceCommandsMenuItem.Items.Count == 0)
                        deviceCommandsMenuItem = null;

                    #endregion
                }

                #endregion

                #region DeviceControlMode - ASUCommutationDeviceControlMode

                if (!cc.ASUTagIDControlMode.Equals("-1"))
                {
                    //CreateBinding(cc, CBaseControl.ASUCommutationDeviceControlModeProperty, cc.ASUTagIDControlMode, null, null);
                    if (!tagsToSubscribe.Contains(cc.ASUTagIDControlMode))
                        tagsToSubscribe.Add(cc.ASUTagIDControlMode);
                }

                #endregion

                #region BannersState - ASUBannersState

                if (!cc.ASUTagIDBanners.Equals("-1"))
                {
                    CreateBinding(cc, CBaseControl.ASUBannersStateProperty, cc.ASUTagIDBanners, "TagValueAsObject", null);
                    if (!tagsToSubscribe.Contains(cc.ASUTagIDBanners))  
                        tagsToSubscribe.Add(cc.ASUTagIDBanners);

                    bannersMenuItem = new MenuItem();
                    bannersMenuItem.Header = "Плакаты";

                    #region Banner1

                    var banner1MenuItem = new MenuItem();
                    banner1MenuItem.IsCheckable = true;
                    banner1MenuItem.Style = Application.Current.FindResource("Banner1MenuItemStyle") as Style;
                    banner1MenuItem.Command = ChangeBannerStateAsyncCommand;
                    banner1MenuItem.CommandParameter = new Tuple<string, int>(cc.ASUTagIDBanners, 1);
                    CreateBinding(banner1MenuItem, MenuItem.IsCheckedProperty, cc.ASUTagIDBanners, "TagValue", new AnalogTagValueToIsBannerSetOnBoolean { BannerId = 1 });
                    
                    #endregion

                    #region Banner2

                    var banner2MenuItem = new MenuItem();
                    banner2MenuItem.IsCheckable = true;
                    banner2MenuItem.Style = Application.Current.FindResource("Banner2MenuItemStyle") as Style;
                    banner2MenuItem.Command = ChangeBannerStateAsyncCommand;
                    banner2MenuItem.CommandParameter = new Tuple<string, int>(cc.ASUTagIDBanners, 2);
                    CreateBinding(banner2MenuItem, MenuItem.IsCheckedProperty, cc.ASUTagIDBanners, "TagValue", new AnalogTagValueToIsBannerSetOnBoolean { BannerId = 2 });

                    #endregion

                    #region Banner3

                    var banner3MenuItem = new MenuItem();
                    banner3MenuItem.IsCheckable = true;
                    banner3MenuItem.Style = Application.Current.FindResource("Banner3MenuItemStyle") as Style;
                    banner3MenuItem.Command = ChangeBannerStateAsyncCommand;
                    banner3MenuItem.CommandParameter = new Tuple<string, int>(cc.ASUTagIDBanners, 3);
                    CreateBinding(banner3MenuItem, MenuItem.IsCheckedProperty, cc.ASUTagIDBanners, "TagValue", new AnalogTagValueToIsBannerSetOnBoolean { BannerId = 3 });

                    #endregion

                    #region Banner4

                    var banner4MenuItem = new MenuItem();
                    banner4MenuItem.IsCheckable = true;
                    banner4MenuItem.Style = Application.Current.FindResource("Banner4MenuItemStyle") as Style;
                    banner4MenuItem.Command = ChangeBannerStateAsyncCommand;
                    banner4MenuItem.CommandParameter = new Tuple<string, int>(cc.ASUTagIDBanners, 4);
                    CreateBinding(banner4MenuItem, MenuItem.IsCheckedProperty, cc.ASUTagIDBanners, "TagValue", new AnalogTagValueToIsBannerSetOnBoolean { BannerId = 4 });

                    #endregion

                    #region Banner5

                    var banner5MenuItem = new MenuItem();
                    banner5MenuItem.IsCheckable = true;
                    banner5MenuItem.Style = Application.Current.FindResource("Banner5MenuItemStyle") as Style;
                    banner5MenuItem.Command = ChangeBannerStateAsyncCommand;
                    banner5MenuItem.CommandParameter = new Tuple<string, int>(cc.ASUTagIDBanners, 5);
                    CreateBinding(banner5MenuItem, MenuItem.IsCheckedProperty, cc.ASUTagIDBanners, "TagValue", new AnalogTagValueToIsBannerSetOnBoolean { BannerId = 5 });

                    #endregion

                    bannersMenuItem.Items.Add(banner1MenuItem);
                    bannersMenuItem.Items.Add(banner2MenuItem);
                    bannersMenuItem.Items.Add(banner3MenuItem);
                    bannersMenuItem.Items.Add(banner4MenuItem);
                    bannersMenuItem.Items.Add(banner5MenuItem);
                }

                #endregion

                #region ContentValue - ASUContentValue

                if (!cc.ASUTagIDAnyValue.Equals("-1"))
                {
                    if (cc is CCurrentDataDisplay)
                    {
                        //CreateBinding(cc, CBaseControl.ASUContentValueProperty, cc.ASUTagIDAnyValue, null);
                        if (!tagsToSubscribe.Contains(cc.ASUTagIDAnyValue))
                            tagsToSubscribe.Add(cc.ASUTagIDAnyValue);
                    }
                }

                #endregion

                #region ToolTip - ASUToolTipsTagIDs

                if (!cc.ASUToolTipsTagIDs.Equals("-1"))
                {
                    var tagIDs = cc.ASUToolTipsTagIDs.Split(';');
                    if (tagIDs.Length > 1)
                    {
                        StackPanel st = new StackPanel();
                        st.Margin = new Thickness(5);
                        st.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)80, (byte)80, (byte)80));
                        foreach (string tagIDFull in tagIDs)
                        {
                            if (!string.IsNullOrEmpty(tagIDFull))
                            {
                                var tmpTag = Configuration.GetTag(tagIDFull);
                                if (tmpTag == null)
                                    continue;

                                var currentDataControl = new CCurrentDataDisplay();
                                currentDataControl.ASUTextContentFontSize = 20;
                                currentDataControl.ASUTagIDAnyValue = tmpTag.TagGuid.ToString(CultureInfo.InvariantCulture);
                                currentDataControl.ASUTagName = tmpTag.TagName;
                                currentDataControl.ASUTagUOM = tmpTag is TagAnalog ? (tmpTag as TagAnalog).Dim : String.Empty;
                                currentDataControl.ASUTagType = tmpTag.GetTypeAsString();

                                //CreateBinding(cc, CBaseControl.ASUContentValueProperty, tagIDFull, null);
                                if (!tagsToSubscribe.Contains(tagIDFull))
                                    tagsToSubscribe.Add(tagIDFull);

                                st.Children.Add(currentDataControl);
                            }
                        }

                        ToolTipService.SetToolTip(cc, st);
                    }
                }

                #endregion

                #region Индикация ручного ввода - ASUControlISManualSetProperty

                if (cc.ASUCommutationDeviceStateManualSetEnabled)
                {
                    CreateBinding(cc, CBaseControl.ASUControlISManualSetProperty, cc.ASUTagIDState, "TagValue", new HandledQualityToBooleanConverter());
                }

                #endregion

                #region Context menu

                cc.ContextMenu = new ContextMenu();

                // Пункт ручного управления
                if (handleDeviceStateMenuItem != null)
                    cc.ContextMenu.Items.Add(handleDeviceStateMenuItem);

                // Пункт упрваления баннерами
                if (bannersMenuItem != null)
                    cc.ContextMenu.Items.Add(bannersMenuItem);

                // Пункт комманд управления выключателем
                if (deviceCommandsMenuItem != null)
                    cc.ContextMenu.Items.Add(deviceCommandsMenuItem);

                if (cc.ContextMenu.Items.Count == 0)
                    cc.ContextMenu = null;

                #endregion

                if (cc is CMnemoLinkArea)
                {
                    #region Commands

                    var c = child as CMnemoLinkArea;
                    c.Command = GotoMnemoCommand;
                    c.CommandParameter = c.ASUMnemoLinkFileName;

                    #endregion
                }
            }

            #region Zoom

            // Задаю прозрачный фон для того, чтобы в пустых местах грида работал mousewheel
            grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            grid.MouseWheel += (sender, args) =>
            {
                var CanvasScale = (ScaleTransform) grid.LayoutTransform;
                if (args.Delta > 0)
                {
                    if (CanvasScale.ScaleX < 2)
                    {
                        CanvasScale.ScaleX = CanvasScale.ScaleX * 1.01;
                        CanvasScale.ScaleY = CanvasScale.ScaleX * 1.01;
                    }
                }
                else
                {
                    if (CanvasScale.ScaleX > 0.5)
                    {
                        CanvasScale.ScaleX = CanvasScale.ScaleX / 1.01;
                        CanvasScale.ScaleY = CanvasScale.ScaleX / 1.01;
                    }
                }
            };

            #endregion

            #region Drag mnemo

            //grid.MouseMove += (sender, args) =>
            //{
            //    if (args.LeftButton == MouseButtonState.Pressed)
            //    {
            //        var renderTransform = grid.RenderTransform;
            //    }
            //};

            #endregion

            Configuration.DsRouterProvider.SubscribeToTagsValuesUpdate(tagsToSubscribe);

            _mnemoPagesCache.Add(xamlFileName, grid);

            return grid;
        }

        /// <summary>
        /// Считывает содержимое файла в строку
        /// </summary>
        private string OpenData(string strPath)
        {
            #region Открываем xml
            string data = String.Empty;
            if (File.Exists(strPath))
            {
                using (StreamReader sr = File.OpenText(strPath))
                {
                    try
                    {
                        data = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        data = ex.Message;
                    }
                }
            }
            return data;
            #endregion Открываем xml
        }

        private void CreateBinding(FrameworkElement fe, DependencyProperty dp, string tagGuidAsStr, string propertyPath, IValueConverter converter, object converterParameter = null)
        {
            var tagViewModel = GetTagViewModel(tagGuidAsStr);
            if (tagViewModel == null)
                return;

            var binding = new Binding
            {
                Source = tagViewModel,
                Path = new PropertyPath(propertyPath),
                Mode = BindingMode.OneWay,
                Converter = converter,
                ConverterParameter = converterParameter,
                IsAsync = true
            };

            fe.SetBinding(dp, binding);
        }

        /// <summary>
        /// Загружает главную мнемосхему и добавляет к ней поле вывода тревог
        /// </summary>
        private void LoadMainMnemo()
        {
            var grid = LoadXaml("Mnemo.xml") as Grid;
            PageHeader = "Главная мнемосхема";

            // Контрол вывода последних тревог
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            stackPanel.Children.Add(new Label { Content = "123123123" });
            stackPanel.Children.Add(grid);

            MainFrameContent = stackPanel;
        }

        #endregion

        #region Работа с содержимым фрейма

        /// <summary>
        /// Устанавливает содержимое фрейма
        /// </summary>
        private void SetFrameContent(string frameHeader, object frameContent, bool isBackNavigation = false)
        {
            if (!isBackNavigation)
                PushCurrentPageStateToStack();

            PageHeader = frameHeader;
            MainFrameContent = frameContent;

            AdjustNavigationCommandsStatus();
        }

        #endregion

        #region Методы для работы со стеком и кешированием страниц

        /// <summary>
        /// Устанавливает возможность запуска навигационных команад
        /// в зависимости от состояния стека страниц
        /// </summary>
        private void AdjustNavigationCommandsStatus()
        {
            if (_backNavigationPagesStack.Count == 0)
            {
                GoBackCommand.CanExecute = false;
                GotoFirstPageCommand.CanExecute = false;
            }
            else if (_backNavigationPagesStack.Count == 1)
            {
                GoBackCommand.CanExecute = true;
                GotoFirstPageCommand.CanExecute = true;
            }
        }

        /// <summary>
        /// Добавляет текущее состояние фрейма в стек
        /// </summary>
        private void PushCurrentPageStateToStack()
        {
            _backNavigationPagesStack.Push(new Tuple<string, object>(PageHeader, MainFrameContent));
        }

        #endregion

        #endregion
    }
}
