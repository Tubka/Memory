using System;
using System.Globalization;

namespace Memory.Services;
public class Date
{

    // Funkcja do logowania błędów do pliku
    public static string Formatter(string date) 
    {
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            return parsedDate.ToString("yyyy-MM-dd");
        }
        else
        {
            throw new ArgumentException("Invalid date format.");
        }
    }

    public static DateTime Utc(string date) 
    {
        DateTime utcDate = ConvertToUtc(date);
        return utcDate;
    }

    public static DateTime Now()
    {
        return Utc(DateTime.UtcNow.ToString("yyyy-MM-dd"));
    }

    public static string Month(string date)
    {
        return DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    public static string Year(string date)
    {
        return DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    public static DateTime ConvertToUtc(string date)
    {
        DateTime parsedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
    }

    public static bool CompareDate(DateTime first, DateTime second)
    {
        return first.Year == second.Year && first.Month == second.Month;
    }
}