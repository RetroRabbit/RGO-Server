using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("FormSubmit")]
public class FormSubmit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("formid")]
    public int Formid { get; set; }

    [Column("createDate")]
    public DateTime CreateDate { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("rejectionReason")]
    public string rejectionReason { get; set; } = null!;

    [ForeignKey("userId")]
    public virtual User UserSubmit { get; set; }

    [ForeignKey("formId")]
    public virtual Form Form { get; set; }
}