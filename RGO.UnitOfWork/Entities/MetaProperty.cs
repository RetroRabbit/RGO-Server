using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RGO.Models;

namespace RGO.UnitOfWork.Entities;

public class MetaProperty : IModel<MetaPropertyDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("table")]
    public string Table { get; set; }

    public MetaProperty() {}

    public MetaProperty(MetaPropertyDto metaProperty)
    {
        Id = metaProperty.Id;
        Table = metaProperty.Table;
    }

    public MetaPropertyDto ToDto()
    {
        return new MetaPropertyDto(Id, Table);

    }
}
