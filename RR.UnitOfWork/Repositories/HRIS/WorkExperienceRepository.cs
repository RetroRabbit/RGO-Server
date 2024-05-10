using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;


namespace RR.UnitOfWork.Repositories.HRIS;

public class WorkExperienceRepository : BaseRepository<WorkExperience, WorkExperienceDto>, IWorkExperienceRepository
{
    public WorkExperienceRepository(DatabaseContext db) : base(db)
    { }
}


