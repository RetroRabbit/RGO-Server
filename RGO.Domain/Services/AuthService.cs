using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;

namespace RGO.Domain.Services
{
    public class AuthService : IAuthService
    {
        public IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepo)
        {
            _authRepository = authRepo;
        }
        public bool CheckUserExist(UserDto user)
        {
            return _authRepository.FindUserByEmail(user.email);
        }

        public string GenerateToken()
        {
            return "token";
        }
    }
}
