using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportValues")]
public class DataReportValues : IModel<DataReportValuesDto>
{
    public DataReportValues() { }

    public DataReportValues(DataReportValuesDto dataReportValuesDto)
    {
        Id = dataReportValuesDto.Id;
        ReportId = dataReportValuesDto.ReportId;
        ColumnId = dataReportValuesDto.ColumnId;
        EmployeeId = dataReportValuesDto.EmployeeId;
        Input = dataReportValuesDto.Input;
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reportId")]
    [ForeignKey("DataReport")]
    public int ReportId { get; set; }

    [Column("columnId")]
    [ForeignKey("DataReportColumns")]
    public int ColumnId { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("input")]
    public string Input { get; set; }

    public virtual DataReport? DataReport { get; set; }

    public virtual DataReportColumns? DataReportColumns { get; set; }

    public virtual Employee? Employee { get; set; }

    public DataReportValuesDto ToDto()
    {
       return new DataReportValuesDto { 
            Id = Id, 
            ReportId = ReportId, 
            ColumnId = ColumnId, 
            Input = Input, 
            EmployeeId = EmployeeId };
    }
}