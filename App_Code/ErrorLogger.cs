using System;
using System.IO;
using System.Web;

public static class ErrorLogger
{
    private static readonly string LogDirectory = HttpContext.Current.Server.MapPath("~/App_Data/Logs");

    static ErrorLogger()
    {
        if (!Directory.Exists(LogDirectory))
        {
            Directory.CreateDirectory(LogDirectory);
        }
    }

    public static void LogError(Exception ex)
    {
        try
        {
            string logFile = Path.Combine(LogDirectory, String.Format("Error_{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
            string errorMessage = FormatErrorMessage(ex);

            File.AppendAllText(logFile, errorMessage);
        }
        catch
        {
            // Logging failed - nothing we can do
        }
    }

    public static void LogError(string message)
    {
        try
        {
            string logFile = Path.Combine(LogDirectory, String.Format("Error_{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
            string errorMessage = FormatErrorMessage(message);

            File.AppendAllText(logFile, errorMessage);
        }
        catch
        {
            // Logging failed - nothing we can do
        }
    }

    private static string FormatErrorMessage(Exception ex)
    {
        string userName = HttpContext.Current != null && 
                         HttpContext.Current.User != null && 
                         HttpContext.Current.User.Identity != null ? 
                         HttpContext.Current.User.Identity.Name : "Unknown";

        string url = HttpContext.Current != null && 
                    HttpContext.Current.Request != null && 
                    HttpContext.Current.Request.Url != null ? 
                    HttpContext.Current.Request.Url.ToString() : "Unknown";

        return String.Format(
            "Timestamp: {0}\r\n" +
            "Message: {1}\r\n" +
            "Source: {2}\r\n" +
            "StackTrace: {3}\r\n" +
            "TargetSite: {4}\r\n" +
            "User: {5}\r\n" +
            "URL: {6}\r\n" +
            "----------------------------------------\r\n",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ex.Message,
            ex.Source,
            ex.StackTrace,
            ex.TargetSite,
            userName,
            url
        );
    }

    private static string FormatErrorMessage(string message)
    {
        string userName = HttpContext.Current != null && 
                         HttpContext.Current.User != null && 
                         HttpContext.Current.User.Identity != null ? 
                         HttpContext.Current.User.Identity.Name : "Unknown";

        string url = HttpContext.Current != null && 
                    HttpContext.Current.Request != null && 
                    HttpContext.Current.Request.Url != null ? 
                    HttpContext.Current.Request.Url.ToString() : "Unknown";

        return String.Format(
            "Timestamp: {0}\r\n" +
            "Message: {1}\r\n" +
            "User: {2}\r\n" +
            "URL: {3}\r\n" +
            "----------------------------------------\r\n",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            message,
            userName,
            url
        );
    }
}
