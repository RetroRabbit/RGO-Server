using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.EmployeeProfile;

public class EmployeeAddressMap :Profile
{
    public EmployeeAddressMap ()
    {
        CreateMap<EmployeeAddress, EmployeeAddressDto> ();
        CreateMap<EmployeeAddressDto, EmployeeAddress>();
    }
}
