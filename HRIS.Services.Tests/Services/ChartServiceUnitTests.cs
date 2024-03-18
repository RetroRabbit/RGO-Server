using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class ChartServiceUnitTests
{
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IServiceProvider> _services;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType employeeType1;
    private readonly EmployeeType employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto employeeTypeDto2;
    private readonly IErrorLoggingService _errorLoggingService;

    public ChartServiceUnitTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _services = new Mock<IServiceProvider>();
        _errorLoggingService = new ErrorLoggingService(_unitOfWork.Object);

        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeDto1 = new EmployeeTypeDto{ Id = 3, Name = "Developer" };
        employeeTypeDto2 = new EmployeeTypeDto{ Id = 7, Name = "People Champion" };
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name!))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name!))
                                .Returns(Task.FromResult(employeeTypeDto2));
        employeeAddressDto =
           new EmployeeAddressDto
           {
               Id = 1,
               UnitNumber = "2",
               ComplexName = "Complex",
               StreetNumber = "2",
               SuburbOrDistrict = "Suburb/District",
               City = "City",
               Country = "Country",
               Province = "Province",
               PostalCode = "1620"
           };
    }

    [Fact]
    public async Task GetAllChartsTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        _unitOfWork.Setup(u => u.Chart.GetAll(null)).ReturnsAsync(new List<ChartDto>());

        var result = await chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.IsType<List<ChartDto>>(result);
        _unitOfWork.Verify(u => u.Chart.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task CreateChartTest()
    {
        var roles = new List<string> { "Developer, Designer" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";

        EmployeeTypeDto developerEmployeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        EmployeeTypeDto designerEmployeeTypeDto = new EmployeeTypeDto { Id = 2, Name = "Designer" };

        var employeeAddressDto =
            new EmployeeAddressDto
            {
                Id = 1,
                UnitNumber = "2",
                ComplexName = "Complex",
                StreetNumber = "2",
                SuburbOrDistrict = "Suburb/District",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "1620"
            };

        EmployeeDto employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = developerEmployeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        EmployeeDto desEmployeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = designerEmployeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };


        var employeeList = new List<Employee>
        {
            new(employeeDto, developerEmployeeTypeDto),
            new(desEmployeeDto, designerEmployeeTypeDto)
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto,
            desEmployeeDto
        };

        var chartDto = new ChartDto
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Data = new List<int> { 1, 1 }
        };


        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task CreateChartTestAll()
    {
        var roles = new List<string> { "All" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";

        EmployeeTypeDto devEmployeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        EmployeeTypeDto desEmployeeTypeDto = new EmployeeTypeDto { Id = 2, Name = "Designer" };

        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        EmployeeDto employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = devEmployeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        EmployeeDto desEmployeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = desEmployeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        var employeeList = new List<Employee>
        {
            new(employeeDto, devEmployeeTypeDto),
            new(desEmployeeDto, desEmployeeTypeDto)
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto,
            desEmployeeDto
        };

        var chartDto = new ChartDto
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Data = new List<int> { 1, 1 }
        };


        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task GetChartDataTest()
    {
        var dataType = new List<string> { "Gender", "Race" };

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        EmployeeType employeeType = new(employeeTypeDto);
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        EmployeeDto employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        var employeeList = new List<EmployeeDto>
        {
            employeeDto
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeList);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.GetChartData(dataType);

        Assert.NotNull(result);
        Assert.IsType<ChartDataDto>(result);
    }

    [Fact]
    public async Task DeleteChartTest()
    {
        var chartId = 1;
        var expectedChartDto = new ChartDto
        {
            Id = chartId,
            Name = "Test",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Data = new List<int> { 1, 1 }
        };


        _unitOfWork.Setup(u => u.Chart.Delete(chartId)).ReturnsAsync(expectedChartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.DeleteChart(chartId);

        Assert.NotNull(result);
        Assert.IsType<ChartDto>(result);
        Assert.Equal(expectedChartDto.Id, result.Id);
        _unitOfWork.Verify(x => x.Chart.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChartTest()
    {
        var chartDtoToUpdate = new ChartDto
        {
            Id = 1,
            Name = "Update",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Data = new List<int> { 1, 1 }
        };

        var existingCharts = new List<ChartDto>
        {
            new ChartDto
            {
                Id = 1,
                Name = "Existing Chart",
                Type = "Existing Type",
                DataTypes = chartDtoToUpdate.DataTypes,
                Labels = chartDtoToUpdate.Labels,
                Data = chartDtoToUpdate.Data
            }
        };

        _unitOfWork.Setup(x => x.Chart.GetAll(null)).Returns(Task.FromResult(existingCharts));

        _unitOfWork.Setup(x => x.Chart.Update(It.IsAny<Chart>()))
                   .Returns(Task.FromResult(chartDtoToUpdate));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.UpdateChart(chartDtoToUpdate);

        Assert.NotNull(result);
        Assert.Equal(chartDtoToUpdate, result);
        _unitOfWork.Verify(x => x.Chart.Update(It.IsAny<Chart>()), Times.Once);
    }

    [Fact]
    public void GetColumnsFromTableTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var columnNames = chartService.GetColumnsFromTable();

        Assert.NotNull(columnNames);
        Assert.NotEmpty(columnNames);
    }

    [Fact]
    public async Task ExportCsvAsyncTest()
    {
        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = 1,
            Disability = false,
            DisabilityNotes = "None",
            Level = 3,
            EmployeeType = employeeTypeDto1,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Estiaan",
            Initials = "MT",
            Surname = "Britz",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Male,
            Photo = null,
            Email = "test1@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var dataTypeList = new List<string> { "Gender", "Race", "Age" };
        var propertyNames = new List<string>();

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);
        var result = await chartService.ExportCsvAsync(dataTypeList);
        var expectedResult = new byte[]
        {
            70, 105, 114, 115, 116, 32, 78, 97, 109, 101, 44, 76, 97, 115, 116, 32, 78, 97, 109, 101,
            44, 65, 103, 101, 44, 71, 101, 110, 100, 101, 114, 44, 82, 97, 99, 101, 13, 10, 69, 115, 116, 105, 97, 97, 110,
            44, 66, 114, 105, 116, 122, 44, 65, 103, 101, 32, 48, 44, 77, 97, 108, 101, 44, 66, 108, 97, 99, 107,
            13, 10
        };



        Assert.NotNull(result);
        Assert.IsType<byte[]>(result);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task ExportCsvAsyncTestFail()
    {
        var dataTypeList = new List<string> { "", "" };

        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = 1,
            Disability = false,
            DisabilityNotes = "None",
            Level = 3,
            EmployeeType = employeeTypeDto1,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Estiaan",
            Initials = "MT",
            Surname = "Britz",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Male,
            Photo = null,
            Email = "test1@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);
        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await chartService.ExportCsvAsync(dataTypeList));

        Assert.Equal("Invalid property name: ", exception.Message);
    }
}
