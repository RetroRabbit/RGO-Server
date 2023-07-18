using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;

namespace RGO.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepo)
        {
            _authRepository = authRepo;
        }
        public async Task<bool> CheckUserExist(string email)
        {
            return await _authRepository.FindUserByEmail(email);
        }

        public string GenerateToken()
        {
            return "token";
        }
    }
}
