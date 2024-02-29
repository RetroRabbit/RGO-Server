using HRIS.Models.Enums;

namespace HRIS.Models;

public class FieldCodeDto
{
    public List<FieldCodeOptionsDto> Options { get; set; }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Regex { get; set; }
    public FieldCodeType Type { get; set; }
    public ItemStatus Status { get; set; }
    public bool Internal { get; set; }
    public string? InternalTable { get; set; }
    public FieldCodeCategory Category { get; set; }
    public bool Required { get; set; }
}
