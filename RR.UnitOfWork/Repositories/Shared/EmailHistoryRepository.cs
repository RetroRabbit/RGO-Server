using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.Shared;

namespace RR.UnitOfWork.Repositories.Shared;

public interface IEmailHistoryRepository : IRepository<EmailHistory>
{
}

public class EmailHistoryRepository : BaseRepository<EmailHistory>, IEmailHistoryRepository
{
    public EmailHistoryRepository(DatabaseContext db) : base(db)
    { }
}