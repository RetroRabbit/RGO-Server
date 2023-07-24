namespace RGO.Repository.Entities;

public class UserStacks
{
    public int id { get; set; }
    public int userId { get; set; } 
    public int backendId { get; set; }
    public int frontendId { get; set; } 
    public int databaseId { get; set; } 
    public DateTime createDate { get; set; }
}
