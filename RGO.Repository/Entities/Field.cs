using RGO.Domain.Models;

namespace RGO.Repository.Entities
{
    public class Field
    {
        public int id { get; set; }
        public int formId { get; set; }
        public int type { get; set; }
        public bool required { get; set; }
        public string label { get; set; } = null!;
        public string description { get; set; } = null!;
        public string errorMessage { get; set; } = null!;

        public Field() { }
        public Field(int id,FieldDto fieldDto) 
        {
            this.id = id;
            formId = fieldDto.formId;
            type = fieldDto.type;
            required = fieldDto.required;
            label = fieldDto.label;
            description = fieldDto.description;
            errorMessage = fieldDto.errorMessage;
        }


        public FieldDto ToDto()
        {
            return new FieldDto
            (
                formId,
                type,
                required,
                label,
                description,
                errorMessage
            );
        }
    }
}
