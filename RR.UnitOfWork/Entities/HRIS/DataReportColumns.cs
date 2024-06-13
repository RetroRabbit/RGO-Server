using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportColumns")]
public class DataReportColumns : IModel<DataReportColumnsDto>
{
    //id
    //name
    //sequence
    //groupBy [int]
    //status
    //isCustom [bool]
    //customType [int] (text,checkbox)

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
    
    [Column("prop")]
    public string Prop { get; set; }

    [Column("mapping")]
    public string Mapping { get; set; }

    [Column("sequence")]
    public int Sequence { get; set; }

    [Column("isCustom")]
    public bool IsCustom { get; set; }

    [Column("fieldType")]
    public int? FieldType { get; set; }

    [Column("reportId")]
    [ForeignKey("DataReport")]
    public int ReportId { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual DataReport? DataReport { get; set; }

    public DataReportColumnsDto ToDto()
    {
        throw new NotImplementedException();
    }
}