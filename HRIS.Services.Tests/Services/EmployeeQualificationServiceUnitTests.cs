using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums.QualificationEnums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;
using MockQueryable.Moq;
using RR.Tests.Data;

namespace HRIS.Services.Tests.Services
{
    public class EmployeeQualificationServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
        private readonly IEmployeeQualificationService _employeeQualificationService;

        private readonly EmployeeDto _employeeDto;
        private readonly EmployeeQualificationDto _employeeQualificationDto;
        private readonly EmployeeQualification _employeeQualification;

        private const int EmployeeId = 1;
        private const int QualificationId = 1;
        private const int NonExistingQualificationId = 9999;

        public EmployeeQualificationServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
            _employeeQualificationService = new EmployeeQualificationService(
                _unitOfWorkMock.Object,
                _employeeServiceMock.Object,
                _errorLoggingServiceMock.Object);

            _employeeDto = new EmployeeDto { Id = 1, EmployeeNumber = "EMP001" };

            _employeeQualification = new EmployeeQualification
            {
                Id = 1,
                EmployeeId = 1,
                HighestQualification = HighestQualification.Bachelor,
                School = "Example School",
                FieldOfStudy = "Example Field",
                NQFLevel = NQFLevel.Level7,
                Year = new DateOnly(2018, 4, 6),
                ProofOfQualification = "qualification",
                DocumentName = "DocumentName"
            };

            _employeeQualificationDto = _employeeQualification.ToDto();
        }

        private void SetupEmployeeExists()
        {
            _employeeServiceMock.Setup(x => x.GetEmployeeById(EmployeeId)).ReturnsAsync(_employeeDto);
        }

        private void SetupEmployeeDoesNotExist()
        {
            _employeeServiceMock.Setup(x => x.GetEmployeeById(EmployeeId)).ReturnsAsync((EmployeeDto)null!);
        }

        private void SetupLogException()
        {
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Returns<Exception>((ex) => ex);
        }

        private void VerifyLogExceptionCalled(Func<Times> times)
        {
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), times);
        }

        private void VerifyAddQualificationCalled(Func<Times> times)
        {
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), times);
        }

        private void VerifyGetEmployeeByIdCalled(Func<Times> times)
        {
            _employeeServiceMock.Verify(x => x.GetEmployeeById(EmployeeId), times);
        }

        private void VerifyDeleteQualificationCalled(Func<Times> times)
        {
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Delete(QualificationId), times);
        }

        [Fact]
        public async Task SaveEmployeeQualification_Success_WithValidData_ReturnsQualificationsList()
        {
            SetupEmployeeExists();
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
                .ReturnsAsync(_employeeQualification);

            var result = await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, EmployeeId);

            Assert.NotNull(result);
            Assert.Equivalent(_employeeQualificationDto, result);

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Never);
            VerifyAddQualificationCalled(Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeQualification_Failure_WithNonExistingEmployee_ThrowsException()
        {
            SetupLogException();
            SetupEmployeeDoesNotExist();

            async Task Act() => await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, EmployeeId);
            var exception = await Assert.ThrowsAsync<Exception>(Act);

            Assert.NotNull(exception);
            VerifyGetEmployeeByIdCalled(Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Success_WithValidData_ReturnsQualificationsList()
        {
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.GetAll(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync(new List<EmployeeQualification> { _employeeQualification });

            var result = await _employeeQualificationService.GetAllEmployeeQualifications();

            Assert.NotNull(result);
            Assert.Single(result);
            var expectedQualification = result.First();
            Assert.Equivalent(_employeeQualificationDto, expectedQualification);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Failure_WhenRetrievalFails_ThrowsException()
        {
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.GetAll(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ThrowsAsync(new Exception("Failed to retrieve qualifications."));

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.GetAllEmployeeQualifications();
            });
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Success_WithValidEmployeeId_ReturnsQualifications()
        {
            SetupEmployeeExists();

            var mockQualifications = new List<EmployeeQualification>
            {
                new() { EmployeeId = EmployeeId, HighestQualification = HighestQualification.Bachelor },
                new() { EmployeeId = EmployeeId, HighestQualification = HighestQualification.Master }
            }.ToMockIQueryable();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                           .Returns(mockQualifications);

            var result = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(EmployeeId);

            Assert.NotNull(result);
            Assert.Equal(HighestQualification.Bachelor, result.HighestQualification);
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_WithInvalidEmployeeId_ThrowsInvalidOperationException()
        {
            SetupLogException();

            _employeeServiceMock.Setup(x => x.GetEmployeeById(EmployeeId)).ThrowsAsync(new InvalidOperationException("Test exception"));

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(EmployeeId);
            });
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_WithNonExistingEmployee_ThrowsKeyNotFoundException()
        {
            SetupLogException();
            SetupEmployeeDoesNotExist();

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(EmployeeId);
            });

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_ExceptionThrown_LogsAndRethrows()
        {
            SetupLogException();
            SetupEmployeeExists();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .Throws(new Exception("Test exception"));

            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(EmployeeId);
            });

            Assert.Equal("Test exception", ex.Message);

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task GetEmployeeQualificationById_Success_WithExistingId_ReturnsQualification()
        {
            SetupLogException();

            var mockQualificationQuery = new List<EmployeeQualification> { _employeeQualification }
                .AsQueryable()
                .BuildMock();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .Returns(mockQualificationQuery);

            var result = await _employeeQualificationService.GetEmployeeQualificationById(QualificationId);

            Assert.NotNull(result);
            Assert.Equal(_employeeQualificationDto.Id, result.Id);
            Assert.Equal(_employeeQualificationDto.EmployeeId, result.EmployeeId);
            Assert.Equal(_employeeQualificationDto.HighestQualification, result.HighestQualification);
            Assert.Equal(_employeeQualificationDto.School, result.School);
            Assert.Equal(_employeeQualificationDto.FieldOfStudy, result.FieldOfStudy);
            Assert.Equal(_employeeQualificationDto.NQFLevel, result.NQFLevel);
            Assert.Equal(_employeeQualificationDto.Year, result.Year);
        }

        [Fact]
        public async Task GetEmployeeQualificationById_Failure_WithNonExistingId_ThrowsException()
        {
            SetupLogException();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
               .Returns(new List<EmployeeQualification>().ToMockIQueryable());

            await Assert.ThrowsAsync<Exception>(async () => await _employeeQualificationService.GetEmployeeQualificationById(NonExistingQualificationId));
        }

        [Fact]
        public async Task GetEmployeeQualificationById_FailureWithNonExistingId_ThrowsNullException()
        {
            SetupLogException();
            SetupEmployeeDoesNotExist();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _employeeQualificationService.GetEmployeeQualificationById(QualificationId);
            });
        }

        [Fact]
        public async Task UpdateEmployeeQualification_Success()
        {
            _unitOfWorkMock.Setup(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
               .ReturnsAsync(_employeeQualification);

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()))
                .ReturnsAsync(_employeeQualification);

            var result = await _employeeQualificationService.UpdateEmployeeQualification(_employeeQualificationDto);

            Assert.NotNull(result);
            Assert.Equivalent(_employeeQualificationDto, result);
        }

        [Fact]
        public async Task UpdateEmployeeQualification_Failure()
        {
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync((EmployeeQualification)null!);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _employeeQualificationService.UpdateEmployeeQualification(_employeeQualificationDto);
            });

            Assert.NotNull(exception);
            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeQualification_Success()
        {
            var deletedQualification = new EmployeeQualification { Id = QualificationId };

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Delete(QualificationId))
                .ReturnsAsync(deletedQualification);

            var result = await _employeeQualificationService.DeleteEmployeeQualification(QualificationId);

            Assert.NotNull(result);
            Assert.Equal(QualificationId, result.Id);

            VerifyDeleteQualificationCalled(Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeQualification_Failure_WhenDeletionFails_ThrowsException()
        {
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Delete(QualificationId))
                .ThrowsAsync(new Exception("Delete operation failed"));

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.DeleteEmployeeQualification(QualificationId);
            });

            Assert.Equal("Delete operation failed", exception.Message);

            VerifyDeleteQualificationCalled(Times.Once);
        }
    }
}