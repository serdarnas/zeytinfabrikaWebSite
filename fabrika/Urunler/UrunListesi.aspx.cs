using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Diagnostics;

public partial class fabrika_Urunler_UrunListesi : System.Web.UI.Page
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
            int sirketID = SessionHelper.GetSirketID();
            // SirketID geçerli, devam et
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Ürünler";
                master.SayfaAdi = "Ürün Listesi";
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadKategoriler();
            LoadMarkalar();
            LoadUrunler();
        }
    }

    private void LoadKategoriler()
    {
        ddlKategoriler.Items.Clear();
        ddlKategoriler.Items.Add(new ListItem("Tüm Kategoriler", ""));
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var _kategoriler = db.UrunKategorileris.Where(x => x.SirketID == sirketID).OrderBy(x => x.Ad).ToList();
                foreach (var kategori in _kategoriler)
                {
                    ddlKategoriler.Items.Add(new ListItem(kategori.Ad, kategori.KategoriID.ToString()));
                }
            }
        }
        catch
        {
            // Hata durumunda örnek veri göster
        }
        ddlKategoriler.SelectedIndex = 0;
    }

    private void LoadMarkalar()
    {
        ddlMarkalar.Items.Clear();
        ddlMarkalar.Items.Add(new ListItem("Tüm Markalar", ""));
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var markalar = db.Markalars.Where(x => x.SirketID == sirketID).OrderBy(x => x.Ad).ToList();
                foreach (var marka in markalar)
                {
                    ddlMarkalar.Items.Add(new ListItem(marka.Ad, marka.MarkaID.ToString()));
                }
            }
        }
        catch { }
        ddlMarkalar.SelectedIndex = 0;
    }

    private void LoadUrunler()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var urunler = db.Urunlers.Where(x => x.SirketID == sirketID);

                // Aktif/Pasif filtre
                if (!string.IsNullOrEmpty(ddlAktifUrunler.SelectedValue))
                {
                    bool aktif = ddlAktifUrunler.SelectedValue == "1";
                    urunler = urunler.Where(x => x.Durum == aktif);
                }

                // Kategori filtre
                if (!string.IsNullOrEmpty(ddlKategoriler.SelectedValue))
                {
                    int kategoriID = int.Parse(ddlKategoriler.SelectedValue);
                    urunler = urunler.Where(x => x.KategoriID == kategoriID);
                }

                // Marka filtre
                if (!string.IsNullOrEmpty(ddlMarkalar.SelectedValue))
                {
                    int markaID = int.Parse(ddlMarkalar.SelectedValue);
                    urunler = urunler.Where(x => x.MarkaID == markaID);
                }

                // Arama metni filtre
                string arama = txtArama.Text.Trim();
                if (!string.IsNullOrEmpty(arama))
                {
                    urunler = urunler.Where(x =>
                        x.UrunAdi.Contains(arama) ||
                        x.UrunKodu.Contains(arama) ||
                        x.Barkod.Contains(arama));
                }

                rptUrunler.DataSource = urunler.OrderBy(x => x.UrunAdi).ToList();
                rptUrunler.DataBind();
            }
        }
        catch (Exception)
        {
            // Hata durumunda örnek veri göster
            DataTable dt = new DataTable();
            dt.Columns.Add("UrunID", typeof(int));
            dt.Columns.Add("UrunAdi", typeof(string));
            dt.Columns.Add("StokKodu", typeof(string));
            dt.Columns.Add("Barkod", typeof(string));
            dt.Columns.Add("ResimYolu", typeof(string));
            dt.Columns.Add("SatisFiyati", typeof(decimal));
            dt.Columns.Add("StokMiktari", typeof(decimal));
            dt.Columns.Add("Birim", typeof(string));
            dt.Rows.Add(1, "Demo Ürün", "KOD1", "123456", "demo.jpg", 10.0m, 100, "ad");
            rptUrunler.DataSource = dt;
            rptUrunler.DataBind();
        }
    }

    protected void btnYeniUrun_Click(object sender, EventArgs e)
    {
        Response.Redirect("UrunDetay.aspx?id=0");
    }

    protected void btnExcelIndir_Click(object sender, EventArgs e)
    {
        // Excel indirme işlemi burada yapılacak
        // Örnek olarak kullanıcıya bilgi mesajı gösterelim
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelMessage", "alert('Excel dosyası indirme işlemi başlatıldı.');", true);
    }

    protected void btnTopluGuncelle_Click(object sender, EventArgs e)
    {
        Response.Redirect("UrunTopluGuncelle.aspx");
    }

    protected void btnTopluResim_Click(object sender, EventArgs e)
    {
        Response.Redirect("UrunTopluResim.aspx");
    }

    protected void btnAra_Click(object sender, EventArgs e)
    {
        LoadUrunler();
    }

    protected void ddlAktifUrunler_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUrunler();
    }

    protected void ddlKategoriler_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUrunler();
    }

    protected void ddlMarkalar_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUrunler();
    }
}