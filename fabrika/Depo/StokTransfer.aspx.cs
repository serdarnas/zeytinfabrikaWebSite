using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_StokTransfer : System.Web.UI.Page
{
    private int kaynakDepoID;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // DepoID parametresini al
            if (Request.QueryString["depoID"] != null)
            {
                if (int.TryParse(Request.QueryString["depoID"], out kaynakDepoID))
                {
                    ViewState["KaynakDepoID"] = kaynakDepoID;
                    KaynakDepoAdiYukle();
                    HedefDepolariYukle();
                    UrunleriYukle();
                    TransferGecmisiniYukle();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        else
        {
            kaynakDepoID = ViewState["KaynakDepoID"] != null ? (int)ViewState["KaynakDepoID"] : 0;
        }
    }

    private void KaynakDepoAdiYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var depo = db.Depolars.FirstOrDefault(d => d.DepoID == kaynakDepoID && d.SirketID == sirketID);
                if (depo != null)
                {
                    lblKaynakDepo.Text = depo.DepoAdi;
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            // Log hatası
            Response.Redirect("Default.aspx");
        }
    }

    private void HedefDepolariYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var depolar = db.Depolars
                    .Where(d => d.SirketID == sirketID && d.DepoID != kaynakDepoID && d.Durum == true)
                    .OrderBy(d => d.DepoAdi)
                    .ToList();

                ddlHedefDepo.Items.Clear();
                ddlHedefDepo.Items.Add(new ListItem("Hedef depo seçiniz...", ""));
                
                foreach (var depo in depolar)
                {
                    ddlHedefDepo.Items.Add(new ListItem(depo.DepoAdi, depo.DepoID.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            // Log hatası
        }
    }

    private void UrunleriYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Kaynak depoda stoku olan ürünleri getir
                var urunler = (from ds in db.DepoStoks
                              join u in db.Urunlers on ds.UrunID equals u.UrunID
                              where ds.DepoID == kaynakDepoID && ds.SirketID == sirketID && ds.Miktar > 0
                              orderby u.UrunAdi
                              select new
                              {
                                  u.UrunID,
                                  u.UrunAdi,
                                  ds.Miktar
                              }).ToList();

                ddlUrun.Items.Clear();
                ddlUrun.Items.Add(new ListItem("Ürün seçiniz...", ""));
                
                foreach (var urun in urunler)
                {
                    ddlUrun.Items.Add(new ListItem(string.Format("{0} (Stok: {1:N2})", urun.UrunAdi, urun.Miktar), urun.UrunID.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            // Log hatası
        }
    }

    protected void ddlHedefDepo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Hedef depo seçildiğinde gerekli işlemler
    }

    protected void ddlUrun_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUrun.SelectedValue))
        {
            MevcutStokuGoster();
            BirimBilgisiniGoster();
        }
        else
        {
            lblMevcutStok.Text = "0";
            lblBirim.Text = "";
        }
    }

    private void MevcutStokuGoster()
    {
        try
        {
            int urunID = Convert.ToInt32(ddlUrun.SelectedValue);
            int sirketID = SessionHelper.GetSirketID();
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                var stok = db.DepoStoks.FirstOrDefault(ds => ds.DepoID == kaynakDepoID && 
                                                           ds.UrunID == urunID && 
                                                           ds.SirketID == sirketID);
                
                if (stok != null)
                {
                    lblMevcutStok.Text = stok.Miktar.ToString("N2");
                    rvMiktar.MaximumValue = stok.Miktar.ToString();
                }
                else
                {
                    lblMevcutStok.Text = "0";
                    rvMiktar.MaximumValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            lblMevcutStok.Text = "0";
        }
    }

    private void BirimBilgisiniGoster()
    {
        try
        {
            int urunID = Convert.ToInt32(ddlUrun.SelectedValue);
            int sirketID = SessionHelper.GetSirketID();
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                var urun = (from u in db.Urunlers
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           where u.UrunID == urunID && u.SirketID == sirketID
                           select new
                           {
                               BirimAdi = b != null ? b.BirimAdi : ""
                           }).FirstOrDefault();
                
                if (urun != null)
                {
                    lblBirim.Text = urun.BirimAdi;
                }
            }
        }
        catch (Exception ex)
        {
            lblBirim.Text = "";
        }
    }

    protected void btnTransferYap_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                int sirketID = SessionHelper.GetSirketID();
                int kullaniciID = SessionHelper.GetKullaniciID();
                int hedefDepoID = Convert.ToInt32(ddlHedefDepo.SelectedValue);
                int urunID = Convert.ToInt32(ddlUrun.SelectedValue);
                decimal miktar = Convert.ToDecimal(txtMiktar.Text);
                string aciklama = txtAciklama.Text.Trim();
                
                if (string.IsNullOrEmpty(aciklama))
                {
                    aciklama = "Depo transfer işlemi";
                }

                using (var db = new FabrikaDataClassesDataContext())
                {
                    // Mevcut stok kontrolü
                    var kaynakStok = db.DepoStoks.FirstOrDefault(ds => ds.DepoID == kaynakDepoID && 
                                                                      ds.UrunID == urunID && 
                                                                      ds.SirketID == sirketID);
                    
                    if (kaynakStok == null || kaynakStok.Miktar < miktar)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                            "alert('Yetersiz stok! Transfer yapılamaz.');", true);
                        return;
                    }

                    // Kaynak depodan çıkış hareketi
                    var cikisHareketi = new StokHareketleri
                    {
                        SirketID = sirketID,
                        DepoID = kaynakDepoID,
                        UrunID = urunID,
                        HareketTipi = "TRANSFER_CIKIS",
                        Miktar = -miktar,
                        IslemTarihi = DateTime.Now,
                        Aciklama = string.Format("Transfer - Hedef: {0} - {1}", ddlHedefDepo.SelectedItem.Text, aciklama),
                        KullaniciID = kullaniciID
                    };

                    // Hedef depoya giriş hareketi
                    var girisHareketi = new StokHareketleri
                    {
                        SirketID = sirketID,
                        DepoID = hedefDepoID,
                        UrunID = urunID,
                        HareketTipi = "TRANSFER_GIRIS",
                        Miktar = miktar,
                        IslemTarihi = DateTime.Now,
                        Aciklama = string.Format("Transfer - Kaynak: {0} - {1}", lblKaynakDepo.Text, aciklama),
                        KullaniciID = kullaniciID
                    };

                    db.StokHareketleris.InsertOnSubmit(cikisHareketi);
                    db.StokHareketleris.InsertOnSubmit(girisHareketi);

                    // Kaynak depo stok güncelleme
                    kaynakStok.Miktar -= miktar;
                    kaynakStok.SonGuncellemeTarihi = DateTime.Now;

                    // Hedef depo stok güncelleme
                    var hedefStok = db.DepoStoks.FirstOrDefault(ds => ds.DepoID == hedefDepoID && 
                                                                     ds.UrunID == urunID && 
                                                                     ds.SirketID == sirketID);
                    
                    if (hedefStok == null)
                    {
                        hedefStok = new DepoStok
                        {
                            SirketID = sirketID,
                            DepoID = hedefDepoID,
                            UrunID = urunID,
                            Miktar = miktar,
                            SonGuncellemeTarihi = DateTime.Now
                        };
                        db.DepoStoks.InsertOnSubmit(hedefStok);
                    }
                    else
                    {
                        hedefStok.Miktar += miktar;
                        hedefStok.SonGuncellemeTarihi = DateTime.Now;
                    }

                    db.SubmitChanges();

                    // Formu temizle
                    ddlHedefDepo.SelectedIndex = 0;
                    ddlUrun.SelectedIndex = 0;
                    txtMiktar.Text = "";
                    txtAciklama.Text = "";
                    lblMevcutStok.Text = "0";
                    lblBirim.Text = "";

                    // Listeleri yenile
                    UrunleriYukle();
                    TransferGecmisiniYukle();

                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "alert('Transfer işlemi başarıyla tamamlandı.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Hata: " + ex.Message + "');", true);
            }
        }
    }

    private void TransferGecmisiniYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var transferler = (from sh in db.StokHareketleris
                                  join u in db.Urunlers on sh.UrunID equals u.UrunID
                                  join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                                  from b in birimGroup.DefaultIfEmpty()
                                  join kd in db.Depolars on sh.DepoID equals kd.DepoID
                                  join k in db.Kullanicilars on sh.KullaniciID equals k.KullaniciID into kullaniciGroup
                                  from k in kullaniciGroup.DefaultIfEmpty()
                                  where sh.SirketID == sirketID && 
                                        (sh.HareketTipi == "TRANSFER_CIKIS" || sh.HareketTipi == "TRANSFER_GIRIS") &&
                                        (sh.DepoID == kaynakDepoID || sh.Aciklama.Contains(lblKaynakDepo.Text))
                                  orderby sh.IslemTarihi descending
                                  select new
                                  {
                                      sh.IslemTarihi,
                                      UrunAdi = u.UrunAdi,
                                      KaynakDepoAdi = sh.HareketTipi == "TRANSFER_CIKIS" ? kd.DepoAdi : ExtractDepoFromAciklama(sh.Aciklama),
                                      HedefDepoAdi = sh.HareketTipi == "TRANSFER_GIRIS" ? kd.DepoAdi : ExtractDepoFromAciklama(sh.Aciklama),
                                      Miktar = Math.Abs(sh.Miktar),
                                      Birim = b != null ? b.BirimAdi : "",
                                      sh.Aciklama,
                                      KullaniciAdi = k != null ? k.AdSoyad : "Sistem"
                                  }).Take(20).ToList();

                if (transferler.Count > 0)
                {
                    rptTransferler.DataSource = transferler;
                    rptTransferler.DataBind();
                    pnlTransferYok.Visible = false;
                }
                else
                {
                    rptTransferler.DataSource = null;
                    rptTransferler.DataBind();
                    pnlTransferYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlTransferYok.Visible = true;
        }
    }

    private string ExtractDepoFromAciklama(string aciklama)
    {
        try
        {
            if (aciklama.Contains("Kaynak:"))
            {
                int start = aciklama.IndexOf("Kaynak:") + 7;
                int end = aciklama.IndexOf(" - ", start);
                if (end > start)
                    return aciklama.Substring(start, end - start).Trim();
            }
            else if (aciklama.Contains("Hedef:"))
            {
                int start = aciklama.IndexOf("Hedef:") + 6;
                int end = aciklama.IndexOf(" - ", start);
                if (end > start)
                    return aciklama.Substring(start, end - start).Trim();
            }
            return "";
        }
        catch
        {
            return "";
        }
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoStok.aspx?id=" + kaynakDepoID);
    }

    protected void btnIptal_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoStok.aspx?id=" + kaynakDepoID);
    }
}
