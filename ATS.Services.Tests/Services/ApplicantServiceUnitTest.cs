using ATS.Models;
using ATS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.ATS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;
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
            //var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };

            _mockUnitOfWork
                .Setup(u => u.Candidate.Any(It.IsAny<Expression<Func<Candidate, bool>>>()))
           .Returns(Task.FromResult(true));

            var serviceResult = await _applicantService.CheckCandidateExists(CandidateDtoTestData.CandidateDto.PersonalEmail);
            var actionResult = Xunit.Assert.IsType<bool>(serviceResult);

            Xunit.Assert.True(actionResult);
        }

        [Fact]
        public async Task SaveCandidatePass()
        {
            var candidateList = new List<Candidate> {
                 new Candidate(CandidateDtoTestData.CandidateDto)
             };

            _mockUnitOfWork
                .Setup(u => u.Candidate.Get(It.IsAny<Expression<Func<Candidate, bool>>>()))
                .Returns(candidateList.AsQueryable().BuildMock());

            _mockUnitOfWork
                .Setup(u => u.Candidate.Add(It.IsAny<Candidate>()))
                .ReturnsAsync(CandidateDtoTestData.CandidateDto);

            var serviceResult = await _applicantService.SaveCandidate(CandidateDtoTestData.CandidateDto);
            //var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

            //CandidateDto? newCandidate = actionResult.Value as CandidateDto;

            Xunit.Assert.Equal(CandidateDtoTestData.CandidateDto, serviceResult);
        }

        [Fact]
        public async Task GetAllCandidatesPass()
        {
            var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };

            _mockUnitOfWork
                .Setup(u => u.Candidate.GetAll(null))
           .Returns(Task.FromResult(candidates));

            var serviceResult = await _applicantService.GetAllCandidates();
            //var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

            List<CandidateDto>? fetchedCandidates = serviceResult as List<CandidateDto>;

            Xunit.Assert.NotNull(fetchedCandidates);
            Xunit.Assert.Equal(2, fetchedCandidates.Count);
        }

         [Fact]
         public async Task GetCandidateByIdPass()
         {
             var candidates = new List<Candidate> { 
                 new Candidate(CandidateDtoTestData.CandidateDto)
             };

             _mockUnitOfWork
                 .Setup(u => u.Candidate.Get(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .Returns(candidates.AsQueryable().BuildMock());

             var serviceResult = await _applicantService.GetCandidateById(CandidateDtoTestData.CandidateDto.Id);
              //var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

             Assert.NotNull(serviceResult);
             Assert.Equivalent(CandidateDtoTestData.CandidateDto, serviceResult);
         }

        //[Fact]
        //public async Task GetCandidateByEmailPass()
        //{
        //    var candidates = new List<CandidateDto> { CandidateDtoTestData.CandidateDto, CandidateDtoTestData.CandidateDtoTwo };

        //    _mockUnitOfWork
        //        .Setup(u => u.Candidate.GetAll(null))
        //   .Returns(Task.FromResult(candidates));

        //    var serviceResult = await _applicantService.GetCandidateByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail);
        //    var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

        //    Xunit.Assert.NotNull(actionResult.Value);
        //    Xunit.Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
        //}

        //[Fact]
        //public async Task UpdateCandidatePass()
        //{
        //    _mockUnitOfWork
        //        .Setup(u => u.Candidate.Update(new Candidate(CandidateDtoTestData.CandidateDto)))
        //    .ReturnsAsync(CandidateDtoTestData.CandidateDto);

        //    var serviceResult = await _applicantService.UpdateCandidate(CandidateDtoTestData.CandidateDto);
        //    var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

        //    Xunit.Assert.NotNull(actionResult.Value);
        //    Xunit.Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
        //}

        //[Fact]
        //public async Task DeleteCandidatePass()
        //{
        //    _mockUnitOfWork
        //        .Setup(u => u.Candidate.Add(new Candidate(CandidateDtoTestData.CandidateDto)))
        //   .ThrowsAsync(new Exception("User Does not Exist"));

        //    var serviceResult = await _applicantService.DeleteCandidate(CandidateDtoTestData.CandidateDto.Id);
        //    var actionResult = Xunit.Assert.IsType<OkObjectResult>(serviceResult);

        //    Xunit.Assert.NotNull(actionResult.Value);
        //    Xunit.Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
        //}
    }
}
