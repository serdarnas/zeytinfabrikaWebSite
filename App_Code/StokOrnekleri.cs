using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Stok işlemleri için örnek kod parçaları ve kullanım şekilleri
/// </summary>
public class StokOrnekleri
{
    #region Satış İşlemi Örnekleri
    
    /// <summary>
    /// Satış işlemi sırasında stok çıkışı yapma örneği
    /// </summary>
    public static void SatisIslemiveStokCikisi()
    {
        int sirketID = 1; // Örnek şirket ID
        int satisID = 100; // Örnek satış ID
        int kullaniciID = 5; // Örnek kullanıcı ID
        
        // Satış detayları örneği
        List<SatisDetaylari> satisDetaylari = new List<SatisDetaylari>
        {
            new SatisDetaylari
            {
                SatisID = satisID,
                UrunID = 10,
                Miktar = 5,
                DepoID = 1
            },
            new SatisDetaylari
            {
                SatisID = satisID,
                UrunID = 15,
                Miktar = 2,
                DepoID = 1
            }
        };
        
        // Stok çıkışı yapma
        bool sonuc = StokHelper.SatisStokCikisiYap(sirketID, satisID, satisDetaylari, kullaniciID);
        
        if (sonuc)
        {
            // İşlem başarılı - loglama veya diğer işlemler
            System.Diagnostics.Debug.WriteLine("Stok çıkışları başarıyla yapıldı.");
        }
        else
        {
            // İşlem hatası - kullanıcıya bildir
            System.Diagnostics.Debug.WriteLine("Stok çıkışı sırasında hata oluştu!");
        }
    }
    
    #endregion
    
    #region Satış Öncesi Stok Kontrolü
    
    /// <summary>
    /// Satış işlemi öncesinde stok kontrolü örneği
    /// </summary>
    /// <returns>Stok yeterli ise true, değilse false</returns>
    public static bool SatisOncesiStokKontrolu(List<SepetItem> sepet)
    {
        bool stokYeterli = true;
        List<string> yetersizUrunler = new List<string>();
        
        foreach (var urun in sepet)
        {
            // Varsayılan depo ID (formdan alınabilir)
            int depoID = 1;
            
            // Stok miktarını kontrol et
            decimal mevcutStok = StokHelper.GetStokMiktari(depoID, urun.UrunID);
            
            if (mevcutStok < urun.Miktar)
            {
                // Stok yetersiz
                stokYeterli = false;
                yetersizUrunler.Add(string.Format("{0} (Stok: {1}, İstenen: {2})", urun.UrunAdi, mevcutStok, urun.Miktar));
            }
        }
        
        // Eğer yetersiz ürün varsa, kullanıcıya bilgi ver
        if (!stokYeterli)
        {
            string mesaj = "Aşağıdaki ürünler için stok yetersiz:\n" +
                           string.Join("\n", yetersizUrunler);
            
            System.Diagnostics.Debug.WriteLine(mesaj);
            // Burada kullanıcıya alert veya başka bir bildirim gösterilebilir
        }
        
        return stokYeterli;
    }
    
    #endregion
    
    #region Manuel Stok Girişi
    
    /// <summary>
    /// Manuel stok girişi örneği
    /// </summary>
    public static void ManuelStokGirisi(int sirketID, int depoID, int urunID, decimal miktar, string aciklama)
    {
        // Kullanıcı ID'si (Session'dan alınabilir)
        int? kullaniciID = GetKullaniciID();
        
        // Stok girişi yap
        bool sonuc = StokHelper.StokHareketiYap(
            sirketID,           // Şirket ID
            "GIRIS",            // Hareket tipi
            depoID,             // Depo ID
            urunID,             // Ürün ID
            miktar,             // Miktar (pozitif)
            "MANUEL",           // Referans tipi
            null,               // Referans ID
            aciklama,           // Açıklama
            kullaniciID         // Kullanıcı ID
        );
        
        if (sonuc)
        {
            // İşlem başarılı
            System.Diagnostics.Debug.WriteLine("Stok girişi başarıyla yapıldı.");
        }
        else
        {
            // İşlem hatası
            System.Diagnostics.Debug.WriteLine("Stok girişi sırasında hata oluştu!");
        }
    }
    
    #endregion
    
    #region Depolar Arası Transfer
    
    /// <summary>
    /// Depolar arası ürün transferi örneği
    /// </summary>
    public static bool DepoTransferi(int sirketID, int kaynakDepoID, int hedefDepoID, int urunID, decimal miktar, string aciklama)
    {
        int? kullaniciID = GetKullaniciID();
        bool sonuc = false;
        
        try
        {
            // 1. Kaynak depodan çıkış yap
            bool cikisSonuc = StokHelper.StokHareketiYap(
                sirketID,
                "TRANSFER_CIKIS",
                kaynakDepoID,
                urunID,
                miktar * -1, // Negatif değer (çıkış)
                "TRANSFER",
                null,
                string.Format("Transfer: {0} → {1} - {2}", kaynakDepoID, hedefDepoID, aciklama),
                kullaniciID
            );
            
            if (!cikisSonuc)
            {
                return false; // Çıkış başarısız
            }
            
            // 2. Hedef depoya giriş yap
            bool girisSonuc = StokHelper.StokHareketiYap(
                sirketID,
                "TRANSFER_GIRIS",
                hedefDepoID,
                urunID,
                miktar, // Pozitif değer (giriş)
                "TRANSFER",
                null,
                string.Format("Transfer: {0} → {1} - {2}", kaynakDepoID, hedefDepoID, aciklama),
                kullaniciID
            );
            
            sonuc = girisSonuc;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Depo transferi hatası: " + ex.Message);
            sonuc = false;
        }
        
        return sonuc;
    }
    
    #endregion
    
    #region Stok Düzeltme
    
    /// <summary>
    /// Stok sayımı sonrası düzeltme örneği
    /// </summary>
    public static bool StokDuzeltme(int sirketID, int depoID, int urunID, decimal mevcutMiktar, decimal gercekMiktar, string aciklama)
    {
        decimal fark = gercekMiktar - mevcutMiktar;
        
        if (fark == 0)
        {
            // Fark yok, düzeltme gerekmiyor
            return true;
        }
        
        string hareketTipi = (fark > 0) ? "DUZELTME_GIRIS" : "DUZELTME_CIKIS";
        int? kullaniciID = GetKullaniciID();
        
        // Stok düzeltme hareketi ekle
        bool sonuc = StokHelper.StokHareketiYap(
            sirketID,
            hareketTipi,
            depoID,
            urunID,
            fark, // Pozitif veya negatif olabilir
            "DUZELTME",
            null,
            string.Format("Stok düzeltme: Mevcut={0}, Gerçek={1} - {2}", mevcutMiktar, gercekMiktar, aciklama),
            kullaniciID
        );
        
        return sonuc;
    }
    
    #endregion
    
    #region Stok Raporu Örneği
    
    /// <summary>
    /// Detaylı stok durumu raporu örneği
    /// </summary>
    public static DataTable GetStokDurumuRaporu(int sirketID)
    {
        DataTable dt = new DataTable();
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            string sorgu = @"
                SELECT 
                    d.DepoAdi,
                    u.UrunKodu,
                    u.UrunAdi,
                    ds.Miktar,
                    b.BirimAdi,
                    ds.MinimumMiktar,
                    CASE 
                        WHEN ds.Miktar <= ds.MinimumMiktar THEN 'Kritik'
                        WHEN ds.Miktar <= (ds.MinimumMiktar * 1.5) THEN 'Uyarı'
                        ELSE 'Normal'
                    END AS StokDurumu
                FROM DepoStok ds
                JOIN Depolar d ON ds.DepoID = d.DepoID
                JOIN Urunler u ON ds.UrunID = u.UrunID
                LEFT JOIN Birimler b ON u.BirimID = b.BirimID
                WHERE ds.SirketID = @SirketID
                ORDER BY StokDurumu, d.DepoAdi, u.UrunAdi";
            
            using (SqlCommand cmd = new SqlCommand(sorgu, conn))
            {
                cmd.Parameters.AddWithValue("@SirketID", sirketID);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        
        return dt;
    }
    
    #endregion
    
    #region Yardımcı Metodlar
    
    /// <summary>
    /// Session'dan kullanıcı ID'sini alma
    /// </summary>
    private static int? GetKullaniciID()
    {
        if (HttpContext.Current.Session["KullaniciID"] != null)
        {
            return Convert.ToInt32(HttpContext.Current.Session["KullaniciID"]);
        }
        return null;
    }
    
    #endregion
} 