using System;
using System.Collections.Generic;
using UICore.ViewModels;

namespace ARMConfigurator.ViewModels
{
    class TrendViewModel : BaseViewModel
    {
        public IEnumerable<Tuple<uint, object>> TrendSource { get; set; }

        public string TagName { get; set; }
    }
}
