using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.ScriptManager;

public partial class Musteriler_MusteriSatis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
            int sirketID = SessionHelper.GetSirketID();
            // SirketID geçerli, devam et
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müşteri";
                master.SayfaAdi = "Müşteri Satiş";
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }
        
        if (!IsPostBack)
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();
            LoadProjeler(sirketID);
            LoadPazarlamacilar(sirketID);
            int _MusteriID = int.Parse(Request.QueryString["id"]);
            Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == _MusteriID);
            lblMusteriAd.Text = gelenMusteriler.FirmaAdi;


            hplbtnGeriDon.NavigateUrl = "MusteriDetay.aspx?id=" + _MusteriID;
            
            // Script dosyalarını kaydet
            RegisterScripts();

            // Button event handlers
            btnProformaSiparis.Click += new EventHandler(btnProformaSiparis_Click);
            btnIrsaliyeKaydet.Click += new EventHandler(btnIrsaliyeKaydet_Click);
        }
    }

    // Proforma/Sipariş kaydetme
    protected void btnProformaSiparis_Click(object sender, EventArgs e)
    {
        SatisKaydet("PROFORMA");
    }

    // İrsaliye kaydetme
    protected void btnIrsaliyeKaydet_Click(object sender, EventArgs e)
    {
        SatisKaydet("IRSALIYE");
    }

    // Fatura kaydetme - mevcut method
    protected void btnFaturaKaydet_Click(object sender, EventArgs e)
    {
        SatisKaydet("FATURA");
    }

    // Ortak satış kaydetme metodu
    private void SatisKaydet(string satisTipi)
    {
        try
        {
            // Session'da sepet var mı kontrol et
            if (Session["Sepet"] == null || ((List<SepetItem>)Session["Sepet"]).Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "sepetBos", 
                    "alert('Sepetinizde ürün bulunmamaktadır. Lütfen sepete ürün ekleyin.');", true);
                return;
            }
            
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                
                // URL'den müşteri ID'sini al
                int musteriID = GetMusteriIDFromSession();
                if (musteriID <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "musteriHata", 
                        "alert('Müşteri bilgisi bulunamadı. Sayfayı yenileyin veya müşteri listesine geri dönün.');", true);
                    return;
                }
                
                // Ana satış kaydını oluştur
                Satislar yeniSatis = new Satislar();
                yeniSatis.SirketID = sirketID;
                yeniSatis.MusteriID = musteriID;
                yeniSatis.SatisBelgeNo = txtBelgeNo.Text.Trim();
                
                // SatisTipi alanını ayarla
                yeniSatis.SatisTipi = satisTipi;
                
                // Tarih ve saat bilgilerini parse et
                DateTime satistarihi;
                if (DateTime.TryParse(txtTarih.Text, out satistarihi))
                {
                    yeniSatis.SatisTarihi = satistarihi;
                }
                else
                {
                    yeniSatis.SatisTarihi = DateTime.Now; // Geçerli tarih-saat olarak kullan
                }
                
                // Vade tarihini parse et
                DateTime vadetarihi;
                if (DateTime.TryParse(txtVadesi.Text, out vadetarihi))
                {
                    yeniSatis.VadeTarihi = vadetarihi;
                }
                
                // Sevk tarihini parse et
                DateTime sevktarihi;
                if (DateTime.TryParse(txtSevkTarihi.Text, out sevktarihi))
                {
                    yeniSatis.SevkTarihi = sevktarihi;
                }
                
                // Sipariş tarihini parse et (sevk tarihine atıyoruz)
                DateTime siparistarihi;
                if (DateTime.TryParse(txtSiparisTarih.Text, out siparistarihi))
                {
                    yeniSatis.SevkTarihi = siparistarihi;
                }
                
                // Düz text alanlarını doldur
                yeniSatis.IrsaliyeNo = txtIrsaliyeNo.Text.Trim();
                yeniSatis.Notlar = txtAciklama.Text.Trim();
                
                // Dropdown seçimlerini parse et
                int pazarlamaciID;
                if (!string.IsNullOrEmpty(ddlPazarlama.SelectedValue) && int.TryParse(ddlPazarlama.SelectedValue, out pazarlamaciID))
                {
                    yeniSatis.PazarlamaciID = pazarlamaciID;
                }
                
                int projeID;
                if (!string.IsNullOrEmpty(ddlProje.SelectedValue) && int.TryParse(ddlProje.SelectedValue, out projeID))
                {
                    yeniSatis.ProjeID = projeID;
                }
                
                // Oluşturma tarihi
                yeniSatis.OlusturmaTarihi = DateTime.Now;
                
                // Ödeme durumu - varsayılan olarak "Ödenmedi"
                yeniSatis.OdemeDurumu = "Ödenmedi";
                
                // Toplamları hesapla
                decimal toplamTutar = 0;
                decimal toplamIndirim = 0;
                decimal toplamKdv = 0;
                
                // Sepet ürünlerini al
                List<SepetItem> sepet = (List<SepetItem>)Session["Sepet"];
                
                foreach (var item in sepet)
                {
                    // Ara toplamları hesapla
                    decimal araToplam = item.Miktar * item.BirimFiyat;
                    decimal indirimTutari = 0;
                    
                    if (item.IskontoTuru == "%")
                    {
                        indirimTutari = araToplam * item.Iskonto / 100;
                    }
                    else
                    {
                        indirimTutari = item.Iskonto;
                    }
                    
                    decimal netTutar = araToplam - indirimTutari;
                    decimal kdvTutari = netTutar * item.KDV / 100;
                    
                    // Genel toplamları güncelle
                    toplamTutar += araToplam;
                    toplamIndirim += indirimTutari;
                    toplamKdv += kdvTutari;
                }
                
                // Toplamları ana kayda ekle
                yeniSatis.ToplamTutar = toplamTutar;
                yeniSatis.indirimTutari = toplamIndirim;
                yeniSatis.KDVToplam = toplamKdv;
                yeniSatis.GenelToplam = toplamTutar - toplamIndirim + toplamKdv;
                
                // Veritabanına ana kaydı ekle
                db.Satislars.InsertOnSubmit(yeniSatis);
                db.SubmitChanges(); // SatisID'yi almak için hemen kaydet
                
                // Satış detayları listesi oluştur
                List<SatisDetaylari> satisDetaylari = new List<SatisDetaylari>();
                
                // Sepet içindeki her ürün için detay kaydı oluştur
                foreach (var item in sepet)
                {
                    SatisDetaylari detay = new SatisDetaylari();
                    detay.SirketID = sirketID;
                    detay.SatisID = yeniSatis.SatisID;
                    detay.UrunID = item.UrunID;
                    detay.Miktar = item.Miktar;
                    detay.BirimFiyat = item.BirimFiyat;
                    
                    // İndirimi hesapla
                    decimal araToplam = item.Miktar * item.BirimFiyat;
                    decimal indirimTutari = 0;
                    decimal indirimOrani = 0;
                    
                    if (item.IskontoTuru == "%")
                    {
                        indirimTutari = araToplam * item.Iskonto / 100;
                        indirimOrani = item.Iskonto;
                    }
                    else
                    {
                        indirimTutari = item.Iskonto;
                        // Yüzde olarak indirim oranını hesapla
                        indirimOrani = (araToplam > 0) ? (indirimTutari * 100) / araToplam : 0;
                    }
                    
                    detay.indirimOrani = indirimOrani;
                    detay.indirimTutari = indirimTutari;
                    
                    decimal netTutar = araToplam - indirimTutari;
                    decimal kdvTutari = netTutar * item.KDV / 100;
                    
                    detay.KDVOrani = item.KDV;
                    detay.KDVTutari = kdvTutari;
                    detay.ToplamTutar = netTutar + kdvTutari;
                    
                    // Eğer depo seçilirse (şu anda sabit Ana Depo)
                    detay.DepoID = 1; // Ana Depo ID'si
                    
                    db.SatisDetaylaris.InsertOnSubmit(detay);
                    satisDetaylari.Add(detay);
                }
                
                // Tüm değişiklikleri kaydet
                db.SubmitChanges();
                
                                // SatisTipi'ne göre stok hareketi yap veya yapma                if (satisTipi == "FATURA") // Sadece fatura için stok hareketi yapılacak                {                    // Fatura işlemi için stok hareketi oluştur                    bool stokSonuc = StokHelper.SatisStokCikisiYap(sirketID, yeniSatis.SatisID, satisDetaylari, GetKullaniciID());                                        if (!stokSonuc)                    {                        // Stok çıkışı yapılamadı ama satış kaydedildi - log ekle                        System.Diagnostics.Debug.WriteLine("UYARI: Satış kaydedildi fakat stok çıkışı yapılamadı. SatisID: " + yeniSatis.SatisID);                    }                }                // İrsaliye ve Proforma/Sipariş için stok hareketi yapılmayacak
                
                // Sepeti temizle
                Session["Sepet"] = new List<SepetItem>();
                
                // İşlem tipine göre başarılı mesajı göster
                string islemAdi = satisTipi == "FATURA" ? "Fatura" : 
                                 (satisTipi == "IRSALIYE" ? "İrsaliye" : "Proforma/Sipariş");
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "kayitBasarili", 
                    "alert('" + islemAdi + " başarıyla kaydedildi. Belge No: " + yeniSatis.SatisID + "'); " +
                    "window.location='MusteriDetay.aspx?id=" + musteriID + "';", true);
            }
        }
        catch (Exception ex)
        {
            // Hata mesajı göster
            ScriptManager.RegisterStartupScript(this, this.GetType(), "kayitHatasi", 
                "alert('Kayıt sırasında bir hata oluştu: " + ex.Message + "');", true);
            
            // Hatayı logla
            System.Diagnostics.Debug.WriteLine("Satış kaydetme hatası: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Hata detayı: " + ex.StackTrace);
        }
    }
} 