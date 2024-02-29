using HRIS.Models;

namespace HRIS.Services.Services;

public class SalaryType : BaseDataType
{
    public override string Name => "Salary";

    public override string GenerateData(EmployeeDto? employee, IServiceProvider services)
    {
        if (employee == null)
            return null!;

        if (employee.Salary == null)
            return null!;

        return $"Salary {employee.Salary}, ";
    }
}