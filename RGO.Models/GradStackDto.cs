using RGO.Models.Enums;

namespace RGO.Models
{
    public record GradStackDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public StacksDto Backend { get; set; }
        public StacksDto Frontend { get; set; }
        public StacksDto Database { get; set; }
        public string Description { get; set; }
        public GradProjectStatus Status { get; set; }
        public DateTime CreateDate { get; set; }

        public GradStackDto(
            int Id,
            int UserId,
            StacksDto Backend,
            StacksDto Frontend,
            StacksDto Database,
            string Description,
            GradProjectStatus Status,
            DateTime CreateDate)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.Backend = Backend;
            this.Frontend = Frontend;
            this.Database = Database;
            this.Description = Description;
            this.Status = Status;
            this.CreateDate = CreateDate;
        }
    }
}
