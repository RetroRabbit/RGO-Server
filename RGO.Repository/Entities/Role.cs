using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities
{
    public class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /*[Column("userId")]
        [ForeignKey("User")]
        public int UserId { get; set; }*/

        [Column("Description")]
        public string Description { get; set; }

        /*public virtual User User { get; set; }*/
        
        public Role()
        {
        }

        /*public Role(RoleDto roleDto)
        {
            Id = roleDto.Id;
            UserId = roleDto.UserId;
            Description = roleDto.Description;
        }

        public RoleDto ToDTO()
        {
            return new RoleDto(
                Id,
                UserId,
                Description);
        }*/
    }
}
