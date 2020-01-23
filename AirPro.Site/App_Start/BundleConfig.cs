using System.Diagnostics;
using System.Web.Optimization;

namespace AirPro.Site
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (Debugger.IsAttached)
            {
                BundleTable.EnableOptimizations = true;
            }

            BundleTable.Bundles.UseCdn = true;

            #region Global Bundles

            bundles.Add(new StyleBundle("~/site/css").Include(
                "~/Content/bootstrap/css/bootstrap.css",
                "~/Content/bootstrap/css/bootstrap-theme.css",
                "~/Content/font-awesome/font-awesome.css",
                "~/Content/Site.css",
                "~/Content/Gridmvc.css",
                "~/Content/jquery.bootgrid/jquery.bootgrid.css"));

            bundles.Add(new ScriptBundle("~/site/js").Include(
                "~/Content/jquery/jquery.js",
                "~/Content/signalr/jquery.signalR.js",
                "~/Content/respond/respond.src.js",
                "~/Content/jquery.inputmask/jquery.inputmask.bundle.js",
                "~/Content/bootstrap/js/bootstrap.js",
                "~/Scripts/gridmvc.js",
                "~/Content/moment/moment.js",
                "~/Content/jquery.bootgrid/jquery.bootgrid.js"));

            bundles.Add(new ScriptBundle("~/user/js").Include(
                "~/Scripts/app/common.js",
                "~/Scripts/app/signalr.js",
                "~/Scripts/app/bootgridSettings.js"));

            bundles.Add(new ScriptBundle("~/knockout/js")
                .Include("~/Content/knockout/knockout.js")
                .IncludeDirectory("~/Scripts/knockout", "*.js"));

            #endregion

            #region Common Tool Bundles

            bundles.Add(new ScriptBundle("~/jqueryval/js").Include(
                "~/Content/jquery-validation/jquery.validate.js",
                "~/Content/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
                "~/Scripts/validators/requiredIfValidator.js"));

            bundles.Add(new ScriptBundle("~/chart/js")
                .Include("~/Content/chart.js/chart.js"));

            #endregion

            #region Page Specific Bundles

            bundles.Add(new ScriptBundle("~/repairs/js")
                .IncludeDirectory("~/Views/Repairs", "*.js"));

            bundles.Add(new ScriptBundle("~/request/js")
                .IncludeDirectory("~/Views/Request", "*.js"));

            bundles.Add(new ScriptBundle("~/client/registration/js")
                .IncludeDirectory("~/Views/Client", "*.js"));

            bundles.Add(new ScriptBundle("~/admin/access/js")
                .IncludeDirectory("~/Areas/Admin/Views/Access", "*.js"));

            bundles.Add(new ScriptBundle("~/admin/techprofile/js")
                .IncludeDirectory("~/Areas/Admin/Views/TechProfile", "*.js"));

            bundles.Add(new ScriptBundle("~/admin/requesttype/js")
                .IncludeDirectory("~/Areas/Admin/Views/RequestType", "*.js"));

            bundles.Add(new ScriptBundle("~/admin/inventory/js")
                .IncludeDirectory("~/Areas/Admin/Views/Inventory", "*.js"));

            #endregion
        }
    }
}