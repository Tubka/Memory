using System.Windows;
using Memory.Database;
using Microsoft.AspNetCore.Identity;
using System.Security;
using System.Runtime.InteropServices;

namespace Memory.Services;

public partial class UserService
{
    public static int? Login(string username, SecureString password)
    {

        using (var context = new AppDbContext())
        {
            // Pobranie użytkownika po nazwie użytkownika
            var user = context.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            // Konwersja SecureString na string
            string plainPassword = ConvertSecureStringToString(password);

            // Weryfikacja hasła
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, plainPassword);

            if (result == PasswordVerificationResult.Success)
            {
                return user.Id;
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login lub hasło.");
                return null;
            }
        }
    }
    // Lista danych do wyświetlenia w ComboBox
    public static void New(string name, SecureString password)
    {
        try
        {
            using (var context = new AppDbContext()) // Użycie EF DbContext
            {
                // Sprawdzenie, czy użytkownik już istnieje
                var existingUser = context.Users.FirstOrDefault(u => u.Name == name);

                if (existingUser != null)
                {
                    MessageBox.Show("Użytkownik o tej nazwie już istnieje!");
                    return;
                }

                // Konwersja SecureString na string
                string plainPassword = ConvertSecureStringToString(password);

                // Haszowanie hasła
                var passwordHasher = new PasswordHasher<User>();
                string hashedPassword = passwordHasher.HashPassword(null, plainPassword);

                // Tworzenie nowego użytkownika
                var newUser = new User
                {
                    Name = name,
                    PasswordHash = hashedPassword
                };

                // Zapis do bazy danych
                context.Users.Add(newUser);
                context.SaveChanges();

                // Czyścimy hasło z pamięci
                plainPassword = null;


                MessageBox.Show("Nowe konto zostało utworzone!");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas tworzenia użytkownika:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Konwertuje SecureString na zwykły string (tylko tymczasowo!)
    private static string ConvertSecureStringToString(SecureString secureString)
    {
        IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
        try
        {
            return Marshal.PtrToStringBSTR(ptr);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(ptr); // Czyści pamięć po użyciu
        }
    }

    // public static List<MonthlyCheckbox> GetMonthlyCheckboxes(int companyId, DateTime date)
    // {
    //     using (var context = new AppDbContext())
    //     {
    //         var monthlyCheckboxes = context.MonthlyCheckboxes
    //             .Include(m => m.CheckboxTemplate) // Zawiera powiązany Template
    //             .Where(m => m.CompanyId == companyId && m.Date.Month == date.Month && m.Date.Year == date.Year)
    //             .ToList();


    //         return monthlyCheckboxes;
    //     }
    // }
    // public static void Login(string login, PasswordBox password)
    // {
    //     MessageBox.Show($"{login}");
    //     using (var context = new AppDbContext())
    //     {
    //         // Pobranie najnowszego wpisu dla danej firmy
    //         var latestDescription = context.Descriptions
    //             .Where(d => d.CompanyId == companyId)
    //             .OrderByDescending(d => d.Date) // Sortowanie malejące po dacie
    //             .FirstOrDefault();

    //         if (latestDescription == null)
    //         {
    //             throw new Exception($"Brak opisu dla firmy ID: {companyId}");
    //         }

    //         return latestDescription;
    //     }
    // }
}

