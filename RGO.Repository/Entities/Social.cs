using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;
public class Social
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("discord")]
    public string Discord { get; set; } = null!;

    [Column("codeWars")]
    public string CodeWars{ get; set; } = null!;

    [Column("gitHub")]
    public string GitHub { get; set; } = null!;

    [Column("linkedIn")]
    public string LinkedIn { get; set; } = null!;/*

    [Column("userId")]
    public int UserId { get; set; }*/

    [ForeignKey("userId")]
    public virtual User UserSocial{ get; set; }
    public Social()
    {

    }

    public Social(SocialDto social)
    {
        Id = social.Id;
        Discord = social.Discord;
        CodeWars = social.CodeWars;
        GitHub  = social.GitHub;
        LinkedIn = social.LinkedIn;
    }

    public SocialDto ToDTO()
    {
        return new SocialDto(
            Id,
            Discord,
            CodeWars,
            GitHub,
            LinkedIn
            );
    }
}
