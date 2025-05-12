using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

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
                {txtAd.Text = m.Ad;
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
        pnlHata.Visible = false;
        pnlBasari.Visible = false;

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
            }
            ShowSuccess(MustahsilID.HasValue ? "Müstahsil başarıyla güncellendi." : "Müstahsil başarıyla kaydedildi.");
        }
        catch (Exception ex)
        {
            ShowError("Kayıt sırasında hata oluştu: " + ex.Message);
        }
    }

    private void ShowError(string mesaj)
    {
        pnlHata.Visible = true;
        lblHata.Text = mesaj;
    }
    private void ShowSuccess(string mesaj)
    {
        pnlBasari.Visible = true;
        lblBasari.Text = mesaj;
    }
}