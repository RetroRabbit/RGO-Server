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

public class WorkExperienceUnitTest
{
    private readonly WorkExperienceService _workExperienceService;
    private readonly WorkExperienceDto _workExperienceDto;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IErrorLoggingService> _errorLogggingServiceMock;

    

}
