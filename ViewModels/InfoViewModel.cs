using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Memory.ViewModels;
public class InfoViewModel : ObservableObject 
{

    public InfoViewModel()
    {
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
