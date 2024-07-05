using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class WorkExperienceServiceUnitTest
{
    private readonly WorkExperienceService _workExperienceService;
    private readonly WorkExperience _workExperience;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IErrorLoggingService> _errorLogggingServiceMock;

    public WorkExperienceServiceUnitTest()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _errorLogggingServiceMock = new Mock<IErrorLoggingService>();
        _workExperienceService = new WorkExperienceService(_mockDb.Object, _errorLogggingServiceMock.Object);

        _workExperience = new WorkExperience
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            ProjectDescription = "This project was a payment distribution and invoicing system, which was designed to run debit orders"
        };
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(false);

        var exists = await _workExperienceService.CheckIfExists(_workExperience.ToDto());

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _workExperienceService.CheckIfExists(_workExperience.ToDto());

        Assert.True(exists);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
            .ReturnsAsync(true);

        _errorLogggingServiceMock.Setup(err => err.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _workExperienceService.Save(_workExperience.ToDto()));
    }

    [Fact]
    public async Task SavePassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
             .ReturnsAsync(false);
        
        _mockDb.Setup(x => x.WorkExperience.Add(It.IsAny<WorkExperience>()))
             .ReturnsAsync(_workExperience);

        await _workExperienceService.Save(_workExperience.ToDto());

        _mockDb.Verify(x => x.WorkExperience.Add(It.IsAny<WorkExperience>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkExperiencePassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Delete(It.IsAny<int>()))
               .ReturnsAsync(_workExperience);

        await _workExperienceService.Delete(1);

        _mockDb.Verify(x => x.WorkExperience.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkExperiencePassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.WorkExperience.Update(It.IsAny<WorkExperience>()))
            .ReturnsAsync(_workExperience);

        await _workExperienceService.Update(_workExperience.ToDto());

        _mockDb.Verify(x => x.WorkExperience.Update(It.IsAny<WorkExperience>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkExperienceFailtest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(false);

        _errorLogggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _workExperienceService.Update(_workExperience.ToDto()));
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
        var workExperience = new WorkExperience
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            ProjectDescription = "This project was a payment distribution and invoicing system, which was designed to run debit orders"
        };

        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Returns(workExperience.ToMockIQueryable());

        await _workExperienceService.GetWorkExperienceByEmployeeId(1);

        _mockDb.Verify(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()), Times.Once);
    }
}
