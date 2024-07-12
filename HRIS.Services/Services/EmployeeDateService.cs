using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeDateService : IEmployeeDateService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;

    public EmployeeDateService(IUnitOfWork db, IEmployeeService employeeService)
    {
        _db = db;
        _employeeService = employeeService;
    }

    public async Task<bool> CheckIfExists(EmployeeDateDto employeeDate)
    {
        var exists = await _db.EmployeeDate.Any(x => x.Id == employeeDate.Id && x.Employee!.Email == employeeDate.Employee!.Email);
        return exists;
    }

    //----------------------------------//
    public async Task SaveEmployeeDate(EmployeeDateInput employeeDateInput)
    {
        var employee = await _employeeService.GetEmployee(employeeDateInput.Email!);
        var employeeDate = new EmployeeDateDto
        {
            Id = 0,
            Employee = employee,
            Subject = employeeDateInput.Subject,
            Note = employeeDateInput.Note,
            Date = employeeDateInput.Date
        };
        
        var exists = await CheckIfExists(employeeDate);

        if (exists)
            throw new CustomException("Employee Date already exists");

        await _db.EmployeeDate.Add(new EmployeeDate(employeeDate));
    }

    //----------------------------------//
    public async Task UpdateEmployeeDate(EmployeeDateDto employeeDate)
    {
        var employee = await _employeeService.GetEmployee(employeeDate.Employee!.Email!);
        var newEmployeeDate = new EmployeeDateDto
        {
            Id = employeeDate.Id,
            Employee = employee,
            Subject = employeeDate.Subject,
            Note = employeeDate.Note,
            Date = employeeDate.Date
        };
        
        var exists = await CheckIfExists(newEmployeeDate);

        if (!exists)
            throw new CustomException("Employee Date does not exist");

        var employeeDateToUpdate = new EmployeeDateDto
        {
            Id = newEmployeeDate.Id,
            Employee = newEmployeeDate.Employee,
            Subject = newEmployeeDate.Subject,
            Note = newEmployeeDate.Note,
            Date = newEmployeeDate.Date
        };

        await _db.EmployeeDate.Update(new EmployeeDate(employeeDateToUpdate));
    }

    public async Task Delete(int employeeDateId)
    {
        await _db.EmployeeDate.Delete(employeeDateId);
    }

    public async Task<EmployeeDateDto> Get(EmployeeDateDto employeeDate)
    {
        var employeeDateDto = await _db.EmployeeDate.Get(x =>
                                                             x.Employee!.Email == employeeDate.Employee!.Email &&
                                                             x.Subject == employeeDate.Subject &&
                                                             x.Note == employeeDate.Note)
                                       .AsNoTracking()
                                       .Include(x => x.Employee)
                                       .Include(x => x.Employee!.EmployeeType)
                                       .Select(x => x.ToDto())
                                       .FirstOrDefaultAsync();

        if (employeeDateDto == null)
            throw new CustomException("Employee Data does not exist");

        return employeeDateDto;
    }

    //----------------------------------//
    public List<EmployeeDateDto> GetEmployeeDates(DateOnly? date, string? email, string? subject)
    {
        if (date != null)
            return GetAllByDate((DateOnly)date);
        if (email != null)
            return GetAllByEmployee(email);
        if (subject != null)
            return GetAllBySubject(subject);
        return GetAll();
    }

    public List<EmployeeDateDto> GetAll()
    {
        var employeeDates = from employeeDate in _db.EmployeeDate.Get()
                            join employee in _db.Employee.Get() on employeeDate.EmployeeId equals employee.Id
                            orderby employeeDate.Date
                            select new EmployeeDateDto
                            {
                                Id = employeeDate.Id,
                                Employee = employee.ToDto(),
                                Subject = employeeDate.Subject,
                                Note = employeeDate.Note,
                                Date = employeeDate.Date
                            };

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllByDate(DateOnly Date)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
                                                           join employee in _db.Employee.Get() on employeeDate
                                                               .EmployeeId equals employee.Id
                                                           where employeeDate.Date == Date
                                                           select new EmployeeDateDto
                                                           {
                                                               Id = employeeDate.Id,
                                                               Employee = employee.ToDto(),
                                                               Subject = employeeDate.Subject,
                                                               Note = employeeDate.Note,
                                                               Date = employeeDate.Date
                                                           }
                                                           into employeeDateDto
                                                           orderby employeeDateDto.Date
                                                           select employeeDateDto;

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllByEmployee(string email)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
                                                           join employee in _db.Employee.Get() on employeeDate
                                                               .EmployeeId equals employee.Id
                                                           where employee.Email == email
                                                           select new EmployeeDateDto
                                                           {
                                                               Id = employeeDate.Id,
                                                               Employee = employee.ToDto(),
                                                               Subject = employeeDate.Subject,
                                                               Note = employeeDate.Note,
                                                               Date = employeeDate.Date
                                                           }
                                                           into employeeDateDto
                                                           orderby employeeDateDto.Date
                                                           select employeeDateDto;

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllBySubject(string subject)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
                                                           join employee in _db.Employee.Get() on employeeDate
                                                               .EmployeeId equals employee.Id
                                                           where employeeDate.Subject == subject
                                                           select new EmployeeDateDto
                                                           {
                                                               Id = employeeDate.Id,
                                                               Employee = employee.ToDto(),
                                                               Subject = employeeDate.Subject,
                                                               Note = employeeDate.Note,
                                                               Date = employeeDate.Date
                                                           }
                                                           into employeeDateDto
                                                           orderby employeeDateDto.Date
                                                           select employeeDateDto;

        return employeeDates.ToList();
    }
}
