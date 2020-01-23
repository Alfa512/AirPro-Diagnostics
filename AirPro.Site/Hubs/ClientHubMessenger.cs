using System;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Logging;
using Microsoft.AspNet.SignalR;

namespace AirPro.Site.Hubs
{
    public class ClientHubMessenger : HubMessengerBase
    {
        private IHubContext Hub => GlobalHost.ConnectionManager.GetHubContext<ClientHub>();

        #region Legacy Events - Toast Notifications Removed

        public async Task ScanRequestCreated(int? requestId = null, string requestUrl = null) => await Hub.Clients.All.scanRequestCreated(requestId);

        public async Task ScanRequestCompleted(int? requestId = null, string requestUrl = null) => await Hub.Clients.All.scanRequestCompleted(requestId);

        public async Task RepairCreated(int? repairId = null, string repairUrl = null) => await Hub.Clients.All.repairCreated(repairId);

        public async Task RepairCompleted(int? repairId = null, string billingUrl = null) => await Hub.Clients.All.repairCompleted(repairId);

        public async Task InvoiceCompleted(int? repairId = null, string invoiceUrl = null) => await Hub.Clients.All.invoiceCompleted(repairId);

        #endregion

        public async Task ScanRequestOutDated(Guid userGuid, int requestId)
        {
            try
            {
                // Load Connections.                
                var connections = (await GetConnections(null, $"/Request/Report/{requestId}")).Where(c => c.UserGuid != userGuid).Select(c => c.ConnectionGuid.ToString()).ToArray();
                
                // Update All Scan Requests.
                await Hub.Clients.Clients(connections).updateAllOutdatedScans(requestId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public async Task DownloadCompleted(Guid userGuid, string pageUrl, string buttonId)
        {
            try
            {
                // Load Connections.
                var connections = (await GetConnections(null, pageUrl)).Where(c => c.UserGuid == userGuid).Select(c => c.ConnectionGuid.ToString()).ToArray();

                // Send Notification.
                await Hub.Clients.Clients(connections).resetButton(buttonId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}