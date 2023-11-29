using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.Services.Services;

public class SalaryType : BaseDataType
{
    public override string Name => "Salary";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("Salary");
        if (prop == null)
            return null;

        return $"Salary {prop.GetValue(employee)}, ";
    }

}