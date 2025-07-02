using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class fabrika_Nakit_Cekler : System.Web.UI.Page
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

            // Tabloların varlığını kontrol et, yoksa uyarı ver
            if (!TabloKontrolEt())
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Çek tabloları henüz oluşturulmamış. Lütfen veritabanı yöneticinizle iletişime geçin.");
                return;
            }

            YuklemeIslemi();
        }
    }

    private bool TabloKontrolEt()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME IN ('Cekler', 'CekHareketleri', 'CekDurumlari', 'CekIslemTipleri')";

                SqlCommand cmd = new SqlCommand(checkQuery, conn);
                conn.Open();
                int tabloSayisi = Convert.ToInt32(cmd.ExecuteScalar());
                
                return tabloSayisi >= 4;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void YuklemeIslemi()
    {
        try
        {
            ParaBirimleriniYukle();
            MusterileriYukle();
            TedarikcileriYukle();
            FinansalKurumlariYukle();
            TarihFiltreleriniAyarla();
            CekleriYukle();
            IstatistikleriHesapla();
            PortfoydeCekleriYukle();
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

                // Filtre için
                ddlMusteri.DataTextField = "MusteriAdi";
                ddlMusteri.DataValueField = "MusteriID";
                ddlMusteri.DataSource = dt;
                ddlMusteri.DataBind();
                ddlMusteri.Items.Insert(0, new ListItem("Tümü", ""));

                // Çek ekleme için
                ddlCekMusteri.DataTextField = "MusteriAdi";
                ddlCekMusteri.DataValueField = "MusteriID";
                ddlCekMusteri.DataSource = dt;
                ddlCekMusteri.DataBind();
                ddlCekMusteri.Items.Insert(0, new ListItem("Seçiniz...", "0"));
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

    private void FinansalKurumlariYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = "SELECT FinansalKurumID, KurumAdi FROM FinansalKurumlar WHERE SirketID = @SirketID AND AktifMi = 1 ORDER BY KurumAdi";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ViewState["FinansalKurumListesi"] = dt;
            }
        }
        catch (Exception ex)
        {
            // Finansal kurumlar tablosu henüz oluşturulmamış olabilir
            ViewState["FinansalKurumListesi"] = new DataTable();
        }
    }

    private void TarihFiltreleriniAyarla()
    {
        // Bu ayın başı ve sonu varsayılan
        DateTime bugun = DateTime.Now;
        DateTime ayinBaslangici = new DateTime(bugun.Year, bugun.Month, 1);
        DateTime ayinSonu = ayinBaslangici.AddMonths(1).AddDays(-1);

        txtBaslangicTarihi.Text = ayinBaslangici.ToString("yyyy-MM-dd");
        txtBitisTarihi.Text = ayinSonu.ToString("yyyy-MM-dd");

        // Bugünün tarihi varsayılanlar
        txtAlisTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtKesimTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    private void CekleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        c.CekID,
                        c.SeriNo,
                        c.BankaAdi,
                        c.SubeAdi,
                        c.HesapNo,
                        c.Kesideci,
                        c.Tutar,
                        c.VadeTarihi,
                        c.KesideTarihi,
                        c.AlisTarihi,
                        c.OdemeYeri,
                        c.DurumID,
                        c.Aciklama,
                        m.MusteriAdi,
                        p.ParaBirimi
                    FROM Cekler c
                    LEFT JOIN Musteriler m ON c.AlinanMusteriID = m.MusteriID
                    LEFT JOIN ParaBirimileri p ON c.ParaBirimiID = p.ParaBirimiID
                    WHERE c.SirketID = @SirketID";

                // Filtreler
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SirketID", SirketID));

                if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                {
                    query += " AND c.AlisTarihi >= @BaslangicTarihi";
                    parameters.Add(new SqlParameter("@BaslangicTarihi", DateTime.Parse(txtBaslangicTarihi.Text)));
                }

                if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                {
                    DateTime bitisTarihi = DateTime.Parse(txtBitisTarihi.Text).AddDays(1).AddSeconds(-1);
                    query += " AND c.AlisTarihi <= @BitisTarihi";
                    parameters.Add(new SqlParameter("@BitisTarihi", bitisTarihi));
                }

                if (!string.IsNullOrEmpty(ddlDurum.SelectedValue))
                {
                    query += " AND c.DurumID = @DurumID";
                    parameters.Add(new SqlParameter("@DurumID", ddlDurum.SelectedValue));
                }

                if (!string.IsNullOrEmpty(ddlMusteri.SelectedValue))
                {
                    query += " AND c.AlinanMusteriID = @MusteriID";
                    parameters.Add(new SqlParameter("@MusteriID", ddlMusteri.SelectedValue));
                }

                query += " ORDER BY c.VadeTarihi DESC, c.AlisTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddRange(parameters.ToArray());
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptCekler.DataSource = dt;
                    rptCekler.DataBind();
                    pnlVeriYok.Visible = false;
                }
                else
                {
                    rptCekler.DataSource = null;
                    rptCekler.DataBind();
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Çekler yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void IstatistikleriHesapla()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        COUNT(*) as ToplamCek,
                        ISNULL(SUM(CASE WHEN DurumID = 1 THEN Tutar ELSE 0 END), 0) as Portfoyde,
                        ISNULL(SUM(CASE WHEN DurumID = 2 THEN Tutar ELSE 0 END), 0) as CiroEdildi,
                        ISNULL(SUM(CASE WHEN DurumID = 3 THEN Tutar ELSE 0 END), 0) as TahsileVerildi,
                        ISNULL(SUM(CASE WHEN DurumID = 4 THEN Tutar ELSE 0 END), 0) as Faktoring,
                        ISNULL(SUM(CASE WHEN DurumID = 5 THEN Tutar ELSE 0 END), 0) as TahsilEdildi,
                        ISNULL(SUM(CASE WHEN DurumID = 6 THEN Tutar ELSE 0 END), 0) as Karsilik
                    FROM Cekler 
                    WHERE SirketID = @SirketID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblToplamCek.Text = reader["ToplamCek"].ToString();
                    lblPortfoyde.Text = Convert.ToDecimal(reader["Portfoyde"]).ToString("N2");
                    lblCiroEdildi.Text = Convert.ToDecimal(reader["CiroEdildi"]).ToString("N2");
                    lblTahsileVerildi.Text = Convert.ToDecimal(reader["TahsileVerildi"]).ToString("N2");
                    lblTahsilEdildi.Text = Convert.ToDecimal(reader["TahsilEdildi"]).ToString("N2");
                    lblKarsilik.Text = Convert.ToDecimal(reader["Karsilik"]).ToString("N2");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İstatistikler hesaplanırken hata oluştu: " + ex.Message);
        }
    }

    private void PortfoydeCekleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT CekID, SeriNo + ' - ' + BankaAdi + ' (' + CONVERT(VARCHAR, Tutar) + ' TL)' as CekBilgi
                    FROM Cekler 
                    WHERE SirketID = @SirketID AND DurumID = 1
                    ORDER BY VadeTarihi";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCekSecim.DataTextField = "CekBilgi";
                ddlCekSecim.DataValueField = "CekID";
                ddlCekSecim.DataSource = dt;
                ddlCekSecim.DataBind();
                ddlCekSecim.Items.Insert(0, new ListItem("Seçiniz...", "0"));
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda log yazılabilir
        }
    }

    protected string GetDurumClass(string durumID)
    {
        switch (durumID)
        {
            case "1": return "portfoyde";
            case "2": return "ciro";
            case "3": return "tahsil";
            case "4": return "faktoring";
            case "5": return "tahsil-edildi";
            case "6": return "karsilik";
            default: return "portfoyde";
        }
    }

    protected string GetDurumText(string durumID)
    {
        switch (durumID)
        {
            case "1": return "Portföyde";
            case "2": return "Ciro Edildi";
            case "3": return "Tahsile Verildi";
            case "4": return "Faktöringe Verildi";
            case "5": return "Tahsil Edildi";
            case "6": return "Karşılıksız";
            default: return "Bilinmeyen";
        }
    }

    protected string GetVadeDurumClass(object vadeTarihi)
    {
        try
        {
            DateTime vade = Convert.ToDateTime(vadeTarihi);
            DateTime bugun = DateTime.Now.Date;
            
            if (vade < bugun)
                return "vade-gecen";
            else if (vade <= bugun.AddDays(7))
                return "vade-yaklasan";
            else
                return "";
        }
        catch
        {
            return "";
        }
    }

    protected void btnCekKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCekMusteri.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Müşteri seçiniz!");
                return;
            }

            if (string.IsNullOrEmpty(txtSeriNo.Text.Trim()))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Seri no giriniz!");
                return;
            }

            decimal tutar = 0;
            if (!decimal.TryParse(txtTutar.Text, out tutar) || tutar <= 0)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Geçerli bir tutar giriniz!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Çek kaydı
                        string cekQuery = @"
                            INSERT INTO Cekler (
                                SirketID, AlinanMusteriID, AlisTarihi, SeriNo, BankaAdi, SubeAdi, 
                                HesapNo, Kesideci, Tutar, ParaBirimiID, VadeTarihi, KesideTarihi, 
                                OdemeYeri, DurumID, Aciklama, OlusturmaTarihi
                            )
                            VALUES (
                                @SirketID, @AlinanMusteriID, @AlisTarihi, @SeriNo, @BankaAdi, @SubeAdi,
                                @HesapNo, @Kesideci, @Tutar, @ParaBirimiID, @VadeTarihi, @KesideTarihi,
                                @OdemeYeri, 1, @Aciklama, GETDATE()
                            );
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cekCmd = new SqlCommand(cekQuery, conn, trans);
                        cekCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        cekCmd.Parameters.AddWithValue("@AlinanMusteriID", ddlCekMusteri.SelectedValue);
                        cekCmd.Parameters.AddWithValue("@AlisTarihi", DateTime.Parse(txtAlisTarihi.Text));
                        cekCmd.Parameters.AddWithValue("@SeriNo", txtSeriNo.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@BankaAdi", txtBankaAdi.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@SubeAdi", txtSubeAdi.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@HesapNo", txtHesapNo.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@Kesideci", txtKesideci.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@Tutar", tutar);
                        cekCmd.Parameters.AddWithValue("@ParaBirimiID", ddlParaBirimi.SelectedValue);
                        cekCmd.Parameters.AddWithValue("@VadeTarihi", DateTime.Parse(txtVadeTarihi.Text));
                        cekCmd.Parameters.AddWithValue("@KesideTarihi", DateTime.Parse(txtKesimTarihi.Text));
                        cekCmd.Parameters.AddWithValue("@OdemeYeri", txtOdemeYeri.Text.Trim());
                        cekCmd.Parameters.AddWithValue("@Aciklama", txtCekAciklama.Text.Trim());

                        int cekID = Convert.ToInt32(cekCmd.ExecuteScalar());

                        // Çek hareketi kaydı
                        string hareketQuery = @"
                            INSERT INTO CekHareketleri (
                                SirketID, CekID, IslemTarihi, IslemTipiID, IlgiliMusteriID, 
                                Tutar, Aciklama
                            )
                            VALUES (
                                @SirketID, @CekID, GETDATE(), 10, @IlgiliMusteriID, 
                                @Tutar, @Aciklama
                            )";

                        SqlCommand hareketCmd = new SqlCommand(hareketQuery, conn, trans);
                        hareketCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        hareketCmd.Parameters.AddWithValue("@CekID", cekID);
                        hareketCmd.Parameters.AddWithValue("@IlgiliMusteriID", ddlCekMusteri.SelectedValue);
                        hareketCmd.Parameters.AddWithValue("@Tutar", tutar);
                        hareketCmd.Parameters.AddWithValue("@Aciklama", "Müşteriden çek alındı");
                        hareketCmd.ExecuteNonQuery();

                        trans.Commit();

                        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Çek başarıyla kaydedildi!");

                        // Formu temizle
                        ModalTemizle();
                        YuklemeIslemi();

                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('yeniCekModal');", true);
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
            MessageHelper.ShowErrorMessage(this, "Hata", "Çek kaydedilirken hata oluştu: " + ex.Message);
        }
    }

    private void ModalTemizle()
    {
        ddlCekMusteri.SelectedIndex = 0;
        txtSeriNo.Text = "";
        txtTutar.Text = "";
        txtBankaAdi.Text = "";
        txtSubeAdi.Text = "";
        txtHesapNo.Text = "";
        txtKesideci.Text = "";
        txtOdemeYeri.Text = "";
        txtCekAciklama.Text = "";
        txtAlisTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtKesimTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtVadeTarihi.Text = "";
    }

    protected void btnIslemKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCekSecim.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Çek seçiniz!");
                return;
            }

            if (string.IsNullOrEmpty(ddlIslemTipi.SelectedValue))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "İşlem tipi seçiniz!");
                return;
            }

            // İşlem tipine göre işlem yap
            CekIslemYap(Convert.ToInt32(ddlCekSecim.SelectedValue), Convert.ToInt32(ddlIslemTipi.SelectedValue));

            MessageHelper.ShowSuccessMessage(this, "Başarılı", "Çek işlemi başarıyla gerçekleştirildi!");
            YuklemeIslemi();
            ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('cekIslemModal');", true);
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Çek işlemi sırasında hata oluştu: " + ex.Message);
        }
    }

    private void CekIslemYap(int cekID, int islemTipiID)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
        {
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    // Çek durumunu güncelle
                    int yeniDurum = GetYeniDurum(islemTipiID);
                    
                    string updateQuery = "UPDATE Cekler SET DurumID = @YeniDurum WHERE CekID = @CekID";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn, trans);
                    updateCmd.Parameters.AddWithValue("@YeniDurum", yeniDurum);
                    updateCmd.Parameters.AddWithValue("@CekID", cekID);
                    updateCmd.ExecuteNonQuery();

                    // Hareket kaydı ekle
                    string hareketQuery = @"
                        INSERT INTO CekHareketleri (
                            SirketID, CekID, IslemTarihi, IslemTipiID, Tutar, Aciklama
                        )
                        SELECT @SirketID, @CekID, GETDATE(), @IslemTipiID, Tutar, @Aciklama
                        FROM Cekler WHERE CekID = @CekID";

                    SqlCommand hareketCmd = new SqlCommand(hareketQuery, conn, trans);
                    hareketCmd.Parameters.AddWithValue("@SirketID", SirketID);
                    hareketCmd.Parameters.AddWithValue("@CekID", cekID);
                    hareketCmd.Parameters.AddWithValue("@IslemTipiID", islemTipiID);
                    hareketCmd.Parameters.AddWithValue("@Aciklama", txtIslemAciklama.Text.Trim());
                    hareketCmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }

    private int GetYeniDurum(int islemTipiID)
    {
        switch (islemTipiID)
        {
            case 20: return 2; // Tedarikçiye ciro edildi
            case 30: return 3; // Bankaya tahsile verildi
            case 40: return 4; // Faktöringe verildi
            case 50: return 5; // Tahsil edildi
            case 60: return 6; // Karşılıksız
            default: return 1; // Portföyde
        }
    }

    protected void rptCekler_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Detay")
            {
                int cekID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("CekDetay.aspx?CekID=" + cekID);
            }
            else if (e.CommandName == "Islem")
            {
                int cekID = Convert.ToInt32(e.CommandArgument);
                ddlCekSecim.SelectedValue = cekID.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "$('#cekIslemModal').modal('show');", true);
            }
            else if (e.CommandName == "Duzenle")
            {
                int cekID = Convert.ToInt32(e.CommandArgument);
                // Düzenleme modalını aç
                ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "$('#yeniCekModal').modal('show');", true);
            }
            else if (e.CommandName == "Sil")
            {
                int cekID = Convert.ToInt32(e.CommandArgument);
                CekSil(cekID);
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İşlem sırasında hata oluştu: " + ex.Message);
        }
    }

    private void CekSil(int cekID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string deleteQuery = "DELETE FROM Cekler WHERE CekID = @CekID AND SirketID = @SirketID AND DurumID = 1";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@CekID", cekID);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);
                
                conn.Open();
                int etkilenen = cmd.ExecuteNonQuery();
                
                if (etkilenen > 0)
                {
                    MessageHelper.ShowSuccessMessage(this, "Başarılı", "Çek başarıyla silindi!");
                    YuklemeIslemi();
                }
                else
                {
                    MessageHelper.ShowWarningMessage(this, "Uyarı", "Sadece portföydeki çekler silinebilir!");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Çek silinirken hata oluştu: " + ex.Message);
        }
    }

    protected void btnFiltrele_Click(object sender, EventArgs e)
    {
        CekleriYukle();
        IstatistikleriHesapla();
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        txtBaslangicTarihi.Text = "";
        txtBitisTarihi.Text = "";
        ddlDurum.SelectedIndex = 0;
        ddlMusteri.SelectedIndex = 0;
        
        TarihFiltreleriniAyarla();
        CekleriYukle();
        IstatistikleriHesapla();
    }

    protected void btnFinansalKurumlar_Click(object sender, EventArgs e)
    {
        MessageHelper.ShowInfoMessage(this, "Bilgi", "Finansal kurumlar yönetimi yakında eklenecektir.");
    }

    protected void btnExcelRapor_Click(object sender, EventArgs e)
    {
        MessageHelper.ShowInfoMessage(this, "Bilgi", "Excel raporu özelliği yakında eklenecektir.");
    }
} 