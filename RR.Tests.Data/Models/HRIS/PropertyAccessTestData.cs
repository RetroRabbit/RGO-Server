using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS
{
    public class PropertyAccessTestData
    {

        public static PropertyAccess PropertyAccessOne = new()
        {
            Id = 1,
            Role = new Role(),
            RoleId = 1,
            Table = "Employee",
            Field = "Name",
            AccessLevel = PropertyAccessLevel.write
        };
        
        public static PropertyAccess PropertyAccessTwo = new()
        {
            Id = 2,
            Role = new Role(),
            RoleId = 2,
            Table = "Employee",
            Field = "surname",
            AccessLevel = PropertyAccessLevel.write
        };
        
        public static PropertyAccess PropertyAccessThree = new()
        {
            Id = 3,
            Role = new Role(),
            RoleId = 3,
            Table = "Employee",
            Field = "email",
            AccessLevel = PropertyAccessLevel.write
        };

        public static List<PropertyAccess> PropertyAccessList = new()
        {
            PropertyAccessOne,
            PropertyAccessTwo,
            PropertyAccessThree
        };
    }
}
