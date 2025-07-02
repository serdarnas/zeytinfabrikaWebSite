using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_StokEkle : System.Web.UI.Page
{
    private int depoID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // DepoID parametresini al
            if (Request.QueryString["depoID"] != null && int.TryParse(Request.QueryString["depoID"], out depoID))
            {
                ViewState["DepoID"] = depoID;
                DepoAdiYukle();
                UrunleriYukle();
                SonStoklariYukle();
            }
            else
            {
                Response.Redirect("DepoListesi.aspx");
            }
        }
        else
        {
            depoID = (int)ViewState["DepoID"];
        }
    }

    private void DepoAdiYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoID && x.SirketID == sirketID);
            if (depo != null)
            {
                lblDepoAdi.Text = depo.DepoAdi;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Depo bilgileri yüklenirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    private void UrunleriYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            var urunler = from u in db.Urunlers
                         where u.SirketID == sirketID && u.Durum == true
                         orderby u.UrunAdi
                         select new
                         {
                             u.UrunID,
                             u.UrunAdi,
                             u.BirimID
                         };

            ddlUrun.DataSource = urunler.ToList();
            ddlUrun.DataTextField = "UrunAdi";
            ddlUrun.DataValueField = "UrunID";
            ddlUrun.DataBind();
            ddlUrun.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Ürün Seçiniz...", ""));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Ürünler yüklenirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void ddlUrun_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUrun.SelectedValue))
        {
            UrunBilgileriniGoster();
        }
        else
        {
            txtMevcutStok.Text = "";
            txtBirim.Text = "";
            txtMinimumStok.Text = "";
        }
    }

    private void UrunBilgileriniGoster()
    {
        try
        {
            int urunID = Convert.ToInt32(ddlUrun.SelectedValue);
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Mevcut stok miktarını al
            var mevcutStok = db.DepoStoks.FirstOrDefault(x => x.DepoID == depoID && x.UrunID == urunID && x.SirketID == sirketID);
            if (mevcutStok != null)
            {
                txtMevcutStok.Text = mevcutStok.Miktar.ToString("0.##");
                txtMinimumStok.Text = (mevcutStok.MinimumMiktar ?? 0).ToString("0.##");
            }
            else
            {
                txtMevcutStok.Text = "0";
                txtMinimumStok.Text = "0";
            }

            // Ürün bilgilerini al
            var urun = db.Urunlers.FirstOrDefault(x => x.UrunID == urunID && x.SirketID == sirketID);
            if (urun != null)
            {
                // Birim bilgisi
                if (urun.BirimID.HasValue)
                {
                    var birim = db.Birimlers.FirstOrDefault(x => x.BirimID == urun.BirimID.Value);
                    txtBirim.Text = birim != null ? birim.BirimAdi : "";
                }
                else
                {
                    txtBirim.Text = "";
                }


            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Ürün bilgileri yüklenirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnStokEkle_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && !string.IsNullOrEmpty(ddlUrun.SelectedValue))
        {
            try
            {
                int urunID = Convert.ToInt32(ddlUrun.SelectedValue);
                decimal eklenecekMiktar = Convert.ToDecimal(txtEklenecekMiktar.Text.Replace(".", ","));

                decimal minimumStok = string.IsNullOrEmpty(txtMinimumStok.Text) ? 0 : Convert.ToDecimal(txtMinimumStok.Text.Replace(".", ","));
                string aciklama = txtAciklama.Text.Trim();
                
                int sirketID = SessionHelper.GetSirketID();
                int kullaniciID = SessionHelper.GetKullaniciID();
                
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                
                // Mevcut stok kaydını kontrol et
                var mevcutStok = db.DepoStoks.FirstOrDefault(x => x.DepoID == depoID && x.UrunID == urunID && x.SirketID == sirketID);
                
                if (mevcutStok != null)
                {
                    // Mevcut stoku güncelle
                    mevcutStok.Miktar += eklenecekMiktar;
                    mevcutStok.MinimumMiktar = minimumStok;
                    mevcutStok.SonGuncellemeTarihi = DateTime.Now;
                }
                else
                {
                    // Yeni stok kaydı oluştur
                    DepoStok yeniStok = new DepoStok
                    {
                        SirketID = sirketID,
                        DepoID = depoID,
                        UrunID = urunID,
                        Miktar = eklenecekMiktar,
                        MinimumMiktar = minimumStok,
                        SonGuncellemeTarihi = DateTime.Now
                    };
                    db.DepoStoks.InsertOnSubmit(yeniStok);
                }

                // Stok hareketi kaydet
                StokHareketleri hareket = new StokHareketleri
                {
                    SirketID = sirketID,
                    DepoID = depoID,
                    UrunID = urunID,
                    HareketTipi = "Giriş",
                    Miktar = eklenecekMiktar,
                    IslemTarihi = DateTime.Now,
                    Aciklama = string.IsNullOrEmpty(aciklama) ? "Manuel stok ekleme" : aciklama,
                    KullaniciID = kullaniciID
                };
                db.StokHareketleris.InsertOnSubmit(hareket);

                db.SubmitChanges();

                ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                    "alert('Stok başarıyla eklendi!'); window.location.href='DepoStok.aspx?depoID=" + depoID + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Stok eklenirken hata oluştu: " + ex.Message + "');", true);
            }
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        ddlUrun.SelectedIndex = 0;
        txtMevcutStok.Text = "";
        txtBirim.Text = "";
        txtEklenecekMiktar.Text = "";
        txtMinimumStok.Text = "";
        txtAciklama.Text = "";
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoStok.aspx?depoID=" + depoID);
    }

    private void SonStoklariYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            var sonStoklar = (from sh in db.StokHareketleris
                             join u in db.Urunlers on sh.UrunID equals u.UrunID
                             join b in db.Birimlers on u.BirimID equals b.BirimID into birimJoin
                             from b in birimJoin.DefaultIfEmpty()
                             join k in db.Kullanicilars on sh.KullaniciID equals k.KullaniciID into kullaniciJoin
                             from k in kullaniciJoin.DefaultIfEmpty()
                             where sh.SirketID == sirketID && sh.DepoID == depoID && sh.HareketTipi == "Giriş"
                             orderby sh.IslemTarihi descending
                             select new
                             {
                                 sh.IslemTarihi,
                                 u.UrunAdi,
                                 sh.Miktar,
                                 BirimAdi = b != null ? b.BirimAdi : "",
                                 sh.Aciklama,
                                 KullaniciAdi = k != null ? k.AdSoyad : ""
                             }).Take(10);

            rptSonStoklar.DataSource = sonStoklar.ToList();
            rptSonStoklar.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Son stoklar yüklenirken hata oluştu: " + ex.Message + "');", true);
        }
    }
}
