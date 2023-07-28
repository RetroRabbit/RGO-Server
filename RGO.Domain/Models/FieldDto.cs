namespace RGO.Domain.Models
{
    public record FieldDto( int formid, int type, bool required, string label, string description, string errormessage);  
}
