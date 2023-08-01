using RGO.Domain.Models;


namespace RGO.Domain.Interfaces.Repository
{
    public interface ICertificationsRepository
    {
        Task<bool> HasCertifications(int userid);
        Task<List<CertificationsDto>> GetCertificationsByUserId(int userid);
    }
}
