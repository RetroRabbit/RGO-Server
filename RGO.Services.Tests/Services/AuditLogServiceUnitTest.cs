using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.Tests.Data.Models;
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
    public class AuditLogServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AuditLogService _auditLogService;
        private readonly Mock<IServiceProvider> _services;
        private AuditLogService auditLogService;
        private readonly Mock<IAuditLogService> _auditLogServiceMock;
        private readonly AuditLogDto _auditLogDto;
        private readonly EmployeeDto _employee;

        public AuditLogServiceUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditLogServiceMock = new Mock<IAuditLogService>();
            _services = new Mock<IServiceProvider>();
            _auditLogService = new AuditLogService(_unitOfWork.Object);

            EmployeeDto? editFor = null;
            EmployeeDto? editBy = null;

            //EmployeeTypeDto employeeTypeDto = new(1, "Developer");
            //_employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
            //   null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Kamo", "K.G.",
            //   "Smith", new DateTime(), "South Africa", "South African", "1234457899", " ",
            //   new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null!,
            //   "ksmith@retrorabbit.co.za", "kmaosmith@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            _auditLogDto = new AuditLogDto
            {
                Id = 0,
                EditFor = editFor,
                EditBy = editBy,
                EditDate = new DateTime(),
                Description = ""
            };
        }

        [Fact]
        public async Task GetAllAuditLogTest()
        {
            List<AuditLogDto> auditlogs = new List<AuditLogDto>() { _auditLogDto };

            _unitOfWork.Setup(a => a.AuditLog.GetAll(null)).Returns(Task.FromResult(auditlogs));
            var result = await _auditLogService.GetAllAuditLogs();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(auditlogs, result);

        }

        [Fact]
        public async Task GetAuditLogByEditedByIdTest()
        {

        }

        [Fact]
        public async Task SaveAuditLogsTest()
        {
            List<AuditLogDto> auditlog = new List<AuditLogDto>() { _auditLogDto };
            _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>())).Returns(Task.FromResult(auditlog));

            _unitOfWork.Setup(x => x.AuditLog.Add(It.IsAny<AuditLog>())).Returns(Task.FromResult(_auditLogDto));

            var result = await _auditLogService.SaveAuditLog(_auditLogDto);

            Assert.NotNull(result);
            Assert.Equal(_auditLogDto, result);
            _unitOfWork.Verify(x => x.AuditLog.Add(It.IsAny<AuditLog>()));
        }

        [Fact]
        public async Task UpdateAuditLogTest()
        {

        }

        [Fact]
        public async Task DeleteAuditLogTest()
        {
            List<AuditLogDto> auditlog = new List<AuditLogDto>() { _auditLogDto };
            _unitOfWork.Setup(x => x.AuditLog.GetAll(null)).Returns(Task.FromResult(auditlog));

            _unitOfWork.Setup(x => x.AuditLog.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(_auditLogDto));

            var result = await _auditLogService.DeleteAuditLog(_auditLogDto);
            Assert.NotNull(result);
            Assert.Equal(_auditLogDto, result);
            _unitOfWork.Verify(r => r.AuditLog.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CheckAuditLogTest()
        {

        }

    }
}
