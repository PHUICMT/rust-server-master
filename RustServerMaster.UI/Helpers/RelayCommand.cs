using System;
using System.Windows.Input;

namespace RustServerMaster.UI.Helpers
{
  /// <summary>
  /// A simple ICommand implementation for synchronous actions.
  /// </summary>
  public class RelayCommand : ICommand
  {
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
      return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
      _execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Force WPF to re-query CanExecute on this command.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
      CommandManager.InvalidateRequerySuggested();
    }
  }
}
