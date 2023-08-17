using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Extensions;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class GradStackService : IGradStackService
    {
        private readonly IUnitOfWork _db;

        public GradStackService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<GradStackDto> AddGradStack(int userId)
        {
            if (await HasTechStack(userId))
            {
                var stack = await GetGradStack(userId);
                return stack;
            }

            var backendStackObject = await GetStack(StackTypes.Backend);
            var frontendStackObject = await GetStack(StackTypes.Backend);
            var databaseStackObject = await GetStack(StackTypes.Backend);

            var newGradStack = new GradStackDto
            (
                0,
                userId,
                backendStackObject,
                frontendStackObject,
                databaseStackObject,
                "Personal project tech stack default text.",
                GradProjectStatus.Submitted,
                DateTime.UtcNow);

            try
            {
                return await _db.GradStack.Add(new GradStacks(newGradStack));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create Tech Stack for User {userId}.(Error: {ex.Message})");
            }
        }

        public async Task<GradStackDto> GetGradStack(int userId)
        {
            return (await _db.GradStack.Get()
                .Include(x => x.User)
                .Include(x => x.BackendGradStack)
                .Include(x => x.FrontendGradStack)
                .Include(x => x.DatabaseGradStack)
                .FirstOrDefaultAsync(x => x.UserId == userId))
                .ToDto();
        }

        public async Task<bool> HasTechStack(int userId)
        {
            return await _db.GradStack.Any(x => x.UserId == userId);
        }

        public async Task<GradStackDto> RemoveGradStack(int userId)
        {
            var obj = await GetGradStack(userId);
            return await _db.GradStack.Delete(obj.Id);
        }

        public async Task<GradStackDto> UpdateGradStack(int userId, string description)
        {
            var obj = await GetGradStack(userId);
            obj.Description = description;
            return await _db.GradStack.Update(new GradStacks(obj));
        }

        private async Task<StacksDto> GetStack(StackTypes type)
        {
            return (await _db.Stack.GetAll(x => x.StackType == (int)type)).GetRandom();
        }
    }
}
