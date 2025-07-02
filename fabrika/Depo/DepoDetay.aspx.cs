using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_DepoDetay : System.Web.UI.Page
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
                master.SayfaAdi = "Depo Detayı";
            }

            // QueryString'den depo ID'sini al
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out depoID))
            {
                if (!IsPostBack)
                {
                    DepoDetaylariniYukle();
                    StokOzetiniYukle();
                    SonStokHareketleriniYukle();
                    TanklariYukle();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            // Hata logla ve güvenli yönlendirme yap
            System.Diagnostics.Debug.WriteLine("DepoDetay Page_Load Hatası: " + ex.Message);
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }

    private void DepoDetaylariniYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoID && x.SirketID == sirketID);
                
                if (depo != null)
                {
                    // Null-safe string assignments
                    lblDepoAdi.Text = depo.DepoAdi ?? "N/A";
                    lblDepoKodu.Text = depo.DepoKodu ?? "N/A";
                    lblKapasite.Text = (depo.Kapasite ?? 0).ToString("N2") + " Kg";
                    lblDoluMiktar.Text = (depo.DoluMiktar ?? 0).ToString("N2") + " Kg";
                    lblBosKapasite.Text = ((depo.Kapasite ?? 0) - (depo.DoluMiktar ?? 0)).ToString("N2") + " Kg";

                    // Durum badge'i - null-safe kontrol
                    if (depo.Durum == true)
                    {
                        lblDurum.Text = "Aktif";
                        lblDurum.CssClass = "badge badge-success";
                    }
                    else
                    {
                        lblDurum.Text = "Pasif";
                        lblDurum.CssClass = "badge badge-danger";
                    }

                    // Doluluk oranı hesapla ve progress bar'ı ayarla - Division by zero koruması
                    decimal dolulukOrani = 0;
                    if (depo.Kapasite.HasValue && depo.Kapasite.Value > 0)
                    {
                        dolulukOrani = ((depo.DoluMiktar ?? 0) / depo.Kapasite.Value) * 100;
                        // Negatif değer koruması
                        if (dolulukOrani < 0) dolulukOrani = 0;
                        // 100'den büyük değer koruması
                        if (dolulukOrani > 100) dolulukOrani = 100;
                    }

                    progressBar.Style["width"] = dolulukOrani.ToString("N1") + "%";
                    progressBar.InnerText = dolulukOrani.ToString("N1") + "%";
                    
                    // Progress bar rengi
                    if (dolulukOrani <= 25)
                        progressBar.Attributes["class"] = "progress-bar progress-bar-success";
                    else if (dolulukOrani <= 50)
                        progressBar.Attributes["class"] = "progress-bar progress-bar-warning";
                    else if (dolulukOrani <= 75)
                        progressBar.Attributes["class"] = "progress-bar progress-bar-info";
                    else
                        progressBar.Attributes["class"] = "progress-bar progress-bar-danger";

                    // Ürün çeşidi sayısı - güvenli sorgu
                    var urunCesidi = db.DepoStoks.Count(x => x.DepoID == depoID && x.Miktar > 0);
                    lblUrunCesidi.Text = urunCesidi.ToString() + " çeşit";
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda varsayılan değerler ve hata loglama
            System.Diagnostics.Debug.WriteLine("DepoDetaylariniYukle Hatası: " + ex.Message);
            lblDepoAdi.Text = "Depo bulunamadı";
            lblDepoKodu.Text = "N/A";
            lblKapasite.Text = "0 Kg";
            lblDoluMiktar.Text = "0 Kg";
            lblBosKapasite.Text = "0 Kg";
            lblUrunCesidi.Text = "0 çeşit";
            lblDurum.Text = "Bilinmiyor";
            lblDurum.CssClass = "badge badge-secondary";
        }
    }

    private void StokOzetiniYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                // Önce toplam miktarı güvenli şekilde hesapla
                var toplamMiktar = db.DepoStoks
                    .Where(y => y.DepoID == depoID && y.SirketID == sirketID && y.Miktar > 0)
                    .Sum(y => (decimal?)y.Miktar) ?? 0;

                List<object> stokOzeti = new List<object>();

                // Division by zero koruması
                if (toplamMiktar > 0)
                {
                    stokOzeti = db.DepoStoks
                        .Where(x => x.DepoID == depoID && x.SirketID == sirketID && x.Miktar > 0)
                        .Select(x => new
                        {
                            UrunAdi = x.Urunler != null ? x.Urunler.UrunAdi ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                            Miktar = x.Miktar,
                            Oran = (x.Miktar / toplamMiktar) * 100
                        })
                        .OrderByDescending(x => x.Miktar)
                        .Take(10)
                        .ToList<object>();
                }

                rptStokOzeti.DataSource = stokOzeti;
                rptStokOzeti.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Hata loglama ve güvenli fallback
            System.Diagnostics.Debug.WriteLine("StokOzetiniYukle Hatası: " + ex.Message);
            rptStokOzeti.DataSource = null;
            rptStokOzeti.DataBind();
        }
    }

    private void SonStokHareketleriniYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                var hareketler = db.StokHareketleris
                    .Where(x => x.DepoID == depoID && x.SirketID == sirketID)
                    .OrderByDescending(x => x.IslemTarihi)
                    .Take(20)
                    .Select(x => new
                    {
                        x.IslemTarihi,
                        HareketTipi = x.HareketTipi ?? "BILINMIYOR",
                        UrunAdi = x.Urunler != null ? x.Urunler.UrunAdi ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                        x.Miktar,
                        ReferansNo = x.ReferansNo ?? "",
                        Aciklama = x.Aciklama ?? ""
                    })
                    .ToList();

                rptStokHareketleri.DataSource = hareketler;
                rptStokHareketleri.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Hata loglama ve güvenli fallback
            System.Diagnostics.Debug.WriteLine("SonStokHareketleriniYukle Hatası: " + ex.Message);
            rptStokHareketleri.DataSource = null;
            rptStokHareketleri.DataBind();
        }
    }

    protected string GetHareketTipiBadgeClass(string hareketTipi)
    {
        // Null-safe string operation - C# 5 uyumlu
        if (string.IsNullOrEmpty(hareketTipi))
        {
            return "badge-secondary";
        }

        switch (hareketTipi.ToUpper())
        {
            case "GIRIS":
                return "badge-success";
            case "CIKIS":
                return "badge-danger";
            case "TRANSFER":
                return "badge-warning";
            default:
                return "badge-secondary";
        }
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    protected void btnDuzenle_Click(object sender, EventArgs e)
    {
        Response.Redirect("YeniDepo.aspx?id=" + depoID);
    }

    protected void btnStokGoruntule_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoStok.aspx?id=" + depoID);
    }

    protected void btnStokHareketleri_Click(object sender, EventArgs e)
    {
        Response.Redirect("StokHareketleri.aspx?depoID=" + depoID);
    }

#region Tank Yönetimi Metodları

private void TanklariYukle()
{
    try
    {
        using (var db = new FabrikaDataClassesDataContext())
        {
            int sirketID = SessionHelper.GetSirketID();
            int depoID = Convert.ToInt32(Request.QueryString["id"]);
            
            // Tankları hem SirketID hem de DepoID'ye göre filtrele
            var tanklar = db.Tanklars.Where(t => t.SirketID == sirketID && t.DepoID == depoID)
                                     .OrderBy(t => t.TankAdi)
                                     .ToList();
        
            if (tanklar.Count > 0)
            {
                rptTanklar.DataSource = tanklar;
                rptTanklar.DataBind();
                pnlTankYok.Visible = false;
            }
            else
            {
                rptTanklar.DataSource = null;
                rptTanklar.DataBind();
                pnlTankYok.Visible = true;
            }
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine("TanklariYukle Hatası: " + ex.Message);
        // Hata durumunda boş liste göster
        rptTanklar.DataSource = null;
        rptTanklar.DataBind();
        pnlTankYok.Visible = true;
    }
}

protected void btnYeniTank_Click(object sender, EventArgs e)
{
    // Formu temizle ve göster
    hfTankID.Value = "";
    txtTankAdi.Text = "";
    txtTankKodu.Text = "";
    txtKapasite.Text = "";
    txtDoluMiktar.Text = "0";
    ddlDurum.SelectedValue = "Aktif";
    pnlTankForm.Visible = true;
}

protected void btnTankKaydet_Click(object sender, EventArgs e)
{
    if (Page.IsValid)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                bool isEdit = !string.IsNullOrEmpty(hfTankID.Value);
                
                Tanklar tank;
                
                if (isEdit)
                {
                    // Düzenleme
                    int tankID = Convert.ToInt32(hfTankID.Value);
                    tank = db.Tanklars.FirstOrDefault(t => t.TankID == tankID && t.SirketID == sirketID);
                    
                    if (tank == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                            "alert('Tank bulunamadı!');", true);
                        return;
                    }
                }
                else
                {
                    // Yeni tank ekleme
                    int depoID = Convert.ToInt32(Request.QueryString["id"]);
                    tank = new Tanklar
                    {
                        SirketID = sirketID,
                        DepoID = depoID
                    };
                    db.Tanklars.InsertOnSubmit(tank);
                }
                
                // Ortak alanları güncelle
                tank.TankAdi = txtTankAdi.Text.Trim();
                tank.TankKodu = txtTankKodu.Text.Trim();
                tank.Kapasite = Convert.ToDecimal(txtKapasite.Text);
                tank.DoluMiktar = Convert.ToDecimal(txtDoluMiktar.Text);
                
                // Durum alanı bool? türünde
                if (ddlDurum.SelectedValue == "Aktif")
                    tank.Durum = true;
                else if (ddlDurum.SelectedValue == "Pasif")
                    tank.Durum = false;
                else
                    tank.Durum = null; // Bakımda durumu için
                
                // Dolu miktarın kapasiteyi aşmadığını kontrol et
                if (tank.DoluMiktar > tank.Kapasite)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                        "alert('Dolu miktar kapasiteyi aşamaz!');", true);
                    return;
                }
                
                db.SubmitChanges();
                
                // Başarı mesajı ve formu gizle
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessMessage", 
                    string.Format("alert('Tank başarıyla {0}!');", isEdit ? "güncellendi" : "eklendi"), true);
                
                pnlTankForm.Visible = false;
                TanklariYukle();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("btnTankKaydet_Click Hatası: " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                "alert('Tank kaydedilirken hata oluştu!');", true);
        }
    }
}

protected void btnTankIptal_Click(object sender, EventArgs e)
{
    pnlTankForm.Visible = false;
}

protected void rptTanklar_ItemCommand(object source, RepeaterCommandEventArgs e)
{
    int tankID = Convert.ToInt32(e.CommandArgument);
    
    if (e.CommandName == "Duzenle")
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                int depoID = Convert.ToInt32(Request.QueryString["id"]);
                var tank = db.Tanklars.FirstOrDefault(t => t.TankID == tankID && t.SirketID == sirketID && t.DepoID == depoID);
                
                if (tank != null)
                {
                    // Formu doldur ve göster
                    hfTankID.Value = tank.TankID.ToString();
                    txtTankAdi.Text = tank.TankAdi;
                    txtTankKodu.Text = tank.TankKodu ?? "";
                    txtKapasite.Text = tank.Kapasite != null ? tank.Kapasite.ToString() : "0";
                    txtDoluMiktar.Text = tank.DoluMiktar != null ? tank.DoluMiktar.ToString() : "0";
                    
                    // bool? türündeki Durum alanını dropdown değerine çevir
                    if (tank.Durum == true)
                        ddlDurum.SelectedValue = "Aktif";
                    else if (tank.Durum == false)
                        ddlDurum.SelectedValue = "Pasif";
                    else
                        ddlDurum.SelectedValue = "Bakımda";
                    
                    pnlTankForm.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Tank Düzenleme Hatası: " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                "alert('Tank bilgileri yüklenirken hata oluştu!');", true);
        }
    }
    else if (e.CommandName == "Sil")
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                int depoID = Convert.ToInt32(Request.QueryString["id"]);
                var tank = db.Tanklars.FirstOrDefault(t => t.TankID == tankID && t.SirketID == sirketID && t.DepoID == depoID);
                
                if (tank != null)
                {
                    db.Tanklars.DeleteOnSubmit(tank);
                    db.SubmitChanges();
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessMessage", 
                        "alert('Tank başarıyla silindi!');", true);
                    
                    TanklariYukle();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Tank Silme Hatası: " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                "alert('Tank silinirken hata oluştu!');", true);
        }
    }
    }

    // Yardımcı metodlar - UI için
    protected string GetDolulukYuzdesi(double doluMiktar, double kapasite)
    {
        if (kapasite == 0) return "0";
        double yuzde = (doluMiktar / kapasite) * 100;
        return Math.Round(yuzde, 1).ToString();
    }

    protected string GetDolulukBarClass(double doluMiktar, double kapasite)
    {
        if (kapasite == 0) return "progress-bar-danger";
        double yuzde = (doluMiktar / kapasite) * 100;
        
        if (yuzde >= 90) return "progress-bar-danger";
        if (yuzde >= 70) return "progress-bar-warning";
        return "progress-bar-success";
    }

    protected string GetDurumBadgeClass(string durum)
    {
        switch (durum != null ? durum.ToLower() : null)
        {
            case "aktif":
                return "badge-success";
            case "pasif":
                return "badge-secondary";
            case "bakımda":
                return "badge-warning";
            default:
                return "badge-secondary";
        }
    }

    protected string GetDurumText(object durum)
    {
        if (durum == null || durum == DBNull.Value)
            return "Bakımda";
        
        bool? durumValue = durum as bool?;
        if (durumValue == true)
            return "Aktif";
        else if (durumValue == false)
            return "Pasif";
        else
            return "Bakımda";
    }

    #endregion
}