using RR.UnitOfWork.Entities.Shared;

namespace RR.UnitOfWork.Repositories.Shared;

public interface IEmailTemplateRepository : IRepository<EmailTemplate>
{
}

public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(DatabaseContext db) : base(db)
    {
    }
}