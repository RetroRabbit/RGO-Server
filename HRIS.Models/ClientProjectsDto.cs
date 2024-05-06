namespace HRIS.Models
{
    public class ClientProjectsDto
    {
        public int Id { get; set; }
        public string NameOfClient { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UploadProjectUrl { get; set; }
    }
}
