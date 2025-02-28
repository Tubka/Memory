using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memory.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Memory.ViewModels;
public class MainViewModel : ObservableObject
{
    public HeaderViewModel HeaderViewModel { get; }
    public CompanyViewModel CompanyViewModel { get; }
    public LoginViewModel LoginViewModel { get; }
    private ObservableObject _currentView; 
    public CompanyView CompanyView { get; }


    public MainViewModel()
    {
        HeaderViewModel = App.ServiceProvider.GetRequiredService<HeaderViewModel>();
        CompanyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
        LoginViewModel = App.ServiceProvider.GetRequiredService<LoginViewModel>();
        // CompanyView.DataContext = this; // 🔥 Przekazanie DataContext
        var startViewModel = App.ServiceProvider.GetRequiredService<StartViewModel>();
        // StartViewModel = App.ServiceProvider.GetRequiredService<StartViewModel>();
        // CurrentView = App.ServiceProvider.GetRequiredService<StartViewModel>(); // Ustaw domyślny ViewModel
        CurrentView = startViewModel; // Domyślnie ustawiamy pierwszy widok
    }

    public ObservableObject CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value); 
    }
    public ICommand ChangeViewCommand => new RelayCommand<object>(vm => CurrentView = vm as ObservableObject);
}


// namespace Memory.ViewModels;
// public class MainViewModel : ObservableObject
// {
//     public HeaderViewModel HeaderViewModel { get; }
//     public CompanyViewModel CompanyViewModel { get; }
//     private UserControl _currentView;

//     public MainViewModel()
//     {
//         HeaderViewModel = App.ServiceProvider.GetRequiredService<HeaderViewModel>();  // ✅ Pobieramy singletona
//         CompanyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
//     }

//     public UserControl CurrentView
//     {
//         get => _currentView;
//         set => SetProperty(ref _currentView, value);
//     }

//     public event PropertyChangedEventHandler PropertyChanged;
//     public ICommand ChangeViewCommand => new RelayCommand<UserControl>(view => CurrentView = view);

// }


