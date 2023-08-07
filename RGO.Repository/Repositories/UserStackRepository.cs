using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories
{
    public class UserStackRepository : IUserStackRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IStackRepository _stackRepository;

        public UserStackRepository(DatabaseContext databaseContext, IStackRepository stackRepository)
        {
            _databaseContext = databaseContext;
            _stackRepository = stackRepository;
        }

        public async Task<UserStackDto> AddUserStack(int userId)
        {
            Random random = new Random();

            List<StacksDto> backendStack = await _stackRepository.GetBackendStack();
            var backendStackObject = backendStack[random.Next(0, backendStack.Count)];
            List<StacksDto> frontendStack = await _stackRepository.GetFrontendStack();
            var frontendStackObject = frontendStack[random.Next(0, frontendStack.Count)];
            List<StacksDto> databaseStack = await _stackRepository.GetDatabaseStack();
            var databaseStackObject = databaseStack[random.Next(0, databaseStack.Count)];

            UserStackDto newUserStack = new UserStackDto
            (
                0,
                userId,
                backendStackObject,
                frontendStackObject,
                databaseStackObject,
                "Personal project tech stack default text.",
                (UserStackStatus)1,
                DateTime.UtcNow);

            try
            {
                var stack = await _databaseContext.userStacks.AddAsync(new UserStacks(newUserStack));
                await _databaseContext.SaveChangesAsync();

                return stack.Entity.ToDTO();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create Tech Stack for User {userId}.(Error: {ex.Message})");
            }
        }

        public async Task<UserStackDto> GetUserStack(int userId)
        {
            UserStacks? userStack = await _databaseContext.userStacks
                .Include(x => x.User)
                .Include(x => x.BackendUserStack)
                .Include(x => x.FrontendUserStack)
                .Include(x => x.DatabaseUserStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userStack == null)
                throw new Exception("User stack not found");

            return userStack.ToDTO();
        }

        public async Task<bool> HasTechStack(int userId)
        {
            bool found = await _databaseContext.userStacks.AnyAsync(x => x.UserId == userId);
            return found;
        }

        public async Task<UserStackDto> RemoveUserStack(int userId)
        {
            UserStacks? userStack = await _databaseContext.userStacks
                .Include(x => x.User)
                .Include(x => x.BackendUserStack)
                .Include(x => x.FrontendUserStack)
                .Include(x => x.DatabaseUserStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userStack == null)
                throw new Exception("User stack not found");

            var oldUserStack = _databaseContext.userStacks.Remove(userStack);
            await _databaseContext.SaveChangesAsync();

            return oldUserStack.Entity.ToDTO();
        }

        public async Task<UserStackDto> UpdateUserStack(int userId, string description)
        {
            UserStacks? userStack = await _databaseContext.userStacks
                .Include(x => x.User)
                .Include(x => x.BackendUserStack)
                .Include(x => x.FrontendUserStack)
                .Include(x => x.DatabaseUserStack)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userStack == null)
                throw new Exception("User stack not found");

            userStack.Description = description;

            var currentUserstack = _databaseContext.userStacks.Update(userStack);
            await _databaseContext.SaveChangesAsync();

            return currentUserstack.Entity.ToDTO();
        }
    }
}
