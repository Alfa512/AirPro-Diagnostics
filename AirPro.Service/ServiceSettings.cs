using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using AirPro.Entities;
using Dapper;

namespace AirPro.Service
{
    internal class ServiceSettings : IServiceSettings
    {
        private string _connectionString;
        private SqlConnection _sqlConnection;
        private EntityDbContext _entityContext;

        private IIdentity _identity;
        private IServiceUser _user;

        public string ConnectionString
        {
            get => !string.IsNullOrEmpty(_connectionString) ? _connectionString : (_sqlConnection?.ConnectionString) ?? (_entityContext?.Database?.Connection?.ConnectionString) ?? throw new NullReferenceException("Connection String is Null.");
            set => _connectionString = value;
        }

        public SqlConnection SqlConnection
        {
            get => _sqlConnection = _sqlConnection ?? (_sqlConnection = _entityContext?.Database?.Connection as SqlConnection ?? new SqlConnection(ConnectionString));
            set => _sqlConnection = value;
        }

        public EntityDbContext EntityContext
        {
            get => _entityContext = _entityContext ?? (_entityContext = !string.IsNullOrEmpty(_connectionString) ? new EntityDbContext(ConnectionString) : new EntityDbContext());
            set => _entityContext = value;
        }

        public IIdentity Identity
        {
            get => _identity ?? throw new NullReferenceException("User Identity is Null.");
            set => _identity = value;
        }

        public IServiceUser User
        {
            get
            {
                if (_user != null) return _user;

                string name = Identity.Name;
                if(string.IsNullOrWhiteSpace(name))
                {
                    name = "system@airprodiag.com"; // For non authenticated users
                }

                _user = SqlConnection.Query("Access.usp_GetServiceUser",
                    new { UserName = name }, commandType: CommandType.StoredProcedure)
                    .Select(u =>
                    {
                        var result = new ServiceUser
                        {
                            UserGuid = u.UserGuid,
                            UserName = u.UserName,
                            TimeZoneInfoId = u.TimeZoneInfoId,
                            UserLockedOut = u.UserLockedOut
                        };

                        var roles = new List<Guid>();
                        if (!string.IsNullOrEmpty(u.UserRoleGuids))
                        {
                            foreach (var roleGuid in u.UserRoleGuids.Split(','))
                            {
                                Guid role;
                                if (Guid.TryParse(roleGuid, out role))
                                    roles.Add(role);
                            }
                        }
                        result.UserRoleGuids = roles.ToArray();

                        return result;
                    }).FirstOrDefault();

                if (_user == null)
                    throw new NullReferenceException("User Account Not Found.");

                if (_user.UserLockedOut)
                    throw new Exception(@"User Account is Locked Out.");

                return _user; 
            }
            set => _user = value;
        }
    }
}