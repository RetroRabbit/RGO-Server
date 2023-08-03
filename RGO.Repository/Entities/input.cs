using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RGO.Repository.Entities;

public class Input
{
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("userId")]
    public int UserId { get; set; }

    [Column("formSubmitId")]
    public int FormSubmitId { get; set; }

    [Column("fieldId")]
    public int FieldId { get; set; }*/

    [Column("value")]
    public string Value { get; set; }

    [Column("createDate")]
    public int CreateDate { get; set; }

/*    [Column("inputId")]
    public int InputId { get; set; }*/

    [ForeignKey("fieldId")]
    public virtual Field InputFieldId { get; set; }

    [ForeignKey("userId")]
    public virtual User InputUserId { get; set; }

    /*[ForeignKey("formSubmitId")]
    public virtual List<FormSubmit> InputSubmit { get; set; }*/

/*    [ForeignKey("inputId")]
    public virtual List<FormSubmit> InputSubmit { get; set; }*/
}