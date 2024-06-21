using HRIS.Models.DataReport;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReport")]
public class DataReport : IModel<DataReportDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual List<DataReportColumns>? DataReportColumns { get; set; }

    public virtual List<DataReportFilter>? DataReportFilter { get; set; }

    public virtual List<DataReportValues>? DataReportValues { get; set; }

    public DataReport()
    {
    }

    public DataReport(DataReportDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Code = dto.Code;
        Status = dto.Status;
    }

    public DataReportDto ToDto()
    {
        return new DataReportDto
        {
            Id = Id,
            Name = Name,
            Code = Code,
            Status = Status,
            Columns = DataReportColumns?.Select(x => x.ToDto()).ToList(),
            Filters = DataReportFilter?.Select(x => x.ToDto()).ToList()
        };
    }
}