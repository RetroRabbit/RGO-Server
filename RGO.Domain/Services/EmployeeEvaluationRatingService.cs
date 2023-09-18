using Microsoft.EntityFrameworkCore;
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
            var existingRating = await _db.EmployeeEvaluation
                .Get(evaluation => evaluation.Id == employeeEvaluationRatingDto.Id)
                .FirstOrDefaultAsync();

            if (existingRating != null)
            {
                throw new InvalidOperationException("An evaluation rating with the given ID already exists.");
            }

            return await _db.EmployeeEvaluationRating.Add(new EmployeeEvaluationRating(employeeEvaluationRatingDto));
        }

        public async Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            var existingRating = await GetEmployeeEvaluationRating(employeeEvaluationRatingDto);

            if (existingRating == null)
            {
                throw new InvalidOperationException("No evaluation rating found with the given ID to delete.");
            }

            return await _db.EmployeeEvaluationRating.Delete(existingRating.Id);
        }

        public async Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            return await _db.EmployeeEvaluationRating
                .Get(rating => rating.Id == employeeEvaluationRatingDto.Id)
                .Select(rating => rating.ToDto())
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException("No evaluation rating found with the given ID.");
        }

        public async Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
        {
            var existingRating = await GetEmployeeEvaluationRating(employeeEvaluationRatingDto);

            if (existingRating == null)
            {
                throw new InvalidOperationException("No evaluation rating found with the given ID to update.");
            }

            return await _db.EmployeeEvaluationRating.Update(new EmployeeEvaluationRating(existingRating));
        }

        public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings()
        {
            var entities = await _db.EmployeeEvaluationRating.GetAll();
            var dtoList = entities.Select(entity => new EmployeeEvaluationRatingDto(
                entity.Id,
                entity.EmployeeEvaluationId,
                entity.EmployeeId,
                entity.Score,
                entity.Comment)).ToList();

            return dtoList;
        }
    }
}
