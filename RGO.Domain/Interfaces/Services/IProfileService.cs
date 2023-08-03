using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IProfileService
{
    Task<UserDto> GetUserByEmail(string email);
}
