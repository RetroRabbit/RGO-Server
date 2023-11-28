using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.Services.Services;

public class AgeType : BaseDataType
{
    public override string Name => "Age";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var dobPropertyInfo = typeof(EmployeeDto).GetProperty("DateOfBirth");
        if (dobPropertyInfo == null)
            return null;

        var dob = (DateOnly)dobPropertyInfo.GetValue(employee);
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