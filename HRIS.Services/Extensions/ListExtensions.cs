namespace HRIS.Services.Extensions;

public static class ListExtensions
{
    public static T GetRandom<T>(this List<T> source)
    {
        return source.OrderBy(i => Guid.NewGuid()).FirstOrDefault()!;
    }
}