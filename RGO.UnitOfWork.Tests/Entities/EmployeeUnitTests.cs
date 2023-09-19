using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class EmployeeUnitTests
    {
        [Fact]
        public async Task EmployeeTest()
        {
            var employee = new Employee();
            Assert.IsType<Employee>(employee);
            Assert.NotNull(employee);
        }
    }
}
