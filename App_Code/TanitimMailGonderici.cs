using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class TanitimMailGonderici
{
    // MSSQL bağlantı stringi (web.config veya sabit olarak alınabilir)
    private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;

    // Tanıtım şablonlarının yolu
    private readonly string sablonKlasoru = "/fabrika/Musteriler/MailSablonlari/";
    private readonly int toplamSablon = 14;

    // Gönderim limitleri
    private const int DakikaLimiti = 50;
    private const int SaatLimiti = 200;
    private const int GunLimiti = 500;

    // Çalışma saatleri (Türkiye saati)
    private readonly TimeSpan baslangicSaat = new TimeSpan(9, 30, 0);
    private readonly TimeSpan bitisSaat = new TimeSpan(15, 30, 0);

    public void GonderimYap()
    {
        bool tanitimMailiAktif = System.Configuration.ConfigurationManager.AppSettings["TanitimMailiAktif"] == "true";
        if (!tanitimMailiAktif)
            return;
        // 1. Saat kontrolü
        if (!SaatAraligindaMi())
            return;

        // 2. Limit kontrolü
        if (GunlukLimitAsildi() || SaatlikLimitAsildi() || DakikalikLimitAsildi())
            return;

        // 3. Gönderilecek mail listesini çek
        var mailler = GetBekleyenMailler();
        if (mailler == null || mailler.Rows.Count == 0)
            return;

        // 4. Bugünkü şablon dosyasını seç
        string sablonDosya = GetBugunkuSablonDosyasi();

        // 5. Her mail için gönderim
        foreach (DataRow row in mailler.Rows)
        {
            string to = row["Mail_to"].ToString();
            string userName = row["Email"].ToString(); // veya başka bir alan
            string subject = row["Mail_subject"].ToString();
            string body = GetSablonIcerik(sablonDosya, userName);

            // Email gönder
            EmailHelper.SendMail(to, subject, body);

            // Veritabanında güncelle
            MailGonderildiOlarakIsaretle(Convert.ToInt32(row["MailListeID"]));
        }
    }

    private bool SaatAraligindaMi()
    {
        var simdi = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Turkey Standard Time").TimeOfDay;
        return simdi >= baslangicSaat && simdi <= bitisSaat;
    }

    private bool GunlukLimitAsildi()
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(@"SELECT COUNT(*) FROM MailListeleri WHERE Gonderildimi = 1 AND CAST(GonderimTarihSaat AS DATE) = CAST(GETDATE() AS DATE)", conn))
        {
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count >= GunLimiti;
        }
    }

    private bool SaatlikLimitAsildi()
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(@"SELECT COUNT(*) FROM MailListeleri WHERE Gonderildimi = 1 AND DATEPART(YEAR, GonderimTarihSaat) = DATEPART(YEAR, GETDATE()) AND DATEPART(MONTH, GonderimTarihSaat) = DATEPART(MONTH, GETDATE()) AND DATEPART(DAY, GonderimTarihSaat) = DATEPART(DAY, GETDATE()) AND DATEPART(HOUR, GonderimTarihSaat) = DATEPART(HOUR, GETDATE())", conn))
        {
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count >= SaatLimiti;
        }
    }

    private bool DakikalikLimitAsildi()
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(@"SELECT COUNT(*) FROM MailListeleri WHERE Gonderildimi = 1 AND DATEDIFF(MINUTE, GonderimTarihSaat, GETDATE()) = 0", conn))
        {
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count >= DakikaLimiti;
        }
    }

    private DataTable GetBekleyenMailler()
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(@"SELECT TOP (@limit) * FROM MailListeleri WHERE Gonderildimi = 0 ORDER BY MailListeID", conn))
        {
            cmd.Parameters.AddWithValue("@limit", DakikaLimiti); // Her seferde en fazla 50 mail
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    private string GetBugunkuSablonDosyasi()
    {
        // 1 Ocak 2024'ten bugüne kaç gün geçtiyse ona göre şablon seç (1-14 arası döngü)
        int gun = (int)((DateTime.Now.Date - new DateTime(2024, 1, 1)).TotalDays) % toplamSablon + 1;
        return "tanitim_{gun}.html";
    }

    private string GetSablonIcerik(string dosyaAdi, string userName)
    {
        string path = System.Web.HttpContext.Current.Server.MapPath(sablonKlasoru + dosyaAdi);
        string html = System.IO.File.ReadAllText(path);
        return html.Replace("[Müşteri Adı]", userName);
    }

    private void MailGonderildiOlarakIsaretle(int mailListeID)
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(@"UPDATE MailListeleri SET Gonderildimi = 1, GonderimTarihSaat = GETDATE() WHERE MailListeID = @id", conn))
        {
            cmd.Parameters.AddWithValue("@id", mailListeID);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
} 