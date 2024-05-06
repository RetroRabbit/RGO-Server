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
    }
}
