using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeBanking")]
public class EmployeeBanking : IModel<EmployeeBankingDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("bankName")]
    public string BankName { get; set; }

    [Column("branch")]
    public string Branch { get; set; }

    [Column("accountNo")]
    public string AccountNo { get; set; }

    [Column("accountType")]
    public EmployeeBankingAccountType AccountType { get; set; }

    [Column("accountHolderName")]
    public string AccountHolderName { get; set; }

    [Column("status")]
    public BankApprovalStatus Status { get; set; }

    [Column("reason")]
    public string DeclineReason { get; set; }

    [Column("file")]
    public string File { get; set; }

    [Column("lastUpdateDate")]
    public DateOnly LastUpdateDate { get; set; }

    [Column("pendingUpdateDate")]
    public DateOnly PendingUpdateDate { get; set; }
    public virtual Employee Employee { get; set; }

    public EmployeeBanking() { }
    public EmployeeBanking(EmployeeBankingDto employeeBankingDto)
    {
        Id = employeeBankingDto.Id;
        EmployeeId = employeeBankingDto.EmployeeId;
        BankName = employeeBankingDto.BankName;
        Branch = employeeBankingDto.Branch;
        AccountNo = employeeBankingDto.AccountNo;
        AccountType = employeeBankingDto.AccountType;
        AccountHolderName = employeeBankingDto.AccountHolderName;
        Status = employeeBankingDto.Status;
        DeclineReason = employeeBankingDto.DeclineReason;
        File = employeeBankingDto.File;
        LastUpdateDate = employeeBankingDto.LastUpdateDate;
        PendingUpdateDate = employeeBankingDto.PendingUpdateDate;
    }

    public EmployeeBankingDto ToDto()
    {
        return new EmployeeBankingDto(
            Id,
            EmployeeId,
            BankName,
            Branch,
            AccountNo,
            AccountType,
            AccountHolderName,
            Status,
            DeclineReason,
            File,
            LastUpdateDate,
            PendingUpdateDate
            );
    }
}
