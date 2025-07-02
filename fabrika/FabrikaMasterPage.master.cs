using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;

public partial class fabrika_FabrikaMasterPage : System.Web.UI.MasterPage
{
    public string KlasorAdi
    {
        get { return lblKlasörAdi != null ? lblKlasörAdi.Text : string.Empty; }
        set { if (lblKlasörAdi != null) lblKlasörAdi.Text = value; }
    }

    public string SayfaAdi
    {
        get { return lblSayfaAdi != null ? lblSayfaAdi.Text : string.Empty; }
        set { if (lblSayfaAdi != null) lblSayfaAdi.Text = value; }
    }

    //public string SayfaHata
    //{
    //    get { return lblHata.Text; }
    //    set { lblHata.Text = value; }
    //}
    //public string Sayfabilgi
    //{
    //    get { return lblSayfabilgi.Text; }
    //    set { lblSayfabilgi.Text = value; }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (lblHata != null)
        //{
        //    pnlHata.Visible = true; 
        //}

        //if (lblSayfabilgi != null)
        //{
        //    pnlBilgi.Visible = true; 
        //}

        if (!IsPostBack)
        {
            // Önce kullanıcının giriş yapmış olup olmadığını kontrol et
            if (HttpContext.Current.User.Identity.IsAuthenticated && IsValidSession())
            {
                // Geçerli oturum varsa menüleri ve kullanıcı bilgilerini yükle
                try
                {
                    YetkiHelper.TumMenuYetkileriniVer(SessionHelper.GetKullaniciID());
                    MenuleriOlustur();
                    KullaniciBilgileriniYukle();
                }
                catch (Exception ex)
                {
                    // Hata oluşursa oturumu temizle ve giriş sayfasına yönlendir
                    MessageHelper.LogError(ex);
                    ClearSessionAndRedirectToLogin();
                }
            }
            else
            {
                // Kullanıcı giriş yapmamışsa veya geçersiz oturum varsa login sayfasına yönlendir
                ClearSessionAndRedirectToLogin();
            }
        }
        else
        {
            // PostBack durumunda da oturum kontrolü yap
            if (HttpContext.Current.User.Identity.IsAuthenticated && IsValidSession())
            {
                try
                {
                    YetkiHelper.TumMenuYetkileriniVer(SessionHelper.GetKullaniciID());
                }
                catch (Exception ex)
                {
                    MessageHelper.LogError(ex);
                    ClearSessionAndRedirectToLogin();
                }
            }
        }
    }
    
    /// <summary>
    /// Geçerli bir oturumun olup olmadığını kontrol eder
    /// </summary>
    private bool IsValidSession()
    {
        try
        {
            // Session'da temel bilgiler var mı kontrol et
            if (HttpContext.Current.Session != null && 
                HttpContext.Current.Session["KullaniciID"] != null && 
                HttpContext.Current.Session["SirketID"] != null)
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
                        // Cookie'den session'ı yeniden oluştur
                        HttpContext.Current.Session["SirketID"] = Convert.ToInt32(parts[0]);
                        HttpContext.Current.Session["SirketAdi"] = parts[1];
                        HttpContext.Current.Session["KullaniciID"] = Convert.ToInt32(parts[2]);
                        HttpContext.Current.Session["KullaniciAdSoyad"] = parts[3];
                        return true;
                    }
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
            return false;
        }
    }
    
    /// <summary>
    /// Oturumu temizler ve giriş sayfasına yönlendirir
    /// </summary>
    private void ClearSessionAndRedirectToLogin()
    {
        try
        {
            // Session'ı temizle
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
            
            // Authentication çerezini temizle
            FormsAuthentication.SignOut();
            
            // Çerezi manuel olarak da temizle
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);
            
            // Giriş sayfasına yönlendir
            FormsAuthentication.RedirectToLoginPage();
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
            // Hata durumunda da giriş sayfasına yönlendir
            HttpContext.Current.Response.Redirect("~/giris.aspx");
        }
    }

    private void MenuleriOlustur()
    {
        try
        {
            // Mevcut sayfa URL'ini al
            string currentUrl = Request.Url.AbsolutePath;

            // Menüleri oluştur
            var menuler = MenuYonetimi.KullaniciMenuleriniGetir();
            string menuHtml = MenuYonetimi.MenuHtmlOlustur(menuler, currentUrl);

            // URL'leri düzenle - ResolveUrl kullanarak tüm URL'leri düzenle
            menuHtml = DuzenleResolveUrl(menuHtml);

            // Menüleri sayfaya ekle
            ltlMenu.Text = menuHtml;

            // Breadcrumb için sayfa başlığını ayarla
            AyarlaSayfaBilgileri(currentUrl, menuler);
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama yapılabilir
            MessageHelper.LogError(ex);
        }
    }

    /// <summary>
    /// HTML içerisindeki tüm URL'leri ResolveUrl ile düzenler
    /// </summary>
    private string DuzenleResolveUrl(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        // URL formatlarını düzenle
        html = html.Replace("href='~/", "href='" + ResolveUrl("~/"));
        html = html.Replace("href=\"~/", "href=\"" + ResolveUrl("~/"));

        // Olası çift fabrika durumlarını düzelt
        html = html.Replace("href='/fabrika/fabrika/", "href='/fabrika/");
        html = html.Replace("href=\"/fabrika/fabrika/", "href=\"/fabrika/");
        html = html.Replace("href='fabrika/fabrika/", "href='fabrika/");
        html = html.Replace("href=\"fabrika/fabrika/", "href=\"fabrika/");

        return html;
    }

    private void AyarlaSayfaBilgileri(string currentUrl, List<MenuItems> menuler)
    {
        try
        {
            // Aktif menüyü bul
            MenuItems aktifMenu = null;
            MenuItems aktifUstMenu = null;

            // Önce alt menülerde ara
            foreach (var anaMenu in menuler)
            {
                var altMenu = anaMenu.AltMenuler.Find(m =>
                    !string.IsNullOrEmpty(m.SayfaURL) &&
                    currentUrl.EndsWith(m.SayfaURL, StringComparison.OrdinalIgnoreCase));

                if (altMenu != null)
                {
                    aktifMenu = altMenu;
                    aktifUstMenu = anaMenu;
                    break;
                }
            }

            // Alt menülerde bulunamadıysa ana menülerde ara
            if (aktifMenu == null)
            {
                aktifMenu = menuler.Find(m =>
                    !string.IsNullOrEmpty(m.SayfaURL) &&
                    currentUrl.EndsWith(m.SayfaURL, StringComparison.OrdinalIgnoreCase));
            }

            // Breadcrumb'ı ayarla - sadece child page'den değer gelmemişse menü sisteminden al
            if (lblKlasörAdi != null && lblSayfaAdi != null)
            {
                // Eğer child page'den değer gelmemişse (boş veya null ise) menü sisteminden al
                if (string.IsNullOrEmpty(lblKlasörAdi.Text) && string.IsNullOrEmpty(lblSayfaAdi.Text))
                {
                    if (aktifUstMenu != null)
                    {
                        lblKlasörAdi.Text = aktifUstMenu.MenuAdi;
                        lblSayfaAdi.Text = aktifMenu != null ? aktifMenu.MenuAdi : "";
                    }
                    else
                    {
                        lblKlasörAdi.Text = aktifMenu != null ? aktifMenu.MenuAdi : "";
                        lblSayfaAdi.Text = "";
                    }
                }
                // Child page'den değer gelmişse o değerleri koru
            }
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
        }
    }

    private void KullaniciBilgileriniYukle()
    {
        try
        {
            lblKullaniciAdi.Text = SessionHelper.GetKullaniciAdSoyad();
            lblSirketAd.Text = SessionHelper.GetSirketAdi();
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
        }
    }

    /// <summary> Çıkış yapma işlemi
    /// </summary>
    protected void btnCikis_Click(object sender, EventArgs e)
    {
        try
        {
            // Forms Authentication çerezini temizle
            FormsAuthentication.SignOut();

            // Diğer çerezleri temizle
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            // Login sayfasına yönlendir
            FormsAuthentication.RedirectToLoginPage();
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
        }
    }

    /// <summary>
    /// Mevcut URL'ye göre ilgili menü öğesini aktif olarak işaretler
    /// </summary>
    protected void SetActiveMenuItem()
    {
        try
        {
            // Mevcut sayfa URL'sini al
            string currentUrl = Request.Url.AbsolutePath.ToLower();

            // Sidebar menüsünü bul
            HtmlGenericControl sidebarMenu = (HtmlGenericControl)FindControl("nav-accordion");
            if (sidebarMenu == null)
            {
                // ID'yi bulamadıysak CSS sınıfıyla deneme yapalım
                var sidebarMenus = FindControl("sidebar").Controls.OfType<HtmlGenericControl>()
                                 .Where(ul => ul.Attributes["class"] != null &&
                                        ul.Attributes["class"].Contains("sidebar-menu")).FirstOrDefault();

                if (sidebarMenus != null)
                    sidebarMenu = sidebarMenus;
                else
                    return; // Menü bulunamadı
            }

            // Menu öğelerinin her birini kontrol et
            var menuItems = sidebarMenu.Controls.OfType<HtmlGenericControl>()
                            .Where(li => li.TagName.Equals("li", StringComparison.OrdinalIgnoreCase));

            foreach (var menuItem in menuItems)
            {
                // Önce mevcut sınıfı kontrol edelim
                string currentClass = menuItem.Attributes["class"] ?? string.Empty;

                // Alt menü varsa (sub-menu sınıfına sahip mi?)
                bool hasSubMenu = currentClass.Contains("sub-menu");

                if (hasSubMenu)
                {
                    // Alt menü ul etiketini bul
                    var subMenu = menuItem.Controls.OfType<HtmlGenericControl>()
                                    .FirstOrDefault(ul => ul.TagName.Equals("ul", StringComparison.OrdinalIgnoreCase) &&
                                                    ul.Attributes["class"] != null &&
                                                    ul.Attributes["class"].Contains("sub"));

                    if (subMenu != null)
                    {
                        // Alt menüleri kontrol et
                        var subItems = subMenu.Controls.OfType<HtmlGenericControl>()
                                             .Where(li => li.TagName.Equals("li", StringComparison.OrdinalIgnoreCase));

                        bool subMenuActive = false;

                        foreach (var subItem in subItems)
                        {
                            // Alt menü bağlantısını bul
                            var anchor = subItem.Controls.OfType<HtmlAnchor>().FirstOrDefault();
                            if (anchor != null)
                            {
                                // Sayfanın URL'sini normalize et
                                string href = NormalizeUrl(anchor.HRef.ToLower());
                                string normalizedCurrentUrl = NormalizeUrl(currentUrl);

                                if (normalizedCurrentUrl.EndsWith(href))
                                {
                                    // Alt menüyü aktif et
                                    subItem.Attributes["class"] = "active";
                                    subMenuActive = true;
                                }
                            }
                        }

                        // Ana menüyü aktif et (eğer alt menü aktifse)
                        if (subMenuActive)
                        {
                            // Mevcut sınıfı koruyarak active ekleyelim
                            if (!currentClass.Contains("active"))
                            {
                                menuItem.Attributes["class"] = currentClass + " active";
                            }
                        }
                    }
                }
                else
                {
                    // Tek bağlantılı menü öğesi
                    var anchor = menuItem.Controls.OfType<HtmlAnchor>().FirstOrDefault();
                    if (anchor != null)
                    {
                        // Sayfanın URL'sini normalize et
                        string href = NormalizeUrl(anchor.HRef.ToLower());
                        string normalizedCurrentUrl = NormalizeUrl(currentUrl);

                        if (normalizedCurrentUrl.EndsWith(href))
                        {
                            // Menüyü aktif et
                            if (!currentClass.Contains("active"))
                            {
                                menuItem.Attributes["class"] = currentClass + " active";
                            }
                        }
                        else
                        {
                            // active sınıfını kaldır
                            if (currentClass.Contains("active"))
                            {
                                menuItem.Attributes["class"] = currentClass.Replace("active", "").Trim();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda sessizce devam et (UI'yı etkilemesin)
            MessageHelper.LogError(ex);
        }
    }

    /// <summary>
    /// URL'yi normalize eder, tekrarlanan yol segmentlerini kaldırır
    /// </summary>
    private string NormalizeUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "";

        // URL'den "fabrika/fabrika/" gibi tekrarları temizleme
        url = url.Replace("/fabrika/fabrika/", "/fabrika/");
        url = url.Replace("/Fabrika/Fabrika/", "/Fabrika/");

        // Başındaki ve sonundaki '/' karakterini kaldırma
        url = url.Trim('/');

        return url;
    }
}
