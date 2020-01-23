using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Logging
{
    public static partial class Logger
    {
        public static void LogException(Exception exception)
        {
            LogException(ConnectionString, exception);
        }

        public static void LogException(Exception exception, object obj)
        {
            LogException(ConnectionString, exception, obj);
        }

        public static void LogException(string connectionString, Exception exception)
        {
            Initialize(connectionString);

            using (var conn = new SqlConnection(connectionString))
            {
                ProcessException(conn, exception);
            }
        }

        public static void LogException(string connectionString, Exception exception, object obj)
        {
            Initialize(connectionString);

            using (var conn = new SqlConnection(connectionString))
            {
                ProcessException(conn, exception, obj);
            }
        }

        private static void ProcessException(IDbConnection connection, Exception exception, object obj = null)
        {
            // Log Exceptions.
            var parent = 0;
            var ex = exception;
            while (ex != null)
            {
                // Load Object Info.
                string objInfo = null;
                try
                {
                    if (obj != null) objInfo = JsonConvert.SerializeObject(obj);
                }
                catch (Exception e)
                {
                    objInfo = $"Unable to Serialize Object: {e.StackTrace}";
                }

                // Create Log.
                var log = new
                {
                    ExceptionParentId = parent,
                    ExceptionMessage = ex.Message,
                    ExceptionStackTrace = ex.StackTrace,
                    ExceptionObjectInfo = objInfo
                };

                // Log Exceptions.
                parent = connection.ExecuteScalar<int>("Support.usp_SaveApplicationException", log, commandType: CommandType.StoredProcedure);

                // Load Inner Exception.
                ex = ex.InnerException;
            }
        }
    }
}
