using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeProjectService : IEmployeeProjectService
{
    private readonly IUnitOfWork _db;

    public EmployeeProjectService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeProjectDto> SaveEmployeeProject(EmployeeProjectDto employeeProjectDto)
    {
        EmployeeProjectDto newEmployeeProject = await _db.EmployeeProject.Add(new EmployeeProject(employeeProjectDto));

        return newEmployeeProject;
    }

    public async Task<EmployeeProjectDto> DeleteEmployeeProject(string name)
    {
        EmployeeProjectDto existsingEmployeeProject = await GetEmployeeProject(name);

        EmployeeProjectDto employeeProjectDto = await _db.EmployeeProject
            .Delete(existsingEmployeeProject.Id);

        return employeeProjectDto;
    }

    public Task<List<EmployeeProjectDto>> GetAllEmployeeProjects()
    {
        return _db.EmployeeProject.GetAll();
    }

    public async Task<EmployeeProjectDto> GetEmployeeProject(string name)
    {
        EmployeeProjectDto existingEmployeeProject = await _db.EmployeeProject
            .Get(employeeProject => employeeProject.Name == name)
            .Select(employeeProject => employeeProject.ToDto())
            .FirstAsync();

        return existingEmployeeProject;
    }

    public async Task<EmployeeProjectDto> UpdateEmployeeProject(EmployeeProjectDto employeeProjectDto)
    {
        EmployeeProjectDto existingEmployeeProject = await GetEmployeeProject(employeeProjectDto.Name);

        EmployeeProjectDto updatedEmployeeProject = await _db.EmployeeProject
            .Update(new EmployeeProject(existingEmployeeProject));

        return updatedEmployeeProject;
    }
}
