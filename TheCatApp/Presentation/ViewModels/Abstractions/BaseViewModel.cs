using System.Windows;
using TheCatApp.Models.Abstractions;

namespace TheCatApp.Presentation.ViewModels.Abstractions;

public abstract class BaseViewModel : BaseModel
{
    protected static void UpdateUI(Delegate method)
    {
        Application.Current.Dispatcher.BeginInvoke(method);
    }
}
