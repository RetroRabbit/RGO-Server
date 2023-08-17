namespace RGO.Models
{
    public record ProfileDto
    {
        public int Id { get; set; }
        public int? GradGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public int Status { get; set; }
        public string Bio { get; set; }
        public int Level { get; set; }
        public string Phone { get; set; }
        public List<SkillDto> Skills { get; set; }
        public List<SocialDto> Socials { get; set; }
        public List<CertificationsDto> Certifications { get; set; }
        public List<ProjectsDto> Projects { get; set; }

        public ProfileDto(
            UserDto user,
            List<SkillDto> Skills,
            List<SocialDto> Socials,
            List<CertificationsDto> Certifications,
            List<ProjectsDto> Projects
        )
        {
            this.Id = user.Id;
            this.GradGroupId = user.GradGroupId;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.JoinDate = user.JoinDate;
            this.Status = user.Status;
            this.Bio = user.Bio;
            this.Level = user.Level;
            this.Phone = user.Phone;
            this.Skills = Skills;
            this.Socials = Socials;
            this.Certifications = Certifications;
            this.Projects = Projects;
        }
    }
}
