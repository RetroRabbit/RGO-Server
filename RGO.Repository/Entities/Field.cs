using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RGO.Repository.Entities;

[Table("Field")]
public class Field
{
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("formId")]
    public int FormId { get; set; }*/

    [Column("type")]
    public int Type { get; set; }

    [Column("required")]
    public bool Required { get; set; }

    [Column("label")]
    public string Label { get; set; } = null!;
    
    [Column("description")]
    public string Description { get; set; } = null!;
    
    [Column("errorMessage")]
    public string ErrorMessage { get; set; } = null!;

    [ForeignKey("formId")]
    public virtual Form FormFields { get; set; }

    public Field() { }
    public Field(int id,FieldDto fieldDto) 
    {
        this.Id = id;/*
        FormId = fieldDto.FormId;*/
        Type = fieldDto.Type;
        Required = fieldDto.Required;
        Label = fieldDto.Label;
        Description = fieldDto.Description;
        ErrorMessage = fieldDto.ErrorMessage;
    }


    public FieldDto ToDto()
    {
        return new FieldDto
        (
            FormFields.Id,
            Type,
            Required,
            Label,
            Description,
            ErrorMessage
        );
    }
}
