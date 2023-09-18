using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationTemplateItemService : IEmployeeEvaluationTemplateItemService
    {
        private readonly IUnitOfWork _db;

        public EmployeeEvaluationTemplateItemService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeEvaluationTemplateItemDto> SaveEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto evaluationTemplateItemDto)
        {
            return await _db.EmployeeEvaluationTemplateItem.Add(new EmployeeEvaluationTemplateItem(evaluationTemplateItemDto));
        }

        public async Task<EmployeeEvaluationTemplateItemDto> DeleteEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto evaluationTemplateItemDto)
        {
            return await _db.EmployeeEvaluationTemplateItem.Delete(evaluationTemplateItemDto.Id);
        }

        public async Task<EmployeeEvaluationTemplateItemDto> GetEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto evaluationTemplateItemDto)
        {
            return await _db.EmployeeEvaluationTemplateItem.GetById(evaluationTemplateItemDto.Id);
        }

        public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItems()
        {
            return await _db.EmployeeEvaluationTemplateItem.GetAll();
        }

        public async Task<EmployeeEvaluationTemplateItemDto> UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto evaluationTemplateItemDto)
        {
            return await _db.EmployeeEvaluationTemplateItem.Update(new EmployeeEvaluationTemplateItem(evaluationTemplateItemDto));
        }
    }
}
