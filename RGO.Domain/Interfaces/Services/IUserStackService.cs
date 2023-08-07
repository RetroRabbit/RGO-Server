using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services
{
    public interface IUserStackService
    {
        /// <summary>
        /// Checks if the user has a tech stack
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasTechStack(int userId);

        /// <summary>
        /// Gets the user's tech stack
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserStackDto> GetUserStack(int userId);

        /// <summary>
        /// Adds a tech stack to the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserStackDto> AddUserStack(int userId);

        /// <summary>
        /// Removes a tech stack from the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserStackDto> RemoveUserStack(int userId);

        /// <summary>
        /// Updates the user's tech stack
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<UserStackDto> UpdateUserStack(int userId, string description);
    }
}
