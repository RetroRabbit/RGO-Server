using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork.Interfaces;

namespace RR.Tests.Data;

public static class EntityListExtension
{
    public static List<T> EntityToList<T>(this T value, params T[] extraValues)
        where T : class, IModel
    {
        var list = new List<T>() { value };
        if (extraValues.Length > 0)
            list.AddRange(extraValues);
        return list;
    }
    
    public static Mock<DbSet<T>> ToMockDbSet<T>(this T value, params T[] extraValues)
        where T : class, IModel
    {
        return EntityToList(value, extraValues).AsQueryable().BuildMockDbSet();
    }
    
    public static IQueryable<T> ToMockIQueryable<T>(this T value, params T[] extraValues)
        where T : class, IModel
    {
        return EntityToList(value, extraValues).ToMockIQueryable();
    }
    
    public static IQueryable<T> ToMockIQueryable<T>(this List<T> value)
        where T : class, IModel
    {
        return value.AsQueryable().BuildMock();
    }
    
    public static IQueryable<T> ToMockIQueryable<T>(this IEnumerable<T> value)
        where T : class, IModel
    {
        return value.AsQueryable().BuildMock();
    }
}