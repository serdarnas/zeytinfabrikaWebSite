using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class fabrika_Nakit_Default : System.Web.UI.Page
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
            IstatistikleriHesapla();
            UyarilariKontrolEt();
            SonIslemleriYukle();
            lblSonGuncelleme.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken hata oluştu: " + ex.Message);
        }
    }

    private void IstatistikleriHesapla()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();

                // Kasa bakiyeleri
                string kasaQuery = @"
                    SELECT 
                        COUNT(*) as KasaSayisi,
                        ISNULL(SUM(Bakiye), 0) as ToplamBakiye
                    FROM Kasalar 
                    WHERE SirketID = @SirketID AND AktifMi = 1";

                SqlCommand kasaCmd = new SqlCommand(kasaQuery, conn);
                kasaCmd.Parameters.AddWithValue("@SirketID", SirketID);
                SqlDataReader kasaReader = kasaCmd.ExecuteReader();

                if (kasaReader.Read())
                {
                    lblKasaSayisi.Text = kasaReader["KasaSayisi"].ToString();
                    decimal toplamKasaBakiye = Convert.ToDecimal(kasaReader["ToplamBakiye"]);
                    lblToplamKasaBakiye.Text = toplamKasaBakiye.ToString("N2");
                }
                kasaReader.Close();

                // Çek bilgileri (tablolar varsa)
                decimal portfoydeCekTutar = 0;
                int portfoydeCekAdet = 0;
                try
                {
                    string cekQuery = @"
                        SELECT 
                            COUNT(*) as CekAdet,
                            ISNULL(SUM(Tutar), 0) as CekTutar
                        FROM Cekler 
                        WHERE SirketID = @SirketID AND DurumID = 1"; // Portföyde

                    SqlCommand cekCmd = new SqlCommand(cekQuery, conn);
                    cekCmd.Parameters.AddWithValue("@SirketID", SirketID);
                    SqlDataReader cekReader = cekCmd.ExecuteReader();

                    if (cekReader.Read())
                    {
                        portfoydeCekAdet = Convert.ToInt32(cekReader["CekAdet"]);
                        portfoydeCekTutar = Convert.ToDecimal(cekReader["CekTutar"]);
                    }
                    cekReader.Close();
                }
                catch (Exception)
                {
                    // Çek tabloları henüz oluşturulmamış
                }

                lblCekAdet.Text = portfoydeCekAdet.ToString();
                lblPortfoydeCek.Text = portfoydeCekTutar.ToString("N2");

                // Senet bilgileri (tablolar varsa)
                decimal portfoydeSenetTutar = 0;
                int portfoydeSenetAdet = 0;
                try
                {
                    string senetQuery = @"
                        SELECT 
                            COUNT(*) as SenetAdet,
                            ISNULL(SUM(Tutar), 0) as SenetTutar
                        FROM Senetler 
                        WHERE SirketID = @SirketID AND SenetTipi = 'A' AND DurumID = 10"; // Alınan senetler, portföyde

                    SqlCommand senetCmd = new SqlCommand(senetQuery, conn);
                    senetCmd.Parameters.AddWithValue("@SirketID", SirketID);
                    SqlDataReader senetReader = senetCmd.ExecuteReader();

                    if (senetReader.Read())
                    {
                        portfoydeSenetAdet = Convert.ToInt32(senetReader["SenetAdet"]);
                        portfoydeSenetTutar = Convert.ToDecimal(senetReader["SenetTutar"]);
                    }
                    senetReader.Close();
                }
                catch (Exception)
                {
                    // Senet tabloları henüz oluşturulmamış
                }

                lblSenetAdet.Text = portfoydeSenetAdet.ToString();
                lblPortfoydeSenet.Text = portfoydeSenetTutar.ToString("N2");

                // Toplam finansal değer
                decimal toplamDeger = Convert.ToDecimal(lblToplamKasaBakiye.Text) + portfoydeCekTutar + portfoydeSenetTutar;
                lblToplamDeger.Text = toplamDeger.ToString("N2");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "İstatistikler hesaplanırken hata oluştu: " + ex.Message);
        }
    }

    private void UyarilariKontrolEt()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                conn.Open();

                // Vadesi yaklaşan çekler (7 gün içinde)
                try
                {
                    string vadesiYaklasanCekQuery = @"
                        SELECT COUNT(*) as Adet, ISNULL(SUM(Tutar), 0) as Tutar
                        FROM Cekler 
                        WHERE SirketID = @SirketID AND DurumID IN (1,3) 
                        AND VadeTarihi BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())";

                    SqlCommand cmd = new SqlCommand(vadesiYaklasanCekQuery, conn);
                    cmd.Parameters.AddWithValue("@SirketID", SirketID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int adet = Convert.ToInt32(reader["Adet"]);
                        if (adet > 0)
                        {
                            decimal tutar = Convert.ToDecimal(reader["Tutar"]);
                            ltVadesiYaklasan.Text = string.Format("{0} adet çek (Toplam: {1:N2} TL) vadesi 7 gün içinde dolacak.", adet, tutar);
                            pnlVadesiYaklasan.Visible = true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    // Çek tabloları henüz oluşturulmamış
                }

                // Vadesi geçen çekler
                try
                {
                    string vadesiGecenCekQuery = @"
                        SELECT COUNT(*) as Adet, ISNULL(SUM(Tutar), 0) as Tutar
                        FROM Cekler 
                        WHERE SirketID = @SirketID AND DurumID IN (1,3) 
                        AND VadeTarihi < GETDATE()";

                    SqlCommand cmd = new SqlCommand(vadesiGecenCekQuery, conn);
                    cmd.Parameters.AddWithValue("@SirketID", SirketID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int adet = Convert.ToInt32(reader["Adet"]);
                        if (adet > 0)
                        {
                            decimal tutar = Convert.ToDecimal(reader["Tutar"]);
                            ltVadesiGecen.Text = string.Format("{0} adet çek (Toplam: {1:N2} TL) vadesi geçmiş!", adet, tutar);
                            pnlVadesiGecen.Visible = true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    // Çek tabloları henüz oluşturulmamış
                }

                // Düşük bakiyeli kasalar (1000 TL altı)
                string dusukBakiyeQuery = @"
                    SELECT COUNT(*) as Adet
                    FROM Kasalar 
                    WHERE SirketID = @SirketID AND AktifMi = 1 AND Bakiye < 1000";

                SqlCommand dusukCmd = new SqlCommand(dusukBakiyeQuery, conn);
                dusukCmd.Parameters.AddWithValue("@SirketID", SirketID);
                int dusukBakiyeAdet = Convert.ToInt32(dusukCmd.ExecuteScalar());

                if (dusukBakiyeAdet > 0)
                {
                    ltDusukBakiye.Text = string.Format("{0} kasanın bakiyesi 1.000 TL'nin altında.", dusukBakiyeAdet);
                    pnlDusukBakiye.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            // Uyarı kontrollerindeki hatalar kritik değil, log'lanabilir
        }
    }

    private void SonIslemleriYukle()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                // Son 10 nakit işlemi
                string query = @"
                    SELECT TOP 10 
                        IslemTarihi as Tarih,
                        CASE IslemTuru 
                            WHEN 'T' THEN 'Tahsilat'
                            WHEN 'O' THEN 'Ödeme'
                        END as Tip,
                        CASE 
                            WHEN MusteriID IS NOT NULL THEN 'Müşteri: ' + ISNULL(m.MusteriAdi, 'Bilinmeyen')
                            WHEN TedarikciID IS NOT NULL THEN 'Tedarikçi: ' + ISNULL(t.TedarikciAdi, 'Bilinmeyen')
                            WHEN MustahsilID IS NOT NULL THEN 'Müstahsil: ' + ISNULL(mu.MustahsilAdi, 'Bilinmeyen')
                            ELSE ISNULL(Aciklama, 'Genel İşlem')
                        END as Aciklama,
                        Tutar
                    FROM NakitIslemler ni
                    LEFT JOIN Musteriler m ON ni.MusteriID = m.MusteriID
                    LEFT JOIN Tedarikciler t ON ni.TedarikciID = t.TedarikciID
                    LEFT JOIN Mustahsiller mu ON ni.MustahsilID = mu.MustahsilID
                    WHERE ni.SirketID = @SirketID
                    ORDER BY ni.IslemTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SirketID", SirketID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptSonIslemler.DataSource = dt;
                    rptSonIslemler.DataBind();
                    pnlSonIslemYok.Visible = false;
                }
                else
                {
                    rptSonIslemler.DataSource = null;
                    rptSonIslemler.DataBind();
                    pnlSonIslemYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlSonIslemYok.Visible = true;
        }
    }

    protected string GetIslemTipiClass(string tip)
    {
        switch (tip)
        {
            case "Tahsilat":
                return "success";
            case "Ödeme":
                return "danger";
            default:
                return "default";
        }
    }

    protected void btnBugunkuIslemler_Click(object sender, EventArgs e)
    {
        string bugunkuTarih = DateTime.Now.ToString("yyyy-MM-dd");
        Response.Redirect(string.Format("NakitIslemler.aspx?baslangic={0}&bitis={1}", bugunkuTarih, bugunkuTarih));
    }

    protected void btnRaporlar_Click(object sender, EventArgs e)
    {
        MessageHelper.ShowInfoMessage(this, "Bilgi", "Detaylı raporlar özelliği yakında eklenecektir.");
    }

    protected void btnYenile_Click(object sender, EventArgs e)
    {
        YuklemeIslemi();
        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Veriler yenilendi!");
    }
} 