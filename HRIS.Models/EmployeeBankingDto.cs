using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeBankingDto
{
    [Required(ErrorMessage = "Employee Banking 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Banking 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Employee Banking 'BankName' field is missing.")]
    public string? BankName { get; set; }
    [Required(ErrorMessage = "Employee Banking 'Branch' field is missing.")]
    public string? Branch { get; set; }
    [Required(ErrorMessage = "Employee Banking 'AccountNo' field is missing.")]
    public string? AccountNo { get; set; }
    [Required(ErrorMessage = "Employee Banking 'AccountType' field is missing.")]
    public EmployeeBankingAccountType AccountType { get; set; }
    public BankApprovalStatus Status { get; set; }
    public string? DeclineReason { get; set; }
    public string? File { get; set; }
    [DataType(DataType.Date)]
    public DateOnly LastUpdateDate { get; set; }
    [DataType(DataType.Date)]
    public DateOnly PendingUpdateDate { get; set; }
}
