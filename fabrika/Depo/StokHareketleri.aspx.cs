using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_StokHareketleri : System.Web.UI.Page
{
    private int depoID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Depo";
                master.SayfaAdi = "Stok Hareketleri";
            }

            // QueryString'den depo ID'sini al
            if (Request.QueryString["depoID"] != null && int.TryParse(Request.QueryString["depoID"], out depoID))
            {
                if (!IsPostBack)
                {
                    DepoAdiniBelirle();
                    UrunleriYukle();
                    StokHareketleriniListele();
                    TarihFiltreleriniAyarla();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            // Log hatası
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }

    private void DepoAdiniBelirle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var depo = db.Depolars.FirstOrDefault(d => d.DepoID == depoID && d.SirketID == sirketID);
                if (depo != null)
                {
                    lblDepoAdi.Text = depo.DepoAdi + " - Stok Hareketleri";
                }
            }
        }
        catch (Exception ex)
        {
            lblDepoAdi.Text = "Stok Hareketleri";
        }
    }

    private void TarihFiltreleriniAyarla()
    {
        // Son 30 günü varsayılan olarak ayarla
        txtBitisTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtBaslangicTarihi.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
    }

    private void UrunleriYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var urunler = db.Urunlers.Where(u => u.SirketID == sirketID && u.Durum == true)
                                        .OrderBy(u => u.UrunAdi)
                                        .ToList();

                ddlUrun.DataSource = urunler;
                ddlUrun.DataBind();
                ddlUrun.Items.Insert(0, new ListItem("Ürün Seçin", ""));
            }
        }
        catch (Exception ex)
        {
            // Log hatası
        }
    }

    private void StokHareketleriniListele()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var query = from sh in db.StokHareketleris
                           join u in db.Urunlers on sh.UrunID equals u.UrunID
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           join k in db.Kullanicilars on sh.KullaniciID equals k.KullaniciID into kullaniciGroup
                           from k in kullaniciGroup.DefaultIfEmpty()
                           where sh.DepoID == depoID && sh.SirketID == sirketID
                           select new
                           {
                               sh.HareketID,
                               sh.IslemTarihi,
                               UrunAdi = u.UrunAdi,
                               sh.HareketTipi,
                               sh.Miktar,
                               Birim = b != null ? b.BirimAdi : "",
                               sh.Aciklama,
                               KullaniciAdi = k != null ? k.AdSoyad : "Sistem"
                           };

                // Hareket tipi filtresi
                if (!string.IsNullOrEmpty(ddlHareketTipi.SelectedValue))
                {
                    query = query.Where(x => x.HareketTipi == ddlHareketTipi.SelectedValue);
                }

                // Tarih filtresi
                DateTime baslangic, bitis;
                if (DateTime.TryParse(txtBaslangicTarihi.Text, out baslangic))
                {
                    query = query.Where(x => x.IslemTarihi >= baslangic);
                }
                if (DateTime.TryParse(txtBitisTarihi.Text, out bitis))
                {
                    bitis = bitis.AddDays(1); // Bitiş gününü dahil et
                    query = query.Where(x => x.IslemTarihi < bitis);
                }

                // Arama filtresi
                if (!string.IsNullOrEmpty(txtArama.Text.Trim()))
                {
                    string arama = txtArama.Text.Trim().ToLower();
                    query = query.Where(x => x.UrunAdi.ToLower().Contains(arama) || 
                                           x.Aciklama.ToLower().Contains(arama));
                }

                var hareketler = query.OrderByDescending(x => x.IslemTarihi).ToList();

                if (hareketler.Count > 0)
                {
                    rptStokHareketleri.DataSource = hareketler;
                    rptStokHareketleri.DataBind();
                    pnlHareketYok.Visible = false;
                    lblToplamHareket.Text = hareketler.Count.ToString();
                }
                else
                {
                    rptStokHareketleri.DataSource = null;
                    rptStokHareketleri.DataBind();
                    pnlHareketYok.Visible = true;
                    lblToplamHareket.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            // Log hatası
            pnlHareketYok.Visible = true;
            lblToplamHareket.Text = "0";
        }
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoDetay.aspx?id=" + depoID);
    }

    protected void btnStokGirisi_Click(object sender, EventArgs e)
    {
        lblModalBaslik.Text = "Stok Girişi";
        hfHareketID.Value = "";
        txtMiktar.Text = "";
        txtAciklama.Text = "Stok girişi";
        ddlUrun.SelectedIndex = 0;
        pnlStokHareketForm.Visible = true;
        
        ScriptManager.RegisterStartupScript(this, GetType(), "showModal", 
            "$('#stokHareketModal').modal('show');", true);
    }

    protected void btnStokCikisi_Click(object sender, EventArgs e)
    {
        lblModalBaslik.Text = "Stok Çıkışı";
        hfHareketID.Value = "";
        txtMiktar.Text = "";
        txtAciklama.Text = "Stok çıkışı";
        ddlUrun.SelectedIndex = 0;
        pnlStokHareketForm.Visible = true;
        
        ScriptManager.RegisterStartupScript(this, GetType(), "showModal", 
            "$('#stokHareketModal').modal('show');", true);
    }

    protected void btnHareketKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int sirketID = SessionHelper.GetSirketID();
                int kullaniciID = SessionHelper.GetKullaniciID();
                
                using (var db = new FabrikaDataClassesDataContext())
                {
                    var hareket = new StokHareketleri
                    {
                        SirketID = sirketID,
                        DepoID = depoID,
                        UrunID = Convert.ToInt32(ddlUrun.SelectedValue),
                        HareketTipi = lblModalBaslik.Text.Contains("Giriş") ? "GIRIS" : "CIKIS",
                        Miktar = lblModalBaslik.Text.Contains("Giriş") ? 
                                Convert.ToDecimal(txtMiktar.Text) : 
                                -Convert.ToDecimal(txtMiktar.Text),
                        IslemTarihi = DateTime.Now,
                        Aciklama = txtAciklama.Text.Trim(),
                        KullaniciID = kullaniciID
                    };

                    db.StokHareketleris.InsertOnSubmit(hareket);
                    db.SubmitChanges();

                    // Depo stok tablosunu güncelle
                    StokGuncelle(db, Convert.ToInt32(ddlUrun.SelectedValue), hareket.Miktar);

                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal", 
                        "$('#stokHareketModal').modal('hide');", true);
                    
                    StokHareketleriniListele();
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "alert('Stok hareketi başarıyla kaydedildi.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Hata: " + ex.Message + "');", true);
        }
    }

    private void StokGuncelle(FabrikaDataClassesDataContext db, int urunID, decimal miktar)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            // Mevcut stok kaydını bul veya oluştur
            var stok = db.DepoStoks.FirstOrDefault(s => s.DepoID == depoID && 
                                                       s.UrunID == urunID && 
                                                       s.SirketID == sirketID);
            
            if (stok == null)
            {
                stok = new DepoStok
                {
                    SirketID = sirketID,
                    DepoID = depoID,
                    UrunID = urunID,
                    Miktar = miktar,
                    SonGuncellemeTarihi = DateTime.Now
                };
                db.DepoStoks.InsertOnSubmit(stok);
            }
            else
            {
                stok.Miktar += miktar;
                stok.SonGuncellemeTarihi = DateTime.Now;
            }

            db.SubmitChanges();
        }
        catch (Exception ex)
        {
            // Log hatası
            throw new Exception("Stok güncellenirken hata oluştu: " + ex.Message);
        }
    }

    protected void btnHareketIptal_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "hideModal", 
            "$('#stokHareketModal').modal('hide');", true);
    }

    protected void ddlHareketTipi_SelectedIndexChanged(object sender, EventArgs e)
    {
        StokHareketleriniListele();
    }

    protected void txtTarih_TextChanged(object sender, EventArgs e)
    {
        StokHareketleriniListele();
    }

    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        StokHareketleriniListele();
    }

    protected void rptStokHareketleri_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Detay")
        {
            int hareketID = Convert.ToInt32(e.CommandArgument);
            // Detay sayfasına yönlendir veya modal aç
            ScriptManager.RegisterStartupScript(this, GetType(), "info", 
                "alert('Hareket detayı: ID " + hareketID + "');", true);
        }
    }

    #region Helper Methods

    protected string GetHareketTipiText(string hareketTipi)
    {
        switch (hareketTipi != null ? hareketTipi.ToUpper() : "")
        {
            case "GIRIS":
                return "Giriş";
            case "CIKIS":
                return "Çıkış";
            case "TRANSFER":
                return "Transfer";
            case "SATIS":
                return "Satış";
            case "IADE":
                return "İade";
            default:
                return "Diğer";
        }
    }

    protected string GetHareketTipiBadgeClass(string hareketTipi)
    {
        switch (hareketTipi != null ? hareketTipi.ToUpper() : "")
        {
            case "GIRIS":
                return "badge-success";
            case "CIKIS":
                return "badge-danger";
            case "TRANSFER":
                return "badge-warning";
            case "SATIS":
                return "badge-info";
            case "IADE":
                return "badge-primary";
            default:
                return "badge-secondary";
        }
    }

    #endregion
}
