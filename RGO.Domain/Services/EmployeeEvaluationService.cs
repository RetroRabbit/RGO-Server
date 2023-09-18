using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationService : IEmployeeEvaluationService
    {
        private readonly IUnitOfWork _db;

        public EmployeeEvaluationService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
        {
            var existingEvaluation = await _db.EmployeeEvaluation
                .Get(evaluation => evaluation.Id == employeeEvaluationDto.Id)
                .FirstOrDefaultAsync();

            if (existingEvaluation != null)
            {
                throw new InvalidOperationException("An evaluation with the given ID already exists.");
            }

            return await _db.EmployeeEvaluation.Add(new EmployeeEvaluation(employeeEvaluationDto));
        }

        public async Task<EmployeeEvaluationDto> DeleteEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
        {
            var existingEvaluation = await GetEmployeeEvaluation(employeeEvaluationDto);

            if (existingEvaluation == null)
            {
                throw new InvalidOperationException("No evaluation found with the given ID to delete.");
            }

            return await _db.EmployeeEvaluation.Delete(existingEvaluation.Id);
        }

        public async Task<EmployeeEvaluationDto> GetEmployeeEvaluation(EmployeeEvaluationDto evaluationDto)
        {
            return await _db.EmployeeEvaluation
                .Get(evaluation => evaluation.Id == evaluationDto.Id)
                .Select(evaluation => evaluation.ToDto())
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException("No evaluation found with the given ID.");
        }

        public async Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
        {
            var existingEvaluation = await GetEmployeeEvaluation(employeeEvaluationDto);

            if (existingEvaluation == null)
            {
                throw new InvalidOperationException("No evaluation found with the given ID to update.");
            }

            return await _db.EmployeeEvaluation.Update(new EmployeeEvaluation(existingEvaluation));
        }

        public Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations()
        {
            return _db.EmployeeEvaluation.GetAll();
        }
    }
}