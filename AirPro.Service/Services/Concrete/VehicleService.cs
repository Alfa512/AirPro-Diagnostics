using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Service.Services.Concrete
{
    internal class VehicleService : ServiceBase, IService<IVehicleDto>
    {
        public VehicleService(IServiceSettings settings) : base(settings)
        {
        }

        public IVehicleDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IVehicleDto> GetByIdAsync(string id)
        {
            // Check VIN.
            if (id == null) throw new ArgumentNullException(nameof(id));

            // Search Vehicle By VIN.
            var result = await Conn.QueryFirstOrDefaultAsync<VehicleDto>("Repair.usp_GetVehicleByVIN", new { VehicleVIN = id }, commandType: CommandType.StoredProcedure);

            // Vehicle Found.
            if (result?.VehicleVIN != null && !(result?.ManualEntryInd ?? true)) return result;

            // Search Vehicle in NHTSA.
            var nhtsa = await NhtsaVehicleSearch(id);
            if (!nhtsa.ManualEntryInd) return nhtsa;

            // Search Vehicle Internally.
            result = await Conn.QueryFirstOrDefaultAsync<VehicleDto>(sql: "Repair.usp_GetDecodeVIN", param: new { DecodeVIN = id }, commandType: CommandType.StoredProcedure);

            // Return.
            return result ?? new VehicleDto { VehicleVIN = id };
        }

        public string GetDisplayName(string id)
        {
            // Lookup Vehicle.
            var vehicle = GetById(id);

            // Return Display Name.
            return $"{vehicle.VehicleYear} {vehicle.VehicleMakeName} {vehicle.VehicleModel}";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            // Return Display List.
            return GetAll()
                .Select(v => new KeyValuePair<string, string>(v.VehicleVIN,
                    $"{v.VehicleYear} {v.VehicleMakeName} {v.VehicleModel}")).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicleDto> GetAll(ServiceArgs args = null)
        {
            // Load Vehicles.
            return Conn.Query<VehicleDto>("Repair.usp_GetVehicleByVIN").ToList();
        }

        public Task<IEnumerable<IVehicleDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IVehicleDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IVehicleDto Save(IVehicleDto update)
        {
            // Update Vehicle.
            return Update(update, null);
        }

        public Task<IVehicleDto> SaveAsync(IVehicleDto update)
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

        #region Support Methods

        private IVehicleDto Update(IVehicleDto vehicle, int? lookupId)
        {
            try
            {
                // Update Vehicle.
                Conn.Execute("Repair.usp_SaveVehicle @VehicleVIN, @VehicleMakeId, @VehicleMakeName, @VehicleModel, @VehicleYear, @VehicleTransmission, @VehicleLookupId", new
                {
                    VehicleVIN = vehicle.VehicleVIN.ToUpper(),
                    VehicleMakeId = vehicle.VehicleMakeId,
                    VehicleMakeName = vehicle.VehicleMakeName,
                    VehicleModel = vehicle.VehicleModel,
                    VehicleYear = vehicle.VehicleYear,
                    VehicleTransmission = vehicle.VehicleTransmission,
                    VehicleLookupId = lookupId
                });

                // Load Vehicle.
                return Conn.QueryFirstOrDefault<VehicleDto>("Repair.usp_GetVehicleByVIN @VehicleVIN;", new { VehicleVIN = vehicle.VehicleVIN });
            }
            catch
            {
                vehicle.ManualEntryInd = true;
            }

            return vehicle;
        }

        private async Task<IVehicleDto> NhtsaVehicleSearch(string vin)
        {
            // Create Result
            IVehicleDto result = new VehicleDto { VehicleVIN = vin };

            // Lookup Settings.
            var lookup = new VehicleLookupDto
            {
                VehicleVIN = vin.ToUpper(),
                Service = VehicleLookupService.NHTSA,
                RequestBaseURL = "https://vpic.nhtsa.dot.gov/",
                RequestString = $"api/vehicles/DecodeVin/{vin}?format=json"
            };

            // Perform Lookup.
            await WebApiRequest(lookup);

            // Check Response.
            if (!lookup.RequestSuccess) return result;

            // Load Response.
            dynamic response = JsonConvert.DeserializeObject(lookup.ResponseContent);

            // Check Response.
            if (response.Message != "Results returned successfully") return result;

            // Load Results.
            var results = ((IEnumerable)response.Results).Cast<dynamic>().ToList();

            // Load Vehicle Information.
            var make = results.Where(r => r.Variable == "Make").Select(r => r.Value.ToString()).FirstOrDefault();
            var model = results.Where(r => r.Variable == "Model").Select(r => r.Value.ToString()).FirstOrDefault();
            var year = results.Where(r => r.Variable == "Model Year").Select(r => r.Value.ToString()).FirstOrDefault();
            var tran = results.Where(r => r.Variable == "Transmission Style").Select(r => r.Value.ToString()).FirstOrDefault();

            // Check Make.
            if (string.IsNullOrEmpty(make)) return result;

            // Populate Vehicle.
            var vehicle = new VehicleDto
            {
                VehicleVIN = lookup.VehicleVIN.ToUpper(),
                VehicleMakeName = make,
                VehicleModel = string.IsNullOrEmpty(model) ? "Unknown" : model,
                VehicleYear = string.IsNullOrEmpty(year) ? "UNKN" : year,
                VehicleTransmission = string.IsNullOrEmpty(tran) ? "Unknown" : tran,
                ManualEntryInd = false
            };

            // Update Vehicle.
            result = Update(vehicle, lookup.VehicleLookupId);

            // Return.
            return result;
        }

        private async Task WebApiRequest(VehicleLookupDto lookup)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Process Request.
                    client.BaseAddress = new Uri(lookup.RequestBaseURL);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                                                           SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    HttpResponseMessage resp = await client.GetAsync(lookup.RequestString);

                    // Read Response.
                    lookup.RequestSuccess = resp.IsSuccessStatusCode;
                    lookup.ResponseStatusCode = resp.StatusCode;
                    lookup.RequestMessage = resp.RequestMessage.Content?.ReadAsStringAsync().Result;
                    lookup.ResponseContent = resp.Content?.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                // Set Status.
                lookup.RequestSuccess = false;
                lookup.ResponseStatusCode = HttpStatusCode.BadRequest;

                // Load Last Exception Message.
                var innerEx = e;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                lookup.RequestMessage = innerEx.Message;
            }

            // Update Lookup.
            lookup.VehicleLookupId =
                Conn.ExecuteScalar<int>(
                    "Repair.usp_SaveVehicleLookup @VehicleVIN, @Service, @RequestBaseURL, @RequestString, @RequestSuccess, @ResponseStatusCode, @RequestMessage, @ResponseContent",
                    lookup);
        }

        #endregion
    }
}
