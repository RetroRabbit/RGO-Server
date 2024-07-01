using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IWorkExperienceRepository : IRepository<WorkExperience>
{
}

public class WorkExperienceRepository : BaseRepository<WorkExperience>, IWorkExperienceRepository
{
    public WorkExperienceRepository(DatabaseContext db) : base(db)
    { }
}


