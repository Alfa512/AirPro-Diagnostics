using AirPro.Common.Enumerations;
using AirPro.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AirPro.Service;

namespace AirPro.Site
{
    public enum SelectListItemCacheType
    {
        RequestType,
        RepairStatus,
        WarningIndicator,
        InsuranceCompany,
        RequestTypeCategory,
        BillingCurrency
    }

    public static class SelectListItemCache
    {
        private const string RequestTypeCacheKey = "RequestTypeSelection";
        private const string RepairStatusCacheKey = "RepairStatusSelection";
        private const string WarningIndicatorCacheKey = "WarningIndicatorSelection";
        private const string InsuranceCompanyCacheKey = "InsuranceCompanySelection";
        private const string RequestTypeCategoryCacheKey = "RequestTypeCategorySelection";
        private const string BillingCurrencySelectionCacheKey = "BillingCurrencySelection";

        public static void ClearCache(SelectListItemCacheType type)
        {
            if (System.Web.HttpContext.Current?.Cache == null) return;

            switch (type)
            {
                case SelectListItemCacheType.RequestType:
                    System.Web.HttpContext.Current.Cache.Remove(RequestTypeCacheKey);
                    break;
                case SelectListItemCacheType.RepairStatus:
                    System.Web.HttpContext.Current.Cache.Remove(RepairStatusCacheKey);
                    break;
                case SelectListItemCacheType.WarningIndicator:
                    System.Web.HttpContext.Current.Cache.Remove(WarningIndicatorCacheKey);
                    break;
                case SelectListItemCacheType.InsuranceCompany:
                    System.Web.HttpContext.Current.Cache.Remove(InsuranceCompanyCacheKey);
                    break;
                case SelectListItemCacheType.RequestTypeCategory:
                    System.Web.HttpContext.Current.Cache.Remove(RequestTypeCategoryCacheKey);
                    break;
                case SelectListItemCacheType.BillingCurrency:
                    System.Web.HttpContext.Current.Cache.Remove(BillingCurrencySelectionCacheKey);
                    break;
            }
        }

        public static IEnumerable<SelectListItem> RepairStatusSelectItems()
        {
            // Load Cache.
            var repairStatuses = System.Web.HttpContext.Current?.Cache?[RepairStatusCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (repairStatuses?.Any() ?? false) return repairStatuses;

            // Load Select List.
            repairStatuses = new List<SelectListItem>();
            foreach (RepairStatuses r in Enum.GetValues(typeof(RepairStatuses)))
            {
                repairStatuses.Add(new SelectListItem() { Value = r.ToString(), Text = Enum.GetName(typeof(RepairStatuses), r) });
            }

            // Save to Cache.
            if ((repairStatuses?.Any() ?? false) && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[RepairStatusCacheKey] = repairStatuses;

            // Return.
            return repairStatuses;
        }

        public static IEnumerable<SelectListItem> RequestTypeSelectItems(Guid? shopGuid = null)
        {
            // Load Cache.
            var requestTypes = System.Web.HttpContext.Current?.Cache?[RequestTypeCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (requestTypes?.Any() ?? false) return FilterRequestTypes(requestTypes, shopGuid);

            using (var context = new EntityDbContext())
            {
                // Load List From Database.
                requestTypes = context.ScanRequestTypes?.Where(t => t.ActiveFlag)
                    .OrderBy(t => t.SortOrder)
                    .Where(x => x.RequestTypeId != 6 && x.RequestTypeId != 7) // Exclude Self Scan & Scan Analysis.
                    .Select(t => new SelectListItem { Value = t.RequestTypeId.ToString(), Text = t.TypeName })
                    .ToList();
            }

            // Save to Cache.
            if ((requestTypes?.Any() ?? false) && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[RequestTypeCacheKey] = requestTypes;

            // Return.
            return FilterRequestTypes(requestTypes, shopGuid);
        }

        private static IEnumerable<SelectListItem> FilterRequestTypes(List<SelectListItem> requestTypes, Guid? shopGuid = null)
        {
            using (var context = new EntityDbContext())
            {
                List<int> allowedRequestTypeIds = new List<int>();
                if (shopGuid != null)
                {
                    allowedRequestTypeIds = context.ShopRequestTypes.Where(x => x.ShopGuid == shopGuid).Select(x => x.RequestTypeId).ToList();

                    requestTypes = requestTypes.Where(x => allowedRequestTypeIds.Any(y => y.ToString() == x.Value)).ToList();
                }
            }

            return requestTypes;
        }

        public static IEnumerable<SelectListItem> RequestTypeCategorySelectItems()
        {
            // Load Cache.
            var requestTypeCategories = System.Web.HttpContext.Current?.Cache?[RequestTypeCategoryCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (requestTypeCategories?.Any() ?? false) return requestTypeCategories;

            using (var context = new EntityDbContext())
            {
                // Load List From Database.
                requestTypeCategories = context.ScanRequestCategoryTypes.AsQueryable()
                    .Include(t => t.Category)
                    .Where(t => t.Category.IsActive)
                    .OrderBy(t => t.Category.Order)
                    .Select(t => new SelectListItem
                    {
                        Value = t.RequestCategoryId.ToString(),
                        Text = t.Category.RequestCategoryName,
                        Group = new SelectListGroup
                        {
                            Name = t.RequestTypeId.ToString()
                        }
                    }).ToList();

                var allTypeCategories = context.ScanRequestCategories.Where(t => t.IsActive)
                    .OrderBy(t => t.Order)
                    .Select(t => new SelectListItem
                    {
                        Value = t.RequestCategoryId.ToString(),
                        Text = t.RequestCategoryName,
                        Group = new SelectListGroup
                        {
                            Name = "0"
                        }
                    }).ToList();

                requestTypeCategories = requestTypeCategories.Concat(allTypeCategories).ToList();
            }

            // Save to Cache.
            if (requestTypeCategories.Any() && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[RequestTypeCategoryCacheKey] = requestTypeCategories;

            // Return.
            return requestTypeCategories;
        }

        public static IEnumerable<SelectListItem> WarningIndicatorSelectItems()
        {
            // Load Cache.
            var warningIndicators =
                System.Web.HttpContext.Current?.Cache[WarningIndicatorCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (warningIndicators?.Any() ?? false) return warningIndicators;

            // Load From Database.
            using (var context = new EntityDbContext())
            {
                warningIndicators = context.ScanWarningIndicators?.OrderBy(i => i.Name)
                    .Select(i => new SelectListItem { Value = i.WarningIndicatorId.ToString(), Text = i.Name }).ToList();
            }

            // Save to Cache.
            if ((warningIndicators?.Any() ?? false) && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[WarningIndicatorCacheKey] = warningIndicators;

            // Return.
            return warningIndicators;
        }

        public static IEnumerable<SelectListItem> InsuranceCompanySelectItems()
        {
            // Load Cache.
            var insuranceCompanies =
                System.Web.HttpContext.Current?.Cache[InsuranceCompanyCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (insuranceCompanies?.Any() ?? false) return insuranceCompanies;

            // Load From Database.
            using (var context = new EntityDbContext())
            {
                insuranceCompanies = context.InsuranceCompanies?.OrderBy(i => i.InsuranceCompanyName).Where(i => !i.DisabledInd)
                    .Select(i => new SelectListItem { Value = i.InsuranceCompanyId.ToString(), Text = i.InsuranceCompanyName }).ToList();
            }

            // Save to Cache.
            if ((insuranceCompanies?.Any() ?? false) && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[InsuranceCompanyCacheKey] = insuranceCompanies;

            // Return.
            return insuranceCompanies;
        }

        public static IEnumerable<SelectListItem> BillingCurrencySelectItems()
        {
            // Load Cache.
            var currencies =
                System.Web.HttpContext.Current?.Cache[BillingCurrencySelectionCacheKey] as List<SelectListItem>;

            // Check Cache.
            if (currencies?.Any() ?? false) return currencies;

            // Load From Database.
            using (var context = new EntityDbContext())
            {
                currencies = context.Currencies?
                    .Select(i => new SelectListItem { Value = i.CurrencyId.ToString(), Text = i.Name }).ToList();
            }

            // Save to Cache.
            if ((currencies?.Any() ?? false) && System.Web.HttpContext.Current?.Cache != null)
                System.Web.HttpContext.Current.Cache[BillingCurrencySelectionCacheKey] = currencies;

            // Return.
            return currencies;
        }
    }
}