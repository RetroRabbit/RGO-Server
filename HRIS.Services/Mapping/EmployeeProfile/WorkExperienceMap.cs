using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Mapping.EmployeeProfile;

public class WorkExperienceMap : Profile
{
    public WorkExperienceMap()
    {
        CreateMap<WorkExperience, WorkExperienceDto>();
        CreateMap<WorkExperienceDto, WorkExperience>();
    }
}
