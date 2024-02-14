namespace RGO.Models;

public class EmployeeAccessDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeAccessDto(int Id,
        int Condition,              //view, edit, Hidden
        bool Internal,              //Custom or internal
        string PropName,            //unique identifier
        string Label,               //Just for display
        string Type,                //text, bool, int
        string Value,               //value of the selected value
        string? Description,        //additional front-end info
        string? Regex,              //additional validation for front-end
        List<string>? Options)
    {
        this.Id = Id;
        this.Condition = Condition;
        this.Internal = Internal;
        this.PropName = PropName;
        this.Label = Label;
        this.Type = Type;
        this.Value = Value;
        this.Description = Description;
        this.Regex = Regex;
        this.Options = Options;
    }

    public int Id { get; set; }
    public int Condition { get; set; }
    public bool Internal { get; set; }
    public string PropName { get; set; }
    public string Label { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public string? Regex { get; set; }
    public List<string>? Options { get; set; }
}