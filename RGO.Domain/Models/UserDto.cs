namespace RGO.Domain.Models
{
    public record UserDto(
        int id,
        int groupid,
        string firstname,
        string lastname,
        string email,
        int type,
        DateTime joindate,
        int status,
        List<SkillDto> skill,
        List<CertificationsDto> certifications,
        List<ProjectsDto> projects
        );

}
