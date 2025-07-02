using System;
using System.Web;

public partial class fabrika_Fabrika_customErrors : System.Web.UI.Page
{
    protected int RemainingSeconds
    {
        get
        {
            // 3 gün geri sayım için
            return 3 * 24 * 60 * 60;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
 
        try
        {
            if (!IsPostBack)
            {
                // Master Page'e başlık bilgisini ayarla
                if (Master is fabrika_FabrikaMasterPage)
                {
                    fabrika_FabrikaMasterPage master = (fabrika_FabrikaMasterPage)Master;
                    master.KlasorAdi = "Hata";
                    master.SayfaAdi = "Hata Sayfası";
                }

                // URL'den hata yolunu al
                string errorPath = Request.QueryString["aspxerrorpath"];
                if (!string.IsNullOrEmpty(errorPath))
                {
                    lblErrorCode.Text = "404";
                    lblErrorMessage.Text = "Sayfa Bulunamadı";
                    lblErrorDetails.Text = string.Format("Aradığınız sayfa ({0}) bulunamadı veya taşınmış olabilir.", errorPath);
                }
                else
                {
                    lblErrorCode.Text = "500";
                    lblErrorMessage.Text = "Üzgünüz, bir hata oluştu!";
                    lblErrorDetails.Text = "İşleminiz sırasında beklenmeyen bir hata oluştu. Lütfen tekrar deneyin veya sistem yöneticinize başvurun.";
                }
            }
        }
        catch (Exception ex)
        {
            // Hata sayfasında hata oluşursa sadece loglama yap, kullanıcıya basit mesaj göster
            try
            {
                MessageHelper.LogError(ex);
            }
            catch
            {
                // MessageHelper bile çalışmazsa hiçbir şey yapma
            }
            
            lblErrorCode.Text = "500";
            lblErrorMessage.Text = "Sistem Hatası";
            lblErrorDetails.Text = "Beklenmeyen bir hata oluştu. Lütfen sistem yöneticinize başvurun.";
        }
    }

    protected void btnRetry_Click(object sender, EventArgs e)
    {
        // Önceki sayfaya geri dön
        if (Request.UrlReferrer != null)
        {
            Response.Redirect(Request.UrlReferrer.ToString());
        }
        else
        {
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        // Ana sayfaya yönlendir
        Response.Redirect("~/fabrika/Default.aspx");
    }
}