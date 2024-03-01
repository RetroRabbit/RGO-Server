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
    [Fact]
    public async Task GetAllEmployeeEvaluationAudiencesValidInputReturnsOkResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        var expectedAudiences = new List<EmployeeEvaluationAudienceDto>
{
            new EmployeeEvaluationAudienceDto
            {
                Id = 1,
                Evaluation = new EmployeeEvaluationDto
                {
                    Id = 1,
                    Employee = EmployeeTestData.EmployeeDto,
                    Template = new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),
                    Owner = EmployeeTestData.EmployeeDto2,
                    Subject = "Employee Evaluation Subject",
                    StartDate = new DateOnly(2022, 1, 1),
                    EndDate = new DateOnly(2022, 2, 1)
                },
                Employee = EmployeeTestData.EmployeeDto3
            }
        };

        serviceMock.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(expectedAudiences);

        var result = await controller.GetAll(new EmployeeEvaluationInput
        {
            Id = 1,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Test Template",
            Subject = "Test Subject"
        });

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAudiences = Assert.IsType<List<EmployeeEvaluationAudienceDto>>(okResult.Value);
        Assert.Equal(expectedAudiences, actualAudiences);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationAudiencesExceptionThrownReturnsNotFoundResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        serviceMock.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Error retrieving employee evaluation audiences."));

        var result = await controller.GetAll(new EmployeeEvaluationInput
        {
            Id = 1,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Test Template",
            Subject = "Test Subject"
        });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal("Error retrieving employee evaluation audiences.", exceptionMessage);
    }


    [Fact]
    public async Task SaveEmployeeEvaluationAudienceValidInputReturnsOkResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        var savedAudience = new EmployeeEvaluationAudienceDto
        { Id = 1, Evaluation = new EmployeeEvaluationDto
        {
            Id = 1,
            Employee = EmployeeTestData.EmployeeDto,
            Template = new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),
            Owner = EmployeeTestData.EmployeeDto2,
            Subject = "Employee Evaluation Subject",
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2022, 2, 1)
        },

        Employee = EmployeeTestData.EmployeeDto3
        };

        serviceMock.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(savedAudience);

        var result = await controller.SaveEmployeeEvaluationAudience( "test@retrorabbit.co.za",
                                                                     new EmployeeEvaluationInput
                                                                     {
                                                                         Id = 1,
                                                                         OwnerEmail = "owner@retrorabbit.co.za",
                                                                         EmployeeEmail = "employee@retrorabbit.co.za",
                                                                         Template = "Test Template",
                                                                         Subject = "Test Subject"
                                                                     });
                                                                      

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedAudience = Assert.IsType<EmployeeEvaluationAudienceDto>(okResult.Value);

        Assert.Equal(savedAudience, actualSavedAudience);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        serviceMock.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Exception occurred while saving employee evaluation audience."));
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        var result = await controller.SaveEmployeeEvaluationAudience("test@retrorabbit.co.za",
                                                                     new EmployeeEvaluationInput
                                                                     {
                                                                         Id = 1,
                                                                         OwnerEmail = "owner@retrorabbit.co.za",
                                                                         EmployeeEmail = "employee@retrorabbit.co.za",
                                                                         Template = "Test Template",
                                                                         Subject = "Test Subject"
                                                                     });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Exception occurred while saving employee evaluation audience.", exceptionMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationAudienceValidInputReturnsOkResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        serviceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()));
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za",
                                                                       new EmployeeEvaluationInput
                                                                       {
                                                                           Id = 1,
                                                                           OwnerEmail = "owner@retrorabbit.co.za",
                                                                           EmployeeEmail = "employee@retrorabbit.co.za",
                                                                           Template = "Test Template",
                                                                           Subject = "Test Subject"
                                                                       });

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
        serviceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Exception occurred while deleting employee evaluation audience."));
        var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za",
                                                                       new EmployeeEvaluationInput
                                                                       {
                                                                           Id = 1,
                                                                           OwnerEmail = "owner@retrorabbit.co.za",
                                                                           EmployeeEmail = "employee@retrorabbit.co.za",
                                                                           Template = "Test Template",
                                                                           Subject = "Test Subject"
                                                                       });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Exception occurred while deleting employee evaluation audience.", exceptionMessage);
    }
}
