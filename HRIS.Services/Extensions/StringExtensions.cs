﻿using System.Text;

namespace HRIS.Services.Extensions;

public static class StringExtensions
{
    public static string StripToKeyWord(this string str)
    {
        var strippedString = new StringBuilder("");
        const string strangeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        foreach (var character in str.Where(character => strangeCharacters.IndexOf(character) > 0))
            strippedString.Append(character);
        return strippedString.ToString().Trim();
    }
}