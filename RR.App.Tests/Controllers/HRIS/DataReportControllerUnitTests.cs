using HRIS.Services.Interfaces.Reporting;
using RR.App.Controllers.HRIS;
using Moq;
using Xunit;
using HRIS.Models.Report.Response;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using HRIS.Models.Update;
using HRIS.Models.Report;
using HRIS.Models.Report.Request;

namespace RR.App.Tests.Controllers.HRIS
{
    public class DataReportControllerUnitTests
    {
        private readonly DataReportController _dataReportController;
        private readonly Mock<IDataReportService> _dataReportService;
        private readonly Mock<IDataReportControlService> _control;
        private readonly Mock<IDataReportCreationService> _creation;
        private readonly Mock<IDataReportAccessService> _access;
        private readonly List<DataReportListResponse> _dataReportList;
        private readonly DataReportListResponse _dataReport;
        private readonly UpdateReportCustomValue _updateReportCustomValue;
        private readonly ReportColumnRequest _reportColumnRequest;

        public DataReportControllerUnitTests()
        {
            _dataReportService = new Mock<IDataReportService>();
            _control = new Mock<IDataReportControlService>();
            _access = new Mock<IDataReportAccessService>();
            _creation = new Mock<IDataReportCreationService>();
            _dataReportController = new DataReportController(_dataReportService.Object, _control.Object,_access.Object);


            _dataReport = new DataReportListResponse
            {
                Id = 1,
                Name = "Test Report",
                Code = "TR01",
                Status = 0
            };

            _dataReportList = new List<DataReportListResponse>();
            _dataReportList.Add(_dataReport);

            _updateReportCustomValue = new UpdateReportCustomValue
            {
                ReportId = 1,
                ColumnId = 1,
                EmployeeId = 17,
                Input = "Test Input"
            };

            _reportColumnRequest = new ReportColumnRequest
            {
                Id = 1,
                ReportId = 1,
                MenuId = 5,
                Sequence = 0,
            };


        }

        [Fact]
        public async Task GetDataReportListTestFail()
        {
            _dataReportService.Setup(service => service.GetDataReportList()).ThrowsAsync(new CustomException("No reports found"));

            var result = await Assert.ThrowsAsync<CustomException>(() => _dataReportController.GetDataReportList());
            Assert.Equal("No reports found",result.Message);
        }

        [Fact]
        public async Task GetDataReportTestPass()
        {
            _dataReportService.Setup(service => service.GetDataReport(It.IsAny<string>())).ReturnsAsync(_dataReportList);

            var result = await _dataReportController.GetDataReport("TF01");

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateReportInputTestPass()
        {
            _dataReportService.Setup(service => service.UpdateReportInput(_updateReportCustomValue));

            var result = await _dataReportController.UpdateReportInput(_updateReportCustomValue);

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateReportInputTestFail()
        {
            _dataReportService.Setup(service => service.UpdateReportInput(_updateReportCustomValue))
                .ThrowsAsync(new Exception("An error occurred while updating report"));

            var result = await _dataReportController.UpdateReportInput(_updateReportCustomValue);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while updating report", noFoundResult.Value);
        }


        [Fact]
        public async Task GetColumnMenuTestPass()
        {
            _control.Setup(service => service.GetColumnMenu()).ReturnsAsync(new List<DataReportColumnMenuDto>());

            var result = await _dataReportController.GetColumnMenu();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task GetColumnMenuTestFail()
        {
            _control.Setup(service => service.GetColumnMenu())
                .ThrowsAsync(new Exception("An error occurred while retreiving column menu"));

            var result = await _dataReportController.GetColumnMenu();
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while retreiving column menu", noFoundResult.Value);
        }

        [Fact]
        public async Task AddColumnToReportTestPass()
        {
            _control.Setup(service => service.AddColumnToReport(new ReportColumnRequest())).ReturnsAsync(new DataReportColumnsDto());

            var result = await _dataReportController.AddColumnToReport(_reportColumnRequest);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task AddColumnToReportTestFail()
        {
            _control.Setup(service => service.AddColumnToReport(_reportColumnRequest))
                .ThrowsAsync(new Exception("An error occurred while retreiving data report column"));

            var result = await _dataReportController.AddColumnToReport(_reportColumnRequest);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while retreiving data report column", noFoundResult.Value);
        }

        [Fact]
        public async Task ArchiveColumnFromReportTestPass()
        {
            _control.Setup(service => service.ArchiveColumnFromReport(14));

            var result = await _dataReportController.ArchiveColumnFromReport(14);

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task ArchiveColumnFromReportTestFail()
        {
            _control.Setup(service => service.ArchiveColumnFromReport(14))
                .ThrowsAsync(new Exception("An error occurred while trying to achive a data report column"));

            var result = await _dataReportController.ArchiveColumnFromReport(14);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to achive a data report column", noFoundResult.Value);
        }

        [Fact]
        public async Task MoveColumnOnReportTestPass()
        {
            _control.Setup(service => service.MoveColumnOnReport(_reportColumnRequest)).ReturnsAsync(new DataReportColumnsDto());

            var result = await _dataReportController.MoveColumnOnReport(_reportColumnRequest);

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task MoveColumnOnReportTestFail()
        {
            _control.Setup(service => service.MoveColumnOnReport(It.IsAny<ReportColumnRequest>()))
                .ThrowsAsync(new Exception("The report does not seem to exist."));


            var result = await _dataReportController.MoveColumnOnReport(null);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("The report does not seem to exist.", noFoundResult.Value);
        }

        [Fact]
        public async Task AddOrUpdateReportTestPass()
        {
            _control.Setup(service => service.AddOrUpdateReport(new UpdateReportRequest()));

            var result = await _dataReportController.AddOrUpdateReport(new UpdateReportRequest());

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task AddOrUpdateReportTestFail()
        {
            _control.Setup(service => service.AddOrUpdateReport(It.IsAny<UpdateReportRequest>()))
                .ThrowsAsync(new Exception("An error occurred while updating report"));

            var result = await _dataReportController.AddOrUpdateReport(new UpdateReportRequest());
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while updating report", noFoundResult.Value);
        }

        [Fact]
        public async Task GetReportAccessAvailabilityTestPass()
        {
            _access.Setup(service => service.GetReportAccessAvailability(2));

            var result = await _dataReportController.GetReportAccessAvailability(2);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task GetReportAccessAvailabilityTestFail()
        {
            _access.Setup(service => service.GetReportAccessAvailability(0))
               .ThrowsAsync(new Exception("An error occurred while trying to get data report access availability"));

            var result = await _dataReportController.GetReportAccessAvailability(0);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to get data report access availability", noFoundResult.Value);
        }

        [Fact]
        public async Task AddOrUpdateReportAccessTestPass()
        {
            _access.Setup(service => service.AddOrUpdateReportAccess(new UpdateReportAccessRequest()));

            var result = await _dataReportController.AddOrUpdateReportAccess(new UpdateReportAccessRequest());

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task AddOrUpdateReportAccessTestFail()
        {
            _access.Setup(service => service.AddOrUpdateReportAccess(It.IsAny<UpdateReportAccessRequest>()))
               .ThrowsAsync(new Exception("An error occurred while trying to add or update data report access availability"));

            var result = await _dataReportController.AddOrUpdateReportAccess(new UpdateReportAccessRequest());
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to add or update data report access availability", noFoundResult.Value);
        }

        [Fact]
        public async Task ArchiveReportAccessTestPass()
        {
            _access.Setup(service => service.ArchiveReportAccess(3));

            var result = await _dataReportController.ArchiveReportAccess(3);

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task ArchiveReportAccessTestFail()
        {
            _access.Setup(service => service.ArchiveReportAccess(0))
               .ThrowsAsync(new Exception("An error occurred while trying to archive data report access"));

            var result = await _dataReportController.ArchiveReportAccess(0);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to archive data report access", noFoundResult.Value);
        }

        [Fact]
        public async Task DeleteReportTestPass()
        {
            _dataReportService.Setup(service => service.DeleteReportfromList("T08"));

            var result = await _dataReportController.DeleteReport("T08");

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task DeleteReportTestFail()
        {
            _dataReportService.Setup(service => service.DeleteReportfromList("T08"))
               .ThrowsAsync(new Exception("An error occurred while trying to delete data report access"));

            var result = await _dataReportController.DeleteReport("T08");
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to delete data report access", noFoundResult.Value);
        }

        [Fact]
        public async Task GetDataReportFilterTestPass()
        {
            _control.Setup(service => service.AddOrUpdateReportFilter(It.IsAny<ReportFilterRequest>()));

            var result = await _dataReportController.GetDataReportFilter(new ReportFilterRequest());

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task GetDataReportFilterTestFail()
        {
            _control.Setup(service => service.AddOrUpdateReportFilter(It.IsAny<ReportFilterRequest>()))
               .ThrowsAsync(new Exception("An error occurred while trying to add data report filter"));

            var result = await _dataReportController.GetDataReportFilter(new ReportFilterRequest());
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to add data report filter", noFoundResult.Value);
        }

        [Fact]
        public async Task ArchiveDataReportFilterTestPass()
        {
            _control.Setup(service => service.DeleteReportFilterfromList(2));

            var result = await _dataReportController.ArchiveDataReportFilter(2);

            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task ArchiveDataReportFilterTestFail()
        {
            _control.Setup(service => service.DeleteReportFilterfromList(12))
                .ThrowsAsync(new Exception("An error occurred while trying to delete a data report"));

            var result = await _dataReportController.ArchiveDataReportFilter(12);
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to delete a data report", noFoundResult.Value);
        }

        [Fact]
        public async Task GetDataReportFiltersTestPass()
        {
            _control.Setup(service => service.GetDataReportFilters("T0w"));

            var result = await _dataReportController.GetDataReportFilters("T0w");

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

        }

        [Fact]
        public async Task GetDataReportFiltersTestFail()
        {
            _control.Setup(service => service.GetDataReportFilters("T0w"))
                .ThrowsAsync(new Exception("An error occurred while trying to delete a data report"));

            var result = await _dataReportController.GetDataReportFilters("T0w");
            var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An error occurred while trying to delete a data report", noFoundResult.Value);
        }

    }
}
