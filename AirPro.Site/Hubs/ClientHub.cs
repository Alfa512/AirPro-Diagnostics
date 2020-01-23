using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace AirPro.Site.Hubs
{
    [Authorize]
    public class ClientHub : HubBase
    {
        public string GetUserSettings(string controlId)
        {
            string result;

            var sql = @"SELECT SettingsJson FROM Access.UserPreferences WHERE UserGuid = @UserGuid AND ControlId = @ControlId";

            var param = new
            {
                UserGuid = Context.Request.User.Identity.GetUserId(),
                ControlId = controlId
            };

            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                result = conn.Query<string>(sql, param, null, true, null, CommandType.Text).FirstOrDefault();
            }

            return result;
        }

        public void SaveUserSettings(string controlId, string settings)
        {
            var param = new
            {
                UserGuid = Context.Request.User.Identity.GetUserId(),
                ControlId = controlId,
                SettingsJson = settings
            };

            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var update = conn.Execute("Access.usp_SaveUserPreferences", param, null,
                    null, CommandType.StoredProcedure);
            }
        }
    }
}