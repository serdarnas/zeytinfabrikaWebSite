using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
// StokHelper sınıfını kullanmak için gerekli

public partial class fabrika_Musteriler_MusteriSatis : System.Web.UI.Page
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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Page_Load Hata: " + ex.Message);
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }
        
        if (!IsPostBack)
        {
            try
            {
                using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
                {
                    int sirketID = SessionHelper.GetSirketID();
                    LoadProjeler(sirketID);
                    LoadPazarlamacilar(sirketID);
                    LoadDepolar(sirketID);
                    
                    // QueryString güvenli parse işlemi
                    int _MusteriID;
                    if (!int.TryParse(Request.QueryString["id"], out _MusteriID) || _MusteriID <= 0)
                    {
                        Response.Redirect("~/fabrika/Musteriler/Default.aspx");
                        return;
                    }
                    
                    Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == _MusteriID && x.SirketID == sirketID);
                    if (gelenMusteriler == null)
                    {
                        Response.Redirect("~/fabrika/Musteriler/Default.aspx");
                        return;
                    }
                    
                    // Null-safe assignment
                    lblMusteriAd.Text = !string.IsNullOrEmpty(gelenMusteriler.FirmaAdi) ? gelenMusteriler.FirmaAdi : "Bilinmeyen Müşteri";
                    
                    // Varsayılan değerleri ayarla
                    txtTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    txtSaat.Text = DateTime.Now.ToString("HH:mm");
                    txtVadesi.Text = DateTime.Now.AddDays(30).ToString("dd.MM.yyyy");
                    txtSevkTarihi.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    txtSiparisTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    
                    // Otomatik belge numarası üret
                    string belgeNo = GenerateBelgeNo(sirketID);
                    txtBelgeNo.Text = belgeNo;

                    hplbtnGeriDon.NavigateUrl = "MusteriDetay.aspx?id=" + _MusteriID;
                    
                    // Script dosyalarını kaydet
                    RegisterScripts();
                    
                    // Button event handlers
                    btnProformaSiparis.Click += new EventHandler(btnProformaSiparis_Click);
                    btnIrsaliyeKaydet.Click += new EventHandler(btnIrsaliyeKaydet_Click);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Page_Load İnitializasyon Hatası: " + ex.Message);
                Response.Redirect("~/fabrika/Musteriler/Default.aspx");
            }
        }
    }

    private void RegisterScripts()
    {
        // jQuery CDN üzerinden yükleniyor, burada bir şey yapılmasına gerek yok
    }

    private void LoadPazarlamacilar(int sirketID)
    {
        try
        {
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                var tumPazarlamacilar = db.Pazarlamacilars
                    .Where(x => x.SirketID == sirketID && x.Durum == true)
                    .ToList();
                
                // Boş seçenek ekle
                ddlPazarlama.Items.Clear();
                ddlPazarlama.Items.Add(new ListItem("Seçiniz", ""));
                ddlPazarlama.AppendDataBoundItems = true;
                
                ddlPazarlama.DataSource = tumPazarlamacilar;
                ddlPazarlama.DataTextField = "PazarlamaciAdi";
                ddlPazarlama.DataValueField = "PazarlamaciID";
                ddlPazarlama.DataBind();
                
                // Boş seçeneği seç
                ddlPazarlama.SelectedValue = "";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("LoadPazarlamacilar Hatası: " + ex.Message);
            // Hata durumunda en azından boş dropdown olsun
            ddlPazarlama.Items.Clear();
            ddlPazarlama.Items.Add(new ListItem("Seçiniz", ""));
        }
    }

    private void LoadProjeler(int sirketID)
    {
        try
        {
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                var tumProjeler = db.Projelers
                    .Where(x => x.SirketID == sirketID)
                    .ToList();
                
                // Boş seçenek ekle
                ddlProje.Items.Clear();
                ddlProje.Items.Add(new ListItem("Seçiniz", ""));
                ddlProje.AppendDataBoundItems = true;
                
                ddlProje.DataSource = tumProjeler;
                ddlProje.DataTextField = "ProjeAdi";
                ddlProje.DataValueField = "ProjeID";
                ddlProje.DataBind();
                
                // Boş seçeneği seç
                ddlProje.SelectedValue = "";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("LoadProjeler Hatası: " + ex.Message);
            // Hata durumunda en azından boş dropdown olsun
            ddlProje.Items.Clear();
            ddlProje.Items.Add(new ListItem("Seçiniz", ""));
        }
    }

    private void LoadDepolar(int sirketID)
    {
        try
        {
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                System.Diagnostics.Debug.WriteLine("LoadDepolar başlıyor - SirketID: " + sirketID);
                
                // Önce tüm depoları debug için listele
                var tumDepolar = db.Depolars.Where(d => d.SirketID == sirketID).ToList();
                System.Diagnostics.Debug.WriteLine("Toplam depo sayısı (SirketID=" + sirketID + "): " + tumDepolar.Count);
                
                foreach (var d in tumDepolar)
                {
                    System.Diagnostics.Debug.WriteLine("Depo - ID: " + d.DepoID + ", Adı: " + (d.DepoAdi ?? "null") + ", Kodu: " + (d.DepoKodu ?? "null") + ", Durum: " + d.Durum);
                }
                
                // Depoları JSON formatında Frontend'e gönder
                // Durum kontrolünü nullable boolean için yap
                var depolar = db.Depolars
                    .Where(d => d.SirketID == sirketID && (d.Durum == true || d.Durum.HasValue && d.Durum.Value == true))
                    .Select(d => new { 
                        DepoID = d.DepoID, 
                        DepoAdi = d.DepoAdi ?? "",
                        DepoKodu = d.DepoKodu ?? "",
                        Kapasite = d.Kapasite ?? 0,
                        DoluMiktar = d.DoluMiktar ?? 0,
                        Durum = d.Durum
                    })
                    .ToList();
                    
                System.Diagnostics.Debug.WriteLine("Aktif depo sayısı: " + depolar.Count);
                
                // JavaScript'e depo listesini gönder
                string depoJson = Newtonsoft.Json.JsonConvert.SerializeObject(depolar);
                System.Diagnostics.Debug.WriteLine("Depo JSON: " + depoJson);
                
                ClientScript.RegisterStartupScript(this.GetType(), "DepoListesi", 
                    string.Format("console.log('Depo listesi yüklendi:', {0}); window.depoListesi = {0}; window.depolarYuklendi = true;", depoJson), true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("LoadDepolar Hatası: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("LoadDepolar Hata Detayı: " + ex.StackTrace);
            // Hata durumunda boş depo listesi gönder
            ClientScript.RegisterStartupScript(this.GetType(), "DepoListesi", 
                "console.error('Depo yükleme hatası'); window.depoListesi = []; window.depolarYuklendi = false;", true);
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<UrunArama> UrunAra(string aramaMetni)
    {
        try 
        {
            int sirketID = SessionHelper.GetSirketID();
            List<UrunArama> sonuclar = new List<UrunArama>();
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Önce sadece şirket ID'sine göre filtreleme yapalım
                var urunlerQuery = db.Urunlers.Where(u => u.SirketID == sirketID);
                
                // Arama metni varsa arama kriterlerini uygulayalım
                if (!string.IsNullOrEmpty(aramaMetni))
                {
                    urunlerQuery = urunlerQuery.Where(u => 
                        u.UrunAdi.Contains(aramaMetni) || 
                        (u.UrunKodu != null && u.UrunKodu.Contains(aramaMetni)) || 
                        (u.Barkod != null && u.Barkod.Contains(aramaMetni)));
                }
                
                // Sonuçları alıp dönüştürelim
                var urunler = urunlerQuery
                    .Take(20)
                    .ToList()
                    .Select(u => new UrunArama
                    {
                        UrunID = u.UrunID,
                        UrunKodu = u.UrunKodu ?? "",
                        UrunTipiStoklu = u.UrunTipiStoklu.HasValue ? u.UrunTipiStoklu.Value : false,
                        Barkod = u.Barkod ?? "",
                        UrunAdi = u.UrunAdi,
                        SatisFiyati = u.SatisFiyati.HasValue ? u.SatisFiyati.Value : 0,
                        StokMiktari = u.StokMiktari.HasValue ? u.StokMiktari.Value : 0,
                        BirimAdi = u.BirimID.HasValue ? (db.Birimlers.FirstOrDefault(b => b.BirimID == u.BirimID) != null ? db.Birimlers.FirstOrDefault(b => b.BirimID == u.BirimID).BirimAdi : "") : "",
                        KDVOrani = u.KDVOrani.HasValue ? u.KDVOrani.Value : 0
                    })
                    .ToList();
                
                return urunler;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda boş liste dön
            return new List<UrunArama> { 
                new UrunArama { 
                    UrunID = -1, 
                    UrunAdi = "HATA: " + ex.Message, 
                    UrunKodu = ex.StackTrace != null ? ex.StackTrace.Substring(0, Math.Min(50, ex.StackTrace.Length)) : ""
                }
            };
        }
    }
    
    [WebMethod]
    public static UrunDetay GetUrunDetay(int urunId)
    {
        using (var db = new FabrikaDataClassesDataContext())
        {
            var urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId);
            if (urun != null)
            {
                return new UrunDetay
                {
                    UrunID = urun.UrunID,
                    UrunAdi = urun.UrunAdi,
                    UrunKodu = urun.UrunKodu,
                    Barkod = urun.Barkod,
                    BirimFiyat = urun.SatisFiyati.HasValue ? urun.SatisFiyati.Value : 0,
                    KDVOrani = urun.KDVOrani.HasValue ? urun.KDVOrani.Value : 0,
                    BirimAdi = urun.BirimID.HasValue ? (db.Birimlers.FirstOrDefault(b => b.BirimID == urun.BirimID) != null ? db.Birimlers.FirstOrDefault(b => b.BirimID == urun.BirimID).BirimAdi : "") : "",
                    StokMiktari = urun.StokMiktari.HasValue ? urun.StokMiktari.Value : 0
                };
            }
            return null;
        }
    }

    // Sayfa üzerinden yapılacak ürün araması
    protected void txtUrunAra_TextChanged(object sender, EventArgs e)
    {
        // Otomatik arama için direkt tıklama butonunu aktive et
        if (!string.IsNullOrWhiteSpace(txtUrunAra.Text) && txtUrunAra.Text.Length >= 2)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchModalAutoOpen", "araUrunler();", true);
        }
    }
    
    protected void btnUrunAra_Click(object sender, EventArgs e)
    {
        // AJAX ile yapılacak, OnClientClick'te araUrunler() fonksiyonu çağrılıyor
    }
    
    [WebMethod]
    public static bool UrunSepeteEkle(int urunId, decimal miktar, decimal birimFiyat, int kdvOrani, decimal indirim, string indirimTuru, string aciklama, int depoId)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            int musteriID = GetMusteriIDFromSession();
            
            // Geçerlilik kontrolleri
            if (urunId <= 0)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Geçersiz ürün ID - " + urunId);
                return false;
            }
            
            if (miktar <= 0)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Geçersiz miktar - " + miktar);
                return false;
            }
            
            if (birimFiyat < 0)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Geçersiz birim fiyat - " + birimFiyat);
                return false;
            }
            
            // Session kontrolü
            if (HttpContext.Current.Session == null)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Session null");
                return false;
            }
            
            // Session'da sepet yoksa oluştur
            if (HttpContext.Current.Session["Sepet"] == null)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle: Yeni sepet oluşturuluyor");
                HttpContext.Current.Session["Sepet"] = new List<SepetItem>();
            }
            
            // Sepeti al
            var sepet = HttpContext.Current.Session["Sepet"] as List<SepetItem>;
            if (sepet == null)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Sepet list null");
                // Session'ı resetleme girişimi
                HttpContext.Current.Session["Sepet"] = new List<SepetItem>();
                sepet = HttpContext.Current.Session["Sepet"] as List<SepetItem>;
                
                if (sepet == null)
                {
                    System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Sepet yeniden oluşturma başarısız");
                    return false;
                }
            }
            
            // Ürün var mı kontrol et
            var mevcutUrun = sepet.FirstOrDefault(x => x.UrunID == urunId);
            if (mevcutUrun != null)
            {
                // Varsa güncelle
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle: Mevcut ürün güncelleniyor - ID: " + urunId);
                mevcutUrun.Miktar = miktar;
                mevcutUrun.BirimFiyat = birimFiyat;
                mevcutUrun.KDV = kdvOrani;
                mevcutUrun.Iskonto = indirim;
                mevcutUrun.IskontoTuru = indirimTuru;
                mevcutUrun.Aciklama = aciklama;
                mevcutUrun.DepoID = depoId;
            }
            else
            {
                // Yoksa ekle
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle: Yeni ürün ekleniyor - ID: " + urunId);
                
                using (var db = new FabrikaDataClassesDataContext())
                {
                    var urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId);
                    if (urun != null)
                    {
                        var birimAdi = "Adet";
                        if (urun.BirimID.HasValue)
                        {
                            var birim = db.Birimlers.FirstOrDefault(b => b.BirimID == urun.BirimID);
                            if (birim != null)
                            {
                                birimAdi = birim.BirimAdi;
                            }
                        }
                        
                        var yeniUrun = new SepetItem
                        {
                            UrunID = urunId,
                            UrunKodu = urun.UrunKodu,
                            UrunAdi = urun.UrunAdi,
                            Miktar = miktar,
                            BirimFiyat = birimFiyat,
                            Birim = birimAdi,
                            KDV = kdvOrani,
                            Iskonto = indirim,
                            IskontoTuru = indirimTuru ?? "%",
                            Aciklama = aciklama,
                            DepoID = depoId
                        };
                        sepet.Add(yeniUrun);
                        System.Diagnostics.Debug.WriteLine("UrunSepeteEkle: Ürün başarıyla eklendi - ID: " + urunId);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: Ürün veritabanında bulunamadı - ID: " + urunId);
                        return false; // Ürün bulunamadı
                    }
                }
            }
            
            // Session'a geri kaydet
            HttpContext.Current.Session["Sepet"] = sepet;
            System.Diagnostics.Debug.WriteLine("UrunSepeteEkle: Session'a sepet kaydedildi - Ürün sayısı: " + sepet.Count);
            
            return true;
        }
        catch (Exception ex)
        {
            // Hata detaylı loglanıyor
            System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Stack Trace: " + ex.StackTrace);
            
            // InnerException varsa onu da logla
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine("UrunSepeteEkle Inner Exception: " + ex.InnerException.Message);
            }
            
            return false;
        }
    }
    
    [WebMethod]
    public static List<SepetItem> GetSepetUrunleri()
    {
        try
        {
            // Session'da sepet yoksa oluştur
            if (HttpContext.Current.Session["Sepet"] == null)
            {
                HttpContext.Current.Session["Sepet"] = new List<SepetItem>();
            }
            
            // Sepeti al
            var sepet = (List<SepetItem>)HttpContext.Current.Session["Sepet"];
            return sepet;
        }
        catch (Exception ex)
        {
            // Hata loglanabilir
            System.Diagnostics.Debug.WriteLine("GetSepetUrunleri Error: " + ex.Message);
            return new List<SepetItem>();
        }
    }
    
    [WebMethod]
    public static bool UrunSepettenCikar(int urunId)
    {
        try
        {
            // Session'da sepet yoksa false döndür
            if (HttpContext.Current.Session["Sepet"] == null)
            {
                return false;
            }
            
            // Sepeti al
            var sepet = (List<SepetItem>)HttpContext.Current.Session["Sepet"];
            
            // Ürünü bul ve çıkar
            var urun = sepet.FirstOrDefault(x => x.UrunID == urunId);
            if (urun != null)
            {
                sepet.Remove(urun);
                
                // Session'a geri kaydet
                HttpContext.Current.Session["Sepet"] = sepet;
                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            // Hata loglanabilir
            System.Diagnostics.Debug.WriteLine("UrunSepettenCikar Error: " + ex.Message);
            return false;
        }
    }
    
    private static int GetMusteriIDFromSession()
    {
        try
        {
            // URL'den müşteri ID'sini al
            if (HttpContext.Current.Request.QueryString["id"] != null)
            {
                int musteriID;
                if (int.TryParse(HttpContext.Current.Request.QueryString["id"], out musteriID) && musteriID > 0)
                {
                    return musteriID;
                }
            }
            
            return 0; // Geçersiz müşteri ID
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GetMusteriIDFromSession Hatası: " + ex.Message);
            return 0; // Hata durumunda geçersiz ID dön
        }
    }

    // Proforma/Sipariş button handler
    protected void btnProformaSiparis_Click(object sender, EventArgs e)
    {
        SatisKaydet("PROFORMA");
    }

    // İrsaliye button handler
    protected void btnIrsaliyeKaydet_Click(object sender, EventArgs e)
    {
        SatisKaydet("IRSALIYE");
    }

    // Existing Fatura button handler modified to call shared method
    protected void btnFaturaKaydet_Click(object sender, EventArgs e)
    {
        SatisKaydet("FATURA");
    }

    // Shared sale saving method with SatisTipi parameter
    private void SatisKaydet(string satisTipi)
    {
        try
        {
            // Session'da sepet var mı kontrol et
            if (Session["Sepet"] == null || ((List<SepetItem>)Session["Sepet"]).Count == 0)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Sepette ürün bulunmamaktadır. Lütfen önce sepete ürün ekleyiniz.");
                return;
            }
            
            int sirketID = SessionHelper.GetSirketID();
            int tempMusteriID;
            int? musteriID = int.TryParse(Request.QueryString["id"], out tempMusteriID) ? (int?)tempMusteriID : null;
            
            if (musteriID == null)
            {
                MessageHelper.ShowErrorMessage(this, "Hata", "Geçerli bir müşteri seçilmelidir.");
                return;
            }
            
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                // Ana satış kaydını oluştur
                Satislar yeniSatis = new Satislar();
                yeniSatis.SirketID = sirketID;
                yeniSatis.MusteriID = musteriID.Value;
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
                
                // Toplam indirim oranını hesapla (toplam tutar sıfır değilse)
                if (toplamTutar > 0)
                {
                    yeniSatis.indirimOrani = (toplamIndirim * 100) / toplamTutar;
                }
                else
                {
                    yeniSatis.indirimOrani = 0;
                }
                
                yeniSatis.KDVToplam = toplamKdv;
                yeniSatis.GenelToplam = toplamTutar - toplamIndirim + toplamKdv;
                
                // Ana satış kaydını ekle
                db.Satislars.InsertOnSubmit(yeniSatis);
                db.SubmitChanges();

                // Detay kayıtlarını topla
                List<SatisDetaylari> satisDetaylari = new List<SatisDetaylari>();
                
                // Satış detaylarını ekle
                foreach (var item in sepet)
                {
                    SatisDetaylari detay = new SatisDetaylari();
                    detay.SirketID = sirketID;
                    detay.SatisID = yeniSatis.SatisID;
                    detay.UrunID = item.UrunID;
                    decimal miktar = item.Miktar;
                    detay.Miktar = (int)item.Miktar;
                    detay.BirimFiyat = item.BirimFiyat;
                    
                    // İndirimi hesapla
                    decimal araToplam = miktar * item.BirimFiyat;
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
                    
                    detay.indirimOrani = (int?)indirimOrani;
                    detay.indirimTutari = indirimTutari;
                    
                    decimal netTutar = araToplam - indirimTutari;
                    decimal kdvTutari = netTutar * item.KDV / 100;
                    
                    detay.KDVOrani = (int)item.KDV;
                    detay.KDVTutari = kdvTutari;
                    detay.ToplamTutar = netTutar + kdvTutari;
                    
                    // Sepet öğesinden depo ID'sini al
                    detay.DepoID = item.DepoID > 0 ? item.DepoID : 1; // Varsayılan olarak Ana Depo (ID=1)
                    
                    db.SatisDetaylaris.InsertOnSubmit(detay);
                    satisDetaylari.Add(detay);
                }
                
                // Tüm değişiklikleri kaydet
                db.SubmitChanges();
                
                // SatisTipi'ne göre stok hareketi yapıp yapmamayı belirle
                if (satisTipi == "FATURA")
                {
                    // Fatura işlemi için stok hareketi oluştur
                    bool stokSonuc = StokHelper.SatisStokCikisiYap(sirketID, yeniSatis.SatisID, satisDetaylari, GetKullaniciID());
                    
                    if (!stokSonuc)
                    {
                        // Stok çıkışı yapılamadı ama satış kaydedildi - log ekle
                        System.Diagnostics.Debug.WriteLine("UYARI: Satış kaydedildi fakat stok çıkışı yapılamadı. SatisID: " + yeniSatis.SatisID);
                    }
                }
                // Not: Button text says "İrsaliye eksilir" but requirements now say it shouldn't
                /*
                else if (satisTipi == "IRSALIYE")
                {
                    // İrsaliye işlemi için stok hareketi oluştur
                    bool stokSonuc = StokHelper.SatisStokCikisiYap(sirketID, yeniSatis.SatisID, satisDetaylari, GetKullaniciID());
                    
                    if (!stokSonuc)
                    {
                        System.Diagnostics.Debug.WriteLine("UYARI: İrsaliye kaydedildi fakat stok çıkışı yapılamadı. SatisID: " + yeniSatis.SatisID);
                    }
                }
                */
                
                // Sepeti temizle
                Session["Sepet"] = new List<SepetItem>();
                
                // İşlem tipine göre başarılı mesajı göster
                string islemAdi = satisTipi == "FATURA" ? "Fatura" : 
                                 (satisTipi == "IRSALIYE" ? "İrsaliye" : "Proforma/Sipariş");
                
                // SweetAlert2 ile başarı mesajı ve yönlendirme
                string successScript = string.Format(@"
                    Swal.fire({{
                        icon: 'success',
                        title: 'Başarılı!',
                        text: '{0} başarıyla kaydedildi. Belge No: {1}',
                        confirmButtonText: 'Tamam',
                        confirmButtonColor: '#3085d6'
                    }}).then((result) => {{
                        if (result.isConfirmed) {{
                            window.location = 'MusteriDetay.aspx?id={2}';
                        }}
                    }});", islemAdi, yeniSatis.SatisID, musteriID);
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "kayitBasarili", successScript, true);
            }
        }
        catch (Exception ex)
        {
            // Hata mesajı göster - SweetAlert2 ile
            MessageHelper.ShowErrorMessage(this, "Hata", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            
            // Hatayı logla
            System.Diagnostics.Debug.WriteLine("Satış kaydetme hatası: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Hata detayı: " + ex.StackTrace);
        }
    }

    // Belge numarası üretme fonksiyonu
    private string GenerateBelgeNo(int sirketID)
    {
        try
        {
            using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
            {
                // Bugünkü tarih için en son satış belge numarasını bul
                string today = DateTime.Now.ToString("yyyyMMdd");
                string prefix = "S" + today;
                
                var sonSatis = db.Satislars
                    .Where(s => s.SirketID == sirketID && s.SatisBelgeNo.StartsWith(prefix))
                    .OrderByDescending(s => s.SatisBelgeNo)
                    .FirstOrDefault();
                
                int siraNo = 1;
                if (sonSatis != null)
                {
                    // Son belge numarasından sıra numarasını çıkar
                    string sonBelgeNo = sonSatis.SatisBelgeNo;
                    if (sonBelgeNo.Length > prefix.Length)
                    {
                        string sonSiraStr = sonBelgeNo.Substring(prefix.Length);
                        int sonSira;
                        if (int.TryParse(sonSiraStr, out sonSira))
                        {
                            siraNo = sonSira + 1;
                        }
                    }
                }
                
                // Yeni belge numarası: S20250115001 formatında
                return prefix + siraNo.ToString("000");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GenerateBelgeNo Hatası: " + ex.Message);
            // Hata durumunda rastgele numara üret
            return "S" + DateTime.Now.ToString("yyyyMMddHHmm");
        }
    }
    
    // SessionHelper için kullanıcı ID'sini alma yardımcı metodu
    private static int? GetKullaniciID()
    {
        if (HttpContext.Current.Session["KullaniciID"] != null)
        {
            return Convert.ToInt32(HttpContext.Current.Session["KullaniciID"]);
        }
        return null;
    }
}

// Ürün arama sonuçları için yardımcı sınıf
public class UrunArama
{
    public int UrunID { get; set; }
    public string UrunKodu { get; set; }
    public bool UrunTipiStoklu { get; set; }
    public string Barkod { get; set; }
    public string UrunAdi { get; set; }
    public decimal SatisFiyati { get; set; }
    public decimal StokMiktari { get; set; }
    public string BirimAdi { get; set; }
    public decimal KDVOrani { get; set; }
}

// Ürün detayları için yardımcı sınıf
public class UrunDetay
{
    public int UrunID { get; set; }
    public string UrunAdi { get; set; }
    public string UrunKodu { get; set; }
    public string Barkod { get; set; }
    public decimal BirimFiyat { get; set; }
    public decimal KDVOrani { get; set; }
    public string BirimAdi { get; set; }
    public decimal StokMiktari { get; set; }
}

// Sepet öğeleri için yardımcı sınıf
public class SepetItem
{
    public int UrunID { get; set; }
    public string UrunKodu { get; set; }
    public string UrunAdi { get; set; }
    public decimal Miktar { get; set; }
    public string Birim { get; set; }
    public decimal BirimFiyat { get; set; }
    public decimal Iskonto { get; set; }
    public string IskontoTuru { get; set; }
    public decimal KDV { get; set; }
    public string Aciklama { get; set; }
    public int DepoID { get; set; }
}