﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("MonthlyEmployeeTotal")]
public class MonthlyEmployeeTotal : IModel
{
    public MonthlyEmployeeTotal()
    {
    }

    public MonthlyEmployeeTotal(MonthlyEmployeeTotalDto monthlyEmployeeTotalDto)
    {
        Id = monthlyEmployeeTotalDto.Id;
        EmployeeTotal = monthlyEmployeeTotalDto.EmployeeTotal;
        DeveloperTotal = monthlyEmployeeTotalDto.DeveloperTotal;
        DesignerTotal = monthlyEmployeeTotalDto.DesignerTotal;
        ScrumMasterTotal = monthlyEmployeeTotalDto.ScrumMasterTotal;
        BusinessSupportTotal = monthlyEmployeeTotalDto.BusinessSupportTotal;
        Month = monthlyEmployeeTotalDto.Month;
        Year = monthlyEmployeeTotalDto.Year;
    }

    [Column("employeeTotal")] public int EmployeeTotal { get; set; }

    [Column("developerTotal")] public int DeveloperTotal { get; set; }

    [Column("designerTotal")] public int DesignerTotal { get; set; }

    [Column("scrumMasterTotal")] public int ScrumMasterTotal { get; set; }

    [Column("businessSupportTotal")] public int BusinessSupportTotal { get; set; }

    [Column("month")] public string? Month { get; set; }

    [Column("year")] public int Year { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public MonthlyEmployeeTotalDto ToDto()
    {
        return new MonthlyEmployeeTotalDto
        {
            Id = this.Id,
            EmployeeTotal = this.EmployeeTotal,
            DeveloperTotal = this.DeveloperTotal,
            DesignerTotal = this.DesignerTotal,
            ScrumMasterTotal = this.ScrumMasterTotal,
            BusinessSupportTotal = this.BusinessSupportTotal,
            Month = this.Month,
            Year = this.Year
        };
    }
}
