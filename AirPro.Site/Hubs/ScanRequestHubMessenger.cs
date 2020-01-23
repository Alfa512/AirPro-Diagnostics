using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace AirPro.Site.Hubs
{
    public class ScanRequestHubMessenger : HubMessengerBase
    {
        private readonly ServiceFactory _factory;

        private IHubContext Hub => GlobalHost.ConnectionManager.GetHubContext<ScanRequestHub>();

        public ScanRequestHubMessenger()
        {
            _factory = new ServiceFactory(MvcApplication.ConnectionString, MvcApplication.SystemIdentity);
        }

        public async Task NotifyScanCreated(int requestId)
        {
            var requestDto = await _factory.GetByIdAsync<IRequestDto>(requestId.ToString());
            if (requestDto == null) return;

            await Hub.Clients.Clients(await GetConnectionIds()).addScanRequest(requestDto);
        }

        public async Task NotifyScanUpdated(int requestId)
        {
            var requestDto = await _factory.GetByIdAsync<IRequestDto>(requestId.ToString());
            if (requestDto == null) return;

            await Hub.Clients.Clients(await GetConnectionIds()).updateScanRequest(requestDto);
        }

        public async Task NotifyScanRemoved(int requestId)
        {
            var requestDto = await _factory.GetByIdAsync<IRequestDto>(requestId.ToString());
            if (requestDto == null) return;

            await Hub.Clients.Clients(await GetConnectionIds()).removeScanRequest(requestDto);
        }
    }
}