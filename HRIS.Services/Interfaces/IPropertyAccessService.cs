using HRIS.Models;
using HRIS.Models.Enums;

namespace HRIS.Services.Interfaces;

public interface IPropertyAccessService
{

    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="employeeId"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task<List<PropertyAccessDto>> GetAccessListByEmployeeId(int employeeId);

    ///// <summary>
    /////     Get's all the Property that a user has access to based on role
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task<List<PropertyAccessDto>> GetAccessListByRoleId(int roleId);

    ///// <summary>
    /////     Get's all the Properties
    ///// </summary>
    ///// <param name=""></param>
    ///// <returns>dictionary of properties with the access level for all user</returns>
    Task<List<PropertyAccessDto>> GetAll();

    ///// <summary>
    /////     updates a single properties access level
    ///// </summary>
    ///// <param name="propertyId"></param>
    ///// <param name="propertyAccess"></param>
    ///// <returns></returns>
    Task UpdatePropertyAccess(int propertyId, PropertyAccessLevel propertyAccess);

    ///// <summary>
    /////    Creates all properties and updates if a new table is added to the list
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns></returns>
    Task CreatePropertyAccessEntries();

    //// <summary>
    //// Check if Employee exists by ID
    //// </summary>
    //// <param name="employeeId"></param>
    //// <returns></returns>
    Task<bool> CheckEmployee(int employeeId);

    //// <summary>
    //// Check if Role Exists by ID
    //// </summary>
    //// <param name="roleId"></param>
    //// <returns></returns>
    Task<bool> CheckRoleById(int roleId);

    //// <summary>
    //// Check if the Property Access exists by ID
    //// </summary>
    //// <param name="id"></param>
    //// <returns></returns>
    Task<bool> CheckPropertyAccess(int id);

}