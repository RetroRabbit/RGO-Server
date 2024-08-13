using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Mapping;

public class EmployeeDataMap : Profile
{
    public EmployeeDataMap()
    {
        CreateMap<EmployeeData, EmployeeDataDto>();
    }
}