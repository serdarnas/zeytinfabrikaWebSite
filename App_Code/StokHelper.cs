using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Stok hareketlerini yönetmek için yardımcı sınıf
/// </summary>
public class StokHelper
{
    /// <summary>
    /// Satış işleminde stok çıkışı yapmak için kullanılır
    /// </summary>
    /// <param name="sirketID">Şirket ID</param>
    /// <param name="satisID">Satış ID (referans için)</param>
    /// <param name="satisDetaylari">Satış detayları</param>
    /// <param name="kullaniciID">İşlemi yapan kullanıcı ID</param>
    /// <returns>İşlem başarılı ise true, değilse false</returns>
    public static bool SatisStokCikisiYap(int sirketID, int satisID, List<SatisDetaylari> satisDetaylari, int? kullaniciID = null)
    {
        bool basarili = true;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            
            foreach (var detay in satisDetaylari)
            {
                using (SqlCommand cmd = new SqlCommand("SP_StokHareket", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@SirketID", sirketID);
                    cmd.Parameters.AddWithValue("@HareketTipi", "SATIS");
                    cmd.Parameters.AddWithValue("@DepoID", detay.DepoID);
                    cmd.Parameters.AddWithValue("@UrunID", detay.UrunID);
                    cmd.Parameters.AddWithValue("@Miktar", detay.Miktar);
                    cmd.Parameters.AddWithValue("@ReferansNo", "SATIS-" + satisID.ToString());
                    cmd.Parameters.AddWithValue("@ReferansID", satisID);
                    cmd.Parameters.AddWithValue("@ReferansTipi", "SATIS");
                    cmd.Parameters.AddWithValue("@Aciklama", "Satış işlemi üzerinden stok çıkışı");
                    
                    if (kullaniciID.HasValue)
                        cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID.Value);
                    else
                        cmd.Parameters.AddWithValue("@KullaniciID", DBNull.Value);
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Stok hatası durumunda işlem başarısız olur
                        basarili = false;
                        // Hata loglanabilir
                        System.Diagnostics.Debug.WriteLine("Stok çıkış hatası: " + ex.Message);
                    }
                }
            }
        }
        
        return basarili;
    }
    
    /// <summary>
    /// Tek bir ürün için stok hareketi oluşturur
    /// </summary>
    public static bool StokHareketiYap(int sirketID, string hareketTipi, int depoID, int urunID, decimal miktar, 
        string referansTipi = null, int? referansID = null, string aciklama = null, int? kullaniciID = null)
    {
        bool basarili = true;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            
            using (SqlCommand cmd = new SqlCommand("SP_StokHareket", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@SirketID", sirketID);
                cmd.Parameters.AddWithValue("@HareketTipi", hareketTipi);
                cmd.Parameters.AddWithValue("@DepoID", depoID);
                cmd.Parameters.AddWithValue("@UrunID", urunID);
                cmd.Parameters.AddWithValue("@Miktar", miktar);
                
                if (!string.IsNullOrEmpty(referansTipi))
                {
                    cmd.Parameters.AddWithValue("@ReferansTipi", referansTipi);
                    if (referansID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ReferansID", referansID.Value);
                        cmd.Parameters.AddWithValue("@ReferansNo", referansTipi + "-" + referansID.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ReferansID", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ReferansNo", DBNull.Value);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferansTipi", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReferansID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReferansNo", DBNull.Value);
                }
                
                cmd.Parameters.AddWithValue("@Aciklama", aciklama ?? "Manuel stok hareketi");
                
                if (kullaniciID.HasValue)
                    cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID.Value);
                else
                    cmd.Parameters.AddWithValue("@KullaniciID", DBNull.Value);
                
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    basarili = false;
                    System.Diagnostics.Debug.WriteLine("Stok hareket hatası: " + ex.Message);
                }
            }
        }
        
        return basarili;
    }
    
    /// <summary>
    /// Belirli bir depo ve ürün için stok miktarını döndürür
    /// </summary>
    public static decimal GetStokMiktari(int depoID, int urunID)
    {
        decimal miktar = 0;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            
            string query = "SELECT Miktar FROM DepoStok WHERE DepoID = @DepoID AND UrunID = @UrunID";
            
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@DepoID", depoID);
                cmd.Parameters.AddWithValue("@UrunID", urunID);
                
                object result = cmd.ExecuteScalar();
                
                if (result != null && result != DBNull.Value)
                {
                    miktar = Convert.ToDecimal(result);
                }
            }
        }
        
        return miktar;
    }
} 