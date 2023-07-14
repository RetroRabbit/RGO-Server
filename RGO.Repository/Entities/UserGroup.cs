

using RGO.Domain.Models;

namespace RGO.Repository.Entities
{
    public class UserGroup
    {
        //  id SERIAL PRIMARY KEY,
  //title VARCHAR(255)
        public int id { get; set; }
        public string title { get; set; } = null!;

        public UserGroupDTO ToDTO()
        {
            return new UserGroupDTO(title);
        }

    }
}
