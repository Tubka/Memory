using System.ComponentModel;
using System.ServiceModel.Channels;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Memory.ViewModels;
public class StartViewModel : ObservableObject 
{

    public StartViewModel()
    {
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
