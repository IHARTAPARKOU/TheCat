using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheCatApp.Models.Abstractions;

public abstract class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetValue<T>(ref T local, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(local, newValue))
        {
            return false;
        }

        local = newValue;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
