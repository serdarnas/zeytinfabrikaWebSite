using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class fabrika_Zeytinyagi_ZeytinKabul : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Master Page'e başlık bilgisini ayarla
        if (Master is fabrika_FabrikaMasterPage)
        {
            fabrika_FabrikaMasterPage master = (fabrika_FabrikaMasterPage)Master;
            master.KlasorAdi = "Zeytinyağı";
            master.SayfaAdi = "Zeytin Kabul";
        }

        if (!IsPostBack)
        {
            // ID parametresi kontrolü
            int zeytinyagiUretimID;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out zeytinyagiUretimID))
            {
                // Detay görünümü
                Response.Redirect(string.Format("ZeytinKabulYeni.aspx?id={0}&mode=readonly", zeytinyagiUretimID));
            }
            else
            {
                // Normal liste yükleme
                LoadZeytinRecords();
            }
        }
    }

    // Durum sınıfı ayarlama
    protected string GetStatusBadgeClass(string status)
    {
        switch (status)
        {
            case "Beklemede":
                return "badge bg-warning text-dark";
            case "İşleniyor":
                return "badge bg-primary";
            case "Tamamlandı":
                return "badge bg-success";
            default:
                return "badge bg-secondary";
        }
    }

    // Durum metni ayarlama
    protected string GetStatusText(string status)
    {
        switch (status)
        {
            case "Beklemede":
                return "<i class=\"fa fa-hourglass-half\"></i> Beklemede";
            case "İşleniyor":
                return "<i class=\"fa fa-cogs\"></i> İşleniyor";
            case "Tamamlandı":
                return "<i class=\"fa fa-check-circle\"></i> Tamamlandı";
            default:
                return status;
        }
    }

    // Arama yapıldığında listeyi yenile
    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        LoadZeytinRecords();
    }

    // GridView sayfalama
    protected void gvZeytinler_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvZeytinler.PageIndex = e.NewPageIndex;
        LoadZeytinRecords();
    }

    // Zeytin üretim kayıtlarını yükleme
    private void LoadZeytinRecords()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Arama filtresini kontrol et
                string filter = string.Empty;
                string searchTerm = string.Empty;
                if (!string.IsNullOrEmpty(txtArama.Text))
                {
                    searchTerm = "%" + txtArama.Text.Trim() + "%";
                    filter = @" WHERE z.PartiNo LIKE @Search 
                             OR (m.Ad + ' ' + m.Soyad) LIKE @Search 
                             OR z.PlakaNo LIKE @Search 
                             OR EXISTS (SELECT 1 FROM ZeytinBoxKasalari zb 
                                      JOIN ZeytinyagiUretimi_ZeytinBoxKasa_Map map ON zb.ZeytinBoxKasaID = map.ZeytinBoxKasaID 
                                      WHERE map.ZeytinyagiUretimID = z.ZeytinyagiUretimID 
                                      AND CONVERT(VARCHAR, zb.ZeytinBoxNo) LIKE @Search)";
                }

                // SQL Server 2017+ için STRING_AGG fonksiyonu
                string query = @"SELECT z.ZeytinyagiUretimID, z.PartiNo, (m.Ad + ' ' + m.Soyad) AS MustahsilAdi, z.PlakaNo, z.GelisTarihi, 
                               z.GelisKg, i.islem_Ad, u.UrunAdi,
                               (SELECT STRING_AGG(CONVERT(VARCHAR, zb.ZeytinBoxNo), ', ') 
                                FROM ZeytinBoxKasalari zb 
                                JOIN ZeytinyagiUretimi_ZeytinBoxKasa_Map map ON zb.ZeytinBoxKasaID = map.ZeytinBoxKasaID 
                                WHERE map.ZeytinyagiUretimID = z.ZeytinyagiUretimID) AS ZeytinBoxNo,
                               CASE 
                                  WHEN z.UretimBaslamaZamani IS NULL THEN 'Beklemede'
                                  WHEN z.UretimBitisZamani IS NULL THEN 'İşleniyor'
                                  ELSE 'Tamamlandı'
                               END AS Durum
                               FROM ZeytinyagiUretimleri z
                               LEFT JOIN Mustahsiller m ON z.MustahsilID = m.MustahsilID
                               LEFT JOIN ZeytinyagiUretim_islemleri i ON z.ZeytinyagiUretim_islemID = i.ZeytinyagiUretim_islemID
                               LEFT JOIN Urunler u ON z.UrunID = u.UrunID"
                               + filter +
                               " ORDER BY z.GelisTarihi DESC";

                // Alternatif query (SQL Server 2016 ve öncesi için)
                string alternativeQuery = @"SELECT z.ZeytinyagiUretimID, z.PartiNo, (m.Ad + ' ' + m.Soyad) AS MustahsilAdi, z.PlakaNo, z.GelisTarihi, 
                               z.GelisKg, i.islem_Ad, u.UrunAdi,
                               STUFF((SELECT ', ' + CONVERT(VARCHAR, zb.ZeytinBoxNo) 
                                    FROM ZeytinBoxKasalari zb 
                                    JOIN ZeytinyagiUretimi_ZeytinBoxKasa_Map map ON zb.ZeytinBoxKasaID = map.ZeytinBoxKasaID 
                                    WHERE map.ZeytinyagiUretimID = z.ZeytinyagiUretimID
                                    FOR XML PATH('')), 1, 2, '') AS ZeytinBoxNo,
                               CASE 
                                  WHEN z.UretimBaslamaZamani IS NULL THEN 'Beklemede'
                                  WHEN z.UretimBitisZamani IS NULL THEN 'İşleniyor'
                                  ELSE 'Tamamlandı'
                               END AS Durum
                               FROM ZeytinyagiUretimleri z
                               LEFT JOIN Mustahsiller m ON z.MustahsilID = m.MustahsilID
                               LEFT JOIN ZeytinyagiUretim_islemleri i ON z.ZeytinyagiUretim_islemID = i.ZeytinyagiUretim_islemID
                               LEFT JOIN Urunler u ON z.UrunID = u.UrunID"
                               + filter +
                               " ORDER BY z.GelisTarihi DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        cmd.Parameters.AddWithValue("@Search", searchTerm);
                    }

                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        gvZeytinler.DataSource = dt;
                        gvZeytinler.DataBind();
                    }
                    catch (SqlException)
                    {
                        // STRING_AGG fonksiyonu olmayan eski SQL Server sürümleri için alternatif sorguyu dene
                        cmd.CommandText = alternativeQuery;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        gvZeytinler.DataSource = dt;
                        gvZeytinler.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Bildirim ile hata mesajı gösterme
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ErrorNotification", 
                string.Format("showErrorMessage('Hata', '{0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }

    // GridView komut işlemleri
    protected void gvZeytinler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Detay" || e.CommandName == "Duzenle" || e.CommandName == "IslemBaslat")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "Detay")
            {
                // Detay görünümü
                Response.Redirect(string.Format("ZeytinKabulYeni.aspx?id={0}&mode=readonly", id));
            }
            else if (e.CommandName == "Duzenle")
            {
                // Düzenleme modu - ZeytinKabulYeni sayfasına yönlendir
                Response.Redirect(string.Format("ZeytinKabulYeni.aspx?id={0}", id));
            }
            else if (e.CommandName == "IslemBaslat")
            {
                // Üretim başlatma
                StartProduction(id);
            }
        }
    }

    // Üretim başlatma işlemi
    private void StartProduction(int zeytinyagiUretimID)
    {
        try
        {
            // PartiMakineSecimi.aspx sayfasına yönlendir
            Response.Redirect(string.Format("PartiMakineSecimi.aspx?id={0}", zeytinyagiUretimID));
        }
        catch (Exception ex)
        {
            // Bildirim ile hata mesajı gösterme
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ErrorNotification", 
                string.Format("showErrorMessage('Hata', '{0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }
} 