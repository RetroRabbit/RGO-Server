namespace HRIS.Models;

public class ClientDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public ClientDto(int Id,
                     string? Name)
    {
        this.Id = Id;
        this.Name = Name;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
}