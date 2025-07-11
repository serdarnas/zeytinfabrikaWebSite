using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_TahsilatCek : System.Web.UI.Page
{
    private int? _MusteriID
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
            // Master page ayarları
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müşteri";
                master.SayfaAdi = "Çek Tahsilat";
            }

            // Müşteri ID'sini al
            if (Request.QueryString["id"] != null)
            {
                int musteriID;
                if (int.TryParse(Request.QueryString["id"], out musteriID))
                {
                    _MusteriID = musteriID;
                    MusteriBilgileriniYukle();
                    SonCekleriYukle();
                }
                else
                {
                    Response.Redirect("MusteriListesi.aspx");
                }
            }
            else
            {
                Response.Redirect("MusteriListesi.aspx");
            }
        }
    }

    private void MusteriBilgileriniYukle()
    {
        if (_MusteriID == null) return;

        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            var musteri = db.Musterilers.FirstOrDefault(m => m.MusteriID == _MusteriID && m.SirketID == sirketID);
            if (musteri != null)
            {
                lblMusteriAdi.Text = musteri.FirmaAdi;
                lblMusteriAdres.Text = musteri.Adres ?? "";

                // Mevcut bakiyeyi hesapla
                decimal mevcutBakiye = HesaplaMevcutBakiye();
                lblMevcutBakiye.Text = String.Format("{0:N2}", mevcutBakiye);
            }
            else
            {
                Response.Redirect("MusteriListesi.aspx");
            }
        }
        catch (Exception ex)
        {
            lblMusteriAdi.Text = "Müşteri bilgisi yüklenemedi";
        }
    }

    private decimal HesaplaMevcutBakiye()
    {
        if (_MusteriID == null) return 0;

        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            // Satışlardan alacak
            var satisAlacak = db.Satislars
                .Where(s => s.MusteriID == _MusteriID && s.SirketID == sirketID)
                .Sum(s => (decimal?)s.ToplamTutar) ?? 0;

            // Tahsilatlardan düşülecek (T = Tahsilat)
            var tahsilatlar = db.NakitIslemlers
                .Where(n => n.MusteriID == _MusteriID && n.SirketID == sirketID && n.IslemTuru == 'T')
                .Sum(n => (decimal?)n.Tutar) ?? 0;

            // Çeklerden düşülecek
            var cekler = db.Ceklers
                .Where(c => c.AlinanMusteriID == _MusteriID && c.SirketID == sirketID)
                .Sum(c => (decimal?)c.Tutar) ?? 0;

            // Alınan senetlerden düşülecek
            var alinanSenetler = db.Senetlers
                .Where(s => s.IlgiliMusteriID == _MusteriID && s.SirketID == sirketID && s.SenetTipi == 'A')
                .Sum(s => (decimal?)s.Tutar) ?? 0;

            // Verilen senetler borç olarak eklenir
            var verilenSenetler = db.Senetlers
                .Where(s => s.IlgiliMusteriID == _MusteriID && s.SirketID == sirketID && s.SenetTipi == 'V')
                .Sum(s => (decimal?)s.Tutar) ?? 0;

            return satisAlacak - tahsilatlar - cekler - alinanSenetler + verilenSenetler;
        }
        catch
        {
            return 0;
        }
    }

    private void SonCekleriYukle()
    {
        if (_MusteriID == null) return;

        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            var sonCekler = db.Ceklers
                .Where(c => c.AlinanMusteriID == _MusteriID && c.SirketID == sirketID)
                .OrderByDescending(c => c.OlusturmaTarihi)
                .Take(5)
                .ToList();

            if (sonCekler.Any())
            {
                rptSonCekler.DataSource = sonCekler;
                rptSonCekler.DataBind();
                pnlSonCekYok.Visible = false;
            }
            else
            {
                pnlSonCekYok.Visible = true;
            }
        }
        catch
        {
            pnlSonCekYok.Visible = true;
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || _MusteriID == null) return;

        try
        {
            decimal tutar;
            if (!decimal.TryParse(txtTutar.Text, out tutar) || tutar <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Geçerli bir tutar giriniz.');", true);
                return;
            }

            DateTime cekTarihi, vadeTarihi, islemTarihi;
            if (!DateTime.TryParse(txtCekTarihi.Text, out cekTarihi))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Geçerli bir çek tarihi giriniz.');", true);
                return;
            }

            if (!DateTime.TryParse(txtVadeTarihi.Text, out vadeTarihi))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Geçerli bir vade tarihi giriniz.');", true);
                return;
            }

            if (!DateTime.TryParse(txtIslemTarihi.Text, out islemTarihi))
            {
                islemTarihi = DateTime.Now;
            }

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            // Yeni çek kaydı
            Cekler yeniCek = new Cekler
            {
                SirketID = sirketID,
                AlinanMusteriID = _MusteriID.Value,
                SeriNo = txtCekNo.Text.Trim(),
                Tutar = tutar,
                BankaAdi = txtBankaAdi.Text.Trim(),
                SubeAdi = txtSube.Text.Trim(),
                HesapNo = "", // Boş bırakılabilir
                Kesideci = "", // Boş bırakılabilir
                KesideTarihi = cekTarihi,
                VadeTarihi = vadeTarihi,
                AlisTarihi = islemTarihi,
                ParaBirimiID = 1, // Varsayılan TL
                DurumID = int.Parse(ddlCekDurumu.SelectedValue),
                OdemeYeri = "",
                Aciklama = txtAciklama.Text.Trim(),
                OlusturmaTarihi = DateTime.Now
            };

            db.Ceklers.InsertOnSubmit(yeniCek);
            db.SubmitChanges();

            // Başarı mesajı ve form temizleme
            ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                "alert('Çek başarıyla kaydedildi.'); window.location.href = window.location.href;", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Çek kaydedilirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        txtTutar.Text = "";
        txtCekNo.Text = "";
        txtBankaAdi.Text = "";
        txtSube.Text = "";
        txtAciklama.Text = "";
        txtIslemTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtCekTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtVadeTarihi.Text = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
        ddlCekDurumu.SelectedValue = "Portföyde";
        lblTutarGoster.Text = "0,00";
        lblVadeGoster.Text = "-";
        lblDurumGoster.Text = "Portföyde";
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        if (_MusteriID != null)
        {
            Response.Redirect(String.Format("MusteriDetay.aspx?id={0}", _MusteriID));
        }
        else
        {
            Response.Redirect("MusteriListesi.aspx");
        }
    }
}
