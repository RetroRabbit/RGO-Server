using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS
{
    public class PropertyAccessTestData
    {

        public static PropertyAccess EmployeeTable1 = new PropertyAccess
        {
            Id = 1,
            Role = new Role(),
            RoleId = 1,
            Table = "Employee",
            Field = "Name",
            AccessLevel = PropertyAccessLevel.write
        };
        public static PropertyAccess EmployeeTable2 = new PropertyAccess
        {
            Id = 2,
            Role = new Role(),
            RoleId = 2,
            Table = "Employee",
            Field = "surname",
            AccessLevel = PropertyAccessLevel.write
        };
        
        public static PropertyAccess EmployeeTable3 = new PropertyAccess
        {
            Id = 3,
            Role = new Role(),
            RoleId = 3,
            Table = "Employee",
            Field = "email",
            AccessLevel = PropertyAccessLevel.write
        };

        public static List<PropertyAccess> PropertyAccessList = new List<PropertyAccess>()
        { EmployeeTable1,
        EmployeeTable2,
        EmployeeTable3};
    }
}
