using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using EntityFrameworkCore.EncryptColumn.Attribute;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.Shared;

[Table("EmailHistory")]
public class EmailHistory : IModel
{
    public EmailHistory()
    {
    }

    public EmailHistory(MailMessage message, int templateId)
    {
        To = message.To.ToString();
        Cc = message.CC.ToString();
        Bcc = message.Bcc.ToString();
        TemplateId = templateId;
        Subject = message.Subject;
        Body = message.Body;
        Status = EmailStatus.Draft;
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("templateId")]
    [ForeignKey("EmailTemplate")]
    public int? TemplateId { get; set; }

    [Column("subject")]
    public string Subject { get; set; }

    [Column("to")]
    public string To { get; set; }

    [Column("cc")]
    public string? Cc { get; set; }

    [Column("bcc")]
    public string? Bcc { get; set; }

    [Column("body")]
    [EncryptColumn]
    public string Body { get; set; }

    [Column("status")]
    public EmailStatus Status { get; set; }

    public virtual EmailTemplate? EmailTemplate { get; set; }
}