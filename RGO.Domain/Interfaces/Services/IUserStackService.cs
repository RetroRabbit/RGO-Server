using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services
{
    public interface IUserStackService
    {
        Task<bool> HasTechStack(int userId);
        Task<UserStackDto> GetUserStack(int userId);
        Task<UserStackDto> AddUserStack(int userId);
        Task<UserStackDto> RemoveUserStack(int userId);

        Task<UserStackDto> UpdateUserStack(int userId, string description);
    }
}
