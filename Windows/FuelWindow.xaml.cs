using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Memory.Services;
using Memory.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Memory.Views
{
    public partial class FuelWindow : Window
    {
        private string originalTextDescription; // Przechowuje tekst przed edycją
        private bool isEditing = false;

        private HeaderViewModel _headerViewModel;
        private CompanyViewModel _companyViewModel;

        public FuelWindow()
        {
            InitializeComponent(); // Inicjalizacja komponentów okna
            _headerViewModel = App.ServiceProvider.GetRequiredService<HeaderViewModel>();
            _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
            // originalText = GetFuelDescription();
            GetFuelDescription();
        }

        public void GetFuelDescription()
        {
            var textDescription = DatabaseServices.GetFuelDescription(_companyViewModel.Company.Id, _companyViewModel.Date);
            originalTextDescription = textDescription?.Description?.ToString() ?? "";
            TextInput.Text = originalTextDescription;           
        }
        public void PostFuelDescription()
        {
            var companyId = _companyViewModel.Company.Id;
            var userId = _companyViewModel.User.Id;
            var date = _companyViewModel.Date;
            var description = TextInput.Text;

            // MessageBox.Show($"{companyId}-{userId}-{description}-{date}");
            DatabaseServices.AddFuelDescription(companyId, userId, description, date);
            // MessageBox.Show("Zapisano!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public SecureString GetSecurePassword(PasswordBox passwordBox)
        {
            return passwordBox.SecurePassword;
        }
        // Obsługuje kliknięcie przycisku "Utwórz"
        private string ConvertSecureStringToString(SecureString secureString)
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

    // Obsługuje kliknięcie przycisku "Utwórz"
        private void CloseNewUserPopup_Click(object sender, RoutedEventArgs e)
        {
            // string username = UsernameTextBox.Text;
            // SecureString password = PasswordBox.SecurePassword;
            // SecureString repeatPassword = RepeatPasswordBox.SecurePassword;

            // if (!string.IsNullOrWhiteSpace(username) && password.Length > 0)
            // {
            //     if (AreSecureStringsEqual(password, repeatPassword))
            //     {
            //         UserService.New(username, password);
            //         MessageBox.Show("Nowe konto zostało utworzone!");
            //         this.Close();
            //     }
            //     else
            //     {
            //         MessageBox.Show("Hasła nie są identyczne!");
            //     }
            // }
            // else
            // {
            //     MessageBox.Show("Wszystkie pola muszą być wypełnione!");
            // }
        }
        private bool AreSecureStringsEqual(SecureString s1, SecureString s2)
        {
            if (s1.Length != s2.Length) return false;

            IntPtr ptr1 = Marshal.SecureStringToBSTR(s1);
            IntPtr ptr2 = Marshal.SecureStringToBSTR(s2);

            try
            {
                string str1 = Marshal.PtrToStringBSTR(ptr1);
                string str2 = Marshal.PtrToStringBSTR(ptr2);
                return str1 == str2;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr1);
                Marshal.ZeroFreeBSTR(ptr2);
            }
        }

        private void SaveText(object sender, RoutedEventArgs e)
        {
            // File.WriteAllText("tekst.txt", TextInput.Text);
            // var companyId = _companyViewModel.Company.Id;
            // var userId = _companyViewModel.Company.Id;
            // var date = _companyViewModel.Date;
            // DatabaseServices.AddFuelDescription(companyId, userId, description, date);
            // MessageBox.Show("Zapisano!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ToggleEditMode(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                // Rozpoczęcie edycji
                isEditing = true;
                // originalText = TextInput.Text; // Zapisujemy oryginalny tekst
                TextInput.IsReadOnly = false;
                EditButton.Content = "Zapisz";
                CancelButton.Visibility = Visibility.Visible;
            }
            else
            {
                // Zapis i zakończenie edycji
                try
                {
                    PostFuelDescription();
                    isEditing = false;
                    TextInput.IsReadOnly = true;
                    EditButton.Content = "Edytuj";
                    CancelButton.Visibility = Visibility.Collapsed;
                } 
                catch
                {
                    MessageBox.Show("cos nie tak");
                }
            }
        }

        private void CancelEdit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz anulować zmiany?", 
                "Potwierdzenie", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            // Przywracamy oryginalny tekst i blokujemy edycję
            TextInput.Text = originalTextDescription;
            isEditing = false;
            TextInput.IsReadOnly = true;
            EditButton.Content = "Edytuj";
            CancelButton.Visibility = Visibility.Collapsed;
        }

    }
}
