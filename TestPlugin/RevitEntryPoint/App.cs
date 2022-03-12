using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace CollisionsManager.RevitEntryPoint
{
    class App : IExternalApplication
    {
        string AppName = "Collisions Manager";
        public PushButton testPlugin_BTN;
        public static string PathOfDownloadeedInstaller;

        public Result OnShutdown(UIControlledApplication application)
        {
            string RibbonTabName = "Eneca";
            string RibbonTabPanelName = "Links";

            // Method to add Tab and Panel 
            RibbonPanel panel = CreateRibbonPanel(RibbonTabName, RibbonTabPanelName, application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            #region BUTTON
            if (panel.AddItem(
                new PushButtonData(AppName, "Collisions\nManager", thisAssemblyPath,
                    typeof(EntryCommand).FullName)) is PushButton button1)
            {
                button1.ToolTip = "Test plugin tooltip"; //Заголовок подсказки
                button1.LongDescription = "Long description";//Properties.Resources._Rib_BTN_LongDiscription;
                Uri uriIcon = new Uri("pack://application:,,,/TestPlugin;component/Resources/icon.png");
                BitmapImage largeImage = new BitmapImage(uriIcon);
                button1.LargeImage = largeImage;
                this.testPlugin_BTN = button1;

            }
            #endregion

            return Result.Succeeded;
        }

        private RibbonPanel CreateRibbonPanel(string TabName, string TabPanelName, UIControlledApplication a)
        {
            // Try to create ribbon tab. 
            try
            {
                Autodesk.Windows.RibbonTab RibbonTab = null;

                foreach (var tab in Autodesk.Windows.ComponentManager.Ribbon.Tabs)
                {
                    if (tab.Title == TabName)
                    {
                        RibbonTab = tab;
                        break;
                    }
                }
                if (RibbonTab == null)
                {
                    a.CreateRibbonTab(TabName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in creating ribbon panel");
            }

            // Empty ribbon panel 
            RibbonPanel ribbonPanel = null;

            // Try to create ribbon panel.
            try
            {
                foreach (var p in a.GetRibbonPanels(TabName))
                {
                    if (p.Name == TabPanelName)
                    {
                        ribbonPanel = p;
                        break;
                    }
                }
                if (ribbonPanel == null)
                {
                    ribbonPanel = a.CreateRibbonPanel(TabName, TabPanelName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in creating ribbon panel");
            }

            //return panel 
            return ribbonPanel;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            if (PathOfDownloadeedInstaller != null)
            {
                System.Diagnostics.Process.Start(PathOfDownloadeedInstaller);
            }
            return Result.Succeeded;
        }
    }
}
