namespace HRIS.Models.Update;

public class UpdateFieldValueDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public UpdateFieldValueDto(int fieldId, object value)
    {
        this.fieldId = fieldId;
        this.value = value;
    }

    public int fieldId { get; set; }
    public object value { get; set; }
}