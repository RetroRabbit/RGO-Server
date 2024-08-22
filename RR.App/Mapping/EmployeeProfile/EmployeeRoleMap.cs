using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeRoleMap: Profile
{
    public EmployeeRoleMap() 
    { 
        CreateMap<EmployeeRole, EmployeeRoleDto>();
        CreateMap<EmployeeRoleDto, EmployeeRole>();
    }
}
