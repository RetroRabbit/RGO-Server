using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface ISkillRepository
    {
        Task<bool> HasSkills(string title);
        Task<List<SkillDto>> GetSkillsByUserId(int userid);
    }
}
