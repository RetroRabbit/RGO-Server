using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface ITerminationRepository : IRepository<Termination>
{
}

public class TerminationRepository : BaseRepository<Termination>, ITerminationRepository
{
    public TerminationRepository(DatabaseContext db) : base(db)
    {
    }
}