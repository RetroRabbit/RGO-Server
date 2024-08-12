

using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() 
        {
            //entity to dto
            CreateMap<EmployeeData, EmployeeDataDto>();

            //dto to entity
        }
    }
}
