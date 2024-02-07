﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;
using Xunit;

namespace RGO.UnitOfWork.Tests.Repositories
{
    public class EmployeeDateRepositoryUnitTest : BaseRepositoryUnitTest
    {
        private readonly Mock<DbSet<EmployeeDate>> _mockDbSet;
        private readonly Mock<DatabaseContext> _mockDbContext;
        private readonly EmployeeDateRepository _repository;
        EmployeeTypeDto employeeTypeDto;
        EmployeeType employeeType;
        EmployeeAddressDto employeeAddressDto;
        EmployeeDto employeeDto;
        Employee employee;

        public EmployeeDateRepositoryUnitTest()
        {
            _mockDbSet = new Mock<DbSet<EmployeeDate>>();
            _mockDbContext = new Mock<DatabaseContext>();
            _repository = new EmployeeDateRepository(_mockDbContext.Object);

            _mockDbContext.Setup(m => m.Set<EmployeeDate>()).Returns(_mockDbSet.Object);

            employeeTypeDto = new EmployeeTypeDto(1, "Developer");
            employeeType = new EmployeeType(employeeTypeDto);
            employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            employee = new Employee();
            employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                1, false, "None", 3, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matthew", "MT",
                "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                new DateTime(), null, Race.Black, Gender.Male, null,
                $"test1@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
        }

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
        public async Task GetAllWithoutCriteriaTest()
        {
            Employee employee = new Employee(employeeDto, employeeTypeDto);
            var employeeDateEntities = new List<EmployeeDate>
            {
               new EmployeeDate
               {
                    Id = 1,
                    Employee = employee,
                    Subject = "Meeting",
                    Note = "Discuss project details",
                    Date = new DateOnly(2024, 2, 6)
               }
            };

            var queryableEmployeeDateEntities = employeeDateEntities.AsQueryable();

            var mockDbSet = new Mock<DbSet<EmployeeDate>>();
            mockDbSet.As<IAsyncEnumerable<EmployeeDate>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new AsyncEnumeratorWrapper<EmployeeDate>(employeeDateEntities.GetEnumerator()));

            mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.Provider).Returns(queryableEmployeeDateEntities.Provider);
            mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.Expression).Returns(queryableEmployeeDateEntities.Expression);
            mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.ElementType).Returns(queryableEmployeeDateEntities.ElementType);
            mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.GetEnumerator()).Returns(queryableEmployeeDateEntities.GetEnumerator());

            _mockDbContext.Setup(m => m.Set<EmployeeDate>()).Returns(mockDbSet.Object);

            var repository = new EmployeeDateRepository(_mockDbContext.Object);
            var result = await repository.GetAll();

            Assert.NotNull(result);
            Assert.Single(result);
            var expectedResult = employeeDateEntities.Select(e => e.ToDto());
            Assert.Equivalent(expectedResult, result);
        }

        //[Fact]
        //public async Task GetAllWithCriteriaTest()
        //{
        //    Employee employee = new Employee(employeeDto, employeeTypeDto);

        //    var employeeDateEntities = new List<EmployeeDate>
        //    {
        //        new EmployeeDate
        //        {
        //            Id = 1,
        //            Employee = employee,
        //            Subject = "Meeting",
        //            Note = "Discuss project details",
        //            Date = new DateOnly(2024, 2, 6)
        //        }
        //    };

        //    var mockDbSet = new Mock<DbSet<EmployeeDate>>();
        //    var queryableEmployeeDateEntities = employeeDateEntities.AsQueryable();

        //    mockDbSet.As<IAsyncEnumerable<EmployeeDate>>()
        //        .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
        //        .Returns(new TestAsyncEnumerable<EmployeeDate>(employeeDateEntities.GetEnumerator()));

        //    mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.Provider).Returns(queryableEmployeeDateEntities.Provider);
        //    mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.Expression).Returns(queryableEmployeeDateEntities.Expression);
        //    mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.ElementType).Returns(queryableEmployeeDateEntities.ElementType);
        //    mockDbSet.As<IQueryable<EmployeeDate>>().Setup(m => m.GetEnumerator()).Returns(queryableEmployeeDateEntities.GetEnumerator());

        //    _mockDbContext.Setup(m => m.Set<EmployeeDate>()).Returns(mockDbSet.Object);

        //    var repository = new EmployeeDateRepository(_mockDbContext.Object);
        //    var result = await repository.GetAll(x => x.Id == 1);

        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    var expectedResult = employeeDateEntities.Where(e => e.Id == 1).Select(e => e.ToDto());
        //    Assert.Equal(expectedResult, result);
        //}

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
        public async Task AddRangeTest()
        {
            var mockEntities = new List<EmployeeDate>
            {              
                new EmployeeDate {  Id = 1,
                    Employee = employee,
                    Subject = "Meeting",
                    Note = "Discuss project details",
                    Date = new DateOnly(2024, 2, 6)},              
            };

            var dbSetMock = new Mock<DbSet<EmployeeDate>>();
            var mockDbContext = new Mock<DatabaseContext>();
            
            mockDbContext.Setup(x => x.Set<EmployeeDate>()).Returns(dbSetMock.Object);
            var repository = new EmployeeDateRepository(mockDbContext.Object);
            await repository.AddRange(mockEntities);

            dbSetMock.Verify(x=> x.AddRangeAsync(It.Is<IEnumerable<EmployeeDate>>(entities => entities.SequenceEqual(mockEntities)),
                    It.IsAny<CancellationToken>()), Times.Once); 
        }
    }
}
