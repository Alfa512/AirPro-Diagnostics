using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Services.Concrete
{
    internal class OutstandingInvoiceService : ServiceBase, IService<IOutstandingInvoiceDto>
    {
        public OutstandingInvoiceService(IServiceSettings settings) : base(settings)
        {
        }

        public IOutstandingInvoiceDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IOutstandingInvoiceDto> GetByIdAsync(string id)
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

        public IEnumerable<IOutstandingInvoiceDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IOutstandingInvoiceDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IOutstandingInvoiceDto> GetAllByGridPage(ServiceArgs args = null)
        {
            Guid shopGuid = Guid.Empty;
            if ((args?.ContainsKey("ShopGuid") ?? false) &&
                Guid.TryParse(args["ShopGuid"]?.ToString(), out shopGuid))
            {
            }

            int currencyId = 0;
            if ((args?.ContainsKey("CurrencyId") ?? false) &&
                int.TryParse(args["CurrencyId"]?.ToString(), out currencyId))
            {
            }

            var shopParam = shopGuid == Guid.Empty
                ? new SqlParameter("@ShopGuid", DBNull.Value)
                : new SqlParameter("@ShopGuid", shopGuid);

            var currencyParam = currencyId <= 0
                ? new SqlParameter("@CurrencyId", DBNull.Value)
                : new SqlParameter("@CurrencyId", currencyId);
            var search = args?["SearchPhrase"]?.ToString();
            var sqlParams = new[]
            {
                shopParam,
                currencyParam,
                new SqlParameter("@Search", search)
            };

            var unpaidInvoices = Db.Database.SqlQuery<OutstandingInvoice>("Billing.usp_GetOutstandingInvoices @ShopGuid, @CurrencyId, @Search", sqlParams).ToList();

            // Localize Invoiced Date.
            unpaidInvoices.ForEach(e => e.InvoiceDateTime = e.InvoiceDateTime.ConvertToUserTime(User.TimeZoneInfoId));

            // Localize Repair Date.
            unpaidInvoices.ForEach(e => e.RepairCreatedDateTime = e.RepairCreatedDateTime.ConvertToUserTime(User.TimeZoneInfoId));

            return unpaidInvoices.ToList<IOutstandingInvoiceDto>().GetGridPageFromCollection(args);
        }

        public Task<IGridPageDto<IOutstandingInvoiceDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IOutstandingInvoiceDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IOutstandingInvoiceDto Save(IOutstandingInvoiceDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IOutstandingInvoiceDto> SaveAsync(IOutstandingInvoiceDto update)
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
