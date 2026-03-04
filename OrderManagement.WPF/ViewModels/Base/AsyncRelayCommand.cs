using System.Windows.Input;

namespace OrderManagement.WPF.ViewModels.Base;

public class AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
    : ICommand
{
    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => !_isExecuting && (canExecute?.Invoke(parameter) ?? true);

    public async void Execute(object? parameter)
    {
        if (_isExecuting) return;

        _isExecuting = true;
        CommandManager.InvalidateRequerySuggested();

        try
        {
            await execute(parameter);
        }
        finally
        {
            _isExecuting = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}