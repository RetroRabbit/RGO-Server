using HRIS.Models.Enums;

namespace HRIS.Models;

public class FieldCodeDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public FieldCodeDto(int Id,
                        string? Code,
                        string? Name,
                        string? Description,
                        string? Regex,
                        FieldCodeType Type,
                        ItemStatus Status,
                        bool Internal,
                        string? InternalTable,
                        FieldCodeCategory Category,
                        bool Required)
    {
        this.Id = Id;
        this.Code = Code;
        this.Name = Name;
        this.Description = Description;
        this.Regex = Regex;
        this.Type = Type;
        this.Status = Status;
        this.Internal = Internal;
        this.InternalTable = InternalTable;
        this.Category = Category;
        this.Required = Required;
    }

    public List<FieldCodeOptionsDto>? Options { get; set; }
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Regex { get; set; }
    public FieldCodeType Type { get; set; }
    public ItemStatus Status { get; set; }
    public bool Internal { get; set; }
    public string? InternalTable { get; set; }
    public FieldCodeCategory Category { get; set; }
    public bool Required { get; set; }
}