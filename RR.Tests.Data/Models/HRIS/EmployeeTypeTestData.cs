using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeTypeTestData
{
    public static EmployeeType DeveloperType = new()
    {
        Id = 1,
        Name = "Developer"
    };
    public static EmployeeType DesignerType = new()
    {
        Id = 2,
        Name = "Designer"
    };
    public static EmployeeType ScrumType = new()
    {
        Id = 3,
        Name = "Scrum Master"
    };
    public static EmployeeType OtherType = new()
    { 
        Id = 4,
        Name = "Other"
    };
    public static EmployeeType PeopleChampionType = new()
    {
        Id = 5,
        Name = "People Champion"
    };

    public static EmployeeType NullType = new()
    {
        Id = 0,
        Name = ""
    };
}
