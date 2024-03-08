using ATS.Models;
using RR.UnitOfWork.Entities;

namespace RR.UnitOfWork.Interfaces;

public interface IErrorLoggingRepository : IRepository<ErrorLogging,ErrorLoggingDto>
{
}
