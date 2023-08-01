using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface ISocialRepository
    {
        Task<bool> HasSocials(int userid);
        Task<ProjectsDto> GetSocialsByUserId(int userid);
    }
}
