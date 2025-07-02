using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class fabrika_Nakit_Senetler : System.Web.UI.Page
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

            // Tabloların varlığını kontrol et
            if (!TabloKontrolEt())
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Senet tabloları henüz oluşturulmamış. Lütfen veritabanı yöneticinizle iletişime geçin.");
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
                    WHERE TABLE_NAME IN ('Senetler', 'SenetHareketleri', 'SenetDurumlari', 'SenetIslemTipleri')";

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
            TarihFiltreleriniAyarla();
            SenetleriYukle();
            IstatistikleriHesapla();
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

                ddlSenetParaBirimi.DataTextField = "ParaBirimiAd";
                ddlSenetParaBirimi.DataValueField = "ParaBirimiID";
                ddlSenetParaBirimi.DataSource = dt;
                ddlSenetParaBirimi.DataBind();

                // TL'yi varsayılan olarak seç
                if (ddlSenetParaBirimi.Items.FindByText("Türk Lirası") != null)
                    ddlSenetParaBirimi.SelectedValue = ddlSenetParaBirimi.Items.FindByText("Türk Lirası").Value;
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

                ddlSenetMusteri.DataTextField = "MusteriAdi";
                ddlSenetMusteri.DataValueField = "MusteriID";
                ddlSenetMusteri.DataSource = dt;
                ddlSenetMusteri.DataBind();
                ddlSenetMusteri.Items.Insert(0, new ListItem("Seçiniz...", "0"));
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

    private void TarihFiltreleriniAyarla()
    {
        // Bu ayın başı ve sonu varsayılan
        DateTime bugun = DateTime.Now;
        DateTime ayinBaslangici = new DateTime(bugun.Year, bugun.Month, 1);
        DateTime ayinSonu = ayinBaslangici.AddMonths(1).AddDays(-1);

        txtAlinanBaslangic.Text = ayinBaslangici.ToString("yyyy-MM-dd");
        txtAlinanBitis.Text = ayinSonu.ToString("yyyy-MM-dd");

        // Bugünün tarihi varsayılanlar
        txtSenetDuzenlemeTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    private void SenetleriYukle()
    {
        try
        {
            AlinanSenetleriYukle();
            VerilenSenetleriYukle();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Senetler yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void AlinanSenetleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        s.SenetID,
                        s.SeriNo,
                        s.VadeTarihi,
                        s.DuzenlemeTarihi,
                        s.Tutar,
                        s.Borclu,
                        s.OdemeYeri,
                        s.DurumID,
                        s.Aciklama,
                        m.MusteriAdi,
                        p.ParaBirimi
                    FROM Senetler s
                    LEFT JOIN Musteriler m ON s.IlgiliMusteriID = m.MusteriID
                    LEFT JOIN ParaBirimileri p ON s.ParaBirimiID = p.ParaBirimiID
                    WHERE s.SirketID = @SirketID AND s.SenetTipi = 'A'";

                // Filtreler
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SirketID", SirketID));

                if (!string.IsNullOrEmpty(txtAlinanBaslangic.Text))
                {
                    query += " AND s.DuzenlemeTarihi >= @BaslangicTarihi";
                    parameters.Add(new SqlParameter("@BaslangicTarihi", DateTime.Parse(txtAlinanBaslangic.Text)));
                }

                if (!string.IsNullOrEmpty(txtAlinanBitis.Text))
                {
                    DateTime bitisTarihi = DateTime.Parse(txtAlinanBitis.Text).AddDays(1).AddSeconds(-1);
                    query += " AND s.DuzenlemeTarihi <= @BitisTarihi";
                    parameters.Add(new SqlParameter("@BitisTarihi", bitisTarihi));
                }

                if (!string.IsNullOrEmpty(ddlAlinanDurum.SelectedValue))
                {
                    query += " AND s.DurumID = @DurumID";
                    parameters.Add(new SqlParameter("@DurumID", ddlAlinanDurum.SelectedValue));
                }

                query += " ORDER BY s.VadeTarihi ASC, s.DuzenlemeTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddRange(parameters.ToArray());
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptAlinanSenetler.DataSource = dt;
                    rptAlinanSenetler.DataBind();
                    pnlAlinanVeriYok.Visible = false;
                }
                else
                {
                    rptAlinanSenetler.DataSource = null;
                    rptAlinanSenetler.DataBind();
                    pnlAlinanVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Alınan senetler yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void VerilenSenetleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        s.SenetID,
                        s.SeriNo,
                        s.VadeTarihi,
                        s.DuzenlemeTarihi,
                        s.Tutar,
                        s.Borclu,
                        s.OdemeYeri,
                        s.DurumID,
                        s.Aciklama,
                        t.TedarikciAdi,
                        p.ParaBirimi
                    FROM Senetler s
                    LEFT JOIN Tedarikciler t ON s.IlgiliTedarikciID = t.TedarikciID
                    LEFT JOIN ParaBirimileri p ON s.ParaBirimiID = p.ParaBirimiID
                    WHERE s.SirketID = @SirketID AND s.SenetTipi = 'V'
                    ORDER BY s.VadeTarihi ASC, s.DuzenlemeTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptVerilenSenetler.DataSource = dt;
                    rptVerilenSenetler.DataBind();
                    pnlVerilenVeriYok.Visible = false;
                }
                else
                {
                    rptVerilenSenetler.DataSource = null;
                    rptVerilenSenetler.DataBind();
                    pnlVerilenVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Verilen senetler yüklenirken hata oluştu: " + ex.Message);
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
                        SUM(CASE WHEN SenetTipi = 'A' THEN Tutar ELSE 0 END) as AlinanTutar,
                        SUM(CASE WHEN SenetTipi = 'A' THEN 1 ELSE 0 END) as AlinanAdet,
                        SUM(CASE WHEN SenetTipi = 'V' THEN Tutar ELSE 0 END) as VerilenTutar,
                        SUM(CASE WHEN SenetTipi = 'V' THEN 1 ELSE 0 END) as VerilenAdet,
                        COUNT(*) as ToplamSenet
                    FROM Senetler 
                    WHERE SirketID = @SirketID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal alinanTutar = Convert.ToDecimal(reader["AlinanTutar"] ?? 0);
                    decimal verilenTutar = Convert.ToDecimal(reader["VerilenTutar"] ?? 0);
                    int alinanAdet = Convert.ToInt32(reader["AlinanAdet"] ?? 0);
                    int verilenAdet = Convert.ToInt32(reader["VerilenAdet"] ?? 0);
                    int toplamSenet = Convert.ToInt32(reader["ToplamSenet"] ?? 0);

                    lblAlinanSenet.Text = alinanTutar.ToString("N2");
                    lblAlinanAdet.Text = alinanAdet.ToString();
                    lblVerilenSenet.Text = verilenTutar.ToString("N2");
                    lblVerilenAdet.Text = verilenAdet.ToString();
                    lblToplamSenet.Text = toplamSenet.ToString();

                    decimal netDurum = alinanTutar - verilenTutar;
                    lblNetDurum.Text = netDurum.ToString("N2");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İstatistikler hesaplanırken hata oluştu: " + ex.Message);
        }
    }

    protected string GetDurumClass(string durumID)
    {
        switch (durumID)
        {
            case "10": return "portfoyde";
            case "11": return "ciro";
            case "12": return "tahsil";
            case "13": return "faktoring";
            case "14": return "tahsil-edildi";
            case "15": return "protesto";
            case "20": return "tedarikci";
            case "21": return "odendi";
            default: return "portfoyde";
        }
    }

    protected string GetDurumText(string durumID)
    {
        switch (durumID)
        {
            case "10": return "Portföyde";
            case "11": return "Ciro Edildi";
            case "12": return "Tahsile Verildi";
            case "13": return "Faktöringe Verildi";
            case "14": return "Tahsil Edildi";
            case "15": return "Protesto Edildi";
            case "20": return "Tedarikçide";
            case "21": return "Ödendi";
            default: return "Bilinmeyen";
        }
    }

    protected void btnSenetKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlSenetMusteri.SelectedValue == "0")
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Müşteri seçiniz!");
                return;
            }

            if (string.IsNullOrEmpty(txtBorclu.Text.Trim()))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Borçlu adı giriniz!");
                return;
            }

            decimal tutar = 0;
            if (!decimal.TryParse(txtSenetTutar.Text, out tutar) || tutar <= 0)
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
                        // Senet kaydı
                        string senetQuery = @"
                            INSERT INTO Senetler (
                                SirketID, SenetTipi, SeriNo, VadeTarihi, DuzenlemeTarihi, 
                                Tutar, ParaBirimiID, Borclu, OdemeYeri, IlgiliMusteriID, 
                                DurumID, Aciklama, OlusturmaTarihi
                            )
                            VALUES (
                                @SirketID, 'A', @SeriNo, @VadeTarihi, @DuzenlemeTarihi, 
                                @Tutar, @ParaBirimiID, @Borclu, @OdemeYeri, @IlgiliMusteriID, 
                                10, @Aciklama, GETDATE()
                            );
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand senetCmd = new SqlCommand(senetQuery, conn, trans);
                        senetCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        senetCmd.Parameters.AddWithValue("@SeriNo", txtSenetSeriNo.Text.Trim());
                        senetCmd.Parameters.AddWithValue("@VadeTarihi", DateTime.Parse(txtSenetVadeTarihi.Text));
                        senetCmd.Parameters.AddWithValue("@DuzenlemeTarihi", DateTime.Parse(txtSenetDuzenlemeTarihi.Text));
                        senetCmd.Parameters.AddWithValue("@Tutar", tutar);
                        senetCmd.Parameters.AddWithValue("@ParaBirimiID", ddlSenetParaBirimi.SelectedValue);
                        senetCmd.Parameters.AddWithValue("@Borclu", txtBorclu.Text.Trim());
                        senetCmd.Parameters.AddWithValue("@OdemeYeri", txtSenetOdemeYeri.Text.Trim());
                        senetCmd.Parameters.AddWithValue("@IlgiliMusteriID", ddlSenetMusteri.SelectedValue);
                        senetCmd.Parameters.AddWithValue("@Aciklama", txtSenetAciklama.Text.Trim());

                        int senetID = Convert.ToInt32(senetCmd.ExecuteScalar());

                        // Senet hareketi kaydı
                        string hareketQuery = @"
                            INSERT INTO SenetHareketleri (
                                SirketID, SenetID, IslemTarihi, IslemTipiID, IlgiliMusteriID, 
                                Tutar, Aciklama
                            )
                            VALUES (
                                @SirketID, @SenetID, GETDATE(), 100, @IlgiliMusteriID, 
                                @Tutar, @Aciklama
                            )";

                        SqlCommand hareketCmd = new SqlCommand(hareketQuery, conn, trans);
                        hareketCmd.Parameters.AddWithValue("@SirketID", SirketID);
                        hareketCmd.Parameters.AddWithValue("@SenetID", senetID);
                        hareketCmd.Parameters.AddWithValue("@IlgiliMusteriID", ddlSenetMusteri.SelectedValue);
                        hareketCmd.Parameters.AddWithValue("@Tutar", tutar);
                        hareketCmd.Parameters.AddWithValue("@Aciklama", "Müşteriden senet alındı");
                        hareketCmd.ExecuteNonQuery();

                        trans.Commit();

                        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Senet başarıyla kaydedildi!");

                        // Formu temizle
                        ModalTemizle();
                        YuklemeIslemi();

                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeModal('alinanSenetModal');", true);
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
            MessageHelper.ShowErrorMessage(this, "Hata", "Senet kaydedilirken hata oluştu: " + ex.Message);
        }
    }

    private void ModalTemizle()
    {
        ddlSenetMusteri.SelectedIndex = 0;
        txtSenetSeriNo.Text = "";
        txtSenetTutar.Text = "";
        txtBorclu.Text = "";
        txtSenetOdemeYeri.Text = "";
        txtSenetAciklama.Text = "";
        txtSenetDuzenlemeTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtSenetVadeTarihi.Text = "";
    }

    protected void rptAlinanSenetler_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Detay")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("SenetDetay.aspx?SenetID=" + senetID);
            }
            else if (e.CommandName == "Islem")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                MessageHelper.ShowInfoMessage(this, "Bilgi", "Senet işlemleri özelliği yakında eklenecektir.");
            }
            else if (e.CommandName == "Duzenle")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                MessageHelper.ShowInfoMessage(this, "Bilgi", "Senet düzenleme özelliği yakında eklenecektir.");
            }
            else if (e.CommandName == "Sil")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                SenetSil(senetID);
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İşlem sırasında hata oluştu: " + ex.Message);
        }
    }

    protected void rptVerilenSenetler_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Detay")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("SenetDetay.aspx?SenetID=" + senetID);
            }
            else if (e.CommandName == "Ode")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                MessageHelper.ShowInfoMessage(this, "Bilgi", "Senet ödeme özelliği yakında eklenecektir.");
            }
            else if (e.CommandName == "Duzenle")
            {
                int senetID = Convert.ToInt32(e.CommandArgument);
                MessageHelper.ShowInfoMessage(this, "Bilgi", "Senet düzenleme özelliği yakında eklenecektir.");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İşlem sırasında hata oluştu: " + ex.Message);
        }
    }

    private void SenetSil(int senetID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string deleteQuery = "DELETE FROM Senetler WHERE SenetID = @SenetID AND SirketID = @SirketID AND DurumID = 10";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@SenetID", senetID);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);
                
                conn.Open();
                int etkilenen = cmd.ExecuteNonQuery();
                
                if (etkilenen > 0)
                {
                    MessageHelper.ShowSuccessMessage(this, "Başarılı", "Senet başarıyla silindi!");
                    YuklemeIslemi();
                }
                else
                {
                    MessageHelper.ShowWarningMessage(this, "Uyarı", "Sadece portföydeki senetler silinebilir!");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Senet silinirken hata oluştu: " + ex.Message);
        }
    }

    protected void btnAlinanFiltrele_Click(object sender, EventArgs e)
    {
        AlinanSenetleriYukle();
    }

    protected void btnAlinanTemizle_Click(object sender, EventArgs e)
    {
        txtAlinanBaslangic.Text = "";
        txtAlinanBitis.Text = "";
        ddlAlinanDurum.SelectedIndex = 0;
        
        TarihFiltreleriniAyarla();
        AlinanSenetleriYukle();
    }

    protected void btnAlinanExcel_Click(object sender, EventArgs e)
    {
        MessageHelper.ShowInfoMessage(this, "Bilgi", "Excel raporu özelliği yakında eklenecektir.");
    }

    protected void btnVerilenExcel_Click(object sender, EventArgs e)
    {
        MessageHelper.ShowInfoMessage(this, "Bilgi", "Excel raporu özelliği yakında eklenecektir.");
    }
} 