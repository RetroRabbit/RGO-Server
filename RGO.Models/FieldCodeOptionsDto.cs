namespace RGO.Models;

public class FieldCodeOptionsDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public FieldCodeOptionsDto(int Id,
        int FieldCodeId,
        string Option)
    {
        this.Id = Id;
        this.FieldCodeId = FieldCodeId;
        this.Option = Option;
    }

    public int Id { get; set; }
    public int FieldCodeId { get; set; }
    public string Option { get; set; }
}