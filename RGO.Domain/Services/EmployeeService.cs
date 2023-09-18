using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Text;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IUnitOfWork _db;
    private const string QueueName = "employee_data_queue";
    public static ConnectionFactory _employeeFactory;
    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
    }

    public async Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto)
    {
        Employee employee;

        try
        {
            var ExistingEmployeeType = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType.Name);

            employee = new Employee(employeeDto, ExistingEmployeeType);
            PushToProducer(employee);
        }
        catch (Exception)
        {
            EmployeeTypeDto newEmployeeType = await _employeeTypeService
                .SaveEmployeeType(new EmployeeTypeDto(0, employeeDto.EmployeeType.Name));

            employee = new Employee(employeeDto, newEmployeeType);
        }

        EmployeeDto newEmployee = await _db.Employee.Add(employee);

        return newEmployee;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _db.Employee
            .Any(employee => employee.Email == email);
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        var existingEmployee = await GetEmployee(email);

        return await _db.Employee.Delete(existingEmployee.Id);
    }

    public async Task<List<EmployeeDto>> GetAll()
    {
        return await _db.Employee
            .Get(employee => true)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .ToListAsync();
    }

    public async Task<EmployeeDto> GetEmployee(string email)
    {
        var employee = await _db.Employee
            .Get(employee => employee.Email == email)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("User not found"); }

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email)
    {
        EmployeeTypeDto employeeTypeDto = await _employeeTypeService
            .GetEmployeeType(employeeDto.EmployeeType.Name);

        Employee employee = new Employee(employeeDto, employeeTypeDto);

        employee.Email = email;

        return await _db.Employee.Update(employee);
    }

    private void PushToProducer(Employee employeeData)
    {
        try
        {
            using (IConnection connection = _employeeFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                var messageBody = JsonConvert.SerializeObject(employeeData);
                var body = Encoding.UTF8.GetBytes(messageBody);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, null);
                channel.BasicPublish(string.Empty, QueueName, properties, body); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
