using ATS.Models;
using RR.UnitOfWork.Entities.ATS;

namespace RR.UnitOfWork.Interfaces.ATS;

public interface ICandidateRepository : IRepository<Candidate, CandidateDto>
{
}
