using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace RGO.Services.Services;

public class LeaveIntervalType : BaseDataType
{
    public override string Name => "LeaveInterval";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("LeaveInterval");
        if (prop.GetValue(employee) == null)
            return null;

        if (prop.GetValue(employee).ToString() != "1")
            return $"{prop.GetValue(employee)} Days, ";
        else
            return $"{prop.GetValue(employee)} Day, ";
    }
}