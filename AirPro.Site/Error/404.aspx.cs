using System;

namespace AirPro.Site.Errors
{
    public partial class Error404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.StatusCode = 404;
            Response.WriteFile("~/Error/404.html");
        }
    }
}