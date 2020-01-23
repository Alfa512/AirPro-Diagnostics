using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Concrete;
using AutoMapper;

namespace AirPro.Service.Services.Concrete
{
    internal class FeedbackService : ServiceBase, IService<IFeedbackDto>
    {
        public FeedbackService(IServiceSettings settings) : base(settings)
        {
        }

        public IFeedbackDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IFeedbackDto> GetByIdAsync(string id)
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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedbackDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFeedbackDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IFeedbackDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IFeedbackDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IFeedbackDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IFeedbackDto Save(IFeedbackDto update)
        {
            // Load Type.
            var result = update;

            var repair = Db.RepairOrders.FirstOrDefault(d=> d.OrderId == update.RepairId);
            if (repair == null)
            {
                // No Group Access.
                result.UpdateResult = new UpdateResultDto(false, "Repair does not exists.");
                return result;
            }

            if (repair.Feedback == null)
            {
                var entity = Mapper.Map<FeedbackEntityModel>(update);
                entity.CreatedByUserGuid = User.UserGuid;
                entity.CreatedDt = DateTime.UtcNow;
                Db.Entry(entity).State = EntityState.Added;

                repair.Feedback = entity;
                Db.SaveChanges();

                var updateMessage = "Feedback Submitted Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            else
            {
                repair.Feedback.AdditionalFeedback = update.AdditionalFeedback;
                repair.Feedback.ConcernsAddressedRate = update.ConcernsAddressedRate;
                repair.Feedback.ReportCompletionRate = update.ReportCompletionRate;
                repair.Feedback.ResponseTimeRate = update.ResponseTimeRate;
                repair.Feedback.TechnicianCommunicationRate = update.TechnicianCommunicationRate;
                repair.Feedback.TechnicianKnowledgeRate = update.TechnicianKnowledgeRate;
                repair.Feedback.UpdatedDt = DateTimeOffset.UtcNow;
                repair.Feedback.UpdatedByUserGuid = User.UserGuid;
                Db.SaveChanges();

                var updateMessage = "Repair Updated Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }

            return result;
        }

        public Task<IFeedbackDto> SaveAsync(IFeedbackDto update)
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
