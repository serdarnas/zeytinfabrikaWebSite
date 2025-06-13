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
        // Skip if we're already on the error page
        if (Request.Url.AbsolutePath.ToLower().Contains("fabrika_customerrors.aspx"))
            return;

        try
        {
            Exception ex = Server.GetLastError();
            if (ex == null) return;

            string errorId = Guid.NewGuid().ToString();
            HttpContext.Current.Session["Error_" + errorId] = ex.ToString();
            HttpContext.Current.Server.ClearError();

            HttpException httpEx = ex as HttpException;
            if (httpEx != null)
            {
                Response.Clear();
                Response.StatusCode = httpEx.GetHttpCode();
            }
            else
            {
                Response.Clear();
                Response.StatusCode = 500;
            }
            
            string redirectUrl = string.Format(
                "~/fabrika/Fabrika_customErrors.aspx?errorId={0}", 
                errorId
            );
            Response.Redirect(redirectUrl, false);
        }
        catch
        {
            // Avoid recursive errors
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
