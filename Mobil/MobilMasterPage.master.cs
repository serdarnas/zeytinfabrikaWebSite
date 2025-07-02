using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

public partial class Mobil_MobilMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Önce Session kontrolü yap
        if (Session["SirketID"] == null)
        {
            // Session yok, login'e yönlendir
            Response.Redirect("~/Mobil/Login.aspx");
            return;
        }

        // Session var ama Authentication yok ise cookie'yi tekrar set et
        if (Session["SirketID"] != null && !HttpContext.Current.User.Identity.IsAuthenticated && Session["Email"] != null)
        {
            System.Diagnostics.Debug.WriteLine("MasterPage: Session var ama Authentication yok, cookie tekrar set ediliyor");
            FormsAuthentication.SetAuthCookie(Session["Email"].ToString(), true);
        }

        // Kullanıcı bilgilerini göster
        if (Session["KullaniciAdSoyad"] != null)
        {
            lblKullanici.Text = Session["KullaniciAdSoyad"].ToString();
        }
        else
        {
            lblKullanici.Text = HttpContext.Current.User.Identity.Name;
        }
    }

    protected void lnkCikis_Click(object sender, EventArgs e)
    {
        try
        {
            // Session'ı temizle
            Session.Clear();
            Session.Abandon();
            
            // Authentication cookie'yi temizle
            FormsAuthentication.SignOut();
            
            // Cache'i temizle
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            
            // Login sayfasına yönlendir
            Response.Redirect("~/Mobil/Login.aspx", true);
        }
        catch (Exception ex)
        {
            // Hata durumunda da login'e yönlendir
            System.Diagnostics.Debug.WriteLine("Çıkış hatası: " + ex.Message);
            Response.Redirect("~/Mobil/Login.aspx", true);
        }
    }

    private void YenileSession()
    {
        try
        {
            string email = HttpContext.Current.User.Identity.Name;
            if (!string.IsNullOrEmpty(email))
            {
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                var kullanici = db.Kullanicilars.FirstOrDefault(x => x.Email == email);
                
                if (kullanici != null)
                {
                    var sirket = db.Sirketlers.FirstOrDefault(x => x.SirketID == kullanici.SirketID);
                    
                    if (sirket != null)
                    {
                        Session["KullaniciID"] = kullanici.KullaniciID;
                        Session["KullaniciAdSoyad"] = kullanici.AdSoyad;
                        Session["SirketID"] = kullanici.SirketID;
                        Session["SirketAdi"] = sirket.SirketAdi;
                        Session["Email"] = kullanici.Email;
                        
                        System.Diagnostics.Debug.WriteLine("Session yenilendi - SirketID: " + kullanici.SirketID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Session yenileme hatası: " + ex.Message);
            Response.Redirect("~/Mobil/Login.aspx");
        }
    }
}
