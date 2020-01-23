using System;
using System.Data;
using System.Data.SqlClient;
using AirPro.Logging.Interface;
using Dapper;

namespace AirPro.Logging
{
    public static partial class Logger
    {
        public static void LogRequest(IRequestLogEntryDto request)
        {
            try
            {
                // Log Request.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("Support.usp_SaveRequestLog", request, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
