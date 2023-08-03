using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

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

    [ForeignKey("gradGroupId")]
    public virtual UserGroup? UserGroup { get; set; }
    [ForeignKey("userId")]
    public virtual List<Certifications> UserCertifications { get; set; }
    [ForeignKey("userId")]
    public virtual List<Skill> Skills { get; set; }
    [ForeignKey("userId")]
    public virtual List<Projects> UserProjects { get; set; }
    public User()
    {
    }

    public User(UserDto user)
    {
        Id = user.Id;/*
        GradGroupId = user.GradGroupId;*/
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
            UserGroup?.Id,
            FirstName,
            LastName,
            Email,
            Type,
            JoinDate,
            Status
            );
    }
}