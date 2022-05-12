using Autodesk.Revit.UI;
using System.Windows;

namespace ClashesManager.ViewModels.Utils
{
    internal interface IViewModel
    {
        void Initialize(UIApplication uiApp, Window window);
    }
}
