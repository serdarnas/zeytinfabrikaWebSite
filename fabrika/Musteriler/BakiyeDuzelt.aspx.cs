using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_BakiyeDuzelt : System.Web.UI.Page
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
                master.SayfaAdi = "Bakiye Düzelt";
            }

            // Müşteri ID'sini al
            if (Request.QueryString["id"] != null)
            {
                int musteriID;
                if (int.TryParse(Request.QueryString["id"], out musteriID))
                {
                    _MusteriID = musteriID;
                    MusteriBilgileriniYukle();
                    SonDuzeltmeleriYukle();
                    BakiyeDuzeltmeGecmisiniYukle();
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

            // Bakiye düzeltmeleri
            var bakiyeDuzeltmeleri = db.BakiyeDuzeltmelers
                .Where(b => b.MusteriID == _MusteriID && b.SirketID == sirketID)
                .Sum(b => (decimal?)b.DuzeltmeTutari) ?? 0;

            return satisAlacak - tahsilatlar - cekler - alinanSenetler + verilenSenetler + bakiyeDuzeltmeleri;
        }
        catch
        {
            return 0;
        }
    }

    private void SonDuzeltmeleriYukle()
    {
        if (_MusteriID == null) return;

        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            var sonDuzeltmeler = db.BakiyeDuzeltmelers
                .Where(b => b.MusteriID == _MusteriID && b.SirketID == sirketID)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .Take(5)
                .ToList();

            if (sonDuzeltmeler.Any())
            {
                rptSonDuzeltmeler.DataSource = sonDuzeltmeler;
                rptSonDuzeltmeler.DataBind();
                pnlSonDuzeltmeYok.Visible = false;
            }
            else
            {
                pnlSonDuzeltmeYok.Visible = true;
            }
        }
        catch
        {
            pnlSonDuzeltmeYok.Visible = true;
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || _MusteriID == null) return;

        try
        {
            decimal tutar;
            if (!decimal.TryParse(txtTutar.Text, out tutar))
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

            string duzeltmeTuru = ddlDuzeltmeTuru.SelectedValue;
            decimal mevcutBakiye = HesaplaMevcutBakiye();
            decimal duzeltmeTutari = 0;

            // Düzeltme tutarını hesapla
            switch (duzeltmeTuru)
            {
                case "Artir":
                    duzeltmeTutari = tutar;
                    break;
                case "Azalt":
                    duzeltmeTutari = -tutar;
                    break;
                case "Sifirla":
                    duzeltmeTutari = -mevcutBakiye;
                    break;
                case "Manuel":
                    duzeltmeTutari = tutar - mevcutBakiye;
                    break;
            }

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            // Yeni bakiye düzeltme kaydı
            if (!_MusteriID.HasValue)
            {
                throw new InvalidOperationException("Müşteri ID'si belirtilmemiş.");
            }

            BakiyeDuzeltmeler yeniDuzeltme = new BakiyeDuzeltmeler
            {
                SirketID = sirketID,
                MusteriID = _MusteriID.Value,
                EskiBakiye = mevcutBakiye,
                DuzeltmeTutari = duzeltmeTutari,
                YeniBakiye = mevcutBakiye + duzeltmeTutari,
                DuzeltmeTipi = duzeltmeTuru.Length > 0 ? duzeltmeTuru[0] : 'M',
                Aciklama = txtAciklama.Text.Trim(),
                ReferansBelgeNo = "",
                ReferansTipi = "Manuel",
                OlusturanKullaniciID = SessionHelper.GetKullaniciID(),
                OlusturmaTarihi = DateTime.Now,
                DurumID = 1, // Beklemede
                AktifMi = true
            };

            db.BakiyeDuzeltmelers.InsertOnSubmit(yeniDuzeltme);
            db.SubmitChanges();

            // Bakiye geçmişini yeniden yükle
            BakiyeDuzeltmeGecmisiniYukle();
            
            // Mevcut bakiyeyi güncelle
            decimal yeniBakiye = HesaplaMevcutBakiye();
            lblMevcutBakiye.Text = String.Format("{0:N2}", yeniBakiye);

            // Başarı mesajı ve form temizleme
            ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                "alert('Bakiye düzeltmesi başarıyla kaydedildi.'); document.getElementById('" + txtTutar.ClientID + "').value = ''; document.getElementById('" + txtAciklama.ClientID + "').value = '';", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Bakiye düzeltmesi kaydedilirken hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        txtTutar.Text = "";
        txtAciklama.Text = "";
        txtIslemTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        ddlDuzeltmeTuru.SelectedValue = "Artir";
        ddlDuzeltmeNedeni.SelectedValue = "Diğer";
        
        // JavaScript ile özet alanlarını temizle
        ScriptManager.RegisterStartupScript(this, GetType(), "temizle", 
            "document.getElementById('" + lblDuzeltmeMiktari.ClientID + "').innerText = '0,00'; " +
            "document.getElementById('" + lblYeniBakiye.ClientID + "').innerText = document.getElementById('" + lblMevcutBakiye.ClientID + "').innerText; " +
            "document.getElementById('" + lblDuzeltmeTuruGoster.ClientID + "').innerText = 'Artır';", true);
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

    private void BakiyeDuzeltmeGecmisiniYukle()
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();

            var gecmis = (from bd in db.BakiyeDuzeltmelers
                         join k in db.Kullanicilars on bd.OlusturanKullaniciID equals k.KullaniciID
                         where bd.SirketID == sirketID && bd.MusteriID == _MusteriID && bd.AktifMi == true
                         orderby bd.OlusturmaTarihi descending
                         select new
                         {
                             bd.BakiyeDuzeltmeID,
                             IslemTarihi = bd.OlusturmaTarihi,
                             bd.EskiBakiye,
                             bd.DuzeltmeTutari,
                             bd.YeniBakiye,
                             DuzeltmeTuru = bd.DuzeltmeTipi,
                             DuzeltmeNedeni = bd.Aciklama,
                             bd.Aciklama,
                             KayitTarihi = bd.OlusturmaTarihi,
                             KullaniciAdi = k.AdSoyad,
                             OnayDurumu = bd.DurumID == 2
                         }).Take(20).ToList();

            string html = "<div class='table-responsive'><table class='table table-striped table-sm'>";
            html += "<thead><tr><th>İşlem Tarihi</th><th>Eski Bakiye</th><th>Düzeltme</th><th>Yeni Bakiye</th><th>Tür</th><th>Neden</th><th>Kullanıcı</th><th>Durum</th></tr></thead><tbody>";

            foreach (var item in gecmis)
            {
                string durumText = item.OnayDurumu == true ? "Onaylandı" : "Bekliyor";
                string durumClass = item.OnayDurumu == true ? "text-success" : "text-warning";
                string turText = item.DuzeltmeTuru == 'A' ? "Artır" : item.DuzeltmeTuru == 'Z' ? "Azalt" : item.DuzeltmeTuru == 'S' ? "Sıfırla" : "Manuel";
                
                html += String.Format("<tr><td>{0}</td><td>{1:N2}</td><td class='{2}'>{3:N2}</td><td>{4:N2}</td><td>{5}</td><td>{6}</td><td>{7}</td><td class='{8}'>{9}</td></tr>",
                    item.IslemTarihi.ToString("dd.MM.yyyy"),
                    item.EskiBakiye,
                    item.DuzeltmeTutari >= 0 ? "text-success" : "text-danger",
                    item.DuzeltmeTutari,
                    item.YeniBakiye,
                    turText,
                    item.DuzeltmeNedeni,
                    item.KullaniciAdi,
                    durumClass,
                    durumText);
            }

            html += "</tbody></table></div>";

            // JavaScript ile tabloyu güncelle
            ScriptManager.RegisterStartupScript(this, GetType(), "updateHistory", 
                String.Format("document.getElementById('divBakiyeGecmisi').innerHTML = '{0}';", html.Replace("'", "\\'").Replace("\n", "")), true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "historyError", 
                String.Format("console.log('Bakiye geçmişi yüklenirken hata: {0}');", ex.Message), true);
        }
    }
}
