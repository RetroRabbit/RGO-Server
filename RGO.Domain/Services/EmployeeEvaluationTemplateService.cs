using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationTemplateService : IEmployeeEvaluationTemplateService
    {
        private readonly IUnitOfWork _db;
        public EmployeeEvaluationTemplateService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<EmployeeEvaluationTemplateDto> SaveEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto evaluationTemplateDto)
        {
            return await _db.EmployeeEvaluationTemplate.Add(new EmployeeEvaluationTemplate(evaluationTemplateDto));
        }

        public async Task<EmployeeEvaluationTemplateDto> DeleteEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto evaluationTemplateDto)
        {
            return await _db.EmployeeEvaluationTemplate.Delete(evaluationTemplateDto.Id);
        }

        public async Task<EmployeeEvaluationTemplateDto> GetEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto evaluationTemplateDto)
        {
            return await _db.EmployeeEvaluationTemplate.GetById(evaluationTemplateDto.Id);
        }

        public async Task<List<EmployeeEvaluationTemplateDto>> GetAllEmployeeEvaluationTemplates()
        {
            return await _db.EmployeeEvaluationTemplate.GetAll();
        }

        public async Task<EmployeeEvaluationTemplateDto> UpdateEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto evaluationTemplateDto)
        {
            return await _db.EmployeeEvaluationTemplate.Update(new EmployeeEvaluationTemplate(evaluationTemplateDto));
        }
    }
}
