using HRIS.Models;

namespace HRIS.Services.Services;

public class AgeType : BaseDataType
{
    public override string Name => "Age";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var dobPropertyInfo = typeof(EmployeeDto).GetProperty("DateOfBirth");
        var employee_dob = (DateTime)dobPropertyInfo!.GetValue(employee)!;
        var dob = new DateOnly(employee_dob.Year, employee_dob.Month, employee_dob.Day);
        var age = CalculateAge(dob);
        return $"Age {age}, ";
    }

    private int CalculateAge(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dob.Year;

        if (today.DayOfYear < dob.DayOfYear)
            age--;

        return age;
    }
}