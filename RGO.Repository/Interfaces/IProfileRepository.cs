using RGO.Models;

namespace RGO.Repository.Interfaces
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Gets a user with all thier dependancies
        /// </summary>
        /// <param email="user's email"></param>
        /// <returns></returns>
        /// 
        Task<ProfileDto> GetUserProfileByEmail(string email);

    }
}
