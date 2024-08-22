using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;
public class TerminationMap : Profile
{
    public TerminationMap() 
    { 
        CreateMap<Termination, TerminationDto>();
        CreateMap<TerminationDto, Termination>();
    }
}