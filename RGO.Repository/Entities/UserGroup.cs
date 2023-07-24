using RGO.Domain.Models;

namespace RGO.Repository.Entities
{
    public class UserGroup
    {
        public int id { get; set; }
        public string title { get; set; } = null!;

        public UserGroupDTO ToDTO()
        {
            return new UserGroupDTO(title);
        }

    }
}
