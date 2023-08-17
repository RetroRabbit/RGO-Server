﻿using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Repository.Interfaces;

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


        public async Task<ProfileDto> GetUserProfileByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            int Id = user.Id;
            int? GradGroupId = user.GradGroupId;
            string FirstName = user.FirstName;
            string LastName = user.LastName;    
            string Email    = user.Email;
            DateTime JoinDate = DateTime.Now;
            int Status = user.Status;
            string Bio= user.Bio;
            int Level= user.Level;
            string Phone=user.Phone;

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
                    JoinDate,
                    Status,
                    Bio,
                    Level,
                    Phone,
                    skills,
                    social,
                    certs,
                    projects
                );

            return profileDto;
        }
    }
}
