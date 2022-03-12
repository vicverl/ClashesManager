using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionsManager.RevitEntryPoint
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class EntryCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                ShowForm(commandData.Application);

                #region Registration Of External Events

                //new AppOnShutDownEvent();
                #endregion

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        /// <summary>
        /// Этод метод описывает запуск формы
        /// </summary>
        /// <param name="uiapp">Текущий объект Revit UIApplication, описывающий окно Revit</param>
        public void ShowForm(Autodesk.Revit.UI.UIApplication uiapp)
        {
            if (UI.CurrentWindow == null || (UI.CurrentWindow != null && !UI.CurrentWindow.IsLoaded))
            {
                new UI(uiapp).Show();
            }
        }
    }
}
