using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("UserGroup")]
public class UserGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; }
    public UserGroup() { }
    public UserGroup(UserGroupDto userGroup)
    {
        Id = userGroup.Id;
        Title = userGroup.Title;  
    }
    public UserGroupDto ToDTO()
    {
        return new UserGroupDto(
            Id,
            Title);
    }

}
