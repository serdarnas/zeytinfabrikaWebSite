using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class fabrika_Nakit_KasaHareketleri : System.Web.UI.Page
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

    private int KasaID
    {
        get
        {
            if (Request.QueryString["KasaID"] != null)
                return Convert.ToInt32(Request.QueryString["KasaID"]);
            return 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SirketID == 0 || KasaID == 0)
            {
                Response.Redirect("Kasalar.aspx");
                return;
            }

            YuklemeIslemi();
        }
    }

    private void YuklemeIslemi()
    {
        try
        {
            KasaBilgileriniYukle();
            TarihFiltreleriniAyarla();
            KasaHareketleriniYukle();
            IstatistikleriHesapla();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void KasaBilgileriniYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT k.KasaKodu, k.KasaAdi, k.Bakiye, p.ParaBirimi
                    FROM Kasalar k
                    LEFT JOIN ParaBirimileri p ON k.ParaBirimiID = p.ParaBirimiID
                    WHERE k.KasaID = @KasaID AND k.SirketID = @SirketID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@KasaID", KasaID);
                cmd.Parameters.AddWithValue("@SirketID", SirketID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblKasaKodu.Text = reader["KasaKodu"].ToString();
                    lblKasaAdi.Text = reader["KasaAdi"].ToString();
                    lblMevcutBakiye.Text = Convert.ToDecimal(reader["Bakiye"]).ToString("N2");
                    
                    string paraBirimi = reader["ParaBirimi"].ToString();
                    lblParaBirimi.Text = paraBirimi;
                    lblParaBirimi2.Text = paraBirimi;
                    lblParaBirimi3.Text = paraBirimi;
                }
                else
                {
                    MessageHelper.ShowErrorMessage(this, "Hata", "Kasa bulunamadı!");
                    Response.Redirect("Kasalar.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasa bilgileri yüklenirken hata oluştu: " + ex.Message);
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

    private void KasaHareketleriniYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                string query = @"
                    SELECT 
                        kh.HareketID,
                        kh.IslemTarihi,
                        kh.IslemTipi,
                        kh.Tutar,
                        ISNULL(kh.ReferansTipi, 'Genel') as ReferansTipi,
                        kh.ReferansID,
                        kh.Aciklama
                    FROM KasaHareketler kh
                    WHERE kh.KasaID = @KasaID AND kh.SirketID = @SirketID";

                // Filtreler
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@KasaID", KasaID));
                parameters.Add(new SqlParameter("@SirketID", SirketID));

                if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                {
                    query += " AND kh.IslemTarihi >= @BaslangicTarihi";
                    parameters.Add(new SqlParameter("@BaslangicTarihi", DateTime.Parse(txtBaslangicTarihi.Text)));
                }

                if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                {
                    DateTime bitisTarihi = DateTime.Parse(txtBitisTarihi.Text).AddDays(1).AddSeconds(-1);
                    query += " AND kh.IslemTarihi <= @BitisTarihi";
                    parameters.Add(new SqlParameter("@BitisTarihi", bitisTarihi));
                }

                if (!string.IsNullOrEmpty(ddlHareketTipi.SelectedValue))
                {
                    query += " AND kh.IslemTipi = @IslemTipi";
                    parameters.Add(new SqlParameter("@IslemTipi", ddlHareketTipi.SelectedValue));
                }

                query += " ORDER BY kh.IslemTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddRange(parameters.ToArray());
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptKasaHareketleri.DataSource = dt;
                    rptKasaHareketleri.DataBind();
                    pnlVeriYok.Visible = false;
                }
                else
                {
                    rptKasaHareketleri.DataSource = null;
                    rptKasaHareketleri.DataBind();
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kasa hareketleri yüklenirken hata oluştu: " + ex.Message);
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
                        ISNULL(SUM(CASE WHEN IslemTipi = 'G' THEN Tutar ELSE 0 END), 0) as ToplamGiris,
                        ISNULL(SUM(CASE WHEN IslemTipi = 'C' THEN Tutar ELSE 0 END), 0) as ToplamCikis
                    FROM KasaHareketler 
                    WHERE KasaID = @KasaID AND SirketID = @SirketID";

                // Filtreler (istatistik için aynı filtreler uygulanır)
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@KasaID", KasaID));
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

                if (!string.IsNullOrEmpty(ddlHareketTipi.SelectedValue))
                {
                    query += " AND IslemTipi = @IslemTipi";
                    parameters.Add(new SqlParameter("@IslemTipi", ddlHareketTipi.SelectedValue));
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal toplamGiris = Convert.ToDecimal(reader["ToplamGiris"]);
                    decimal toplamCikis = Convert.ToDecimal(reader["ToplamCikis"]);

                    lblToplamGiris.Text = toplamGiris.ToString("N2");
                    lblToplamCikis.Text = toplamCikis.ToString("N2");
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
            KasaHareketleriniYukle();
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
            ddlHareketTipi.SelectedIndex = 0;

            // Varsayılan tarih aralığını ayarla
            TarihFiltreleriniAyarla();

            // Verileri yeniden yükle
            KasaHareketleriniYukle();
            IstatistikleriHesapla();
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Filtreler temizlenirken hata oluştu: " + ex.Message);
        }
    }
}
