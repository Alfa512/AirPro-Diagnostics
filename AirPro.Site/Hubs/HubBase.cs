using System;
using System.Threading.Tasks;
using AirPro.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace AirPro.Site.Hubs
{
    public abstract class HubBase : Hub
    {
        public override Task OnConnected()
        {
            LogConnection(true);

            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            LogConnection(false);

            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            LogConnection(true);

            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.
            return base.OnReconnected();
        }

        private void LogConnection(bool connectionStart)
        {
            try
            {
                // Check Connection.
                if (Context?.ConnectionId == null) return;
                var connectionId = Guid.Parse(Context.ConnectionId);

                // Add/Update Connection Log.
                var userId = Context.User.Identity.GetUserId();
                var pageUrl = Context?.QueryString?["page"];
                Logger.LogConnection(connectionId, userId, pageUrl, connectionStart);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}