using System.Collections.Generic;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.Services.Interface
{
    internal interface IService<TDto>
    {
        TDto GetById(string id);
        Task<TDto> GetByIdAsync(string id);

        string GetDisplayName(string id);
        Task<string> GetDisplayNameAsync(string id);

        IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null);
        Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null);

        IEnumerable<TDto> GetAll(ServiceArgs args = null);
        Task<IEnumerable<TDto>> GetAllAsync(ServiceArgs args = null);

        IGridPageDto<TDto> GetAllByGridPage(ServiceArgs args = null);
        Task<IGridPageDto<TDto>> GetAllByGridPageAsync(ServiceArgs args = null);

        IGridPageDto<TDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase);

        TDto Save(TDto update);
        Task<TDto> SaveAsync(TDto update);

        bool Delete(string id);
        Task<bool> DeleteAsync(string id);
    }
}