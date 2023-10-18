using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.Services.Tests.Services
{
    public class EmployeeAddressServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly EmployeeAddressService _employeeAddressService;
        private readonly EmployeeDto _employeeDto;

        public EmployeeAddressServiceUnitTest()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _employeeAddressService = new EmployeeAddressService(_dbMock.Object);
            EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            _employeeDto = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", null,
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, "",
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");
        }


        [Fact]
        public async Task SaveAddressFailTest()
        {
            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee> { }.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.SaveEmployeeAddress(new EmployeeAddressDto(1, _employeeDto,
            RGO.Models.Enums.AddressType.City, "2", "Complex", "2", "Street Name", "Suburb", "City", "1620")));
        }

        [Fact]
        public async Task SaveAddressPassTest()
        {
            var employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            var employeeAddressDto = new EmployeeAddressDto(1, _employeeDto,
            RGO.Models.Enums.AddressType.City, "2", "Complex", "2", "Street Name", "Suburb", "City", "1620");

            var employeeDto = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", null,
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, "",
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            List<Employee> employeeList = new List<Employee> { new Employee(employeeDto, employeeTypeDto) };

            List<EmployeeAddress> employeeAddressList = new List<EmployeeAddress>
            { new EmployeeAddress(employeeAddressDto)};

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
                .Returns(new List<EmployeeAddress>().AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(employeeAddressDto));

            var result = await _employeeAddressService.SaveEmployeeAddress(employeeAddressDto);

            _dbMock.Verify(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(employeeAddressDto, result);
        }

        [Fact]
        public async Task GetAddressFailTest()
        {
            var employeeAddress = new EmployeeAddressDto(1, _employeeDto, RGO.Models.Enums.AddressType.City, "2", "Complex", "2",
            "Street Name", "Suburb", "City", "1620");

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee> { }.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
            .Returns(new List<EmployeeAddress> { }.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.GetEmployeeAddress(employeeAddress.Id));
        }

        [Fact]
        public async Task GetAddressPassTest()
        {
            var employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            var employeeDto = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", null,
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, "",
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

            var employeeAddressDto = new EmployeeAddressDto(1, employeeDto, RGO.Models.Enums.AddressType.City, "2", "Complex", "2",
            "Street Name", "Suburb", "City", "1620");

            List<EmployeeType> employeeTypeList = new List<EmployeeType> {new EmployeeType(employeeTypeDto) };

            List<Employee> employeeList = new List<Employee> { new Employee(employeeDto, employeeTypeDto)};
            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.AsQueryable().BuildMock());

            List<EmployeeAddress> employeeAddressList = new List<EmployeeAddress> { new EmployeeAddress(employeeAddressDto) };

            employeeAddressList[0].Employee = employeeList[0];
            employeeAddressList[0].Employee.EmployeeType = employeeTypeList[0];

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
            .Returns(employeeAddressList.AsQueryable().BuildMock());

            var result = await _employeeAddressService.GetEmployeeAddress(employeeAddressDto.Id);

            Assert.NotNull(result);
            Assert.Equal(employeeAddressDto, result);
            _dbMock.Verify(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAddressFailTest()
        {
            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee> { }.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.UpdateEmployeeAddress(new EmployeeAddressDto(1, _employeeDto,
            RGO.Models.Enums.AddressType.City, "2", "Complex", "2", "Street Name", "Suburb", "City", "1620")));
        }

        [Fact]
        public async Task UpdateAddressPassTest()
        {
            var employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            var employeeDto = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", null,
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, "",
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

            var employeeAddressDto = new EmployeeAddressDto(1, employeeDto, RGO.Models.Enums.AddressType.City, "2", "Complex", "2",
            "Street Name", "Suburb", "City", "1620");

            List<EmployeeType> employeeTypeList = new List<EmployeeType> { new EmployeeType(employeeTypeDto) };

            List<Employee> employeeList = new List<Employee> { new Employee(employeeDto, employeeTypeDto) };

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            List<EmployeeAddress> employeeAddressList = new List<EmployeeAddress> { new EmployeeAddress(employeeAddressDto) };

            employeeAddressList[0].Employee = employeeList[0];
            employeeAddressList[0].Employee.EmployeeType = employeeTypeList[0];

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
            .Returns(employeeAddressList.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(employeeAddressDto));

            var result = await _employeeAddressService.UpdateEmployeeAddress(employeeAddressDto);

            Assert.NotNull(result);
            Assert.Equal(employeeAddressDto, result);

            _dbMock.Verify(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAddressFailTest()
        {
            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee> { }.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.DeleteEmployeeAddress(new EmployeeAddressDto(1, _employeeDto,
            RGO.Models.Enums.AddressType.City, "2", "Complex", "2", "Street Name", "Suburb", "City", "1620")));
        }

        [Fact]
        public async Task DeleteAddressPassTest()
        {
            var employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            var employeeDto = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", null,
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, "",
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

            var employeeAddressDto = new EmployeeAddressDto(1, employeeDto, RGO.Models.Enums.AddressType.City, "2", "Complex", "2",
            "Street Name", "Suburb", "City", "1620");

            List<EmployeeType> employeeTypeList = new List<EmployeeType> { new EmployeeType(employeeTypeDto) };

            List<Employee> employeeList = new List<Employee> { new Employee(employeeDto, employeeTypeDto) };

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.AsQueryable().BuildMock());

            List<EmployeeAddress> employeeAddressList = new List<EmployeeAddress> { new EmployeeAddress(employeeAddressDto) };

            employeeAddressList[0].Employee = employeeList[0];
            employeeAddressList[0].Employee.EmployeeType = employeeTypeList[0];

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
            .Returns(employeeAddressList.AsQueryable().BuildMock());

            _dbMock.Setup(r => r.EmployeeAddress.Delete(employeeAddressDto.Id)).Returns(Task.FromResult(employeeAddressDto));

            var result = await _employeeAddressService.DeleteEmployeeAddress(employeeAddressDto);

            Assert.NotNull(result);
            Assert.Equal(employeeAddressDto, result);

            _dbMock.Verify(x => x.EmployeeAddress.Delete(It.IsAny<int>()), Times.Once);
        }

    }
}
