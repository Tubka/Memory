using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Memory.Services;
using Memory.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Memory.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace Memory.Views
{
    public partial class StartView : UserControl
    {
        private ObservableCollection<string> Companies { get; set; }
        private ICollectionView CompaniesView { get; set; }

        private MainWindow _mainWindow { get; set; }
        // private CompanyViewModel _companyViewModel {get; set;}

        public StartView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
            _mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
            _mainWindow.SetTitle("Wybierz firmę");


            CompanySelectionService.AddItemsToComboBox(CompanyList);
        }

        private void AddItemsToComboBox()
        {
            var Companies = DatabaseServices.GetAllCompanies();
            if(Companies is null) 
            {
                return;
            }

            CompaniesView = CollectionViewSource.GetDefaultView(Companies);
            CompanyList.Items.Clear();
            foreach (var company in Companies)
            {
                CompanyList.Items.Add(company);
            }
        }
        private bool MyItemFilter(object item, string filter)
        {
            if (item == null || string.IsNullOrWhiteSpace(filter))
                return true; // Jeśli nie ma filtra, pokaż wszystkie elementy

            // Filtruj elementy, ignorując wielkość liter
            return item.ToString().IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        private void GoToCompany(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CompanyList.SelectedItem is not Company selectedCompany)
                {
                    MessageBox.Show("Wybierz firmę przed przejściem dalej!");
                    return;
                }

                // Ustawiamy wybraną firmę w _mainWindow
                // _mainWindow._selectedCompany = selectedCompany;
                // MessageBox.Show($"Wybrana firma: {selectedCompany.Name}");

                // Pobieramy widok CompanyView
                // var companyView = App.ServiceProvider.GetRequiredService<CompanyView>();
                // if (companyView == null)
                //     throw new Exception("CompanyView jest null!");

                // MessageBox.Show("Przechodzę do widoku firmy...");

                // if (DataContext == null)
                //     throw new Exception("DataContext w MainWindow jest null!");

                // var mainViewModel = DataContext as MainViewModel;
                // if (mainViewModel == null)
                //     throw new Exception("DataContext nie jest MainViewModel!");
                var companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
                var newCompany = new Company { Id = selectedCompany.Id, Name = selectedCompany.Name };
                WeakReferenceMessenger.Default.Send(new CompanyChangedMessage(newCompany));
                companyViewModel.Date = DateTime.Now;
                // var companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
                var mainVM = App.ServiceProvider.GetRequiredService<MainViewModel>();
                mainVM.CurrentView = companyViewModel;
                // mainViewModel.CurrentView = companyViewModel;
                // mainViewModel.CurrentView = companyView; // Przełączamy widok
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}\n\n{ex.StackTrace}");
            }
        }

        // private void GoToCompany(object sender, RoutedEventArgs e)
        // {
        //     try
        //     {
        //         MessageBox.Show("jest");

        //         var companyView = App.ServiceProvider.GetRequiredService<CompanyView>();
        //         if (companyView == null)
        //             throw new Exception("CompanyView jest null!");

        //         MessageBox.Show("jest2");

        //         if (DataContext == null)
        //             throw new Exception("DataContext w MainWindow jest null!");

        //         var mainViewModel = DataContext as MainViewModel;
        //         if (mainViewModel == null)
        //             throw new Exception("DataContext nie jest MainViewModel!");

        //         ((MainViewModel)DataContext).CurrentView = companyView; // Zmiana widoku po zalogowaniu
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show($"Błąd: {ex.Message}\n\n{ex.StackTrace}");
        //     }
        //     // _mainWindow._selectedCompany = (Company)CompanyList.SelectedItem;
        // }
        private void GoAddNewCompany(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = true;
        }

        private void AddNewCompany(object sender, RoutedEventArgs e)
        {
            string InputText = InputTextBox.Text;
            Company company = DatabaseServices.AddCompany(InputText);
            if(company is not Company)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania firmy: {InputText}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                MyPopup.IsOpen = false;
                return;
            }
            CompanySelectionService.AddItemsToComboBox(CompanyList);
            MyPopup.IsOpen = false;
        }

    }
}
