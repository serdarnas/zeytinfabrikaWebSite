using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

public partial class fabrika_Urunler_YeniUrun : System.Web.UI.Page
{
    private const string ResimSessionKey = "YukluResimler";
    private const string VaryantSessionKey = "UrunVaryantlari";
    private FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
    private int SirketID;
    private JavaScriptSerializer serializer = new JavaScriptSerializer();

    protected void Page_Load(object sender, EventArgs e)
    {
        SirketID = SessionHelper.GetSirketID();
        int urunId = 0;
        int.TryParse(Request.QueryString["UrunID"], out urunId);
        if (!IsPostBack)
        {
            // İlk yüklemede varsayılan tab'ı ayarla
            hfActiveTab.Value = "tanim";
            ViewState["ActiveTab"] = hfActiveTab.Value;
            
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Ürünler";
                master.SayfaAdi = urunId > 0 ? "Ürün Güncelle" : "Yeni Ürün";
            }

            KategorileriYukle();
            BirimleriYukle();
            MarkalariYukle();
            VaryantTurleriniYukle();
            ResimleriYukleVeGoster();
            VaryantlariYukle();

            // Eğer güncelleme ise, ürün bilgilerini doldur
            if (urunId > 0)
            {
                var urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId && u.SirketID == SirketID);
                if (urun != null)
                {
                    txtUrunAdi.Text = urun.UrunAdi;
                    ddlKategoriID.SelectedValue = urun.KategoriID.HasValue ? urun.KategoriID.ToString() : "0";
                    ddlBirimID.SelectedValue = urun.BirimID.HasValue ? urun.BirimID.ToString() : "0";
                    txtStokMiktari.Text = urun.StokMiktari.ToString();
                    txtMinimumStok.Text = urun.MinimumStok.ToString();
                    chkUrunTipiStoklu.Checked = urun.UrunTipiStoklu ?? false;
                    chkDurum.Checked = urun.Durum ?? false;
                    txtAlisFiyati.Text = urun.AlisFiyati.ToString();
                    txtAlisKdv.Text = urun.AlisKdv.ToString();
                    chkAlisFiyatiKdvDahilmi.Checked = urun.AlisFiyatiKdvDahilmi ?? false;
                    txtSatisFiyati.Text = urun.SatisFiyati.ToString();
                    txtSatisKdv.Text = urun.SatisKdv.ToString();
                    chkSatisFiyatiKdvDahilmi.Checked = urun.SatisFiyatiKdvDahilmi ?? false;
                    ddlParaBirimiID.SelectedValue = urun.ParaBirimiID.HasValue ? urun.ParaBirimiID.ToString() : "1";
                    txtKDVOrani.Text = urun.KDVOrani.ToString();
                    chkPerakendeSatisVarmi.Checked = urun.PerakendeSatisVarmi ?? false;
                    txtPerakendeSatisFiyati.Text = urun.PerakendeSatisFiyati.ToString();
                    chkPerakendeSatisKdvDahilmi.Checked = urun.PerakendeSatisKdvDahilmi ?? false;
                    ddlMarkaID.SelectedValue = urun.MarkaID.HasValue ? urun.MarkaID.ToString() : "0";
                    txtUrunKodu.Text = urun.UrunKodu;
                    txtBarkod.Text = urun.Barkod;
                    txtNotlar.Text = urun.Notlar;
                    // Mamul/YariMamül checkbox'ları eklenebilir
                }
            }
        }
        else
        {
            // Postback'te HiddenField'ın değerini ViewState'ten al
            if (ViewState["ActiveTab"] != null)
            {
                hfActiveTab.Value = ViewState["ActiveTab"].ToString();
            }
        }
    }

    private void KategorileriYukle()
    {
        var kategoriler = from k in db.UrunKategorileris
                          where k.SirketID == SirketID && k.Durumu == true
                          select new { k.KategoriID, k.Ad };

        ddlKategoriID.DataSource = kategoriler;
        ddlKategoriID.DataTextField = "Ad";
        ddlKategoriID.DataValueField = "KategoriID";
        ddlKategoriID.DataBind();
        ddlKategoriID.Items.Insert(0, new ListItem("Seçiniz", "0"));
    }

    private void BirimleriYukle()
    {
        var birimler = from b in db.Birimlers
                       where b.SirketID == SirketID
                       select new { b.BirimID, b.BirimAdi };

        ddlBirimID.DataSource = birimler;
        ddlBirimID.DataTextField = "BirimAdi";
        ddlBirimID.DataValueField = "BirimID";
        ddlBirimID.DataBind();
        ddlBirimID.Items.Insert(0, new ListItem("Seçiniz", "0"));
    }

    private void MarkalariYukle()
    {
        var markalar = from m in db.Markalars
                       where m.SirketID == SirketID
                       select new { m.MarkaID, m.Ad };

        ddlMarkaID.DataSource = markalar;
        ddlMarkaID.DataTextField = "Ad";
        ddlMarkaID.DataValueField = "MarkaID";
        ddlMarkaID.DataBind();
        ddlMarkaID.Items.Insert(0, new ListItem("Seçiniz", "0"));
    }

    private void VaryantTurleriniYukle()
    {
        var varyantTurleri = from vt in db.VaryantTurleris
                             where vt.SirketID == SirketID
                             select new { vt.VaryantTurID, vt.TurAdi };

        // DropDownList için varyant türlerini yükle
        ddlVaryantTuru.DataSource = varyantTurleri;
        ddlVaryantTuru.DataTextField = "TurAdi";
        ddlVaryantTuru.DataValueField = "VaryantTurID";
        ddlVaryantTuru.DataBind();
        ddlVaryantTuru.Items.Insert(0, new ListItem("Varyant Türü Seçiniz", "0"));
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            int urunId = 0;
            int.TryParse(Request.QueryString["UrunID"], out urunId);
            Urunler urun = null;
            if (urunId > 0)
            {
                // Güncelleme
                urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId && u.SirketID == SirketID);
                if (urun == null)
                {
                    MessageHelper.ShowErrorMessage(this, "Hata", "Ürün bulunamadı.");
                    return;
                }
            }
            else
            {
                // Yeni ürün
                urun = new Urunler();
                urun.SirketID = SirketID;
                urun.OlusturmaTarihi = DateTime.Now;
            }

            urun.UrunAdi = txtUrunAdi.Text;
            urun.KategoriID = Convert.ToInt32(ddlKategoriID.SelectedValue);
            urun.BirimID = Convert.ToInt32(ddlBirimID.SelectedValue);
            urun.StokMiktari = Convert.ToDecimal(txtStokMiktari.Text);
            urun.MinimumStok = Convert.ToDecimal(txtMinimumStok.Text);
            urun.UrunTipiStoklu = chkUrunTipiStoklu.Checked;
            urun.Durum = chkDurum.Checked;
            urun.AlisFiyati = Convert.ToDecimal(txtAlisFiyati.Text);
            urun.AlisKdv = Convert.ToInt32(txtAlisKdv.Text);
            urun.AlisParaBirimi = 1; // Varsayılan değer
            urun.AlisFiyatiKdvDahilmi = chkAlisFiyatiKdvDahilmi.Checked;
            urun.SatisFiyati = Convert.ToDecimal(txtSatisFiyati.Text);
            urun.SatisKdv = Convert.ToInt32(txtSatisKdv.Text);
            urun.SatisParaBirimi = 1; // Varsayılan değer
            urun.SatisFiyatiKdvDahilmi = chkSatisFiyatiKdvDahilmi.Checked;
            urun.ParaBirimiID = Convert.ToInt32(ddlParaBirimiID.SelectedValue);
            urun.KDVOrani = Convert.ToInt32(txtKDVOrani.Text);
            urun.MarkaID = Convert.ToInt32(ddlMarkaID.SelectedValue);
            urun.UrunKodu = txtUrunKodu.Text;
            urun.Barkod = txtBarkod.Text;
            urun.Notlar = txtNotlar.Text;
            // Mamul/YariMamül checkbox'ları eklenebilir
            urun.PerakendeSatisVarmi = chkPerakendeSatisVarmi.Checked;
            urun.PerakendeSatisFiyati = Convert.ToDecimal(txtPerakendeSatisFiyati.Text);
            urun.PerakendeSatisKdvDahilmi = chkPerakendeSatisKdvDahilmi.Checked;

            if (urunId == 0)
            {
                db.Urunlers.InsertOnSubmit(urun);
            }
            db.SubmitChanges();

            // Ürün resimlerini ve varyantları kaydetme kodu burada güncellenebilir

            MessageHelper.ShowSuccessMessage(this, "Başarılı", urunId > 0 ? "Ürün başarıyla güncellendi." : "Ürün başarıyla kaydedildi.");
            Response.Redirect("YeniUrun.aspx?UrunID=" + urun.UrunID);
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Ürün kaydedilirken bir hata oluştu: " + ex.Message);
        }
    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        // Güncelleme işlemleri benzer şekilde implement edilecek
    }

    protected void btnResimYukle_Click(object sender, EventArgs e)
    {
        string ResimKlasoru = "/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/UrunResimleri/";
        if (fuResimler.HasFiles)
        {
            List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
            foreach (HttpPostedFile uploadedFile in fuResimler.PostedFiles)
            {
                string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string sunucuYolu = Server.MapPath(ResimKlasoru + dosyaAdi);
                Directory.CreateDirectory(Server.MapPath(ResimKlasoru));
                uploadedFile.SaveAs(sunucuYolu);
                resimYollari.Add(ResimKlasoru + dosyaAdi);
            }
            Session[ResimSessionKey] = resimYollari;
            ResimleriYukleVeGoster();
        }
    }

    private void ResimleriYukleVeGoster()
    {
        List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
        var resimList = resimYollari.Select(x => new { ImageUrl = x }).ToList();
        rptResimler.DataSource = resimList;
        rptResimler.DataBind();
    }

    protected void ddlVaryantTuru_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVaryantTuru.SelectedValue != "0")
        {
            int varyantTurID = Convert.ToInt32(ddlVaryantTuru.SelectedValue);
            var varyantDegerleri = from v in db.VaryantDegerleris
                                   where v.SirketID == SirketID &&
                                         v.VaryantTurID == varyantTurID
                                   select new { v.VaryantDegerID, v.DegerAdi };

            // Multi-select için varyant değerlerini JavaScript ile yükle
            string script = @"$(document).ready(function() {
                $('#varyantSecici').empty();
                var varyantDegerleri = " + serializer.Serialize(varyantDegerleri) + @";
                $.each(varyantDegerleri, function(index, item) {
                    $('#varyantSecici').append($('<option></option>').val(item.VaryantDegerID).text(item.DegerAdi));
                });
                $('#varyantSecici').multiSelect('refresh');
            });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadVaryantDegerleri", script, true);
        }
        else
        {
            // Seçim yapılmadığında multi-select'i temizle
            string script = @"$(document).ready(function() {
                $('#varyantSecici').empty();
                $('#varyantSecici').multiSelect('refresh');
            });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearVaryantDegerleri", script, true);
        }
    }

    protected void chkVaryantKullan_CheckedChanged(object sender, EventArgs e)
    {
        pnlVaryantlar.Visible = chkVaryantKullan.Checked;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static List<VaryantDegerDTO> GetVaryantDegerleri(int varyantTurID)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                var varyantDegerleri = from v in db.VaryantDegerleris
                                       where v.SirketID == sirketID &&
                                             v.VaryantTurID == varyantTurID
                                       select new VaryantDegerDTO
                                       {
                                           VaryantDegerID = v.VaryantDegerID,
                                           DegerAdi = v.DegerAdi
                                       };
                return varyantDegerleri.ToList();
            }
        }
        catch (Exception)
        {
            return new List<VaryantDegerDTO>();
        }
    }

    protected void btnVaryantEkle_Click(object sender, EventArgs e)
    {
        try
        {
            string varyantSeciciValue = Request.Form["varyantSecici[]"];
            string[] secilenVaryantlar = null;
            if (!string.IsNullOrEmpty(varyantSeciciValue))
            {
                secilenVaryantlar = varyantSeciciValue.Split(',');
            }

            if (secilenVaryantlar == null || secilenVaryantlar.Length == 0)
            {
                MessageHelper.ShowErrorMessage(this, "Uyarı", "Lütfen en az bir varyant seçiniz.");
                return;
            }

            string resimYolu = string.Empty;
            if (fuVaryantResim.HasFile)
            {
                string ResimKlasoru = "/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/VaryantResimleri/";
                string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(fuVaryantResim.FileName);
                string sunucuYolu = Server.MapPath(ResimKlasoru + dosyaAdi);
                Directory.CreateDirectory(Server.MapPath(ResimKlasoru));
                fuVaryantResim.SaveAs(sunucuYolu);
                resimYolu = ResimKlasoru + dosyaAdi;
            }

            List<VaryantBilgisi> varyantlar = Session[VaryantSessionKey] as List<VaryantBilgisi> ?? new List<VaryantBilgisi>();
            
            foreach (string varyantDegerID in secilenVaryantlar)
            {
                int degerID;
                if (int.TryParse(varyantDegerID, out degerID))
                {
                    var varyantDeger = db.VaryantDegerleris.FirstOrDefault(v => v.VaryantDegerID == degerID);
                    if (varyantDeger != null && varyantDeger.VaryantTurID.HasValue)
                    {
                        var varyantTur = db.VaryantTurleris.FirstOrDefault(vt => vt.VaryantTurID == varyantDeger.VaryantTurID.Value);
                        if (varyantTur != null)
                        {
                            varyantlar.Add(new VaryantBilgisi
                            {
                                VaryantID = varyantlar.Count + 1,
                                VaryantTurID = varyantDeger.VaryantTurID.Value,
                                VaryantTuru = varyantTur.TurAdi,
                                VaryantDegerID = varyantDeger.VaryantDegerID,
                                VaryantDegeri = varyantDeger.DegerAdi,
                                ResimYolu = resimYolu
                            });
                        }
                    }
                }
            }

            Session[VaryantSessionKey] = varyantlar;
            VaryantlariYukle();
            MessageHelper.ShowSuccessMessage(this, "Başarılı", "Seçilen varyantlar başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Varyant ekleme sırasında bir hata oluştu: " + ex.Message);
        }
    }

    private void VaryantlariYukle()
    {
        List<VaryantBilgisi> varyantlar = Session[VaryantSessionKey] as List<VaryantBilgisi> ?? new List<VaryantBilgisi>();
        gvVaryantlar.DataSource = varyantlar;
        gvVaryantlar.DataBind();
    }

    protected void gvVaryantlar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                List<VaryantBilgisi> varyantlar = Session[VaryantSessionKey] as List<VaryantBilgisi>;

                if (varyantlar != null && index >= 0 && index < varyantlar.Count)
                {
                    varyantlar.RemoveAt(index);
                    Session[VaryantSessionKey] = varyantlar;
                    VaryantlariYukle();
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Varyant silme işlemi sırasında bir hata oluştu: " + ex.Message);
        }
    }

    protected void gvVaryantlar_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVaryantlar.EditIndex = e.NewEditIndex;
        VaryantlariYukle();
    }

    protected void gvVaryantlar_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        List<VaryantBilgisi> varyantlar = Session[VaryantSessionKey] as List<VaryantBilgisi>;
        varyantlar.RemoveAt(e.RowIndex);
        Session[VaryantSessionKey] = varyantlar;
        VaryantlariYukle();
    }

    protected void gvVaryantlar_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        List<VaryantBilgisi> varyantlar = Session[VaryantSessionKey] as List<VaryantBilgisi>;
        GridViewRow row = gvVaryantlar.Rows[e.RowIndex];

        varyantlar[e.RowIndex].VaryantTuru = ((TextBox)row.Cells[1].Controls[0]).Text;
        varyantlar[e.RowIndex].VaryantDegeri = ((TextBox)row.Cells[2].Controls[0]).Text;

        Session[VaryantSessionKey] = varyantlar;
        gvVaryantlar.EditIndex = -1;
        VaryantlariYukle();
    }

    protected void gvVaryantlar_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVaryantlar.EditIndex = -1;
        VaryantlariYukle();
    }

    protected void btnKategoriKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            string yeniKategoriAdi = txtYeniKategoriAdi.Text.Trim();
            if (!string.IsNullOrEmpty(yeniKategoriAdi))
            {
                // Aynı isimde kategori var mı kontrol et
                var mevcutKategori = db.UrunKategorileris.FirstOrDefault(k => k.SirketID == SirketID && k.Ad == yeniKategoriAdi && k.Durumu == true);
                if (mevcutKategori == null)
                {
                    var yeniKategori = new UrunKategorileri
                    {
                        SirketID = SirketID,
                        Ad = yeniKategoriAdi,
                        Durumu = true
                    };
                    db.UrunKategorileris.InsertOnSubmit(yeniKategori);
                    db.SubmitChanges();
                    KategorileriYukle();
                    ddlKategoriID.SelectedValue = yeniKategori.KategoriID.ToString();
                }
                else
                {
                    // Zaten varsa onu seç
                    KategorileriYukle();
                    ddlKategoriID.SelectedValue = mevcutKategori.KategoriID.ToString();
                }
                txtYeniKategoriAdi.Text = "";
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this, "Hata", "Kategori eklenirken hata oluştu: " + ex.Message);
        }
    }
}

public class VaryantBilgisi
{
    public int VaryantID { get; set; }
    public int VaryantTurID { get; set; }
    public string VaryantTuru { get; set; }
    public int VaryantDegerID { get; set; }
    public string VaryantDegeri { get; set; }
    public string ResimYolu { get; set; }
}

public class VaryantDegerDTO
{
    public int VaryantDegerID { get; set; }
    public string DegerAdi { get; set; }
}