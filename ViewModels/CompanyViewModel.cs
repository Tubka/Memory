using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using Memory.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Memory.ViewModels
{
    public class CompanyViewModel : ObservableObject 
    {
        public ICommand SetCompanyId { get; }
        public ICommand SetCompanyName { get; }
        private DateTime _date;
        private Company _company; 
        private User _user;

        // public DateTime Date
        // {
        //     get => _date;
        //     set
        //     {
        //         SetProperty(ref _date, nameof(Month));
        //         // if (SetProperty(ref _date, value)) // 🔥 Automatyczne powiadomienie UI
        //         // {
        //             // OnPropertyChanged(nameof(Month)); // 🔄 Powiadomienie o zmianie Month
        //             // OnPropertyChanged(nameof(Year));  // 🔄 Powiadomienie o zmianie Year
        //         // }
        //     }
        // }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    MessageBox.Show($"Date przed onpropertychanged {Month}");
                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(Month));
                    OnPropertyChanged(nameof(Year));
                    WeakReferenceMessenger.Default.Send(new RefreshCompanyViewMessage());
                }
            }
        }

        public Company Company
        {
            get => _company;
            set
            {
                if (_company != value)
                {
                    _company = value;
                    OnPropertyChanged(nameof(Company));
                    OnPropertyChanged(nameof(CompanyId));
                    OnPropertyChanged(nameof(CompanyName));
                    WeakReferenceMessenger.Default.Send(new RefreshCompanyViewMessage());
                }
            }
        }

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(UserId));
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        
        public int UserId
        {
            get => _user?.Id ?? 0;
            set
            {
                if (_user != null && _user.Id != value)
                {
                    _user.Id = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        public string UserName
        {
            get => _user?.Name ?? string.Empty;
            set
            {
                if (_user != null && _user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public CompanyViewModel()
        {
            Date = DateTime.Today;
            Company = new Company();

            WeakReferenceMessenger.Default.Register<DateChangedMessage>(this, (r, m) =>
            {
                Date = m.Value;
            });

            WeakReferenceMessenger.Default.Register<CompanyChangedMessage>(this, (r, m) =>
            {
                Company = m.Value;
            });

            WeakReferenceMessenger.Default.Register<UserChangedMessage>(this, (r, m) =>
            {
                User = m.Value;
            });
        }

        public int Month
        {
            get => _date.Month;
            set
            {
                if (_date.Month != value)
                {
                    Date = new DateTime(_date.Year, value, 1);
                    OnPropertyChanged(nameof(Month));
                }
            }
        }

        public int Year
        {
            get => _date.Year;
            set
            {
                if (_date.Year != value)
                {
                    Date = new DateTime(value, _date.Month, 1);
                }
            }
        }

        public int CompanyId
        {
            get => _company?.Id ?? 0;
            set
            {
                if (_company != null && _company.Id != value)
                {
                    _company.Id = value;
                    OnPropertyChanged(nameof(CompanyId));
                }
            }
        }

        public string CompanyName 
        {
            get => _company?.Name ?? string.Empty;
            set
            {
                if (_company != null && _company.Name != value)
                {
                    _company.Name = value;
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
