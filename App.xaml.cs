using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Memory.Views;
using Memory.ViewModels;
using Memory.Database;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Memory;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public App()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
            .AddSingleton<CompanyContext>() // Rejestracja klasy CompanyContext
            .AddSingleton<CompanyViewModel>()
            .AddSingleton<HeaderViewModel>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<StartViewModel>()
            .AddSingleton<LoginViewModel>()
            .AddSingleton<InfoViewModel>()
            .AddSingleton<HeaderView>()
            .AddSingleton<StartView>()
            .AddSingleton<InfoView>()
            .AddSingleton<LoginView>()
            .AddSingleton<MainWindow>()
            .AddSingleton<CompanyView>();
            // .AddSingleton<CompanyView>();

        AppDbContext.InitializeDatabase();
        ServiceProvider = serviceProvider.BuildServiceProvider();

    }
    protected override void OnStartup(StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            MessageBox.Show($"Nieobsłużony wyjątek: {args.ExceptionObject}");
        };

        base.OnStartup(e);

        try
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            // Application.Current.MainWindow.KeyDown += OnKeyDown;
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd przy uruchamianiu: {ex.Message}");
        }
    }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Sprawdzamy, czy naciśnięto ESC
            if (e.Key == Key.Escape)
            {
                // Zamykamy wszystkie popupy w aplikacji
                foreach (var window in Application.Current.Windows)
                {
                    var mainWindow = window as Window;
                    if (mainWindow != null)
                    {
                        // Zamykamy popupy w bieżącym oknie
                        foreach (var child in mainWindow.Content as UIElementCollection)
                        {
                            if (child is Popup popup && popup.IsOpen)
                            {
                                popup.IsOpen = false;
                            }
                        }
                    }
                }
            }
        }
}


        // var companyContext = ServiceProvider.GetRequiredService<CompanyContext>();
        // companyContext.SelectedCompanyName = null;  // Przykładowe ustawienie ID firmy po zalogowaniu