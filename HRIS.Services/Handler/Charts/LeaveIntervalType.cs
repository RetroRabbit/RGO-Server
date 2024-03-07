using HRIS.Models;

namespace HRIS.Services.Services;

public class LeaveIntervalType : BaseDataType
{
    public override string Name => "LeaveInterval";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("LeaveInterval");
        if (prop!.GetValue(employee) == null)
            return null!;

        if (prop.GetValue(employee)!.ToString() != "1")
            return $"{prop.GetValue(employee)} Days, ";
        return $"{prop.GetValue(employee)} Day, ";
    }
}