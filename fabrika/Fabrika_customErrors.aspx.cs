using System;
using System.Web;
using System.IO;
using System.Text;

public partial class fabrika_Fabrika_customErrors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Only attempt to get error from session
            string errorId = Request.QueryString["errorId"];
            if (!string.IsNullOrEmpty(errorId))
            {
                string sessionKey = "Error_" + errorId;
                if (Session[sessionKey] != null)
                {
                    Response.Write(Session[sessionKey].ToString());
                    Session.Remove(sessionKey);
                    return;
                }
            }
            
            // Fallback message
            Response.Write("An error occurred, but no details are available.");
        }
        catch
        {
            Response.Write("An error occurred while displaying error details.");
        }
    }

    private void LogError(Exception ex)
    {
        try
        {
            string logPath = Server.MapPath("~/App_Data/Logs");
            Directory.CreateDirectory(logPath);

            string logFile = Path.Combine(logPath, 
                String.Format("{0:yyyy-MM-dd}_error.log", DateTime.Now));

            string message = String.Format(
                "[{0:yyyy-MM-dd HH:mm:ss}]\nURL: {1}\nHata: {2}\nDetay: {3}\n{4}\n",
                DateTime.Now, Request.RawUrl, ex.Message, ex.StackTrace,
                new string('-', 50));

            File.AppendAllText(logFile, message);
        }
        catch { } // Loglama hatasını yutuyoruz
    }

    protected void btnRetry_Click(object sender, EventArgs e)
    {
        string returnUrl = Request.QueryString["ReturnUrl"];
        Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? 
            returnUrl : "~/fabrika/default.aspx");
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/fabrika/default.aspx");
    }
}