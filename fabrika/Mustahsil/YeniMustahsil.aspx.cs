using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Services;

public partial class fabrika_Mustahsil_YeniMustahsil : System.Web.UI.Page
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
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Mustahsil";
                master.SayfaAdi = "Mustahsil bilgileri güncelleme";
            }

            txtBakiyesi.Text = "0,00";
            cbAktif.Checked = true;

            int id;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                MustahsilID = id;
                btnKaydet.Text = "Güncelle";
                LoadMustahsil(id);
            }
            else
            {
                btnKaydet.Text = "Müstahsili Kaydet";
            }
            
            // TC Kimlik Numarası için JavaScript olayını kaydet
            SetupTCKimlikValidation();
        }
    }

    private void SetupTCKimlikValidation()
    {
        // TC Kimlik Numarası değiştiğinde kontrol eden JavaScript kodu
        string script = @"
            function checkTCKimlik() {
                var tcKimlik = $('#" + txtTCKimlikNo.ClientID + @"').val();
                if (tcKimlik && tcKimlik.length === 11) {
                    // Ajax çağrısı ile TC Kimlik No kontrolü
                    $.ajax({
                        type: 'POST',
                        url: '" + ResolveUrl("~/fabrika/Mustahsil/YeniMustahsil.aspx/CheckTCKimlikNo") + @"',
                        data: JSON.stringify({ tcKimlikNo: tcKimlik }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function(response) {
                            var result = response.d;
                            if (result && result.Exists) {
                                if (confirm('Bu TC Kimlik Numarası ile kayıtlı bir müstahsil bulunmaktadır. Bilgilerini getirmek ister misiniz?')) {
                                    window.location.href = 'YeniMustahsil.aspx?id=' + result.MustahsilID;
                                }
                            }
                        },
                        error: function(xhr, status, error) {
                            console.error('TC Kimlik kontrolünde hata: ' + error);
                        }
                    });
                }
            }

            $(document).ready(function() {
                $('#" + txtTCKimlikNo.ClientID + @"').on('blur', checkTCKimlik);
            });";

        ScriptManager.RegisterStartupScript(this, GetType(), "TCKimlikValidation", script, true);
    }

    [WebMethod]
    public static object CheckTCKimlikNo(string tcKimlikNo)
    {
        if (string.IsNullOrEmpty(tcKimlikNo) || tcKimlikNo.Length != 11)
        {
            return new { Exists = false };
        }

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var mustahsil = db.Mustahsillers.Where(m => m.TCKimlikNo != null && m.TCKimlikNo == tcKimlikNo).FirstOrDefault();
                if (mustahsil != null)
                {
                    return new { Exists = true, MustahsilID = mustahsil.MustahsilID };
                }
                return new { Exists = false };
            }
        }
        catch
        {
            return new { Exists = false, Error = true };
        }
    }

    private void LoadMustahsil(int id)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var m = db.Mustahsillers.FirstOrDefault(x => x.MustahsilID == id);
                if (m != null)
                {
                    txtAd.Text = m.Ad;
                    txtSoyad.Text = m.Soyad;
                    txtTCKimlikNo.Text = m.TCKimlikNo;
                    txtTelefon.Text = m.Telefon;
                    txtEmail.Text = m.Email;
                    txtAdres.Text = m.Adres;
                    txtNotlar.Text = m.Notlar;
                    txtBakiyesi.Text = m.Bakiyesi.HasValue ? m.Bakiyesi.Value.ToString("N2") : "0,00";
                    txtBankaBilgileri.Text = m.BankaBilgileri;
                    cbAktif.Checked = m.Durum ?? true;
                }
                else
                {
                    ShowError("Müstahsil bulunamadı.");
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Müstahsil yüklenirken hata: " + ex.Message);
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        

        if (string.IsNullOrWhiteSpace(txtAd.Text))
        {
            ShowError("Ad alanı zorunludur.");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtSoyad.Text))
        {
            ShowError("Soyad alanı zorunludur.");
            return;
        }

        // TC Kimlik No benzersizlik kontrolü
        if (!string.IsNullOrWhiteSpace(txtTCKimlikNo.Text))
        {
            try
            {
                using (var db = new FabrikaDataClassesDataContext())
                {

                    string tcKimlik = txtTCKimlikNo.Text;
                    int _sirketID = SessionHelper.GetSirketID();
                    Mustahsiller existingMustahsil = db.Mustahsillers.Where(m => m.SirketID == _sirketID && m.TCKimlikNo == tcKimlik).FirstOrDefault();

                    if (existingMustahsil != null)
                    {
                        MessageHelper.ShowErrorMessage(this, "Yeni Müstahsil", "Bu TC Kimlik Numarası başka bir müstahsil tarafından kullanılmaktadır. Lütfen kontrol ediniz. ");
                        
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("TC Kimlik Numarası kontrolünde hata: " + ex.Message);
                return;
            }
        }

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                Mustahsiller mustahsil;
                if (MustahsilID.HasValue)
                {
                    mustahsil = db.Mustahsillers.FirstOrDefault(x => x.MustahsilID == MustahsilID.Value);
                    if (mustahsil == null)
                    {
                        ShowError("Güncellenecek müstahsil bulunamadı.");
                        return;
                    }
                }
                else
                {
                    mustahsil = new Mustahsiller();
                    mustahsil.SirketID = SessionHelper.GetSirketID();
                    mustahsil.OlusturmaTarihi = DateTime.Now;
                    db.Mustahsillers.InsertOnSubmit(mustahsil);
                }
                mustahsil.Ad = txtAd.Text.Trim();
                mustahsil.Soyad = txtSoyad.Text.Trim();
                mustahsil.TCKimlikNo = txtTCKimlikNo.Text.Trim();
                mustahsil.Telefon = txtTelefon.Text.Trim();
                mustahsil.Email = txtEmail.Text.Trim();
                mustahsil.Adres = txtAdres.Text.Trim();
                mustahsil.Notlar = txtNotlar.Text.Trim();
                decimal bakiye = 0;
                decimal.TryParse(txtBakiyesi.Text.Replace(".", ","), out bakiye);
                mustahsil.Bakiyesi = bakiye;
                mustahsil.BankaBilgileri = txtBankaBilgileri.Text.Trim();
                mustahsil.Durum = cbAktif.Checked;

                db.SubmitChanges();
                
                // Kayıt işlemi başarılı ise
                MustahsilID = mustahsil.MustahsilID;
                btnKaydet.Text = "Güncelle";
                ShowSuccess(MustahsilID.HasValue ? "Müstahsil başarıyla güncellendi." : "Müstahsil başarıyla kaydedildi.");
            }
        }
        catch (Exception ex)
        {
            ShowError("Kayıt sırasında hata oluştu: " + ex.Message);
        }
    }

    private void ShowError(string mesaj)
    { 
        
        // Gritter bildirimi ekle
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorNotification", 
            string.Format("showErrorMessage('Hata', '{0}');", mesaj.Replace("'", "\\'")), true);
    }

    private void ShowSuccess(string mesaj)
    {
        
        
        // Gritter bildirimi ekle
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessNotification", 
            string.Format("showSuccessMessage('Başarılı', '{0}');", mesaj.Replace("'", "\\'")), true);
    }

    protected void txtTCKimlikNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                string tcKimlik = txtTCKimlikNo.Text;
                int _sirketID = SessionHelper.GetSirketID();
                Mustahsiller existingMustahsil = db.Mustahsillers.Where(m =>m.SirketID==_sirketID&&m.TCKimlikNo== tcKimlik).FirstOrDefault();

                if (existingMustahsil != null)
                {
                    ShowError("Bu TC Kimlik Numarası başka bir müstahsil tarafından kullanılmaktadır. Lütfen kontrol ediniz.");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("TC Kimlik Numarası kontrolünde hata: " + ex.Message);
            return;
        }
    }
}