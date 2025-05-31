using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class fabrika_Zeytinyagi_ZeytinBoxKasaOlustur : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Master Page'e başlık bilgisini ayarla
        if (Master is fabrika_FabrikaMasterPage)
        {
            fabrika_FabrikaMasterPage master = (fabrika_FabrikaMasterPage)Master;
            master.KlasorAdi = "Zeytinyağı";
            master.SayfaAdi = "Zeytin Box Kasa Oluştur";
        }

        if (!IsPostBack)
        {
            // Tarih kontrolünü bugünün tarihine ayarla
            txtAlimTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
            // Kasa listesini yükle
            LoadBoxKasalar();
        }
    }

    /// <summary>
    /// Zeytin Box Kasa listesini yükler
    /// </summary>
    private void LoadBoxKasalar()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Arama filtresini kontrol et
                string filter = string.Empty;
                if (!string.IsNullOrEmpty(txtArama.Text))
                {
                    filter = " WHERE z.ZeytinBoxNo LIKE @Search OR m.Ad + ' ' + m.Soyad LIKE @Search";
                }

                string query = @"SELECT z.ZeytinBoxKasaID, z.ZeytinBoxNo, z.Durumu, z.AlimTarihi, 
                                m.Ad + ' ' + m.Soyad AS MustahsilAdi
                                FROM ZeytinBoxKasalari z
                                LEFT JOIN Mustahsiller m ON z.KulananMustahsilID = m.MustahsilID"
                                + filter +
                                " ORDER BY z.ZeytinBoxNo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + txtArama.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvBoxKasalar.DataSource = dt;
                    gvBoxKasalar.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            // ShowError("Kayıtlar yüklenirken bir hata oluştu: " + ex.Message);
        }
    }

    /// <summary>
    /// Yeni Kasa Üret düğmesine tıklandığında
    /// </summary>
    protected void btnYeniKasa_Click(object sender, EventArgs e)
    {
        pnlYeniKasa.Visible = true;
        txtKasaSayisi.Text = "1";
        txtAlimTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// İptal düğmesine tıklandığında
    /// </summary>
    protected void btnIptal_Click(object sender, EventArgs e)
    {
        pnlYeniKasa.Visible = false;
    }

    /// <summary>
    /// Kasaları Oluştur düğmesine tıklandığında
    /// </summary>
    protected void btnOlustur_Click(object sender, EventArgs e)
    {
        try
        {
            // Form validasyonu
            if (!Page.IsValid)
                return;

            int kasaSayisi = Convert.ToInt32(txtKasaSayisi.Text);
            if (kasaSayisi <= 0 || kasaSayisi > 100)
            {
                // ShowError("Geçerli bir kasa sayısı girin (1-100 arası).");
                return;
            }

            DateTime alimTarihi = Convert.ToDateTime(txtAlimTarihi.Text);
            
            // SirketID al
            int sirketID = SessionHelper.GetSirketID();
            
            // Son kasa numarasını bul
            int sonBoxNo = GetSonBoxNo();
            
            // Bağlantı kur
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Transaction başlat
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int basariliKayit = 0;
                        
                        // Kasaları oluştur
                        for (int i = 0; i < kasaSayisi; i++)
                        {
                            sonBoxNo++;
                            
                            // ZeytinBoxKasaID alanını sorguda belirtmiyoruz, bu ID IDENTITY olduğu için
                            // otomatik olarak veritabanı tarafından oluşturulacak
                            string query = @"INSERT INTO ZeytinBoxKasalari 
                                          (SirketID, ZeytinBoxNo, Durumu, AlimTarihi) 
                                          VALUES 
                                          (@SirketID, @ZeytinBoxNo, @Durumu, @AlimTarihi)";
                            
                            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SirketID", sirketID);
                                cmd.Parameters.AddWithValue("@ZeytinBoxNo", sonBoxNo);
                                cmd.Parameters.AddWithValue("@Durumu", true);
                                cmd.Parameters.AddWithValue("@AlimTarihi", alimTarihi);
                                
                                int result = cmd.ExecuteNonQuery();
                                if (result > 0)
                                    basariliKayit++;
                            }
                        }
                        
                        // İşlem başarılı ise commit
                        transaction.Commit();
                        
                        // Başarı mesajı göster
                        // ShowSuccess(String.Format("{0} adet zeytin box kasa başarıyla oluşturuldu.", basariliKayit));
                        
                        // Panel gizle ve listeyi yenile
                        pnlYeniKasa.Visible = false;
                        LoadBoxKasalar();
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda rollback
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // ShowError("Kasa oluşturulurken bir hata oluştu: " + ex.Message);
        }
    }

    /// <summary>
    /// Son zeytin box numarasını getirir
    /// </summary>
    private int GetSonBoxNo()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(ZeytinBoxNo), 0) FROM ZeytinBoxKasalari";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            // ShowError("Son kasa numarası alınırken bir hata oluştu: " + ex.Message);
            return 0;
        }
    }

    /// <summary>
    /// Arama kutusunda metin değiştiğinde çalışır
    /// </summary>
    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        LoadBoxKasalar();
    }

    /// <summary>
    /// GridView'da sayfalama olayı
    /// </summary>
    protected void gvBoxKasalar_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBoxKasalar.PageIndex = e.NewPageIndex;
        LoadBoxKasalar();
    }

    /// <summary>
    /// GridView'da komut tetiklendiğinde
    /// </summary>
    protected void gvBoxKasalar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Duzenle")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            // Düzenleme sayfasına yönlendir - {id} yerine gerçek id değerini kullan
            Response.Redirect("ZeytinBoxKasaDuzenle.aspx?id=" + id);
        }
        else if (e.CommandName == "Sil")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            DeleteBoxKasa(id);
        }
    }

    /// <summary>
    /// Belirtilen ID'ye sahip box kasayı siler
    /// </summary>
    private void DeleteBoxKasa(int boxKasaID)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ZeytinBoxKasalari WHERE ZeytinBoxKasaID = @ID AND Durumu = 1";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", boxKasaID);
                    
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        // ShowSuccess("Zeytin box kasa başarıyla silindi.");
                        LoadBoxKasalar();
                    }
                    else
                    {
                        // ShowError("Kasa silinemedi. Kasa kullanımda olabilir.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // ShowError("Kasa silinirken bir hata oluştu: " + ex.Message);
        }
    }
}