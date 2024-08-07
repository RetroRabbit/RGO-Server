﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Client")]
public class Client : IModel
{
    public Client()
    {
    }

    public Client(ClientDto clientDto)
    {
        Id = clientDto.Id;
        Name = clientDto.Name;
    }

    [Column("name")] public string? Name { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public ClientDto ToDto()
    {
        return new ClientDto
        {
            Id = Id,
            Name = Name
        };
    }
}
