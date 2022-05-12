using AutoUpdater;
using System;
using System.Reflection;

namespace ClashesManager.Utils
{
    /// <summary>
    /// Helps application to recieve updates
    /// </summary>
    internal class UpdatingApplication : IUpdatingApplication
    {

        public string ApplicationName => Analytics.AppName;
        public Version CurrentVersion => Analytics.Version;
        public string ApplicationUpdateId => (Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyProductAttribute)) as AssemblyProductAttribute)?.Product;

        public Uri UpdateInfoXMLLocation => new Uri(@"https://raw.githubusercontent.com/EnecaTechnology/Updates/master/update.xml");
    }
}
