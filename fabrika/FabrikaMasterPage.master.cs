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
        get { return lblKlasörAdi.Text; }set { lblKlasörAdi.Text = value; }
    }

    public string SayfaAdi
    {
        get { return lblSayfaAdi.Text; }
        set { lblSayfaAdi.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        YetkiHelper.TumMenuYetkileriniVer(SessionHelper.GetKullaniciID());
        if (!IsPostBack)
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                MenuleriOlustur();
                KullaniciBilgileriniYukle();
            }
            else
            {
                // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
                FormsAuthentication.RedirectToLoginPage();
            }
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
            System.Diagnostics.Debug.WriteLine("Menü oluşturma hatası: " + ex.Message);
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

            // Breadcrumb'ı ayarla
            if (lblKlasörAdi != null && lblSayfaAdi != null)
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
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Sayfa bilgileri ayarlama hatası: " + ex.Message);
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
            System.Diagnostics.Debug.WriteLine("Kullanıcı bilgileri yükleme hatası: " + ex.Message);
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
            System.Diagnostics.Debug.WriteLine("Çıkış yapma hatası: " + ex.Message);
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
            // Hata durumunda sessizce devam et
            System.Diagnostics.Debug.WriteLine("SetActiveMenuItem Hatası: " + ex.Message);
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
