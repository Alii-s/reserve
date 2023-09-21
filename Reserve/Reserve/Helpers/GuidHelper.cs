namespace Reserve.Helpers;

using System;

public class GuidShortener
{
    public static string ShortenGuid(Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();
        return Convert.ToBase64String(guidBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Substring(0, 22);
    }

    public static Guid RestoreGuid(string shortenedGuid)
    {
        while (shortenedGuid.Length < 22)
        {
            shortenedGuid += "=";
        }

        shortenedGuid = shortenedGuid.Replace("-", "+").Replace("_", "/");
        byte[] guidBytes = Convert.FromBase64String(shortenedGuid + "==");
        return new Guid(guidBytes);
    }
}
