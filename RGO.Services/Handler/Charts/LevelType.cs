﻿using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.Services.Services;

public class LevelType : BaseDataType
{
    public override string Name => "Level";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("Level");
        if (prop.GetValue(employee) == null)
            return null;

        return $"Level {prop.GetValue(employee)}, ";
    }
}