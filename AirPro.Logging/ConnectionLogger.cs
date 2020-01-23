using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace AirPro.Logging
{
    public static partial class Logger
    {
        public static void LogConnection(Guid connectionId, string userId, string pageUrl, bool connectionStart)
        {
            try
            {
                // Add/Update Connection Log.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("Support.usp_SaveConnection", new
                    {
                        ConnectionGuid = connectionId,
                        UserGuid = userId,
                        PageUrl = pageUrl,
                        ConnectionStartInd = connectionStart
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
