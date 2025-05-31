using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

public partial class fabrika_Mustahsil_Mustahsil_yukle_Cks_pdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string AlIlk(string text, string label, string sonrakiEtiket)
    {
        var pattern = Regex.Escape(label) + @"\s*(.*?)\s*" + Regex.Escape(sonrakiEtiket);
        var match = Regex.Match(text, pattern, RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value.Trim() : "";
    }

    private string AlSatir(string text, string label)
    {
        var pattern = Regex.Escape(label) + @"\s*(.*)";
        var match = Regex.Match(text, pattern);
        return match.Success ? match.Groups[1].Value.Trim() : "";
    }

    public class AraziSatir
    {
        public string SiraNo { get; set; }
        public string BelgeTuru { get; set; }
        public string AdaParsel { get; set; }
        public string TapuAlan { get; set; }
        public string TasarrufAlan { get; set; }
        public string KullanilanAlan { get; set; }
        public string Urun { get; set; }
        public string EkimDikimHasatTarihi { get; set; }
        public string Sulama { get; set; }
        public string MulkTipi { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Mahalle { get; set; }
        public string Durumu { get; set; }
    }

    protected void BtnUpload_Click(object sender, EventArgs e)
    {if (FileUpload1.HasFile)
        {
            string text = "";
            using (var reader = new PdfReader(FileUpload1.FileBytes))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(reader, i);
                }
            }

            // Başvuru Merkezi için tüm metinde arama yapan daha esnek regex
            var basvuruMatch = Regex.Match(text, @"Başvuru Merkezi\s*:\s*([^\n\r]*)");
            //txtbasvuruMerkezi.Text = basvuruMatch.Success ? basvuruMatch.Groups[1].Value.Trim() : "";

            // PDF metnini geçici olarak göster (debug için)
            txtAdSoyad.Text = text;
            //txtcksdilekceno.Text = AlIlk(text, "ÇKS Dilekçe No:", "ÇKS Kayıt Tarihi");
            //txtckskayitTarihi.Text = AlIlk(text, "ÇKS Kayıt Tarihi", "T.C. Kimlik No");
            txtTckimlikNo.Text = AlIlk(text, "T.C. Kimlik No :", "Adı Soyadı:");
            txtAdSoyad.Text = AlIlk(text, "Adı Soyadı:", "Doğum Tarihi :");
            txtdogumTarihi.Text = AlIlk(text, "Doğum Tarihi :", "Yerleşim Birimi :");
            string yerlesimbirimi = "";
            yerlesimbirimi = AlIlk(text, "Yerleşim Birimi :", "Baba Adı :");
            //txtYerlesimBirimi.Text = AlIlk(text, "Yerleşim Birimi :", "Baba Adı :");
            //txtbabaadi.Text = AlIlk(text, "Baba Adı :", "Ana Adı :");
            //txtanaadi.Text = AlIlk(text, "Ana Adı :", "Telefon :");
            //txtTelefonCep.Text = AlIlk(text, "Cep :", "Veriliş Sebebi :");
            //txtVerilisSebebi.Text = AlIlk(text, "Veriliş Sebebi :", "Ev");
            // txtTelefonEv.Text = AlIlk(text, "Ev :", "Cep :"); // Eğer Ev telefonu varsa açabilirsin

            // Doğum Tarihi boşsa, Yerleşim Birimi'nden tarih çek
            if (string.IsNullOrEmpty(txtdogumTarihi.Text))
            {
                var tarihMatch = Regex.Match(yerlesimbirimi, @"(\d{2}/\d{2}/\d{4})");
                txtdogumTarihi.Text = tarihMatch.Success ? tarihMatch.Groups[1].Value : "";
                if (tarihMatch.Success)
                    yerlesimbirimi = yerlesimbirimi.Replace(tarihMatch.Groups[1].Value, "").Trim();
            }

            // 1. "ÇKS' ye Kayıtlı Arazi Bilgileri" sonrası metni al
            var araziTabloMatch = Regex.Match(text, "ÇKS' ye Kayıtlı Arazi Bilgileri(.+?)(Ürünlerin Dağılımı|$)", RegexOptions.Singleline);
            string araziTablo = araziTabloMatch.Success ? araziTabloMatch.Groups[1].Value : "";

            // 1. Satırları birleştir
            var lines = araziTablo.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> groupedRows = new List<string>();
            string currentRow = "";
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (Regex.IsMatch(trimmed, @"^\d+\s+Tapu")) // Yeni satır başlıyor
                {
                    if (!string.IsNullOrEmpty(currentRow))
                        groupedRows.Add(currentRow.Trim());
                    currentRow = trimmed;
                }
                else
                {
                    // Devam satırı, birleştir
                    currentRow += " " + trimmed;
                }
            }
            if (!string.IsNullOrEmpty(currentRow))
                groupedRows.Add(currentRow.Trim());

            List<AraziSatir> araziList = new List<AraziSatir>();
            foreach (var row in groupedRows)
            {
                var parcalar = row.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // Alan sayısı 14'ten fazlaysa, son parçaları Mahalle ve Durumu olarak birleştir
                string mahalle = "";
                string durumu = "";
                if (parcalar.Length > 15)
                {
                    // Son iki parçayı birleştirerek Mahalle ve Durumu oluştur
                    mahalle = parcalar[parcalar.Length - 2];
                    durumu = parcalar[parcalar.Length - 1];
                }
                else if (parcalar.Length > 14)
                {
                    mahalle = parcalar[parcalar.Length - 1];
                }
                // Diğer alanları indexlere göre doldur
                var arazi = new AraziSatir
                {
                    SiraNo = parcalar.Length > 0 ? parcalar[0] : "",
                    BelgeTuru = parcalar.Length > 1 ? parcalar[1] : "",
                    AdaParsel = parcalar.Length > 4 ? parcalar[2] + " " + parcalar[3] + " " + parcalar[4] : "",
                    TapuAlan = parcalar.Length > 5 ? parcalar[5] : "",
                    TasarrufAlan = parcalar.Length > 6 ? parcalar[6] : "",
                    KullanilanAlan = parcalar.Length > 7 ? parcalar[7] : "",
                    Urun = parcalar.Length > 8 ? parcalar[8] : "",
                    EkimDikimHasatTarihi = parcalar.Length > 9 ? parcalar[9] : "",
                    Sulama = parcalar.Length > 10 ? parcalar[10] : "",
                    MulkTipi = parcalar.Length > 11 ? parcalar[11] : "",
                    Il = parcalar.Length > 12 ? parcalar[12] : "",
                    Ilce = parcalar.Length > 13 ? parcalar[13] : "",
                    Mahalle = mahalle,
                    Durumu = durumu
                };
                araziList.Add(arazi);
            }
            GridViewMustahsilTarlalar.DataSource = araziList;
            GridViewMustahsilTarlalar.DataBind();

            GridViewMustahsilTarlalar.Visible = true;
        }
    }
}