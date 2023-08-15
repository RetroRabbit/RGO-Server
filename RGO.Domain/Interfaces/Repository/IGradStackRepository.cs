using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IGradStackRepository
    {
        /// <summary>
        /// Checks if a grad has a tech stack 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> boolean </returns>
        Task<bool> HasTechStack(int userId);

        /// <summary>
        /// Gets a grad's tech stack 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>grad stack object</returns>
        Task<GradStackDto> GetGradStack(int userId);

        /// <summary>
        /// adds a tech stack to a grad
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GradStackDto> AddGradStack(int userId);

        /// <summary>
        /// Removes a grad's tech stack     
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GradStackDto> RemoveGradStack(int userId);

        /// <summary>
        /// Updates grad's tech stack 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<GradStackDto> UpdateGradStack(int userId, string description);
    }
}
