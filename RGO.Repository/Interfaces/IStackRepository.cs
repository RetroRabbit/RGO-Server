using RGO.Models;

namespace RGO.Repository.Interfaces
{
    public interface IStackRepository
    {
        Task<bool> StackExists(int id);
        Task<StacksDto> GetStack(int id);
        Task<List<StacksDto>> GetBackendStack();
        Task<List<StacksDto>> GetFrontendStack();
        Task<List<StacksDto>> GetDatabaseStack();
    }
}
