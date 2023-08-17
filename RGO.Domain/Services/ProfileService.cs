using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _db;

    public ProfileService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<ProfileDto> GetUserProfileByEmail(string email)
    {
        var user = await _db.User.GetByEmail(email);

        //var projects = await _databaseContext.projects.Where(projects => projects.UserId == user.Id).Select(project => project.ToDto()).ToListAsync();
        //var skills = await _databaseContext.skill.Where(skills => skills.UserId == user.Id).Select(skill => skill.ToDto()).ToListAsync();
        //var certs = await _databaseContext.certifications.Where(certs => certs.UserId == user.Id).Select(cert => cert.ToDto()).ToListAsync();
        //var social = await _databaseContext.social.Where(social => social.UserId == user.Id).Select(socials => socials.ToDto()).ToListAsync();

        return new ProfileDto(user, null, null, null, null);
    }
}
