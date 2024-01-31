using Microsoft.Extensions.DependencyInjection;
using RGO.Models;

namespace RGO.Services.Services;

public abstract class BaseDataType
{
    public abstract string Name { get; }
    public abstract string GenerateData(EmployeeDto employee, IServiceProvider services);

    public static List<BaseDataType> Charts => new List<BaseDataType>()
    {
        new AgeType(),
        new PeopleChampionType(),
        new LevelType(),
        new SalaryType(),
        new PayRateType(),
        new LeaveIntervalType(),
    };

    public static bool HasCustom(string dataType)
    {
        return Charts.Any(x => x.Name == dataType);
    }

    public static BaseDataType GetCustom(string dataType)
    {
        return Charts.FirstOrDefault(x => x.Name == dataType);
    }
}