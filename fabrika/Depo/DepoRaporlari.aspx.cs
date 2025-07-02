using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;

public partial class fabrika_Depo_DepoRaporlari : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TarihleriniAyarla();
            DepolariYukle();
            IstatistikleriYukle();
            RaporOlustur();
        }
    }

    private void TarihleriniAyarla()
    {
        txtBitisTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtBaslangicTarihi.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
    }

    private void DepolariYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var depolar = db.Depolars
                    .Where(d => d.SirketID == sirketID && d.Durum == true)
                    .OrderBy(d => d.DepoAdi)
                    .ToList();

                ddlDepo.Items.Clear();
                ddlDepo.Items.Add(new System.Web.UI.WebControls.ListItem("Tüm Depolar", ""));
                
                foreach (var depo in depolar)
                {
                    ddlDepo.Items.Add(new System.Web.UI.WebControls.ListItem(depo.DepoAdi, depo.DepoID.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            // Log hatası
        }
    }

    private void IstatistikleriYukle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Toplam depo sayısı
                var toplamDepo = db.Depolars.Count(d => d.SirketID == sirketID && d.Durum == true);
                lblToplamDepo.Text = toplamDepo.ToString();

                // Toplam ürün çeşidi
                var toplamUrun = db.DepoStoks
                    .Where(ds => ds.SirketID == sirketID && ds.Miktar > 0)
                    .Select(ds => ds.UrunID)
                    .Distinct()
                    .Count();
                lblToplamUrun.Text = toplamUrun.ToString();

                // Toplam stok değeri
                var toplamDeger = (from ds in db.DepoStoks
                                  join u in db.Urunlers on ds.UrunID equals u.UrunID
                                  where ds.SirketID == sirketID && ds.Miktar > 0
                                  select ds.Miktar * (u.SatisFiyati ?? 0m)).Sum();
                lblToplamStokDegeri.Text = toplamDeger.ToString("N0");

                // Minimum stok altı ürün sayısı
                var minimumStokAlti = db.DepoStoks
                    .Where(ds => ds.SirketID == sirketID && 
                                ds.MinimumMiktar.HasValue && 
                                ds.Miktar < ds.MinimumMiktar.Value)
                    .Count();
                lblMinimumStokAlti.Text = minimumStokAlti.ToString();
            }
        }
        catch (Exception ex)
        {
            // Log hatası
        }
    }

    protected void ddlDepo_SelectedIndexChanged(object sender, EventArgs e)
    {
        RaporOlustur();
    }

    protected void ddlRaporTuru_SelectedIndexChanged(object sender, EventArgs e)
    {
        RaporOlustur();
    }

    protected void btnRaporOlustur_Click(object sender, EventArgs e)
    {
        RaporOlustur();
    }

    private void RaporOlustur()
    {
        string raporTuru = ddlRaporTuru.SelectedValue;
        
        // Tüm panelleri gizle
        pnlStokRaporu.Visible = false;
        pnlHareketRaporu.Visible = false;
        pnlMinimumStokRaporu.Visible = false;
        pnlVeriYok.Visible = false;

        switch (raporTuru)
        {
            case "stok":
                lblRaporBaslik.Text = "Stok Durumu Raporu";
                StokRaporuOlustur();
                break;
            case "hareket":
                lblRaporBaslik.Text = "Stok Hareketleri Raporu";
                HareketRaporuOlustur();
                break;
            case "minimum":
                lblRaporBaslik.Text = "Minimum Stok Raporu";
                MinimumStokRaporuOlustur();
                break;
            case "deger":
                lblRaporBaslik.Text = "Stok Değer Raporu";
                StokRaporuOlustur(); // Aynı rapor, farklı görünüm
                break;
            case "transfer":
                lblRaporBaslik.Text = "Transfer Raporu";
                TransferRaporuOlustur();
                break;
        }
    }

    private void StokRaporuOlustur()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var query = from ds in db.DepoStoks
                           join d in db.Depolars on ds.DepoID equals d.DepoID
                           join u in db.Urunlers on ds.UrunID equals u.UrunID
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           where ds.SirketID == sirketID
                           select new
                           {
                               ds.DepoID,
                               DepoAdi = d.DepoAdi,
                               UrunAdi = u.UrunAdi,
                               ds.Miktar,
                               BirimAdi = b != null ? b.BirimAdi : "",
                               ds.MinimumMiktar,
                               BirimFiyat = u.SatisFiyati ?? 0m,
                               ToplamDeger = ds.Miktar * (u.SatisFiyati ?? 0m),
                               ds.SonGuncellemeTarihi
                           };

                // Depo filtresi
                if (!string.IsNullOrEmpty(ddlDepo.SelectedValue))
                {
                    int depoID = Convert.ToInt32(ddlDepo.SelectedValue);
                    query = query.Where(x => x.DepoID == depoID);
                }

                var sonuc = query.OrderBy(x => x.DepoAdi).ThenBy(x => x.UrunAdi).ToList();

                if (sonuc.Count > 0)
                {
                    rptStokRaporu.DataSource = sonuc;
                    rptStokRaporu.DataBind();
                    pnlStokRaporu.Visible = true;
                }
                else
                {
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlVeriYok.Visible = true;
        }
    }

    private void HareketRaporuOlustur()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            DateTime baslangic = Convert.ToDateTime(txtBaslangicTarihi.Text);
            DateTime bitis = Convert.ToDateTime(txtBitisTarihi.Text).AddDays(1);

            using (var db = new FabrikaDataClassesDataContext())
            {
                var query = from sh in db.StokHareketleris
                           join d in db.Depolars on sh.DepoID equals d.DepoID
                           join u in db.Urunlers on sh.UrunID equals u.UrunID
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           join k in db.Kullanicilars on sh.KullaniciID equals k.KullaniciID into kullaniciGroup
                           from k in kullaniciGroup.DefaultIfEmpty()
                           where sh.SirketID == sirketID && 
                                 sh.IslemTarihi >= baslangic && 
                                 sh.IslemTarihi < bitis
                           select new
                           {
                               sh.DepoID,
                               sh.IslemTarihi,
                               DepoAdi = d.DepoAdi,
                               UrunAdi = u.UrunAdi,
                               sh.HareketTipi,
                               sh.Miktar,
                               BirimAdi = b != null ? b.BirimAdi : "",
                               sh.Aciklama,
                               KullaniciAdi = k != null ? k.AdSoyad : "Sistem"
                           };

                // Depo filtresi
                if (!string.IsNullOrEmpty(ddlDepo.SelectedValue))
                {
                    int depoID = Convert.ToInt32(ddlDepo.SelectedValue);
                    query = query.Where(x => x.DepoID == depoID);
                }

                var sonuc = query.OrderByDescending(x => x.IslemTarihi).ToList();

                if (sonuc.Count > 0)
                {
                    rptHareketRaporu.DataSource = sonuc;
                    rptHareketRaporu.DataBind();
                    pnlHareketRaporu.Visible = true;
                }
                else
                {
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlVeriYok.Visible = true;
        }
    }

    private void MinimumStokRaporuOlustur()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            using (var db = new FabrikaDataClassesDataContext())
            {
                var query = from ds in db.DepoStoks
                           join d in db.Depolars on ds.DepoID equals d.DepoID
                           join u in db.Urunlers on ds.UrunID equals u.UrunID
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           where ds.SirketID == sirketID && 
                                 ds.MinimumMiktar.HasValue && 
                                 ds.Miktar < ds.MinimumMiktar.Value
                           select new
                           {
                               ds.DepoID,
                               DepoAdi = d.DepoAdi,
                               UrunAdi = u.UrunAdi,
                               ds.Miktar,
                               ds.MinimumMiktar,
                               EksikMiktar = ds.MinimumMiktar.Value - ds.Miktar,
                               BirimAdi = b != null ? b.BirimAdi : ""
                           };

                // Depo filtresi
                if (!string.IsNullOrEmpty(ddlDepo.SelectedValue))
                {
                    int depoID = Convert.ToInt32(ddlDepo.SelectedValue);
                    query = query.Where(x => x.DepoID == depoID);
                }

                var sonuc = query.OrderBy(x => x.DepoAdi).ThenBy(x => x.UrunAdi).ToList();

                if (sonuc.Count > 0)
                {
                    rptMinimumStokRaporu.DataSource = sonuc;
                    rptMinimumStokRaporu.DataBind();
                    pnlMinimumStokRaporu.Visible = true;
                }
                else
                {
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlVeriYok.Visible = true;
        }
    }

    private void TransferRaporuOlustur()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            DateTime baslangic = Convert.ToDateTime(txtBaslangicTarihi.Text);
            DateTime bitis = Convert.ToDateTime(txtBitisTarihi.Text).AddDays(1);

            using (var db = new FabrikaDataClassesDataContext())
            {
                var query = from sh in db.StokHareketleris
                           join d in db.Depolars on sh.DepoID equals d.DepoID
                           join u in db.Urunlers on sh.UrunID equals u.UrunID
                           join b in db.Birimlers on u.BirimID equals b.BirimID into birimGroup
                           from b in birimGroup.DefaultIfEmpty()
                           join k in db.Kullanicilars on sh.KullaniciID equals k.KullaniciID into kullaniciGroup
                           from k in kullaniciGroup.DefaultIfEmpty()
                           where sh.SirketID == sirketID && 
                                 sh.IslemTarihi >= baslangic && 
                                 sh.IslemTarihi < bitis &&
                                 (sh.HareketTipi == "TRANSFER_CIKIS" || sh.HareketTipi == "TRANSFER_GIRIS")
                           select new
                           {
                               sh.DepoID,
                               sh.IslemTarihi,
                               DepoAdi = d.DepoAdi,
                               UrunAdi = u.UrunAdi,
                               sh.HareketTipi,
                               sh.Miktar,
                               BirimAdi = b != null ? b.BirimAdi : "",
                               sh.Aciklama,
                               KullaniciAdi = k != null ? k.AdSoyad : "Sistem"
                           };

                // Depo filtresi
                if (!string.IsNullOrEmpty(ddlDepo.SelectedValue))
                {
                    int depoID = Convert.ToInt32(ddlDepo.SelectedValue);
                    query = query.Where(x => x.DepoID == depoID);
                }

                var sonuc = query.OrderByDescending(x => x.IslemTarihi).ToList();

                if (sonuc.Count > 0)
                {
                    rptHareketRaporu.DataSource = sonuc;
                    rptHareketRaporu.DataBind();
                    pnlHareketRaporu.Visible = true;
                }
                else
                {
                    pnlVeriYok.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            pnlVeriYok.Visible = true;
        }
    }

    // Helper metodları
    protected string GetStokDurumText(object miktar, object minimumMiktar)
    {
        try
        {
            decimal mevcutMiktar = Convert.ToDecimal(miktar);
            decimal? minMiktar = minimumMiktar as decimal?;
            
            if (minMiktar.HasValue)
            {
                if (mevcutMiktar <= 0)
                    return "Stok Yok";
                else if (mevcutMiktar < minMiktar.Value)
                    return "Kritik Seviye";
                else if (mevcutMiktar < minMiktar.Value * 1.5m)
                    return "Düşük Stok";
                else
                    return "Normal";
            }
            else
            {
                return mevcutMiktar > 0 ? "Stokta" : "Stok Yok";
            }
        }
        catch
        {
            return "Bilinmiyor";
        }
    }

    protected string GetStokDurumBadgeClass(object miktar, object minimumMiktar)
    {
        try
        {
            decimal mevcutMiktar = Convert.ToDecimal(miktar);
            decimal? minMiktar = minimumMiktar as decimal?;
            
            if (minMiktar.HasValue)
            {
                if (mevcutMiktar <= 0)
                    return "badge-dark";
                else if (mevcutMiktar < minMiktar.Value)
                    return "badge-danger";
                else if (mevcutMiktar < minMiktar.Value * 1.5m)
                    return "badge-warning";
                else
                    return "badge-success";
            }
            else
            {
                return mevcutMiktar > 0 ? "badge-info" : "badge-dark";
            }
        }
        catch
        {
            return "badge-secondary";
        }
    }

    protected string GetHareketTipiText(string hareketTipi)
    {
        switch (hareketTipi)
        {
            case "GIRIS": return "Giriş";
            case "CIKIS": return "Çıkış";
            case "TRANSFER_GIRIS": return "Transfer Giriş";
            case "TRANSFER_CIKIS": return "Transfer Çıkış";
            case "SAYIM": return "Sayım";
            case "FIRE": return "Fire";
            default: return hareketTipi;
        }
    }

    protected string GetHareketTipiBadgeClass(string hareketTipi)
    {
        switch (hareketTipi)
        {
            case "GIRIS":
            case "TRANSFER_GIRIS":
                return "badge-success";
            case "CIKIS":
            case "TRANSFER_CIKIS":
            case "FIRE":
                return "badge-danger";
            case "SAYIM":
                return "badge-info";
            default:
                return "badge-secondary";
        }
    }

    protected void btnExcelAktar_Click(object sender, EventArgs e)
    {
        try
        {
            // Excel export işlemi
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Depo Raporu");
                
                // Başlık satırı
                worksheet.Cells[1, 1].Value = "Depo Raporu - " + DateTime.Now.ToString("dd.MM.yyyy");
                worksheet.Cells[1, 1, 1, 8].Merge = true;
                
                // Veri başlıkları
                worksheet.Cells[3, 1].Value = "Depo";
                worksheet.Cells[3, 2].Value = "Ürün";
                worksheet.Cells[3, 3].Value = "Miktar";
                worksheet.Cells[3, 4].Value = "Birim";
                worksheet.Cells[3, 5].Value = "Min. Stok";
                worksheet.Cells[3, 6].Value = "Birim Fiyat";
                worksheet.Cells[3, 7].Value = "Toplam Değer";
                worksheet.Cells[3, 8].Value = "Son Güncelleme";
                
                // Veri ekleme işlemi burada yapılacak
                
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=DepoRaporu_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('Excel aktarımında hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnPDFAktar_Click(object sender, EventArgs e)
    {
        try
        {
            // PDF export işlemi
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
            MemoryStream stream = new MemoryStream();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
            
            document.Open();
            
            // Başlık
            var titleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 16);
            var title = new iTextSharp.text.Paragraph("Depo Raporu - " + DateTime.Now.ToString("dd.MM.yyyy"), titleFont);
            title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new iTextSharp.text.Paragraph(" "));
            
            // Tablo oluşturma işlemi burada yapılacak
            
            document.Close();
            
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=DepoRaporu_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                "alert('PDF aktarımında hata oluştu: " + ex.Message + "');", true);
        }
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
