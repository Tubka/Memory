using System.Windows;
using Memory.Database;
using Memory.Services;
using Memory.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;

namespace Memory.Views
{
    public partial class MainWindow : Window
    {
        public CompanyContext CompanyContext { get; }
        public Company _selectedCompany {get; set; }
        public DateTime _date { get;set; }
        public MainWindow _mainWindow;
        public MainWindow()
        {
            InitializeComponent();
            AppDbContext.InitializeDatabase();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();

            if (DataContext == null)
            {
                MessageBox.Show("blee MainViewModel jest NULL ❌");
            }
            else
            {
                // MessageBox.Show("MainViewModel poprawnie załadowany ✅");
            }

            WeakReferenceMessenger.Default.Register<SwitchViewMessage>(this, (r, m) =>
            {
                ((MainViewModel)DataContext).CurrentView = m.NewView;
            });
            // this.Resources["MonthHighlightConverter"] = new MonthHighlightConverter();

            var loginView = App.ServiceProvider.GetRequiredService<LoginViewModel>();
            ((MainViewModel)DataContext).CurrentView = loginView;
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }
    }
}

