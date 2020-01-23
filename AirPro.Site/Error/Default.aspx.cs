using System;

namespace AirPro.Site.Errors
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.WriteFile("~/Error/Default.html");
        }
    }
}