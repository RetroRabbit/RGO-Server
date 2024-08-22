using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeQualificationMap :Profile
{
    public EmployeeQualificationMap() 
    {
        CreateMap<EmployeeQualification, EmployeeQualificationDto>();
        CreateMap<EmployeeQualificationDto, EmployeeQualification>();
    }
}
