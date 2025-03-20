using System.Windows.Input;

namespace TheCatApp.Presentation.Commands;

class AsyncRelayCommand(Func<Task> execute, Func<object?, bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public AsyncRelayCommand(Func<Task> execute) : this(execute, null)
    {
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;
    public async void Execute(object? parameter) => await ExecuteAsync();
    private async Task ExecuteAsync() => await execute();
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}