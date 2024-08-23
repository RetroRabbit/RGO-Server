using AutoMapper;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Mapping.Common;

public class PropertyAccessMap : Profile
{
    public PropertyAccessMap() 
    { 
        CreateMap<PropertyAccess, PropertyAccessDto>();
        CreateMap<PropertyAccessDto, PropertyAccess>();
    }
}
