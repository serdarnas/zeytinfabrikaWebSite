using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Mobil_YeniMustahsil : System.Web.UI.Page
{
    private int? MustahsilID
    {
        get { return ViewState["MustahsilID"] != null ? (int?)ViewState["MustahsilID"] : null; }
        set { ViewState["MustahsilID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü
            if (Session["SirketID"] == null || Session["KullaniciID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Varsayılan değerler
            txtBakiyesi.Text = "0";
            cbAktif.Checked = true;

            // URL'den ID parametresi kontrolü (güncelleme için)
            int id;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                MustahsilID = id;
                lblBaslik.Text = "Müstahsil Güncelle";
                btnKaydet.Text = "Güncelle";
                pnlGuncellemeButonlari.Visible = true;
                
                // Mal Kabul butonunu ayarla
                lnkMalKabul.NavigateUrl = "MalKabul.aspx?mustahsilID=" + id;
                lnkMalKabul.Visible = true;
                
                LoadMustahsil(id);
            }
            else
            {
                lblBaslik.Text = "Yeni Müstahsil Ekle";
                btnKaydet.Text = "Müstahsili Kaydet";
                pnlGuncellemeButonlari.Visible = false;
                lnkMalKabul.Visible = false;
            }
        }
    }

    protected void txtTCKimlikNo_TextChanged(object sender, EventArgs e)
    {
        string tcKimlik = txtTCKimlikNo.Text.Trim();
        
        if (string.IsNullOrEmpty(tcKimlik) || tcKimlik.Length != 11)
            return;

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = Convert.ToInt32(Session["SirketID"]);
                
                var mustahsil = db.Mustahsillers
                    .Where(m => m.SirketID == sirketID && m.TCKimlikNo == tcKimlik)
                    .FirstOrDefault();

                if (mustahsil != null)
                {
                    // Eğer şu anda güncelleme yapıyorsak ve aynı müstahsil ise sorun yok
                    if (MustahsilID.HasValue && MustahsilID.Value == mustahsil.MustahsilID)
                        return;

                    // Otomatik olarak müstahsil bilgilerini getir ve formu doldur
                    OtomatikMustahsilBilgileriniGetir(mustahsil, "TC Kimlik No");
                }
            }
        }
        catch (Exception ex)
        {
            MesajGoster("TC Kimlik kontrolünde hata: " + ex.Message, false);
        }
    }

    protected void txtTelefon_TextChanged(object sender, EventArgs e)
    {
        string telefon = txtTelefon.Text.Trim();
        
        if (string.IsNullOrEmpty(telefon) || telefon.Length < 10)
            return;

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = Convert.ToInt32(Session["SirketID"]);
                
                var mustahsil = db.Mustahsillers
                    .Where(m => m.SirketID == sirketID && m.Telefon == telefon)
                    .FirstOrDefault();

                if (mustahsil != null)
                {
                    // Eğer şu anda güncelleme yapıyorsak ve aynı müstahsil ise sorun yok
                    if (MustahsilID.HasValue && MustahsilID.Value == mustahsil.MustahsilID)
                        return;

                    // Otomatik olarak müstahsil bilgilerini getir ve formu doldur
                    OtomatikMustahsilBilgileriniGetir(mustahsil, "Telefon");
                }
            }
        }
        catch (Exception ex)
        {
            MesajGoster("Telefon kontrolünde hata: " + ex.Message, false);
        }
    }

    private void LoadMustahsil(int id)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = Convert.ToInt32(Session["SirketID"]);
                
                var mustahsil = db.Mustahsillers
                    .FirstOrDefault(x => x.MustahsilID == id && x.SirketID == sirketID);

                if (mustahsil != null)
                {
                    txtAd.Text = mustahsil.Ad;
                    txtSoyad.Text = mustahsil.Soyad;
                    txtTCKimlikNo.Text = mustahsil.TCKimlikNo;
                    txtTelefon.Text = mustahsil.Telefon;
                    txtEmail.Text = mustahsil.Email;
                    txtAdres.Text = mustahsil.Adres;
                    txtNotlar.Text = mustahsil.Notlar;
                    txtBakiyesi.Text = mustahsil.Bakiyesi.HasValue ? mustahsil.Bakiyesi.Value.ToString("0.00") : "0.00";
                    txtBankaBilgileri.Text = mustahsil.BankaBilgileri;
                    cbAktif.Checked = mustahsil.Durum ?? true;
                }
                else
                {
                    MesajGoster("Müstahsil bulunamadı.", false);
                    Response.Redirect("MustahsilListesi.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            MesajGoster("Müstahsil yüklenirken hata: " + ex.Message, false);
        }
    }

    private void OtomatikMustahsilBilgileriniGetir(Mustahsiller mustahsil, string kontrolTuru)
    {
        try
        {
            // Güncelleme moduna geç
            MustahsilID = mustahsil.MustahsilID;
            lblBaslik.Text = "Müstahsil Güncelle";
            btnKaydet.Text = "Güncelle";
            pnlGuncellemeButonlari.Visible = true;

            // Tüm form alanlarını doldur
            txtAd.Text = mustahsil.Ad ?? "";
            txtSoyad.Text = mustahsil.Soyad ?? "";
            txtTCKimlikNo.Text = mustahsil.TCKimlikNo ?? "";
            txtTelefon.Text = mustahsil.Telefon ?? "";
            txtEmail.Text = mustahsil.Email ?? "";
            txtAdres.Text = mustahsil.Adres ?? "";
            txtNotlar.Text = mustahsil.Notlar ?? "";
            txtBakiyesi.Text = mustahsil.Bakiyesi.HasValue ? mustahsil.Bakiyesi.Value.ToString("0.00") : "0.00";
            txtBankaBilgileri.Text = mustahsil.BankaBilgileri ?? "";
            cbAktif.Checked = mustahsil.Durum ?? true;

            // Başarı mesajı göster
            string mesaj = string.Format("{0} ile mevcut müstahsil bulundu: <strong>{1} {2}</strong><br/>Bilgileri otomatik yüklendi, güncelleyebilirsiniz.", 
                kontrolTuru, mustahsil.Ad, mustahsil.Soyad);
            
            MesajGoster(mesaj, true);

            // JavaScript ile form alanlarını vurgula (isteğe bağlı)
            string script = @"
                document.addEventListener('DOMContentLoaded', function() {
                    // Form alanlarını kısa süre vurgula
                    var formInputs = document.querySelectorAll('.form-control');
                    formInputs.forEach(function(input) {
                        if (input.value && input.value.trim() !== '' && input.value !== '0.00') {
                            input.style.borderColor = '#28a745';
                            input.style.backgroundColor = '#f8fff9';
                            
                            setTimeout(function() {
                                input.style.borderColor = '';
                                input.style.backgroundColor = '';
                            }, 3000);
                        }
                    });
                });
            ";

            ClientScript.RegisterStartupScript(this.GetType(), "vurgulaFormAlanları", script, true);

            System.Diagnostics.Debug.WriteLine("Otomatik müstahsil yüklendi: " + mustahsil.Ad + " " + mustahsil.Soyad + " (" + kontrolTuru + " ile)");
        }
        catch (Exception ex)
        {
            MesajGoster("Müstahsil bilgileri yüklenirken hata: " + ex.Message, false);
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(txtAd.Text))
        {
            MesajGoster("Ad alanı zorunludur.", false);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtSoyad.Text))
        {
            MesajGoster("Soyad alanı zorunludur.", false);
            return;
        }

        // TC Kimlik No benzersizlik kontrolü
        if (!string.IsNullOrWhiteSpace(txtTCKimlikNo.Text))
        {
            string tcKimlik = txtTCKimlikNo.Text.Trim();
            
            if (tcKimlik.Length != 11)
            {
                MesajGoster("TC Kimlik No 11 haneli olmalıdır.", false);
                return;
            }

            try
            {
                using (var db = new FabrikaDataClassesDataContext())
                {
                    int sirketID = Convert.ToInt32(Session["SirketID"]);
                    
                    var existingMustahsil = db.Mustahsillers
                        .Where(m => m.SirketID == sirketID && m.TCKimlikNo == tcKimlik)
                        .FirstOrDefault();

                    if (existingMustahsil != null)
                    {
                        // Güncelleme durumunda aynı kayıt ise sorun yok
                        if (!MustahsilID.HasValue || MustahsilID.Value != existingMustahsil.MustahsilID)
                        {
                            MesajGoster("Bu TC Kimlik Numarası başka bir müstahsil tarafından kullanılmaktadır.", false);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MesajGoster("TC Kimlik Numarası kontrolünde hata: " + ex.Message, false);
                return;
            }
        }

        // Telefon numarası benzersizlik kontrolü
        if (!string.IsNullOrWhiteSpace(txtTelefon.Text))
        {
            string telefon = txtTelefon.Text.Trim();
            
            if (telefon.Length < 10)
            {
                MesajGoster("Telefon numarası en az 10 haneli olmalıdır.", false);
                return;
            }

            try
            {
                using (var db = new FabrikaDataClassesDataContext())
                {
                    int sirketID = Convert.ToInt32(Session["SirketID"]);
                    
                    var existingMustahsil = db.Mustahsillers
                        .Where(m => m.SirketID == sirketID && m.Telefon == telefon)
                        .FirstOrDefault();

                    if (existingMustahsil != null)
                    {
                        // Güncelleme durumunda aynı kayıt ise sorun yok
                        if (!MustahsilID.HasValue || MustahsilID.Value != existingMustahsil.MustahsilID)
                        {
                            MesajGoster("Bu telefon numarası başka bir müstahsil tarafından kullanılmaktadır.", false);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MesajGoster("Telefon numarası kontrolünde hata: " + ex.Message, false);
                return;
            }
        }

        // E-posta formatı kontrolü
        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                if (addr.Address != txtEmail.Text)
                {
                    MesajGoster("Geçerli bir e-posta adresi giriniz.", false);
                    return;
                }
            }
            catch
            {
                MesajGoster("Geçerli bir e-posta adresi giriniz.", false);
                return;
            }
        }

        // Bakiye kontrolü
        decimal bakiye = 0;
        if (!string.IsNullOrWhiteSpace(txtBakiyesi.Text))
        {
            if (!decimal.TryParse(txtBakiyesi.Text.Replace(',', '.'), out bakiye))
            {
                MesajGoster("Geçerli bir bakiye değeri giriniz.", false);
                return;
            }
        }

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                Mustahsiller mustahsil;
                bool isNew = false;

                if (MustahsilID.HasValue)
                {
                    // Güncelleme
                    mustahsil = db.Mustahsillers.FirstOrDefault(x => x.MustahsilID == MustahsilID.Value);
                    if (mustahsil == null)
                    {
                        MesajGoster("Güncellenecek müstahsil bulunamadı.", false);
                        return;
                    }
                }
                else
                {
                    // Yeni kayıt
                    mustahsil = new Mustahsiller();
                    mustahsil.SirketID = Convert.ToInt32(Session["SirketID"]);
                    mustahsil.OlusturmaTarihi = DateTime.Now;
                    isNew = true;
                }

                // Ortak alanları doldur
                mustahsil.Ad = txtAd.Text.Trim();
                mustahsil.Soyad = txtSoyad.Text.Trim();
                mustahsil.TCKimlikNo = string.IsNullOrWhiteSpace(txtTCKimlikNo.Text) ? null : txtTCKimlikNo.Text.Trim();
                mustahsil.Telefon = string.IsNullOrWhiteSpace(txtTelefon.Text) ? null : txtTelefon.Text.Trim();
                mustahsil.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                mustahsil.Adres = string.IsNullOrWhiteSpace(txtAdres.Text) ? null : txtAdres.Text.Trim();
                mustahsil.Notlar = string.IsNullOrWhiteSpace(txtNotlar.Text) ? null : txtNotlar.Text.Trim();
                mustahsil.BankaBilgileri = string.IsNullOrWhiteSpace(txtBankaBilgileri.Text) ? null : txtBankaBilgileri.Text.Trim();
                mustahsil.Bakiyesi = bakiye;
                mustahsil.Durum = cbAktif.Checked;

                if (isNew)
                {
                    db.Mustahsillers.InsertOnSubmit(mustahsil);
                }

                db.SubmitChanges();

                string mesaj = isNew ? 
                    "Müstahsil başarıyla kaydedildi." : 
                    "Müstahsil bilgileri başarıyla güncellendi.";
                
                MesajGoster(mesaj, true);

                // Yeni kayıttan sonra güncelleme moduna geç
                if (isNew)
                {
                    MustahsilID = mustahsil.MustahsilID;
                    lblBaslik.Text = "Müstahsil Güncelle";
                    btnKaydet.Text = "Güncelle";
                    pnlGuncellemeButonlari.Visible = true;
                    
                    // URL'yi güncelle
                    string newUrl = Request.Url.AbsolutePath + "?id=" + mustahsil.MustahsilID;
                    Response.Redirect(newUrl, false);
                }
            }
        }
        catch (Exception ex)
        {
            MesajGoster("Kaydetme sırasında hata oluştu: " + ex.Message, false);
        }
    }

    private void MesajGoster(string mesaj, bool basarili)
    {
        lblMesaj.Text = mesaj;
        pnlMesaj.Visible = true;

        string cssClass = basarili ? "alert-success" : "alert-danger";
        string iconClass = basarili ? "fas fa-check-circle" : "fas fa-exclamation-triangle";
        
        ClientScript.RegisterStartupScript(this.GetType(), "mesaj",
            string.Format("document.getElementById('divMesaj').className = 'alert {0}'; document.querySelector('#divMesaj i').className = '{1}';", 
                cssClass, iconClass), true);
    }

    // Ajax method for TC Kimlik No check
    [WebMethod]
    public static object CheckTCKimlikNo(string tcKimlikNo, int? currentMustahsilID)
    {
        if (string.IsNullOrEmpty(tcKimlikNo) || tcKimlikNo.Length != 11)
        {
            return new { Exists = false };
        }

        try
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["SirketID"] == null)
            {
                return new { Exists = false, Error = "Session expired" };
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);

            using (var db = new FabrikaDataClassesDataContext())
            {
                var mustahsil = db.Mustahsillers
                    .Where(m => m.SirketID == sirketID && m.TCKimlikNo == tcKimlikNo)
                    .FirstOrDefault();

                if (mustahsil != null)
                {
                    // Aynı kayıt ise sorun yok
                    if (currentMustahsilID.HasValue && currentMustahsilID.Value == mustahsil.MustahsilID)
                    {
                        return new { Exists = false };
                    }

                    return new { 
                        Exists = true, 
                        MustahsilID = mustahsil.MustahsilID,
                        AdSoyad = mustahsil.Ad + " " + mustahsil.Soyad
                    };
                }

                return new { Exists = false };
            }
        }
        catch (Exception ex)
        {
            return new { Exists = false, Error = ex.Message };
        }
    }

    // Ajax method for Telefon check
    [WebMethod]
    public static object CheckTelefon(string telefon, int? currentMustahsilID)
    {
        if (string.IsNullOrEmpty(telefon) || telefon.Length < 10)
        {
            return new { Exists = false };
        }

        try
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["SirketID"] == null)
            {
                return new { Exists = false, Error = "Session expired" };
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);

            using (var db = new FabrikaDataClassesDataContext())
            {
                var mustahsil = db.Mustahsillers
                    .Where(m => m.SirketID == sirketID && m.Telefon == telefon)
                    .FirstOrDefault();

                if (mustahsil != null)
                {
                    // Aynı kayıt ise sorun yok
                    if (currentMustahsilID.HasValue && currentMustahsilID.Value == mustahsil.MustahsilID)
                    {
                        return new { Exists = false };
                    }

                    return new { 
                        Exists = true, 
                        MustahsilID = mustahsil.MustahsilID,
                        AdSoyad = mustahsil.Ad + " " + mustahsil.Soyad,
                        Telefon = mustahsil.Telefon
                    };
                }

                return new { Exists = false };
            }
        }
        catch (Exception ex)
        {
            return new { Exists = false, Error = ex.Message };
        }
    }
}