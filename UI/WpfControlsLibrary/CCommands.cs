using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SilverlightControlsLibrary
{
    public static class CCommands
    {
        public static ICommand GotoMnemoCommand = new RoutedCommand("GotoMnemoCommand", typeof(CMnemoLinkArea));
    }
}
