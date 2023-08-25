using Microsoft.EntityFrameworkCore.Metadata;
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
    [Table("EmployeeDate")]
    public class EmployeeDate : IModel<EmployeeDateDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employeeId")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }
        public EmployeeDate() { }

        public EmployeeDate(EmployeeDateDto employeeDateDto) { 
            Id = employeeDateDto.Id;
            EmployeeId = employeeDateDto.Employee.Id;
            Code = employeeDateDto.Code;
            Date = employeeDateDto.Date;
        }

        public EmployeeDateDto ToDto()
        {
            return new EmployeeDateDto(
                Id,
                Employee.ToDto(),
                Code,
                Date);
        }
    }
}
