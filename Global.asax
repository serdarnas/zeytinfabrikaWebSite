<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Web.SessionState" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // Uygulama başladığında çalışacak kod
    }

    void Application_End(object sender, EventArgs e)
    {
        // Uygulama kapandığında çalışacak kod
    }

    void Application_Error(object sender, EventArgs e)
    {
        try
        {
            // Son hatayı al
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                // Hatayı logla
                MessageHelper.LogError(ex);

                // Hata tipine göre yönlendirme yap
                if (ex is HttpException)
                {
                    HttpException httpEx = (HttpException)ex;
                    Server.ClearError();
                    Response.Clear();

                    // Orijinal URL'i al
                    string originalUrl = Request.Url.AbsolutePath;

                    //switch (httpEx.GetHttpCode())
                    //{
                    //    case 404:
                    //        Response.StatusCode = 404;
                    //        Response.Redirect(string.Format("~/fabrika/Fabrika_customErrors.aspx?aspxerrorpath={0}", Server.UrlEncode(originalUrl)));
                    //        break;

                    //    default:
                    //        Response.StatusCode = 500;
                    //        Response.Redirect(string.Format("~/fabrika/Fabrika_customErrors.aspx?aspxerrorpath={0}", Server.UrlEncode(originalUrl)));
                    //        break;
                    //}
                }
                else
                {
                    // Sistem hataları için
                    Server.ClearError();
                    Response.Clear();
                    Response.StatusCode = 500;
                    Response.Redirect("~/fabrika/Fabrika_customErrors.aspx");
                }
            }
        }
        catch (Exception finalEx)
        {
            // Son çare: Eğer hata yönetimi sırasında hata oluşursa
            try
            {
                Server.ClearError();
                Response.Clear();
                Response.Write("<html><body><h1>Sistem Hatası</h1><p>Beklenmeyen bir hata oluştu. Lütfen sistem yöneticinize başvurun.</p></body></html>");
                Response.End();
            }
            catch
            {
                // Yapılacak bir şey kalmadı
            }
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Yeni oturum başladığında çalışacak kod
    }

    void Session_End(object sender, EventArgs e)
    {
        // Oturum sonlandığında çalışacak kod
    }
</script>
