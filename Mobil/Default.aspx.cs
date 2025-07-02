using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

public partial class Mobil_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        VeriYukle();
    }

    private void VeriYukle()
    {
        try
        {
            // Session kontrolü
            if (Session["SirketID"] == null)
            {
                Session.Clear();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Mobil/Login.aspx");
                return;
            }

            // Eğer Session var ama Authentication yok ise cookie'yi tekrar set et
            if (Session["SirketID"] != null && !User.Identity.IsAuthenticated && Session["Email"] != null)
            {
                FormsAuthentication.SetAuthCookie(Session["Email"].ToString(), true);
            }

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Bugünün tarihi
            DateTime bugun = DateTime.Today;
            DateTime ayinIlki = new DateTime(bugun.Year, bugun.Month, 1);

            // Arama terimi al
            string aramaTermi = txtArama.Text.Trim();

            // Şirketin üretim kayıtları (müştahsil, kullanıcı ve ürün bilgileri ile birlikte - LEFT JOIN)
            var tumUretimler = (from u in db.ZeytinyagiUretimleris
                           join m in db.Mustahsillers on u.MustahsilID equals m.MustahsilID into mustahsilJoin
                           from m in mustahsilJoin.DefaultIfEmpty()
                           join k in db.Kullanicilars on u.Tesmilalan_KullaniciID equals k.KullaniciID into kullaniciJoin
                           from k in kullaniciJoin.DefaultIfEmpty()
                           join ur in db.Urunlers on u.Cikan_UrunID equals ur.UrunID into urunJoin
                           from ur in urunJoin.DefaultIfEmpty()
                           join gur in db.Urunlers on u.UrunID equals gur.UrunID into gelisUrunJoin
                           from gur in gelisUrunJoin.DefaultIfEmpty()
                           where u.SirketID == sirketID
                           orderby u.GelisTarihi descending
                           select new
                           {
                               u.ZeytinyagiUretimID,
                               u.PartiNo,
                               u.PlakaNo,
                               u.GelisTarihi,
                               u.GelisKg,
                               u.UrunID,
                               u.Asidite,
                               u.UretimBitisZamani,
                               u.Tesmilalan_KullaniciID,
                               u.CikanUrunKg,
                               u.Cikan_UrunID,
                               u.TahsiliyeToplamUcreti,
                               MustahsilID = u.MustahsilID,
                               MustahsilAd = m != null ? (m.Ad + " " + m.Soyad) : "Müştahsil Bulunamadı",
                               MustahsilAdi = m != null ? m.Ad : "",
                               MustahsilSoyadi = m != null ? m.Soyad : "",
                               MustahsilTelefon = m != null ? m.Telefon : "",
                               MustahsilEmail = m != null ? m.Email : "",
                               MustahsilTCKimlikNo = m != null ? m.TCKimlikNo : "",
                               TesmilalanKullaniciAdi = k != null ? k.AdSoyad : "Kullanıcı Bulunamadı",
                               CikanUrunAdi = ur != null ? ur.UrunAdi : "Bekleniyor",
                               GelisUrunAdi = gur != null ? gur.UrunAdi : "Ürün Bulunamadı"
                           }).ToList();

            // ARAMA FİLTRESİ UYGULA
            var uretimler = tumUretimler;
            if (!string.IsNullOrEmpty(aramaTermi))
            {
                aramaTermi = aramaTermi.ToLower();
                uretimler = tumUretimler.Where(x =>
                    (x.PartiNo != null && x.PartiNo.ToLower().Contains(aramaTermi)) ||
                    (x.MustahsilAd != null && x.MustahsilAd.ToLower().Contains(aramaTermi)) ||
                    (x.MustahsilTelefon != null && x.MustahsilTelefon.ToLower().Contains(aramaTermi)) ||
                    (x.MustahsilEmail != null && x.MustahsilEmail.ToLower().Contains(aramaTermi)) ||
                    (x.MustahsilTCKimlikNo != null && x.MustahsilTCKimlikNo.ToLower().Contains(aramaTermi)) ||
                    (x.PlakaNo != null && x.PlakaNo.ToLower().Contains(aramaTermi))
                ).ToList();
            }

            // İlk 20 sonucu al
            uretimler = uretimler.Take(20).ToList();

            // İstatistikler
            var bugunkuUretimler = db.ZeytinyagiUretimleris
                .Where(x => x.SirketID == sirketID && x.GelisTarihi >= bugun && x.GelisTarihi < bugun.AddDays(1));

            var aylikUretimler = db.ZeytinyagiUretimleris
                .Where(x => x.SirketID == sirketID && x.GelisTarihi >= ayinIlki);

            var aktifUretimler = db.ZeytinyagiUretimleris
                .Where(x => x.SirketID == sirketID && x.UretimBaslamaZamani != null && x.UretimBitisZamani == null);

            // İstatistik kartlarını doldur
            lblBugunkuUretim.Text = bugunkuUretimler.Count().ToString();
            var toplamKg = bugunkuUretimler.Sum(x => x.GelisKg) ?? 0;
            lblToplamKg.Text = toplamKg.ToString("N0");
            lblAktifUretim.Text = aktifUretimler.Count().ToString();
            lblAylikToplam.Text = aylikUretimler.Count().ToString();

            // Liste doldur
            if (uretimler.Any())
            {
                rptUretimler.DataSource = uretimler;
                rptUretimler.DataBind();
                pnlEmpty.Visible = false;
                
                // Sonuç sayısını göster
                if (!string.IsNullOrEmpty(aramaTermi))
                {
                    lblSonucSayisi.Text = string.Format("({0} sonuç)", uretimler.Count);
                }
                else
                {
                    lblSonucSayisi.Text = string.Format("({0} kayıt)", uretimler.Count);
                }
            }
            else
            {
                pnlEmpty.Visible = true;
                lblSonucSayisi.Text = "";
                
                // Arama yapılmış ama sonuç yoksa özel mesaj
                if (!string.IsNullOrEmpty(aramaTermi))
                {
                    litEmptyMessage.Text = "<i class='fas fa-search'></i><h5>Arama sonucu bulunamadı</h5><p>'" + aramaTermi + "' için sonuç bulunamadı. Farklı bir terim deneyin.</p>";
                }
                else
                {
                    litEmptyMessage.Text = "<i class='fas fa-clipboard-list'></i><h5>Henüz üretim kaydı yok</h5><p>İlk üretim kaydınız oluşturulduğunda burada görünecek.</p>";
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda boş değerler göster
            lblBugunkuUretim.Text = "0";
            lblToplamKg.Text = "0";
            lblAktifUretim.Text = "0";
            lblAylikToplam.Text = "0";
            pnlEmpty.Visible = true;
            
            // Debug için hata logla
            System.Diagnostics.Debug.WriteLine("VeriYukle hatası: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
        }
    }

    // Status helper metodları
    protected string GetStatusClass(object uretimBitisZamani)
    {
        if (uretimBitisZamani == null || uretimBitisZamani == DBNull.Value)
        {
            return "status-active";
        }
        return "status-completed";
    }

    protected string GetStatusText(object uretimBitisZamani)
    {
        if (uretimBitisZamani == null || uretimBitisZamani == DBNull.Value)
        {
            return "Devam Ediyor";
        }
        return "Tamamlandı";
    }



    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        // Arama yapıldığında sayfayı yeniden yükle
        VeriYukle();
    }

    protected void btnAra_Click(object sender, EventArgs e)
    {
        // Ara butonuna tıklandığında arama yap
        VeriYukle();
    }

    protected void btnTemizle_Click(object sender, EventArgs e)
    {
        // Arama kutusunu temizle ve tüm sonuçları göster
        txtArama.Text = "";
        VeriYukle();
    }
}