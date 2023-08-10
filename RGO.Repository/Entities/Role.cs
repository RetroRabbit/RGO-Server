using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities
{
    public class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }
    }
}
