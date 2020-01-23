using System.Web.Mvc;
using AirPro.Site.Attributes;
using AirPro.Site.Filters;

namespace AirPro.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
#if !DEBUG
            filters.Add(new RequireHttpsAttribute());
#endif
            filters.Add(new RequestLoggingAttribute());
            filters.Add(new NoCacheAttribute());
        }
    }
}
