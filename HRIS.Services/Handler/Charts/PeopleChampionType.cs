﻿using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HRIS.Services.Services;

public class PeopleChampionType : BaseDataType
{
    public override string Name => "PeopleChampion";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("PeopleChampion");
        if (prop == null || employee.PeopleChampion == null)
            return null!;

        var id = (int)prop.GetValue(employee)!;
        var task = services.GetService<IEmployeeService>()!.GetEmployeeById(id);
        var champion = task.GetAwaiter().GetResult();
        return champion!.Name + ' ' + champion.Surname + ", ";
    }
}