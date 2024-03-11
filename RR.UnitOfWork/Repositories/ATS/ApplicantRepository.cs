using ATS.Models;
using RR.UnitOfWork.Entities.ATS;
using RR.UnitOfWork.Interfaces.ATS;

namespace RR.UnitOfWork.Repositories.ATS;

public  class CandidateRepository : BaseRepository<Candidate, CandidateDto>, ICandidateRepository
{
    public CandidateRepository(DatabaseContext db) : base(db) 
    { }
}
