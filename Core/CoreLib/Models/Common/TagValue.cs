using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Models.Configuration;

namespace CoreLib.Models.Common
{
    public class TagValue
    {
        public object TagValueAsObject { get; set; }

        public TagValueQuality TagValueQuality { get; set; }
    }
}
