using RR.UnitOfWork.Entities;

namespace RR.UnitOfWork.Repositories;

public interface IErrorLoggingRepository : IRepository<ErrorLogging>
{
}

public class ErrorLoggingRepository: BaseRepository<ErrorLogging>, IErrorLoggingRepository
{
    public ErrorLoggingRepository(DatabaseContext db) : base(db)
    { }
}
