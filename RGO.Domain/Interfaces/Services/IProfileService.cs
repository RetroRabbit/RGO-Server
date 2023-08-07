using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IProfileService
{
    /// <summary>
    /// Gets a user with all thier dependancies
    /// </summary>
    /// <param email="user's email"></param>
    /// <returns></returns>
    Task<ProfileDto> GetUserProfileByEmail(string email);
}
