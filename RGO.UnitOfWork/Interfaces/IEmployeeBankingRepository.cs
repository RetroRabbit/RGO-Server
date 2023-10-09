using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork.Interfaces;

public interface IEmployeeBanking : IRepository<EmployeeBanking, EmployeeBankingDto>
{
}