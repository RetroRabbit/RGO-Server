using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;
    public class ClientProjectsDto
    {
        [Required(ErrorMessage = "Client Project 'Id' field is missing.")]
        public int Id { get; set; }
        [Required (ErrorMessage = "Client Project 'EmployeeId' field is missing.")]
        public int EmployeeId { get; set; }
        [Required (ErrorMessage = "Client Project 'ClientName' field is missing.")]
        public string ClientName { get; set; }
        [Required (ErrorMessage = "Client Project 'ProjectName' field is missing.")]
        public string ProjectName { get; set; }
        [Required (ErrorMessage = "Client Project 'StartDate' field is missing.")]
        public DateTime StartDate { get; set; }
        [Required (ErrorMessage = "Client Project 'EndDate' field is missing.")]
        public DateTime EndDate { get; set; }
        public string? ProjectURL { get; set; }
    }
