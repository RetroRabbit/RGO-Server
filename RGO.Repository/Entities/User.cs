using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("gradGroupId")]
    [ForeignKey("UserGroup")]
    public int? GradGroupId { get; set; }

    [Column("firstName")]
    public string FirstName { get; set; }

    [Column("lastName")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("joinDate")]
    public DateTime JoinDate { get; set; }

    [Column("status")]
    public int Status { get; set; }
    public virtual UserGroup? UserGroup { get; set; }
    public virtual List<Certifications> UserCertifications { get; set; }
    public virtual List<Skill> Skills { get; set; }
    public virtual List<Projects> UserProjects { get; set; }
    public virtual List<Social> Socials { get; set; }
    public User() { }
    public User(UserDto user)
    {
        Id = user.Id;
        GradGroupId = user.GradGroupId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Type = user.Type;
        JoinDate = user.JoinDate;
        Status = user.Status;
    }
    public UserDto ToDTO()
    {
        return new UserDto(
            Id,
            GradGroupId,
            FirstName,
            LastName,
            Email,
            Type,
            JoinDate,
            Status);
    }
}