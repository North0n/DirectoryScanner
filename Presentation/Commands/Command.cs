using System;
using System.Windows.Input;

namespace Presentation.Commands;

public class Command : ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;

    public Command(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
    
    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}