using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;

namespace RGO.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public IGradEventsRepository GradEvents { get; private set; }
    public IGradGroupRepository GradGroup { get; private set; }
    public IGradStackRepository GradStack { get; private set; }
    public IStackRepository Stack { get; private set; }
    public IUserRepository User { get; private set; }
    public IWorkshopRepository Workshop { get; private set; }

    private DatabaseContext _db;

    public UnitOfWork(DatabaseContext db)
    {
        _db = db;
        GradEvents = new GradEventsRepository(_db);
        GradGroup = new GradGroupRepository(_db);
        Stack = new StackRepository(_db);
        User = new UserRepository(_db);
        Workshop = new WorkshopRepository(_db);
        GradStack = new GradStackRepository(_db);
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}