using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.Services.Services;

public class PayRateType : BaseDataType
{
    public override string Name => "PayRate";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("PayRate");
        if (prop == null)
            return null;

        return $"PayRate {prop.GetValue(employee).ToString()}, ";
    }

}