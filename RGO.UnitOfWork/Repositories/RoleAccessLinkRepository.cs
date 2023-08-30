using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class RoleAccessLinkRepository : BaseRepository<RoleAccessLink, RoleAccessLinkDto>, IRoleAccessLinkRepository
{
    public RoleAccessLinkRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}