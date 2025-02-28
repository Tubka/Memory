using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Memory.Services;

namespace Memory.Views
{
    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent(); // Inicjalizacja komponentów okna
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
            string username = UsernameTextBox.Text;
            SecureString password = PasswordBox.SecurePassword;
            SecureString repeatPassword = RepeatPasswordBox.SecurePassword;

            if (!string.IsNullOrWhiteSpace(username) && password.Length > 0)
            {
                if (AreSecureStringsEqual(password, repeatPassword))
                {
                    UserService.New(username, password);
                    MessageBox.Show("Nowe konto zostało utworzone!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hasła nie są identyczne!");
                }
            }
            else
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione!");
            }
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

    }
}
