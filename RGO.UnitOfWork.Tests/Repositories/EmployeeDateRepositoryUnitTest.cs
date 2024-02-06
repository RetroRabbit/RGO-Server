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

        [Fact]
        public async Task DeletePassTest()
        { 
            var dbContextMock = new Mock<DatabaseContext>();
            var employeeDateMock = new EmployeeDate { Id = 1 };
            var dbSetMock = new Mock<DbSet<EmployeeDate>>();
            dbSetMock.Setup(x => x.FindAsync(1)).ReturnsAsync(employeeDateMock);
            dbContextMock.Setup(x => x.Set<EmployeeDate>()).Returns(dbSetMock.Object);

            var repository = new EmployeeDateRepository(dbContextMock.Object);

            var result = await repository.Delete(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            dbSetMock.Verify(x => x.Remove(It.IsAny<EmployeeDate>()), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteFailThrowExeptionTest()
        {
            var dbContextMock = new Mock<DatabaseContext>();
            var dbSetMock = new Mock<DbSet<EmployeeDate>>();
            dbSetMock.Setup(x => x.FindAsync(1)).ReturnsAsync((EmployeeDate)null);
            dbContextMock.Setup(x => x.Set<EmployeeDate>()).Returns(dbSetMock.Object);

            var repository = new EmployeeDateRepository(dbContextMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.Delete(1));
            dbSetMock.Verify(x => x.Remove(It.IsAny<EmployeeDate>()), Times.Never);
            dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task UpdateEntityTest()
        { 
            
        }

    }
}
