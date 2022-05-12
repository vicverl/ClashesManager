using System;
using System.IO;

namespace ClashesManager.Utils
{
    /// <summary>
    /// Helps to collect analycts data from projects
    /// </summary>
    public static class Analytics
    {
        public static string AppName { get; set; }
        public static Version Version { get; set; }
        public static string UserName { get; set; }
        public static string OpenedDocumentPath { get; set; }
        public static string RevitVersion { get; set; }

        public static void SaveAnalytics(string comments)
        {
            try
            {
                var time = DateTime.Now;
                string fileName = @"R:\1 - Проекты\- Координация\- Аналитика\plugins-LOG.txt";
                string AnalyticsLine = $"Revit {RevitVersion}\t{AppName} v{Version}\t{time}\t{UserName}\t{Path.GetFileNameWithoutExtension(OpenedDocumentPath)}\t{comments}";
                if (File.Exists(fileName))
                {
                    File.AppendAllLines(fileName, new string[] { AnalyticsLine });
                }
            }

            catch (Exception ex)
            {
                SaveExceptionReport(ex, "Не удалось сохранить аналитику");
            }
        }

        public static void SaveExceptionReport(Exception ex, string comments)
        {
            try
            {
                var time = DateTime.Now;
                string Report =
                    $"Time: {time}\n" +
                    $"AppName: {AppName}\n" +
                    $"Version: {Version}\n" +
                    $"Revit {RevitVersion}\n" +
                    $"User: {UserName}\n" +
                    $"Opened document: {OpenedDocumentPath}\n" +
                    $"\n" +
                    $"{ex.Message}\n" +
                    $"{ex.GetType()}\n" +
                    $"Source: {ex.Source}\n" +
                    $"StackTrace\n{ex.StackTrace}\n" +
                    $"\nComments: {comments}";

                string ReportName = $"{AppName}-{ex.GetType()}-{time.TimeOfDay.ToString().Replace(":", ".")}.txt";
                string ReportCatalog = @"R:\1 - Проекты\- Координация\- Аналитика\ErrorLogs\";
                string fileName = Path.Combine(ReportCatalog, ReportName);

                File.WriteAllText(fileName, Report);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Не удалось отправить отчет об исключении", "Внимание!");
            }

        }

        public static void SaveExceptionReport(Exception ex)
        {
            SaveExceptionReport(ex, String.Empty);
        }

    }
}
