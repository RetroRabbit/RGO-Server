using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Certifications")]
public class Certifications : IModel<CertificationsDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    public virtual User User { get; set; }

    public Certifications() { }

    public Certifications(CertificationsDto certifications)
    {
        Id = certifications.Id;
        UserId = certifications.UserId;
        Title = certifications.Title;
        Description = certifications.Description;
    }

    public CertificationsDto ToDto()
    {
        return new CertificationsDto(
            Id,
            UserId,
            Title,
            Description);
    }
}