using ATS.Models;
using ATS.Services.Services;
using Moq;
using Newtonsoft.Json;
using RR.Tests.Data.Models.ATS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;
using System.Linq.Expressions;
using RR.Tests.Data;
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
                .Setup(u => u.Candidate.Any(It.IsAny<Expression<Func<Candidate, bool>>>()))
           .ReturnsAsync(true);

            var serviceResult = await _applicantService.CheckCandidateExists(CandidateTestData.CandidateOne.PersonalEmail);
            var actionResult = Xunit.Assert.IsType<bool>(serviceResult);

            Xunit.Assert.True(actionResult);
        }

        [Fact]
        public async Task SaveCandidatePass()
        {
            var candidates = new List<Candidate> { CandidateTestData.CandidateOne, CandidateTestData.CandidateTwo };
            _mockUnitOfWork.Setup(x => x.Candidate.GetAll(null)).ReturnsAsync(candidates);

            _mockUnitOfWork.Setup(x => x.Candidate.Add(It.IsAny<Candidate>())).ReturnsAsync(CandidateTestData.CandidateOne);

            var serviceResult = await _applicantService.SaveCandidate(CandidateTestData.CandidateOne.ToDto());
            var actionResult = Xunit.Assert.IsType<CandidateDto>(serviceResult);

            Xunit.Assert.Equivalent(CandidateTestData.CandidateOne.ToDto(), actionResult);
        }

        [Fact]
        public async Task SaveCandidateFail()
        {
            var candidates = new List<Candidate> { CandidateTestData.CandidateOne, CandidateTestData.CandidateTwo };
            _mockUnitOfWork.Setup(x => x.Candidate.GetAll(null)).ReturnsAsync(candidates);

            _mockUnitOfWork.Setup(x => x.Candidate.Any(It.IsAny<Expression<Func<Candidate, bool>>>()))
                   .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _applicantService.SaveCandidate(CandidateTestData.CandidateOne.ToDto()));
        }

        [Fact]
        public async Task GetAllCandidatesPass()
        {
            var candidates = new List<Candidate> { CandidateTestData.CandidateOne, CandidateTestData.CandidateTwo };

            _mockUnitOfWork
                .Setup(u => u.Candidate.GetAll(null))
           .ReturnsAsync(candidates);

            var serviceResult = await _applicantService.GetAllCandidates();

            var fetchedCandidates = serviceResult as List<CandidateDto>;

            Xunit.Assert.NotNull(fetchedCandidates);
            Xunit.Assert.Equal(2, fetchedCandidates.Count);
        }

         [Fact]
         public async Task GetCandidateByIdPass()
         {
             var candidates = new List<Candidate> { 
                 CandidateTestData.CandidateOne
             };

             _mockUnitOfWork
                 .Setup(u => u.Candidate.Get(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .Returns(candidates.ToMockIQueryable());

             var serviceResult = await _applicantService.GetCandidateById(CandidateTestData.CandidateOne.Id);

             Assert.NotNull(serviceResult);
             Assert.Equivalent(CandidateTestData.CandidateOne.ToDto(), serviceResult);
         }

        [Fact]
        public async Task GetCandidateByEmailPass()
        {
            var candidates = new List<Candidate> {
                 CandidateTestData.CandidateOne
             };
            
             _mockUnitOfWork
                 .Setup(u => u.Candidate.Get(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .Returns(candidates.ToMockIQueryable());

            var serviceResult = await _applicantService.GetCandidateByEmail(CandidateTestData.CandidateOne.PersonalEmail);

            Assert.NotNull(serviceResult); 
            var actualResult = JsonConvert.SerializeObject(serviceResult);
            var expectedResult = JsonConvert.SerializeObject(CandidateTestData.CandidateOne);

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task UpdateCandidatePass()
        {
            _mockUnitOfWork.Setup(x => x.Candidate.Update(It.IsAny<Candidate>()))
                   .ReturnsAsync(CandidateTestData.CandidateOne);

            var serviceResult = await _applicantService.UpdateCandidate(CandidateTestData.CandidateOne.ToDto());
            var actionResult = Assert.IsType<CandidateDto>(serviceResult);

            Assert.Equivalent(CandidateTestData.CandidateOne.ToDto(), actionResult);

            _mockUnitOfWork.Verify(x => x.Candidate.Update(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidatePass()
        {
            _mockUnitOfWork.Setup(x => x.Candidate.Delete(It.IsAny<int>()))
                   .ReturnsAsync(CandidateTestData.CandidateOne);

            var serviceResult = await _applicantService.DeleteCandidate(CandidateTestData.CandidateOne.Id);
            var actionResult = Assert.IsType<CandidateDto>(serviceResult);

            Assert.Equivalent(CandidateTestData.CandidateOne.ToDto(), actionResult);

            _mockUnitOfWork.Verify(x => x.Candidate.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
