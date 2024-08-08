using Auth0.ManagementApi.Models;
using HRIS.Models;
using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities
{
    public class DataReportUnitTests
    {
        [Fact]
        public void DataReportTest()
        {
            var dataReport = new DataReport();
            Assert.IsType<DataReport>(dataReport);
            Assert.NotNull(dataReport);
        }
        private List<DataReportColumnsDto> GetDataReportColumnsDtos() {
            var dataReportColumn = new DataReportColumnsDto
            {
                Id = 1,
                Name = "Level",
                Prop = "Level",
                Sequence = 4,
                IsCustom = true,
                FieldType = "2",
                Status = 0
            };
            var dataReportColumnList = new List<DataReportColumnsDto>();
            dataReportColumnList.Add(dataReportColumn);

            return dataReportColumnList;
        }
        private List<DataReportFilterDto> GetDataReportFilterDto()
        {
            var dataReportFilter = new DataReportFilterDto
            {
                Id = 1,
                Table = "Employee",
                Column = "employeeTypeId",
                Condition = "IN",
                Value = "(1,2,3)",
                Select = "id",
                ReportFilterName = "ET01",
                ReportId =1,
                Status = 0
            };
            var dataReportFilterList = new List<DataReportFilterDto>();
            dataReportFilterList.Add(dataReportFilter);

            return dataReportFilterList;
        }
        private List<DataReportAccessDto> GetDataReportAccessDto()
        {
            var dataReportAccess = new DataReportAccessDto
            {
                Id = 1,
                ReportId = 1,
                EmployeeId = 1,
                RoleId = 1,
                ViewOnly = false
            };
            var dataReportAccessList = new List<DataReportAccessDto>();
            dataReportAccessList.Add(dataReportAccess);

            return dataReportAccessList;
        }

        [Fact]
        public void DataReporttoDTOTest() {
            var dataReportDto = new DataReportDto
            {
                Id = 1,
                Name = "TestReport",
                Code = "TR123",
                Status = 0,
                DataReportColumns = GetDataReportColumnsDtos(),
                DataReportFilter = GetDataReportFilterDto(),
                DataReportValues = new List<DataReportValuesDto>(),
                DataReportAccess = GetDataReportAccessDto()
            };

            var dataReport = new DataReport(dataReportDto);

            var dto = dataReport;

            Assert.Equivalent(dto.ToDto(), dataReport);
        }
    }
}
