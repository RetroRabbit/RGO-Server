using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployee(string email);
    }
}
