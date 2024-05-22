using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS
{
    [Table("ClientProject")]
    public class ClientProject : IModel<ClientProjectsDto>
    {
        public ClientProject()
        {
        }

        public ClientProject(ClientProjectsDto dto)
        {
            Id = dto.Id;
            EmployeeId = dto.EmployeeId;
            ClientName = dto.ClientName;
            ProjectName = dto.ProjectName;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            ProjectURL = dto.ProjectURL;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("clientName")]
        public string ClientName { get; set; }

        [Column("projectName")]
        public string ProjectName { get; set; }

        [Column("startDate")]
        public DateTime StartDate { get; set; }

        [Column("endDate")]
        public DateTime EndDate { get; set; }

        [Column("projectURL")]
        public string ProjectURL { get; set; }

        [Column("employeeId")]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public ClientProjectsDto ToDto()
        {
            return new ClientProjectsDto
            {
                Id = Id,
                EmployeeId = EmployeeId,
                ClientName = ClientName,
                ProjectName = ProjectName,
                StartDate = StartDate,
                EndDate = EndDate,
                ProjectURL = ProjectURL
            };
        }
    }
}
