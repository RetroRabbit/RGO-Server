using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.HRIS
{
    public class ClientProject : IModel<ClientProjectsDto>
    {
        public ClientProject()
        {
        }

        public ClientProject(ClientProjectsDto dto)
        {
            Id = dto.Id;
            NameOfClient = dto.NameOfClient;
            ProjectName = dto.ProjectName;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            UploadProjectUrl = dto.UploadProjectUrl;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nameOfClient")]
        public string? NameOfClient { get; set; }

        [Column("projectName")]
        public string? ProjectName { get; set; }

        [Column("startDate")]
        public DateTime StartDate { get; set; }

        [Column("endDate")]
        public DateTime EndDate { get; set; }

        [Column("uploadProjectUrl")]
        public string? UploadProjectUrl { get; set; }

        [Column("employeeId")]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public ClientProjectsDto ToDto()
        {
            return new ClientProjectsDto
            {
                Id = Id,
                NameOfClient = NameOfClient,
                ProjectName = ProjectName,
                StartDate = StartDate,
                EndDate = EndDate,
                UploadProjectUrl = UploadProjectUrl
            };
        }
    }
}
