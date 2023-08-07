using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories
{
    public class StackRepository : IStackRepository
    {
        private readonly DatabaseContext _databaseContext;

        public StackRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<StacksDto>> GetBackendStack()
        {
            List<StacksDto> stacks = await _databaseContext.stacks
                .Where(x => x.StackType == 2)
                .Select(x => x.ToDTO())
                .ToListAsync();
            return stacks;
        }

        public async Task<List<StacksDto>> GetDatabaseStack()
        {
            List<StacksDto> stacks = await _databaseContext.stacks
                .Where(x => x.StackType == 0)
                .Select(x => x.ToDTO())
                .ToListAsync();
            return stacks;
        }

        public async Task<List<StacksDto>> GetFrontendStack()
        {
            List<StacksDto> stacks = await _databaseContext.stacks
                .Where(x => x.StackType == 1)
                .Select(x => x.ToDTO())
                .ToListAsync();
            return stacks;
        }

        public async Task<StacksDto> GetStack(int id)
        {
            Stacks? stack = await _databaseContext.stacks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (stack == null) 
                throw new Exception("Stack not found");

            return stack.ToDTO();
        }

        public async Task<bool> StackExists(int id)
        {
            bool found = await _databaseContext.stacks
                .AnyAsync(x => x.Id == id);
            return found;
        }
    }
}
