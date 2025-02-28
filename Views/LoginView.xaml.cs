using System.Windows;
using System.Windows.Controls;
using Memory.Services;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Memory.ViewModels;
using System.Security;
using Memory.Messages;

namespace Memory.Views
{
    public partial class LoginView : UserControl
    {
        private CompanyViewModel _companyViewModel;
        private readonly MainViewModel _mainViewModel;
        public LoginView()
        {
            InitializeComponent();
            _mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
            _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
            DataContext = _mainViewModel;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // var startView = App.ServiceProvider.GetRequiredService<StartView>();
            // ((MainViewModel)DataContext).CurrentView = startView; // Zmiana widoku po zalogowaniu

            string username = UsernameTextBox.Text;
            SecureString password = PasswordBox.SecurePassword; // Pobieramy SecureString

            int? userId = UserService.Login(username, password); // Wywołujemy funkcję logowania

            if (userId.HasValue) // Jeśli logowanie powiodło się
            {
                var user = new User { Id = userId ?? 0, Name = username };

                if (_mainViewModel == null)
                {
                    // MessageBox.Show("MainViewModel is NULL!");
                    return;
                }

                // var mainVM = App.ServiceProvider.GetRequiredService<MainViewModel>();
                WeakReferenceMessenger.Default.Send(new UserChangedMessage(user));
MessageBox.Show($"DataContext to: {DataContext?.GetType().FullName}");
                var loginView = App.ServiceProvider.GetRequiredService<StartViewModel>();
                ((MainViewModel)DataContext).CurrentView = loginView;
                // _mainViewModel.CurrentView = App.ServiceProvider.GetRequiredService<StartViewModel>();
            }
            else
            {
                MessageBox.Show("Błędne dane logowania!");
            }
        }
        private void ShowNewUserWindow(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new NewUserWindow();
            // {
            //     Owner = Application.Current.MainWindow, // Powiązanie z głównym oknem
            //     Topmost = true // Zawsze na wierzchu
            // };
            newUserWindow.Owner = Application.Current.MainWindow; // Powiązanie z głównym oknem
            newUserWindow.ShowDialog(); // Otwórz jako modalne okno (blokuje interakcje z głównym oknem)
        }
        private void OpenNewUserPopup_Click(object sender, RoutedEventArgs e)
        {
            PopupNewUser.IsOpen = true; // Zamknij popup
            // MessageBox.Show("jest nowy user");
        }

        private void CloseNewUserPopup_Click(object sender, RoutedEventArgs e)
        {
            PopupNewUser.IsOpen = false; // Zamknij popup
        }
    }
}
