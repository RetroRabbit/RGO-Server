namespace HRIS.Models;
    public class ClientProjectsDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ProjectURL { get; set; }
    }
