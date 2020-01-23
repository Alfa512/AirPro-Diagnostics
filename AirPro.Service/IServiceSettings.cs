using System.Data.SqlClient;
using System.Security.Principal;
using AirPro.Entities;

namespace AirPro.Service
{
    public interface IServiceSettings
    {
        string ConnectionString { get; set; }
        EntityDbContext EntityContext { get; set; }
        SqlConnection SqlConnection { get; set; }
        IIdentity Identity { get; set; }
        IServiceUser User { get; set; }
    }
}