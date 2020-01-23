using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AirPro.Site.Startup))]

namespace AirPro.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
