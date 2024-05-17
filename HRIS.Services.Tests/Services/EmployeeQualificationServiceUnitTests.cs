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

        private readonly int _employeeId;
        private readonly int _qualificationId;

        private const int _nonExistingQualificationId = 9999;

        public EmployeeQualificationServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
            _employeeQualificationService = new EmployeeQualificationService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

            // Initialize shared test mock data
            _employeeId = 1;
            _qualificationId = 1;

            _employeeDto = new EmployeeDto
            {
                Id = 1,
                EmployeeNumber = "EMP001",
            };

            _employeeQualificationDto = new EmployeeQualificationDto
            {
                Id = 1,
                EmployeeId = 1,
                HighestQualification = HighestQualification.Bachelor,
                School = "Example School",
                Degree = "Example Degree",
                FieldOfStudy = "Example Field",
                NQFLevel = Models.Enums.QualificationEnums.NQFLevel.Level7,
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

            _employeeQualificationDtoList = new List<EmployeeQualificationDto>
            {
                // Add sample data here
                new EmployeeQualificationDto
                {
                    Id = 1,
                    EmployeeId = 1,
                    HighestQualification = HighestQualification.Bachelor,
                    School = "Example School",
                    Degree = "Example Degree",
                    FieldOfStudy = "Example Field",
                    NQFLevel = Models.Enums.QualificationEnums.NQFLevel.Level7,
                    Year = new DateOnly(2018, 4, 6)
                },
                // Add more sample data if needed
            };
        }

        [Fact]
        public async Task SaveEmployeeQualification_Success_WithValidData_ReturnsQualificationsList()
        {
            // Mock setup for dependencies
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync(_employeeDto);
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);
            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
                .ReturnsAsync(_employeeQualificationDto);

            // Act
            var result = await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, _employeeId);

            // Assert
            // Verify that the result is not null
            Assert.NotNull(result);

            // Additional Assertions
            // Verify that the employee service's GetEmployeeById method was called with the correct employeeId
            _employeeServiceMock.Verify(x => x.GetEmployeeById(_employeeId), Times.Once);

            // Verify that the error logging service was not called during a successful operation
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Never);

            // Verify that the unitOfWork's EmployeeQualification.Add method was called with the correct parameter
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Once);

            // Verify that the returned result matches the expected employee qualification DTO
            Assert.Equal(_employeeQualificationDto, result);
        }

        [Fact]
        public async Task SaveEmployeeQualification_Failure_WithNonExistingEmployee_ThrowsException()
        {
            // Setup mock for NullReferenceException 
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            // Setup mock to return null, simulating the scenario where employee does not exist
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync((EmployeeDto)null);

            // Act
            // Execute the method under test and capture the thrown exception
            async Task Act() => await _employeeQualificationService.SaveEmployeeQualification(_employeeQualificationDto, _employeeId);
            var exception = await Assert.ThrowsAsync<Exception>(Act);

            // Assert
            // Verify that the exception is not null
            Assert.NotNull(exception);

            // Additional Assertion
            // Verify that the GetEmployeeById method of the employee service was called with the correct employeeId
            _employeeServiceMock.Verify(x => x.GetEmployeeById(_employeeId), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Success_WithValidData_ReturnsQualificationsList()
        {
            // Setup mock to return the shared test data
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);
            _unitOfWorkMock.Setup(x => x.EmployeeQualification.GetAll(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync(new List<EmployeeQualificationDto> { _employeeQualificationDto });

            // Act
            var result = await _employeeQualificationService.GetAllEmployeeQualifications();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Since there's only one qualification in the list
            var expectedQualification = result.First();
            Assert.Equal(_employeeQualificationDto.EmployeeId, expectedQualification.EmployeeId);
            Assert.Equal(_employeeQualificationDto.HighestQualification, expectedQualification.HighestQualification);
            Assert.Equal(_employeeQualificationDto.School, expectedQualification.School);
            Assert.Equal(_employeeQualificationDto.Degree, expectedQualification.Degree);
            Assert.Equal(_employeeQualificationDto.FieldOfStudy, expectedQualification.FieldOfStudy);
            Assert.Equal(_employeeQualificationDto.NQFLevel, expectedQualification.NQFLevel);
            Assert.Equal(_employeeQualificationDto.Year, expectedQualification.Year);
        }

        [Fact]
        public async Task GetAllEmployeeQualifications_Failure_WhenRetrievalFails_ThrowsException()
        {
            // Setup mock
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);
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
            // Setup mock for getting employee by ID
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync(_employeeDto);

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
            // Setup mock for NullReferenceException 
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);
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
            // Setup mock for error logging service
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            // Setup mock to return null when getting employee by ID
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync((EmployeeDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                // Act: Calling the method under test
                await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(_employeeId);
            });

            // Additional Assertions
            // Verify that GetEmployeeById method of EmployeeService was called exactly once with the expected employeeId
            _employeeServiceMock.Verify(x => x.GetEmployeeById(_employeeId), Times.Once);

            // Verify that LogException method of error logging service was called once with any exception
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsByEmployeeId_Failure_ExceptionThrown_LogsAndRethrows()
        {
            // Setup mock for error logging service
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            // Setup mock to return a valid employee DTO
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync(_employeeDto);

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

            // Additional Assertions
            // Verify that GetEmployeeById method of EmployeeService was called exactly once with the expected employeeId
            _employeeServiceMock.Verify(x => x.GetEmployeeById(_employeeId), Times.Once);

            // Verify that LogException method of error logging service was called once with the exception thrown by the method under test
            _errorLoggingServiceMock.Verify(x => x.LogException(ex), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeQualificationById_Success_WithExistingId_ReturnsQualification()
        {
            // Setup mock
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

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
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

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
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            // Setup mock for the employee service to return null, simulating no employee found
            _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeId)).ReturnsAsync((EmployeeDto)null);

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
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
                .ReturnsAsync((EmployeeQualificationDto)null); // Simulating scenario where no employee qualification is found

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _employeeQualificationService.UpdateEmployeeQualification(_employeeQualificationDto);
            });

            // Assert
            Assert.NotNull(exception);

            // Verify that the error logging service was called with the exception
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
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

            // Verify that the Delete method of the repository was called with the correct ID
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Delete(_qualificationId), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeQualification_Failure_WhenDeletionFails_ThrowsException()
        {
            // Setup mock
            _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
                .Returns<Exception>((ex) => ex);

            _unitOfWorkMock.Setup(x => x.EmployeeQualification.Delete(_qualificationId))
                .ThrowsAsync(new Exception("Delete operation failed")); // Simulate a failure scenario where an exception occurs during deletion

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _employeeQualificationService.DeleteEmployeeQualification(_qualificationId);
            });

            // Assert
            Assert.Equal("Delete operation failed", exception.Message); // Verify the correct exception message

            // Verify that the Delete method of the repository was called with the correct ID
            _unitOfWorkMock.Verify(x => x.EmployeeQualification.Delete(_qualificationId), Times.Once);
        }

    }
}
