namespace RGO.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> CheckUserExist(string email);
        string GenerateToken();
    }
}
