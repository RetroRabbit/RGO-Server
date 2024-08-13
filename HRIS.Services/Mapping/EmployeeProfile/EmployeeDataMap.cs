using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Mapping.EmployeeProfile;

public class EmployeeDataMap : Profile
{
    public EmployeeDataMap()
    {
        CreateMap<EmployeeData, EmployeeDataDto>();
    }
}