﻿using System;
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
        int Type,
        DateTime JoinDate,
        List<SkillDto> Skills,
        List<SocialDto> Socials,
        List<CertificationsDto> Certifications,
        List<ProjectsDto> Projects
        );
}
