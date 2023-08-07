using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IProfileRepository
    {
        Task<ProfileDto> GetUserProfileByEmail(string email);

    }
}
