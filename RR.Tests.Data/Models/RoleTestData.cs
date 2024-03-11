using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models
{
    public class RoleTestData
    {
        public List<Role> roleList = new List<Role>
        {
            new() { Id = 1, Description = "SuperAdmin" },
            new() { Id = 2, Description = "Admin" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Talent" },
            new() { Id = 5, Description = "Journey" }
        };
        public Role superAdminRole = new Role { Id = 1, Description = "SuperAdmin" };
        public Role adminRole = new Role { Id = 2, Description = "Admin" };
        public Role employeeRole = new Role { Id = 3, Description = "Employee" };
        public Role talentRole = new Role { Id = 4, Description = "Talent" };
        public Role journeyRole = new Role { Id = 4, Description = "Journey" };
    }
}
