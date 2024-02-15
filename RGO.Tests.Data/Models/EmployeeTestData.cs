using RGO.Models;
using RGO.Models.Enums;

namespace RGO.Tests.Data.Models;

public class EmployeeTestData
{
    public static EmployeeDto EmployeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, EmployeeTypeTestData.DeveloperType, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Smith", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, EmployeeAddressTestData.EmployeeAddressDto, EmployeeAddressTestData.EmployeeAddressDto, null, null, null);

    public static EmployeeDto EmployeeDto2 = new(2, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, EmployeeTypeTestData.DeveloperType, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Smith", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, EmployeeAddressTestData.EmployeeAddressDto, EmployeeAddressTestData.EmployeeAddressDto, null, null, null);

    public static EmployeeDto EmployeeDto3 = new(3, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, EmployeeTypeTestData.DeveloperType, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Smith", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, EmployeeAddressTestData.EmployeeAddressDto, EmployeeAddressTestData.EmployeeAddressDto, null, null, null);

    public static EmployeeDto EmployeeDto4 = new EmployeeDto(4, "001", "34434434", new DateTime(), new DateTime(),
        2, false, "None", 4, EmployeeTypeTestData.DeveloperType, "Notes", 1, 28, 128, 100000, "Dotty", "D",
        "Missile", new DateTime(), "South Africa", "South African", "5522522655", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", 1, 3, EmployeeAddressTestData.EmployeeAddressDto, EmployeeAddressTestData.EmployeeAddressDto, null, null, null);
}