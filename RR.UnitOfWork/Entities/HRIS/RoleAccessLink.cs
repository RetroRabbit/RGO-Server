﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("RoleAccessLink")]
public class RoleAccessLink : IModel<RoleAccessLinkDto>
{
    public RoleAccessLink()
    {
    }

    public RoleAccessLink(RoleAccessLinkDto roleAccessLinkDto)
    {
        Id = roleAccessLinkDto.Id;
        RoleId = roleAccessLinkDto.Role!.Id;
        RoleAccessId = roleAccessLinkDto.RoleAccess!.Id;
    }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("roleAccessId")]
    [ForeignKey("RoleAccess")]
    public int RoleAccessId { get; set; }

    public virtual Role? Role { get; set; }
    public virtual RoleAccess? RoleAccess { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public RoleAccessLinkDto ToDto()
    {
        return new RoleAccessLinkDto
        {
            Id = Id,
            Role = Role?.ToDto(),
            RoleAccess = RoleAccess?.ToDto()
        };
    }
}