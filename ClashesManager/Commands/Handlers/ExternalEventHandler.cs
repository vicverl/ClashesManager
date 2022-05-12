using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ClashesManager.Core;
using ClashesManager.Utils;
using System;
using System.Windows;


namespace ClashesManager.Commands.Handlers
{
    public class ExternalEventHandler : BaseEventHandler
    {
        private Window _window;
        private string _someText;

        public override void Execute(UIApplication app)
        {
            try
            {
                using (Transaction t = new Transaction(RevitApi.Document, "ProjectName_DocumentChanged"))
                {
                    t.Start();

                    System.Windows.MessageBox.Show(_window, _someText);

                    t.Commit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_window, "Описание ошибки", "Ошибка");
                Analytics.SaveExceptionReport(e, "Комментарий к логу");
            }


        }

        public void Raise(Window window, string someText)
        {
            this._window = window;
            this._someText = someText;
            Raise();
        }
    }
}