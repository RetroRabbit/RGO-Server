using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record ProfileDto(
        int Id,
        int? GradGroupId,
        string FirstName,
        string LastName,
        string Email,
        DateTime JoinDate,
        int Status,
        string Bio,
        int Level,
        string Phone, 
        List<SkillDto> Skills,
        List<SocialDto> Socials,
        List<CertificationsDto> Certifications,
        List<ProjectsDto> Projects
        );
}
