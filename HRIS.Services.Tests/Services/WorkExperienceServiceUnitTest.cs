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


        _workExperienceDto = new WorkExperienceDto
        {
            Id = 1,
            Title = "Senior Developer",
            EmploymentType = "Permanent",
            CompanyName = "Retro Rabbit",
            Location = "Pretoria",
            EmployeeId = 1,
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2024, 1, 1),
        };
    }

    //[Fact]
    //public async Task CheckIfExistsFailTest()
    //{
    //    _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
    //           .ReturnsAsync(false);

    //    var exists = await _workExperienceService.CheckIfExists(new WorkExperienceDto
    //    {
    //        Id = 1,
    //        Title = "Senior Developer",
    //        EmploymentType = "Permanent",
    //        CompanyName = "Retro Rabbit",
    //        Location = "Pretoria",
    //        EmployeeId = 1,
    //        StartDate = new DateOnly(2022, 1, 1),
    //        EndDate = new DateOnly(2024, 1, 1),
    //    });

    //    Assert.False(exists);
    //}

    //[Fact]
    //public async Task CheckIfExistsPassTest()
    //{
    //    _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
    //           .ReturnsAsync(false);

    //    var exists = await _workExperienceService.CheckIfExists(new WorkExperienceDto
    //    {
    //        Id = 1,
    //        Title = "Senior Developer",
    //        EmploymentType = "Permanent",
    //        CompanyName = "Retro Rabbit",
    //        Location = "Pretoria",
    //        EmployeeId = 1,
    //        StartDate = new DateOnly(2022, 1, 1),
    //        EndDate = new DateOnly(2024, 1, 1),
    //    });

    //    Assert.True(exists);
    //}

    //[Fact]
    //public async Task DeletePassTest()
    //{
    //    var workExperience = new WorkExperienceDto
    //    {
    //        Id = 1,
    //        Title = "Senior Developer",
    //        EmploymentType = "Permanent",
    //        CompanyName = "Retro Rabbit",
    //        Location = "Pretoria",
    //        EmployeeId = 1,
    //        StartDate = new DateOnly(2022, 1, 1),
    //        EndDate = new DateOnly(2024, 1, 1),
    //    };

    //    _mockDb.Setup(x => x.WorkExperience.Any(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
    //           .ReturnsAsync(true);

    //    _mockDb.Setup(x => x.WorkExperience.Get(It.IsAny<Expression<Func<WorkExperience, bool>>>()))
    //           .Returns(new List<WorkExperience> { new(workExperience) }.AsQueryable().BuildMock());

    //    await _workExperienceService.Delete(1);

    //    _mockDb.Verify(x => x.EmployeeDate.Delete(It.IsAny<int>()), Times.Once);
    //}
}
