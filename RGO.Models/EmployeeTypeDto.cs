namespace HRIS.Models;

public class EmployeeTypeDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeTypeDto(int Id,
                           string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}