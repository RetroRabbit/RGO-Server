using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Termination")]
public class Termination : IModel
{
    public Termination()
    {
    }

    public Termination(TerminationDto terminationDto)
    {
        Id = terminationDto.Id;
        EmployeeId = terminationDto.EmployeeId;
        TerminationOption = terminationDto.TerminationOption;
        DayOfNotice = terminationDto.DayOfNotice;
        LastDayOfEmployment = terminationDto.LastDayOfEmployment;
        ReemploymentStatus = terminationDto.ReemploymentStatus;
        EquipmentStatus = terminationDto.EquipmentStatus;
        AccountsStatus = terminationDto.AccountsStatus;
        TerminationDocument = terminationDto.TerminationDocument;
        DocumentName = terminationDto.DocumentName;
        TerminationComments = terminationDto.TerminationComments;
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Employee")]
    [Column("employeeId")] public int EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }

    [Column("terminationOption")]
    public TerminationOption TerminationOption { get; set; }

    [Column("dayOfNotice")]
    public DateTime DayOfNotice { get; set; }

    [Column("lastDayOfEmployment")]
    public DateTime LastDayOfEmployment { get; set; }

    [Column("reemploymentStatus")]
    public bool ReemploymentStatus { get; set; }

    [Column("equipmentStatus")]
    public bool EquipmentStatus { get; set; }

    [Column("accountsStatus")]
    public bool AccountsStatus { get; set; }

    [Column("terminationDocument")]
    public string TerminationDocument { get; set; }

    [Column("documentName")]
    public string DocumentName { get; set; }

    [Column("terminationComments")]
    public string TerminationComments { get; set; }

    public TerminationDto ToDto()
    {
        return new TerminationDto { 
            Id = Id, 
            EmployeeId = EmployeeId,
            TerminationOption = TerminationOption, 
            DayOfNotice = DayOfNotice,
            LastDayOfEmployment = LastDayOfEmployment,
            ReemploymentStatus = ReemploymentStatus,
            EquipmentStatus = EquipmentStatus,
            AccountsStatus = AccountsStatus,
            TerminationDocument = TerminationDocument, 
            DocumentName = DocumentName,
            TerminationComments = TerminationComments,
        };
    }
}