using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IGradStackRepository
    {
        Task<bool> HasTechStack(int userId);
        Task<GradStackDto> GetGradStack(int userId);
        Task<GradStackDto> AddGradStack(int userId);
        Task<GradStackDto> RemoveGradStack(int userId);
        Task<GradStackDto> UpdateGradStack(int userId, string description);
    }
}
