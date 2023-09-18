using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeEvaluationRatingService : IEmployeeEvaluationRatingService
    {
        private readonly IUnitOfWork _db;

        public EmployeeEvaluationRatingService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            return await _db.EmployeeEvaluationRating.Add(new EmployeeEvaluationRating(employeeEvaluationRatingDto));
        }

        public async Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            return await _db.EmployeeEvaluationRating.Delete(employeeEvaluationRatingDto.Id);
        }

        public async Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            return await _db.EmployeeEvaluationRating.GetById(employeeEvaluationRatingDto.Id);
        }

        public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings()
        {
            return await _db.EmployeeEvaluationRating.GetAll();
        }

        public async Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            return await _db.EmployeeEvaluationRating.Update(new EmployeeEvaluationRating(employeeEvaluationRatingDto));
        }
    }
}
