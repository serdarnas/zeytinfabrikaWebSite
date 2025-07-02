using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Tüm proje genelinde kullanılacak standart mesajlaşma sistemi
/// Gritter tabanlı kullanıcı mesajları ve hata loglama
/// </summary>
public static class MessageHelper
{
    /// <summary>
    /// Başarılı işlem mesajı gösterir (yeşil)
    /// </summary>
    public static void ShowSuccessMessage(Page page, string title, string message, bool sticky = false)
    {
        string script = string.Format(
            "showSuccessMessage('{0}', '{1}', {2});",
            EscapeJavaScript(title),
            EscapeJavaScript(message),
            sticky.ToString().ToLower()
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "successMessage_" + DateTime.Now.Ticks, script, true);
    }

    /// <summary>
    /// Hata mesajı gösterir (kırmızı)
    /// </summary>
    public static void ShowErrorMessage(Page page, string title, string message, bool sticky = false)
    {
        string script = string.Format(
            "showErrorMessage('{0}', '{1}', {2});",
            EscapeJavaScript(title),
            EscapeJavaScript(message),
            sticky.ToString().ToLower()
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage_" + DateTime.Now.Ticks, script, true);
    }

    /// <summary>
    /// Uyarı mesajı gösterir (sarı)
    /// </summary>
    public static void ShowWarningMessage(Page page, string title, string message, bool sticky = false)
    {
        string script = string.Format(
            "showWarningMessage('{0}', '{1}', {2});",
            EscapeJavaScript(title),
            EscapeJavaScript(message),
            sticky.ToString().ToLower()
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "warningMessage_" + DateTime.Now.Ticks, script, true);
    }

    /// <summary>
    /// Bilgi mesajı gösterir (mavi)
    /// </summary>
    public static void ShowInfoMessage(Page page, string title, string message, bool sticky = false)
    {
        string script = string.Format(
            "showInfoMessage('{0}', '{1}', {2});",
            EscapeJavaScript(title),
            EscapeJavaScript(message),
            sticky.ToString().ToLower()
        );
        ScriptManager.RegisterStartupScript(page, page.GetType(), "infoMessage_" + DateTime.Now.Ticks, script, true);
    }

    /// <summary>
    /// Exception'ı veritabanına loglar
    /// </summary>
    public static void LogError(Exception ex, Page page = null)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO ErrorLogs (ErrorDate, UserName, ErrorMessage, StackTrace, PageUrl, UserAgent) 
                    VALUES (@ErrorDate, @UserName, @ErrorMessage, @StackTrace, @PageUrl, @UserAgent)", conn))
                {
                    cmd.Parameters.AddWithValue("@ErrorDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : "Anonim");
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message ?? string.Empty);
                    cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageUrl", page != null ? page.Request.Url.ToString() : HttpContext.Current.Request.Url.ToString());
                    cmd.Parameters.AddWithValue("@UserAgent", HttpContext.Current.Request.UserAgent ?? string.Empty);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception logEx)
        {
            // Loglama sırasında hata oluşursa sadece gerçekten kritik durumlarda Event Log'a yazalım
            try
            {
                System.Diagnostics.EventLog.WriteEntry("ZeytinFabrika", 
                    string.Format("Hata loglanırken hata oluştu: {0}. Orijinal hata: {1}", 
                    logEx.Message, ex.Message), 
                    System.Diagnostics.EventLogEntryType.Error);
            }
            catch 
            {
                // Event Log'a da yazamazsak hiçbir şey yapma (sonsuz döngü önlemi)
            }
        }
    }

    /// <summary>
    /// Exception'ı loglar ve kullanıcıya hata mesajı gösterir
    /// En çok kullanılan metod - hem loglama hem kullanıcı bildirimi
    /// </summary>
    public static void ShowAndLogError(Page page, Exception ex, string userMessage = null)
    {
        // Hatayı logla
        LogError(ex, page);

        // Kullanıcıya gösterilecek mesaj
        string message = string.IsNullOrEmpty(userMessage) 
            ? "İşlem sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            : userMessage;

        // Kullanıcıya hata mesajını göster (hata mesajları yapışkan olsun)
        ShowErrorMessage(page, "Hata", message, true);
    }

    /// <summary>
    /// JavaScript string'lerini güvenli hale getirir
    /// </summary>
    private static string EscapeJavaScript(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
            
        return text.Replace("\\", "\\\\")
                   .Replace("'", "\\'")
                   .Replace("\"", "\\\"")
                   .Replace("\r", "\\r")
                   .Replace("\n", "\\n")
                   .Replace("\t", "\\t");
    }
}