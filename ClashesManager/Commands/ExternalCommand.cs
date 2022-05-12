using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ClashesManager.Commands.Handlers;
using ClashesManager.Core;
using ClashesManager.Views;

namespace ClashesManager.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ExternalCommand : IExternalCommand
    {
        private static ClashesManagerView _view;
        public static readonly ExternalEventHandler ExternalEvent = new();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (_view is not null && _view.IsLoaded)
            {
                _view.Focus();
                return Result.Succeeded;
            }

            RevitApi.UiApplication = commandData.Application;

            _view = new ClashesManagerView(RevitApi.UiApplication);
            _view.Show();

            return Result.Succeeded;
        }
    }
}