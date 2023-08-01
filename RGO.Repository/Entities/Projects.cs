using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public class Projects
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public string role { get; set; } = null!;

        public Projects()
        {
            
        }

        public Projects(ProjectsDto projects)
        {
            id = projects.id;
            name = projects.name;
            description = projects.description;
            role = projects.role;
        }

        public ProjectsDto ToDTO()
        {
            return new ProjectsDto(
                id,
                name,
                description,
                role
                );
        }
    }
}
