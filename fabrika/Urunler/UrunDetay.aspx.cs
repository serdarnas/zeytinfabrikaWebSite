using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Urunler_UrunDetay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Ürünler";
            master.SayfaAdi = "Ürün Detay";
        }
        if (!IsPostBack)
        {
            int urunId = 0;
            int.TryParse(Request.QueryString["UrunID"], out urunId);
            if (urunId > 0)
            {
                LoadUrunDetay(urunId);
            }
        }
    }

    private void LoadUrunDetay(int urunId)
    {
        using (var db = new FabrikaDataClassesDataContext())
        {
            var urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId);
            if (urun == null) return;

            lblUrunAdi.Text = urun.UrunAdi;
            lblKategori.Text = urun.UrunKategorileri != null ? urun.UrunKategorileri.Ad : "";
            lblMarka.Text = urun.Markalar != null ? urun.Markalar.Ad : "";
            lblBirim.Text = urun.Birimler != null ? urun.Birimler.BirimAdi : "";
            lblUrunKodu.Text = urun.UrunKodu;
            lblBarkod.Text = urun.Barkod;
            lblStokMiktari.Text = urun.StokMiktari.HasValue ? urun.StokMiktari.Value.ToString("N2") : "";
            lblMinimumStok.Text = urun.MinimumStok.HasValue ? urun.MinimumStok.Value.ToString("N2") : "";
            lblUrunTipiStoklu.Text = urun.UrunTipiStoklu.HasValue && urun.UrunTipiStoklu.Value ? "Evet" : "Hayır";
            lblDurum.Text = urun.Durum.HasValue && urun.Durum.Value ? "Aktif" : "Pasif";
            lblMamul.Text = urun.Mamul.HasValue && urun.Mamul.Value ? "Evet" : "Hayır";
            lblYariMamul.Text = urun.YariManul.HasValue && urun.YariManul.Value ? "Evet" : "Hayır";
            lblAlisFiyati.Text = urun.AlisFiyati.HasValue ? urun.AlisFiyati.Value.ToString("N2") : "";
            lblAlisKdv.Text = urun.AlisKdv.HasValue ? urun.AlisKdv.Value.ToString() : "";
            lblAlisFiyatiKdvDahilmi.Text = urun.AlisFiyatiKdvDahilmi.HasValue && urun.AlisFiyatiKdvDahilmi.Value ? "Evet" : "Hayır";
            lblSatisFiyati.Text = urun.SatisFiyati.HasValue ? urun.SatisFiyati.Value.ToString("N2") : "";
            //lblSatisKdv.Text = urun.SatisKdv.HasValue ? urun.SatisKdv.Value.ToString() : "";
            //lblSatisFiyatiKdvDahilmi.Text = urun.SatisFiyatiKdvDahilmi.HasValue && urun.SatisFiyatiKdvDahilmi.Value ? "Evet" : "Hayır";
            //lblParaBirimi.Text = urun.ParaBirimileri != null ? urun.ParaBirimileri.ParaBirimiAd : "";
            lblKDVOrani.Text = urun.KDVOrani.HasValue ? urun.KDVOrani.Value.ToString("N2") : "";
            //lblPerakendeSatisVarmi.Text = urun.PerakendeSatisVarmi.HasValue && urun.PerakendeSatisVarmi.Value ? "Evet" : "Hayır";
            //lblPerakendeSatisFiyati.Text = urun.PerakendeSatisFiyati.HasValue ? urun.PerakendeSatisFiyati.Value.ToString("N2") : "";
            //lblPerakendeSatisKdvDahilmi.Text = urun.PerakendeSatisKdvDahilmi.HasValue && urun.PerakendeSatisKdvDahilmi.Value ? "Evet" : "Hayır";
            lblNotlar.Text = urun.Notlar;

            // Stok Değeri Hesaplama
            decimal stokDegeri = (urun.StokMiktari ?? 0) * (urun.AlisFiyati ?? 0);
            lblStokDegeri.Text = stokDegeri.ToString("N2");

            // Satışlar
            var satislar = (from s in db.SatisDetaylaris
                            where s.UrunID == urunId
                            orderby s.Satislar.SatisTarihi descending
                            select new
                            {
                                Tarih = s.Satislar != null ? s.Satislar.SatisTarihi : (DateTime?)null,
                                Musteri = s.Satislar != null && s.Satislar.Musteriler != null ? s.Satislar.Musteriler.FirmaAdi : "",
                                Miktar = s.Miktar,
                                Fiyat = s.BirimFiyat
                            }).Take(10).ToList();
            rptSatislar.DataSource = satislar;
            rptSatislar.DataBind();
            pnlSatislarBos.Visible = satislar == null || satislar.Count == 0;

            // Alışlar
            var alislar = (from a in db.AlisDetaylaris
                           where a.UrunID == urunId
                           orderby a.Alislar.AlisTarihi descending
                           select new
                           {
                               Tarih = a.Alislar != null ? a.Alislar.AlisTarihi : (DateTime?)null,
                               Tedarikci = a.Alislar != null && a.Alislar.Tedarikciler != null ? a.Alislar.Tedarikciler.FirmaAdi : "",
                               Miktar = a.Miktar,
                               Fiyat = a.BirimFiyat
                           }).Take(10).ToList();
            rptAlislar.DataSource = alislar;
            rptAlislar.DataBind();
            pnlAlislarBos.Visible = alislar == null || alislar.Count == 0;

            // Resimler
            var resimler = urun.UrunGorselleris.Select(r => new { ImageUrl = r.ResimYol }).ToList();
            rptResimler.DataSource = resimler;
            rptResimler.DataBind();
            if (resimler.Count > 0)
                imgUrunResim.ImageUrl = resimler[0].ImageUrl;
            else
                imgUrunResim.ImageUrl = "/App_Themes/serdarnas_admin_flat/img/no-image.png";

            // Varyantlar
            var varyantlar = urun.UrunVaryantlaris.SelectMany(v => v.UrunVaryantDetays.Select(d => new
            {
                VaryantTuru = d.VaryantDegerleri != null && d.VaryantDegerleri.VaryantTurleri != null ? d.VaryantDegerleri.VaryantTurleri.TurAdi : "",
                VaryantDegeri = d.VaryantDegerleri != null ? d.VaryantDegerleri.DegerAdi : "",
                ResimYolu = urun.UrunGorselleris.FirstOrDefault(g => g.UrunVaryantID == v.UrunVaryantID) != null ? urun.UrunGorselleris.FirstOrDefault(g => g.UrunVaryantID == v.UrunVaryantID).ResimYol : ""
            })).ToList();
            if (varyantlar.Count <= 0)
                VaryatDiv.Visible = false;

            gvVaryantlar.DataSource = varyantlar;
            gvVaryantlar.DataBind();

            // Güncelle butonu
            btnGuncelle.NavigateUrl = "YeniUrun.aspx?UrunID=" + urun.UrunID;
        }
    }
}