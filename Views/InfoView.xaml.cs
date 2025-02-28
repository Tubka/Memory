using System.Windows;
using System.Windows.Controls;
using Memory.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Memory.Views
{
    public partial class InfoView : UserControl
    {
        public InfoView()
        {
            // _mainWindow = mainWindow;
            // _mainWindow.Title = _mainWindow._selectedCompany.Name + " " + _mainWindow._selectedCompany.Id;
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            var CompanyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
            var MainVM = App.ServiceProvider.GetRequiredService<MainViewModel>();
            MainVM.CurrentView = CompanyViewModel;
            // _mainWindow.SwitchView(new StartView(_mainWindow));
        }

        private void ShowNewUserWindow(object sender, RoutedEventArgs e)
        {
            // var newUserWindow = new InfoView();
            // // {
            // //     Owner = Application.Current.MainWindow, // Powiązanie z głównym oknem
            // //     Topmost = true // Zawsze na wierzchu
            // // };
            // newUserWindow.Owner = Application.Current.MainWindow; // Powiązanie z głównym oknem
            // newUserWindow.ShowDialog(); // Otwórz jako modalne okno (blokuje interakcje z głównym oknem)
        }


        // private void GoToHelloSelection(object sender, RoutedEventArgs e)
        // {
        //     // Przełącz widok na HelloWorldView
        //     // _mainWindow._selectedCompany = (Company)CompanyList.SelectedItem;
        //     _mainWindow.SwitchView(new HelloWorldView(_mainWindow));
        // }

    }
}
