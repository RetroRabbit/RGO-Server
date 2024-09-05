using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeBankingMap: Profile
{
    public EmployeeBankingMap() 
    {
        CreateMap<EmployeeBanking, EmployeeBankingDto>();
        CreateMap<EmployeeBankingDto, EmployeeBanking>();
    }
}
