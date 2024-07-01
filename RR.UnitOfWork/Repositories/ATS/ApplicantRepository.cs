using RR.UnitOfWork.Entities.ATS;

namespace RR.UnitOfWork.Repositories.ATS;

public interface ICandidateRepository : IRepository<Candidate>
{
}

public  class CandidateRepository : BaseRepository<Candidate>, ICandidateRepository
{
    public CandidateRepository(DatabaseContext db) : base(db) 
    { }
}
