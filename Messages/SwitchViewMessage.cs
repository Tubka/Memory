using CommunityToolkit.Mvvm.ComponentModel;
public class SwitchViewMessage
{
    public ObservableObject NewView { get; }
    public SwitchViewMessage(ObservableObject newView) => NewView = newView;
}