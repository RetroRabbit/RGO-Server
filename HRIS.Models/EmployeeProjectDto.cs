namespace HRIS.Models;

public class EmployeeProjectDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeProjectDto(int Id,
                              int EmployeeId,
                              string Name,
                              string Description,
                              string Client,
                              DateTime StartDate,
                              DateTime? EndDate)
    {
        this.Id = Id;
        this.EmployeeId = EmployeeId;
        this.Name = Name;
        this.Description = Description;
        this.Client = Client;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
    }

    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Client { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}