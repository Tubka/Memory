using System.IO;

public class Logger
{
    private static string logFilePath = "app.log";  // Ścieżka do pliku logu

    public static void LogError(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisu do pliku logu: {ex.Message}");
        }
    }
}

public class Program
{
    public static void Main2(string[] args)
    {
        try
        {
            // Symulacja błędu
            throw new Exception("Przykładowy błąd!");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Wystąpił błąd: {ex.Message}");
        }
    }
}
