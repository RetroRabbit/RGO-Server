﻿using Microsoft.AspNetCore.Mvc;
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
    /*public class EmployeeAddressServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly EmployeeAddressService _employeeAddressService;
        private readonly EmployeeDto _employeeDto;

        public EmployeeAddressServiceUnitTest()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _employeeAddressService = new EmployeeAddressService(_dbMock.Object);
        }

        private EmployeeAddressDto CreateAddress( int id = 0)
        {
            return new EmployeeAddressDto(
                id,
                "2",
                "Complex",
                "2",
                "Suburb/District",
                "City",
                "Country",
                "Province",
                "1620");
        }

        [Fact]
        public async Task CheckIfExistsFailTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            bool result = await _employeeAddressService.CheckIfExitsts(address);

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIfExistsPassTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            bool result = await _employeeAddressService.CheckIfExitsts(address);

            Assert.True(result);
        }

        [Fact]
        public async Task GetFailTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Get(address));
        }

        [Fact]
        public async Task GetPassTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
                .Returns(new List<EmployeeAddress> { new EmployeeAddress(address) }.AsQueryable().BuildMock());

            var result = await _employeeAddressService.Get(address);

            Assert.NotNull(result);
            Assert.Equal(address, result);
        }

        [Fact]
        public async Task DeleteFailTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Delete(address));
        }

        [Fact]
        public async Task DeletePassTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
                .Returns(new List<EmployeeAddress> { new EmployeeAddress(address) }.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Delete(It.IsAny<int>())).Returns(Task.FromResult(address));

            var result = await _employeeAddressService.Delete(address);

            Assert.Equal(address, result);
        }

        [Fact]
        public async Task SaveFailTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Save(address));
        }

        [Fact]
        public async Task SavePassTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            _dbMock.Setup(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(address));

            var result = await _employeeAddressService.Save(address);

            Assert.Equal(address, result);
        }

        [Fact]
        public async Task UpdateFailTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Update(address));
        }

        [Fact]
        public async Task UpdatePassTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
                .Returns(new List<EmployeeAddress> { new EmployeeAddress(address) }.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(address));

            var result = await _employeeAddressService.Update(address);

            Assert.Equal(address, result);
        }

        [Fact]
        public async Task GetAllTest()
        {
            var address = CreateAddress(1);

            _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).Returns(new List<EmployeeAddress> { new EmployeeAddress(address) }.AsQueryable().BuildMock());

            var result = await _employeeAddressService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(address, result.First());
        }*/
    }
}
