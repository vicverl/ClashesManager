using Autodesk.Revit.UI;
using ClashesManager.Commands;
using ClashesManager.Utils;
using System.Reflection;

namespace ClashesManager
{
    [UsedImplicitly]
    public class ExternalApplication : IExternalApplication
    {
        public static ExternalApplication ThisApp;
        public static string PathOfDownloadedInstaller;
        public static PushButton showButton;

        public ExternalApplication()
        {
            ThisApp = this;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            Analytics.AppName = (Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute)?.Title;
            Analytics.Version = Assembly.GetExecutingAssembly().GetName().Version;

            var panel = application.CreatePanel("Manage", "Eneca");

            showButton = panel.AddPushButton<ExternalCommand>("Clashes\nManager");
            showButton.SetImage("/ClashesManager;component/Resources/Icons/RibbonIcon16.png");
            showButton.SetLargeImage("/ClashesManager;component/Resources/Icons/RibbonIcon32.png");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            if (PathOfDownloadedInstaller != null)
            {
                System.Diagnostics.Process.Start(PathOfDownloadedInstaller);
            }
            return Result.Succeeded;
        }
    }
}