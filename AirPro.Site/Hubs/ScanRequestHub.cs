using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AirPro.Site.Models.Hubs;
using Dapper;
using Microsoft.AspNet.Identity;

namespace AirPro.Site.Hubs
{
    public class ScanRequestHub : HubBase
    {
        public IEnumerable<TechnicianScheduleModel> GetSchedules()
        {
            var result = new List<TechnicianScheduleModel>();
            var param = new
            {
                UserGuid = Context.Request.User.Identity.GetUserId()
            };

            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var schedule = conn.Query<TechnicianScheduleModel>("Technician.usp_GetWeeklySchedule", param, null,
                    true, null, CommandType.StoredProcedure).ToList();
                result.AddRange(schedule.Where(s => s.StartTime < DateTimeOffset.UtcNow && s.EndTime > DateTimeOffset.UtcNow));
            }

            return result;
        }

        public IEnumerable<TechncianConnectionModel> GetConnections()
        {
            var result = new List<TechncianConnectionModel>();
            var param = new
            {
                UserGuid = Context.Request.User.Identity.GetUserId()
            };

            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                result.AddRange(conn.Query<TechncianConnectionModel>("Technician.usp_GetQueueConnections", param, null, true, null, CommandType.StoredProcedure).ToList());
            }

            return result;
        }
    }
}