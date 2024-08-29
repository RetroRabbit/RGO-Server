using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeMap : Profile
{
    public EmployeeMap()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeDto, Employee>();
        CreateMap<EmployeeSalaryDetailsDto, Employee>();

        CreateMap<EmployeeDto, EmployeeProfileDto>()
            .ForMember(dest => dest.ClientAllocatedId, opt => opt.MapFrom(src => src.ClientAllocated))
            .ForMember(dest => dest.TeamLeadId, opt => opt.MapFrom(src => src.TeamLead))
            .ForMember(dest => dest.PeopleChampionId, opt => opt.MapFrom(src => src.PeopleChampion));
    }
}