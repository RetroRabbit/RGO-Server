namespace RGO.Repository.Entities;

public class events
{
    public int id { get; set; }
    public UserGroup groupid { get; set; } = null!;
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public int userType { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public int eventType { get; set; }
}
