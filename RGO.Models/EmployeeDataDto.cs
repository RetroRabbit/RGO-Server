namespace HRIS.Models;

public class EmployeeDataDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeDataDto(int Id,
                           int EmployeeId,
                           int FieldCodeId,
                           string Value)
    {
        this.Id = Id;
        this.EmployeeId = EmployeeId;
        this.FieldCodeId = FieldCodeId;
        this.Value = Value;
    }

    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int FieldCodeId { get; set; }
    public string Value { get; set; }
}