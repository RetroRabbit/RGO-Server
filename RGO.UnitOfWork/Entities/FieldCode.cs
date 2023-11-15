﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGO.Models.Enums;
using RGO.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities
{
    [Table("FieldCode")]
    public class FieldCode : IModel<FieldCodeDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("regex")]
        public string? Regex { get; set; }

        [Column("type")]
        public FieldCodeType Type { get; set; }

        [Column("status")]
        public ItemStatus Status { get; set; }

        [Column("internal")]
        public bool Internal { get; set; }

        [Column("internalTable")]
        public string? InternalTable { get; set; }


        [Column("category")]
        public FieldCodeCategory Category { get; set; }
        public FieldCode()
        {
            Internal = false;
        }

        public FieldCode(FieldCodeDto fieldCodeDto) {
            Id = fieldCodeDto.Id;
            Code = fieldCodeDto.Code;
            Name = fieldCodeDto.Name;
            Description = fieldCodeDto.Description;
            Regex = fieldCodeDto.Regex;
            Type = fieldCodeDto.Type;
            Status = fieldCodeDto.Status;
            Internal = fieldCodeDto.Internal;
            InternalTable = fieldCodeDto.InternalTable;
            Category = fieldCodeDto.Category;
        }

        public FieldCodeDto ToDto()
        {
            return new FieldCodeDto(
                Id, 
                Code,
                Name,
                Description,
                Regex,
                Type,
                Status,
                Internal,
                InternalTable,
                Category);
        }
    }
}
