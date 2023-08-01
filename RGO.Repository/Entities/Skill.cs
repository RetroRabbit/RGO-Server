using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public class Skill
    {
        public int id { get; set; }
        public User user { get; set; }
        public int userid { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public Skill()
        {

        }

        public Skill(SkillDto skill)
        {
            id = skill.id;
            userid = skill.userid;
            title = skill.title;
            description = skill.description;
        }

        public SkillDto ToDTO()
        {
            return new SkillDto 
            (
                id,
                userid,
                title,
                description
            );
        }


    }
}
