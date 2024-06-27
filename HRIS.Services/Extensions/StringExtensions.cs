using System.Text;

namespace HRIS.Services.Extensions;

public static class StringExtensions
{
    public static string StripToKeyWord(this string str)
    {
        var strippedString = new StringBuilder(string.Empty);
        const string strangeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        foreach (var character in str.Where(character => strangeCharacters.IndexOf(character) > 0))
            strippedString.Append(character);
        return strippedString.ToString().Trim();
    }

    public static string TrimOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : str.Trim();
    }

    public static string ToDisplay(this string? str)
    {
        str = str.TrimOrEmpty();
        var newString = string.Empty;
        foreach (var s in str)
            if (char.IsUpper(s))
                newString += " " + s;
            else
                newString += string.Empty + s;
        return newString.TrimOrEmpty().Replace("_", string.Empty);
    }
}