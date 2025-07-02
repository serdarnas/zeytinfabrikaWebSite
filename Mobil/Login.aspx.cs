using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mobil_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Eğer session varsa yönlendir (Authentication cookie sorunlu olabilir)
            if (Session["SirketID"] != null)
            {
                Response.Redirect("~/Mobil/Default.aspx");
            }
            
            // Çıkış parametresi kontrolü
            if (Request.QueryString["cikis"] != null)
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
        }
    }

    protected void btnGiris_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            // Basit validation
            if (string.IsNullOrEmpty(email))
            {
                HataGoster("E-posta adresinizi girin!");
                return;
            }

            if (string.IsNullOrEmpty(sifre))
            {
                HataGoster("Şifrenizi girin!");
                return;
            }

            // Veritabanında kullanıcıyı ara
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            var kullanici = db.Kullanicilars.FirstOrDefault(x => x.Email == email && x.Sifre == sifre);

            if (kullanici != null)
            {
                // Şirket bilgisini al
                var sirket = db.Sirketlers.FirstOrDefault(x => x.SirketID == kullanici.SirketID);
                
                if (sirket != null)
                {
                    // Önce Forms Authentication'ı ayarla
                    FormsAuthentication.SetAuthCookie(email, chkBeniHatirla.Checked);
                    
                    // Session'a kullanıcı bilgilerini kaydet
                    Session["KullaniciID"] = kullanici.KullaniciID;
                    Session["KullaniciAdSoyad"] = kullanici.AdSoyad;
                    Session["SirketID"] = kullanici.SirketID;
                    Session["SirketAdi"] = sirket.SirketAdi;
                    Session["Email"] = kullanici.Email;

                    // Debug için
                    System.Diagnostics.Debug.WriteLine("Session oluşturuldu - SirketID: " + kullanici.SirketID);
                    System.Diagnostics.Debug.WriteLine("Authentication cookie set edildi - Email: " + email);

                    // Ana sayfaya yönlendir (Response.Redirect kullan ki cookie düzgün gönderilsin)
                    Response.Redirect("~/Mobil/Default.aspx");
                }
                else
                {
                    HataGoster("Şirket bilgisi bulunamadı!");
                }
            }
            else
            {
                HataGoster("E-posta veya şifre hatalı!");
            }
        }
        catch (Exception ex)
        {
            HataGoster("Giriş sırasında hata oluştu: " + ex.Message);
        }
    }

    private void HataGoster(string mesaj)
    {
        lblHata.Text = mesaj;
        pnlHata.Visible = true;
    }
}