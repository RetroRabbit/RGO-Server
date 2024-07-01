using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models
{
    public class RoleTestData
    {
        public static List<Role> Roles = new()
        {
            new() { Id = 1, Description = "SuperAdmin" },
            new() { Id = 2, Description = "Admin" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Talent" },
            new() { Id = 5, Description = "Journey" }
        };
        public static Role SuperAdminRole = new() { Id = 1, Description = "SuperAdmin" };
        public static Role AdminRole = new() { Id = 2, Description = "Admin" };
        public static Role EmployeeRole = new() { Id = 3, Description = "Employee" };
        public static Role TalentRole = new() { Id = 4, Description = "Talent" };
        public static Role JourneyRole = new() { Id = 4, Description = "Journey" };
    }
}
