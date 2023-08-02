using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Form")]
public class Form
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("groupId")]
    public int GroupId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("startDate")]
    public DateTime StartDate { get; set; }

    [Column("endDate")]
    public DateTime EndDate { get; set; }


    [ForeignKey("groupId")]
    public virtual UserGroup UserGroupForm { get; set; }


/*    
    public List<Field> fields { get; set; } = new();
    public List<FormSubmit> formSubmits { get; set; } = new();
*/
}
