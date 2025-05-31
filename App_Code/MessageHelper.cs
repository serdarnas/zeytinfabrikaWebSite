using System;
using System.Web.UI;

public static class MessageHelper
{
    public static void ShowSuccessMessage(Page page, string title, string message)
    {
        string script = string.Format(
            "showSuccessMessage('{0}', '{1}');",
            title.Replace("'", "\\'"),
            message.Replace("'", "\\'")
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "successMessage", script, true);
    }

    public static void ShowErrorMessage(Page page, string title, string message)
    {
        string script = string.Format(
            "showErrorMessage('{0}', '{1}');",
            title.Replace("'", "\\'"),
            message.Replace("'", "\\'")
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", script, true);
    }

    public static void ShowWarningMessage(Page page, string title, string message)
    {
        string script = string.Format(
            "showWarningMessage('{0}', '{1}');",
            title.Replace("'", "\\'"),
            message.Replace("'", "\\'")
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "warningMessage", script, true);
    }

    public static void ShowInfoMessage(Page page, string title, string message)
    {
        string script = string.Format(
            "showInfoMessage('{0}', '{1}');",
            title.Replace("'", "\\'"),
            message.Replace("'", "\\'")
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "infoMessage", script, true);
    }
} 