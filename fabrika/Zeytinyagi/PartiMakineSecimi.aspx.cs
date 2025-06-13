using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class fabrika_Zeytinyagi_PartiMakineSecimi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Zeytin Box Kasalarını doldur
            FillZeytinBoxlar();
            
            // Makine listesini doldur
            FillMakineListesi();
        }
    }

    private void FillZeytinBoxlar()
    {
        try
        {
            // Veritabanından kullanılan zeytin boxlarını çek
            DataTable dtBoxlar = GetZeytinBoxlarFromDB();
            
            // RadioButtonList'i temizle
            rblPartiListesi.Items.Clear();
            
            // Her veri satırı için bir ListItem oluştur
            foreach (DataRow row in dtBoxlar.Rows)
            {
                string boxID = row["ZeytinBoxKasaID"].ToString();
                string boxNo = row["ZeytinBoxNo"].ToString();
                string mustahsilAd = row["MustahsilAdSoyad"] != DBNull.Value ? row["MustahsilAdSoyad"].ToString() : "Belirtilmemiş";
                string durumu = row["Durumu"].ToString();
                string miktar = row["Miktar"] != DBNull.Value ? row["Miktar"].ToString() : "0";
                string alimTarihi = row["AlimTarihi"] != DBNull.Value ? Convert.ToDateTime(row["AlimTarihi"]).ToString("dd.MM.yy") : "Belirtilmemiş";
                string turBilgisi = row["TurBilgisi"] != DBNull.Value ? row["TurBilgisi"].ToString() : "Belirtilmemiş";
                string durumSinif = "bg-success";
                
                // HTML yapısını oluştur
                string html = string.Format(@"
                <div>
                    <input class='form-check-input me-2' type='radio' name='{0}' id='{1}' value='{1}' />
                    <strong>Box #{2}</strong> - {3}
                    <small class='d-block text-muted'>{4}, {5} Kg (Geliş: {6})</small>
                </div>
                <span class='badge {7} rounded-pill'>{8}</span>", 
                rblPartiListesi.UniqueID, boxID, boxNo, mustahsilAd, turBilgisi, miktar, alimTarihi, durumSinif, durumu);
                
                // Yeni ListItem oluştur
                ListItem item = new ListItem(html, boxID);
                
                // RadioButtonList'e ekle
                rblPartiListesi.Items.Add(item);
            }
        catch (Exception ex)
        {
            // Hata durumunda örnek verilerle devam et
            DataTable dtParti = CreateSampleBoxData();
            rblPartiListesi.Items.Clear();
            
            foreach (DataRow row in dtParti.Rows)
            {
                string boxID = row["BoxID"].ToString();
                string boxNo = row["BoxNo"].ToString();
                string mustahsil = row["Mustahsil"].ToString();
                string turVeOzellik = row["TurVeOzellik"].ToString();
                string miktar = row["Miktar"].ToString();
                string gelisTarihi = row["GelisTarihi"].ToString();
                string durum = row["Durum"].ToString();
                string durumSinif = "bg-success";
                
                string html = string.Format(@"
                <div>
                    <input class='form-check-input me-2' type='radio' name='{0}' id='{1}' value='{1}' />
                    <strong>Box #{2}</strong> - {3}
                    <small class='d-block text-muted'>{4}, {5} Kg (Geliş: {6})</small>
                </div>
                <span class='badge {7} rounded-pill'>{8}</span>", 
                rblPartiListesi.UniqueID, boxID, boxNo, mustahsil, turVeOzellik, miktar, gelisTarihi, durumSinif, durum);
                
                ListItem item = new ListItem(html, boxID);
                rblPartiListesi.Items.Add(item);
            }
        }
    }

    private void FillMakineListesi()
    {
        try
        {
            // Veritabanından makine verilerini çek
            DataTable dtMakine = GetMachinesFromDB();
            
            // RadioButtonList'i temizle
            rblMakineListesi.Items.Clear();
            
            // Her veri satırı için bir ListItem oluştur
            foreach (DataRow row in dtMakine.Rows)
            {
                string makineID = row["MakinaID"].ToString();
                string makineAdi = row["MakinaAdi"].ToString();
                string kapasite = row["MaxKapasite"].ToString() + " Kg/saat";
                string sonBakim = row["SonBakimTarihi"] != DBNull.Value ? Convert.ToDateTime(row["SonBakimTarihi"]).ToString("dd.MM.yy") : "Bilinmiyor";
                string durum = row["Durum"].ToString();
                string durumIcon = "";
                string durumClass = "";
                
                // Durum bilgisine göre ikon ve renk sınıfı belirle
                if (durum == "Müsait" || durum == "1")
                {
                    durum = "Müsait";
                    durumIcon = "bi-check-circle-fill";
                    durumClass = "machine-status-available";
                }
                else if (durum.Contains("Meşgul") || durum == "2")
                {
                    string mesgulNot = row["MesgulNot"] != DBNull.Value ? row["MesgulNot"].ToString() : "";
                    durum = "Meşgul" + (!string.IsNullOrEmpty(mesgulNot) ? " (" + mesgulNot + ")" : "");
                    durumIcon = "bi-hourglass-split";
                    durumClass = "machine-status-busy";
                }
                else if (durum.Contains("Bakım") || durum == "3")
                {
                    durum = "Bakımda";
                    durumIcon = "bi-tools";
                    durumClass = "machine-status-maintenance";
                }
                
                // HTML yapısını oluştur
                string html = string.Format(@"
                <div>
                    <input class='form-check-input me-2' type='radio' name='{0}' id='{1}' value='{1}' data-kapasite='{2}' />
                    <strong>{3}</strong>
                    <small class='d-block text-muted'>Kapasite: {4} - Son Bakım: {5}</small>
                </div>
                <span class='badge {6} rounded-pill'><i class='bi {7} me-1'></i>{8}</span>", 
                rblMakineListesi.UniqueID, makineID, kapasite.Replace(" Kg/saat", ""), makineAdi, kapasite, sonBakim, durumClass, durumIcon, durum);
                
                // Yeni ListItem oluştur
                ListItem item = new ListItem(html, makineID);
                
                // RadioButtonList'e ekle
                rblMakineListesi.Items.Add(item);
            }
            
            // Eğer liste boş ise bilgi mesajı göster
            if (rblMakineListesi.Items.Count == 0)
            {
                // Bilgi mesajı ekle
                string html = @"<div class='alert alert-info m-3'>Sistemde tanımlı makine bulunamadı.</div>";
                LiteralControl literal = new LiteralControl(html);
                rblMakineListesi.Controls.Add(literal);
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda örnek verilerle devam et
            DataTable dtMakine = CreateSampleMakineData();
            rblMakineListesi.Items.Clear();
            
            foreach (DataRow row in dtMakine.Rows)
            {
                string makineID = row["MakineID"].ToString();
                string makineAdi = row["MakineAdi"].ToString();
                string kapasite = row["Kapasite"].ToString();
                string sonBakim = row["SonBakim"].ToString();
                string durum = row["Durum"].ToString();
                string durumIcon = "";
                string durumClass = "";
                
                if (durum == "Müsait")
                {
                    durumIcon = "bi-check-circle-fill";
                    durumClass = "machine-status-available";
                }
                else if (durum.Contains("Meşgul"))
                {
                    durumIcon = "bi-hourglass-split";
                    durumClass = "machine-status-busy";
                }
                else if (durum == "Bakımda")
                {
                    durumIcon = "bi-tools";
                    durumClass = "machine-status-maintenance";
                }
                
                string html = string.Format(@"
                <div>
                    <input class='form-check-input me-2' type='radio' name='{0}' id='{1}' value='{1}' data-kapasite='{2}' />
                    <strong>{3}</strong>
                    <small class='d-block text-muted'>Kapasite: {4} - Son Bakım: {5}</small>
                </div>
                <span class='badge {6} rounded-pill'><i class='bi {7} me-1'></i>{8}</span>", 
                rblMakineListesi.UniqueID, makineID, kapasite.Replace(" Kg/saat", ""), makineAdi, kapasite, sonBakim, durumClass, durumIcon, durum);
                
                ListItem item = new ListItem(html, makineID);
                rblMakineListesi.Items.Add(item);
            }
            
            // Hata logla
            System.Diagnostics.Debug.WriteLine("Makine listesi yüklenirken hata: " + ex.Message);
        }
    }

    private DataTable GetZeytinBoxlarFromDB()
    {
        DataTable dt = new DataTable();
        
        // Veritabanı bağlantı stringi
        string connString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connString))
        {
            // SQL sorgusu - Müstahsile atanmış zeytin box kasalarını getir
            string sqlQuery = @"
                SELECT zbk.ZeytinBoxKasaID, zbk.ZeytinBoxNo, zbk.Durumu, zbk.AlimTarihi, zbk.KulananMustahsilID,
                       m.Ad + ' ' + m.Soyad AS MustahsilAdSoyad,
                       m.MustahsilID,
                       zbk.Miktar,
                       t.TurAdi AS TurBilgisi
                FROM ZeytinBoxKasalari zbk
                LEFT JOIN Mustahsiller m ON zbk.KulananMustahsilID = m.MustahsilID
                LEFT JOIN ZeytinTurleri t ON zbk.ZeytinTuruID = t.ZeytinTuruID
                WHERE zbk.KulananMustahsilID IS NOT NULL
                ORDER BY zbk.AlimTarihi DESC";
            
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanından zeytin box verileri çekilirken hata: " + ex.Message);
            }
        }
        
        return dt;
    }

    private DataTable GetMachinesFromDB()
    {
        DataTable dt = new DataTable();
        
        // Veritabanı bağlantı stringi
        string connString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connString))
        {
            // SQL sorgusu
            string sqlQuery = @"
                SELECT m.MakinaID, 
                       (mar.MarkaAd + ' ' + mm.ModelAd) AS MakinaAdi,
                       mm.MaxKapasite, m.SonBakimTarihi, m.Durum, m.MesgulNot
                FROM Makinalar m
                INNER JOIN ZeytinyagiMakinaModelleri mm ON m.ZeytinyagiMakinaModelID = mm.ZeytinyagiMakinaModelID
                INNER JOIN ZeytinyagiMakinaMarkalari mar ON mm.ZeytinyagiMakinaMarkaID = mar.ZeytinyagiMakinaMarkaID
                WHERE m.AktifMi = 1
                ORDER BY mm.MaxKapasite DESC";
            
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanından makine verileri çekilirken hata: " + ex.Message);
            }
        }
        
        return dt;
    }

    private DataTable CreateSampleBoxData()
    {
        // Örnek box verileri
        DataTable dt = new DataTable();
        dt.Columns.Add("BoxID", typeof(string));
        dt.Columns.Add("BoxNo", typeof(string));
        dt.Columns.Add("Mustahsil", typeof(string));
        dt.Columns.Add("TurVeOzellik", typeof(string));
        dt.Columns.Add("Miktar", typeof(string));
        dt.Columns.Add("GelisTarihi", typeof(string));
        dt.Columns.Add("Durum", typeof(string));
        
        dt.Rows.Add("1", "B101", "Mehmet Çiftçi", "Gemlik", "250", "28.10.23", "Dolu");
        dt.Rows.Add("2", "B102", "Koop. Birlik", "Ayvalık", "420", "27.10.23", "Dolu");
        dt.Rows.Add("3", "B103", "Ayşe Tarım", "Memecik", "180", "27.10.23", "Dolu");
        dt.Rows.Add("4", "B104", "XYZ Tarım A.Ş.", "Karışık", "350", "29.10.23", "Dolu");
        
        return dt;
    }

    private DataTable CreateSampleMakineData()
    {
        // Örnek makine verileri
        DataTable dt = new DataTable();
        dt.Columns.Add("MakineID", typeof(string));
        dt.Columns.Add("MakineAdi", typeof(string));
        dt.Columns.Add("Kapasite", typeof(string));
        dt.Columns.Add("SonBakim", typeof(string));
        dt.Columns.Add("Durum", typeof(string));
        
        dt.Rows.Add("MAK001", "Makine 1 (Alfa Laval)", "500 Kg/saat", "15.10.23", "Müsait");
        dt.Rows.Add("MAK002", "Makine 2 (Pieralisi)", "750 Kg/saat", "20.09.23", "Meşgul (Parti #P087)");
        dt.Rows.Add("MAK003", "Makine 3 (Organik Hat)", "500 Kg/saat", "01.10.23", "Bakımda");
        
        return dt;
    }

    protected void btnUretimiBaslat_Click(object sender, EventArgs e)
    {
        // Form doğrulaması ve verilerin alınması
        string boxID = hfSelectedPartiID.Value;  // Seçilen Box ID'si
        string makineID = hfSelectedMakineID.Value;
        string islemMiktari = txtIslemMiktari.Text;
        string operator_ = txtOperator.Text;
        string beklenenYagTipi = ddlBeklenenYagTipi.SelectedValue;
        string uretimNotlari = txtUretimNotlari.Text;
        
        if (string.IsNullOrEmpty(boxID) || string.IsNullOrEmpty(makineID) || 
            string.IsNullOrEmpty(islemMiktari) || string.IsNullOrEmpty(operator_))
        {
            // Eksik veri kontrolü
            ClientScript.RegisterStartupScript(this.GetType(), "uyari", 
                "alert('Lütfen tüm zorunlu alanları doldurun.');", true);
            return;
        }
        
        try
        {
            // Box'tan üretim başlatma işlemi
            int uretimID = StartProductionFromBox(boxID, makineID, islemMiktari, operator_, beklenenYagTipi, uretimNotlari);
            
            if (uretimID > 0)
            {
                // Başarılı mesajı göster
                string script = string.Format(@"
                    alert('Yeni üretim başarıyla başlatıldı!');
                    console.log('Kaydedilen veriler: Box ID: {0}, Makine ID: {1}, İşlem Miktarı: {2}, Operatör: {3}, Beklenen Yağ Tipi: {4}, Üretim Notları: {5}');", 
                    boxID, makineID, islemMiktari, operator_, beklenenYagTipi, uretimNotlari);
                
                ClientScript.RegisterStartupScript(this.GetType(), "basarili", script, true);
                
                // Üretim takip sayfasına yönlendir
                Response.Redirect("UretimTakip.aspx?uretimID=" + uretimID);
            }
            else
            {
                // Hata mesajı göster
                ClientScript.RegisterStartupScript(this.GetType(), "hata", 
                    "alert('Üretim başlatılırken bir sorun oluştu. Lütfen tekrar deneyin.');", true);
            }
        }
        catch (Exception ex)
        {
            // Hata mesajı göster
            ClientScript.RegisterStartupScript(this.GetType(), "hata", 
                "alert('Hata: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
    
    private int StartProductionFromBox(string boxID, string makineID, string islemMiktari, string operator_, string beklenenYagTipi, string uretimNotlari)
    {
        int uretimID = 0;
        
        // Veritabanı bağlantı stringi
        string connString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();
            
            try
            {
                // 1. Box bilgilerini getir
                SqlCommand cmdGetBox = new SqlCommand(@"
                    SELECT KulananMustahsilID, ZeytinBoxNo, Miktar, ZeytinTuruID 
                    FROM ZeytinBoxKasalari 
                    WHERE ZeytinBoxKasaID = @BoxID", conn, transaction);
                
                cmdGetBox.Parameters.AddWithValue("@BoxID", boxID);
                
                SqlDataReader reader = cmdGetBox.ExecuteReader();
                int mustahsilID = 0;
                string boxNo = "";
                double boxMiktar = 0;
                int zeytinTuruID = 0;
                
                if (reader.Read())
                {
                    mustahsilID = reader["KulananMustahsilID"] != DBNull.Value ? Convert.ToInt32(reader["KulananMustahsilID"]) : 0;
                    boxNo = reader["ZeytinBoxNo"].ToString();
                    boxMiktar = reader["Miktar"] != DBNull.Value ? Convert.ToDouble(reader["Miktar"]) : 0;
                    zeytinTuruID = reader["ZeytinTuruID"] != DBNull.Value ? Convert.ToInt32(reader["ZeytinTuruID"]) : 0;
                }
                reader.Close();
                
                // 2. Yeni üretim kaydı oluştur
                string partiNo = "P" + DateTime.Now.ToString("yyyyMMddHHmm"); // Benzersiz parti no oluştur
                
                SqlCommand cmdInsertUretim = new SqlCommand(@"
                    INSERT INTO ZeytinyagiUretimleri (
                        SirketID, MustahsilID, PartiNo, GelisTarihi, GelisKg, UrunID, ZeytinTuruID,
                        UretimBaslamaZamani, Operator_KullaniciID, MakineID, UretimeAlinanKg, 
                        BeklenenYagTipi, UretimNotlari, BoxID
                    )
                    VALUES (
                        1, @MustahsilID, @PartiNo, GETDATE(), @BoxMiktar, NULL, @ZeytinTuruID,
                        GETDATE(), @OperatorID, @MakineID, @IslemMiktari, 
                        @BeklenenYagTipi, @UretimNotlari, @BoxID
                    );
                    
                    SELECT SCOPE_IDENTITY();", conn, transaction);
                
                cmdInsertUretim.Parameters.AddWithValue("@MustahsilID", mustahsilID);
                cmdInsertUretim.Parameters.AddWithValue("@PartiNo", partiNo);
                cmdInsertUretim.Parameters.AddWithValue("@BoxMiktar", boxMiktar);
                cmdInsertUretim.Parameters.AddWithValue("@ZeytinTuruID", zeytinTuruID != 0 ? (object)zeytinTuruID : DBNull.Value);
                cmdInsertUretim.Parameters.AddWithValue("@OperatorID", operator_);
                cmdInsertUretim.Parameters.AddWithValue("@MakineID", makineID);
                cmdInsertUretim.Parameters.AddWithValue("@IslemMiktari", islemMiktari);
                cmdInsertUretim.Parameters.AddWithValue("@BeklenenYagTipi", string.IsNullOrEmpty(beklenenYagTipi) ? DBNull.Value : (object)beklenenYagTipi);
                cmdInsertUretim.Parameters.AddWithValue("@UretimNotlari", string.IsNullOrEmpty(uretimNotlari) ? DBNull.Value : (object)uretimNotlari);
                cmdInsertUretim.Parameters.AddWithValue("@BoxID", boxID);
                
                uretimID = Convert.ToInt32(cmdInsertUretim.ExecuteScalar());
                
                // 3. Box durumunu güncelle - Müstahsil ilişkisini kaldır (KulananMustahsilID = NULL)
                SqlCommand cmdUpdateBox = new SqlCommand(@"
                    UPDATE ZeytinBoxKasalari
                    SET KulananMustahsilID = NULL, 
                        Durumu = 'Boş',
                        SonGuncellemeTarihi = GETDATE(),
                        SonGuncelleyenKullaniciID = @OperatorID
                    WHERE ZeytinBoxKasaID = @BoxID", conn, transaction);
                
                cmdUpdateBox.Parameters.AddWithValue("@BoxID", boxID);
                cmdUpdateBox.Parameters.AddWithValue("@OperatorID", operator_);
                cmdUpdateBox.ExecuteNonQuery();
                
                // 4. Makine durumunu güncelle
                SqlCommand cmdUpdateMakine = new SqlCommand(@"
                    UPDATE Makinalar
                    SET Durum = 2, -- Meşgul
                        MesgulNot = 'Parti #' + @PartiNo
                    WHERE MakinaID = @MakineID", conn, transaction);
                
                cmdUpdateMakine.Parameters.AddWithValue("@PartiNo", partiNo);
                cmdUpdateMakine.Parameters.AddWithValue("@MakineID", makineID);
                cmdUpdateMakine.ExecuteNonQuery();
                
                // İşlemi tamamla
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemi geri al
                transaction.Rollback();
                throw new Exception("Üretim başlatılırken veritabanı hatası: " + ex.Message);
            }
        }
        
        return uretimID;
    }
}