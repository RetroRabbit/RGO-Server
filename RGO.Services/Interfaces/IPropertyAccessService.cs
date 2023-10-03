using RGO.Models;
using RGO.Models.Update;

namespace RGO.Services.Interfaces
{
    public interface IPropertyAccessService
    {
        /// <summary>
        /// Get's all the Property that a user has access to
        /// </summary>
        /// <param name="email"></param>
        /// <returns>dictionary of properties with the access level for selected user</returns>
        Task<List<EmployeeAccessDto>> GetPropertiesWithAccess(string email);

        /// <summary>
        /// Updates fields that employee has access to
        /// </summary>
        /// <param name="Fields"></param>
        /// <returns></returns>
        Task UpdatePropertiesWithAccess(List<UpdateFieldValueDto> field, string email);
    }
}
