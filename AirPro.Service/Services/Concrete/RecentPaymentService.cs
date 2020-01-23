using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class RecentPaymentService : ServiceBase, IService<IRecentPaymentDto>
    {
        private readonly ShopService _shopService;

        public RecentPaymentService(IServiceSettings settings) : base(settings)
        {
            _shopService = new ShopService(settings);
        }

        public IRecentPaymentDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IRecentPaymentDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRecentPaymentDto> GetAll(ServiceArgs args = null)
        {
            // Load Options.
            var paymentsOptions = new
            {
                User.UserGuid,
                ShopGuid = args?["ShopGuid"]?.ToString(),
                Search = args?["SearchPhrase"]?.ToString()
            };

            // Load Repairs.
            Dictionary<int, RecentPaymentDto> paymentsDictionary = new Dictionary<int, RecentPaymentDto>();
            var payments = Conn.Query<RecentPaymentDto, int?, RecentPaymentDto>("Billing.usp_GetPaymentsGrid", 
                    (p, i) => 
                    {
                        if (!paymentsDictionary.TryGetValue(p.PaymentId, out var n))
                        {
                            n = p;
                            n.InvoiceIds = new List<int>();
                            paymentsDictionary.Add(n.PaymentId, n);
                        }

                        if (i.HasValue)
                            (n.InvoiceIds as List<int>)?.Add(i.Value);

                        return n;
                    }, paymentsOptions, null, true, "InvoiceId", null, CommandType.StoredProcedure)
                .Distinct()
                .ToList();

            // Return.
            return payments;
        }

        public Task<IEnumerable<IRecentPaymentDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRecentPaymentDto> GetAllByGridPage(ServiceArgs args = null)
        {
            // Load All.
            var payments = GetAll(args).ToList();

            // Paging.
            return payments.ToList().GetGridPageFromCollection(args);
        }

        public Task<IGridPageDto<IRecentPaymentDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRecentPaymentDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IRecentPaymentDto Save(IRecentPaymentDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IRecentPaymentDto> SaveAsync(IRecentPaymentDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
