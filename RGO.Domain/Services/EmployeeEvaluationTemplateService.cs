using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationTemplateService : IEmployeeEvaluationTemplateService
    {
        private readonly IUnitOfWork _db;
        public EmployeeEvaluationTemplateService(IUnitOfWork db)
        {
            _db = db;
        }
        public Task<EmployeeEvaluationTemplateDto> DeleteEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<EmployeeEvaluationTemplateDto>> GetAllEmployeeEvaluationTemplates()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateDto> GetEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateDto> SaveEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateDto> UpdateEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
        {
            throw new NotImplementedException();
        }
    }
}
