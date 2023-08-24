using RGO.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities
{
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
        public int AccountType { get; set; }

        [Column("accountHolderName")]
        public string AccountHolderName { get; set; }

        public virtual Employee Employee { get; set; }

        public EmployeeBanking() { }
        public EmployeeBanking(EmployeeBankingDto employeeBankingDto)
        {
            Id = employeeBankingDto.Id;
            EmployeeId = employeeBankingDto.Employee.Id;
            BankName = employeeBankingDto.BankName;
            Branch = employeeBankingDto.Branch;
            AccountNo = employeeBankingDto.AccountNo;
            AccountType = employeeBankingDto.AccountType;
            AccountHolderName = employeeBankingDto.AccountHolderName;
        }

        public EmployeeBankingDto ToDto()
        {
            return new EmployeeBankingDto(
                Id,
                Employee.ToDto(),
                BankName,
                Branch,
                AccountNo,
                AccountType,
                AccountHolderName
                );
        }
    }
}
