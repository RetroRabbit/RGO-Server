

namespace HRIS.Models
{
    public class SimpleEmployeeData
    {
        public int employeeId { get; set; }
        public int? Level { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Client { get; set; }
        public DateTime? EngagementDate { get; set; }
        public string? Position { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string? InactiveReason { get; set; }
        public string? Roles { get; set; }


    }
}
