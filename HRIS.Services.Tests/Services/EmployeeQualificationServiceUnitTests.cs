using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services
{
    public class EmployeeQualificationServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IEmployeeQualificationService _employeeQualificationService;

        public EmployeeQualificationServiceUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeQualificationService = new EmployeeQualificationService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task SaveEmployeeQualificationValidInputReturnsNewQualification()
        {
            var qualificationDto = new EmployeeQualificationDto
            {
                Id = 1,
                EmployeeId = 1,
                Qualification = "Bachelor's Degree",
                School = "University of Pretoria",
                Degree = "Computer Science",
                FieldOfStudy = "Software Engineering",
                NQF = "NQF Level 7",
                StartDate = new DateTime(2016, 9, 1),
                EndDate = new DateTime(2020, 6, 30)
            };
        }

        [Fact]
        public async Task GetAllEmployeeQualificationsReturnsListOfQualifications()
        {
            var qualifications = new List<EmployeeQualificationDto> 
            {
                new EmployeeQualificationDto
                {
                    Id = 1,
                    EmployeeId = 1,
                    Qualification = "Bachelor's Degree",
                    School = "University of Pretoria",
                    Degree = "Computer Science",
                    FieldOfStudy = "Software Engineering",
                    NQF = "NQF Level 7",
                    StartDate = new DateTime(2016, 9, 1),
                    EndDate = new DateTime(2020, 6, 30)
                },
                new EmployeeQualificationDto
                {
                    Id = 2,
                    EmployeeId = 1,
                    Qualification = "Master's Degree",
                    School = "University of Johannesburg",
                    Degree = "Information Technology",
                    FieldOfStudy = "Data Science",
                    NQF = "NQF Level 9",
                    StartDate = new DateTime(2018, 9, 1),
                    EndDate = new DateTime(2022, 6, 30)
                },
            };
        }
    }
}
