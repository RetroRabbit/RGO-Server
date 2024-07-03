using System.Reflection;

namespace HRIS.Services.Extensions;

public static class PropertyInfoExtensions
{
    public static bool HaveAttribute<T>(this PropertyInfo? prop)
        where T : System.Attribute, new()
    {
        try
        {
            return prop.GetCustomAttributes(typeof(T), false) is T[] attributes && attributes.Any() && attributes.FirstOrDefault() != null;
        }
        catch
        {
            return false;
        }
    }

    public static T GetAttribute<T>(this PropertyInfo? prop)
        where T : System.Attribute, new()
    {
        try
        {
            return prop.GetCustomAttributes(typeof(T),
                false) is T[] attributes && attributes.Any()
                ? attributes.First()
                : new T();
        }
        catch
        {
            return new T();
        }
    }
}