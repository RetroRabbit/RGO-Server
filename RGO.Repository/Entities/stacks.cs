namespace RGO.Repository.Entities;

public record Stacks(
    int id,
    string title,
    string description,
    string url,
    int stackType
    );