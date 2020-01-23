using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site
{
    public class BaseController : Controller
    {
        private ServiceFactory _factory;
        protected ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        protected IEnumerable<KeyValuePair<string, string>> GetStateSelectList()
        {
            var states = Factory.GetDisplayList<IStateDto>().ToList();
            states.Insert(0, new KeyValuePair<string, string>("", @"<-- Select State -->"));

            return states;
        }

        protected IEnumerable<KeyValuePair<string, string>> GetBillingCycleSelectList()
        {
            var cycles = Factory.GetDisplayList<IBillingCycleDto>().ToList();
            cycles.Insert(0, new KeyValuePair<string, string>("", @"<-- Select Billing Cycle -->"));

            return cycles;
        }

        protected List<SelectListItem> GetTypeSelection(ICollection<KeyValuePair<string, string>> collection = null, IEnumerable<Guid> selectedOptions = null)
        {
            if (collection == null)
            {
                collection = Factory.GetDisplayList<IGroupDto>().ToList();
            }

            var elements = collection.OrderBy(g => g.Value)
                .Select(g => new SelectListItem
                {
                    Value = g.Key,
                    Text = g.Value,
                    Selected = selectedOptions?.Any(d => d.ToString() == g.Key) ?? false
                }).ToList();

            return elements;
        }

        protected List<SelectListItem> GetInsuranceCompanies()
        {
            var defInsCos = SelectListItemCache.InsuranceCompanySelectItems()?.OrderBy(m => m.Value).ToList();
            defInsCos?.Insert(0, new SelectListItem { Text = @"<-- Not Assigned -->", Value = "0" });
            return defInsCos;
        }
    }
}