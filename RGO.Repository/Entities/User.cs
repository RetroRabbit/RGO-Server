using RGO.Domain.Models;

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
                status);
        }
    }
}
