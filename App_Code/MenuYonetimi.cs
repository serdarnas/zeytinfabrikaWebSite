using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;

public class MenuItems
{
    public int MenuID { get; set; }
    public int? UstMenuID { get; set; }
    public string MenuAdi { get; set; }
    public string Ikon { get; set; }
    public string SayfaURL { get; set; }
    public int Sira { get; set; }
    public string YetkiKodu { get; set; }
    public List<MenuItems> AltMenuler { get; set; }
}

public static class MenuYonetimi
{
    private static readonly string CacheKey = "KullaniciMenu_{0}";

    public static List<MenuItems> KullaniciMenuleriniGetir()
    {
        try
        {
            // Debug mesajı
            System.Diagnostics.Debug.WriteLine("KullaniciMenuleriniGetir metodu çağrıldı");
            
            int kullaniciId = GetKullaniciIdFromCookie();
            System.Diagnostics.Debug.WriteLine("Kullanıcı ID: " + kullaniciId);

            // Kullanıcı ID 0 ise hata var demektir
            if (kullaniciId == 0)
            {
                System.Diagnostics.Debug.WriteLine("HATA: Kullanıcı ID alınamadı!");
                return new List<MenuItems>(); // Boş liste döndür
            }
            
            string key = string.Format(CacheKey, kullaniciId);

            if (HttpRuntime.Cache[key] != null)
            {
                System.Diagnostics.Debug.WriteLine("Cache'den menüler alındı");
                return (List<MenuItems>)HttpRuntime.Cache[key];
            }

            var menuler = new List<MenuItems>();

            try 
            {
                System.Diagnostics.Debug.WriteLine("Veritabanı bağlantısı kuruluyor...");
                string baglanti = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
                System.Diagnostics.Debug.WriteLine("Connection string: " + baglanti);
                
                using (SqlConnection conn = new SqlConnection(baglanti))
                {
                    string sql = @"SELECT DISTINCT m.* 
                                  FROM Menu m
                                  INNER JOIN KullaniciYetki ky ON m.YetkiKodu = ky.YetkiKodu
                                  WHERE ky.KullaniciID = @KullaniciID 
                                  AND ky.Aktif = 1";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@KullaniciID", kullaniciId);
                        conn.Open();
                        System.Diagnostics.Debug.WriteLine("Veritabanı bağlantısı açıldı");

                        var tumMenuler = new List<MenuItems>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int menuCount = 0;
                            while (reader.Read())
                            {
                                menuCount++;
                                tumMenuler.Add(new MenuItems
                                {
                                    MenuID = Convert.ToInt32(reader["MenuID"]),
                                    UstMenuID = reader["UstMenuID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["UstMenuID"]) : null,
                                    MenuAdi = reader["MenuAdi"].ToString(),
                                    Ikon = reader["Ikon"].ToString(),
                                    SayfaURL = reader["SayfaURL"].ToString(),
                                    Sira = Convert.ToInt32(reader["Sira"]),
                                    YetkiKodu = reader["YetkiKodu"].ToString(),
                                    AltMenuler = new List<MenuItems>()
                                });
                            }
                            System.Diagnostics.Debug.WriteLine("Toplam menü sayısı: " + menuCount);
                        }

                        // Sıralı ana menüler
                        menuler = tumMenuler
                            .Where(m => m.UstMenuID == null)
                            .OrderBy(m => m.Sira)
                            .ToList();

                        System.Diagnostics.Debug.WriteLine("Ana menü sayısı: " + menuler.Count);

                        // Sıralı alt menüler
                        foreach (var menu in menuler)
                        {
                            menu.AltMenuler = tumMenuler
                                .Where(m => m.UstMenuID == menu.MenuID)
                                .OrderBy(m => m.Sira)
                                .ToList();
                                
                            System.Diagnostics.Debug.WriteLine(menu.MenuAdi + " menüsü altında " + 
                                menu.AltMenuler.Count + " alt menü var");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Veritabanı hatası: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
                return new List<MenuItems>(); // Hata durumunda boş liste döndür
            }

            if (menuler.Count > 0)
            {
                HttpRuntime.Cache.Insert(key, menuler, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration);
                System.Diagnostics.Debug.WriteLine("Menüler cache'e eklendi");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("UYARI: Menü bulunamadı!");
            }
            
            return menuler;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Genel hata: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
            return new List<MenuItems>();
        }
    }

    private static int GetKullaniciIdFromCookie()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("GetKullaniciIdFromCookie metodu çağrıldı");
            
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                System.Diagnostics.Debug.WriteLine("Auth cookie bulundu: " + authCookie.Value);
                
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Ticket içeriği: " + ticket.UserData);
                        
                        if (!string.IsNullOrEmpty(ticket.UserData))
                        {
                            var parts = ticket.UserData.Split('|');
                            if (parts.Length > 0)
                            {
                                int kullaniciId = Convert.ToInt32(parts[0]);
                                System.Diagnostics.Debug.WriteLine("Kullanıcı ID alındı: " + kullaniciId);
                                return kullaniciId;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("HATA: Ticket UserData boş!");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("HATA: Ticket null!");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Ticket çözme hatası: " + ex.Message);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("HATA: Auth cookie bulunamadı!");
            }
            
            // Hata durumunda login sayfasına yönlendirme yapmak istiyorsak:
            // FormsAuthentication.RedirectToLoginPage();
            
            // Test için geçici olarak sabit bir kullanıcı ID döndürelim - SADECE TEST İÇİN!
            System.Diagnostics.Debug.WriteLine("TEST İÇİN: Sabit kullanıcı ID 1 döndürülüyor");
            return 1;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Genel hata: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
            return 0;
        }
    }

    public static string MenuHtmlOlustur(List<MenuItems> menuler, string currentUrl)
    {
        StringBuilder menuHtml = new StringBuilder();
        
        // Tam yol gerekli
        if (!string.IsNullOrEmpty(currentUrl) && !currentUrl.StartsWith("/"))
            currentUrl = "/" + currentUrl;
            
        // Debug için URL bilgisi
        System.Diagnostics.Debug.WriteLine("Mevcut URL: " + currentUrl);

        foreach (var menu in menuler)
        {
            // Menünün URL'si ile şu anki URL karşılaştırması
            bool aktifAnaMenu = false;
            
            // Ana menü aktif kontrolü
            if (!string.IsNullOrEmpty(menu.SayfaURL) && !string.IsNullOrEmpty(currentUrl))
            {
                string menuUrl = menu.SayfaURL;
                if (!string.IsNullOrEmpty(menuUrl) && !menuUrl.StartsWith("javascript:"))
                {
                    if (!menuUrl.StartsWith("/") && !menuUrl.StartsWith("~/"))
                        menuUrl = "/" + menuUrl;
                }
                
                if (menuUrl.StartsWith("~/"))
                    menuUrl = menuUrl.Substring(2);
                if (!menuUrl.StartsWith("/") && !menuUrl.StartsWith("javascript:"))
                    menuUrl = "/" + menuUrl;
                    
                aktifAnaMenu = currentUrl.EndsWith(menuUrl, StringComparison.OrdinalIgnoreCase) || 
                               currentUrl.Equals(menuUrl, StringComparison.OrdinalIgnoreCase);
                               
                System.Diagnostics.Debug.WriteLine("Ana menü URL karşılaştırma: '" + menuUrl + "' ile '" + currentUrl + "', sonuç: " + aktifAnaMenu);
            }
            
            // Alt menülerden biri aktif mi?
            if (!aktifAnaMenu && menu.AltMenuler.Count > 0)
            {
                foreach (var altMenu in menu.AltMenuler)
                {
                    if (!string.IsNullOrEmpty(altMenu.SayfaURL) && !string.IsNullOrEmpty(currentUrl))
                    {
                        string altMenuUrl = altMenu.SayfaURL;
                        if (!string.IsNullOrEmpty(altMenuUrl) && !altMenuUrl.StartsWith("javascript:"))
                        {
                            if (!altMenuUrl.StartsWith("/") && !altMenuUrl.StartsWith("~/"))
                                altMenuUrl = "/" + altMenuUrl;
                        }
                        
                        if (altMenuUrl.StartsWith("~/"))
                            altMenuUrl = altMenuUrl.Substring(2);
                        if (!altMenuUrl.StartsWith("/") && !altMenuUrl.StartsWith("javascript:"))
                            altMenuUrl = "/" + altMenuUrl;
                            
                        bool aktifAltMenu = currentUrl.EndsWith(altMenuUrl, StringComparison.OrdinalIgnoreCase) || 
                                           currentUrl.Equals(altMenuUrl, StringComparison.OrdinalIgnoreCase);
                                           
                        System.Diagnostics.Debug.WriteLine("Alt menü URL karşılaştırma: '" + altMenuUrl + "' ile '" + currentUrl + "', sonuç: " + aktifAltMenu);
                        
                        if (aktifAltMenu)
                        {
                            aktifAnaMenu = true;
                            break;
                        }
                    }
                }
            }

            if (menu.AltMenuler.Count > 0)
            {
                menuHtml.AppendFormat(@"
                    <li class='sub-menu{0}'>
                        <a href='javascript:;'{1}>
                            <i class='{2}'></i>
                            <span>{3}</span>
                        </a>
                        <ul class='sub'>",
                    aktifAnaMenu ? " active" : "",
                    aktifAnaMenu ? " class='active'" : "",
                    !string.IsNullOrEmpty(menu.Ikon) ? menu.Ikon : "fa fa-list", menu.MenuAdi);

                foreach (var altMenu in menu.AltMenuler)
                {
                    // Alt menü aktif kontrolü
                    bool aktifAltMenu = false;
                    
                    if (!string.IsNullOrEmpty(altMenu.SayfaURL) && !string.IsNullOrEmpty(currentUrl))
                    {
                        string altMenuUrl = altMenu.SayfaURL;
                        if (!string.IsNullOrEmpty(altMenuUrl) && !altMenuUrl.StartsWith("javascript:"))
                        {
                            if (!altMenuUrl.StartsWith("/") && !altMenuUrl.StartsWith("~/"))
                                altMenuUrl = "/" + altMenuUrl;
                        }
                        
                        if (altMenuUrl.StartsWith("~/"))
                            altMenuUrl = altMenuUrl.Substring(2);
                        if (!altMenuUrl.StartsWith("/") && !altMenuUrl.StartsWith("javascript:"))
                            altMenuUrl = "/" + altMenuUrl;
                            
                        aktifAltMenu = currentUrl.EndsWith(altMenuUrl, StringComparison.OrdinalIgnoreCase) || 
                                      currentUrl.Equals(altMenuUrl, StringComparison.OrdinalIgnoreCase);
                    }

                    // URL oluştur
                    string url = DuzeltUrl(altMenu.SayfaURL);

                    menuHtml.AppendFormat(@"
                        <li{0}><a href='{1}'{2}>{3}</a></li>",
                        aktifAltMenu ? " class='active'" : "",
                        url,
                        aktifAltMenu ? " class='active'" : "",
                        altMenu.MenuAdi);
                }

                menuHtml.Append("</ul></li>");
            }
            else
            {
                // URL oluştur
                string url = DuzeltUrl(menu.SayfaURL);

                menuHtml.AppendFormat(@"
                    <li{0}>
                        <a href='{1}'{2}>
                            <i class='{3}'></i>
                            <span>{4}</span>
                        </a>
                    </li>",
                    aktifAnaMenu ? " class='active'" : "",
                    !string.IsNullOrEmpty(url) ? url : "javascript:;",
                    aktifAnaMenu ? " class='active'" : "",
                    !string.IsNullOrEmpty(menu.Ikon) ? menu.Ikon : "fa fa-list", menu.MenuAdi);
            }
        }

        return menuHtml.ToString();
    }

    /// <summary>
    /// URL'yi düzeltir: ~/fabrika/ veya fabrika/fabrika/ gibi önekleri temizler
    /// </summary>
    private static string DuzeltUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "javascript:;";

        // JavaScript link ise olduğu gibi döndür
        if (url.StartsWith("javascript:"))
            return url;

        // Uygulama kök dizininden başlatan bir URL için
        if (!url.StartsWith("~/") && !url.StartsWith("/"))
            url = "~/" + url;
        
        // Eğer zaten ~/ ile başlıyorsa olduğu gibi bırak
        if (url.StartsWith("~/"))
            return url;
            
        return url;
    }
} 