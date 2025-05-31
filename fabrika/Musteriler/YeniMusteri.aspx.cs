using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class fabrika_Musteriler_YeniMusteri : System.Web.UI.Page
{
    // Not: Şirket ID için artık SessionHelper.GetSirketID() kullanılıyor
    private int? MusteriID
    {
        get
        {
            if (ViewState["MusteriID"] != null)
                return Convert.ToInt32(ViewState["MusteriID"]);
            return null;
        }
        set
        {
            ViewState["MusteriID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // SirketID kontrolü
                int sirketID = SessionHelper.GetSirketID();
                var master = this.Master as fabrika_FabrikaMasterPage;
                if (master != null)
                {
                    master.KlasorAdi = "Müşteri";
                    master.SayfaAdi = "Yeni Müşteri";
                }
                // Kategori ve Para Birimi dropdownlarını doldur
                LoadKategoriler();
                LoadParaBirimleri();

                // Varsayılan değerler
                txtBakiyesi.Text = "0,00";

                // URL'den MusteriID parametresini kontrol et
                int musteriId;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out musteriId))
                {
                    MusteriID = musteriId;
                    btnKaydet.Text = "✓ Güncelle";
                    LoadCustomerData(musteriId);
                }
                else
                {
                    btnKaydet.Text = "✓ Kaydet";
                }
            }
            catch (Exception ex)
            {
                ShowError("Sayfa yüklenirken bir hata oluştu: " + ex.Message);
            }
        }
    }

    private void LoadKategoriler()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                var kategoriler = db.MusteriKategorileris
                    .Where(k => k.SirketID == sirketID)
                    .OrderBy(k => k.KategoriAdi)
                    .ToList();

                ddlKategori.DataSource = kategoriler;
                ddlKategori.DataTextField = "KategoriAdi";
                ddlKategori.DataValueField = "KategoriID";
                ddlKategori.DataBind();
                ddlKategori.Items.Insert(0, new ListItem("Seçiniz", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowError("Kategori listesi yüklenirken hata: " + ex.Message);
        }
    }

    private void LoadParaBirimleri()
    {
        try
        {
            ddlParaBirimi.Items.Clear();
            ddlParaBirimi.Items.Add(new ListItem("TL", "1"));
            ddlParaBirimi.Items.Add(new ListItem("USD", "2"));
            ddlParaBirimi.Items.Add(new ListItem("EUR", "3"));
        }
        catch (Exception ex)
        {
            ShowError("Para birimi listesi yüklenirken hata: " + ex.Message);
        }
    }

    private void LoadCustomerData(int musteriId)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var musteri = db.Musterilers.FirstOrDefault(m => m.MusteriID == musteriId);
                if (musteri != null)
                {
                    txtMusteriKodu.Text = musteri.MüsteriKodu;
                    txtMusteriAdi.Text = musteri.FirmaAdi;
                    txtYetkiliAdi.Text = musteri.YetkiliAdi;
                    txtTelefon.Text = musteri.Telefon;
                    txtCepTelefonu.Text = musteri.CepTelefonu;
                    txtEmail.Text = musteri.Email;
                    txtAdres.Text = musteri.Adres;
                    txtVergiDairesi.Text = musteri.VergiDairesi;
                    txtVergiNo.Text = musteri.VergiNo;
                    txtBakiyesi.Text = Convert.ToDecimal(musteri.Bakiyesi).ToString("N2").Replace(".", ",");
                    txtSabitİskonto.Text = Convert.ToDecimal(musteri.Sabitİskonto).ToString("N2").Replace(".", ",");
                    txtVade.Text = musteri.Vadesi.HasValue ? musteri.Vadesi.Value.ToString() : "0";
                    txtNot.Text = musteri.Notlar;
                    cbAktif.Checked = musteri.Durum ?? false;

                    if (musteri.KategoriID.HasValue)
                        ddlKategori.SelectedValue = musteri.KategoriID.ToString();

                    if (musteri.ParaBirimiID.HasValue)
                        ddlParaBirimi.SelectedValue = musteri.ParaBirimiID.ToString();

                    if (!string.IsNullOrEmpty(musteri.MusteriResim))
                    {
                        imgMusteri.ImageUrl = musteri.MusteriResim;
                        imgMusteri.Visible = true;
                    }
                }
                else
                {
                    ShowError("Müşteri bulunamadı.");
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Müşteri bilgileri yüklenirken bir hata oluştu: " + ex.Message);
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateForm())
                return;

            using (var db = new FabrikaDataClassesDataContext())
            {
                if (MusteriID.HasValue)
                {
                    UpdateCustomer(db);
                }
                else
                {
                    InsertCustomer(db);
                }
            }

            ShowSuccess("Müşteri başarıyla " + (MusteriID.HasValue ? "güncellendi" : "kaydedildi") + ".");
            RedirectToDefault();
        }
        catch (Exception ex)
        {
            ShowError("İşlem sırasında bir hata oluştu: " + ex.Message);
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtMusteriAdi.Text))
        {
            ShowError("Müşteri adı boş olamaz.");
            return false;
        }

        if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
        {
            ShowError("Geçerli bir e-posta adresi giriniz.");
            return false;
        }

        if (fuResim.HasFile)
        {
            string extension = Path.GetExtension(fuResim.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            
            if (!allowedExtensions.Contains(extension))
            {
                ShowError("Lütfen geçerli bir resim dosyası seçin (jpg, jpeg, png, gif).");
                return false;
            }
        }

        return true;
    }

    private void UpdateCustomer(FabrikaDataClassesDataContext db)
    {
        var musteri = db.Musterilers.FirstOrDefault(m => m.MusteriID == MusteriID.Value);
        if (musteri == null)
            throw new Exception("Güncellenecek müşteri bulunamadı.");

        UpdateCustomerProperties(musteri);
        HandleImageUpload(musteri);
        db.SubmitChanges();
    }

    private void InsertCustomer(FabrikaDataClassesDataContext db)
    {
        var yeniMusteri = new Musteriler
        {
            SirketID = SessionHelper.GetSirketID(),
            OlusturmaTarihi = DateTime.Now,
            Durum = true
        };

        UpdateCustomerProperties(yeniMusteri);
        HandleImageUpload(yeniMusteri);
        db.Musterilers.InsertOnSubmit(yeniMusteri);
        db.SubmitChanges();
    }

    private void UpdateCustomerProperties(Musteriler musteri)
    {
        musteri.MüsteriKodu = txtMusteriKodu.Text.Trim();
        musteri.FirmaAdi = txtMusteriAdi.Text.Trim();
        musteri.YetkiliAdi = txtYetkiliAdi.Text.Trim();
        musteri.Telefon = txtTelefon.Text.Trim();
        musteri.CepTelefonu = txtCepTelefonu.Text.Trim();
        musteri.Email = txtEmail.Text.Trim();
        musteri.Adres = txtAdres.Text.Trim();
        musteri.VergiDairesi = txtVergiDairesi.Text.Trim();
        musteri.VergiNo = txtVergiNo.Text.Trim();
        musteri.Notlar = txtNot.Text.Trim();
        musteri.Durum = cbAktif.Checked;

        // Sayısal değerler
        decimal bakiye;
        if (decimal.TryParse(txtBakiyesi.Text.Replace(',', '.'), out bakiye))
            musteri.Bakiyesi = bakiye;

        decimal iskonto;
        if (decimal.TryParse(txtSabitİskonto.Text.Replace(',', '.'), out iskonto))
            musteri.Sabitİskonto = iskonto;

        int vade;
        if (int.TryParse(txtVade.Text, out vade))
            musteri.Vadesi = (byte?)vade;

        // Dropdown değerleri
        int kategoriId;
        if (int.TryParse(ddlKategori.SelectedValue, out kategoriId) && kategoriId > 0)
            musteri.KategoriID = kategoriId;
        else
            musteri.KategoriID = null;

        int paraBirimiId;
        if (int.TryParse(ddlParaBirimi.SelectedValue, out paraBirimiId))
            musteri.ParaBirimiID = paraBirimiId;
    }

    private void HandleImageUpload(Musteriler musteri)
    {
        if (fuResim.HasFile)
        {
            string klasorYolu = Server.MapPath("~/fabrika/Musteriler/MusteriResimleri/");
            if (!Directory.Exists(klasorYolu))
                Directory.CreateDirectory(klasorYolu);

            string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(fuResim.FileName);
            string tamYol = Path.Combine(klasorYolu, dosyaAdi);
            
            fuResim.SaveAs(tamYol);
            musteri.MusteriResim = "~/fabrika/Musteriler/MusteriResimleri/" + dosyaAdi;
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private void ShowError(string message)
    {
        
        MessageHelper.ShowErrorMessage(this,"Yeni Müşteri",message);
    }

    private void ShowSuccess(string message)
    {
        MessageHelper.ShowErrorMessage(this, "Yeni Müşteri", message);
    }

    private void RedirectToDefault()
    {
        string script = "setTimeout(function() { window.location.href = 'Default.aspx'; }, 2000);";
        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", script, true);
    }

    protected void btnKategoriKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            string kategoriAdi = txtYeniKategoriAdi.Text.Trim();
            string kategoriAciklama = txtYeniKategoriAciklama.Text.Trim();

            if (string.IsNullOrEmpty(kategoriAdi))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Kategori adı boş olamaz!');", true);
                return;
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                var yeniKategori = new MusteriKategorileri
                {
                    SirketID = SessionHelper.GetSirketID(),
                    KategoriAdi = kategoriAdi,
                    Aciklama = kategoriAciklama
                };

                db.MusteriKategorileris.InsertOnSubmit(yeniKategori);
                db.SubmitChanges();

                // Yeni eklenen kategoriyi seç
                ddlKategori.SelectedValue = yeniKategori.KategoriID.ToString();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "kategoriEklendi", "kategoriEklendi();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", 
                "alert('Kategori eklenirken bir hata oluştu: " + ex.Message + "');", true);
        }
    }
}
