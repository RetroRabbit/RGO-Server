using HRIS.Models;

namespace HRIS.Services.Services;

public class PayRateType : BaseDataType
{
    public override string Name => "PayRate";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("PayRate");

        if(prop != null)
        {
            if (prop.GetValue(employee) == null)
                return null!;

            return $"PayRate {prop.GetValue(employee)}, ";
        }

        return null!;
    }
}