using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;

namespace HRIS.Services.Interfaces;

public interface IPropertyAccessService
{

    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task<List<PropertyAccessDto>> GetAccessListByEmployeeId(int employeeId);
    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task<List<PropertyAccessDto>> GetAccessListByRoleId(int roleId);
    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task<List<PropertyAccessDto>> GetAll();
    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task UpdatePropertyAccess(int propertyId, PropertyAccessLevel propertyAccess);
    ///// <summary>
    /////     Get's all the Property that a user has access to
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns>dictionary of properties with the access level for selected user</returns>
    Task CreatePropertyAccessEntries();

}