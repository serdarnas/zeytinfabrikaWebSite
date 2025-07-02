using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_YeniDepo : System.Web.UI.Page
{
    private int depoID = 0;
    private bool duzenlemeModu = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Depo";
                master.SayfaAdi = "Yeni Depo";
            }

            // Düzenleme modu kontrolü
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out depoID))
            {
                duzenlemeModu = true;
                if (master != null)
                    master.SayfaAdi = "Depo Düzenle";

                if (!IsPostBack)
                {
                    DepoVeriYukle();
                }
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }

    private void DepoVeriYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoID && x.SirketID == sirketID);
            
            if (depo != null)
            {
                txtDepoAdi.Text = depo.DepoAdi;
                txtDepoKodu.Text = depo.DepoKodu;
                txtKapasite.Text = (depo.Kapasite ?? 0).ToString();
                txtDoluMiktar.Text = (depo.DoluMiktar ?? 0).ToString();
                chkDurum.Checked = depo.Durum ?? true;

                // Depo kodu düzenlenemez
                txtDepoKodu.Enabled = false;
                txtDepoKodu.CssClass += " bg-light";
                
                // Buton metnini değiştir
                btnKaydet.Text = "✏ Depo Güncelle";
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            lblMesaj.Text = "Depo bilgileri yüklenirken hata oluştu.";
            lblMesaj.CssClass = "alert alert-danger";
            lblMesaj.Visible = true;
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int sirketID = SessionHelper.GetSirketID();
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

                // Kapasite ve dolu miktar kontrolü
                decimal kapasite = 0;
                decimal doluMiktar = 0;

                if (!string.IsNullOrEmpty(txtKapasite.Text))
                    kapasite = Convert.ToDecimal(txtKapasite.Text);

                if (!string.IsNullOrEmpty(txtDoluMiktar.Text))
                    doluMiktar = Convert.ToDecimal(txtDoluMiktar.Text);

                if (kapasite > 0 && doluMiktar > kapasite)
                {
                    lblMesaj.Text = "Dolu miktar, kapasite değerinden büyük olamaz!";
                    lblMesaj.CssClass = "alert alert-danger";
                    lblMesaj.Visible = true;
                    return;
                }

                if (duzenlemeModu)
                {
                    // Depo güncelleme
                    var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoID && x.SirketID == sirketID);
                    
                    if (depo != null)
                    {
                        depo.DepoAdi = txtDepoAdi.Text.Trim();
                        depo.Kapasite = kapasite;
                        depo.DoluMiktar = doluMiktar;
                        depo.Durum = chkDurum.Checked;

                        db.SubmitChanges();

                        lblMesaj.Text = "Depo başarıyla güncellendi!";
                        lblMesaj.CssClass = "alert alert-success";
                        lblMesaj.Visible = true;

                        // 2 saniye sonra detay sayfasına yönlendir
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", 
                            "setTimeout(function(){ window.location.href = 'DepoDetay.aspx?id=" + depoID + "'; }, 2000);", true);
                    }
                }
                else
                {
                    // Depo kodu benzersizlik kontrolü
                    string depoKodu = txtDepoKodu.Text.Trim().ToUpper();
                    var mevcutDepo = db.Depolars.FirstOrDefault(x => x.DepoKodu == depoKodu && x.SirketID == sirketID);
                    
                    if (mevcutDepo != null)
                    {
                        lblMesaj.Text = "Bu depo kodu zaten kullanılmaktadır. Lütfen farklı bir kod giriniz.";
                        lblMesaj.CssClass = "alert alert-danger";
                        lblMesaj.Visible = true;
                        return;
                    }

                    // Yeni depo ekleme
                    var yeniDepo = new Depolar
                    {
                        SirketID = sirketID,
                        DepoAdi = txtDepoAdi.Text.Trim(),
                        DepoKodu = depoKodu,
                        Kapasite = kapasite,
                        DoluMiktar = doluMiktar,
                        Durum = chkDurum.Checked
                    };

                    db.Depolars.InsertOnSubmit(yeniDepo);
                    db.SubmitChanges();

                    lblMesaj.Text = "Depo başarıyla eklendi!";
                    lblMesaj.CssClass = "alert alert-success";
                    lblMesaj.Visible = true;

                    // Formu temizle
                    FormTemizle();

                    // 2 saniye sonra depo listesine yönlendir
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", 
                        "setTimeout(function(){ window.location.href = 'Default.aspx'; }, 2000);", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMesaj.Text = "İşlem sırasında hata oluştu: " + ex.Message;
            lblMesaj.CssClass = "alert alert-danger";
            lblMesaj.Visible = true;
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        FormTemizle();
        lblMesaj.Visible = false;
    }

    private void FormTemizle()
    {
        if (!duzenlemeModu)
        {
            txtDepoAdi.Text = "";
            txtDepoKodu.Text = "";
        }
        txtKapasite.Text = "";
        txtDoluMiktar.Text = "0";
        txtAciklama.Text = "";
        chkDurum.Checked = true;
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        if (duzenlemeModu)
            Response.Redirect("DepoDetay.aspx?id=" + depoID);
        else
            Response.Redirect("Default.aspx");
    }

    protected void btnDepoListesi_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}