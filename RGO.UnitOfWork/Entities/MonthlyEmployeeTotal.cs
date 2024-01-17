using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.Xml.Linq;

namespace RGO.UnitOfWork.Entities
{
    [Table("MonthlyEmployeeTotal")]
    public class MonthlyEmployeeTotal : IModel<MonthlyEmployeeTotalDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employeeTotal")]
        public int EmployeeTotal { get; set; }

        [Column("month")]
        public string Month { get; set; }

        [Column("year")]
        public int Year { get; set; }


        public MonthlyEmployeeTotal() { }

        public MonthlyEmployeeTotal(MonthlyEmployeeTotalDto monthlyEmployeeTotalDto)
        {
            Id = monthlyEmployeeTotalDto.Id;
            EmployeeTotal = monthlyEmployeeTotalDto.EmployeeTotal;
            Month = monthlyEmployeeTotalDto.Month;
            Year = monthlyEmployeeTotalDto.Year;
        }

        public MonthlyEmployeeTotalDto ToDto()
        {
            return new MonthlyEmployeeTotalDto(
            Id,
            EmployeeTotal,
            Month,
            Year
            );
        }
    }
}
