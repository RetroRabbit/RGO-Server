﻿using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IClientRepository : IRepository<Client, ClientDto>
{
}