using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class giris : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Çıkış parametresi önce kontrol edilmeli
        if (Request.QueryString["cikis"] != null)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            
            // Çerezleri temizle
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);
            
            Response.Redirect("~/giris.aspx");
            return;
        }
        
        // Eğer kullanıcı zaten geçerli bir oturum ile giriş yaptıysa, fabrika/Default.aspx'e yönlendir
        if (IsUserAuthenticated())
        {
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }
    
    /// <summary>
    /// Kullanıcının gerçekten geçerli bir oturumunun olup olmadığını kontrol eder
    /// </summary>
    private bool IsUserAuthenticated()
    {
        try
        {
            // Önce Forms Authentication kontrolü
            if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return false;
            }
            
            // Session kontrolü
            if (Session["KullaniciID"] != null && Session["SirketID"] != null)
            {
                return true;
            }
            
            // Session'da yoksa cookie'den kontrol et
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var parts = ticket.UserData.Split('|');
                    if (parts.Length >= 4)
                    {
                        // Geçerli cookie varsa Session'ı yeniden oluştur
                        Session["SirketID"] = Convert.ToInt32(parts[0]);
                        Session["SirketAdi"] = parts[1];
                        Session["KullaniciID"] = Convert.ToInt32(parts[2]);
                        Session["KullaniciAdSoyad"] = parts[3];
                        return true;
                    }
                }
            }
            
            // Hiçbir geçerli oturum bulunamadı
            return false;
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
            return false;
        }
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            Kullanicilar kayitli = db.Kullanicilars.FirstOrDefault(x => x.Email == Login1.UserName && x.Sifre == Login1.Password);
            
            if (kayitli != null)
            {
                // Şirket bilgilerini al
                Sirketler gelenSirketler = db.Sirketlers.FirstOrDefault(x => x.SirketID == kayitli.SirketID);
                if (gelenSirketler == null)
                {
                    e.Authenticated = false;
                    return;
                }
                
                // Kalıcı oturum için çerez oluştur (30 gün süreyle)
                bool rememberMe = Login1.RememberMeSet;
                
                // UserData içeriğini oluştur: SirketID|SirketAdi|KullaniciID|AdSoyad
                string userData = string.Format(
                    "{0}|{1}|{2}|{3}", 
                    kayitli.SirketID, 
                    gelenSirketler.SirketAdi,
                    kayitli.KullaniciID,
                    kayitli.AdSoyad
                );
                
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // Versiyon
                    kayitli.Email,                  // Kullanıcı adı
                    DateTime.Now,                   // Oluşturma zamanı
                    DateTime.Now.AddDays(30),       // Son geçerlilik zamanı (30 gün)
                    rememberMe,                     // Kalıcı çerez mi?
                    userData,                       // Kullanıcı verisi (UserData)
                    FormsAuthentication.FormsCookiePath);

                // Çerezi şifrele
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Çerezi oluştur
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                if (rememberMe)
                {
                    cookie.Expires = ticket.Expiration;
                }
                cookie.Path = FormsAuthentication.FormsCookiePath;
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.HttpOnly = true; // JavaScript erişimini engelle (güvenlik için)

                // Çerezi yanıta ekle
                Response.Cookies.Add(cookie);
                
                // Session'a kullanıcı bilgilerini kaydet
                Session["KullaniciID"] = kayitli.KullaniciID;
                Session["KullaniciAdSoyad"] = kayitli.AdSoyad;
                Session["SirketID"] = kayitli.SirketID;
                Session["SirketAdi"] = gelenSirketler.SirketAdi;
                Session["Email"] = kayitli.Email;

                // Kullanıcıyı fabrika/Default.aspx sayfasına yönlendir
                Response.Redirect("~/fabrika/Default.aspx");

                e.Authenticated = true;
            }
            else
            {
                e.Authenticated = false;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log tutabilirsiniz
            MessageHelper.LogError(ex);
            e.Authenticated = false;
        }
    }

    protected void btnSifreSifirla_Click(object sender, EventArgs e)
    {
        string _Email = txtSifreSifirlaEmail.Text;
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        Kullanicilar istennenKullanicilar = db.Kullanicilars.FirstOrDefault(x => x.Email == _Email);
        if (istennenKullanicilar != null)
        {
            //kullanici Kayitli
            EmailHelper.SendForgotPasswordMail(txtSifreSifirlaEmail.Text, istennenKullanicilar.AdSoyad, istennenKullanicilar.Sifre);
            // txtSifreSifirlaEmail
            lblMesaj.Text = lblSifreSifirlaMesaj.Text = "Şifre sıfırlama e-postası gönderildi.";
            lblMesaj.ForeColor = lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            //kullanici Kayitli Değil
            lblMesaj.Text = lblSifreSifirlaMesaj.Text = "Bu e-posta adresi ile kayıtlı bir kullanıcı bulunamadı.";
            lblMesaj.ForeColor = lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Red;
        }
    }
}