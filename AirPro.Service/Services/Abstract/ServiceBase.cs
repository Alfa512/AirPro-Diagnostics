using System.Data.SqlClient;
using System.Linq;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Services.Abstract
{
    public abstract class ServiceBase
    {
        protected SqlConnection Conn => Settings.SqlConnection;
        protected EntityDbContext Db => Settings.EntityContext;
        protected IServiceUser User => Settings.User;

        protected IServiceSettings Settings { get; }

        internal ServiceBase(IServiceSettings settings)
        {
            Settings = settings;
        }

        protected bool UserHasRoles(params ApplicationRoles[] role)
        {
            var roleIds = role.Select(r => r.GetEnumGuid()).ToList();
            return roleIds.Intersect(Settings.User.UserRoleGuids).Any();
        }
    }
}
