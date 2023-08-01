using RGO.Domain.Models;
using System.Linq;

namespace RGO.Repository.Entities
{
    public class User
    {

        public int id { get; set; }
        public int groupid { get; set; }
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public string email { get; set; } = null!;
        public int type { get; set; }
        public DateTime joindate { get; set; }
        public int status { get; set; }
        public List<Skill> skills { get; set; } = new();
        public List<Certifications> certifications { get; set; } = new();
        public List<Projects> projects { get; set; } = new();
        public Social social { get; set; } = new();

        public User()
        {
        }

        public User(UserDto user)
        {
            id = user.id;
            groupid = user.groupid;
            firstname = user.firstname;
            lastname = user.lastname;
            email = user.email;
            type = user.type;
            joindate = user.joindate;
            status = user.status;
            skills= user.skill.Select(x => new Skill(x)).ToList();
            certifications = user.certifications.Select(x => new Certifications(x)).ToList();
            projects =user.projects.Select(x => new Projects(x)).ToList();
            social = new(user.social);

        }

        public UserDto ToDTO()
        {
            return new UserDto(
                id,
                groupid,
                firstname,
                lastname,
                email,
                type,
                joindate,
                status,
                skills.Select(x => x.ToDTO()).ToList(),
                certifications.Select(x => x.ToDTO()).ToList(),
                projects.Select(x => x.ToDTO()).ToList(),
                social.ToDTO()

                );
        }
    }
}
