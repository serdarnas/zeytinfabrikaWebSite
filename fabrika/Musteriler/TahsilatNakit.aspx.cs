using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_TahsilatNakit : System.Web.UI.Page
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
                master.SayfaAdi = "Nakit Tahsilat";
            }

            // Müşteri ID'sini al
            if (Request.QueryString["id"] != null)
            {
                int musteriID;
                if (int.TryParse(Request.QueryString["id"], out musteriID))
                {
                    _MusteriID = musteriID;
                    MusteriBilgileriniYukle();
                    SonIslemleriYukle();
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
            // Hata durumunda log tutulabilir
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

    private void SonIslemleriYukle()
    {
        if (_MusteriID == null) return;

        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            var sonIslemler = db.NakitIslemlers
                .Where(n => n.MusteriID == _MusteriID && n.SirketID == sirketID && 
                           n.IslemTuru == 'T' && n.OdemeTipiID == 1) // T=Tahsilat, 1=Nakit
                .OrderByDescending(n => n.IslemTarihi)
                .Take(5)
                .ToList();

            if (sonIslemler.Any())
            {
                rptSonIslemler.DataSource = sonIslemler;
                rptSonIslemler.DataBind();
                pnlSonIslemYok.Visible = false;
            }
            else
            {
                pnlSonIslemYok.Visible = true;
            }
        }
        catch
        {
            pnlSonIslemYok.Visible = true;
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

            DateTime islemTarihi;
            if (!DateTime.TryParse(txtIslemTarihi.Text, out islemTarihi))
            {
                islemTarihi = DateTime.Now;
            }

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            // Yeni nakit işlem kaydı
            NakitIslemler yeniIslem = new NakitIslemler
            {
                SirketID = sirketID,
                IslemTuru = 'T', // T = Tahsilat (Para Girişi)
                MusteriID = _MusteriID,
                Tutar = tutar,
                ParaBirimiID = 1, // Varsayılan TL
                OdemeTipiID = 1, // Nakit ödeme tipi
                BankaHesapID = null, // Nakit için null
                IslemTarihi = islemTarihi,
                ReferansTipi = "Tahsilat",
                Aciklama = txtAciklama.Text.Trim(),
                KullaniciID = SessionHelper.GetKullaniciID(),
                OlusturmaTarihi = DateTime.Now
            };

            db.NakitIslemlers.InsertOnSubmit(yeniIslem);
            db.SubmitChanges();

            // Başarı mesajı ve form temizleme
            ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                "alert('Nakit tahsilat başarıyla kaydedildi.'); window.location.href = window.location.href;", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Tahsilat kaydedilirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        txtTutar.Text = "";
        txtAciklama.Text = "";
        txtIslemTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        lblTutarGoster.Text = "0,00";
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
