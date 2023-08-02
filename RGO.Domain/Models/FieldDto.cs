namespace RGO.Domain.Models;

public record FieldDto( 
    int FormId, 
    int Type, 
    bool Required, 
    string Label, 
    string Description, 
    string ErrorMessage
    );  
