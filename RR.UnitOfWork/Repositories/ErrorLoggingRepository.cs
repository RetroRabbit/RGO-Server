using ATS.Models;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Repositories;

public class ErrorLoggingRepository: BaseRepository<ErrorLogging,ErrorLoggingDto>, IErrorLoggingRepository
{
    public ErrorLoggingRepository(DatabaseContext db) : base(db)
    { }
}
