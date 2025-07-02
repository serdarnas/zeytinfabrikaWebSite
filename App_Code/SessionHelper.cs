using System;
using System.Web;
using System.Web.Security;

/// <summary>
/// Oturum ve kullanıcı bilgilerini yönetmek için yardımcı sınıf
/// </summary>
public static class SessionHelper
{
    /// <summary>
    /// Mevcut kullanıcının şirket ID'sini döndürür.
    /// Önce Session'dan kontrol eder, yoksa Forms Authentication çerezinden alır.
    /// </summary>
    /// <returns>Şirket ID değeri</returns>
    public static int GetSirketID()
    {
        // Session'dan kontrol et
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["SirketID"] != null)
        {
            return Convert.ToInt32(HttpContext.Current.Session["SirketID"]);
        }
        
        // Session'da yoksa çerezden al
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie != null)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var parts = ticket.UserData.Split('|');
                    if (parts.Length > 0)
                    {
                        int sirketID = Convert.ToInt32(parts[0]);
                        // Session'a da kaydet
                        if (HttpContext.Current.Session != null)
                            HttpContext.Current.Session["SirketID"] = sirketID;
                        return sirketID;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.LogError(ex);
            }
        }
        
        // Cookie bulunamazsa veya hata oluşursa giriş sayfasına yönlendir
        FormsAuthentication.RedirectToLoginPage();
        return 0;
    }

    /// <summary>
    /// Mevcut kullanıcının şirket ID'sini ayarlar.
    /// </summary>
    /// <param name="sirketID">Şirket ID değeri</param>
    public static void SetSirketID(int sirketID)
    {
        HttpContext.Current.Session["SirketID"] = sirketID;
    }

    /// <summary>
    /// Mevcut kullanıcının şirket adını döndürür.
    /// Önce Session'dan kontrol eder, yoksa Forms Authentication çerezinden alır.
    /// </summary>
    /// <returns>Şirket adı</returns>
    public static string GetSirketAdi()
    {
        // Session'dan kontrol et
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["SirketAdi"] != null)
        {
            return HttpContext.Current.Session["SirketAdi"].ToString();
        }
        
        // Session'da yoksa çerezden al
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie != null)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var parts = ticket.UserData.Split('|');
                    if (parts.Length > 1)
                    {
                        string sirketAdi = parts[1];
                        // Session'a da kaydet
                        if (HttpContext.Current.Session != null)
                            HttpContext.Current.Session["SirketAdi"] = sirketAdi;
                        return sirketAdi;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.LogError(ex);
            }
        }
        
        // Cookie bulunamazsa veya hata oluşursa giriş sayfasına yönlendir
        FormsAuthentication.RedirectToLoginPage();
        return string.Empty;
    }

    /// <summary>
    /// Mevcut kullanıcının ID'sini döndürür.
    /// Önce Session'dan kontrol eder, yoksa Forms Authentication çerezinden alır.
    /// </summary>
    /// <returns>Kullanıcı ID değeri</returns>
    public static int GetKullaniciID()
    {
        // Session'dan kontrol et
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["KullaniciID"] != null)
        {
            return Convert.ToInt32(HttpContext.Current.Session["KullaniciID"]);
        }
        
        // Session'da yoksa çerezden al
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie != null)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var parts = ticket.UserData.Split('|');
                    if (parts.Length > 2)
                    {
                        int kullaniciID = Convert.ToInt32(parts[2]);
                        // Session'a da kaydet
                        if (HttpContext.Current.Session != null)
                            HttpContext.Current.Session["KullaniciID"] = kullaniciID;
                        return kullaniciID;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.LogError(ex);
            }
        }
        
        // Cookie bulunamazsa veya hata oluşursa giriş sayfasına yönlendir
        FormsAuthentication.RedirectToLoginPage();
        return 0;
    }

    /// <summary>
    /// Mevcut kullanıcının adını ve soyadını döndürür.
    /// Önce Session'dan kontrol eder, yoksa Forms Authentication çerezinden alır.
    /// </summary>
    /// <returns>Kullanıcı adı ve soyadı</returns>
    public static string GetKullaniciAdSoyad()
    {
        // Session'dan kontrol et
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["KullaniciAdSoyad"] != null)
        {
            return HttpContext.Current.Session["KullaniciAdSoyad"].ToString();
        }
        
        // Session'da yoksa çerezden al
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie != null)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var parts = ticket.UserData.Split('|');
                    if (parts.Length > 3)
                    {
                        string adSoyad = parts[3];
                        // Session'a da kaydet
                        if (HttpContext.Current.Session != null)
                            HttpContext.Current.Session["KullaniciAdSoyad"] = adSoyad;
                        return adSoyad;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.LogError(ex);
            }
        }
        
        return string.Empty;
    }
    
    /// <summary>
    /// Cookie bilgilerinin geçerli olup olmadığını kontrol eder.
    /// Geçersizse session temizler ve giriş sayfasına yönlendirir.
    /// </summary>
    public static void KullaniciOturumKontrol()
    {
        if (HttpContext.Current == null || HttpContext.Current.Session == null)
            return;
            
        // Çerez kontrolü
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie == null)
        {
            // Cookie yok, demek ki oturum düşmüş
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.RedirectToLoginPage();
            return;
        }
        
        try
        {
            // Çerez şifresini çöz
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null || string.IsNullOrEmpty(ticket.UserData) || ticket.Expired)
            {
                // Cookie var ama geçersiz ya da süresi dolmuş
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            
            // Çerez geçerli, session bilgilerini kontrol et ve gerekirse güncelle
            var parts = ticket.UserData.Split('|');
            if (parts.Length >= 4)
            {
                // Session'da değerleri güncelle
                if (HttpContext.Current.Session["SirketID"] == null)
                    HttpContext.Current.Session["SirketID"] = Convert.ToInt32(parts[0]);
                    
                if (HttpContext.Current.Session["SirketAdi"] == null)
                    HttpContext.Current.Session["SirketAdi"] = parts[1];
                    
                if (HttpContext.Current.Session["KullaniciID"] == null)
                    HttpContext.Current.Session["KullaniciID"] = Convert.ToInt32(parts[2]);
                    
                if (HttpContext.Current.Session["KullaniciAdSoyad"] == null)
                    HttpContext.Current.Session["KullaniciAdSoyad"] = parts[3];
            }
        }
        catch (Exception ex)
        {
            MessageHelper.LogError(ex);
            // Hata olursa session temizle ve giriş sayfasına yönlendir
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}
