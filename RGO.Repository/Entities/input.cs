using System.Security.Cryptography;

namespace RGO.Repository.Entities;

public class input
{
    public int id { get; set; }
    public users userId { get; set; } = null!;
    public formSubmits formSubmitId { get; set; } = null!;
    public fields fieldId { get; set; } = null!;
    public string value { get; set; } = null!;
    public DateTime createDate { get; set; }
}
