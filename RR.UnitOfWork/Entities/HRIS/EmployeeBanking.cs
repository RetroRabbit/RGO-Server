﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeBanking")]
public class EmployeeBanking : IModel
{
    public EmployeeBanking()
    {
    }

    public EmployeeBanking(EmployeeBankingDto employeeBankingDto)
    {
        Id = employeeBankingDto.Id;
        EmployeeId = employeeBankingDto.EmployeeId;
        BankName = employeeBankingDto.BankName;
        Branch = employeeBankingDto.Branch;
        AccountNo = employeeBankingDto.AccountNo;
        AccountType = employeeBankingDto.AccountType;
        Status = employeeBankingDto.Status;
        DeclineReason = employeeBankingDto.DeclineReason;
        File = employeeBankingDto.File;
        LastUpdateDate = employeeBankingDto.LastUpdateDate;
        PendingUpdateDate = employeeBankingDto.PendingUpdateDate;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("bankName")] public string? BankName { get; set; }

    [Column("branch")] public string? Branch { get; set; }

    [Column("accountNo")] public string? AccountNo { get; set; }

    [Column("accountType")] public EmployeeBankingAccountType AccountType { get; set; }

    [Column("status")] public BankApprovalStatus Status { get; set; }

    [Column("reason")] public string? DeclineReason { get; set; }

    [Column("file")] public string? File { get; set; }

    [Column("lastUpdateDate")] public DateOnly LastUpdateDate { get; set; }

    [Column("pendingUpdateDate")] public DateOnly PendingUpdateDate { get; set; }

    public virtual Employee? Employee { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeBankingDto ToDto()
    {
        return new EmployeeBankingDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            BankName = BankName,
            Branch = Branch,
            AccountNo = AccountNo,
            AccountType = AccountType,
            Status = Status,
            DeclineReason = DeclineReason,
            File = File,
            LastUpdateDate = LastUpdateDate,
            PendingUpdateDate = PendingUpdateDate
        };
    }
}
