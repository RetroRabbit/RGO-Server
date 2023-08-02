using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Certifications")]
public class Certifications
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [ForeignKey("userId")]
    public virtual List<User> UserCertifications { get; set; }

    public Certifications()
    {
            
    }

    public Certifications(CertificationsDto certifications)
    {
        Id = certifications.Id;
        Title = certifications.Title;
        Description = certifications.Description;
    }

    public CertificationsDto ToDTO()
    {
        return new CertificationsDto
            (
            Id,
            Title, 
            Description
            );
    }
}