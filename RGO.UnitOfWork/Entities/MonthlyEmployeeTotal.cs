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

        [Column("developerTotal")]
        public int DeveloperTotal { get; set; }

        [Column("designerTotal")]
        public int DesignerTotal { get; set; }

        [Column("scrumMasterTotal")]
        public int ScrumMasterTotal { get; set; }

        [Column("businessSupportTotal")]
        public int BusinessSupportTotal { get; set; }

        [Column("month")]
        public string Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        public MonthlyEmployeeTotal() { }

        public MonthlyEmployeeTotal(MonthlyEmployeeTotalDto monthlyEmployeeTotalDto)
        {
            Id = monthlyEmployeeTotalDto.Id;
            EmployeeTotal = monthlyEmployeeTotalDto.EmployeeTotal;
            DeveloperTotal= monthlyEmployeeTotalDto.DeveloperTotal;
            DesignerTotal= monthlyEmployeeTotalDto.DesignerTotal;
            ScrumMasterTotal = monthlyEmployeeTotalDto.ScrumMasterTotal;
            BusinessSupportTotal = monthlyEmployeeTotalDto.BusinessSupportTotal;
            Month = monthlyEmployeeTotalDto.Month;
            Year = monthlyEmployeeTotalDto.Year;
        }

        public MonthlyEmployeeTotalDto ToDto()
        {
            return new MonthlyEmployeeTotalDto(
            Id,
            EmployeeTotal,
            DeveloperTotal,
            DesignerTotal,
            ScrumMasterTotal,
            BusinessSupportTotal,
            Month,
            Year
            );
        }
    }
}
