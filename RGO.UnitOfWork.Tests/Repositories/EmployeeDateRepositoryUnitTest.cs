using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;
using Xunit;

namespace RGO.UnitOfWork.Tests.Repositories
{
    public class EmployeeDateRepositoryUnitTest
    {
        [Fact]
        public async Task GetById()
        {
            var mockDbSet = new Mock<DbSet<EmployeeDate>>();
            var mockDbContext = new Mock<DatabaseContext>();
            mockDbContext.Setup(m => m.Set<EmployeeDate>()).Returns(mockDbSet.Object);

            var repository = new EmployeeDateRepository(mockDbContext.Object);
            var employeeDate = new EmployeeDate { Id = 1 };

            mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(employeeDate);
          
            var result = await repository.GetById(1);

            Assert.NotNull(result);
        }
    }
}
