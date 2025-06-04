using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RustServerMaster.UI.ViewModel
{
  /// <summary>
  /// Base class for all ViewModels, implements INotifyPropertyChanged.
  /// </summary>
  public class BaseViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raise PropertyChanged event for the given property name.
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Set a backing field and raise PropertyChanged if the value actually changed.
    /// </summary>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
      if (Equals(field, value))
        return false;

      field = value;
      OnPropertyChanged(propertyName);
      return true;
    }
  }
}
