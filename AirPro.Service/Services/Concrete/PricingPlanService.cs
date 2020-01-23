using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class PricingPlanService : ServiceBase, IService<IPricingPlanDto>
    {
        public PricingPlanService(IServiceSettings settings) : base(settings)
        {

        }

        public IPricingPlanDto GetById(string id)
        {
            var planDictionary = new Dictionary<int, PricingPlanDto>();
            var result = Conn.Query<PricingPlanDto, PricingPlanLineItemDto, PricingPlanDto>("Billing.usp_GetPricingPlans @Offset, @Search, @PricingPlanId",
                    (p, c) =>
                    {
                        PricingPlanDto plan;

                        if (!planDictionary.TryGetValue(p.PricingPlanId, out plan))
                        {
                            plan = p;
                            plan.LineItems = new List<PricingPlanLineItemDto>();
                            planDictionary.Add(plan.PricingPlanId, plan);
                        }

                        if (c == null) return plan;

                        (plan.LineItems as List<PricingPlanLineItemDto>)?.Add(c);

                        return plan;
                    },
                    new { Offset = User.UserUtcOffset, Search = string.Empty, PricingPlanId = id },
                    splitOn: "PlanGroup")
                .FirstOrDefault();

            return result;
        }

        public Task<IPricingPlanDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            return GetById(id)?.PricingPlanName;
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return GetAll()?.Where(p => p.PricingPlanActiveInd)
                .Select(p =>
                    new KeyValuePair<string, string>(
                        p.PricingPlanId.ToString(),
                        !string.IsNullOrWhiteSpace(p.CurrencyName)
                            ? p.PricingPlanName + $" ({p.CurrencyName})"
                            : p.PricingPlanName))
                .ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPricingPlanDto> GetAll(ServiceArgs args = null)
        {
            var planDictionary = new Dictionary<int, PricingPlanDto>();
            var result = Conn.Query<PricingPlanDto, PricingPlanLineItemDto, PricingPlanDto>("Billing.usp_GetPricingPlans @Offset",
                    (p, c) =>
                    {
                        PricingPlanDto plan;

                        if (!planDictionary.TryGetValue(p.PricingPlanId, out plan))
                        {
                            plan = p;
                            plan.LineItems = new List<PricingPlanLineItemDto>();
                            planDictionary.Add(plan.PricingPlanId, plan);
                        }

                        if (c == null) return plan;

                        (plan.LineItems as List<PricingPlanLineItemDto>)?.Add(c);

                        return plan;
                    },
                    new { Offset = User.UserUtcOffset },
                    splitOn: "PlanGroup")
                .Distinct()
                .ToList();

            return result;
        }

        public Task<IEnumerable<IPricingPlanDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IPricingPlanDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IPricingPlanDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IPricingPlanDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Search Database.
            var planDictionary = new Dictionary<int, PricingPlanDto>();
            var rows = Conn.Query<PricingPlanDto, PricingPlanLineItemDto, PricingPlanDto>("Billing.usp_GetPricingPlans @Offset, @Search",
                    (p, c) =>
                    {
                        PricingPlanDto plan;

                        if (!planDictionary.TryGetValue(p.PricingPlanId, out plan))
                        {
                            plan = p;
                            plan.LineItems = new List<PricingPlanLineItemDto>();
                            planDictionary.Add(plan.PricingPlanId, plan);
                        }

                        if (c == null) return plan;

                        (plan.LineItems as List<PricingPlanLineItemDto>)?.Add(c);

                        return plan;
                    },
                    new { Offset = User.UserUtcOffset, Search = searchPhrase },
                    splitOn: "PlanGroup")
                .Distinct()
                .ToList();

            // Create Result.
            var result = new GridPageDto<IPricingPlanDto>
            {
                Current = pageNumber,
                Total = rows.Count
            };

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? rows.OrderBy(s => s.PricingPlanName) : rows.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page;
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IPricingPlanDto Save(IPricingPlanDto update)
        {
            // Load Type.
            var result = Mapper.Map<PricingPlanDto>(update);
            result.UpdateResult = new UpdateResultDto(false, "Unknown Error has Occurred, Please Refresh the Page and Try Again.");

            // Validate Update.
            if (string.IsNullOrWhiteSpace(update.PricingPlanName))
            {
                result.UpdateResult = new UpdateResultDto(false, "Pricing Plan Name is Required and can NOT be Empty.");
                return result;
            }

            try
            {
                // Update Pricing Plan.
                var pricingPlanId = Conn.ExecuteScalar("Billing.usp_SavePricingPlan @PricingPlanId, @PricingPlanName, @PricingPlanDescription, @CurrencyId, @PricingPlanActiveInd, @UserGuid", new
                {
                    PricingPlanId = update.PricingPlanId,
                    PricingPlanName = update.PricingPlanName,
                    PricingPlanDescription = update.PricingPlanDescription,
                    CurrencyId = update.CurrencyId,
                    PricingPlanActiveInd = update.PricingPlanActiveInd,
                    UserGuid = User.UserGuid
                });

                // Update Line Items.
                Conn.Execute("Billing.usp_SavePricingPlanLineItem @PricingPlanId, @PlanGroup, @TypeId, @DomesticCost, @EuropeanCost, @AsianCost",
                    update.LineItems.Select(i => new
                    {
                        PricingPlanId = pricingPlanId.ToString(),
                        PlanGroup = i.PlanGroup,
                        TypeId = i.TypeId,
                        DomesticCost = i.DomesticCost,
                        EuropeanCost = i.EuropeanCost,
                        AsianCost = i.AsianCost
                    }).ToList());

                // Load Updated Model.
                result = Mapper.Map<PricingPlanDto>(GetById(pricingPlanId.ToString()));

                // Set Update Result.
                var updateMessage = $"Pricing Plan '{ result.PricingPlanName }' { (update.PricingPlanId == result.PricingPlanId ? "Updated" : "Created") } Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                result.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<IPricingPlanDto> SaveAsync(IPricingPlanDto update)
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
