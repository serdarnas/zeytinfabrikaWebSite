using System;
using System.Linq;

public partial class fabrika_Musteriler_MusteriListesi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
            int sirketID = SessionHelper.GetSirketID();
            // SirketID geçerli, devam et
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }
        if (!IsPostBack)
        {
            LoadMusteriKategorileri();
            MusteriListele();
        }
    }

    private void LoadMusteriKategorileri()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            var kategoriler = db.MusteriKategorileris
                .Where(k => k.SirketID == sirketID)
                .OrderBy(k => k.KategoriAdi)
                .Select(k => new { k.KategoriID, k.KategoriAdi })
                .ToList();
            
            ddlSinif.Items.Clear();
            ddlSinif.Items.Add(new System.Web.UI.WebControls.ListItem("Tüm Kategoriler", ""));
            
            foreach (var kategori in kategoriler)
            {
                ddlSinif.Items.Add(new System.Web.UI.WebControls.ListItem(kategori.KategoriAdi, kategori.KategoriID.ToString()));
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda varsayılan olarak sadece "Tüm Kategoriler" seçeneğini ekle
            ddlSinif.Items.Clear();
            ddlSinif.Items.Add(new System.Web.UI.WebControls.ListItem("Tüm Kategoriler", ""));
        }
    }

    private void MusteriListele()
    {
        ddlDurum.SelectedValue = "1";
        int sirketID = SessionHelper.GetSirketID();
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        
        // Durum filtresini al
        bool durum = ddlDurum.SelectedValue == "1"; // "1" ise true, diğer durumlarda false
        
        // Kategori filtresini al
        int? kategoriID = null;
        if (!string.IsNullOrEmpty(ddlSinif.SelectedValue))
        {
            kategoriID = Convert.ToInt32(ddlSinif.SelectedValue);
        }
        
        // Arama metnini al
        string aramaMetni = txtArama.Text.Trim();
        
        // Sorguyu oluştur
        var query = db.Musterilers.Where(x => x.SirketID == sirketID && x.Durum == durum);
        
        // Kategori filtresi varsa uygula
        if (kategoriID.HasValue)
        {
            query = query.Where(x => x.KategoriID == kategoriID.Value);
        }
        
        // Arama filtresi varsa uygula (en az 3 karakter)
        if (!string.IsNullOrEmpty(aramaMetni) && aramaMetni.Length >= 3)
        {
            query = query.Where(x => 
                x.FirmaAdi.Contains(aramaMetni) || 
                x.YetkiliAdi.Contains(aramaMetni) || 
                x.Telefon.Contains(aramaMetni) || 
                x.CepTelefonu.Contains(aramaMetni) || 
                x.Email.Contains(aramaMetni) || 
                x.VergiNo.Contains(aramaMetni) || 
                x.MüsteriKodu.Contains(aramaMetni));
        }
        
        // Sonuçları al
        var musteriler = query.Select(x => new
        {
            MusteriID = x.MusteriID,
            x.FirmaAdi,
            x.Telefon,
            KategoriAdi = x.MusteriKategorileri != null ? x.MusteriKategorileri.KategoriAdi : "",
            AcikBakiye = FabrikaTools.Musteri.Acikbakiye(x.MusteriID),
            CekSenetBakiye = FabrikaTools.Musteri.CekSenetBakiye(x.MusteriID)
        }).ToList();

        rptMusteriler.DataSource = musteriler;
        rptMusteriler.DataBind();
        lblToplamMusteri.Text = musteriler.Count.ToString();
    }

    protected void ddlDurum_SelectedIndexChanged(object sender, EventArgs e)
    {
        MusteriListele();
    }
    
    protected void ddlSinif_SelectedIndexChanged(object sender, EventArgs e)
    {
        MusteriListele();
    }
    
    protected void btnAra_Click(object sender, EventArgs e)
    {
        // Arama butonuna tıklandığında müşteri listesini güncelle
        MusteriListele();
    }
}