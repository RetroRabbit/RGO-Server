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

namespace HRIS.Services.Tests.Services
{
    public class EmployeeQualificationServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
        private readonly IEmployeeQualificationService _employeeQualificationService;

        // Shared test data
        private readonly EmployeeDto _employeeDto;
        private readonly EmployeeQualificationDto _employeeQualificationDto;
        private readonly EmployeeQualification _employeeQualification;
        private readonly List<EmployeeQualificationDto> _employeeQualificationDtoList;

        private const int _employeeId = 1;
        private const int _qualificationId = 1;
        private const int _nonExistingQualificationId = 9999;

        public EmployeeQualificationServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
            _employeeQualificationService = new EmployeeQualificationService(
                _unitOfWorkMock.Object,
                _employeeServiceMock.Object,
                _errorLoggingServiceMock.Object);

            // Initialize shared test mock data
            _employeeDto = new EmployeeDto { Id = 1, EmployeeNumber = "EMP001" };

            _employeeQualificationDto = new EmployeeQualificationDto
            {
                Id = 1,
                EmployeeId = 1,
                HighestQualification = HighestQualification.Bachelor,
                School = "Example School",
                Degree = "Example Degree",
                FieldOfStudy = "Example Field",
                NQFLevel = NQFLevel.Level7,
                Year = new DateOnly(2018, 4, 6)
            };

            _employeeQualification = new EmployeeQualification
            {
                Id = _employeeQualificationDto.Id,
                EmployeeId = _employeeQualificationDto.EmployeeId,
                HighestQualification = _employeeQualificationDto.HighestQualification,
                School = _employeeQualificationDto.School,
                Degree = _employeeQualificationDto.Degree,
                FieldOfStudy = _employeeQualificationDto.FieldOfStudy,
                NQFLevel = _employeeQualificationDto.NQFLevel,
                Year = _employeeQualificationDto.Year
            };

            _employeeQualificationDtoList = new List<EmployeeQualificationDto> { _employeeQualificationDto };
        }

        private void SetupEmployeeExists()
        {
            // Setup mock for getting employee by ID
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync(_employeeDto);
        }

        private void SetupEmployeeDoesNotExist()
        {
            // Setup mock to return null, simulating the scenario where employee does not exist
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync((EmployeeDto)null);
        }

        private void SetupLogException()
        {
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Returns<Exception>((ex) => ex);
        }

        private void VerifyLogExceptionCalled(Func<Times> times)
        {
            // Verify that the error logging service was not called during a successful operation
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), times);
        }

        private void VerifyAddQualificationCalled(Func<Times> times)
        {
            // Verify that the unitOfWork's EmployeeQualification.Add method was called with the correct parameter
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), times);
        }

        private void VerifyGetEmployeeByIdCalled(Func<Times> times)
        {
            // Verify that the employee service's GetEmployeeById method was called with the correct employeeId
            _employeeServiceMock.Verify(x => x.GetEmployeeById(_employeeId), times);
        }

        private void VerifyDeleteQualificationCalled(Func<Times> times)
        {
            // Verify that the Delete method of the repository was called with the correct ID
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Delete(_qualificationId), times);
        }

        [Fact]
        public async Task SaveEmployeeQualification_Success_WithValidData_ReturnsQualificationsList()
        {
            // Setup mock
            SetupEmployeeExists();
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
                .ReturnsAsync(_employeeQualificationDto);

            // Act
            var result = await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, _employeeId);

            // Assert

            // Verify that the result is not null
            Assert.NotNull(result);

            // Verify that the returned result matches the expected employee qualification DTO
            Assert.Equal(_employeeQualificationDto, result);

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Never);
            VerifyAddQualificationCalled(Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeQualification_Failure_WithNonExistingEmployee_ThrowsException()
        {
            // Setup mocks 
            SetupLogException();
            SetupEmployeeDoesNotExist();

            // Act
            // Execute the method under test and capture the thrown exception
            async Task Act() => await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, _employeeId);
            var exception = await Assert.ThrowsAsync<Exception>(Act);

            // Assert
            // Verify that the exception is not null
            Assert.NotNull(exception);

            VerifyGetEmployeeByIdCalled(Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Success_WithValidData_ReturnsQualificationsList()
        {
            // Setup mock
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.GetAll(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync(new List<EmployeeQualificationDto> { _employeeQualificationDto });

            // Act
            var result = await _employeeQualificationService.GetAllEmployeeQualifications();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Since there's only one qualification in the list
            var expectedQualification = result.First();
            Assert.Equal(_employeeQualificationDto, expectedQualification);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Failure_WhenRetrievalFails_ThrowsException()
        {
            // Setup mock
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.GetAll(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ThrowsAsync(new Exception("Failed to retrieve qualifications."));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.GetAllEmployeeQualifications();
            });
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Success_WithValidEmployeeId_ReturnsQualifications()
        {
            // Setup mock
            SetupEmployeeExists();

            // Setup mock qualifications
            var mockQualifications = new List<EmployeeQualification>
            {
                new EmployeeQualification { EmployeeId = _employeeId, HighestQualification = HighestQualification.Bachelor },
                new EmployeeQualification { EmployeeId = _employeeId, HighestQualification = HighestQualification.Master }
            }.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                           .Returns(mockQualifications);

            // Act
            var result = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(_employeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(HighestQualification.Bachelor, result[0].HighestQualification);
            Assert.Equal(HighestQualification.Master, result[1].HighestQualification);
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_WithInvalidEmployeeId_ThrowsInvalidOperationException()
        {
            // Setup mock 
            SetupLogException();

            // Setup mock to throw an exception when getting employee by ID
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ThrowsAsync(new InvalidOperationException("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                // Act: Calling the method under test
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(_employeeId);
            });
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_WithNonExistingEmployee_ThrowsKeyNotFoundException()
        {
            // Setup mock
            SetupLogException();
            SetupEmployeeDoesNotExist();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                // Act: Calling the method under test
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(_employeeId);
            });

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_ExceptionThrown_LogsAndRethrows()
        {
            // Setup mock
            SetupLogException();
            SetupEmployeeExists();

            // Setup mock to throw an exception when retrieving qualifications
            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .Throws(new Exception("Test exception"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(async () =>
            {
                // Act: Calling the method under test
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(_employeeId);
            });

            // Assert
            // Verify that the exception message matches the expected message
            Assert.Equal("Test exception", ex.Message);

            VerifyGetEmployeeByIdCalled(Times.Once);
            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task GetEmployeeQualificationById_Success_WithExistingId_ReturnsQualification()
        {
            // Setup mock
            SetupLogException();

            var mockQualificationQuery = new List<EmployeeQualification> { _employeeQualification }
                .AsQueryable()
                .BuildMock();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .Returns(mockQualificationQuery);

            // Act
            var result = await _employeeQualificationService.GetEmployeeQualificationById(_qualificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_employeeQualificationDto.Id, result.Id);
            Assert.Equal(_employeeQualificationDto.EmployeeId, result.EmployeeId);
            Assert.Equal(_employeeQualificationDto.HighestQualification, result.HighestQualification);
            Assert.Equal(_employeeQualificationDto.School, result.School);
            Assert.Equal(_employeeQualificationDto.Degree, result.Degree);
            Assert.Equal(_employeeQualificationDto.FieldOfStudy, result.FieldOfStudy);
            Assert.Equal(_employeeQualificationDto.NQFLevel, result.NQFLevel);
            Assert.Equal(_employeeQualificationDto.Year, result.Year);
        }

        [Fact]
        public async Task GetEmployeeQualificationById_Failure_WithNonExistingId_ThrowsException()
        {
            // Setup mock
            SetupLogException();

            var mockQualifications = new List<EmployeeQualification>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(u => u.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
               .Returns(mockQualifications); // Returning the mocked IQueryable

            var service = new EmployeeQualificationService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await _employeeQualificationService.GetEmployeeQualificationById(_nonExistingQualificationId));
        }

        [Fact]
        public async Task GetEmployeeQualificationById_FailureWithNonExistingId_ThrowsNullException()
        {
            // Setup mock
            SetupLogException();
            SetupEmployeeDoesNotExist();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _employeeQualificationService.GetEmployeeQualificationById(_qualificationId);
            });
        }

        [Fact]
        public async Task UpdateEmployeeQualification_Success()
        {
            // Setup mock
            _unitOfWorkMock.Setup(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
               .ReturnsAsync(_employeeQualificationDto);

            // Act
            var result = await _employeeQualificationService.UpdateEmployeeQualification(_employeeQualificationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_employeeQualificationDto, result);
        }

        [Fact]
        public async Task UpdateEmployeeQualification_Failure()
        {
            // Setup mock
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync((EmployeeQualificationDto)null); // Simulating scenario where no employee qualification is found

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _employeeQualificationService.UpdateEmployeeQualification(_employeeQualificationDto);
            });

            // Assert
            Assert.NotNull(exception);

            VerifyLogExceptionCalled(Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeQualification_Success()
        {
            // Arrange
            var deletedQualificationDto = new EmployeeQualificationDto { Id = _qualificationId }; // Mock the deleted qualification DTO
           
            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Delete(_qualificationId))
                .ReturnsAsync(deletedQualificationDto); // Setup the mock to return the deleted qualification DTO

            // Act
            var result = await _employeeQualificationService.DeleteEmployeeQualification(_qualificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_qualificationId, result.Id); // Verify that the returned DTO has the expected ID

            VerifyDeleteQualificationCalled(Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeQualification_Failure_WhenDeletionFails_ThrowsException()
        {
            // Setup mock
            SetupLogException();

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Delete(_qualificationId))
                .ThrowsAsync(new Exception("Delete operation failed")); // Simulate a failure scenario where an exception occurs during deletion

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.DeleteEmployeeQualification(_qualificationId);
            });

            // Assert
            Assert.Equal("Delete operation failed", exception.Message); // Verify the correct exception message

            VerifyDeleteQualificationCalled(Times.Once);
        }

    }
}
