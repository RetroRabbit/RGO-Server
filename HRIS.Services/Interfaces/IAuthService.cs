﻿using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns>true if user with given email is found else false</returns>
    Task<bool> CheckUserExist(string email);

    /// <summary>
    ///     Get user roles by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of role, will reflect as list of int</returns>
    Task<Dictionary<string, List<string>>> GetUserRoles(string email);

    /// <summary>
    ///     Login user
    /// </summary>
    /// <param name="email"></param>
    /// <returns>JWT token</returns>
    Task<string> Login(string email);

    /// <summary>
    ///     Register new employee
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>JWT token</returns>
    Task<string> RegisterEmployee(EmployeeDto employeeDto);

    /// <summary>
    ///     Generate a bearer token
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>JWT as a string</returns>
    Task<string> GenerateToken(EmployeeDto employee);
}