using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Memory.Messages;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Memory.Views;

namespace Memory.ViewModels;
public class HeaderViewModel : ObservableObject 
{
    private string _monthText;
    public string MonthText
    {
        get => _monthText;
        set
        {
            if (_monthText != value)
            {
                _monthText = value;
                OnPropertyChanged(nameof(MonthText));
            }
        }
    }
    private DateTime _selectedDate;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                // _messenger.Send(new DateChangedMessage(_selectedDate)); // Wysyłamy datę do innych komponentów
                // _messenger.Send(new RefreshCompanyViewMessage());
            }
        }
    }

    public ICommand IncreaseYearCommand { get; }
    public ICommand DecreaseYearCommand { get; }
    public ICommand IncreaseMonthCommand { get; }
    public ICommand DecreaseMonthCommand { get; }
    public ICommand FastIncreaseMonthCommand { get; }
    public ICommand FastDecreaseMonthCommand { get; }
    public ICommand SetMonthCommand { get; }

    // public ICommand SubmitDateCommand { get; }
    private DateTime _date = new DateTime(2025, 1, 1); // Domyślna data
    private readonly CompanyViewModel _companyViewModel;
    private readonly HeaderView _headerView;
    public ICommand ChangeDateCommand { get; }
    public ICommand SubmitDateCommand { get; }
    // public ObservableCollection<int> Months { get; } = new ObservableCollection<int>(Enumerable.Range(1, 12));//generacja 12 przyciskow

    public HeaderViewModel()
    {
        _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
        // _headerView = App.ServiceProvider.GetRequiredService<HeaderView>();

        // MessageBox.Show($"{_companyViewModel.Date.Month}");
        
        IncreaseYearCommand = new RelayCommand(IncreaseYear);
        DecreaseYearCommand = new RelayCommand(DecreaseYear);
        IncreaseMonthCommand = new RelayCommand(IncreaseMonth);
        DecreaseMonthCommand = new RelayCommand(DecreaseMonth);
        FastIncreaseMonthCommand = new RelayCommand(FastIncreaseMonth);
        FastDecreaseMonthCommand = new RelayCommand(FastDecreaseMonth);
        SubmitDateCommand = new RelayCommand(SubmitDate);
        // WeakReferenceMessenger.Default.Register<DateChangedMessage>(this, (r, m) =>
        // {
        //     _date = m.Value;
        //     Application.Current.Dispatcher.Invoke(() =>
        // {


    // var headerView = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?
    //     .FindName("HeaderView") as HeaderView;

    // _headerView?.RefreshBindings();
    // });
            // Application.Current.Dispatcher.Invoke(() =>
            // {
            //     var binding = MonthTextBox.GetBindingExpression(TextBox.TextProperty);
            //     binding?.UpdateTarget(); 

            //     MessageBox.Show($"invoke [HeaderViewModel] Otrzymałem nową datę: {_date}"); 
            // });
            // OnPropertyChanged(nameof(Month)); // Powiadamiamy UI o zmianie
        // });
        

        // SetMonthCommand = new RelayCommand<int>(month => SetMonth(month)); // zmiana miesiaca po kliknieciu miesiaca


        // WeakReferenceMessenger.Default.Register<DateChangedMessage>(this, (r, m) =>
        // {
        //     Date = m.Value; // Teraz Value będzie dostępne
        // });
        // StrongReferenceMessenger.Default.Register<GetCurrentDateMessage>(this, (r, m) =>
        // {
        //     m.Reply(Date);
        // });
        // Rejestrujemy nasłuchiwanie na zmianę firmy
        // WeakReferenceMessenger.Default.Register<CompanyChangedMessage>(this, (r, m) =>
        // {
        //     Company = m.Value; // Ustawiamy nową firmę, jeśli ktoś ją zmienił globalnie
        // });

        // Pozwalamy innym ViewModelom pobierać bieżącą firmę
        // StrongReferenceMessenger.Default.Register<GetCurrentCompanyMessage>(this, (r, m) =>
        // {
        //     m.Reply(Company);
        // });

        // StrongReferenceMessenger.Default.Register<DateChangedMessage>(this, (r, m) =>
        // {
        //     Date = m.Value;
        // });
    }
    public int Month
    {
        get => _companyViewModel.Month;
        set
        {
            if (_companyViewModel.Month != value)
            {
                _companyViewModel.Month = value;
                OnPropertyChanged(nameof(Month));
            }
        }
    }
    private void SetMonth(int month)
    {
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(new DateTime(_companyViewModel.Year, month, 1)));
    }
    // [RelayCommand]
    public void SubmitDate()
    {
        var _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
        var text = _companyViewModel.Month;
    }
    // [RelayCommand]
    private void IncreaseMonth()
    {
        if (_companyViewModel.Date.Month == 12) {
            return;
        }
        var newDate = _companyViewModel.Date.AddMonths(1);
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(newDate));
    }
    private void FastIncreaseMonth()
    {
        var diff = 4;
        var date = _companyViewModel.Date;
        if (date.Month + diff  >= 12) {
            date = new DateTime(date.Year, 12, 1);
        } 
        else
        {
            date = new DateTime(date.Year, date.AddMonths(diff).Month, 1);
        }
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(date));
    }
    private void FastDecreaseMonth()
    {
        var diff = -4;
        var date = _companyViewModel.Date;
        if (date.Month + diff  <= 1) {
            date = new DateTime(date.Year, 1, 1);
        } 
        else
        {
            date = new DateTime(date.Year, date.AddMonths(diff).Month, 1);
        }
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(date));
    }

    private void DecreaseMonth()
    {

        if (_companyViewModel.Date.Month == 1) {
            return;
        }
        var newDate = _companyViewModel.Date.AddMonths(-1);
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(newDate));
    }

    // [RelayCommand]
    private void IncreaseYear()
    {
        var newDate = _companyViewModel.Date.AddYears(1);
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(newDate));
    }

    // [RelayCommand]
    private void DecreaseYear()
    {
        var newDate = _companyViewModel.Date.AddYears(-1);
        WeakReferenceMessenger.Default.Send(new DateChangedMessage(newDate));
    }

    public void ChangeDate(DateTime newDate)
    {
        // Date = newDate;
        // _companyViewModel.
        // _messenger.Send(new RefreshCompanyViewMessage());
        // WeakReferenceMessenger.Default.Send(new DateChangedMessage(newDate));
    }
    private bool _isRefreshScheduled = false;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
