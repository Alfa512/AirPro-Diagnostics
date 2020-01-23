using System.Web.Mvc;

namespace AirPro.Site.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("UsersMoves", "Users", new { area = "Admin", controller = "Users", action = "Index" });
            context.MapRoute("ShopsMoves", "Shops", new { area = "Admin", controller = "Shops", action = "Index" });
        }
    }
}