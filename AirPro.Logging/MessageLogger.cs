using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace AirPro.Logging
{
    public static partial class Logger
    {
        public static async Task LogTextNotification(string destination, string message, string status)
        {
            // Create Connection.
            using (var conn = new SqlConnection(ConnectionString))
            {
                // Create Log.
                var log = new
                {
                    Destination = destination,
                    Body = ScrubLog(message),
                    StatusMessage = status
                };

                // Insert SQL.
                const string textLogSql = @"INSERT INTO Notification.Logs (Destination, Body, StatusMessage, CreatedDt) VALUES (@Destination, @Body, @StatusMessage, GETUTCDATE());";

                // Save Log.
                await conn.ExecuteAsync(textLogSql, log);
            }
        }

        public static async Task LogEmailNotification(string destination, string subj, string body, string status)
        {
            // Create Connection.
            using (var conn = new SqlConnection(ConnectionString))
            {
                // Create Log.
                var log = new
                {
                    Destination = destination,
                    Subject = subj,
                    Body = ScrubLog(body),
                    StatusMessage = status
                };

                // Insert SQL.
                const string textLogSql = @"INSERT INTO Notification.Logs (Destination, Subject, Body, StatusMessage, CreatedDt) VALUES (@Destination, @Subject, @Body, @StatusMessage, GETUTCDATE());";

                // Save Log.
                await conn.ExecuteAsync(textLogSql, log);
            }
        }

        private static string ScrubLog(string text)
        {
            if (text.StartsWith("Your security code is"))
                return text.Remove(text.Length - 6, 6) + "XXXXXX";
            return text;
        }
    }
}