using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class TerminationRepository : BaseRepository<Termination, TerminationDto>, ITerminationRepository
{
    public TerminationRepository(DatabaseContext db) : base(db)
    {
    }
}