using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationAudienceControllerUnitTests
{
    private readonly Mock<IEmployeeEvaluationAudienceService> _mockEmployeeEvaluationAudienceService;
    private readonly EmployeeEvaluationAudienceController _controller;
    private readonly List<EmployeeEvaluationAudienceDto> _employeeEvaluationList;
    private readonly EmployeeEvaluationInput _employeeEvaluationInput;
    private readonly EmployeeEvaluationAudienceDto _employeeEvaluationAudienceDto;

    public EmployeeEvaluationAudienceControllerUnitTests() 
    {
        _mockEmployeeEvaluationAudienceService = new Mock<IEmployeeEvaluationAudienceService>();
        _controller = new EmployeeEvaluationAudienceController(_mockEmployeeEvaluationAudienceService.Object);

        _employeeEvaluationList = new List<EmployeeEvaluationAudienceDto>
        {
            new EmployeeEvaluationAudienceDto
            {
                Id = 1,
                Evaluation = new EmployeeEvaluationDto
                {
                    Id = 1,
                    Employee = EmployeeTestData.EmployeeOne.ToDto(),
                    Template = new EmployeeEvaluationTemplateDto{ Id = 1, Description = "Employee Evaluation Template 1" },
                    Owner = EmployeeTestData.EmployeeTwo.ToDto(),
                    Subject = "Employee Evaluation Subject",
                    StartDate = new DateOnly(2022, 1, 1),
                    EndDate = new DateOnly(2022, 2, 1)
                },
                Employee = EmployeeTestData.EmployeeThree.ToDto()
            }
        };

        _employeeEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 1,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Test Template",
            Subject = "Test Subject"
        };

        _employeeEvaluationAudienceDto = new EmployeeEvaluationAudienceDto
        {
            Id = 1,
            Evaluation = new EmployeeEvaluationDto
            {
                Id = 1,
                Employee = EmployeeTestData.EmployeeOne.ToDto(),
                Template = new EmployeeEvaluationTemplateDto { Id = 1, Description = "Employee Evaluation Template 1" },
                Owner = EmployeeTestData.EmployeeTwo.ToDto(),
                Subject = "Employee Evaluation Subject",
                StartDate = new DateOnly(2022, 1, 1),
                EndDate = new DateOnly(2022, 2, 1)
            },

            Employee = EmployeeTestData.EmployeeThree.ToDto()
        };
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationAudiencesValidInputReturnsOkResult()
    {
        _mockEmployeeEvaluationAudienceService.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(_employeeEvaluationList);

        var result = await _controller.GetAll(_employeeEvaluationInput);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAudiences = Assert.IsType<List<EmployeeEvaluationAudienceDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationList, actualAudiences);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationAudiencesExceptionThrownReturnsNotFoundResult()
    {
        _mockEmployeeEvaluationAudienceService.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Error retrieving employee evaluation audiences."));

        var result = await _controller.GetAll(_employeeEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving employee evaluation audiences.", exceptionMessage);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationAudienceValidInputReturnsOkResult()
    {
       _mockEmployeeEvaluationAudienceService.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(_employeeEvaluationAudienceDto);

        var result = await _controller.SaveEmployeeEvaluationAudience( "test@retrorabbit.co.za", _employeeEvaluationInput);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedAudience = Assert.IsType<EmployeeEvaluationAudienceDto>(okResult.Value);
        Assert.Equal(_employeeEvaluationAudienceDto, actualSavedAudience);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
    {
        _mockEmployeeEvaluationAudienceService.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Exception occurred while saving employee evaluation audience."));

        var result = await _controller.SaveEmployeeEvaluationAudience("test@retrorabbit.co.za", _employeeEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Exception occurred while saving employee evaluation audience.", exceptionMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationAudienceValidInputReturnsOkResult()
    {
        _mockEmployeeEvaluationAudienceService.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()));

        var result = await _controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za", _employeeEvaluationInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
    {
        _mockEmployeeEvaluationAudienceService.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Exception occurred while deleting employee evaluation audience."));

        var result = await _controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za", _employeeEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Exception occurred while deleting employee evaluation audience.", exceptionMessage);
    }
}