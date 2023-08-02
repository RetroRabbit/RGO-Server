using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> CheckUserExist(string email)
        {
            return await _userRepository.UserExists(email);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            UserDto user = await _userRepository.GetUserByEmail(email);

            return user;
        }
    }
}
