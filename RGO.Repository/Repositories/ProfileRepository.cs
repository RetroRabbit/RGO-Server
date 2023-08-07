using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;

namespace RGO.Repository.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IUserRepository _userRepository;

        private readonly DatabaseContext _databaseContext;

        public ProfileRepository(IUserRepository userRepository, DatabaseContext databaseContext)
        {
            _userRepository = userRepository;
            _databaseContext = databaseContext;
        }


        async Task<ProfileDto> IProfileRepository.GetUserProfileByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            int Id = user.Id;
            int? GradGroupId = user.GradGroupId;
            string FirstName = user.FirstName;
            string LastName = user.LastName;    
            string Email    = user.Email;
            int Type = user.Type;
            DateTime JoinDate = DateTime.Now;
            int Status = user.Status;

            var projects = await _databaseContext.projects.Where(projects => projects.UserId == user.Id).Select(project => project.ToDTO()).ToListAsync();
            var skills = await _databaseContext.skill.Where(skills => skills.UserId == user.Id).Select(skill => skill.ToDTO()).ToListAsync();
            var certs = await _databaseContext.certifications.Where(certs => certs.UserId == user.Id).Select(cert => cert.ToDTO()).ToListAsync();
            var social = await _databaseContext.social.Where(social => social.UserId == user.Id).Select(socials => socials.ToDTO()).ToListAsync();


            var profileDto = new ProfileDto
                (
                    Id,
                    GradGroupId,
                    FirstName,
                    LastName,
                    Email,
                    Type,
                    JoinDate,
                    Status,
                    skills,
                    social,
                    certs,
                    projects
                );

            return profileDto;
        }
    }
}
