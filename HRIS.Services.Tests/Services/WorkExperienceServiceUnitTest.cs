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

public class WorkExperienceServiceUnitTest
{
    private readonly WorkExperienceService _workExperienceService;
    private readonly WorkExperienceDto _workExperienceDto;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IErrorLoggingService> _errorLogggingServiceMock;

    public WorkExperienceServiceUnitTest()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _errorLogggingServiceMock = new Mock<IErrorLoggingService>();
        _workExperienceService = new WorkExperienceService(_mockDb.Object, _errorLogggingServiceMock.Object);

        _workExperienceDto = new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        };
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(false);

        var exists = await _workExperienceService.CheckIfExists(new WorkExperienceDto
        {
            Id = 2,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        });

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _workExperienceService.CheckIfExists(new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        });

        Assert.True(exists);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
            .ReturnsAsync(true);

        _errorLogggingServiceMock.Setup(err => err.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _workExperienceService.Save(new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        }));
    }

    [Fact]
    public async Task SavePasstest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
             .ReturnsAsync(false);

        await _workExperienceService.Save(new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        });

        _mockDb.Verify(x => x.WorkExperience.Add(It.IsAny<WorkExperience>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkExperiencePassTest()
    {

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Returns(new List<WorkExperience> { new(_workExperienceDto) }.AsQueryable().BuildMock());

        await _workExperienceService.Delete(1);

        _mockDb.Verify(x => x.WorkExperience.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkExperiencePassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        await _workExperienceService.Update(new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        });

        _mockDb.Verify(x => x.WorkExperience.Update(It.IsAny<WorkExperience>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkExperienceFailtest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(false);

        _errorLogggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _workExperienceService.Update(new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        }));
    }

    [Fact]
    public async Task GetByIdFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Throws(new Exception("Simulated database exception"));

        _errorLogggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()));

        await Assert.ThrowsAsync<Exception>(() => _workExperienceService.GetWorkExperienceByEmployeeId(1));
    }

    [Fact]
    public async Task GetByIdPassTest()
    {
        var workExperience = new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        };

        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Returns(new List<WorkExperience> { new(workExperience) }.AsQueryable().BuildMock());

        await _workExperienceService.GetWorkExperienceByEmployeeId(1);

        _mockDb.Verify(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()), Times.Once);
    }
}
