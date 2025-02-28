using System.Security.Cryptography;
using System.Text;
using Memory.Database;

public class AuthService
{
    public static void RegisterUser(string username, string password)
    {
        using (var db = new AppDbContext())
        {
            if (db.Users.Any(u => u.Name == username))
            {
                throw new Exception("Użytkownik już istnieje!");
            }

            string passwordHash = HashPassword(password);
            db.Users.Add(new User { Name = username, PasswordHash = passwordHash });
            db.SaveChanges();
        }
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
    public static bool LoginUser(string username, string password)
    {
        using (var db = new AppDbContext())
        {
            string passwordHash = HashPassword(password);
            return db.Users.Any(u => u.Name == username && u.PasswordHash == passwordHash);
        }
    }
}
