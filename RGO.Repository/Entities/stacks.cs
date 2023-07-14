namespace RGO.Repository.Entities;

public class stacks
{
    public int id { get; set; }
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public string url { get; set; } = null!;
    public int stackType { get; set; }
}