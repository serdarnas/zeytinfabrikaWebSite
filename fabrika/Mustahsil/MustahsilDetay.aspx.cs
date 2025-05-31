using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Mustahsil_MustahsilDetay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müstahsil";
                master.SayfaAdi = "Müstahsil Detay";
            }

            // URL'den ID parametresini al
            int mustahsilID;
            if (Request.QueryString["ID"] != null && int.TryParse(Request.QueryString["ID"], out mustahsilID))
            {
                LoadMustahsilDetails(mustahsilID);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void LoadMustahsilDetails(int mustahsilID)
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            Mustahsiller mustahsil = db.Mustahsillers.FirstOrDefault(x => x.MustahsilID == mustahsilID);

            if (mustahsil != null)
            {
                // Profil resmi için varsayılan resim
                MustahsilResim.ImageUrl = "../../App_Themes/serdarnas_admin_flat/img/mustahsil.jpg";

                // Temel bilgileri doldur
                lblMustahsilAdi.Text = mustahsil.Ad + " " + mustahsil.Soyad;
                lblAdres.Text = mustahsil.Adres;
                lblCepTelefonu.Text = mustahsil.Telefon; // Telefon numarasını kullan
                lblmail.Text = mustahsil.Email;
                lblTCKimlik.Text = mustahsil.TCKimlikNo;
              

                // Bakiye bilgilerini doldur
                lblAlacakBakiye.Text = mustahsil.Bakiyesi.HasValue ? mustahsil.Bakiyesi.Value.ToString("N2") + " TL" : "0,00 TL";

                // Ürün miktarı ve ödemelerin toplamını hesapla
                decimal toplamUrunMiktari = 0;
                decimal toplamOdeme = 0;
                decimal ciroToplam = 0;

                // Müstahsil ilişkili alışlar üzerinden toplam ürün miktarını ve ödemeleri hesapla
                var alislar = db.Alislars.Where(a => a.MustahsilID == mustahsilID);
                if (alislar.Any())
                {
                    ciroToplam = alislar.Sum(a => a.GenelToplam ?? 0);

                    // Ürün miktarlarını topla
                    foreach (var alis in alislar)
                    {
                        var detaylar = db.AlisDetaylaris.Where(d => d.AlisID == alis.AlisID);
                        toplamUrunMiktari += detaylar.Sum(d => d.Miktar);
                    }
                }

                // Zeytinyağı üretimleri varsa onların miktarlarını da ekle
                var zeytinyagiUretimleri = db.ZeytinyagiUretimleris.Where(z => z.MustahsilID == mustahsilID);
                if (zeytinyagiUretimleri.Any())
                {
                    toplamUrunMiktari += zeytinyagiUretimleri.Sum(z => z.GelisKg ?? 0);
                }

                lblUrunMiktari.Text = toplamUrunMiktari.ToString("N2") + " kg";
                lblOdemeMiktari.Text = toplamOdeme.ToString("N2") + " TL";
                lblCiroToplam.Text = ciroToplam.ToString("N2") + " TL";

                // Butonların yönlendirme URL'lerini ayarla
                hplinkUrunAlimi.NavigateUrl = "../Alislar/Yeni.aspx?mustahsilID=" + mustahsilID;
                btnOdemeYap.NavigateUrl = "../Finans/OdemeYap.aspx?mustahsilID=" + mustahsilID;
                btnHesapEkstresi.NavigateUrl = "HesapEkstresi.aspx?MustahsilID=" + mustahsilID;
                hplinkMustahsilGuncelle.NavigateUrl = "YeniMustahsil.aspx?MustahsilID=" + mustahsilID;
                hplinkZeytinyagıicinUrunAlimi.NavigateUrl= "~/fabrika/Zeytinyagi/ZeytinKabulYeni.aspx?MustahsilID=" + mustahsilID;

                // Önceki ürün alımları ve ödemeleri doldur
                // Bu kısımlar için ayrı metotlar kullanabilirsiniz
                LoadPreviousPurchases(mustahsilID);
                LoadPreviousPayments(mustahsilID);
                LoadMustahsilVouchers(mustahsilID);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama yapılabilir
            Response.Write("Hata: " + ex.Message);
        }
    }

    private void LoadPreviousPurchases(int mustahsilID)
    {
        // Burada müstahsile ait önceki ürün alımlarını yükleyecek kod olabilir
        // Örneğin bir GridView veya Repeater kontrolü kullanılabilir
    }

    private void LoadPreviousPayments(int mustahsilID)
    {
        // Burada müstahsile yapılan önceki ödemeleri yükleyecek kod olabilir
    }

    private void LoadMustahsilVouchers(int mustahsilID)
    {
        // Burada müstahsil makbuzlarını yükleyecek kod olabilir
    }
}