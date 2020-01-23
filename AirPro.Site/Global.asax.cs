using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using AirPro.Logging;
using AirPro.Site.App_Start;
using AutoMapper;
using Dapper;

namespace AirPro.Site
{
    public class MvcApplication : HttpApplication
    {
        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static GenericIdentity SystemIdentity { get; } =
            new GenericIdentity("system@airprodiag.com");

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine { FileExtensions = new[] { "cshtml" } });

            Logger.Initialize(ConnectionString);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBindersConfig.RegisterBinders(ModelBinders.Binders);

            SqlClientConfig.EnableCodeFirstMigrations();

            GlobalConfiguration.Configuration.EnsureInitialized();

            Mapper.Initialize(cfg => cfg.AddProfiles("AirPro.Site", "AirPro.Service", "AirPro.Library"));
            
            LoadBuildInfo();

            try
            {
                // Start Application.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("Support.usp_ApplicationStart", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Load Exception.
            Exception ex = HttpContext.Current.Error;

            // Ignore Known Errors.
            if (ex.GetType() == typeof(CryptographicException)
                    && ex is ViewStateException
                    && ex.Message.Contains("This is an invalid webresource request")
                    && ex.Message.Contains("Padding is invalid and cannot be removed"))
            {
                HttpContext.Current.ClearError();
                return;
            }

            // Log Error.
            Logger.LogException(ex);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                // Check Session.
                if (Session.SessionID == null) return;

                // Clear Sessions.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("Support.usp_UserSessionEnd", new { SessionId = Session.SessionID }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #region Support Methods

        public static string BuildInfo => HttpContext.Current.Application["BuildInfo"].ToString();

        private void LoadBuildInfo()
        {
            string buildInfo;

            try
            {
                StringBuilder result = new StringBuilder();

                result.AppendLine("<div id='buildInfo'>");
                result.AppendLine($"<h4>Release Version:&nbsp;{ FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion }</h4>");
                result.AppendLine("<ul>");
                foreach (var info in SystemInfo().OrderBy(x => x.Name))
                {
                    result.AppendLine($"<li>{info.Name} v{info.Version} (Built: {info.BuildTime}) - {info.Copyright}</li>"
                        .Replace(" (Built: 1/1/2000 12:00:00 AM)", "").Replace(" 12:00:00 AM", ""));
                }
                result.AppendLine("</ul></div>");

                buildInfo = result.ToString();
            }
            catch (Exception ex)
            {
                buildInfo = ex.Message;
            }

            Application.Add("BuildInfo", buildInfo);
        }

        private List<AssemblyInfo> SystemInfo ()
        {
            List<AssemblyInfo> result = new List<AssemblyInfo>();

            try
            {
                AppDomain current = AppDomain.CurrentDomain;

                foreach (var assem in current.GetAssemblies())
                {
                    if (!assem.IsDynamic)
                    {
                        FileVersionInfo ver = FileVersionInfo.GetVersionInfo(assem.Location);

                        result.Add(new AssemblyInfo
                        {
                            Name = ver.InternalName,
                            Version = ver.FileVersion,
                            BuildTime = CalculateBuildTime(assem.GetName().Version.Build, assem.GetName().Version.Revision),
                            Copyright = ver.LegalCopyright
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Server.ClearError();
            }

            return result;
        }

        private class AssemblyInfo
        {
            public string Name { get; set; }
            public string Copyright { get; set; }
            public string Version { get; set; }
            public DateTime BuildTime { get; set; }
        }

        private DateTime CalculateBuildTime(int build, int revision)
        {
            var result = new DateTime(2000, 1, 1);

            if (build > 10)
                result = new DateTime(2000, 1, 1).AddDays(build).AddSeconds(revision * 2);

            return result;
        }
    }

    #endregion
}
