using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

public partial class fabrika_Urunler_YeniUrun : System.Web.UI.Page
{
    private const string ResimSessionKey = "YukluResimler";
    private int SirketID;

    protected void Page_Load(object sender, EventArgs e)
    {
        SirketID = SessionHelper.GetSirketID();
        int urunId = 0;
        int.TryParse(Request.QueryString["UrunID"], out urunId);
        
        if (!IsPostBack)
        {
            try
            {
                KategorileriYukle();
                BirimleriYukle();
                MarkalariYukle();
                ResimleriYukleVeGoster();

                // Eğer güncelleme ise, ürün bilgilerini doldur
                if (urunId > 0)
                {
                    // UserControl'e UrunID'yi geçir
                    VaryantControl1.SetUrunID(urunId);
                    
                    using (var db = new FabrikaDataClassesDataContext())
                    {
                        var urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId && u.SirketID == SirketID);
                        if (urun != null)
                        {
                            txtUrunAdi.Text = urun.UrunAdi ?? string.Empty;
                            ddlKategoriID.SelectedValue = urun.KategoriID.HasValue ? urun.KategoriID.ToString() : "0";
                            ddlBirimID.SelectedValue = urun.BirimID.HasValue ? urun.BirimID.ToString() : "0";
                            txtStokMiktari.Text = urun.StokMiktari.HasValue ? urun.StokMiktari.ToString() : "0";
                            txtMinimumStok.Text = urun.MinimumStok.HasValue ? urun.MinimumStok.ToString() : "0";
                            chkUrunTipiStoklu.Checked = urun.UrunTipiStoklu.HasValue ? urun.UrunTipiStoklu.Value : false;
                            chkDurum.Checked = urun.Durum.HasValue ? urun.Durum.Value : false;
                            txtAlisFiyati.Text = urun.AlisFiyati.HasValue ? urun.AlisFiyati.ToString() : "0";
                            txtAlisKdv.Text = urun.AlisKdv.HasValue ? urun.AlisKdv.ToString() : "0";
                            chkAlisFiyatiKdvDahilmi.Checked = urun.AlisFiyatiKdvDahilmi.HasValue ? urun.AlisFiyatiKdvDahilmi.Value : false;
                            txtSatisFiyati.Text = urun.SatisFiyati.HasValue ? urun.SatisFiyati.ToString() : "0";
                            txtSatisKdv.Text = urun.SatisKdv.HasValue ? urun.SatisKdv.ToString() : "0";
                            chkSatisFiyatiKdvDahilmi.Checked = urun.SatisFiyatiKdvDahilmi.HasValue ? urun.SatisFiyatiKdvDahilmi.Value : false;
                            ddlParaBirimiID.SelectedValue = urun.ParaBirimiID.HasValue ? urun.ParaBirimiID.ToString() : "1";
                            txtKDVOrani.Text = urun.KDVOrani.HasValue ? urun.KDVOrani.ToString() : "0";
                            chkPerakendeSatisVarmi.Checked = urun.PerakendeSatisVarmi.HasValue ? urun.PerakendeSatisVarmi.Value : false;
                            txtPerakendeSatisFiyati.Text = urun.PerakendeSatisFiyati.HasValue ? urun.PerakendeSatisFiyati.ToString() : "0";
                            chkPerakendeSatisKdvDahilmi.Checked = urun.PerakendeSatisKdvDahilmi.HasValue ? urun.PerakendeSatisKdvDahilmi.Value : false;
                            ddlMarkaID.SelectedValue = urun.MarkaID.HasValue ? urun.MarkaID.ToString() : "0";
                            txtUrunKodu.Text = urun.UrunKodu ?? string.Empty;
                            txtBarkod.Text = urun.Barkod ?? string.Empty;
                            txtNotlar.Text = urun.Notlar ?? string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowAndLogError(this, ex, "Sayfa yüklenirken hata oluştu.");
            }
        }
    }

    private void KategorileriYukle()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var kategoriler = from k in db.UrunKategorileris
                                  where k.SirketID == SirketID && k.Durumu == true
                                  select new { k.KategoriID, k.Ad };

                ddlKategoriID.DataSource = kategoriler.ToList();
                ddlKategoriID.DataTextField = "Ad";
                ddlKategoriID.DataValueField = "KategoriID";
                ddlKategoriID.DataBind();
                ddlKategoriID.Items.Insert(0, new ListItem("Seçiniz", "0"));
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Kategoriler yüklenirken hata oluştu.");
        }
    }

    private void BirimleriYukle()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var birimler = from b in db.Birimlers
                               where b.SirketID == SirketID
                               select new { b.BirimID, b.BirimAdi };

                var birimListesi = birimler.ToList();
                
                // Eğer hiç birim yoksa, default birimleri oluştur
                if (birimListesi.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Birim bulunamadı, default birimler oluşturuluyor...");
                    OlusturDefaultBirimler(SirketID, db);
                    
                    // Tekrar yükle
                    birimler = from b in db.Birimlers
                               where b.SirketID == SirketID
                               select new { b.BirimID, b.BirimAdi };
                    birimListesi = birimler.ToList();
                }

                ddlBirimID.DataSource = birimListesi;
                ddlBirimID.DataTextField = "BirimAdi";
                ddlBirimID.DataValueField = "BirimID";
                ddlBirimID.DataBind();
                ddlBirimID.Items.Insert(0, new ListItem("Seçiniz", "0"));
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Birimler yüklenirken hata oluştu.");
        }
    }

    private void MarkalariYukle()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var markalar = from m in db.Markalars
                               where m.SirketID == SirketID
                               select new { m.MarkaID, m.Ad };

                ddlMarkaID.DataSource = markalar.ToList();
                ddlMarkaID.DataTextField = "Ad";
                ddlMarkaID.DataValueField = "MarkaID";
                ddlMarkaID.DataBind();
                ddlMarkaID.Items.Insert(0, new ListItem("Seçiniz", "0"));
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Markalar yüklenirken hata oluştu.");
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            // Temel validasyonlar
            if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Ürün adı boş bırakılamaz.");
                return;
            }

            int urunId = 0;
            int.TryParse(Request.QueryString["UrunID"], out urunId);
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                Urunler urun = null;
                if (urunId > 0)
                {
                    // Güncelleme
                    urun = db.Urunlers.FirstOrDefault(u => u.UrunID == urunId && u.SirketID == SirketID);
                    if (urun == null)
                    {
                        MessageHelper.ShowErrorMessage(this, "Hata", "Güncellenecek ürün bulunamadı.");
                        return;
                    }
                }
                else
                {
                    // Yeni ürün - aynı isimde ürün var mı kontrol et
                    var mevcutUrun = db.Urunlers.FirstOrDefault(u => u.SirketID == SirketID && u.UrunAdi == txtUrunAdi.Text.Trim());
                    if (mevcutUrun != null)
                    {
                        MessageHelper.ShowWarningMessage(this, "Uyarı", "Bu isimde bir ürün zaten mevcut. Lütfen farklı bir ürün adı kullanın.");
                        return;
                    }
                    
                    // Yeni ürün
                    urun = new Urunler();
                    urun.SirketID = SirketID;
                    urun.OlusturmaTarihi = DateTime.Now;
                }

                // Temel bilgiler
                urun.UrunAdi = txtUrunAdi.Text ?? string.Empty;
                urun.KategoriID = ConvertToNullableInt(ddlKategoriID.SelectedValue);
                urun.BirimID = ConvertToNullableInt(ddlBirimID.SelectedValue);
                urun.StokMiktari = ConvertToNullableDecimal(txtStokMiktari.Text);
                urun.MinimumStok = ConvertToNullableDecimal(txtMinimumStok.Text);
                urun.UrunTipiStoklu = chkUrunTipiStoklu.Checked;
                urun.Durum = chkDurum.Checked;
                
                // Fiyat bilgileri
                urun.AlisFiyati = ConvertToNullableDecimal(txtAlisFiyati.Text);
                urun.AlisKdv = ConvertToNullableInt(txtAlisKdv.Text);
                urun.AlisParaBirimi = 1; // Varsayılan değer
                urun.AlisFiyatiKdvDahilmi = chkAlisFiyatiKdvDahilmi.Checked;
                urun.SatisFiyati = ConvertToNullableDecimal(txtSatisFiyati.Text);
                urun.SatisKdv = ConvertToNullableInt(txtSatisKdv.Text);
                urun.SatisParaBirimi = 1; // Varsayılan değer
                urun.SatisFiyatiKdvDahilmi = chkSatisFiyatiKdvDahilmi.Checked;
                urun.ParaBirimiID = ConvertToNullableInt(ddlParaBirimiID.SelectedValue);
                urun.KDVOrani = ConvertToNullableInt(txtKDVOrani.Text);
                
                // Detay bilgileri
                urun.MarkaID = ConvertToNullableInt(ddlMarkaID.SelectedValue);
                urun.UrunKodu = txtUrunKodu.Text ?? string.Empty;
                urun.Barkod = txtBarkod.Text ?? string.Empty;
                urun.Notlar = txtNotlar.Text ?? string.Empty;
                
                // Perakende bilgileri
                urun.PerakendeSatisVarmi = chkPerakendeSatisVarmi.Checked;
                urun.PerakendeSatisFiyati = ConvertToNullableDecimal(txtPerakendeSatisFiyati.Text);
                urun.PerakendeSatisKdvDahilmi = chkPerakendeSatisKdvDahilmi.Checked;

                if (urunId == 0)
                {
                    db.Urunlers.InsertOnSubmit(urun);
                }
                db.SubmitChanges();

                // Paketleme ve lojistik bilgilerini kaydet
                if (!string.IsNullOrWhiteSpace(txtUrunNetAgirlik_gr.Text))
                {
                    // Mevcut paketleme kaydını kontrol et
                    var mevcutPaketleme = db.UrunPaketlemeveLojistikBilgiIeris.FirstOrDefault(p => p.UrunID == urun.UrunID);
                    UrunPaketlemeveLojistikBilgiIeri paketleme = mevcutPaketleme ?? new UrunPaketlemeveLojistikBilgiIeri();
                    
                    paketleme.UrunID = urun.UrunID;
                    paketleme.SirketID = SirketID;
                    
                    // Byte yerine int/decimal kullan (değerler 255'ten büyük olabilir)
                    paketleme.UrunNetAgirlik_gr = ConvertToNullableInt(txtUrunNetAgirlik_gr.Text);
                    paketleme.UrunBurutAgirlik_gr = ConvertToNullableInt(txtUrunBurutAgirlik_gr.Text);
                    paketleme.Koli_İci_Urun_Adedi = ConvertToNullableInt(txtKoliIciAdet.Text);
                    paketleme.KoliBoyutlariEn_cm = ConvertToNullableInt(txtKoliUzunluk.Text);
                    paketleme.KoliBoyutlariBoy_cm = ConvertToNullableInt(txtKoliGenislik.Text);
                    paketleme.KoliBoyutlariYukseklik_cm = ConvertToNullableInt(txtKoliYukseklik.Text);
                    paketleme.KoliBrutAgirligi_kg = ConvertToNullableDecimal(txtKoliBrutAgirlik.Text);
                    paketleme.KoliNetAgirligi_kg = ConvertToNullableDecimal(txtKoliNetAgirlik.Text);
                    paketleme.KoliBarkodu = string.IsNullOrWhiteSpace(txtKoliBarkodu.Text) ? null : txtKoliBarkodu.Text;
                    paketleme.PaletTipID = ConvertToNullableInt(ddlPaletTipi.SelectedValue);
                    paketleme.PalettekiKoliAdet = ConvertToNullableInt(txtPalettekiKoliSayisi.Text);
                    
                    if (mevcutPaketleme == null)
                    {
                        db.UrunPaketlemeveLojistikBilgiIeris.InsertOnSubmit(paketleme);
                    }
                    db.SubmitChanges();
                }

                // Ürün ID'sini session'a kaydet (varyant işlemleri için)
                Session["YeniUrunID"] = urun.UrunID;
                
                // UserControl'e UrunID'yi geçir
                VaryantControl1.SetUrunID(urun.UrunID);
                
                string basariliMesaj = urunId > 0 ? "Ürün bilgileri başarıyla güncellendi." : "Yeni ürün başarıyla kaydedildi.";
                string detayMesaj = urunId > 0 ? "Ürün bilgileri veritabanında güncellendi." : "Ürün sisteme eklendi. Artık varyant ekleyebilir veya resim yükleyebilirsiniz.";
                
                MessageHelper.ShowSuccessMessage(this, basariliMesaj, detayMesaj);
                
                // Eğer yeni ürün ise ve varyant kullanılacaksa sayfayı yenile
                if (urunId == 0)
                {
                    Response.Redirect("YeniUrun.aspx?UrunID=" + urun.UrunID);
                }
                else
                {
                    // Güncelleme ise, tabları aktif et
                    string script = "aktifTablar();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "aktifTablar", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Ürün kaydedilirken beklenmeyen bir hata oluştu.");
        }
    }

    // Helper metodlar - C# 5.0 uyumlu
    private int? ConvertToNullableInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "0")
            return null;
        
        int result;
        if (int.TryParse(value, out result))
            return result;
        
        return null;
    }

    private decimal? ConvertToNullableDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        decimal result;
        if (decimal.TryParse(value, out result))
            return result;
        
        return null;
    }

    protected void btnKategoriKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtYeniKategoriAdi.Text))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Kategori adı boş bırakılamaz.");
                return;
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                // Aynı isimde kategori var mı kontrol et
                var mevcutKategori = db.UrunKategorileris.FirstOrDefault(k => 
                    k.SirketID == SirketID && 
                    k.Ad.ToLower().Trim() == txtYeniKategoriAdi.Text.ToLower().Trim());

                if (mevcutKategori != null)
                {
                    MessageHelper.ShowWarningMessage(this, "Uyarı", "Bu isimde bir kategori zaten mevcut.");
                    return;
                }

                // Yeni kategori ekle
                var yeniKategori = new UrunKategorileri
                {
                    SirketID = SirketID,
                    Ad = txtYeniKategoriAdi.Text.Trim(),
                    Durumu = true
                };

                db.UrunKategorileris.InsertOnSubmit(yeniKategori);
                db.SubmitChanges();

                // Kategori listesini yenile
                KategorileriYukle();
                
                // Yeni kategoriyi seç
                ddlKategoriID.SelectedValue = yeniKategori.KategoriID.ToString();
                
                // Formu temizle
                txtYeniKategoriAdi.Text = string.Empty;
                
                MessageHelper.ShowSuccessMessage(this, "Başarılı", "Kategori başarıyla eklendi.");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Kategori eklenirken hata oluştu.");
        }
    }

    protected void btnMarkaKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtYeniMarkaAdi.Text))
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Marka adı boş bırakılamaz.");
                return;
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                // Aynı isimde marka var mı kontrol et
                var mevcutMarka = db.Markalars.FirstOrDefault(m => 
                    m.SirketID == SirketID && 
                    m.Ad.ToLower().Trim() == txtYeniMarkaAdi.Text.ToLower().Trim());

                if (mevcutMarka != null)
                {
                    MessageHelper.ShowWarningMessage(this, "Uyarı", "Bu isimde bir marka zaten mevcut.");
                    return;
                }

                // Yeni marka ekle
                var yeniMarka = new Markalar
                {
                    SirketID = SirketID,
                    Ad = txtYeniMarkaAdi.Text.Trim(),   
                };

                db.Markalars.InsertOnSubmit(yeniMarka);
                db.SubmitChanges();

                // Marka listesini yenile
                MarkalariYukle();
                
                // Yeni markayı seç
                ddlMarkaID.SelectedValue = yeniMarka.MarkaID.ToString();
                
                // Formu temizle
                txtYeniMarkaAdi.Text = string.Empty; 
                
                MessageHelper.ShowSuccessMessage(this, "Başarılı", "Marka başarıyla eklendi.");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Marka eklenirken hata oluştu.");
        }
    }

    protected void btnResimYukle_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuResimler.HasFiles)
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Lütfen yüklenecek resim dosyalarını seçin.");
                return;
            }

            string ResimKlasoru = "/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/UrunResimleri/";
            List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
            int yuklenecekDosyaSayisi = fuResimler.PostedFiles.Count;
            int basariliYuklenen = 0;
            
            foreach (HttpPostedFile uploadedFile in fuResimler.PostedFiles)
            {
                try
                {
                    // Dosya türü kontrolü
                    string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png" && fileExtension != ".gif")
                    {
                        continue; // Desteklenmeyen dosya türü
                    }

                    // Dosya boyutu kontrolü (5MB)
                    if (uploadedFile.ContentLength > 5 * 1024 * 1024)
                    {
                        continue; // Çok büyük dosya
                    }

                    string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                    string sunucuYolu = Server.MapPath(ResimKlasoru + dosyaAdi);
                    Directory.CreateDirectory(Server.MapPath(ResimKlasoru));
                    uploadedFile.SaveAs(sunucuYolu);
                    resimYollari.Add(ResimKlasoru + dosyaAdi);
                    basariliYuklenen++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Resim yükleme hatası: " + ex.Message);
                    // Tek dosya hatası diğer dosyaları etkilemesin
                }
            }
            
            Session[ResimSessionKey] = resimYollari;
            ResimleriYukleVeGoster();
            
            if (basariliYuklenen > 0)
            {
                MessageHelper.ShowSuccessMessage(this, "Başarılı", basariliYuklenen + " adet resim başarıyla yüklendi.");
            }
            else
            {
                MessageHelper.ShowWarningMessage(this, "Uyarı", "Hiçbir resim yüklenemedi. Lütfen geçerli resim dosyaları (JPG, PNG, GIF) seçin ve dosya boyutunun 5MB'dan küçük olduğundan emin olun.");
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowAndLogError(this, ex, "Resim yükleme işlemi sırasında hata oluştu.");
        }
    }
    private void ResimleriYukleVeGoster()
    {
        List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
        rptResimler.DataSource = resimYollari;
        rptResimler.DataBind();
    }

    // Varyant WebMethod'ları
    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static List<VaryantTurDTO> GetVaryantTurleri()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("GetVaryantTurleri çağrıldı");
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                System.Diagnostics.Debug.WriteLine("SirketID: " + sirketID);
                
                var varyantTurleri = from vt in db.VaryantTurleris
                                     where vt.SirketID == sirketID
                                     select new VaryantTurDTO
                                     {
                                         VaryantTurID = vt.VaryantTurID,
                                         TurAdi = vt.TurAdi ?? string.Empty
                                     };
                
                var result = varyantTurleri.ToList();
                System.Diagnostics.Debug.WriteLine("Bulunan varyant türü sayısı: " + result.Count);
                
                // Eğer hiç varyant türü yoksa, default türleri oluştur
                if (result.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Varyant türü bulunamadı, default türler oluşturuluyor...");
                    result = OlusturDefaultVaryantTurleri(sirketID, db);
                }
                
                return result;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GetVaryantTurleri HATA: " + ex.Message);
            return new List<VaryantTurDTO>();
        }
    }

    // Default varyant türlerini oluştur
    private static List<VaryantTurDTO> OlusturDefaultVaryantTurleri(int sirketID, FabrikaDataClassesDataContext db)
    {
        try
        {
            var defaultTurler = new List<string> { "Boyut","Kalibre",  "Malzeme", "Ağırlık", "Hacim" };
            var result = new List<VaryantTurDTO>();
            
            foreach (var turAdi in defaultTurler)
            {
                // Varyant türünü oluştur
                var yeniTur = new VaryantTurleri
                {
                    SirketID = sirketID,
                    TurAdi = turAdi
                };
                db.VaryantTurleris.InsertOnSubmit(yeniTur);
                db.SubmitChanges(); // ID'yi almak için
                
                result.Add(new VaryantTurDTO
                {
                    VaryantTurID = yeniTur.VaryantTurID,
                    TurAdi = yeniTur.TurAdi
                });
                
                // Her tür için default değerleri oluştur
                OlusturDefaultVaryantDegerleri(sirketID, yeniTur.VaryantTurID, turAdi, db);
            }
            
            System.Diagnostics.Debug.WriteLine("Default varyant türleri oluşturuldu: " + result.Count + " adet");
            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Default varyant türleri oluşturma hatası: " + ex.Message);
            return new List<VaryantTurDTO>();
        }
    }

    // Default varyant değerlerini oluştur
    private static void OlusturDefaultVaryantDegerleri(int sirketID, int varyantTurID, string turAdi, FabrikaDataClassesDataContext db)
    {
        try
        {
            List<string> defaultDegerler = new List<string>();
            
            switch (turAdi.ToLower())
            {
                case "boyut":
                    defaultDegerler = new List<string> { "Duble","1 Numara", "2 Numara", "3 Numara", "4 Numara", "Sira Zeytin", "Yaglik" };
                    break;
                case "kalibre":
                    defaultDegerler = new List<string> { "80-100 Adet 1 kg 8XL","100-110 Adet 1 kg 7XL","110-120 Adet 1 kg 6XL","121-140 Adet 1 kg 5XL","141-160 Adet 1 kg 4XL","161-180 Adet 1 kg 3XL","181-200 Adet 1 kg 2XL","201-230 Adet 1 kg XL","231-260 Adet 1 kg L","261-290 Adet 1 kg M","291-320 Adet 1 kg S","321-350 Adet 1 kg XS","350-380 Adet 1 kg 2XS","351-400 Adet 1 kg 3XS","400 - 450 Adet 1 kg 4XS" };
                    break;
                case "malzeme":
                    defaultDegerler = new List<string> { "Cam", "Plastik", "Metal", };
                    break;
                case "ağırlık":
                    defaultDegerler = new List<string> { "100g", "250g", "500g", "1kg", "2kg", "5kg", };
                    break;
                case "hacim":
                    defaultDegerler = new List<string> { "100ml", "250ml", "500ml", "750ml", "1L", "2L","3L", "5L" };
                    break;
                default:
                    defaultDegerler = new List<string> { "Seçenek 1", "Seçenek 2", "Seçenek 3" };
                    break;
            }
            
            foreach (var degerAdi in defaultDegerler)
            {
                var yeniDeger = new VaryantDegerleri
                {
                    SirketID = sirketID,
                    VaryantTurID = varyantTurID,
                    DegerAdi = degerAdi
                };
                db.VaryantDegerleris.InsertOnSubmit(yeniDeger);
            }
            
            db.SubmitChanges();
            System.Diagnostics.Debug.WriteLine(turAdi + " için " + defaultDegerler.Count + " adet default değer oluşturuldu");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Default varyant değerleri oluşturma hatası: " + ex.Message);
        }
    }

    // Default birimler oluştur
    private static void OlusturDefaultBirimler(int sirketID, FabrikaDataClassesDataContext db)
    {
        try
        {
            var defaultBirimler = new List<dynamic>
            {
                new { BirimAdi = "Kilogram", BirimKodu = "KG" },
                new { BirimAdi = "Litre", BirimKodu = "LT" },
                new { BirimAdi = "Adet", BirimKodu = "AD" },
                new { BirimAdi = "Ton", BirimKodu = "TON" },
                new { BirimAdi = "Gram", BirimKodu = "GR" },
                new { BirimAdi = "Mililitre", BirimKodu = "ML" }
            };
            
            foreach (var birimData in defaultBirimler)
            {
                var yeniBirim = new Birimler
                {
                    SirketID = sirketID,
                    BirimAdi = birimData.BirimAdi,
                    BirimKodu = birimData.BirimKodu
                };
                db.Birimlers.InsertOnSubmit(yeniBirim);
            }
            
            db.SubmitChanges();
            System.Diagnostics.Debug.WriteLine("Default birimler oluşturuldu: " + defaultBirimler.Count + " adet");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Default birimler oluşturma hatası: " + ex.Message);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static List<VaryantDegerDTO> GetVaryantDegerleri(int varyantTurID)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("GetVaryantDegerleri çağrıldı - VaryantTurID: " + varyantTurID);
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                System.Diagnostics.Debug.WriteLine("SirketID: " + sirketID);
                
                // Önce varyant türünün mevcut olup olmadığını kontrol et
                var varyantTur = db.VaryantTurleris.FirstOrDefault(vt => vt.VaryantTurID == varyantTurID && vt.SirketID == sirketID);
                if (varyantTur == null)
                {
                    System.Diagnostics.Debug.WriteLine("Varyant türü bulunamadı!");
                    return new List<VaryantDegerDTO>();
                }
                
                System.Diagnostics.Debug.WriteLine("Varyant türü bulundu: " + varyantTur.TurAdi);
                
                var varyantDegerleri = from v in db.VaryantDegerleris
                                       where v.SirketID == sirketID &&
                                             v.VaryantTurID == varyantTurID
                                       select new VaryantDegerDTO
                                       {
                                           VaryantDegerID = v.VaryantDegerID,
                                           DegerAdi = v.DegerAdi ?? string.Empty
                                       };
                
                var result = varyantDegerleri.ToList();
                System.Diagnostics.Debug.WriteLine("Bulunan varyant değeri sayısı: " + result.Count);
                
                foreach (var item in result)
                {
                    System.Diagnostics.Debug.WriteLine("- " + item.VaryantDegerID + ": " + item.DegerAdi);
                }
                
                return result;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GetVaryantDegerleri HATA: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            
            // Hata durumunda boş liste döndür, exception fırlatma
            return new List<VaryantDegerDTO>();
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static VaryantKaydetSonucDTO KaydetVaryantlar(List<VaryantKaydetDTO> varyantlar)
    {
        var sonuc = new VaryantKaydetSonucDTO();
        
        try
        {
            System.Diagnostics.Debug.WriteLine("KaydetVaryantlar çağrıldı - Varyant sayısı: " + varyantlar.Count);
            
            int sirketID = SessionHelper.GetSirketID();
            int urunID = 0;
            
            // Ürün ID'sini ViewState'den al
            if (HttpContext.Current.Session["YeniUrunID"] != null)
            {
                int.TryParse(HttpContext.Current.Session["YeniUrunID"].ToString(), out urunID);
            }
            
            if (urunID == 0)
            {
                sonuc.success = false;
                sonuc.message = "Ürün ID'si bulunamadı. Önce ürünü kaydedin.";
                return sonuc;
            }
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Önce mevcut varyantları sil
                var mevcutVaryantlar = db.UrunVaryantlaris.Where(uv => uv.UrunID == urunID && uv.SirketID == sirketID);
                foreach (var mevcut in mevcutVaryantlar)
                {
                    // Önce detayları sil
                    var detaylar = db.UrunVaryantDetays.Where(uvd => uvd.UrunVaryantID == mevcut.UrunVaryantID);
                    db.UrunVaryantDetays.DeleteAllOnSubmit(detaylar);
                }
                db.UrunVaryantlaris.DeleteAllOnSubmit(mevcutVaryantlar);
                
                // Yeni varyantları ekle
                foreach (var varyant in varyantlar)
                {
                    var urunVaryant = new UrunVaryantlari
                    {
                        SirketID = sirketID,
                        UrunID = urunID,
                        Barkod = varyant.Barkod,
                        StokMiktari = varyant.StokMiktari,
                        AlisFiyati = varyant.AlisFiyati,
                        SatisFiyati = varyant.SatisFiyati,
                        PerakendeSatisFiyati = varyant.PerakendeSatisFiyati,
                        Durum = varyant.Durum
                    };
                    
                    db.UrunVaryantlaris.InsertOnSubmit(urunVaryant);
                    db.SubmitChanges(); // ID'yi almak için
                    
                    // Varyant detaylarını ekle
                    foreach (var degerID in varyant.VaryantDegerleri)
                    {
                        var detay = new UrunVaryantDetay
                        {
                            UrunVaryantID = urunVaryant.UrunVaryantID,
                            VaryantDegerID = degerID
                        };
                        db.UrunVaryantDetays.InsertOnSubmit(detay);
                    }
                }
                
                db.SubmitChanges();
                
                sonuc.success = true;
                sonuc.message = varyantlar.Count + " adet varyant başarıyla kaydedildi.";
                System.Diagnostics.Debug.WriteLine("Varyantlar başarıyla kaydedildi: " + varyantlar.Count + " adet");
            }
        }
        catch (Exception ex)
        {
            sonuc.success = false;
            sonuc.message = "Varyant kaydetme hatası: " + ex.Message;
            System.Diagnostics.Debug.WriteLine("KaydetVaryantlar HATA: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
        }
        
        return sonuc;
    }

    // DTO Sınıfları
    public class VaryantTurDTO
    {
        public int VaryantTurID { get; set; }
        public string TurAdi { get; set; }
    }

    public class VaryantDegerDTO
    {
        public int VaryantDegerID { get; set; }
        public string DegerAdi { get; set; }
    }

    public class VaryantKaydetDTO
    {
        public List<int> VaryantDegerleri { get; set; }
        public string Barkod { get; set; }
        public int StokMiktari { get; set; }
        public decimal AlisFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public decimal PerakendeSatisFiyati { get; set; }
        public bool Durum { get; set; }
    }

    public class VaryantKaydetSonucDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}