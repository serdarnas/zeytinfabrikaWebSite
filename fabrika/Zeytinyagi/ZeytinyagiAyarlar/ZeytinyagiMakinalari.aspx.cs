using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

public partial class fabrika_Zeytinyagi_ZeytinyagiAyarlar_ZeytinyagiMakinalari : System.Web.UI.Page
{
    // Şirket ID'sini Session'dan alıyoruz
    private int SirketID
    {
        get
        {
           
            return SessionHelper.GetSirketID();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    { 
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Zeytinyağı";
            master.SayfaAdi = "Zeytinyağı Listesi";
        };

        try
        {
            if (!IsPostBack)
            {
                // İlk yüklemede marka listesini doldur ve makine listesini getir
                LoadMarkalar();
                LoadMakineler();
            }

            // ViewState'den malaksör listesini kontrol et
            if (ViewState["MalaksorDataTable"] == null)
            {
                CreateMalaksorDataTable();
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("Page_Load hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken bir hata oluştu!"+ex.Message);
        }
    }

    // Markaları yükle
    private void LoadMarkalar()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var markalar = (from m in db.ZeytinyagiMakinaMarkalaris
                               orderby m.Ad
                               select new
                               {
                                   ZeytinyagiMakinaMarkaID = m.ZeytinyagiMakinaMarkaID,
                                   Ad = m.Ad
                               }).ToList();

                ddlMarka.DataSource = markalar;
                ddlMarka.DataTextField = "Ad";
                ddlMarka.DataValueField = "ZeytinyagiMakinaMarkaID";
                ddlMarka.DataBind();
                ddlMarka.Items.Insert(0, new ListItem("-- Marka Seçiniz --", "0"));
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("LoadMarkalar hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Markalar yüklenirken bir hata oluştu!"+ex.Message);
        }
    }

    // Makineleri yükle
    private void LoadMakineler()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var makineler = (from m in db.SirketZeytinyagiMakinalaris
                                join model in db.ZeytinyagiMakinaModelleris
                                on m.ZeytinyagiMakinaModelID equals model.ZeytinyagiMakinaModelID
                                join marka in db.ZeytinyagiMakinaMarkalaris
                                on model.ZeytinyagiMakinaMarkaID equals marka.ZeytinyagiMakinaMarkaID
                                where m.SirketID == SirketID
                                select new
                                {
                                    m.SirketZeytinyagiMakinaID,
                                    Marka = marka.Ad,
                                    Model = model.ModelAd,
                                    m.AlimTarihi,
                                    m.Durumu
                                }).ToList();

                gvMakineler.DataSource = makineler;
                gvMakineler.DataBind();
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("LoadMakineler hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Makineler yüklenirken bir hata oluştu!"+ex.Message);
        }
    }

    // Marka seçildiğinde modelleri yükle
    protected void ddlMarka_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int markaId;
            if (!int.TryParse(ddlMarka.SelectedValue, out markaId) || markaId == 0)
            {
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("-- Önce marka seçiniz --", "0"));
                ddlModel.Enabled = false;
                return;
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                var modeller = (from m in db.ZeytinyagiMakinaModelleris
                                where m.ZeytinyagiMakinaMarkaID == markaId
                                orderby m.ModelAd
                                select new
                                {
                                    ZeytinyagiMakinaModelID = m.ZeytinyagiMakinaModelID,
                                    Ad = m.ModelAd
                                }).ToList();

                ddlModel.DataSource = modeller;
                ddlModel.DataTextField = "Ad";
                ddlModel.DataValueField = "ZeytinyagiMakinaModelID";
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListItem("-- Model Seçiniz --", "0"));
                ddlModel.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("ddlMarka_SelectedIndexChanged hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Modeller yüklenirken bir hata oluştu!"+ex.Message);
        }
    }

    // Malaksör tablosunu oluştur
    private void CreateMalaksorDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SirketZeytinyagiMakinaMalaksorID", typeof(int));
        dt.Columns.Add("MalaksorSiraNo", typeof(byte));
        dt.Columns.Add("MalaksorKaparistesi_kg", typeof(int));
        dt.Columns.Add("Durum", typeof(string));
        ViewState["MalaksorDataTable"] = dt;
    }

    // Yeni makine butonuna tıklandığında
    protected void btnYeniMakina_Click(object sender, EventArgs e)
    {
        ClearForm();
        pnlMakineForm.Visible = true;
    }

    // Formu temizle
    private void ClearForm()
    {
        hdnMakinaID.Value = "0";
        ddlMarka.SelectedValue = "0";
        ddlModel.Items.Clear();
        ddlModel.Items.Add(new ListItem("-- Önce marka seçiniz --", "0"));
        ddlModel.Enabled = false;
        txtAlimTarihi.Text = "";
        txtYikamaHizi.Text = "";
        txtKirmaHizi.Text = "";
        txtMalaksasyonHizi.Text = "";
        txtDekantasyonHizi.Text = "";
        CreateMalaksorDataTable();
        gvMalaksorler.DataSource = ViewState["MalaksorDataTable"];
        gvMalaksorler.DataBind();
    }

    // İptal butonuna tıklandığında
    protected void btnIptal_Click(object sender, EventArgs e)
    {
        pnlMakineForm.Visible = false;
        ClearForm();
    }

    // Malaksör ekle butonuna tıklandığında
    protected void btnMalaksorEkle_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtYeniMalaksorKapasite.Text))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Lütfen malaksör kapasitesini giriniz!");
                return;
            }

            DataTable dt = (DataTable)ViewState["MalaksorDataTable"];
            
            // Yeni malaksör için sıra numarasını belirle
            byte yeniSiraNo = 1;
            if (dt.Rows.Count > 0)
            {
                yeniSiraNo = (byte)(dt.AsEnumerable()
                    .Max(row => Convert.ToInt32(row["MalaksorSiraNo"])) + 1);
            }

            int kapasite = Convert.ToInt32(txtYeniMalaksorKapasite.Text);

            DataRow dr = dt.NewRow();
            dr["SirketZeytinyagiMakinaMalaksorID"] = 0;
            dr["MalaksorSiraNo"] = yeniSiraNo;
            dr["MalaksorKaparistesi_kg"] = kapasite;
            dr["Durum"] = "Aktif";
            dt.Rows.Add(dr);

            gvMalaksorler.DataSource = dt;
            gvMalaksorler.DataBind();
            txtYeniMalaksorKapasite.Text = "";
        }
        catch (Exception ex)
        {
            //Trace.TraceError("btnMalaksorEkle_Click hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Malaksör eklenirken bir hata oluştu!"+ex.Message);
        }
    }

    // Kaydet butonuna tıklandığında
    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsValid)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Lütfen gerekli alanları doldurunuz!");
                return;
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                int makinaId;
                int.TryParse(hdnMakinaID.Value, out makinaId);

                SirketZeytinyagiMakinalari makina;
                string message;

                if (makinaId == 0)
                {
                    makina = new SirketZeytinyagiMakinalari
                    {
                        SirketID = SirketID,
                        Durumu = true
                    };
                    db.SirketZeytinyagiMakinalaris.InsertOnSubmit(makina);
                    message = "Makine başarıyla eklendi.";
                }
                else
                {
                    makina = (from m in db.SirketZeytinyagiMakinalaris
                              where m.SirketZeytinyagiMakinaID == makinaId
                              select m).FirstOrDefault();

                    if (makina == null)
                    {
                        MessageHelper.ShowErrorMessage(this, "Hata", "Makine bulunamadı!");
                        return;
                    }
                    message = "Makine başarıyla güncellendi.";
                }

                // Makine bilgilerini güncelle
                makina.ZeytinyagiMakinaModelID = Convert.ToInt32(ddlModel.SelectedValue);
                
                // Alım tarihi kontrolü ve dönüşümü
                if (!string.IsNullOrEmpty(txtAlimTarihi.Text))
                {
                    try
                    {
                        makina.AlimTarihi = DateTime.ParseExact(txtAlimTarihi.Text, "yyyy-MM-dd", null);
                    }
                    catch
                    {
                        makina.AlimTarihi = null;
                    }
                }
                else
                {
                    makina.AlimTarihi = null;
                }
                byte? yikamaHizi = null;
                if (!string.IsNullOrEmpty(txtYikamaHizi.Text))
                    yikamaHizi = Convert.ToByte(txtYikamaHizi.Text);
                makina.YikamaYaprakAyiklama_kg_dk = yikamaHizi;

                byte? kirmaHizi = null;
                if (!string.IsNullOrEmpty(txtKirmaHizi.Text))
                    kirmaHizi = Convert.ToByte(txtKirmaHizi.Text);
                makina.Kirma_kg_dk = kirmaHizi;

                byte? malaksasyonHizi = null;
                if (!string.IsNullOrEmpty(txtMalaksasyonHizi.Text))
                    malaksasyonHizi = Convert.ToByte(txtMalaksasyonHizi.Text);
                makina.Malaksasyon_kg_dk = malaksasyonHizi;

                byte? dekantasyonHizi = null;
                if (!string.IsNullOrEmpty(txtDekantasyonHizi.Text))
                    dekantasyonHizi = Convert.ToByte(txtDekantasyonHizi.Text);
                makina.Dekantasyon_Santrifuj_kg_dk = dekantasyonHizi;

                // Önce makineyi kaydet
                db.SubmitChanges();

                // Sonra malaksörleri kaydet
                DataTable dtMalaksorler = (DataTable)ViewState["MalaksorDataTable"];
                if (dtMalaksorler != null && dtMalaksorler.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMalaksorler.Rows)
                    {
                        int malaksorId = Convert.ToInt32(dr["SirketZeytinyagiMakinaMalaksorID"]);
                        if (malaksorId == 0)
                        {
                            var yeniMalaksor = new SirketZeytinyagiMakinaMalaksorler
                            {
                                SirketID = SirketID,
                                SirketZeytinyagiMakinaID = makina.SirketZeytinyagiMakinaID,
                                MalaksorSiraNo = Convert.ToByte(dr["MalaksorSiraNo"]),
                                MalaksorKaparistesi_kg = Convert.ToInt32(dr["MalaksorKaparistesi_kg"]),
                                Durum = true
                            };
                            db.SirketZeytinyagiMakinaMalaksorlers.InsertOnSubmit(yeniMalaksor);
                        }
                    }
                    
                    // Malaksörleri kaydet
                    db.SubmitChanges();
                }
                MessageHelper.ShowSuccessMessage(this, "Başarılı", message);
                LoadMakineler();
                pnlMakineForm.Visible = false;
                ClearForm();
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("btnKaydet_Click hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Makine kaydedilirken bir hata oluştu!"+ex.Message);
        }
    }

    // GridView komutlarını işle
    protected void gvMakineler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int makinaId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Duzenle")
            {
                using (var db = new FabrikaDataClassesDataContext())
                {
                    var makina = (from m in db.SirketZeytinyagiMakinalaris
                                  where m.SirketZeytinyagiMakinaID == makinaId
                                  select m).FirstOrDefault();

                    if (makina == null)
                    {
                        MessageHelper.ShowErrorMessage(this, "Hata", "Makine bulunamadı!");
                        return;
                    }

                    hdnMakinaID.Value = makina.SirketZeytinyagiMakinaID.ToString();
                    ddlMarka.SelectedValue = makina.ZeytinyagiMakinaModelleri.ZeytinyagiMakinaMarkaID.ToString();
                    ddlMarka_SelectedIndexChanged(null, null);
                    ddlModel.SelectedValue = makina.ZeytinyagiMakinaModelID.ToString();
                    if (makina.AlimTarihi.HasValue)
                        txtAlimTarihi.Text = makina.AlimTarihi.Value.ToString("yyyy-MM-dd");
                    txtYikamaHizi.Text = makina.YikamaYaprakAyiklama_kg_dk.HasValue ? makina.YikamaYaprakAyiklama_kg_dk.Value.ToString() : string.Empty;
                    txtKirmaHizi.Text = makina.Kirma_kg_dk.HasValue ? makina.Kirma_kg_dk.Value.ToString() : string.Empty;
                    txtMalaksasyonHizi.Text = makina.Malaksasyon_kg_dk.HasValue ? makina.Malaksasyon_kg_dk.Value.ToString() : string.Empty;
                    txtDekantasyonHizi.Text = makina.Dekantasyon_Santrifuj_kg_dk.HasValue ? makina.Dekantasyon_Santrifuj_kg_dk.Value.ToString() : string.Empty;

                    // Malaksörleri yükle
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SirketZeytinyagiMakinaMalaksorID", typeof(int));
                    dt.Columns.Add("MalaksorSiraNo", typeof(byte));
                    dt.Columns.Add("MalaksorKaparistesi_kg", typeof(int));
                    dt.Columns.Add("Durum", typeof(string));

                    foreach (var malaksor in makina.SirketZeytinyagiMakinaMalaksorlers.OrderBy(m => m.MalaksorSiraNo))
                    {
                        DataRow dr = dt.NewRow();
                        dr["SirketZeytinyagiMakinaMalaksorID"] = malaksor.SirketZeytinyagiMakinaMalaksorID;
                        dr["MalaksorSiraNo"] = malaksor.MalaksorSiraNo;
                        dr["MalaksorKaparistesi_kg"] = malaksor.MalaksorKaparistesi_kg;
                        dr["Durum"] = malaksor.Durum.HasValue && malaksor.Durum.Value ? "Aktif" : "Pasif";
                        dt.Rows.Add(dr);
                    }

                    ViewState["MalaksorDataTable"] = dt;
                    gvMalaksorler.DataSource = dt;
                    gvMalaksorler.DataBind();

                    pnlMakineForm.Visible = true;
                }
            }
            else if (e.CommandName == "Sil")
            {
                using (var db = new FabrikaDataClassesDataContext())
                {
                    var makina = (from m in db.SirketZeytinyagiMakinalaris
                                  where m.SirketZeytinyagiMakinaID == makinaId
                                  select m).FirstOrDefault();

                    if (makina != null)
                    {
                        makina.Durumu = false;
                        db.SubmitChanges();
                        MessageHelper.ShowSuccessMessage(this, "Başarılı", "Makine başarıyla silindi.");
                        LoadMakineler();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("gvMakineler_RowCommand hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Makine işlemi sırasında bir hata oluştu!"+ex.Message);
        }
    }

    // Malaksör GridView komutlarını işle
    protected void gvMalaksorler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sil")
            {
                int malaksorId = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)ViewState["MalaksorDataTable"];

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["SirketZeytinyagiMakinaMalaksorID"]) == malaksorId)
                    {
                        dr.Delete();
                        break;
                    }
                }

                dt.AcceptChanges();

                // Sıra numaralarını güncelle
                int siraNo = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["MalaksorSiraNo"] = (byte)siraNo++;
                }

                gvMalaksorler.DataSource = dt;
                gvMalaksorler.DataBind();
            }
        }
        catch (Exception ex)
        {
            //Trace.TraceError("gvMalaksorler_RowCommand hatası: " + ex.ToString());
            MessageHelper.ShowErrorMessage(this, "Hata", "Malaksör işlemi sırasında bir hata oluştu!"+ex.Message);
        }
    }
}