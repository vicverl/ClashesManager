using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.UI;
using ClashesManager.ViewModels.Utils;

namespace ClashesManager.ViewModels
{
    public class RewriteViewModel : ViewModelBase
    {
        Window _window;

        private ICommand _rewriteCommand;
        private ICommand _cancelCommand;

        public ICommand RewriteCommand => _rewriteCommand ??= new RelayCommand(param => Rewrite());
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(param => Cancel());

        public bool ToRewrite { get; private set; }

        public void Initialize(Window window)
        {
            _window = window;
        }

        private void Rewrite()
        {
            ToRewrite = true;
            _window.Close();
        }

        private void Cancel()
        {
            _window.Close();
        }
    }
}
