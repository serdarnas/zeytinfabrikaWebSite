using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class fabrika_Nakit_NakitIslemler : System.Web.UI.Page
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
            OdemeTipleriniYukle();
            TarihFiltreleriniAyarla();
            NakitIslemleriniYukle();
            IstatistikleriHesapla();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken hata oluştu: " + ex.Message);
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

                ddlOdemeTipiFiltre.DataTextField = "OdemeTipiAdi";
                ddlOdemeTipiFiltre.DataValueField = "OdemeTipiID";
                ddlOdemeTipiFiltre.DataSource = dt;
                ddlOdemeTipiFiltre.DataBind();
                ddlOdemeTipiFiltre.Items.Insert(0, new ListItem("Tümü", ""));
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

        txtBaslangicTarihi.Text = ayinBaslangici.ToString("yyyy-MM-dd");
        txtBitisTarihi.Text = ayinSonu.ToString("yyyy-MM-dd");
    }

    private void NakitIslemleriniYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        n.NakitIslemID,
                        n.IslemTuru,
                        n.IslemTarihi,
                        n.Tutar,
                        p.ParaBirimi,
                        ot.OdemeTipiAdi as OdemeTipi,
                        n.ReferansNo,
                        n.Aciklama,
                        CASE 
                            WHEN n.MusteriID IS NOT NULL THEN m.MusteriAdi
                            WHEN n.TedarikciID IS NOT NULL THEN t.TedarikciAdi
                            WHEN n.MustahsilID IS NOT NULL THEN ISNULL(mu.MustahsilAdi, mu.AdSoyad)
                            ELSE 'Genel İşlem'
                        END as IlgiliTaraf
                    FROM NakitIslemler n
                    LEFT JOIN ParaBirimileri p ON n.ParaBirimiID = p.ParaBirimiID
                    LEFT JOIN OdemeTipleri ot ON n.OdemeTipiID = ot.OdemeTipiID
                    LEFT JOIN Musteriler m ON n.MusteriID = m.MusteriID
                    LEFT JOIN Tedarikciler t ON n.TedarikciID = t.TedarikciID
                    LEFT JOIN Mustahsiller mu ON n.MustahsilID = mu.MustahsilID
                    WHERE n.SirketID = @SirketID";

                // Filtreler
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SirketID", SirketID));

                if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                {
                    query += " AND n.IslemTarihi >= @BaslangicTarihi";
                    parameters.Add(new SqlParameter("@BaslangicTarihi", DateTime.Parse(txtBaslangicTarihi.Text)));
                }

                if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                {
                    DateTime bitisTarihi = DateTime.Parse(txtBitisTarihi.Text).AddDays(1).AddSeconds(-1);
                    query += " AND n.IslemTarihi <= @BitisTarihi";
                    parameters.Add(new SqlParameter("@BitisTarihi", bitisTarihi));
                }

                if (!string.IsNullOrEmpty(ddlIslemTuruFiltre.SelectedValue))
                {
                    query += " AND n.IslemTuru = @IslemTuru";
                    parameters.Add(new SqlParameter("@IslemTuru", ddlIslemTuruFiltre.SelectedValue));
                }

                if (!string.IsNullOrEmpty(ddlOdemeTipiFiltre.SelectedValue))
                {
                    query += " AND n.OdemeTipiID = @OdemeTipiID";
                    parameters.Add(new SqlParameter("@OdemeTipiID", ddlOdemeTipiFiltre.SelectedValue));
                }

                query += " ORDER BY n.IslemTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddRange(parameters.ToArray());
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptNakitIslemler.DataSource = dt;
                    rptNakitIslemler.DataBind();
                    pnlVeriYok.Visible = false;
                }
                else
                {
                    rptNakitIslemler.DataSource = null;
                    rptNakitIslemler.DataBind();
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Nakit işlemleri yüklenirken hata oluştu: " + ex.Message);
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
                        COUNT(*) as IslemSayisi,
                        ISNULL(SUM(CASE WHEN IslemTuru = 'T' THEN Tutar ELSE 0 END), 0) as ToplamTahsilat,
                        ISNULL(SUM(CASE WHEN IslemTuru = 'O' THEN Tutar ELSE 0 END), 0) as ToplamOdeme
                    FROM NakitIslemler 
                    WHERE SirketID = @SirketID";

                // Filtreler (istatistik için aynı filtreler uygulanır)
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SirketID", SirketID));

                if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                {
                    query += " AND IslemTarihi >= @BaslangicTarihi";
                    parameters.Add(new SqlParameter("@BaslangicTarihi", DateTime.Parse(txtBaslangicTarihi.Text)));
                }

                if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                {
                    DateTime bitisTarihi = DateTime.Parse(txtBitisTarihi.Text).AddDays(1).AddSeconds(-1);
                    query += " AND IslemTarihi <= @BitisTarihi";
                    parameters.Add(new SqlParameter("@BitisTarihi", bitisTarihi));
                }

                if (!string.IsNullOrEmpty(ddlIslemTuruFiltre.SelectedValue))
                {
                    query += " AND IslemTuru = @IslemTuru";
                    parameters.Add(new SqlParameter("@IslemTuru", ddlIslemTuruFiltre.SelectedValue));
                }

                if (!string.IsNullOrEmpty(ddlOdemeTipiFiltre.SelectedValue))
                {
                    query += " AND OdemeTipiID = @OdemeTipiID";
                    parameters.Add(new SqlParameter("@OdemeTipiID", ddlOdemeTipiFiltre.SelectedValue));
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int islemSayisi = Convert.ToInt32(reader["IslemSayisi"]);
                    decimal toplamTahsilat = Convert.ToDecimal(reader["ToplamTahsilat"]);
                    decimal toplamOdeme = Convert.ToDecimal(reader["ToplamOdeme"]);
                    decimal netAkis = toplamTahsilat - toplamOdeme;

                    lblIslemSayisi.Text = islemSayisi.ToString();
                    lblToplamTahsilat.Text = toplamTahsilat.ToString("N2");
                    lblToplamOdeme.Text = toplamOdeme.ToString("N2");
                    lblNetAkis.Text = netAkis.ToString("N2");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İstatistikler hesaplanırken hata oluştu: " + ex.Message);
        }
    }

    protected void btnFiltrele_Click(object sender, EventArgs e)
    {
        try
        {
            NakitIslemleriniYukle();
            IstatistikleriHesapla();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Filtreleme sırasında hata oluştu: " + ex.Message);
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        try
        {
            // Filtreleri temizle
            txtBaslangicTarihi.Text = "";
            txtBitisTarihi.Text = "";
            ddlIslemTuruFiltre.SelectedIndex = 0;
            ddlOdemeTipiFiltre.SelectedIndex = 0;

            // Varsayılan tarih aralığını ayarla
            TarihFiltreleriniAyarla();

            // Verileri yeniden yükle
            NakitIslemleriniYukle();
            IstatistikleriHesapla();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Filtreler temizlenirken hata oluştu: " + ex.Message);
        }
    }
}
