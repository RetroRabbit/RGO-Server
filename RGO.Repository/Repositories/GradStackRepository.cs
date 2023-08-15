using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories
{
    public class GradStackRepository : IGradStackRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IStackRepository _stackRepository;
        private readonly IUserRepository _userRepository;

        public GradStackRepository(DatabaseContext databaseContext, IStackRepository stackRepository, IUserRepository userRepository)
        {
            _databaseContext = databaseContext;
            _stackRepository = stackRepository;
            _userRepository = userRepository;
        }

        public async Task<GradStackDto> AddGradStack(int userId)
        {
            var checkStack = await HasTechStack(userId);
            
            if (checkStack)
            {
                GradStackDto stack = await GetGradStack(userId);
                return stack;
            }
                
            Random random = new Random();

            List<StacksDto> backendStack = await _stackRepository.GetBackendStack();
            var backendStackObject = backendStack[random.Next(0, backendStack.Count)];
            List<StacksDto> frontendStack = await _stackRepository.GetFrontendStack();
            var frontendStackObject = frontendStack[random.Next(0, frontendStack.Count)];
            List<StacksDto> databaseStack = await _stackRepository.GetDatabaseStack();
            var databaseStackObject = databaseStack[random.Next(0, databaseStack.Count)];

            GradStackDto newGradStack = new GradStackDto
            (
                0,
                userId,
                backendStackObject,
                frontendStackObject,
                databaseStackObject,
                "Personal project tech stack default text.",
                (GradStackStatus)1,
                DateTime.UtcNow);

            try
            {
                var stack = await _databaseContext.gradStacks.AddAsync(new GradStacks(newGradStack));
                await _databaseContext.SaveChangesAsync();

                return stack.Entity.ToDTO();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create Tech Stack for User {userId}.(Error: {ex.Message})");
            }
        }

        public async Task<GradStackDto> GetGradStack(int userId)
        {
            GradStacks? userStack = await _databaseContext.gradStacks
                .Include(x => x.User)
                .Include(x => x.BackendGradStack)
                .Include(x => x.FrontendGradStack)
                .Include(x => x.DatabaseGradStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userStack == null)
                return null;

            return userStack.ToDTO();
        }

        public async Task<bool> HasTechStack(int userId)
        {
            bool found = await _databaseContext.gradStacks.AnyAsync(x => x.UserId == userId);
            return found;
        }

        public async Task<GradStackDto> RemoveGradStack(int userId)
        {
            GradStacks? userStack = await _databaseContext.gradStacks
                .Include(x => x.User)
                .Include(x => x.BackendGradStack)
                .Include(x => x.FrontendGradStack)
                .Include(x => x.DatabaseGradStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userStack == null)
                throw new Exception("User stack not found");

            var oldUserStack = _databaseContext.gradStacks.Remove(userStack);
            await _databaseContext.SaveChangesAsync();

            return oldUserStack.Entity.ToDTO();
        }

        public async Task<GradStackDto> UpdateGradStack(int userId, string description)
        {
            GradStacks? gradStack = await _databaseContext.gradStacks
                .Include(x => x.User)
                .Include(x => x.BackendGradStack)
                .Include(x => x.FrontendGradStack)
                .Include(x => x.DatabaseGradStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (gradStack == null)
                throw new Exception("User stack not found");

            gradStack.Description = description;
            gradStack.Status = gradStack.Status == (int)GradStackStatus.Saved ? (int)GradStackStatus.Pending : (int)GradStackStatus.Saved;

            var currentUserstack = _databaseContext.gradStacks.Update(gradStack);
            await _databaseContext.SaveChangesAsync();

            return currentUserstack.Entity.ToDTO();
        }
    }
}
