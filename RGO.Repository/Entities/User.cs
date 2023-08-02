using RGO.Domain.Models;

namespace RGO.Repository.Entities
{
    public class User
    {

        public int id { get; set; }
        public int groupId { get; set; }
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;
        public int type { get; set; }
        public DateTime joinDate { get; set; }
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
            groupId = user.groupId;
            firstName = user.firstName;
            lastName = user.lastName;
            email = user.email;
            type = user.type;
            joinDate = user.joinDate;
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
                groupId,
                firstName,
                lastName,
                email,
                type,
                joinDate,
                status,
                skills.Select(x => x.ToDTO()).ToList(),
                certifications.Select(x => x.ToDTO()).ToList(),
                projects.Select(x => x.ToDTO()).ToList(),
                social.ToDTO()

                );
        }
    }
}
