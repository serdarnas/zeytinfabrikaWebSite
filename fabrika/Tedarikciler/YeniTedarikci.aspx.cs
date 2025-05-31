using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public partial class fabrika_Tedarikciler_YeniTedarikci : System.Web.UI.Page
{
    private int? TedarikciID
    {
        get { return ViewState["TedarikciID"] != null ? (int?)ViewState["TedarikciID"] : null; }
        set { ViewState["TedarikciID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Tedarikci";
                master.SayfaAdi = "Tedarikci bilgileri güncelleme";
            }

            cbAktif.Checked = true;
            int id;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                TedarikciID = id;
                btnKaydet.Text = "Güncelle";
                LoadTedarikci(id);
            }
            else
            {
                btnKaydet.Text = "Tedarikçiyi Kaydet";
            }
        }
    }

    private void LoadTedarikci(int id)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var t = db.Tedarikcilers.FirstOrDefault(x => x.TedarikciID == id);
                if (t != null)
                {
                    txtTedarikciKodu.Text = t.TedarikciKodu;
                    txtFirmaAdi.Text = t.FirmaAdi;
                    txtYetkiliAdi.Text = t.YetkiliAdi;
                    txtTelefon.Text = t.Telefon;
                    txtCepTelefonu.Text = t.CepTelefonu;
                    txtEmail.Text = t.Email;
                    txtAdres.Text = t.Adres;
                    txtVergiDairesi.Text = t.VergiDairesi;
                    txtVergiNo.Text = t.VergiNo;
                    txtNotlar.Text = t.Notlar;
                    txtBakiyesi.Text = t.Bakiyesi.HasValue ? t.Bakiyesi.Value.ToString("N2") : "0,00";
                    ddlParaBirimi.SelectedValue = t.ParaBirimiID.HasValue ? t.ParaBirimiID.Value.ToString() : "1";
                    txtBankaBilgileri.Text = t.BankaBilgileri;
                    cbAktif.Checked = t.Durum ?? true;
                }
                else
                {
                    ShowError("Tedarikçi bulunamadı.");
                }
            }
        }
        catch (Exception ex)
        {
            //ShowError("Tedarikçi yüklenirken hata: " + ex.Message);
            MessageHelper.ShowErrorMessage(this, "Yeni Tedarikçi", ex.Message);
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        //pnlHata.Visible = false;
        //pnlBasari.Visible = false;

        if (string.IsNullOrWhiteSpace(txtFirmaAdi.Text))
        {
            ShowError("Firma Adı alanı zorunludur.");
            return;
        }

        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                Tedarikciler tedarikci;
                if (TedarikciID.HasValue)
                {
                    tedarikci = db.Tedarikcilers.FirstOrDefault(x => x.TedarikciID == TedarikciID.Value);
                    if (tedarikci == null)
                    {
                        ShowError("Güncellenecek tedarikçi bulunamadı.");
                        return;
                    }
                }
                else
                {
                    tedarikci = new Tedarikciler();
                    tedarikci.SirketID = SessionHelper.GetSirketID();
                    tedarikci.OlusturmaTarihi = DateTime.Now;
                    db.Tedarikcilers.InsertOnSubmit(tedarikci);
                }
                tedarikci.TedarikciKodu = txtTedarikciKodu.Text.Trim();
                tedarikci.FirmaAdi = txtFirmaAdi.Text.Trim();
                tedarikci.YetkiliAdi = txtYetkiliAdi.Text.Trim();
                tedarikci.Telefon = txtTelefon.Text.Trim();
                tedarikci.CepTelefonu = txtCepTelefonu.Text.Trim();
                tedarikci.Email = txtEmail.Text.Trim();
                tedarikci.Adres = txtAdres.Text.Trim();
                tedarikci.VergiDairesi = txtVergiDairesi.Text.Trim();
                tedarikci.VergiNo = txtVergiNo.Text.Trim();
                tedarikci.Notlar = txtNotlar.Text.Trim();
                decimal bakiye = 0;
                decimal.TryParse(txtBakiyesi.Text.Replace(".", ","), out bakiye);
                tedarikci.Bakiyesi = bakiye;
                int paraBirimiId = 1;
                int.TryParse(ddlParaBirimi.SelectedValue, out paraBirimiId);
                tedarikci.ParaBirimiID = paraBirimiId;
                tedarikci.BankaBilgileri = txtBankaBilgileri.Text.Trim();
                tedarikci.Durum = cbAktif.Checked;

                db.SubmitChanges();
            }
            ShowSuccess(TedarikciID.HasValue ? "Tedarikçi başarıyla güncellendi." : "Tedarikçi başarıyla kaydedildi.");
        }
        catch (Exception ex)
        {
            ShowError("Kayıt sırasında hata oluştu: " + ex.Message);
        }
    }

    private void ShowError(string mesaj)
    {
        //pnlHata.Visible = true;
        //lblHata.Text = mesaj;
        MessageHelper.ShowErrorMessage(this, "Yeni Tedrikçi", mesaj);
        }
    private void ShowSuccess(string mesaj)
    {
        //pnlBasari.Visible = true;
        //lblBasari.Text = mesaj;

        MessageHelper.ShowSuccessMessage(this, "Yeni Tedrikçi", mesaj);
    }
}