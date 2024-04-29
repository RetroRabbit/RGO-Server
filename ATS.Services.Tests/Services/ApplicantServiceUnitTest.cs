using ATS.Models;
using ATS.Services.Services;
using HRIS.Models;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Newtonsoft.Json;
using RR.Tests.Data.Models.ATS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;
using RR.UnitOfWork.Entities.HRIS;
using System.Linq.Expressions;
using Xunit;

namespace ATS.Services.Tests.Services
{
    public class ApplicantServiceUnitTest
    {
        private readonly CandidateService _applicantService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public ApplicantServiceUnitTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _applicantService = new CandidateService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CheckCandidateExistPass()
        {
            _mockUnitOfWork
                .Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
           .Returns(Task.FromResult(true));

            var serviceResult = await _applicantService.CheckCandidateExists(CandidateDtoTestData.CandidateDto.PersonalEmail);
            var actionResult = Assert.IsType<bool>(serviceResult);

            Assert.True(actionResult);
        }

        [Fact]
        public async Task SaveCandidatePass()
        {
            var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };
            var employeeCandidates = new List<EmployeeDto> { new Employee(CandidateDtoTestData.CandidateDto).ToDto(), new Employee(CandidateDtoTestData.CandidateDtoTwo).ToDto() };
            _mockUnitOfWork.Setup(x => x.Employee.GetAll(null)).Returns(Task.FromResult(employeeCandidates));

            _mockUnitOfWork.Setup(x => x.Employee.Add(It.IsAny<Employee>()))
                   .Returns(Task.FromResult(new Employee(CandidateDtoTestData.CandidateDto).ToDto()));

            var serviceResult = await _applicantService.SaveCandidate(CandidateDtoTestData.CandidateDto);
            var actionResult = Assert.IsType<CandidateDto>(serviceResult);

            Assert.Equivalent(CandidateDtoTestData.CandidateDto, actionResult);
        }

        [Fact]
        public async Task SaveCandidateFail()
        {
            var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };
            var employeeCandidates = new List<EmployeeDto> { new Employee(CandidateDtoTestData.CandidateDto).ToDto(), new Employee(CandidateDtoTestData.CandidateDtoTwo).ToDto() };
            _mockUnitOfWork.Setup(x => x.Employee.GetAll(null)).Returns(Task.FromResult(employeeCandidates));

            _mockUnitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _applicantService.SaveCandidate(CandidateDtoTestData.CandidateDto));
        }

        [Fact]
        public async Task GetAllCandidatesPass()
        {
            var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };
            var employeeCandidates = new List<EmployeeDto> { new Employee(CandidateDtoTestData.CandidateDto).ToDto(), new Employee(CandidateDtoTestData.CandidateDtoTwo).ToDto() };
            _mockUnitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(employeeCandidates));

            var serviceResult = await _applicantService.GetAllCandidates();

            List<CandidateDto>? fetchedCandidates = serviceResult as List<CandidateDto>;

            Assert.NotNull(fetchedCandidates);
            Assert.Equal(2, fetchedCandidates.Count);
        }

         [Fact]
         public async Task GetCandidateByIdPass()
         {
             var candidates = new List<Employee> { 
                 new Employee(CandidateDtoTestData.CandidateDto)
             };

             _mockUnitOfWork
                 .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(candidates.AsQueryable().BuildMock());

             var serviceResult = await _applicantService.GetCandidateById(CandidateDtoTestData.CandidateDto.Id);

             Assert.NotNull(serviceResult);
             Assert.Equivalent(CandidateDtoTestData.CandidateDto, serviceResult);
         }

        [Fact]
        public async Task GetCandidateByEmailPass()
        {
            var candidates = new List<Employee> {
                 new Employee(CandidateDtoTestData.CandidateDto)
             };
            
             _mockUnitOfWork
                 .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(candidates.AsQueryable().BuildMock());

            var serviceResult = await _applicantService.GetCandidateByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail);

            Assert.NotNull(serviceResult); 
            var actualResult = JsonConvert.SerializeObject(serviceResult);
            var expectedResult = JsonConvert.SerializeObject(CandidateDtoTestData.CandidateDto);

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task UpdateCandidatePass()
        {
            _mockUnitOfWork.Setup(x => x.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee(CandidateDtoTestData.CandidateDto).ToDto()));

            var serviceResult = await _applicantService.UpdateCandidate(CandidateDtoTestData.CandidateDto);
            var actionResult = Assert.IsType<CandidateDto>(serviceResult);

            Assert.Equivalent(CandidateDtoTestData.CandidateDto, actionResult);

            _mockUnitOfWork.Verify(x => x.Employee.Update(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidatePass()
        {
            _mockUnitOfWork.Setup(x => x.Employee.Delete(It.IsAny<int>())).Returns(Task.FromResult(new Employee(CandidateDtoTestData.CandidateDto).ToDto()));

            var serviceResult = await _applicantService.DeleteCandidate(CandidateDtoTestData.CandidateDto.Id);
            var actionResult = Assert.IsType<CandidateDto>(serviceResult);

            Assert.Equivalent(CandidateDtoTestData.CandidateDto, actionResult);

            _mockUnitOfWork.Verify(x => x.Employee.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
