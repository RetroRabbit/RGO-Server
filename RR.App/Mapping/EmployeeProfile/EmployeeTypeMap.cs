using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeTypeMap: Profile
{
    public EmployeeTypeMap()
    {
        CreateMap<EmployeeType, EmployeeTypeDto>();
    }
}
