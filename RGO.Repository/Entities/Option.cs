using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGO.Repository.Entities;

public class Option
{
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("fieldId")]
    public int FieldId { get; set; }*/

    [Column("value")]
    public string Value { get; set; } = null!;

    [ForeignKey("fieldId")]
    public virtual Field FieldOptions { get; set; }

}
