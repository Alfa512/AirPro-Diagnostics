using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace AirPro.Site.Hubs
{
    public class HubMessengerBase
    {
        protected async Task<string[]> GetConnectionIds(Guid? shopGuid = null, string pageUrl = null)
        {
            return (await GetConnections(shopGuid, pageUrl)).Select(c => c.ConnectionGuid.ToString()).ToArray();
        }

        protected async Task<IEnumerable<HubConnectionDto>> GetConnections(Guid? shopGuid = null, string pageUrl = null)
        {
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                return (await conn.QueryAsync<HubConnectionDto>("Support.usp_GetConnections", new
                {
                    ShopGuid = shopGuid,
                    PageUrl = pageUrl
                }, null, null, CommandType.StoredProcedure)).ToArray();
            }
        }
    }
}