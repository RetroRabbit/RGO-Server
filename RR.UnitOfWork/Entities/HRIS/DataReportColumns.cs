using HRIS.Models.DataReport;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportColumns")]
public class DataReportColumns : IModel<DataReportColumnsDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reportId")]
    [ForeignKey("DataReport")]
    public int ReportId { get; set; }

    [Column("menuId")]
    [ForeignKey("Menu")]
    public int? MenuId { get; set; }

    [Column("sequence")]
    public int Sequence { get; set; }

    [Column("fieldType")]
    public DataReportColumnType FieldType { get; set; }

    [Column("customName")]
    public string? CustomName { get; set; }

    [Column("customProp")]
    public string? CustomProp { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual DataReport? DataReport { get; set; }
    
    public virtual DataReportColumnMenu? Menu { get; set; }

    public DataReportColumnsDto ToDto()
    {
        if(FieldType != DataReportColumnType.Employee)
            return new DataReportColumnsDto
            {
                Id = Id,
                Name = CustomName,
                FieldType = FieldType.ToString(),
                IsCustom = true,
                Prop = CustomProp,
                Sequence = Sequence
            };

        if(Menu?.FieldCodeId != null)
            return new DataReportColumnsDto
            {
                Id = Id,
                Name = Menu!.FieldCode.Name,
                FieldType = null,
                IsCustom = false,
                Prop = Menu!.FieldCode.Code,
                Sequence = Sequence
            };

        return new DataReportColumnsDto
        {
            Id = Id,
            Name = Menu!.Name,
            FieldType = null,
            IsCustom = false,
            Prop = Menu!.Prop,
            Sequence = Sequence
        };
    }
}