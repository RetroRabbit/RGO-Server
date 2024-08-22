using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeSalaryDetailsMap : Profile
{
    public EmployeeSalaryDetailsMap() 
    { 
        CreateMap<EmployeeSalaryDetails, EmployeeSalaryDetailsDto>();
        CreateMap<EmployeeSalaryDetailsDto, EmployeeSalaryDetails>();
    }
}
