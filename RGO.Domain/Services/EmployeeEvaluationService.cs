using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationService : IEmployeeEvaluationService
{
    private readonly IUnitOfWork _db;
    public EmployeeEvaluationService(IUnitOfWork db)
    {
        _db = db;
    }
    public async Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        EmployeeEvaluationDto newEmployeeEvaluation = await _db.EmployeeEvaluation.Add(new EmployeeEvaluation(employeeEvaluationDto));

        return newEmployeeEvaluation;
    }

    public Task<EmployeeEvaluationDto> DeleteEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        throw new NotImplementedException();
    }

    public Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations()
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeEvaluationDto> GetEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        throw new NotImplementedException();
    }
}