using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Kullanıcı yetkileri için yardımcı sınıf
/// </summary>
public static class YetkiHelper
{
    /// <summary>
    /// Yeni kayıt olan kullanıcıya tüm menü yetkilerini verir
    /// </summary>
    /// <param name="kullaniciID">Kullanıcı ID</param>
    public static void TumMenuYetkileriniVer(int kullaniciID)
    {
        try
        {
            // Önce veritabanından mevcut tüm menü yetkilerini alalım
            List<string> tumYetkiKodlari = new List<string>();
            
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Tüm menü yetki kodlarını al
                string sql = "SELECT DISTINCT YetkiKodu FROM Menu";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tumYetkiKodlari.Add(reader["YetkiKodu"].ToString());
                        }
                    }
                }
                
                // Kullanıcı için varolan yetkileri sil (temiz başlangıç)
                string deleteSQL = "DELETE FROM KullaniciYetki WHERE KullaniciID = @KullaniciID";
                using (SqlCommand cmd = new SqlCommand(deleteSQL, conn))
                {
                    cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
                    cmd.ExecuteNonQuery();
                }
                
                // Tüm menü yetkilerini kullanıcı için ekle
                string insertSQL = "INSERT INTO KullaniciYetki (KullaniciID, YetkiKodu, Aktif) VALUES (@KullaniciID, @YetkiKodu, 1)";
                foreach (string yetkiKodu in tumYetkiKodlari)
                {
                    using (SqlCommand cmd = new SqlCommand(insertSQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
                        cmd.Parameters.AddWithValue("@YetkiKodu", yetkiKodu);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
            System.Diagnostics.Debug.WriteLine(string.Format("Kullanıcı {0} için {1} menü yetkisi eklendi", kullaniciID, tumYetkiKodlari.Count));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Menü yetkisi verme hatası: " + ex.Message);
        }
    }
    
    /// <summary>
    /// Kullanıcının belirli bir menü yetkisine sahip olup olmadığını kontrol eder
    /// </summary>
    /// <param name="kullaniciID">Kullanıcı ID</param>
    /// <param name="yetkiKodu">Yetki kodu</param>
    /// <returns>Yetki varsa true, yoksa false</returns>
    public static bool YetkiVarMi(int kullaniciID, string yetkiKodu)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                string sql = "SELECT COUNT(1) FROM KullaniciYetki WHERE KullaniciID = @KullaniciID AND YetkiKodu = @YetkiKodu AND Aktif = 1";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
                    cmd.Parameters.AddWithValue("@YetkiKodu", yetkiKodu);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Yetki kontrolü hatası: " + ex.Message);
            return false;
        }
    }
} 