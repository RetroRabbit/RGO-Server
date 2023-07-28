namespace RGO.Repository.Entities
{
    public class Form
    {
        public int id {  get; set; }
        public int groupid { get; set; }
        public string title { get; set; } = null!;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

}
