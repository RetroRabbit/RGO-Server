using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class EmployeeDateService : IEmployeeDateService
{
    private IUnitOfWork _db;

    public EmployeeDateService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(EmployeeDateDto employeeDate)
    {
        bool exists = await _db.EmployeeDate.Any(x =>
            x.Employee.Email == employeeDate.Employee!.Email &&
            x.Subject == employeeDate.Subject &&
            x.Note == employeeDate.Note &&
            x.Date == employeeDate.Date);

        return exists;
    }

    public async Task Save(EmployeeDateDto employeeDate)
    {
        bool exists = await CheckIfExists(employeeDate);

        if (exists) throw new Exception("Employee Date already exists");

        await _db.EmployeeDate.Add(new (employeeDate));
    }

    public async Task Update(EmployeeDateDto newEmployeeDate)
    {
        bool exists = await CheckIfExists(newEmployeeDate);

        if (!exists) throw new Exception("Employee Date does not exist");

        EmployeeDateDto employeeDateToUpdate = new EmployeeDateDto(
            newEmployeeDate.Id,
            newEmployeeDate.Employee,
            newEmployeeDate.Subject,
            newEmployeeDate.Note,
            newEmployeeDate.Date);

        await _db.EmployeeDate.Update(new (employeeDateToUpdate));
    }

    public async Task Delete(EmployeeDateDto employeeDate)
    {
        bool exists = await CheckIfExists(employeeDate);

        if (!exists) throw new Exception("Employee Date does not exist");

        var employeeDateToDelete = await Get(employeeDate);

        await _db.EmployeeDate.Delete(employeeDateToDelete.Id);
    }

    public async Task<EmployeeDateDto> Get(EmployeeDateDto employeeDate)
    {

        EmployeeDateDto? employeeDateDto = await _db.EmployeeDate.Get(x =>
            x.Employee.Email == employeeDate.Employee!.Email &&
            x.Subject == employeeDate.Subject &&
            x.Note == employeeDate.Note)
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync();

        if (employeeDateDto == null) throw new Exception("Employee Data does not exist");

        return employeeDateDto;
    }

    public List<EmployeeDateDto> GetAll()
    {
        var employeeDates = from employeeDate in _db.EmployeeDate.Get()
            join employee in _db.Employee.Get() on employeeDate.EmployeeId equals employee.Id
            orderby employeeDate.Date
            select new EmployeeDateDto(
                employeeDate.Id,
                employee.ToDto(),
                employeeDate.Subject,
                employeeDate.Note,
                employeeDate.Date);

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllByDate(DateOnly Date)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
            join employee in _db.Employee.Get() on employeeDate.EmployeeId equals employee.Id
            where employeeDate.Date == Date
            select new EmployeeDateDto(
                employeeDate.Id,
                employee.ToDto(),
                employeeDate.Subject,
                employeeDate.Note,
                employeeDate.Date)
            into employeeDateDto
            orderby employeeDateDto.Date
            select employeeDateDto;

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllByEmployee(string email)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
            join employee in _db.Employee.Get() on employeeDate.EmployeeId equals employee.Id
            where employee.Email == email
            select new EmployeeDateDto(
                employeeDate.Id,
                employee.ToDto(),
                employeeDate.Subject,
                employeeDate.Note,
                employeeDate.Date)
            into employeeDateDto
            orderby employeeDateDto.Date
            select employeeDateDto;

        return employeeDates.ToList();
    }

    public List<EmployeeDateDto> GetAllBySubject(string subject)
    {
        IOrderedQueryable<EmployeeDateDto> employeeDates = from employeeDate in _db.EmployeeDate.Get()
            join employee in _db.Employee.Get() on employeeDate.EmployeeId equals employee.Id
            where employeeDate.Subject == subject
            select new EmployeeDateDto(
                employeeDate.Id,
                employee.ToDto(),
                employeeDate.Subject,
                employeeDate.Note,
                employeeDate.Date)
            into employeeDateDto
            orderby employeeDateDto.Date
            select employeeDateDto;

        return employeeDates.ToList();
    }
}
