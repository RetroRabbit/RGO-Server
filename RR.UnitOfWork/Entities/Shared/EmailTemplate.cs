using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.Shared;

[Table("EmailTemplate")]
public class EmailTemplate : IModel
{
    public EmailTemplate()
    {

    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("subject")]
    public string Subject { get; set; }

    [Column("body")]
    public string Body { get; set; }
}