﻿using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork.Interfaces;

public interface IPropertyAccessRepository : IRepository<PropertyAccess, PropertyAccessDto>
{
}
