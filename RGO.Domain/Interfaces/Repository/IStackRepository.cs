using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IStackRepository
    {
        Task<bool> StackExists(int id);
        Task<StacksDto> GetStack(int id);
        Task<StacksDto[]> GetBackendStack();
        Task<StacksDto[]> GetFrontendStack();
        Task<StacksDto[]> GetDatabaseStack();
    }
}
