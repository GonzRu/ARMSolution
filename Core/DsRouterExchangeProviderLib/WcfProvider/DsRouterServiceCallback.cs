using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsRouterExchangeProviderLib.WcfProvider
{
    class DsRouterServiceCallback : DSRouterService.IDSRouterCallback
    {
        void DSRouterService.IDSRouterCallback.NewErrorEvent(string codeDataTimeEvent)
        {
            throw new NotImplementedException();
        }

        void DSRouterService.IDSRouterCallback.Pong()
        {
            throw new NotImplementedException();
        }

        void DSRouterService.IDSRouterCallback.NotifyChangedTags(Dictionary<string, DSRouterService.DSRouterTagValue> lstChangedTags)
        {
            throw new NotImplementedException();
        }

        void DSRouterService.IDSRouterCallback.NotifyCMDExecuted(byte[] cmdarray)
        {
            throw new NotImplementedException();
        }
    }
}
