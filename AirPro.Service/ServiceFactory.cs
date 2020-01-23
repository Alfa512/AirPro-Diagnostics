using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using AirPro.Entities;
using AirPro.Logging;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Interface;
using AutoMapper;

namespace AirPro.Service
{
    public class ServiceFactory : IServiceFactory
    {
        public IServiceUser User => Settings.User;
        private IServiceSettings Settings { get; } = new ServiceSettings();

        public ServiceFactory(string connectionString, IIdentity user) : this(user)
        {
            Settings.ConnectionString = connectionString;
            Logger.Initialize(Settings.ConnectionString);
        }

        public ServiceFactory(SqlConnection connection, IIdentity user) : this(user)
        {
            Settings.SqlConnection = connection;
            Logger.Initialize(Settings.ConnectionString);
        }

        public ServiceFactory(EntityDbContext context, IIdentity user) : this(user)
        {
            Settings.EntityContext = context;
            Logger.Initialize(Settings.ConnectionString);
        }

        private ServiceFactory(IIdentity user)
        {
            Settings.Identity = user;

            VerifyAutoMapper();
        }

        private static void VerifyAutoMapper()
        {
            try
            {
                // Check for Profiles.
                var profiles = (Mapper.Configuration as MapperConfiguration)?.Profiles;
                if (profiles?.Select(p => p.Name.StartsWith("AirPro.Service")).Any() ?? false) return;
            }
            catch { /* Above will fail if not initialized, we will do that below. */ }

            Mapper.Initialize(cfg => cfg.AddProfiles("AirPro.Service"));
        }

        private IService<T> GetService<T>()
        {
            var service = Assembly.GetExecutingAssembly()
                .GetTypes().FirstOrDefault(mytype => mytype.GetInterfaces().Contains(typeof(IService<T>)));

            if (service == null) throw new NotImplementedException();

            return (IService<T>)Activator.CreateInstance(service, args: Settings);
        }

        public IEnumerable<T> GetAll<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return service.GetAll(args);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return await service.GetAllAsync(args);
        }

        public IGridPageDto<T> GetAllByGridPage<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return service.GetAllByGridPage(args);
        }

        public async Task<IGridPageDto<T>> GetAllByGridPageAsync<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return await service.GetAllByGridPageAsync(args);
        }

        public IGridPageDto<T> GetAllByGridPage<T>(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var service = GetService<T>();

            return service.GetAllByGridPage(pageNumber, pageSize, sort, searchPhrase);
        }

        public string GetDisplayName<T>(string id)
        {
            var service = GetService<T>();

            return service.GetDisplayName(id);
        }

        public async Task<string> GetDisplayNameAsync<T>(string id)
        {
            var service = GetService<T>();

            return await service.GetDisplayNameAsync(id);
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return service.GetDisplayList(args);
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync<T>(ServiceArgs args = null)
        {
            var service = GetService<T>();

            return await service.GetDisplayListAsync(args);
        }

        public T GetById<T>(string id)
        {
            var service = GetService<T>();

            return service.GetById(id);
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var service = GetService<T>();

            return await service.GetByIdAsync(id);
        }

        public T Save<T>(T update)
        {
            var service = GetService<T>();

            return service.Save(update);
        }

        public async Task<T> SaveAsync<T>(T update)
        {
            var service = GetService<T>();

            return await service.SaveAsync(update);
        }

        public bool Delete<T>(string id)
        {
            var service = GetService<T>();

            return service.Delete(id);
        }

        public async Task<bool> DeleteAsync<T>(string id)
        {
            var service = GetService<T>();

            return await service.DeleteAsync(id);
        }
    }
}
