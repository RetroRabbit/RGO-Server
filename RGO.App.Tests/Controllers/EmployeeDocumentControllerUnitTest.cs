using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeDocumentControllerUnitTest
    {
        private readonly Mock<IEmployeeDocumentService> _employeeMockDocumentService;
        private readonly EmployeeDocumentDto _employeeDocument;
        private readonly SimpleEmployeeDocumentDto _simpleEmployeeDocument;
        private readonly EmployeeDocumentController _controller;

        EmployeeDocumentDto employeeDocumentDto;
        EmployeeDocumentDto updateEmployeeDocumentDto;
        static EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        static EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        static EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        static EmployeeDto employeeMock = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        static EmployeeDto UpdateemployeeMock = new EmployeeDto(1, "008", "123456789", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South America", "South African", "0000055000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        public EmployeeDocumentControllerUnitTest()
        {
            _employeeMockDocumentService = new Mock<IEmployeeDocumentService>();
            _controller = new EmployeeDocumentController(_employeeMockDocumentService.Object);

            employeeDocumentDto = new EmployeeDocumentDto(
                Id: 1,
                Employee: employeeMock,
                Reference: null,
                FileName: "e2.pdf",
                FileCategory: FileCategory.Medical,
                Blob: "sadfasdf",
                Status: null,
                UploadDate: DateTime.Now,
                Reason: null,
                CounterSign: false
            );

            updateEmployeeDocumentDto = new EmployeeDocumentDto(
                Id: 1,
                Employee: UpdateemployeeMock,
                Reference: null,
                FileName: "new.pdf",
                FileCategory: FileCategory.Medical,
                Blob: "newBlob",
                Status: null,
                UploadDate: DateTime.Now,
                Reason: null,
                CounterSign: false
            );
        }

        [Fact]
        public async Task GetEmployeeDocumentReturnsOkfoundResult()
        {
            var id = employeeDocumentDto.Id;
            var fileName = employeeDocumentDto.FileName;

            _employeeMockDocumentService.Setup(x => x.GetEmployeeDocument(id, fileName)).ReturnsAsync(employeeDocumentDto);

            var result = await _controller.GetEmployeeDocument(employeeDocumentDto.Id, employeeDocumentDto.FileName);

            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDetails = Assert.IsType<EmployeeDocumentDto>(okResult.Value);

            Assert.Equal(employeeDocumentDto, actualDetails);
        }

        [Fact]
        public async Task GetEmployeeDocumentReturnsExceptionNotfoundResult()
        {
            var id = 15;
            var filename = "";
 
            var err = "An error occurred while fetching the employee document.";

            _employeeMockDocumentService.Setup(x => x.GetEmployeeDocument(id, filename)).ThrowsAsync(new Exception(err));

            var result = await _controller.GetEmployeeDocument(id, filename);
            var notfoundResult = Assert.IsType<ObjectResult>(result);
            var actualExceptionMessage = Assert.IsType<string>(notfoundResult.Value);

            Assert.Equal("An error occurred while fetching the employee document.", actualExceptionMessage);

        }

        [Fact]
        public async Task GetAllEmployeeDocumentReturnsOkResult()
        {
            var id = employeeDocumentDto.Id;

            var listOfEmployeeDocumentsDto = new List<EmployeeDocumentDto>()
            {
                employeeDocumentDto
            };

            var listOfEmployeeDocuments = new List<EmployeeDocument>()
            {
                new EmployeeDocument(employeeDocumentDto)
            };

            _employeeMockDocumentService.Setup(x => x.GetAllEmployeeDocuments(id)).ReturnsAsync(listOfEmployeeDocumentsDto);

            var result = await _controller.GetAllEmployeeDocuments(employeeDocumentDto.Id);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDetails = Assert.IsAssignableFrom<List<EmployeeDocumentDto>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(listOfEmployeeDocumentsDto, actualDetails);

        }

        [Fact]
        public async Task GetAllReturnsNotFoundResultWhenExceptionThrown()
        {
            var id = 15;
            var exceptionMessage = "An error occurred while fetching employee documents.";

            _employeeMockDocumentService.Setup(x => x.GetAllEmployeeDocuments(id)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.GetAllEmployeeDocuments(id);

            var noFoundResult = Assert.IsType<ObjectResult>(result);
            var actualExceptionMessage = Assert.IsType<string>(noFoundResult.Value);

            Assert.Equal(exceptionMessage, actualExceptionMessage);

        }

        [Fact]
        public async Task SaveEmployeeDocumentReturnsOkResult()
        {
            _employeeMockDocumentService.Setup(c => c.SaveEmployeeDocument(_simpleEmployeeDocument)).ReturnsAsync(employeeDocumentDto);
           
            var result = await _controller.Save(_simpleEmployeeDocument);
            var okresult = Assert.IsType<OkObjectResult>(result);
            var actualSavedEmployeeDocument = Assert.IsType<EmployeeDocumentDto>(okresult.Value);

            Assert.Equal(employeeDocumentDto, actualSavedEmployeeDocument);

        }

        [Fact]
        public async Task SaveEmployeeDocumentThrowsExceptionReturnsNotFoundResult()
        {
            _employeeMockDocumentService.Setup(x => x.SaveEmployeeDocument(It.IsAny<SimpleEmployeeDocumentDto>())).Throws(new Exception("An error occurred while saving the employee document."));

            var result = await _controller.Save(_simpleEmployeeDocument);
            var notfoundResult = Assert.IsType<ObjectResult>(result);
            var exceptionMessage = Assert.IsType<string>(notfoundResult.Value);

            Assert.Equal("An error occurred while saving the employee document.", exceptionMessage);

        }

        [Fact]
        public async Task UpdateEmployeeDocumentReturnsOkResult()
        {
            var mockService = new Mock<IEmployeeDocumentService>();
            
            var updateEntry = employeeDocumentDto;

            mockService.Setup(x => x.UpdateEmployeeDocument(It.IsAny<EmployeeDocumentDto>()))
                .ReturnsAsync(updateEmployeeDocumentDto);

            var result = await _controller.Update(updateEntry);
            var okresult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okresult.StatusCode);

        }

        [Fact]
        public async Task UpdateEmployeeDocumentReturnsNotFoundResultWhenExceptionThrown()
        {
            var updateEntry = employeeDocumentDto;
            var errorMessage = "An error occurred while updating the employee document.";

            _employeeMockDocumentService.Setup(x => x.UpdateEmployeeDocument(It.IsAny<EmployeeDocumentDto>())).ThrowsAsync(new Exception(errorMessage));

            var result = await _controller.Update(updateEntry);
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal(errorMessage, actualErrorMessage);
            Assert.Equal(500, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task DeleteEmployeeDocumentsReturnsOkResult()
        {
            var employeeDocomentDelete = employeeDocumentDto;
            var employeeDocomentDeleted = updateEmployeeDocumentDto;

            _employeeMockDocumentService.Setup(e => e.DeleteEmployeeDocument(employeeDocomentDelete)).ReturnsAsync(employeeDocomentDeleted);
         
            var result = await _controller.Delete(employeeDocomentDelete);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualemployeeDocument = Assert.IsAssignableFrom<EmployeeDocumentDto>(okResult.Value);

            Assert.Equal(employeeDocomentDeleted, actualemployeeDocument);

        }

        [Fact]
        public async Task DeleteEmployeeDocumentReturnsNotFoundResultWhenExceptionThrown()
        {
            var employeeDocumentToDelete = employeeDocumentDto;
            var exceptionMessage = "An error occurred while deleting the employee document.";


            _employeeMockDocumentService.Setup(e => e.DeleteEmployeeDocument(employeeDocumentToDelete)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.Delete(employeeDocumentToDelete);
            var notFoundResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(exceptionMessage, notFoundResult.Value);

        }

        [Fact]
        public async Task GetEmployeeDocumentByStatusReturnsOkResult()
        {  
            var id = employeeDocumentDto.Id;
            var status = DocumentStatus.Rejected;

            var listOfEmployeeDocumentsDto = new List<EmployeeDocumentDto>()
            {
                employeeDocumentDto
            };

            _employeeMockDocumentService.Setup(x => x.GetEmployeeDocumentsByStatus(id, status)).ReturnsAsync(listOfEmployeeDocumentsDto);

            var result = await _controller.GetEmployeeDocumentsByStatus(id, status);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDetails = Assert.IsAssignableFrom<List<EmployeeDocumentDto>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(listOfEmployeeDocumentsDto, actualDetails);

        }

        [Fact]
        public async Task GetEmployeeDocumentByStatusReturnsNotFoundResultWhenExceptionThrown()
        {
            var id = 15;
            var documentStatus = (DocumentStatus)(-1);
            var exceptionMessage = "An error occurred while fetching the employee documents.";


            _employeeMockDocumentService.Setup(x => x.GetEmployeeDocumentsByStatus(id, documentStatus))
                .ThrowsAsync(new Exception(exceptionMessage));


            var result = await _controller.GetEmployeeDocumentsByStatus(id, documentStatus);
            var notfoundResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal("An error occurred while fetching the employee documents.", notfoundResult.Value);

        }
    }
}
