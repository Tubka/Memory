namespace Memory.Services;
public class CheckboxesServices
{
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
}