using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClashesManager.ViewModels.Utils
{
    /// <summary>
    ///     The class contains common methods for several ViewModels.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}