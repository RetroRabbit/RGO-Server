using RGO.Models;

namespace RGO.Tests.Data.Models;

public class EmployeeAddressTestData
{
    public static EmployeeAddressDto EmployeeAddressDto = new (1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    public static EmployeeAddressDto EmployeeAddressDto2 = new (1, "1", "Complex Name 1", "Street Number 1", "Suburb or District 1", "City 1", "Country 1", "Province 1", "0001");
    public static EmployeeAddressDto EmployeeAddressDto3 = new(2, "2", "Complex Name 2", "Street Number 2", "Suburb or District 2", "City 2", "Country 2", "Province 2", "0002");
}