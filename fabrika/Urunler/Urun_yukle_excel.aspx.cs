using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text;
using System.Configuration;

public partial class fabrika_Urunler_Urun_yukle_excel : System.Web.UI.Page
{
    // Excel verilerini saklamak için ViewState property
    private DataTable ExcelData
    {
        get
        {
            if (ViewState["ExcelData"] != null)
                return (DataTable)ViewState["ExcelData"];
            return null;
        }
        set
        {
            ViewState["ExcelData"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // SirketID kontrolü - SessionHelper ile
                try
                {
                    // GetSirketID metodu içinde gerekli kontroller yapılacak
                    // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
                    int sirketID = SessionHelper.GetSirketID();
                    // SirketID geçerli, devam et
                    var master = this.Master as fabrika_FabrikaMasterPage;
                    if (master != null)
                    {
                        master.KlasorAdi = "Ürünler";
                        master.SayfaAdi = "Excel ile Toplu Ürün Ekleme";
                    }
                }
                catch
                {
                    // SessionHelper içinde yönlendirme yapılacak
                    return;
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                //pnlHata.Visible = true;
                //lblHata.Text = "Sayfa yüklenirken bir hata oluştu: " + ex.Message;

                MessageHelper.ShowErrorMessage(this, "Urun Excel Kayit", "Sayfa yüklenirken bir hata oluştu: " + ex.Message);
            }
        }
    }

    protected void btnOnizle_Click(object sender, EventArgs e)
    {
        try
        {
            // Hata ve başarı panellerini temizle
            //pnlHata.Visible = false;
            //pnlBasari.Visible = false;

            // Dosya kontrolü
            if (!fuExcel.HasFile)
            {
                //pnlHata.Visible = true;
                //lblHata.Text = "Lütfen bir Excel dosyası seçin.";

                MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme","Lütfen bir Excel dosyası seçin.");
                return;
            }

            // Dosya uzantısı kontrolü
            string fileExtension = Path.GetExtension(fuExcel.FileName).ToLower();
            if (fileExtension != ".xlsx")
            {
                //pnlHata.Visible = true;
                //lblHata.Text = "Lütfen sadece .xlsx formatında dosya yükleyin.";
                MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme", "Lütfen sadece .xlsx formatında dosya yükleyin.");
                return;
            }

            // Dosyayı geçici bir klasöre kaydet
            string fileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Server.MapPath("~/Temp/") + fileName;

            // Temp klasörünün varlığını kontrol et, yoksa oluştur
            string tempFolder = Server.MapPath("~/Temp/");
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            // Dosyayı kaydet
            fuExcel.SaveAs(filePath);

            // Excel dosyasını oku
            DataTable dt = null;
            try
            {
                dt = ExcelHelper.ReadExcelFile(filePath);
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                //pnlHata.Visible = true;
                //lblHata.Text = "Excel dosyası okunurken bir hata oluştu: " + ex.Message;
                MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme", "Excel dosyası okunurken bir hata oluştu: " + ex.Message);

                // Dosyayı sil (geçici dosya)
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return;
            }

            // Dosyayı sil (geçici dosya)
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Excel verilerini kontrol et
            if (dt == null)
            {
                //pnlHata.Visible = true;
                //lblHata.Text = "Excel dosyası okunamadı. Lütfen dosyanın formatını kontrol edin.";
                MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme", "Excel dosyası okunamadı. Lütfen dosyanın formatını kontrol edin.");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                //pnlHata.Visible = true;
                //lblHata.Text = "Excel dosyası boş. Lütfen verileri kontrol edin.";
                MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme", "Excel dosyası boş. Lütfen verileri kontrol edin.");
                return;
            }

            // Gerekli kolonların varlığını kontrol et
            string[] gerekliKolonlar = new string[] {
                "Ürün Adı","KDV Oranı","Ürün Tipi","Ürün Kodu","Barkodu","Markası","Kategorisi","Alış Fiyatı","Satış Fiyatı","KDV Dahil mi?","Para Birimi","Stok Miktarı","Birim"
            };

            foreach (string kolon in gerekliKolonlar)
            {
                if (!dt.Columns.Contains(kolon))
                {
                    //pnlHata.Visible = true;
                    //lblHata.Text = "Excel dosyasında '" + kolon + "' kolonu bulunamadı. Lütfen şablonu kontrol edin.";

                    MessageHelper.ShowErrorMessage(this, "Urun Excel Yükleme", "Excel dosyasında '" + kolon + "' kolonu bulunamadı. Lütfen şablonu kontrol edin.");
                    return;
                }
            }

            // Excel verilerini ViewState'e kaydet
            ExcelData = dt;

            // Önizleme için veri tablosunu oluştur
            DataTable previewTable = new DataTable();
            previewTable.Columns.Add("UrunAdi", typeof(string));
            previewTable.Columns.Add("KDVOrani", typeof(string));
            previewTable.Columns.Add("UrunTipiStoklu", typeof(string));
            previewTable.Columns.Add("UrunKodu", typeof(string));
            previewTable.Columns.Add("Barkod", typeof(string));
            previewTable.Columns.Add("Marka", typeof(string));
            previewTable.Columns.Add("Kategori", typeof(string));
            previewTable.Columns.Add("AlisFiyati", typeof(string));
            previewTable.Columns.Add("SatisFiyati", typeof(string));
            previewTable.Columns.Add("SatisFiyatiKdvDahilmi", typeof(string));
            previewTable.Columns.Add("ParaBirimi", typeof(string));
            previewTable.Columns.Add("StokMiktari", typeof(string));
            previewTable.Columns.Add("Birim", typeof(string));

            // Excel verilerini önizleme tablosuna doldur
            foreach (DataRow row in dt.Rows)
            {
                // Boş satırları atla
                if (string.IsNullOrEmpty(row["Ürün Adı"].ToString()))
                    continue;

                DataRow newRow = previewTable.NewRow();
                newRow["UrunAdi"] = row["Ürün Adı"].ToString();
                newRow["KDVOrani"] = row["KDV Oranı"].ToString();
                newRow["UrunTipiStoklu"] = row["Ürün Tipi"].ToString();
                newRow["UrunKodu"] = row["Ürün Kodu"].ToString();
                newRow["Barkod"] = row["Barkodu"].ToString();
                newRow["Marka"] = row["Markası"].ToString();
                newRow["Kategori"] = row["Kategorisi"].ToString();
                newRow["AlisFiyati"] = row["Alış Fiyatı"].ToString();
                newRow["SatisFiyati"] = row["Satış Fiyatı"].ToString();
                newRow["SatisFiyatiKdvDahilmi"] = row["KDV Dahil mi?"].ToString();
                newRow["ParaBirimi"] = row["Para Birimi"].ToString();
                newRow["StokMiktari"] = row["Stok Miktarı"].ToString();
                newRow["Birim"] = row["Birim"].ToString();
                previewTable.Rows.Add(newRow);
            }

            // GridView'a verileri bağla ve görünür yap
            gvOnizleme.DataSource = previewTable;
            gvOnizleme.DataBind();
            gvOnizleme.Visible = true;

            // Başarı mesajı
            MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", "Ürün Excel dosyası başarıyla yüklendi. Aşağıdaki verileri kontrol edip 'Onayla Kaydet' butonuna tıklayarak kaydedebilirsiniz.");

            //pnlBasari.Visible = true;
            //lblBasari.Text = "Ürün Excel dosyası başarıyla yüklendi. Aşağıdaki verileri kontrol edip 'Onayla Kaydet' butonuna tıklayarak kaydedebilirsiniz.";
        }
        catch (Exception ex)
        {
            // Hata mesajını göster
            //pnlHata.Visible = true;
            //lblHata.Text = "Ürün Excel dosyası işlenirken bir hata oluştu: " + ex.Message;

            MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", "Ürün Excel dosyası işlenirken bir hata oluştu: " + ex.Message);
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
            int _sirketID = SessionHelper.GetSirketID();
        try
        {

            // Excel verilerini kontrol et
            if (ExcelData == null || ExcelData.Rows.Count == 0)
            {
                // Eğer önizleme yapılmadıysa, önizleme işlemini gerçekleştir
                if (fuExcel.HasFile)
                {
                    btnOnizle_Click(sender, e);
                    return;
                }
                else
                {
                    //pnlHata.Visible = true;
                    //lblHata.Text = "Lütfen önce bir Excel dosyası seçin ve önizleyin.";
                    MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", "Lütfen önce bir Excel dosyası seçin ve önizleyin.");
                    return;
                }
            }

            // Verileri veritabanına kaydet
            int basariliKayit = 0;
            int hataliKayit = 0;
            string hataMesaji = "";

            // Veritabanı bağlantısı
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Kategori listesini al 
            var kategoriler = db.UrunKategorileris.Where(k => k.SirketID == _sirketID).ToList();

            // Marka listesini al
            var markalar = db.Markalars.Where(m => m.SirketID == _sirketID).ToList();

            // Birim listesini al
            var birimler = db.Birimlers.Where(b => b.SirketID == _sirketID).ToList();

            // Para birimi listesini al
            var paraBirimleri = new[] {
                new { ParaBirimiID = 1, ParaBirimiAdi = "TL" },
                new { ParaBirimiID = 2, ParaBirimiAdi = "USD" },
                new { ParaBirimiID = 3, ParaBirimiAdi = "EUR" }
            };

            // Excel verilerini işle
            foreach (DataRow row in ExcelData.Rows)
            {
                try
                {
                    // Boş satırları atla
                    if (string.IsNullOrEmpty(row["Ürün Adı"].ToString()))
                        continue;

                    // Ürün bilgilerini al
                    string urunAdi = row["Ürün Adı"].ToString();
                    string kdvOraniStr = row["KDV Oranı"].ToString();
                    string urunTipi = row["Ürün Tipi"].ToString();
                    string urunKodu = row["Ürün Kodu"].ToString();
                    string barkod = row["Barkodu"].ToString();
                    string markaAdi = row["Markası"].ToString();
                    string kategoriAdi = row["Kategorisi"].ToString();
                    string alisFiyatiStr = row["Alış Fiyatı"].ToString();
                    string satisFiyatiStr = row["Satış Fiyatı"].ToString();
                    string kdvDahilmiStr = row["KDV Dahil mi?"].ToString();
                    string paraBirimiAdi = row["Para Birimi"].ToString();
                    string stokMiktariStr = row["Stok Miktarı"].ToString();
                    string birimAdi = row["Birim"].ToString();

                    // Kategori ID'sini bul
                    int? kategoriID = null;
                    if (!string.IsNullOrEmpty(kategoriAdi))
                    {
                        var kategori = kategoriler.FirstOrDefault(k => k.Ad.Equals(kategoriAdi, StringComparison.OrdinalIgnoreCase));
                        if (kategori != null)
                        {
                            kategoriID = kategori.KategoriID;
                        }
                        else
                        {
                            UrunKategorileri yeniUrunKategorileri = new UrunKategorileri();
                            yeniUrunKategorileri.SirketID = _sirketID;
                            yeniUrunKategorileri.Ad = kategoriAdi;
                            db.UrunKategorileris.InsertOnSubmit(yeniUrunKategorileri);
                            db.SubmitChanges();
                            kategoriID = yeniUrunKategorileri.KategoriID;
                        }
                    }

                    // Marka ID'sini bul
                    int? markaID = null;
                    if (!string.IsNullOrEmpty(markaAdi))
                    {
                        var marka = markalar.FirstOrDefault(m => m.Ad.Equals(markaAdi, StringComparison.OrdinalIgnoreCase));
                        if (marka != null)
                        {
                            markaID = marka.MarkaID;
                        }
                        else
                        {
                            Markalar yeniMarka = new Markalar();
                            yeniMarka.Ad = markaAdi;
                            yeniMarka.SirketID = SessionHelper.GetSirketID();
                            db.Markalars.InsertOnSubmit(yeniMarka);
                            db.SubmitChanges();
                            markaID = yeniMarka.MarkaID;
                        }
                    }

                    // Birim ID'sini bul
                    int? birimID = null;
                    if (!string.IsNullOrEmpty(birimAdi))
                    {
                        var birim = birimler.FirstOrDefault(b => b.BirimAdi.Equals(birimAdi, StringComparison.OrdinalIgnoreCase));
                        if (birim != null)
                        {
                            birimID = birim.BirimID;
                        }
                        else
                        {
                            Birimler yeniBirimler = new Birimler();
                            yeniBirimler.SirketID = SessionHelper.GetSirketID();
                            yeniBirimler.BirimAdi = birimAdi;
                            db.Birimlers.InsertOnSubmit(yeniBirimler);
                            db.SubmitChanges();
                            birimID = yeniBirimler.BirimID;
                        }
                    }

                    // Para birimi ID'sini bul
                    int? paraBirimiID = null;
                    if (!string.IsNullOrEmpty(paraBirimiAdi))
                    {
                        var paraBirimi = paraBirimleri.FirstOrDefault(p => p.ParaBirimiAdi.Equals(paraBirimiAdi, StringComparison.OrdinalIgnoreCase));
                        if (paraBirimi != null)
                        {
                            paraBirimiID = paraBirimi.ParaBirimiID;
                        }
                        else
                        {
                            ParaBirimileri yeniParaBirimler = new ParaBirimileri(); 
                            yeniParaBirimler.ParaBirimiAd = paraBirimiAdi;

                            db.ParaBirimileris.InsertOnSubmit(yeniParaBirimler);
                            db.SubmitChanges();
                            paraBirimiID = yeniParaBirimler.ParaBirimiID;
                        }
                    }

                    // KDV Oranını parse et
                    decimal kdvOrani = 0;
                    if (!string.IsNullOrEmpty(kdvOraniStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        kdvOraniStr = kdvOraniStr.Replace(',', '.');
                        if (!decimal.TryParse(kdvOraniStr, NumberStyles.Any, CultureInfo.InvariantCulture, out kdvOrani))
                        {
                            kdvOrani = 0;
                        }
                    }

                    // Alış Fiyatını parse et
                    decimal alisFiyati = 0;
                    if (!string.IsNullOrEmpty(alisFiyatiStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        alisFiyatiStr = alisFiyatiStr.Replace(',', '.');
                        if (!decimal.TryParse(alisFiyatiStr, NumberStyles.Any, CultureInfo.InvariantCulture, out alisFiyati))
                        {
                            alisFiyati = 0;
                        }
                    }

                    // Satış Fiyatını parse et
                    decimal satisFiyati = 0;
                    if (!string.IsNullOrEmpty(satisFiyatiStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        satisFiyatiStr = satisFiyatiStr.Replace(',', '.');
                        if (!decimal.TryParse(satisFiyatiStr, NumberStyles.Any, CultureInfo.InvariantCulture, out satisFiyati))
                        {
                            satisFiyati = 0;
                        }
                    }

                    // Stok Miktarını parse et
                    decimal stokMiktari = 0;
                    if (!string.IsNullOrEmpty(stokMiktariStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        stokMiktariStr = stokMiktariStr.Replace(',', '.');
                        if (!decimal.TryParse(stokMiktariStr, NumberStyles.Any, CultureInfo.InvariantCulture, out stokMiktari))
                        {
                            stokMiktari = 0;
                        }
                    }

                    // KDV Dahil mi değerini parse et
                    bool kdvDahilmi = false;
                    if (!string.IsNullOrEmpty(kdvDahilmiStr))
                    {
                        kdvDahilmiStr = kdvDahilmiStr.Trim().ToLower();
                        kdvDahilmi = kdvDahilmiStr == "evet" || kdvDahilmiStr == "true" || kdvDahilmiStr == "1" || kdvDahilmiStr == "e";
                    }

                    // Ürün tipi stoklu mu değerini belirle
                    bool urunTipiStoklu = false;
                    if (!string.IsNullOrEmpty(urunTipi))
                    {
                        urunTipi = urunTipi.Trim().ToLower();
                        urunTipiStoklu = urunTipi == "stoklu" || urunTipi == "true" || urunTipi == "1" || urunTipi == "evet";
                    }

                    // Yeni Ürün oluştur
                    var yeniUrun = new Urunler()
                    {
                        SirketID = SessionHelper.GetSirketID(),
                        // Foreign key alanları için null kontrolü
                        MarkaID = markaID, // Null olarak bırakılabilir
                        UrunKodu = string.IsNullOrEmpty(urunKodu) ? null : urunKodu,
                        UrunTipiStoklu = urunTipiStoklu,
                        Barkod = barkod,
                        UrunAdi = urunAdi,
                        KategoriID = kategoriID, // Null olarak bırakılabilir
                        BirimID = birimID, // Null olarak bırakılabilir
                        StokMiktari = stokMiktari,
                        MinimumStok = 0, // Varsayılan değer
                        AlisFiyati = alisFiyati,
                        SatisFiyati = satisFiyati,
                        SatisFiyatiKdvDahilmi = kdvDahilmi,
                        ParaBirimiID = paraBirimiID, // Null olarak bırakılabilir
                        KDVOrani = kdvOrani,
                        Durum = true,
                        Notlar = "",
                        OlusturmaTarihi = DateTime.Now
                    };

                    // Veritabanına kaydet
                    db.Urunlers.InsertOnSubmit(yeniUrun);
                    db.SubmitChanges();

                    basariliKayit++;
                }
                catch (Exception ex)
                {
                    hataliKayit++;
                    hataMesaji += "Satır hatası: " + ex.Message + "<br/>";
                }
            }

            // Sonuç mesajı
            //pnlBasari.Visible = true;
            //lblBasari.Text = basariliKayit + " Ürün başarıyla kaydedildi. " + hataliKayit + " ürün kaydedilemedi.";
            MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", basariliKayit + " Ürün başarıyla kaydedildi. " + hataliKayit + " ürün kaydedilemedi.");

            if (!string.IsNullOrEmpty(hataMesaji))
            {
                //pnlHata.Visible = true;
                //lblHata.Text = hataMesaji;
                MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", hataMesaji);

            }

            // Excel verilerini temizle
            ExcelData = null;
            gvOnizleme.Visible = false;

            // 3 saniye bekleyip listeye yönlendir
            string script = "setTimeout(function() { window.location.href = 'UrunListesi.aspx'; }, 10000);";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
        }
        catch (Exception ex)
        {
            // Genel hata mesajı
            //pnlHata.Visible = true;
            //lblHata.Text = "İşlem sırasında bir hata oluştu: " + ex.Message;

            MessageHelper.ShowSuccessMessage(this, "Urun Excel Yükleme", "İşlem sırasında bir hata oluştu: " + ex.Message);
        }
    }
}