namespace RGO.Domain.Models
{
    public record FieldDto( 
        int formId, 
        int type, 
        bool required, 
        string label, 
        string description, 
        string errorMessage
        );  
}
