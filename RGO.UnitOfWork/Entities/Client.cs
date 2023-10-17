using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork.Entities
{
    [Table("Client")]
    public class Client : IModel<ClientDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public Client() { }

        public Client(ClientDto clientDto)
        {
            Id = clientDto.Id;
            Name = clientDto.Name;
        }
        public ClientDto ToDto()
        {
            return new ClientDto(
            Id,
            Name
            );
        }
    }

}
