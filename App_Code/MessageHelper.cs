using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

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
    public static void ShowErrorMessage(Page page, Exception ex, string message)
    {

        LogError(ex, page);
        string script = string.Format(
            "showErrorMessage('{0}', '{1}');",
           page.Request.Url.ToString().Replace("'", "\\'"),
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

    public static void LogError(Exception ex, Page page = null)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ErrorLogs (ErrorDate, UserName, ErrorMessage, StackTrace, PageUrl, UserAgent) VALUES (@ErrorDate, @UserName, @ErrorMessage, @StackTrace, @PageUrl, @UserAgent)", conn))
                {
                    cmd.Parameters.AddWithValue("@ErrorDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : "Anonim");
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                    cmd.Parameters.AddWithValue("@PageUrl", page != null ? page.Request.Url.ToString() : HttpContext.Current.Request.Url.ToString());
                    cmd.Parameters.AddWithValue("@UserAgent", HttpContext.Current.Request.UserAgent);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception logEx)
        {
            // Loglama sırasında hata oluşursa en azından debug çıktısına yazalım
            System.Diagnostics.Debug.WriteLine("Hata loglanırken hata oluştu: "+logEx.Message);
            System.Diagnostics.Debug.WriteLine("Orijinal hata: "+logEx.Message);
        }
    }

    public static void ShowAndLogError(Page page, Exception ex, string userMessage = null)
    {
        // Hatayı logla
        LogError(ex, page);

        // Kullanıcıya gösterilecek mesaj
        string message = string.IsNullOrEmpty(userMessage) 
            ? "İşlem sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            : userMessage;

        // Kullanıcıya hata mesajını göster
        ShowErrorMessage(page, "Hata", message);
    }
}