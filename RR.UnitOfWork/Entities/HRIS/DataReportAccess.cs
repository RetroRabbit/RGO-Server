using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models.Report;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("DataReportAccess")]
public class DataReportAccess : IModel<DataReportAccessDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reportId")]
    [ForeignKey("DataReport")]
    public int ReportId { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int? EmployeeId { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int? RoleId { get; set; }

    public bool ViewOnly { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public virtual DataReport? DataReport { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Role? Role { get; set; }

    public DataReportAccessDto ToDto()
    {
        return new DataReportAccessDto();
    }
}