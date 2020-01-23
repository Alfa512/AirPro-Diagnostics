using System;

namespace AirPro.Site.Errors
{
    public partial class Error500 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.StatusCode = 500;
            Response.WriteFile("~/Error/500.html");
        }
    }
}