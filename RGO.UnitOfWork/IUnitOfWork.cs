using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork;

public interface IUnitOfWork
{
    IGradEventsRepository GradEvents { get; }
    IGradGroupRepository GradGroup { get; }
    IGradStackRepository GradStack { get; }
    IStackRepository Stack { get; }
    IUserRepository User { get; }
    IWorkshopRepository Workshop { get; }
}