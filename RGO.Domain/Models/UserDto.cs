namespace RGO.Domain.Models
{
    public record UserDto(
        int id,
        int groupid,
        string firstName,
        string lastName,
        string email,
        int type,
        DateTime joinDate,
        int status,
        List<SkillDto> skill,
        List<CertificationsDto> certifications,
        List<ProjectsDto> projects,
        SocialDto social
        );

}
