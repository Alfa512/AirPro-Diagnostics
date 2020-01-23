using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class InsuranceCompanyService : ServiceBase, IService<IInsuranceCompanyDto>
    {

        public InsuranceCompanyService(IServiceSettings settings) : base(settings)
        {
        }

        public IInsuranceCompanyDto GetById(string id)
        {
            var entityId = Convert.ToInt32(id);
            IInsuranceCompanyDto result;

            using (var reader = Conn.QueryMultiple("Repair.usp_GetInsuranceCompanies", new {search = string.Empty, insuranceCompanyId = entityId}, commandType: CommandType.StoredProcedure))
            {
                result = reader.Read<InsuranceCompanyDto>().FirstOrDefault() ?? new InsuranceCompanyDto();
                result.CccInsuranceCompanyIds = reader.Read<CCCInsuranceCompanyDto>().Select(x => x.CCCInsuranceCompanyId).ToList();
            }

            return result;
        }

        public Task<IInsuranceCompanyDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return GetAll()?.OrderBy(p => p.InsuranceCompanyName).Where(x => !x.DisabledInd).Select(p => new KeyValuePair<string, string>(p.InsuranceCompanyId.ToString(), p.InsuranceCompanyName)).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IInsuranceCompanyDto> GetAll(ServiceArgs args = null)
        {
            return Mapper.Map<IEnumerable<IInsuranceCompanyDto>>(Db.InsuranceCompanies.ToList());
        }

        public Task<IEnumerable<IInsuranceCompanyDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IInsuranceCompanyDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IInsuranceCompanyDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IInsuranceCompanyDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var rows = Conn.Query<InsuranceCompanyDto>("Repair.usp_GetInsuranceCompanies @search", new { search = searchPhrase })
                .Distinct()
                .ToList();

            // Create Result.
            var result = new GridPageDto<IInsuranceCompanyDto>
            {
                Current = pageNumber,
                Total = rows.Count
            };

            var sorted = !string.IsNullOrEmpty(sort) 
                ? rows.OrderBy(sort) 
                : rows.OrderBy(d => d.InsuranceCompanyName);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page;
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IInsuranceCompanyDto Save(IInsuranceCompanyDto update)
        {
            // Load Type.
            var result = update;

            if (!UserHasRoles(ApplicationRoles.InsuranceCoAdmin))
            {
                // No Group Access.
                result.UpdateResult = new UpdateResultDto(false, "You do not have access to Insurance Companies.");
                return result;
            }

            // Validate Update.
            if (string.IsNullOrWhiteSpace(update.InsuranceCompanyName))
            {
                result.UpdateResult = new UpdateResultDto(false, "Company Name is Required and can NOT be Empty.");
                return result;
            }
            
            try
            {
                // Create Params.
                var param = new
                {
                    InsuranceCompanyId = update.InsuranceCompanyId,
                    InsuranceCompanyName = update.InsuranceCompanyName,
                    InsuranceCompanyCccIds = string.Join(",", update.CccInsuranceCompanyIds ?? new List<string>()),
                    ProgramName = update.ProgramName,
                    DisabledInd = update.DisabledInd
                };

                // Execute Update.
                var exec = Conn.Query<InsuranceCompanyDto>(
                    "Repair.usp_SaveInsuranceCompany @InsuranceCompanyId, @InsuranceCompanyName, @InsuranceCompanyCccIds, @ProgramName, @DisabledInd",
                    param).ToList();

                // Load Updated Model.
                result = exec.FirstOrDefault();

                if (result == null)
                {
                    update.UpdateResult = new UpdateResultDto(false, "Error while saving data.");
                    return update;
                }
                
                // Set Update Result.
                var updateMessage =
                    $"Type '{result.InsuranceCompanyId}' ({result.InsuranceCompanyId}) {(update.InsuranceCompanyId == result.InsuranceCompanyId ? "Updated" : "Created")} Successfully.";

                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                update.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<IInsuranceCompanyDto> SaveAsync(IInsuranceCompanyDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
