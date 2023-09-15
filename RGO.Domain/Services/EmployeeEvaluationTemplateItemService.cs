using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationTemplateItemService : IEmployeeEvaluationTemplateItemService
    {
        private readonly IUnitOfWork _db;
        public EmployeeEvaluationTemplateItemService(IUnitOfWork db)
        {
            _db = db;
        }
        public Task<EmployeeEvaluationTemplateItemDto> DeleteEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItems()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateItemDto> GetEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateItemDto> SaveEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEvaluationTemplateItemDto> UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
        {
            throw new NotImplementedException();
        }
    }
}
