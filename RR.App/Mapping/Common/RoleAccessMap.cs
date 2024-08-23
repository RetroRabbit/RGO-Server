using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.Common;

public class RoleAccessMap : Profile
{
    public RoleAccessMap() 
    {
        CreateMap<RoleAccess, RoleAccessDto>();
        CreateMap<RoleAccessDto, RoleAccess>();
    }
}
