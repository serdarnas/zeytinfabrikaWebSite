using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.Services;

public partial class fabrika_Nakit_Kasalar : System.Web.UI.Page
{
    private int SirketID
    {
        get
        {
            if (Session["SirketID"] != null)
                return Convert.ToInt32(Session["SirketID"]);
            return 0;
        }
    }

    private int KullaniciID
    {
        get
        {
            if (Session["KullaniciID"] != null)
                return Convert.ToInt32(Session["KullaniciID"]);
            return 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SirketID == 0)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            YuklemeIslemi();
        }
    }

    private void YuklemeIslemi()
    {
        try
        {
            ParaBirimleriniYukle();
            OdemeTipleriniYukle();
            KasalariYukle();
            IstatistikleriHesapla();
            MusterileriYukle();
            TedarikcileriYukle();
            MustahsilleriYukle();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void ParaBirimleriniYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT ParaBirimiID, ParaBirimiAd FROM ParaBirimileri ORDER BY ParaBirimiAd";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlParaBirimi.DataTextField = "ParaBirimiAd";
                ddlParaBirimi.DataValueField = "ParaBirimiID";
                ddlParaBirimi.DataSource = dt;
                ddlParaBirimi.DataBind();

                // TL'yi varsayılan olarak seç
                if (ddlParaBirimi.Items.FindByText("Türk Lirası") != null)
                    ddlParaBirimi.SelectedValue = ddlParaBirimi.Items.FindByText("Türk Lirası").Value;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    private void OdemeTipleriniYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT OdemeTipiID, OdemeTipiAdi FROM OdemeTipleri ORDER BY OdemeTipiID";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlOdemeTipi.DataTextField = "OdemeTipiAdi";
                ddlOdemeTipi.DataValueField = "OdemeTipiID";
                ddlOdemeTipi.DataSource = dt;
                ddlOdemeTipi.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    private void KasalariYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT k.KasaID, k.KasaKodu, k.KasaAdi, k.KasaTipi, k.Bakiye, 
                           p.ParaBirimi, k.Aciklama, k.AktifMi
                    FROM Kasalar k
                    LEFT JOIN ParaBirimileri p ON k.ParaBirimiID = p.ParaBirimiID
                    WHERE k.SirketID = @SirketID AND k.AktifMi = 1
                    ORDER BY k.KasaAdi";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptKasalar.DataSource = dt;
                rptKasalar.DataBind();

                // Transfer için kasa listelerini yükle
                KasalariTransferIcinYukle(dt);
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasalar yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void KasalariTransferIcinYukle(DataTable kasalar)
    {
        ddlKaynakKasa.DataTextField = "KasaAdi";
        ddlKaynakKasa.DataValueField = "KasaID";
        ddlKaynakKasa.DataSource = kasalar;
        ddlKaynakKasa.DataBind();
        ddlKaynakKasa.Items.Insert(0, new ListItem("Seçiniz...", "0"));

        ddlHedefKasa.DataTextField = "KasaAdi";
        ddlHedefKasa.DataValueField = "KasaID";
        ddlHedefKasa.DataSource = kasalar;
        ddlHedefKasa.DataBind();
        ddlHedefKasa.Items.Insert(0, new ListItem("Seçiniz...", "0"));

        ddlIslemKasa.DataTextField = "KasaAdi";
        ddlIslemKasa.DataValueField = "KasaID";
        ddlIslemKasa.DataSource = kasalar;
        ddlIslemKasa.DataBind();
        ddlIslemKasa.Items.Insert(0, new ListItem("Seçiniz...", "0"));
    }

    private void IstatistikleriHesapla()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                // Toplam kasa sayısı
                string queryKasaSayisi = "SELECT COUNT(*) FROM Kasalar WHERE SirketID = @SirketID AND AktifMi = 1";
                SqlCommand cmdKasaSayisi = new SqlCommand(queryKasaSayisi, conn);
                cmdKasaSayisi.Parameters.AddWithValue("@SirketID", SirketID);

                // Nakit toplam (Fiziksel kasalar)
                string queryNakitToplam = @"
                    SELECT ISNULL(SUM(k.Bakiye), 0)
                    FROM Kasalar k
                    WHERE k.SirketID = @SirketID AND k.AktifMi = 1 AND k.KasaTipi = 'F'";

                // Banka toplam (Dijital kasalar)
                string queryBankaToplam = @"
                    SELECT ISNULL(SUM(k.Bakiye), 0)
                    FROM Kasalar k
                    WHERE k.SirketID = @SirketID AND k.AktifMi = 1 AND k.KasaTipi = 'D'";

                conn.Open();

                // Kasa sayısı
                int kasaSayisi = Convert.ToInt32(cmdKasaSayisi.ExecuteScalar());
                lblToplamKasa.Text = kasaSayisi.ToString();

                // Nakit toplam
                SqlCommand cmdNakit = new SqlCommand(queryNakitToplam, conn);
                cmdNakit.Parameters.AddWithValue("@SirketID", SirketID);
                decimal nakitToplam = Convert.ToDecimal(cmdNakit.ExecuteScalar());
                lblNakitToplam.Text = nakitToplam.ToString("N2");

                // Banka toplam
                SqlCommand cmdBanka = new SqlCommand(queryBankaToplam, conn);
                cmdBanka.Parameters.AddWithValue("@SirketID", SirketID);
                decimal bankaToplam = Convert.ToDecimal(cmdBanka.ExecuteScalar());
                lblBankaToplam.Text = bankaToplam.ToString("N2");

                // Genel toplam
                decimal genelToplam = nakitToplam + bankaToplam;
                lblGenelToplam.Text = genelToplam.ToString("N2");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İstatistikler hesaplanırken hata oluştu: " + ex.Message);
        }
    }

    private void MusterileriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT MusteriID, MusteriAdi FROM Musteriler WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY MusteriAdi";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ViewState["MusteriListesi"] = dt;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    private void TedarikcileriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT TedarikciID, TedarikciAdi FROM Tedarikciler WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY TedarikciAdi";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ViewState["TedarikciListesi"] = dt;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    private void MustahsilleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT MustahsilID, ISNULL(MustahsilAdi, AdSoyad) as MustahsilAdi FROM Mustahsiller WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY ISNULL(MustahsilAdi, AdSoyad)";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ViewState["MustahsilListesi"] = dt;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    protected void btnKasaKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtKasaKodu.Text.Trim()))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kasa kodu boş olamaz!");
                return;
            }

            if (string.IsNullOrEmpty(txtKasaAdi.Text.Trim()))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kasa adı boş olamaz!");
                return;
            }

            decimal baslangicBakiye = 0;
            if (!decimal.TryParse(txtBaslangicBakiye.Text, out baslangicBakiye))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Geçerli bir başlangıç bakiyesi giriniz!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Düzenleme modu kontrolü
                        if (ViewState["DuzenlemeModu"] != null)
                        {
                            // Kasa güncelleme
                            int kasaID = Convert.ToInt32(ViewState["DuzenlemeModu"]);
                            
                            string updateQuery = @"
                                UPDATE Kasalar 
                                SET KasaAdi = @KasaAdi, KasaTipi = @KasaTipi, ParaBirimiID = @ParaBirimiID, Aciklama = @Aciklama
                                WHERE KasaID = @KasaID AND SirketID = @SirketID";

                            SqlCommand updateCmd = new SqlCommand(updateQuery, conn, trans);
                            updateCmd.Parameters.AddWithValue("@KasaID", kasaID);
                            updateCmd.Parameters.AddWithValue("@SirketID", SirketID);
                            updateCmd.Parameters.AddWithValue("@KasaAdi", txtKasaAdi.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@KasaTipi", ddlKasaTipi.SelectedValue);
                            updateCmd.Parameters.AddWithValue("@ParaBirimiID", ddlParaBirimi.SelectedValue);
                            updateCmd.Parameters.AddWithValue("@Aciklama", txtAciklama.Text.Trim());

                            updateCmd.ExecuteNonQuery();
                            
                            ViewState["DuzenlemeModu"] = null;
                            
                            MessageHelper.ShowSuccessMessage(this, "Başarılı", "Kasa başarıyla güncellendi!");
                        }
                        else
                        {
                            // Kasa kodu kontrolü
                            string checkQuery = "SELECT COUNT(*) FROM Kasalar WHERE SirketID = @SirketID AND KasaKodu = @KasaKodu";
                            SqlCommand checkCmd = new SqlCommand(checkQuery, conn, trans);
                            checkCmd.Parameters.AddWithValue("@SirketID", SirketID);
                            checkCmd.Parameters.AddWithValue("@KasaKodu", txtKasaKodu.Text.Trim());

                            int mevcutSayisi = Convert.ToInt32(checkCmd.ExecuteScalar());
                            if (mevcutSayisi > 0)
                            {
                                MessageHelper.ShowWarningMessage(this, "Uyarı", "Bu kasa kodu zaten kullanılıyor!");
                                return;
                            }

                            // Yeni kasa ekleme
                            string insertQuery = @"
                                INSERT INTO Kasalar (SirketID, KasaKodu, KasaAdi, KasaTipi, ParaBirimiID, Bakiye, Aciklama, OlusturmaTarihi, AktifMi)
                                VALUES (@SirketID, @KasaKodu, @KasaAdi, @KasaTipi, @ParaBirimiID, @Bakiye, @Aciklama, GETDATE(), 1)";

                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn, trans);
                            insertCmd.Parameters.AddWithValue("@SirketID", SirketID);
                            insertCmd.Parameters.AddWithValue("@KasaKodu", txtKasaKodu.Text.Trim());
                            insertCmd.Parameters.AddWithValue("@KasaAdi", txtKasaAdi.Text.Trim());
                            insertCmd.Parameters.AddWithValue("@KasaTipi", ddlKasaTipi.SelectedValue);
                            insertCmd.Parameters.AddWithValue("@ParaBirimiID", ddlParaBirimi.SelectedValue);
                            insertCmd.Parameters.AddWithValue("@Bakiye", baslangicBakiye);
                            insertCmd.Parameters.AddWithValue("@Aciklama", txtAciklama.Text.Trim());

                            insertCmd.ExecuteNonQuery();

                            // Başlangıç bakiyesi > 0 ise kasa hareketi oluştur
                            if (baslangicBakiye > 0)
                            {
                                string hareketQuery = @"
                                    INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, Aciklama, KullaniciID, OlusturmaTarihi)
                                    SELECT @SirketID, KasaID, GETDATE(), 'G', @Tutar, 'Başlangıç bakiyesi', @KullaniciID, GETDATE()
                                    FROM Kasalar WHERE SirketID = @SirketID AND KasaKodu = @KasaKodu";

                                SqlCommand hareketCmd = new SqlCommand(hareketQuery, conn, trans);
                                hareketCmd.Parameters.AddWithValue("@SirketID", SirketID);
                                hareketCmd.Parameters.AddWithValue("@KasaKodu", txtKasaKodu.Text.Trim());
                                hareketCmd.Parameters.AddWithValue("@Tutar", baslangicBakiye);
                                hareketCmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                                hareketCmd.ExecuteNonQuery();
                            }

                            MessageHelper.ShowSuccessMessage(this, "Başarılı", "Kasa başarıyla eklendi!");
                        }

                        trans.Commit();
                        
                        // Formu temizle
                        txtKasaKodu.Text = "";
                        txtKasaAdi.Text = "";
                        txtBaslangicBakiye.Text = "0";
                        txtAciklama.Text = "";

                        // Sayfa verilerini yeniden yükle
                        YuklemeIslemi();

                        // Modal'ı kapat
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('yeniKasaModal');", true);
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasa eklenirken hata oluştu: " + ex.Message);
        }
    }

    protected void btnTransferYap_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlKaynakKasa.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kaynak kasa seçiniz!");
                return;
            }

            if (ddlHedefKasa.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Hedef kasa seçiniz!");
                return;
            }

            if (ddlKaynakKasa.SelectedValue == ddlHedefKasa.SelectedValue)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kaynak ve hedef kasa aynı olamaz!");
                return;
            }

            decimal transferTutar = 0;
            if (!decimal.TryParse(txtTransferTutar.Text, out transferTutar) || transferTutar <= 0)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Geçerli bir transfer tutarı giriniz!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Kaynak kasa bakiye kontrolü
                        string bakiyeQuery = "SELECT Bakiye FROM Kasalar WHERE KasaID = @KasaID AND SirketID = @SirketID";
                        SqlCommand bakiyeCmd = new SqlCommand(bakiyeQuery, conn, trans);
                        bakiyeCmd.Parameters.AddWithValue("@KasaID", ddlKaynakKasa.SelectedValue);
                        bakiyeCmd.Parameters.AddWithValue("@SirketID", SirketID);

                        decimal mevcutBakiye = Convert.ToDecimal(bakiyeCmd.ExecuteScalar());
                        if (mevcutBakiye < transferTutar)
                        {
                            MessageHelper.ShowWarningMessage(this, "Uyarı", "Kaynak kasada yeterli bakiye bulunmuyor!");
                            return;
                        }

                        // Kasa bakiyelerini güncelle
                        string guncelleQuery = @"
                            UPDATE Kasalar SET Bakiye = Bakiye - @Tutar WHERE KasaID = @KaynakKasaID AND SirketID = @SirketID;
                            UPDATE Kasalar SET Bakiye = Bakiye + @Tutar WHERE KasaID = @HedefKasaID AND SirketID = @SirketID;";

                        SqlCommand guncelleCmd = new SqlCommand(guncelleQuery, conn, trans);
                        guncelleCmd.Parameters.AddWithValue("@Tutar", transferTutar);
                        guncelleCmd.Parameters.AddWithValue("@KaynakKasaID", ddlKaynakKasa.SelectedValue);
                        guncelleCmd.Parameters.AddWithValue("@HedefKasaID", ddlHedefKasa.SelectedValue);
                        guncelleCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        guncelleCmd.ExecuteNonQuery();

                        trans.Commit();

                        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Transfer işlemi başarıyla tamamlandı!");

                        // Formu temizle
                        ddlKaynakKasa.SelectedIndex = 0;
                        ddlHedefKasa.SelectedIndex = 0;
                        txtTransferTutar.Text = "";
                        txtTransferAciklama.Text = "";

                        // Sayfa verilerini yeniden yükle
                        YuklemeIslemi();

                        // Modal'ı kapat
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('kasaTransferModal');", true);
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Transfer işlemi sırasında hata oluştu: " + ex.Message);
        }
    }

    protected void btnNakitIslemKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlIslemKasa.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kasa seçiniz!");
                return;
            }

            decimal islemTutar = 0;
            if (!decimal.TryParse(txtIslemTutar.Text, out islemTutar) || islemTutar <= 0)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Geçerli bir tutar giriniz!");
                return;
            }

            int ilgiliTarafID = 0;
            if (ddlIlgiliTaraf.SelectedValue != "0")
                ilgiliTarafID = Convert.ToInt32(ddlIlgiliTaraf.SelectedValue);

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Ödeme tipine göre para birimini al
                        int paraBirimiID = 1; // TL default
                        string paraBirimiQuery = "SELECT TOP 1 ParaBirimiID FROM ParaBirimileri WHERE ParaBirimi = 'TL' OR ParaBirimiAd LIKE '%Türk%'";
                        SqlCommand paraBirimiCmd = new SqlCommand(paraBirimiQuery, conn, trans);
                        object paraBirimiResult = paraBirimiCmd.ExecuteScalar();
                        if (paraBirimiResult != null)
                            paraBirimiID = Convert.ToInt32(paraBirimiResult);

                        // Nakit işlem kaydı oluştur
                        string nakitIslemQuery = @"
                            INSERT INTO NakitIslemler (
                                SirketID, IslemTuru, IslemTarihi, MusteriID, TedarikciID, MustahsilID,
                                Tutar, ParaBirimiID, OdemeTipiID, ReferansNo, Aciklama, KullaniciID
                            )
                            VALUES (
                                @SirketID, @IslemTuru, GETDATE(), @MusteriID, @TedarikciID, @MustahsilID,
                                @Tutar, @ParaBirimiID, @OdemeTipiID, @ReferansNo, @Aciklama, @KullaniciID
                            );
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand nakitIslemCmd = new SqlCommand(nakitIslemQuery, conn, trans);
                        nakitIslemCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        nakitIslemCmd.Parameters.AddWithValue("@IslemTuru", ddlIslemTuru.SelectedValue);
                        
                        // İlgili taraf parametreleri
                        if (ddlTarafTipi.SelectedValue == "M" && ilgiliTarafID > 0)
                        {
                            nakitIslemCmd.Parameters.AddWithValue("@MusteriID", ilgiliTarafID);
                            nakitIslemCmd.Parameters.AddWithValue("@TedarikciID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@MustahsilID", DBNull.Value);
                        }
                        else if (ddlTarafTipi.SelectedValue == "T" && ilgiliTarafID > 0)
                        {
                            nakitIslemCmd.Parameters.AddWithValue("@MusteriID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@TedarikciID", ilgiliTarafID);
                            nakitIslemCmd.Parameters.AddWithValue("@MustahsilID", DBNull.Value);
                        }
                        else if (ddlTarafTipi.SelectedValue == "MU" && ilgiliTarafID > 0)
                        {
                            nakitIslemCmd.Parameters.AddWithValue("@MusteriID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@TedarikciID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@MustahsilID", ilgiliTarafID);
                        }
                        else
                        {
                            nakitIslemCmd.Parameters.AddWithValue("@MusteriID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@TedarikciID", DBNull.Value);
                            nakitIslemCmd.Parameters.AddWithValue("@MustahsilID", DBNull.Value);
                        }

                        nakitIslemCmd.Parameters.AddWithValue("@Tutar", islemTutar);
                        nakitIslemCmd.Parameters.AddWithValue("@ParaBirimiID", paraBirimiID);
                        nakitIslemCmd.Parameters.AddWithValue("@OdemeTipiID", ddlOdemeTipi.SelectedValue);
                        nakitIslemCmd.Parameters.AddWithValue("@ReferansNo", string.IsNullOrEmpty(txtReferansNo.Text) ? (object)DBNull.Value : txtReferansNo.Text.Trim());
                        nakitIslemCmd.Parameters.AddWithValue("@Aciklama", string.IsNullOrEmpty(txtIslemAciklama.Text) ? (object)DBNull.Value : txtIslemAciklama.Text.Trim());
                        nakitIslemCmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);

                        int nakitIslemID = Convert.ToInt32(nakitIslemCmd.ExecuteScalar());

                        // Eğer nakit ödeme ise kasaya hareket ekle
                        if (ddlOdemeTipi.SelectedValue == "1") // Nakit ödeme
                        {
                            // Kasa hareketi ekle
                            string kasaHareketQuery = @"
                                INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, ReferansTipi, ReferansID, Aciklama, KullaniciID)
                                VALUES (@SirketID, @KasaID, GETDATE(), @IslemTipi, @Tutar, 'NakitIslem', @ReferansID, @Aciklama, @KullaniciID)";

                            SqlCommand kasaHareketCmd = new SqlCommand(kasaHareketQuery, conn, trans);
                            kasaHareketCmd.Parameters.AddWithValue("@SirketID", SirketID);
                            kasaHareketCmd.Parameters.AddWithValue("@KasaID", ddlIslemKasa.SelectedValue);
                            kasaHareketCmd.Parameters.AddWithValue("@IslemTipi", ddlIslemTuru.SelectedValue); // T veya O
                            kasaHareketCmd.Parameters.AddWithValue("@Tutar", islemTutar);
                            kasaHareketCmd.Parameters.AddWithValue("@ReferansID", nakitIslemID);
                            kasaHareketCmd.Parameters.AddWithValue("@Aciklama", txtIslemAciklama.Text.Trim());
                            kasaHareketCmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                            kasaHareketCmd.ExecuteNonQuery();

                            // Kasa bakiyesini güncelle
                            string kasaGuncelleQuery = "";
                            if (ddlIslemTuru.SelectedValue == "T") // Tahsilat
                                kasaGuncelleQuery = "UPDATE Kasalar SET Bakiye = Bakiye + @Tutar WHERE KasaID = @KasaID";
                            else // Ödeme
                                kasaGuncelleQuery = "UPDATE Kasalar SET Bakiye = Bakiye - @Tutar WHERE KasaID = @KasaID";

                            SqlCommand kasaGuncelleCmd = new SqlCommand(kasaGuncelleQuery, conn, trans);
                            kasaGuncelleCmd.Parameters.AddWithValue("@Tutar", islemTutar);
                            kasaGuncelleCmd.Parameters.AddWithValue("@KasaID", ddlIslemKasa.SelectedValue);
                            kasaGuncelleCmd.ExecuteNonQuery();
                        }

                        trans.Commit();

                        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Nakit işlem başarıyla kaydedildi!");

                        // Formu temizle
                        ddlIslemKasa.SelectedIndex = 0;
                        ddlIlgiliTaraf.SelectedIndex = 0;
                        txtIslemTutar.Text = "";
                        txtReferansNo.Text = "";
                        txtIslemAciklama.Text = "";

                        // Sayfa verilerini yeniden yükle
                        YuklemeIslemi();

                        // Modal'ı kapat
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('nakitIslemModal');", true);
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Nakit işlem kaydedilirken hata oluştu: " + ex.Message);
        }
    }

    protected void rptKasalar_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Hareketler")
            {
                int kasaID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("KasaHareketleri.aspx?KasaID=" + kasaID);
            }
            else if (e.CommandName == "Duzenle")
            {
                int kasaID = Convert.ToInt32(e.CommandArgument);
                KasaDuzenle(kasaID);
            }
            else if (e.CommandName == "Sil")
            {
                int kasaID = Convert.ToInt32(e.CommandArgument);
                KasaSil(kasaID);
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İşlem sırasında hata oluştu: " + ex.Message);
        }
    }

    private void KasaDuzenle(int kasaID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT KasaKodu, KasaAdi, KasaTipi, ParaBirimiID, Aciklama FROM Kasalar WHERE KasaID = @KasaID AND SirketID = @SirketID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@KasaID", kasaID);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);
                
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    // Formu doldur
                    txtKasaKodu.Text = reader["KasaKodu"].ToString();
                    txtKasaAdi.Text = reader["KasaAdi"].ToString();
                    ddlKasaTipi.SelectedValue = reader["KasaTipi"].ToString();
                    ddlParaBirimi.SelectedValue = reader["ParaBirimiID"].ToString();
                    txtAciklama.Text = reader["Aciklama"].ToString();
                    txtBaslangicBakiye.Text = "0";
                    
                    // Düzenleme modunu belirlemek için hidden field kullan
                    ViewState["DuzenlemeModu"] = kasaID;
                    
                    // Modal'ı aç
                    ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "$('#yeniKasaModal').modal('show');", true);
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasa bilgileri yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void KasaSil(int kasaID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                // Önce kasa hareketlerini kontrol et
                string checkQuery = "SELECT COUNT(*) FROM KasaHareketler WHERE KasaID = @KasaID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@KasaID", kasaID);
                
                conn.Open();
                int hareketSayisi = Convert.ToInt32(checkCmd.ExecuteScalar());
                
                if (hareketSayisi > 0)
                {
                    MessageHelper.ShowWarningMessage(this, "Uyarı", "Bu kasanın hareketleri bulunduğu için silinemez! Kasayı pasif yapabilirsiniz.");
                    return;
                }

                // Kasayı pasif yap
                string updateQuery = "UPDATE Kasalar SET AktifMi = 0 WHERE KasaID = @KasaID AND SirketID = @SirketID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@KasaID", kasaID);
                updateCmd.Parameters.AddWithValue("@SirketID", SirketID);
                updateCmd.ExecuteNonQuery();

                MessageHelper.ShowSuccessMessage(this, "Başarılı", "Kasa başarıyla silindi!");
                YuklemeIslemi();
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasa silinirken hata oluştu: " + ex.Message);
        }
    }

    protected void btnRaporla_Click(object sender, EventArgs e)
    {
        try
        {
            MessageHelper.ShowInfoMessage(this, "Bilgi", "Excel raporu özelliği yakında eklenecektir.");
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Rapor oluşturulurken hata oluştu: " + ex.Message);
        }
    }

    [WebMethod]
    public static string GetIlgiliTaraflar(string tarafTipi)
    {
        try
        {
            int sirketID = 0;
            if (HttpContext.Current.Session["SirketID"] != null)
                sirketID = Convert.ToInt32(HttpContext.Current.Session["SirketID"]);

            if (sirketID == 0)
                return "[]";

            List<object> liste = new List<object>();

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "";
                
                if (tarafTipi == "M") // Müşteri
                {
                    query = "SELECT MusteriID as ID, MusteriAdi as Ad FROM Musteriler WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY MusteriAdi";
                }
                else if (tarafTipi == "T") // Tedarikçi
                {
                    query = "SELECT TedarikciID as ID, TedarikciAdi as Ad FROM Tedarikciler WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY TedarikciAdi";
                }
                else if (tarafTipi == "MU") // Müstahsil
                {
                    query = "SELECT MustahsilID as ID, ISNULL(MustahsilAdi, AdSoyad) as Ad FROM Mustahsiller WHERE SirketID = @SirketID AND Aktif = 1 ORDER BY ISNULL(MustahsilAdi, AdSoyad)";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SirketID", sirketID);
                    
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        liste.Add(new { 
                            ID = reader["ID"].ToString(), 
                            Ad = reader["Ad"].ToString() 
                        });
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(liste);
        }
        catch (Exception ex)
        {
            return "[]";
        }
    }
}