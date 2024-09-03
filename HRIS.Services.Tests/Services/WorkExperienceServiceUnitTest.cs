using System.Linq.Expressions;
using AutoMapper;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class WorkExperienceServiceUnitTest
{
    private readonly WorkExperienceService _workExperienceService;
    private readonly WorkExperience _workExperience;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<AuthorizeIdentityMock> _identity;


    public WorkExperienceServiceUnitTest()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _identity = new Mock<AuthorizeIdentityMock>();
        _workExperienceService = new WorkExperienceService(_mockDb.Object, _mapperMock.Object, _identity.Object);

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

        var exists = await _workExperienceService.CheckIfExists(_workExperience.ToDto().Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _workExperienceService.CheckIfExists(_workExperience.ToDto().Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task SaveUnauthorizedTest()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.Save(_workExperience.ToDto()));
    }

    [Fact]
    public async Task SaveDoesNotExistTest()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.Save(_workExperience.ToDto()));
    }

    [Fact]
    public async Task SaveAuthorizedAndExistsPassTest()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

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
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
        .ReturnsAsync(true);

        _mockDb.Setup(x => x.WorkExperience.Delete(It.IsAny<int>()))
               .ReturnsAsync(_workExperience);

        await _workExperienceService.Delete(1);

        _mockDb.Verify(x => x.WorkExperience.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUnauthorizedTest()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.Delete(_workExperience.ToDto().Id));
    }

    [Fact]
    public async Task UpdateWorkExperiencePassTest()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.WorkExperience.Update(It.IsAny<WorkExperience>()))
            .ReturnsAsync(_workExperience);

        await _workExperienceService.Update(_workExperience.ToDto());

        _mockDb.Verify(x => x.WorkExperience.Update(It.IsAny<WorkExperience>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkExperienceFailTest()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.Update(_workExperience.ToDto()));
    }

    [Fact]
    public async Task UpdateWorkExperienceDoesNotExistTest()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.Update(_workExperience.ToDto()));
    }

    [Fact]
    public async Task GetByIdFailTest()
    {
        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Throws(new CustomException("Simulated database exception"));

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.GetWorkExperienceByEmployeeId(1));
    }

    [Fact]
    public async Task GetByIdUnauthorizedTest()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);

        _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
               .Returns(_workExperience.ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() => _workExperienceService.GetWorkExperienceByEmployeeId(1));
    }

    [Fact]
    public async Task GetByIdPassTest()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

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
