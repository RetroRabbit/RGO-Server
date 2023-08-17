﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("User")]
public class User : IModel<UserDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("gradGroupId")]
    [ForeignKey("GradGroup")]
    public int? GradGroupId { get; set; }

    [Column("firstName")]
    public string FirstName { get; set; }

    [Column("lastName")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("joinDate")]
    public DateTime JoinDate { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("bio")]
    public string Bio { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    [Column("gradType")]
    public int? GradType { get; set; }

    public virtual GradGroup? GradGroup { get; set; }
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
        JoinDate = user.JoinDate;
        Status = user.Status;
        Bio = user.Bio;
        Level = user.Level;
        Phone = user.Phone;
        GradType = user.GradType;

    }

    public UserDto ToDto()
    {
        return new UserDto(
            Id,
            GradGroupId,
            FirstName,
            LastName,
            Email,
            JoinDate,
            Status,
            Bio,
            Level,
            Phone,
            GradType);
    }
}