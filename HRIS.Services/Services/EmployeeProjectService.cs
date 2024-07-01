using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeProjectService : IEmployeeProjectService
{
    private readonly IUnitOfWork _db;

    public EmployeeProjectService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeProjectDto> SaveEmployeeProject(EmployeeProjectDto employeeProjectDto)
    {
        var newEmployeeProject = await _db.EmployeeProject.Add(new EmployeeProject(employeeProjectDto));

        return newEmployeeProject.ToDto();
    }

    public async Task<EmployeeProjectDto> DeleteEmployeeProject(string name)
    {
        var existsingEmployeeProject = await GetEmployeeProject(name);

        var employeeProjectDto = await _db.EmployeeProject
                                          .Delete(existsingEmployeeProject.Id);

        return employeeProjectDto.ToDto();
    }

    public async Task<EmployeeProjectDto> GetEmployeeProject(string name)
    {
        var existingEmployeeProject = await _db.EmployeeProject
                                               .Get(employeeProject => employeeProject.Name == name)
                                               .Select(employeeProject => employeeProject.ToDto())
                                               .FirstAsync();

        return existingEmployeeProject;
    }

    public async Task<EmployeeProjectDto> UpdateEmployeeProject(EmployeeProjectDto employeeProjectDto)
    {
        var existingEmployeeProject = await GetEmployeeProject(employeeProjectDto.Name!);

        var updatedEmployeeProject = await _db.EmployeeProject
                                              .Update(new EmployeeProject(existingEmployeeProject));

        return updatedEmployeeProject.ToDto();
    }
}