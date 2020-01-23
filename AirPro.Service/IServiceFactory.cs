using System.Collections.Generic;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service
{
    public interface IServiceFactory
    {
        IEnumerable<T> GetAll<T>(ServiceArgs args = null);
        Task<IEnumerable<T>> GetAllAsync<T>(ServiceArgs args = null);

        IGridPageDto<T> GetAllByGridPage<T>(ServiceArgs args = null);
        Task<IGridPageDto<T>> GetAllByGridPageAsync<T>(ServiceArgs args = null);

        IGridPageDto<T> GetAllByGridPage<T>(int pageNumber, int pageSize, string sort, string searchPhrase);

        string GetDisplayName<T>(string id);
        Task<string> GetDisplayNameAsync<T>(string id);

        IEnumerable<KeyValuePair<string, string>> GetDisplayList<T>(ServiceArgs args = null);
        Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync<T>(ServiceArgs args = null);

        T GetById<T>(string id);
        Task<T> GetByIdAsync<T>(string id);

        T Save<T>(T update);
        Task<T> SaveAsync<T>(T update);

        bool Delete<T>(string id);
        Task<bool> DeleteAsync<T>(string id);
    }
}