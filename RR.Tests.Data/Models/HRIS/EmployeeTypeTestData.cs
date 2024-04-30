using HRIS.Models;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeTypeTestData
{
    public static EmployeeTypeDto DeveloperType = new EmployeeTypeDto
    {
        Id = 1,
        Name = "Developer"
    };
    public static EmployeeTypeDto DesignerType = new EmployeeTypeDto
    {
        Id = 2,
        Name = "Designer"
    };
    public static EmployeeTypeDto ScrumType = new EmployeeTypeDto
    {
        Id = 3,
        Name = "Scrum Master"
    };
    public static EmployeeTypeDto OtherType = new EmployeeTypeDto
    { 
        Id = 4,
        Name = "Other"
    };
    public static EmployeeTypeDto PeopleChampionType = new EmployeeTypeDto
    {
        Id = 5,
        Name = "People Champion"
    };
}
