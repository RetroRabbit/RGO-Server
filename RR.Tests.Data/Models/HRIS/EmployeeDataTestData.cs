using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeDataTestData
{
    public static EmployeeData EmployeeDataOne = new()
    {
        Id = 1,
        EmployeeId = 2,
        FieldCodeId = 3,
        Value = "string"
    };

    public static EmployeeData EmployeeDataTwo = new ()
    {
        Id = 2,
        EmployeeId = 3,
        FieldCodeId = 4,
        Value = "string"
    };
}