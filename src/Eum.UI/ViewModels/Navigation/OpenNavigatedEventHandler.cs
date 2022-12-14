namespace Eum.UI.ViewModels.Navigation;
public delegate void OpenNavigatedEventHandler(object sender, OpenNavigationEventArgs e);

public class OpenNavigationEventArgs
{
    public Type ToViewModelType
    {
        get;
        init;
    }
}
