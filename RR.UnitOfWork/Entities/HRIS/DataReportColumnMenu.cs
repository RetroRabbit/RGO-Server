using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models.Report;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportColumnMenu")]
public class DataReportColumnMenu : IModel<DataReportColumnMenuDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("parentId")]
    [ForeignKey("ParentColumn")]
    public int? ParentId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("prop")]
    public string? Prop { get; set; }

    [Column("mapping")]
    public string? Mapping { get; set; }

    [Column("fieldCodeId")]
    [ForeignKey("FieldCode")]
    public int? FieldCodeId { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual DataReportColumnMenu? ParentColumn { get; set; }

    public virtual FieldCode? FieldCode { get; set; }

    public virtual List<DataReportColumnMenu>? Children { get; set; }

    public DataReportColumnMenuDto ToDto()
    {
        return new DataReportColumnMenuDto
        {
            Id = Id,
            Name = Name ?? FieldCode.Name,
            Prop = Prop ?? FieldCode.Code,
            Children = Children?.OrderBy(x => x.Name).Select(x => x.ToDto()).ToList()
        };
    }
}