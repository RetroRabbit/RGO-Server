using HRIS.Models.DataReport;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportFilter")]
public class DataReportFilter : IModel<DataReportFilterDto>
{
    public DataReportFilter()
    {
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("table")]
    public string Table { get; set; }

    [Column("column")]
    public string Column { get; set; }

    [Column("condition")]
    public string Condition { get; set; }

    [Column("value")]
    public string? Value { get; set; }

    [Column("select")]
    public string? Select { get; set; }

    [Column("reportId")]
    [ForeignKey("DataReport")]
    public int ReportId { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual DataReport? DataReport { get; set; }

    public DataReportFilterDto ToDto()
    {
        return new DataReportFilterDto
        {
            Id = Id,
            Table = Table,
            Column = Column,
            Condition = Condition,
            Value = Value,
            Select = Select,
            ReportId = ReportId,
            Status = Status
        };
    }
}