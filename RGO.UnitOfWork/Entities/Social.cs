using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Social")]
public class Social : IModel<SocialDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Column("discord")]
    public string Discord { get; set; }
    [Column("codeWars")]
    public string CodeWars { get; set; }
    [Column("gitHub")]
    public string GitHub { get; set; }
    [Column("linkedIn")]
    public string LinkedIn { get; set; }
    public virtual User User { get; set; }
    public Social() { }
    public Social(SocialDto social)
    {
        Id = social.Id;
        UserId = social.UserId;
        Discord = social.Discord;
        CodeWars = social.CodeWars;
        GitHub = social.GitHub;
        LinkedIn = social.LinkedIn;
    }
    public SocialDto ToDto()
    {
        return new SocialDto(
            Id,
            UserId,
            Discord,
            CodeWars,
            GitHub,
            LinkedIn);
    }
}
