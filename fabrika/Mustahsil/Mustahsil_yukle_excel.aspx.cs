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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

public partial class fabrika_Mustahsil_Mustahsil_yukle_excel : System.Web.UI.Page
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
                        master.KlasorAdi = "Mustahsil";
                        master.SayfaAdi = "Excel ile Toplu Mustahsil Ekleme";
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
                MessageHelper.ShowErrorMessage(this, "Hata", "Sayfa yüklenirken bir hata oluştu: " + ex.Message);
            }
        }
    }

    protected void btnOnizle_Click(object sender, EventArgs e)
    {
        try
        {
            // Hata ve başarı panellerini temizle
            // MessageHelper.HideMessage(this);

            // Dosya kontrolü
            if (!fuExcel.HasFile)
            {
                MessageHelper.ShowErrorMessage(this, "Hata", "Lütfen bir Excel dosyası seçin.");
                return;
            }

            // Dosya uzantısı kontrolü
            string fileExtension = Path.GetExtension(fuExcel.FileName).ToLower();
            if (fileExtension != ".xlsx")
            {
                MessageHelper.ShowErrorMessage(this, "Hata", "Lütfen sadece .xlsx formatında dosya yükleyin.");
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
                MessageHelper.ShowErrorMessage(this, "Hata", "Excel dosyası okunurken bir hata oluştu: " + ex.Message);

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
                MessageHelper.ShowErrorMessage(this, "Hata", "Excel dosyası okunamadı. Lütfen dosyanın formatını kontrol edin.");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageHelper.ShowErrorMessage(this, "Hata", "Excel dosyası boş. Lütfen verileri kontrol edin.");
                return;
            }

            // Gerekli kolonların varlığını kontrol et
            string[] gerekliKolonlar = new string[] {
                "Ad", "Soyad","Telefon","E-Posta", "Adres","TC Kimlik No","Bakiyesi","Banka Bilgileri"
            };

            foreach (string kolon in gerekliKolonlar)
            {
                if (!dt.Columns.Contains(kolon))
                {
                    MessageHelper.ShowErrorMessage(this, "Hata", "Excel dosyasında '" + kolon + "' kolonu bulunamadı. Lütfen şablonu kontrol edin.");
                    return;
                }
            }

            // Excel verilerini ViewState'e kaydet
            ExcelData = dt;

            // Önizleme için veri tablosunu oluştur
            DataTable previewTable = new DataTable();
            previewTable.Columns.Add("Ad", typeof(string));
            previewTable.Columns.Add("Soyad", typeof(string));
            previewTable.Columns.Add("Telefon", typeof(string));
            previewTable.Columns.Add("Email", typeof(string));
            previewTable.Columns.Add("Adres", typeof(string));
            previewTable.Columns.Add("TCKimlikNo", typeof(string));
            previewTable.Columns.Add("Bakiyesi", typeof(string));
            previewTable.Columns.Add("BankaBilgileri", typeof(string));

            // Excel verilerini önizleme tablosuna doldur
            foreach (DataRow row in dt.Rows)
            {
                // Boş satırları atla
                if (string.IsNullOrEmpty(row["Ad"].ToString()))
                    continue;

                DataRow newRow = previewTable.NewRow();
                newRow["Ad"] = row["Ad"].ToString();
                newRow["Soyad"] = row["Soyad"].ToString();
                newRow["Telefon"] = row["Telefon"].ToString();
                newRow["Email"] = row["E-Posta"].ToString();
                newRow["Adres"] = row["Adres"].ToString();
                newRow["TCKimlikNo"] = row["TC Kimlik No"].ToString();
                newRow["Bakiyesi"] = row["Bakiyesi"].ToString();
                newRow["BankaBilgileri"] = row["Banka Bilgileri"].ToString();
                previewTable.Rows.Add(newRow);
            }

            // GridView'a verileri bağla ve görünür yap
            gvOnizleme.DataSource = previewTable;
            gvOnizleme.DataBind();
            gvOnizleme.Visible = true;

            // Başarı mesajı
            MessageHelper.ShowSuccessMessage(this, "Başarılı", "Excel dosyası başarıyla yüklendi. Aşağıdaki verileri kontrol edip 'Onayla Kaydet' butonuna tıklayarak kaydedebilirsiniz.");
        }
        catch (Exception ex)
        {
            // Hata mesajını göster
            MessageHelper.ShowErrorMessage(this, "Hata", "Excel dosyası işlenirken bir hata oluştu: " + ex.Message);
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
                    MessageHelper.ShowErrorMessage(this, "Hata", "Lütfen önce bir Excel dosyası seçin ve önizleyin.");
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

            // Excel verilerini işle
            foreach (DataRow row in ExcelData.Rows)
            {
                try
                {
                    // Boş satırları atla
                    if (string.IsNullOrEmpty(row["Ad"].ToString()))
                        continue;

                    // Mustahsil bilgilerini al
                    string Ad = row["Ad"].ToString();
                    string Soyad = row["Soyad"].ToString();
                    string Telefon = row["Telefon"].ToString();
                    string Email = row["E-Posta"].ToString();
                    string Adres = row["Adres"].ToString();
                    string TCKimlikNo = row["TC Kimlik No"].ToString();
                    string bakiyeStr = row["Bakiyesi"].ToString();
                    string BankaBilgileri = row["Banka Bilgileri"].ToString();
                    // Kategori ID'sini bul

                    // Para birimi ID'sini bul

                    // Bakiye değerini parse et
                    decimal _bakiye = 0;
                    if (!string.IsNullOrEmpty(bakiyeStr))
                    {
                        // Hem nokta hem virgül ile ayrılmış sayıları destekle
                        bakiyeStr = bakiyeStr.Replace(',', '.');
                        if (!decimal.TryParse(bakiyeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out _bakiye))
                        {
                            _bakiye = 0;
                        }
                    }

                    bool durumu = true;
                    // Yeni Mustahsil oluştur
                    var yeniMustahsil = new Mustahsiller()
                    {
                        SirketID = SessionHelper.GetSirketID(),
                        Ad = string.IsNullOrEmpty(Ad) ? null : Ad,
                        Soyad = Soyad,
                        Telefon = Telefon,
                        Email = Email,
                        Adres = Adres,
                        TCKimlikNo = TCKimlikNo,
                        Bakiyesi = _bakiye,
                        BankaBilgileri=BankaBilgileri,
                        Durum = durumu,
                        OlusturmaTarihi=DateTime.Now
                    };

                    // Veritabanına kaydet
                    db.Mustahsillers.InsertOnSubmit(yeniMustahsil);
                    db.SubmitChanges();

                    basariliKayit++;
                }
                catch (Exception ex){
                    hataliKayit++;
                    hataMesaji += "Satır hatası: " + ex.Message + "<br/>";
                }
            }

            // Sonuç mesajı
            MessageHelper.ShowSuccessMessage(this, "Başarılı", basariliKayit + " Mustahsil başarıyla kaydedildi. " + hataliKayit + " Mustahsil kaydedilemedi.");

            if (!string.IsNullOrEmpty(hataMesaji))
            {
                MessageHelper.ShowErrorMessage(this, "Hata", hataMesaji);
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
            MessageHelper.ShowErrorMessage(this, "Hata", "İşlem sırasında bir hata oluştu: " + ex.Message);
        }
    }
}