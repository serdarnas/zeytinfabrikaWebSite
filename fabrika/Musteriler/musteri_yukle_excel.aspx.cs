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

public partial class fabrika_Musteriler_musteri_yukle_excel : System.Web.UI.Page
{
    // Not: Şirket ID için artık SessionHelper.GetSirketID() kullanılıyor

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
                        master.KlasorAdi = "Müşteri";
                        master.SayfaAdi = "Excel ile Müşteri Toplu Müşteri Ekleme";
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
                pnlHata.Visible = true;
                lblHata.Text = "Sayfa yüklenirken bir hata oluştu: " + ex.Message;
            }
        }
    }

    protected void btnOnizle_Click(object sender, EventArgs e)
    {
        try
        {
            // Hata ve başarı panellerini temizle
            pnlHata.Visible = false;
            pnlBasari.Visible = false;
            
            // Dosya kontrolü
            if (!fuExcel.HasFile)
            {
                pnlHata.Visible = true;
                lblHata.Text = "Lütfen bir Excel dosyası seçin.";
                return;
            }

            // Dosya uzantısı kontrolü
            string fileExtension = Path.GetExtension(fuExcel.FileName).ToLower();
            if (fileExtension != ".xlsx")
            {
                pnlHata.Visible = true;
                lblHata.Text = "Lütfen sadece .xlsx formatında dosya yükleyin.";
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
                pnlHata.Visible = true;
                lblHata.Text = "Excel dosyası okunurken bir hata oluştu: " + ex.Message;
                
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
                pnlHata.Visible = true;
                lblHata.Text = "Excel dosyası okunamadı. Lütfen dosyanın formatını kontrol edin.";
                return;
            }
            
            if (dt.Rows.Count == 0)
            {
                pnlHata.Visible = true;
                lblHata.Text = "Excel dosyası boş. Lütfen verileri kontrol edin.";
                return;
            }

            // Gerekli kolonların varlığını kontrol et
            string[] gerekliKolonlar = new string[] { 
                "Müşteri Ünvanı", "Müşteri Kodu", "Adres", "Vergi Dairesi", 
                "Vergi No", "Kategori", "E-Posta", "Cep Telefonu", 
                "Yetkili Kişi Adı", "Bakiyesi", "Para Birimi" 
            };
            
            foreach (string kolon in gerekliKolonlar)
            {
                if (!dt.Columns.Contains(kolon))
                {
                    pnlHata.Visible = true;
                    lblHata.Text = "Excel dosyasında '" + kolon + "' kolonu bulunamadı. Lütfen şablonu kontrol edin.";
                    return;
                }
            }

            // Excel verilerini ViewState'e kaydet
            ExcelData = dt;

            // Önizleme için veri tablosunu oluştur
            DataTable previewTable = new DataTable();
            previewTable.Columns.Add("FirmaAdi", typeof(string));
            previewTable.Columns.Add("MusteriKodu", typeof(string));
            previewTable.Columns.Add("Adres", typeof(string));
            previewTable.Columns.Add("VergiDairesi", typeof(string));
            previewTable.Columns.Add("VergiNo", typeof(string));
            previewTable.Columns.Add("Kategori", typeof(string));
            previewTable.Columns.Add("Email", typeof(string));
            previewTable.Columns.Add("CepTelefonu", typeof(string));
            previewTable.Columns.Add("YetkiliAdi", typeof(string));
            previewTable.Columns.Add("Bakiyesi", typeof(string));
            previewTable.Columns.Add("ParaBirimi", typeof(string));

            // Excel verilerini önizleme tablosuna doldur
            foreach (DataRow row in dt.Rows)
            {
                // Boş satırları atla
                if (string.IsNullOrEmpty(row["Müşteri Ünvanı"].ToString()))
                    continue;

                DataRow newRow = previewTable.NewRow();
                newRow["FirmaAdi"] = row["Müşteri Ünvanı"].ToString();
                newRow["MusteriKodu"] = row["Müşteri Kodu"].ToString();
                newRow["Adres"] = row["Adres"].ToString();
                newRow["VergiDairesi"] = row["Vergi Dairesi"].ToString();
                newRow["VergiNo"] = row["Vergi No"].ToString();
                newRow["Kategori"] = row["Kategori"].ToString();
                newRow["Email"] = row["E-Posta"].ToString();
                newRow["CepTelefonu"] = row["Cep Telefonu"].ToString();
                newRow["YetkiliAdi"] = row["Yetkili Kişi Adı"].ToString();
                newRow["Bakiyesi"] = row["Bakiyesi"].ToString();
                newRow["ParaBirimi"] = row["Para Birimi"].ToString();
                previewTable.Rows.Add(newRow);
            }

            // GridView'a verileri bağla ve görünür yap
            gvOnizleme.DataSource = previewTable;
            gvOnizleme.DataBind();
            gvOnizleme.Visible = true;

            // Başarı mesajı
            pnlBasari.Visible = true;
            lblBasari.Text = "Excel dosyası başarıyla yüklendi. Aşağıdaki verileri kontrol edip 'Onayla Kaydet' butonuna tıklayarak kaydedebilirsiniz.";
        }
        catch (Exception ex)
        {
            // Hata mesajını göster
            pnlHata.Visible = true;
            lblHata.Text = "Excel dosyası işlenirken bir hata oluştu: " + ex.Message;
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
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
                    pnlHata.Visible = true;
                    lblHata.Text = "Lütfen önce bir Excel dosyası seçin ve önizleyin.";
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
            int sirketID = SessionHelper.GetSirketID();
            var kategoriler = db.MusteriKategorileris.Where(k => k.SirketID == sirketID).ToList();

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
                    if (string.IsNullOrEmpty(row["Müşteri Ünvanı"].ToString()))
                        continue;

                    // Müşteri bilgilerini al
                    string firmaAdi = row["Müşteri Ünvanı"].ToString();
                    string musteriKodu = row["Müşteri Kodu"].ToString();
                    string adres = row["Adres"].ToString();
                    string vergiDairesi = row["Vergi Dairesi"].ToString();
                    string vergiNo = row["Vergi No"].ToString();
                    string kategoriAdi = row["Kategori"].ToString();
                    string email = row["E-Posta"].ToString();
                    string cepTelefonu = row["Cep Telefonu"].ToString();
                    string yetkiliAdi = row["Yetkili Kişi Adı"].ToString();
                    string bakiyeStr = row["Bakiyesi"].ToString();
                    string paraBirimiAdi = row["Para Birimi"].ToString();

                    // Kategori ID'sini bul
                    int? kategoriID = null;
                    if (!string.IsNullOrEmpty(kategoriAdi))
                    {
                        var kategori = kategoriler.FirstOrDefault(k => k.KategoriAdi.Equals(kategoriAdi, StringComparison.OrdinalIgnoreCase));
                        if (kategori != null)
                        {
                            kategoriID = kategori.KategoriID;
                        }
                        else
                        {
                            MusteriKategorileri yeniKategoriler=new MusteriKategorileri();
                            yeniKategoriler.SirketID = SessionHelper.GetSirketID();
                            yeniKategoriler.KategoriAdi = kategoriAdi;
                            db.MusteriKategorileris.InsertOnSubmit(yeniKategoriler);
                            db.SubmitChanges();
                            kategoriID = yeniKategoriler.KategoriID;
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

                    // Bakiye değerini parse et
                    decimal bakiye = 0;
                    if (!string.IsNullOrEmpty(bakiyeStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        bakiyeStr = bakiyeStr.Replace(',', '.');
                        if (!decimal.TryParse(bakiyeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out bakiye))
                        {
                            bakiye = 0;
                        }
                    }

                    // Yeni müşteri oluştur
                    var yeniMusteri = new Musteriler()
                    {
                        SirketID = SessionHelper.GetSirketID(),
                        MüsteriKodu = string.IsNullOrEmpty(musteriKodu) ? null : musteriKodu,
                        FirmaAdi = firmaAdi,
                        YetkiliAdi = yetkiliAdi,
                        CepTelefonu = cepTelefonu,
                        Email = email,
                        Adres = adres,
                        VergiDairesi = vergiDairesi,
                        VergiNo = vergiNo,
                        KategoriID = kategoriID,
                        Durum = true,
                        OlusturmaTarihi = DateTime.Now,
                        Bakiyesi = bakiye,
                        ParaBirimiID = paraBirimiID
                    };

                    // Veritabanına kaydet
                    db.Musterilers.InsertOnSubmit(yeniMusteri);
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
            pnlBasari.Visible = true;
            lblBasari.Text = basariliKayit + " müşteri başarıyla kaydedildi. " + hataliKayit + " müşteri kaydedilemedi.";

            if (!string.IsNullOrEmpty(hataMesaji))
            {
                pnlHata.Visible = true;
                lblHata.Text = hataMesaji;
            }

            // Excel verilerini temizle
            ExcelData = null;
            gvOnizleme.Visible = false;

            // 3 saniye bekleyip listeye yönlendir
            string script = "setTimeout(function() { window.location.href = 'Default.aspx'; }, 10000);";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
        }
        catch (Exception ex)
        {
            // Genel hata mesajı
            pnlHata.Visible = true;
            lblHata.Text = "İşlem sırasında bir hata oluştu: " + ex.Message;
        }
    }
}