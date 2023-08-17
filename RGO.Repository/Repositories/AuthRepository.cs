using RGO.Models;
using RGO.Repository.Interfaces;

namespace RGO.Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;

        public AuthRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> FindUserByEmail(string email)
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
