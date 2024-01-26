﻿using Xunit;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork;
using RGO.Services.Services;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using RGO.Services.Interfaces;
using MockQueryable.Moq;

namespace RGO.Services.Tests.Services;

public class EmployeeDocumentServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IEmployeeDocumentService> _employeeDocumentServiceMock;

    private readonly EmployeeDocumentService _employeeDocumentService;

    public EmployeeDocumentServiceUnitTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeDocumentServiceMock = new Mock<IEmployeeDocumentService>();
        _employeeDocumentService = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeDocument_ShouldSaveDocument_WhenEmployeeExists()
    {
        var employeeId = 1;
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto employeeMock = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDocDto = new SimpleEmployeeDocumentDto(
            Id: 1,
            EmployeeId: employeeId,
            FileName: "TestFile.pdf",
            FileCategory: FileCategory.FixedTerm,
            File: "TestFileContent",
            UploadDate: DateTime.Now
        );

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
            .ReturnsAsync(employeeMock);

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
            .ReturnsAsync(new EmployeeDocumentDto(
                Id: 1,
                Employee: employeeMock,
                Reference: null,
                FileName: "TestFile.pdf",
                FileCategory: FileCategory.FixedTerm,
                Blob: "TestFileContent",
                Status: DocumentStatus.PendingApproval,
                UploadDate: DateTime.Now,
                Reason: null,
                CounterSign: false
            ));

        var result = await _employeeDocumentService.SaveEmployeeDocument(employeeDocDto);

        Assert.NotNull(result);
        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeDocument_ShouldThrowException_WhenEmployeeDoesNotExist()
    {
        var employeeId = 1;
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        EmployeeDto employeeMock = new(1, "001", "34434434", new DateTime(), new DateTime(),
           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
           "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
           new DateTime(), null, Race.Black, Gender.Male, null,
           "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
            .ReturnsAsync((EmployeeDto)null);

        var employeeDocDto = new SimpleEmployeeDocumentDto(
            Id: 1,
            EmployeeId: employeeId,
            FileName: "TestFile.pdf",
            FileCategory: FileCategory.FixedTerm,
            File: "TestFileContent",
            UploadDate: DateTime.Now
        );

        var exception = await Assert.ThrowsAsync<Exception>(() => _employeeDocumentService.SaveEmployeeDocument(employeeDocDto));
        Assert.Equal("employee not found", exception.Message);

        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDocument_ReturnsCorrectDocument_WhenEmployeeExists()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        int employeeId = 1;
        string fileName = "TestFile.pdf";

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);

        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument { EmployeeId = employeeId, FileName = fileName };
        var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(mockUnitOfWork.Object, mockEmployeeService.Object);

        var result = await service.GetEmployeeDocument(employeeId, fileName);

        Assert.NotNull(result);
        Assert.Equal(fileName, result.FileName);
    }

    [Fact]
    public async Task GetAllEmployeeDocuments_ReturnsDocuments_WhenEmployeeExists()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        int employeeId = 1;

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument { EmployeeId = employeeId, FileName = "TestFile1.pdf" },
            new EmployeeDocument { EmployeeId = employeeId, FileName = "TestFile2.pdf" }
        };
        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(mockUnitOfWork.Object, mockEmployeeService.Object);

        var result = await service.GetAllEmployeeDocuments(employeeId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, doc => doc.FileName == "TestFile1.pdf");
        Assert.Contains(result, doc => doc.FileName == "TestFile2.pdf");
    }

    [Fact]
    public async Task UpdateEmployeeDocument_UpdatesDocumentSuccessfully_WhenEmployeeExists()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        int employeeId = 1;
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
              null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
              "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
              new DateTime(), null, Race.Black, Gender.Male, null,
              "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocumentDto = new EmployeeDocumentDto(
            Id: 1,
            Employee: testEmployee1,
            Reference: null,
            FileName: "e2.pdf",
            FileCategory: FileCategory.Medical,
            Blob: "sadfasdf",
            Status: null,
            UploadDate: DateTime.Now,
            Reason: null,
            CounterSign: false
        );

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);

        mockUnitOfWork.Setup(m => m.EmployeeDocument.Update(It.IsAny<EmployeeDocument>()))
                      .ReturnsAsync(employeeDocumentDto);
        var service = new EmployeeDocumentService(mockUnitOfWork.Object, mockEmployeeService.Object);

        var result = await service.UpdateEmployeeDocument(employeeDocumentDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeDocument_DeletesDocumentSuccessfully_WhenEmployeeExists()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        int employeeId = 1;
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
              null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
              "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
              new DateTime(), null, Race.Black, Gender.Male, null,
              "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMockDbSet();
        mockUnitOfWork.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocumentDto = new EmployeeDocumentDto(
            Id: 1,
            Employee: testEmployee1,
            Reference: null,
            FileName: "e2.pdf",
            FileCategory: FileCategory.Medical,
            Blob: "sadfasdf",
            Status: null,
            UploadDate: DateTime.Now,
            Reason: null,
            CounterSign: false
        );

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);

        mockUnitOfWork.Setup(m => m.EmployeeDocument.Delete(It.IsAny<int>()))
                      .Returns(Task.FromResult(employeeDocumentDto));

        var service = new EmployeeDocumentService(mockUnitOfWork.Object, mockEmployeeService.Object);

        var result = await service.DeleteEmployeeDocument(employeeDocumentDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatus_PendingSuccess()
    {
        int employeeId = 1;

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        EmployeeDocumentDto testDocument = new EmployeeDocumentDto(1, testEmployee1, null, "TestFile.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.PendingApproval, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        EmployeeDocumentDto testDocumentTwo = new EmployeeDocumentDto(2, testEmployee1, null, "TestFile2.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.PendingApproval, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(testDocument),
            new EmployeeDocument(testDocumentTwo)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.PendingApproval);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatus_ApprovedSuccess()
    {
        int employeeId = 1;

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        EmployeeDocumentDto testDocument = new EmployeeDocumentDto(1, testEmployee1, null, "TestFile.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.Approved, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        EmployeeDocumentDto testDocumentTwo = new EmployeeDocumentDto(2, testEmployee1, null, "TestFile2.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.Approved, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(testDocument),
            new EmployeeDocument(testDocumentTwo)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.Approved);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatus_RejectedSuccess()
    {
        int employeeId = 1;

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var mockEmployee = new Employee(testEmployee1, employeeTypeDto);
        var mockEmployeeDbSet = new List<Employee> { mockEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        EmployeeDocumentDto testDocument = new EmployeeDocumentDto(1, testEmployee1, null, "TestFile.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.Rejected, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        EmployeeDocumentDto testDocumentTwo = new EmployeeDocumentDto(2, testEmployee1, null, "TestFile2.pdf", FileCategory.FixedTerm,
        "TestFileContent", Status: DocumentStatus.Rejected, UploadDate: DateTime.Now, Reason: null, CounterSign: false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(testDocument),
            new EmployeeDocument(testDocumentTwo)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.Rejected);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

}

