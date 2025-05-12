using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Menü yükleme sınıfı
/// </summary>
public class MenuYukleme
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
    
    /// <summary>
    /// Kullanıcı için menü HTML'i oluşturur
    /// </summary>
    /// <param name="kullaniciID">Kullanıcı ID</param>
    /// <returns>Menü HTML</returns>
    public string MenuleriYukle(int kullaniciID)
    {
        DataTable menuler = GetMenulerByKullaniciID(kullaniciID);
        StringBuilder sb = new StringBuilder();
        
        // Ana menü container'ı
        sb.Append("<ul class=\"sidebar-menu\" id=\"nav-accordion\">");
        
        // Ana menüleri oluştur
        DataRow[] anaMenuler = menuler.Select("UstMenuID IS NULL", "Sira ASC");
        
        foreach (DataRow anaMenu in anaMenuler)
        {
            int menuID = Convert.ToInt32(anaMenu["MenuID"]);
            string menuAdi = anaMenu["MenuAdi"].ToString();
            string ikon = anaMenu["Ikon"].ToString();
            string sayfaURL = anaMenu["SayfaURL"].ToString();
            
            // Alt menü var mı kontrol et
            DataRow[] altMenuler = menuler.Select("UstMenuID = " + menuID, "Sira ASC");
            bool hasSubMenu = altMenuler.Length > 0;
            
            // Ana menü li başlangıcı
            if (hasSubMenu)
            {
                sb.Append("<li class=\"sub-menu\">");
            }
            else
            {
                sb.Append("<li>");
            }
            
            // Menü link
            if (sayfaURL.Contains("javascript"))
            {
                sb.AppendFormat("<a href=\"{0}\">", sayfaURL);
            }
            else
            {
                if (HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains(sayfaURL.ToLower()))
                {
                    sb.AppendFormat("<a class=\"active\" href=\"{0}\">", VirtualPathUtility.ToAbsolute(sayfaURL));
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\">", VirtualPathUtility.ToAbsolute(sayfaURL));
                }
            }
            
            // İkon ve başlık
            sb.AppendFormat("<i class=\"{0}\"></i>", ikon);
            sb.AppendFormat("<span>{0}</span>", menuAdi);
            sb.Append("</a>");
            
            // Alt menü varsa ekle
            if (hasSubMenu)
            {
                sb.Append("<ul class=\"sub\">");
                
                foreach (DataRow altMenu in altMenuler)
                {
                    string altMenuAdi = altMenu["MenuAdi"].ToString();
                    string altSayfaURL = altMenu["SayfaURL"].ToString();
                    
                    sb.Append("<li>");
                    sb.AppendFormat("<a href=\"{0}\">{1}</a>", altSayfaURL, altMenuAdi);
                    sb.Append("</li>");
                }
                
                sb.Append("</ul>");
            }
            
            sb.Append("</li>");
        }
        
        sb.Append("</ul>");
        return sb.ToString();
    }
    
    /// <summary>
    /// Kullanıcıya ait menüleri veritabanından çeker
    /// </summary>
    private DataTable GetMenulerByKullaniciID(int kullaniciID)
    {
        DataTable dt = new DataTable();
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM [Menu] " +
                                           "WHERE [MenuID] IN (" +
                                           "SELECT DISTINCT m.[MenuID] FROM [Menu] m " +
                                           "INNER JOIN [KullaniciYetki] ky ON ky.[YetkiID] = m.[YetkiKodu] " +
                                           "WHERE ky.[KullaniciID] = @KullaniciID) " +
                                           "ORDER BY [UstMenuID], [Sira]", conn);
            
            cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            conn.Open();
            da.Fill(dt);
        }
        
        return dt;
    }
    
    /// <summary>
    /// Alternatif olarak tüm menüleri çeken yöntem (Admin için)
    /// </summary>
    public string TumMenuleriYukle()
    {
        DataTable menuler = GetTumMenuler();
        StringBuilder sb = new StringBuilder();
        
        // Ana menü container'ı
        sb.Append("<ul class=\"sidebar-menu\" id=\"nav-accordion\">");
        
        // Ana menüleri oluştur
        DataRow[] anaMenuler = menuler.Select("UstMenuID IS NULL", "Sira ASC");
        
        foreach (DataRow anaMenu in anaMenuler)
        {
            int menuID = Convert.ToInt32(anaMenu["MenuID"]);
            string menuAdi = anaMenu["MenuAdi"].ToString();
            string ikon = anaMenu["Ikon"].ToString();
            string sayfaURL = anaMenu["SayfaURL"].ToString();
            
            // Alt menü var mı kontrol et
            DataRow[] altMenuler = menuler.Select("UstMenuID = " + menuID, "Sira ASC");
            bool hasSubMenu = altMenuler.Length > 0;
            
            // Ana menü li başlangıcı
            if (hasSubMenu)
            {
                sb.Append("<li class=\"sub-menu\">");
            }
            else
            {
                sb.Append("<li>");
            }
            
            // Menü link
            if (sayfaURL.Contains("javascript"))
            {
                sb.AppendFormat("<a href=\"{0}\">", sayfaURL);
            }
            else
            {
                if (HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains(sayfaURL.ToLower()))
                {
                    sb.AppendFormat("<a class=\"active\" href=\"{0}\">", VirtualPathUtility.ToAbsolute(sayfaURL));
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\">", VirtualPathUtility.ToAbsolute(sayfaURL));
                }
            }
            
            // İkon ve başlık
            sb.AppendFormat("<i class=\"{0}\"></i>", ikon);
            sb.AppendFormat("<span>{0}</span>", menuAdi);
            sb.Append("</a>");
            
            // Alt menü varsa ekle
            if (hasSubMenu)
            {
                sb.Append("<ul class=\"sub\">");
                
                foreach (DataRow altMenu in altMenuler)
                {
                    string altMenuAdi = altMenu["MenuAdi"].ToString();
                    string altSayfaURL = altMenu["SayfaURL"].ToString();
                    
                    sb.Append("<li>");
                    sb.AppendFormat("<a href=\"{0}\">{1}</a>", altSayfaURL, altMenuAdi);
                    sb.Append("</li>");
                }
                
                sb.Append("</ul>");
            }
            
            sb.Append("</li>");
        }
        
        sb.Append("</ul>");
        return sb.ToString();
    }
    
    /// <summary>
    /// Tüm menüleri çeker
    /// </summary>
    private DataTable GetTumMenuler()
    {
        DataTable dt = new DataTable();
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM [Menu] ORDER BY [UstMenuID], [Sira]", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            conn.Open();
            da.Fill(dt);
        }
        
        return dt;
    }
} 