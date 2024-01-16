using Microsoft.Extensions.DependencyInjection;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.Services.Services;

public class PeopleChampionType : BaseDataType
{
    public override string Name => "PeopleChampion";

    public override string GenerateData(EmployeeDto employee, IServiceProvider services)
    {
        var prop = typeof(EmployeeDto).GetProperty("PeopleChampion");
        if (prop == null || employee.PeopleChampion == null)
            return null;

        var id = (int)prop.GetValue(employee);
        var task = services.GetService<IEmployeeService>().GetById(id);
        var champion = task.GetAwaiter().GetResult();
        return champion.Name + ' ' + champion.Surname + ", ";
    }
}