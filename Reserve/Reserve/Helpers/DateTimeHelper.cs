using System.Globalization;

namespace Reserve.Helpers;

public static class DateTimeHelper
{
    public static string ConvertFrom24To12(string time)
    {
        try
        {
            var temp = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
            DateTime.ParseExact(temp.ToString("hh:mm tt"), "hh:mm tt", CultureInfo.InvariantCulture);
            return temp.ToString("hh:mm tt");
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{time}'");
            return string.Empty;
        }
    }
    public static DateTime DateTimeBuilder(string date, string time)
    {
        try
        {
            string parsedTime = ConvertFrom24To12(time);
            string dateTime = date + " " + parsedTime;
            return DateTime.ParseExact(dateTime, "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{date}' and '{time}'");
            return DateTime.MinValue;
        }
    }
}
