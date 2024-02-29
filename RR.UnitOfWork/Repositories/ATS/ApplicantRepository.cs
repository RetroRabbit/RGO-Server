using ATS.Models;
using RR.UnitOfWork.Entities.ATS;
using RR.UnitOfWork.Interfaces.ATS;

namespace RR.UnitOfWork.Repositories.ATS;

public  class ApplicantRepository : BaseRepository<Applicant, ApplicantDto>, IApplicantRepository
{
    public ApplicantRepository(DatabaseContext db) : base(db) 
    { }
}
