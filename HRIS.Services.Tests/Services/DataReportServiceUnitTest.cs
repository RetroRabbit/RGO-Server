using HRIS.Models.Report.Response;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Services;
using HRIS.Services.Services.Reporting;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HRIS.Services.Tests.Services
{
    public class DataReportServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<AuthorizeIdentity> _identity;
        private readonly Mock<IDataReportService> _dataReportMockService;
        private readonly DataReportService _dataReportService;
        private readonly AuthorizeIdentityMock _authorizeIdentityMock; 
        private readonly Mock<IDataReportHelper> _helperMock;
        private readonly Mock<IDataReportAccessService> _accessMock;
        private readonly List<DataReportListResponse> _dataReportListResponses;
        private readonly DataReportListResponse _dataReportResponse;
        private readonly DataReport _dataReport;
        private readonly List<int> _employeeIds;
        private readonly List<Employee> _employeeList;
        private readonly Employee _employee;


        public DataReportServiceUnitTest() {
            _dbMock = new Mock<IUnitOfWork>();
            _dataReportMockService = new Mock<IDataReportService>();
            _authorizeIdentityMock = new AuthorizeIdentityMock("test@retrorabbit.co.za", "TestUser", "SuperAdmin", 1);
            _helperMock = new Mock<IDataReportHelper>();
            _accessMock = new Mock<IDataReportAccessService>();
            _identity = new Mock<AuthorizeIdentity>();
            _dataReportService = new DataReportService(_authorizeIdentityMock,_dbMock.Object,_helperMock.Object,_accessMock.Object);

            _dataReport = new DataReport
            {
                Id = 3,
                Name = "Test Report",
                Code = "TF01",
                Status = 0
            };

            _dataReportResponse = new DataReportListResponse
            {
                Id=1,
                Name = "Test Report",
                Code = "TF01",
                Status = 0

            };

            _dataReportListResponses = new List<DataReportListResponse>();

            _dataReportListResponses.Add(_dataReportResponse);

            _employeeIds = new List<int> { 1,2,3 };

            _employee = new Employee { 
                Id = 17, 
            };

            _employeeList = new List<Employee>();
            _employeeList.Add(_employee);
        }


        [Fact]
        public async Task GetDataReportListTestPass()
        {
            _dbMock.Setup(x => x.DataReport.GetReportsForEmployee(_authorizeIdentityMock.Email)).ReturnsAsync(_dataReportListResponses);
            
            var result = await _dataReportService.GetDataReportList();

            Assert.Equivalent(_dataReportListResponses, result);
        }

        [Fact]
        public async Task GetDataReportListNotFoundTest()
        {
            _dbMock.Setup(x => x.DataReport.GetReportsForEmployee(It.IsAny<string>())).ReturnsAsync(It.IsAny<List<DataReportListResponse>?>());

            await Assert.ThrowsAsync<CustomException>(() => _dataReportService.GetDataReportList());
        }



        [Fact]
        public async Task DeleteReportfromListTestPass()
        {
            _dbMock.Setup(x => x.DataReport.GetReport("TF01")).ReturnsAsync((_dataReport));
            _dbMock.Setup(x => x.DataReport.Delete(3)).ReturnsAsync((_dataReport));

            var result = await _dataReportService.DeleteReportfromList("TF01");

            Assert.Equivalent(_dataReport.ToDto(), result);
        }

        [Fact]
        public async Task DeleteReportfromListNotFoundTest()
        {
            _dbMock.Setup(x => x.DataReport.GetReport(It.IsAny<string>())).ReturnsAsync(It.IsAny<DataReport>);
            _dbMock.Setup(x => x.DataReport.Delete(It.IsAny<int>())).ReturnsAsync(It.IsAny<DataReport>);

            await Assert.ThrowsAsync<NullReferenceException>(() => _dataReportService.DeleteReportfromList(It.IsAny<string>()));
        }

       /* [Fact]
        public async Task GetDataReportTestPass()
        {
            _dbMock.Setup(x => x.DataReport.GetReport("TF01")).ReturnsAsync((_dataReport));
            _dbMock.Setup(x => x.DataReport.ConfirmAnyAccess(3,17));
            _dataReportMockService.Setup(s => s.IsReportViewOnlyForEmployee(3)).ReturnsAsync(true);
            _helperMock.Setup(x => x.GetEmployeeIdListForReport(_dataReport)).ReturnsAsync(_employeeIds);
            _dbMock.Setup(x => x.GetActiveEmployeeId(_authorizeIdentityMock.Email, "SuperAdmin")).ReturnsAsync(17);
            _identity.Setup(x => x.GetEmployeeId()).ReturnsAsync(17);
            //_authorizeIdentityMock.GetEmployeeId().Result(17);
            *//*
            _helperMock.Setup(x => x.GetEmployeeData(_employeeIds)).ReturnsAsync(_employeeList);
            _helperMock.Setup(x => x.MapEmployeeData(_dataReport, _employeeList)).ReturnsAsync(_dataReport);*//*

            var result = await _dataReportService.GetDataReport("TF01");

            Assert.Equivalent(_dataReport.ToDto(), result);
        }*/


    }
}
