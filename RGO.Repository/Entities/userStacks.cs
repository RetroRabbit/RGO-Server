namespace RGO.Repository.Entities;

public class UserStacks
{
    public int id { get; set; }
    public users userId { get; set; } = null!;
    public stacks backendId { get; set; } = null!;
    public stacks frontendId { get; set; } = null!;
    public stacks databaseId { get; set; } = null!;
    public DateTime createDate { get; set; }
}
