using System.Windows;
using System.Windows.Controls;
using Memory.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Memory.Views
{
    public partial class NewUserView : UserControl
    {
        public NewUserView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // string username = UsernameTextBox.Text;
            // string password = PasswordBox.Password;

            // if (username == "admin" && password == "password")
            // {
            //     MessageBox.Show("Zalogowano!");
            //     var startView = App.ServiceProvider.GetRequiredService<StartView>();
            //     ((MainViewModel)DataContext).CurrentView = startView;
            // }
            // else
            // {
            //     MessageBox.Show("Błędne dane logowania!");
            // }
        }
        // private void LoginButton_Click(object sender, RoutedEventArgs e)
        // {
        //     string username = UsernameTextBox.Text;
        //     string password = PasswordBox.Password;

        //     // Sprawdzenie, czy użytkownik istnieje w bazie danych
        //     var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

        //     if (user != null)
        //     {
        //         // Haszowanie wprowadzonego hasła i porównanie z zapisanym hasłem
        //         var passwordHasher = new PasswordHasher<User>();
        //         var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        //         if (result == PasswordVerificationResult.Success)
        //         {
        //             MessageBox.Show("Zalogowano!");
        //             var startView = App.ServiceProvider.GetRequiredService<StartView>();
        //             ((MainViewModel)DataContext).CurrentView = startView;
        //         }
        //         else
        //         {
        //             MessageBox.Show("Błędne dane logowania!");
        //         }
        //     }
        //     else
        //     {
        //         MessageBox.Show("Błędne dane logowania!");
        //     }
        // }
        private void NewAccountButton_Click(object sender, RoutedEventArgs e)
        {
        //     string username = UsernameTextBox.Text;
        //     string password = PasswordBox.Password;

        //     // Sprawdzamy, czy użytkownik już istnieje
        //     var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        //     if (existingUser != null)
        //     {
        //         MessageBox.Show("Użytkownik już istnieje!");
        //         return;
        //     }

        //     // Haszowanie hasła
        //     var passwordHasher = new PasswordHasher<User>();
        //     string hashedPassword = passwordHasher.HashPassword(null, password);

        //     // Tworzenie nowego użytkownika
        //     var newUser = new User
        //     {
        //         Username = username,
        //         PasswordHash = hashedPassword
        //     };

        //     // Zapisz nowego użytkownika w bazie danych
        //     _dbContext.Users.Add(newUser);
        //     _dbContext.SaveChanges();

        //     MessageBox.Show("Nowe konto zostało utworzone!");
        }

        // private void OpenPopup()
        // {
        //     PopupNewAccount.IsOpen = true; // Zamknij popup
        // }

        // private void ClosePopup()
        // {
        //     PopupNewAccount.IsOpen = false; // Zamknij popup
        // }
    }

}
