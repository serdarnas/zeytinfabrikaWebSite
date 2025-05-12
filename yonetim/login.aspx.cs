using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class yonetim_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Eğer kullanıcı zaten giriş yaptıysa, yönetim Default.aspx'e yönlendir
        if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/yonetim/Default.aspx");
        }
        
        // Çıkış parametresi kontrol
        if (Request.QueryString["cikis"] != null)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            
            // Çerezleri temizle
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);
            
            Response.Redirect("~/yonetim/login.aspx");
        }
    }

    protected void btnGiris_Click(object sender, EventArgs e)
    {
        try
        {
            // Form doğrulama
            if (string.IsNullOrEmpty(txtKullaniciAdi.Text) || string.IsNullOrEmpty(txtSifre.Text))
            {
                pnlHata.Visible = true;
                lblHata.Text = "Kullanıcı adı ve şifre giriniz.";
                return;
            }
            
            // Admin kullanıcı doğrulama (statik)
            if (txtKullaniciAdi.Text == "serdarnas" && txtSifre.Text == "SerdarNas56.")
            {
                // Kalıcı oturum için çerez oluştur
                bool rememberMe = chkBeniHatirla.Checked;
                
                // UserData içeriğini oluştur: SirketID|SirketAdi|KullaniciID|AdSoyad
                string userData = string.Format(
                    "{0}|{1}|{2}|{3}", 
                    1, // Admin SirketID
                    "Admin Şirketi",
                    1, // Admin KullaniciID
                    "Administrator"
                );
                
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // Versiyon
                    txtKullaniciAdi.Text,           // Kullanıcı adı
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
                Session["KullaniciID"] = 1;
                Session["KullaniciAdSoyad"] = "Administrator";
                Session["SirketID"] = 1;
                Session["SirketAdi"] = "Admin Şirketi";
                Session["Email"] = "admin@zeytin.com";

                // Kullanıcıyı yönetim/Default.aspx sayfasına yönlendir
                Response.Redirect("~/yonetim/Default.aspx");
                return;
            }
            else
            {
                // Statik admin doğrulaması başarısız, veritabanı kontrolü yap
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                Kullanicilar kayitli = db.Kullanicilars.FirstOrDefault(x => x.Email == txtKullaniciAdi.Text && x.Sifre == txtSifre.Text);
                
                if (kayitli != null)
                {
                    // Şirket bilgilerini al
                    Sirketler gelenSirketler = db.Sirketlers.FirstOrDefault(x => x.SirketID == kayitli.SirketID);
                    if (gelenSirketler == null)
                    {
                        pnlHata.Visible = true;
                        lblHata.Text = "Şirket bilgisi bulunamadı.";
                        return;
                    }
                    
                    // Kalıcı oturum için çerez oluştur
                    bool rememberMe = chkBeniHatirla.Checked;
                    
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

                    // Kullanıcıyı yönetim/Default.aspx sayfasına yönlendir
                    Response.Redirect("~/yonetim/Default.aspx");
                    return;
                }
                else
                {
                    pnlHata.Visible = true;
                    lblHata.Text = "Kullanıcı adı veya şifre hatalı.";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log tutabilirsiniz
            pnlHata.Visible = true;
            lblHata.Text = "Giriş yapılırken bir hata oluştu: " + ex.Message;
        }
    }

    protected void btnSifreSifirla_Click(object sender, EventArgs e)
    {
        string _Email = txtSifreSifirlaEmail.Text;
        
        if (string.IsNullOrEmpty(_Email))
        {
            lblSifreSifirlaMesaj.Text = "Lütfen e-posta adresinizi giriniz.";
            lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Red;
            return;
        }
        
        try
        {
            // Admin kontrolü
            if (_Email.ToLower() == "serdarnas@admin.com")
            {
                // Şifre sıfırlama e-postası gönder (Burada gerçek bir gönderim kodu yerine simülasyon yapılıyor)
                lblSifreSifirlaMesaj.Text = "Admin hesabı için şifre sıfırlama e-postası gönderildi.";
                lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Green;
                return;
            }
            
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            Kullanicilar istennenKullanicilar = db.Kullanicilars.FirstOrDefault(x => x.Email == _Email);
            if (istennenKullanicilar != null)
            {
                // Kullanıcı kayıtlı, şifre sıfırlama e-postası gönder
                EmailHelper.SendForgotPasswordMail(_Email, istennenKullanicilar.AdSoyad, istennenKullanicilar.Sifre);
                lblSifreSifirlaMesaj.Text = "Şifre sıfırlama e-postası gönderildi.";
                lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                // Kullanıcı kayıtlı değil
                lblSifreSifirlaMesaj.Text = "Bu e-posta adresi ile kayıtlı bir kullanıcı bulunamadı.";
                lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblSifreSifirlaMesaj.Text = "Şifre sıfırlama işlemi sırasında bir hata oluştu: " + ex.Message;
            lblSifreSifirlaMesaj.ForeColor = System.Drawing.Color.Red;
        }
    }
}