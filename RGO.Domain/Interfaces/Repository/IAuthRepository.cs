namespace RGO.Domain.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<bool> FindUserByEmail(string email);
    }
}
