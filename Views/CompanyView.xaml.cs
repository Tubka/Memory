using System.Windows;
using Memory.ViewModels;
using Memory.Messages;
using CommunityToolkit.Mvvm.Messaging;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Memory.Services;
using System.ServiceModel.Channels;

namespace Memory.Views
{
    public partial class CompanyView : UserControl
    {
        private bool isEditing = false;

        public static IServiceProvider ServiceProvider { get; private set; }
        private MainWindow _mainWindow;
        private HeaderViewModel _headerViewModel;
        private HeaderView _headerView;
        private CompanyViewModel _companyViewModel;
        public CompanyView()
        {
            _headerViewModel = App.ServiceProvider.GetRequiredService<HeaderViewModel>();
            _headerView = App.ServiceProvider.GetRequiredService<HeaderView>();
            _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
            var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
            _mainWindow = mainWindow;
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();

            // DataContext = ((MainWindow)System.Windows.Application.Current.MainWindow).DataContext;
            Dispatcher.InvokeAsync(() =>
            {
                if (DataContext == null)
                {
                    MessageBox.Show("jest null");
                }
                else
                {
                    // MessageBox.Show($"[CompanyView] DataContext: {DataContext.GetType().FullName} ✅");
                }
            });
            WeakReferenceMessenger.Default.Register<RefreshCompanyViewMessage>(this, (r, m) =>
            {
                RefreshView();
                _headerView.SetMonth("6");
            });

            RefreshView(); 
            LoadCheckboxes();

            InitializeComponent();
        }

        private void setTitle() 
        {
            var currentDate = _companyViewModel.Date;
            var currentDateMonth = _companyViewModel.Month;
            string month = new CultureInfo("pl-PL").DateTimeFormat.GetMonthName(currentDate.Month);
            string company = _companyViewModel.Company.Name;
            _mainWindow.Title = $"Miesiac: {month}-{currentDate.Year},    {company}, {currentDateMonth}";

        }
        
        private void RefreshView()
        {
            setTitle();
            LoadCheckboxes();
        }

        private void AddAllCheckboxToMonth()
        {
            var companyId = _mainWindow._selectedCompany.Id;
            var currentDate = _mainWindow._date;
        }


        private void ClosePopupInfo(object sender, RoutedEventArgs e)
        {
            // PopupInfo.IsOpen = false;
        }

        private void ShowPopupInfo(object sender, RoutedEventArgs e)
        {
            // PopupInfo.IsOpen = true;
        }
        private void AddNewCheckbox(object sender, RoutedEventArgs e)
        {
            string InputText = InputTextBox.Text;
            // var date = _mainWindow._date;
            var date = _companyViewModel.Date;
            var companyId = _companyViewModel.Company.Id;
            // Company company = DatabaseServices.AddCheckboxTemplate(CompanyId companyId, InputText);
            DatabaseServices.AddCheckboxTemplate(companyId, InputText, date);
            LoadCheckboxes();


            // var checkBoxControl = new CheckBox {
            //     Content = checkboxTemplate.TemplateName,
            //     IsChecked = false
            // };

            // CheckboxPanel.Children.Add(checkBoxControl);

            // if(company is not Company)
            // {
            //     MessageBox.Show($"Wystąpił błąd podczas dodawania firmy: {InputText}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            //     PopupAddCheckbox.IsOpen = false;
            //     return;
            // }
            // CompanySelectionService.AddItemsToComboBox(CompanyList);
            PopupAddCheckbox.IsOpen = false;
        }

        private void ShowPopupNewCheckbox(object sender, RoutedEventArgs e)
        {
            PopupAddCheckbox.IsOpen = true;
        }
        private void LoadCheckboxes()
        {
            try
            {
                CheckboxPanel.Children.Clear();
            }
            catch 
            {
                return;
            }

            // var companyId = _mainWindow._selectedCompany.Id; 
            var companyId = _companyViewModel.Company.Id;
            var date = _companyViewModel.Date;

            DatabaseServices.AddCheckboxesIfNotExist(companyId, date);

            var checkboxesFromDb = DatabaseServices.GetMonthlyCheckboxes(companyId, date);

            foreach (var checkbox in checkboxesFromDb)
            {
                var checkBoxControl = new CheckBox
                {
                    Content = checkbox.CheckboxTemplate.TemplateName, // Treść checkboxa
                    IsChecked = checkbox.IsChecked, // Ustawienie, czy checkbox jest zaznaczony
                    Tag = checkbox.Id // Przypisanie ID checkboxa z bazy danych do Tag
                };
                checkBoxControl.Checked += Checkbox_CheckedChanged;
                checkBoxControl.Unchecked += Checkbox_CheckedChanged;

                CheckboxPanel.Children.Add(checkBoxControl);
            }
        }

        private void Checkbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox clickedCheckbox)
            {
                var checkboxId = clickedCheckbox.Tag;
                bool isChecked = clickedCheckbox.IsChecked == true;
                UpdateCheckboxStateInDatabase(checkboxId, isChecked);
            }
        }

        // Funkcja aktualizująca stan checkboxa w bazie danych
        private void UpdateCheckboxStateInDatabase(object checkboxId, bool isChecked)
        {
            // Tutaj zaimplementuj logikę wysyłania zapytania do bazy danych
            // Możesz użyć swojego serwisu, np. DatabaseServices.UpdateCheckboxState(...)
            // DatabaseServices.UpdateCheckboxState(checkboxId, isChecked);

            // Dla debugowania, możesz dodać komunikat
            // MessageBox.Show($"Checkbox o ID {checkboxId} został {(isChecked ? "zaznaczony" : "odznaczony")}.");
        }

        // Przycisk Zatwierdź
        private void GoToStart(object sender, RoutedEventArgs e)
        {
            var StartView = App.ServiceProvider.GetRequiredService<StartViewModel>();
            ((MainViewModel)DataContext).CurrentView = StartView;
            // _mainWindow.SwitchView(new StartView(_mainWindow));
        }
        private void OpenInfo(object sender, RoutedEventArgs e)
        {
            LoadInfoFromFile();  // Załaduj dane do TextBoxa
            // PopupInfo.IsOpen = true; // Otwórz popup
        }

        private void CancelPopupAddCheckout(object sender, RoutedEventArgs e)
        {
            PopupAddCheckbox.IsOpen = false; // Otwórz popup
        }


        // Zamykanie popupu (po kliknięciu "Anuluj")
        // private void ClosePopupInfo(object sender, RoutedEventArgs e)
        // {
        //     PopupInfo.IsOpen = false; // Zamknij popup
        // }

        // Zapisywanie wprowadzonych informacji
        private void SaveInfo(object sender, RoutedEventArgs e)
        {
        //     string infoText = InputTextInfo.Text; // Pobierz tekst z TextBoxa
        //     File.WriteAllText(filePath, infoText); // Zapisz do pliku
        //     MessageBox.Show("Informacje zostały zapisane!", "Zapisano", MessageBoxButton.OK, MessageBoxImage.Information);
        //     PopupInfo.IsOpen = false; // Zamknij popup po zapisaniu
        }

        // Ładowanie zapisanych informacji z pliku do TextBoxa
        private void LoadInfoFromFile()
        {
        //     if (File.Exists(filePath))
        //     {
        //         string text = File.ReadAllText(filePath);
        //         InputTextInfo.Text = text; // Wstaw wczytany tekst do TextBoxa
        //     }
        //     else
        //     {
        //         InputTextInfo.Text = string.Empty; // Jeśli plik nie istnieje, wyczyść pole
        //     }
        }

        public void ClearCheckboxes()
        {
            try
            {
                CheckboxPanel.Children.Clear();
            }
            catch 
            {
                return;
            }
        }
    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (isEditing)
        {
            // Jeśli jesteśmy w trybie edycji, zapisujemy tekst
            // Możesz dodać logikę zapisu tutaj
            EditButton.Content = "Edytuj";
            isEditing = false;
            richTextBox.IsReadOnly = true;
            CancelButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            // Jeśli nie jesteśmy w trybie edycji, włączamy tryb edycji
            EditButton.Content = "Zapisz";
            isEditing = true;
            richTextBox.IsReadOnly = false;
            CancelButton.Visibility = Visibility.Visible;
            richTextBox.Focus(); // Skupiamy uwagę na RichTextBox, kursor zaczyna migać
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Anulowanie edycji
        EditButton.Content = "Edytuj";
        isEditing = false;
        richTextBox.IsReadOnly = true;
        CancelButton.Visibility = Visibility.Collapsed;
        richTextBox.Focus(); // Kursor wróci do RichTextBox
    }
// private void EditButton_Click(object sender, RoutedEventArgs e)
//     {
//         richTextBox.IsReadOnly = false;  // Odblokowujemy pole do edycji
//         EditButton.Content = "Zapisz";  // Zmieniamy tekst przycisku na "Zapisz"
//         CancelButton.Visibility = Visibility.Visible;  // Pokazujemy przycisk "Anuluj"
//     }

//     // Zapisujemy tekst i blokujemy edycję
//     private void SaveButton_Click(object sender, RoutedEventArgs e)
//     {
//         // Tutaj możesz dodać logikę zapisywania (np. do bazy danych, pliku itp.)
//         richTextBox.IsReadOnly = true;  // Blokujemy pole do edycji
//         EditButton.Content = "Edytuj";  // Zmieniamy tekst przycisku z powrotem na "Edytuj"
//         CancelButton.Visibility = Visibility.Collapsed;  // Ukrywamy przycisk "Anuluj"
//     }

//     // Przycisk Anuluj - przywracamy poprzedni tekst (można dodać logikę)
//     private void CancelButton_Click(object sender, RoutedEventArgs e)
//     {
//         // Można dodać logikę przywrócenia tekstu do poprzedniego stanu, np. za pomocą zmiennej
//         richTextBox.IsReadOnly = true;  // Blokujemy pole do edycji
//         EditButton.Content = "Edytuj";  // Zmieniamy przycisk na "Edytuj"
//         CancelButton.Visibility = Visibility.Collapsed;  // Ukrywamy przycisk "Anuluj"
//     }
        private void ShowFuelWindow(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new FuelWindow
            {
                Owner = Application.Current.MainWindow, // Powiązanie z głównym oknem
                Topmost = true // Zawsze na wierzchu
            };
            newUserWindow.Owner = Application.Current.MainWindow; // Powiązanie z głównym oknem
            newUserWindow.ShowDialog(); // Otwórz jako modalne okno (blokuje interakcje z głównym oknem)
        }

        private void GoToInfo(object sender, RoutedEventArgs e)
        {
            var infoView = App.ServiceProvider.GetRequiredService<InfoViewModel>(); // Pobieramy WIDOK, nie ViewModel
            var mainVM = App.ServiceProvider.GetRequiredService<MainViewModel>(); // Pobieramy WIDOK, nie ViewModel
            mainVM.CurrentView = infoView; 
        }
    }


}
