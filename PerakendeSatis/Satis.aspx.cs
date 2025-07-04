using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.IO;
using System.Transactions;

namespace PerakendeSatis
{
    public partial class PerakendeSatis_Satis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UrunleriYukle();
                KategorileriYukle();
            }
        }

        private void UrunleriYukle()
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var urunler = db.Urunlers
                    .Where(u => u.Durum == true && u.PerakendeSatisVarmi== true &&u.SirketID == (int)Session["SirketID"])
                    .Select(u => new 
                    {
                        u.UrunID,
                        u.UrunAdi,
                        u.Fiyat,
                        u.ResimURL,
                        u.KategoriID
                    }).ToList();

                rptUrunler.DataSource = urunler;
                rptUrunler.DataBind();
            }
        }

        private void KategorileriYukle()
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                var kategoriler = db.UrunKategorileris
                    .Where(k => k.Durumu == true && k.SirketID == (int)Session["SirketID"])
                    .Select(k => new 
                    {
                        k.KategoriID,
                        k.Ad
                    }).ToList();

                rptKategoriler.DataSource = kategoriler;
                rptKategoriler.DataBind();
            }
        }

        protected void rptKategoriler_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lnkKategori = (HyperLink)e.Item.FindControl("lnkKategori");
                lnkKategori.Attributes["data-category"] = DataBinder.Eval(e.Item.DataItem, "KategoriID").ToString();
            }
        }

        protected void rptUrunler_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var imgUrun = (Image)e.Item.FindControl("imgUrun");
                
                string resimURL = DataBinder.Eval(e.Item.DataItem, "ResimURL").ToString();
                if (!string.IsNullOrEmpty(resimURL))
                {
                    imgUrun.ImageUrl = "~/Images/Products/" + resimURL;
                }
                else
                {
                    imgUrun.ImageUrl = "~/Images/Products/default.png";
                }
            }
        }

        [WebMethod]
        public static OdemeSonuc OdemeYap(SepetUrun[] urunler)
        {
            try
            {
                using (var db = new FabrikaDataClassesDataContext())
                {
                    // Stok kontrolü yap
                    foreach (var urun in urunler)
                    {
                        var stok = db.Stoklar.FirstOrDefault(s => s.UrunID == urun.id && s.SirketID == (int)HttpContext.Current.Session["SirketID"]);
                        if (stok == null || stok.Miktar < urun.adet)
                        {
                            return new OdemeSonuc { 
                                success = false, 
                                message = string.Format("{0} ürününden yeteri stok yok!", urun.ad) 
                            };
                        }
                    }

                    using (var transaction = new TransactionScope())
                    {
                        // Yeni satış oluştur
                        var yeniSatis = new Satislar
                        {
                            SirketID = (int)HttpContext.Current.Session["SirketID"],
                            Tarih = DateTime.Now,
                            ToplamTutar = urunler.Sum(u => u.adet * u.fiyat),
                            OdemeDurum = "Tamamlandı",
                            Aciklama = "Perakende Satış",
                            KullaniciID = (int)HttpContext.Current.Session["KullaniciID"]
                        };

                        db.Satislar.InsertOnSubmit(yeniSatis);
                        db.SubmitChanges();

                        // Satış detaylarını ekle
                        foreach (var urun in urunler)
                        {
                            var satisDetay = new SatisDetaylari
                            {
                                SatisID = yeniSatis.SatisID,
                                UrunID = urun.id,
                                Miktar = urun.adet,
                                BirimFiyat = urun.fiyat,
                                KdvOrani = 1 // Varsayılan KDV
                            };

                            db.SatisDetaylari.InsertOnSubmit(satisDetay);

                            // Stok güncelle
                            var stok = db.Stoklar.First(s => s.UrunID == urun.id && s.SirketID == yeniSatis.SirketID);
                            stok.Miktar -= urun.adet;
                        }

                        db.SubmitChanges();
                        transaction.Complete();

                        // Fiş oluştur
                        var fisHTML = FisOlustur(yeniSatis.SatisID, db);
                        FisKaydet(yeniSatis.SatisID, fisHTML);

                        return new OdemeSonuc { 
                            success = true,
                            message = string.Format("SAT-{0}", yeniSatis.SatisID),
                            fisHTML = fisHTML
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OdemeSonuc { success = false, message = ex.Message };
            }
        }

        private static string FisOlustur(int satisId, FabrikaDataClassesDataContext db)
        {
            var satis = db.Satislar.First(s => s.SatisID == satisId);
            var detaylar = db.SatisDetaylari.Where(d => d.SatisID == satisId).ToList();

            string html = "<div class='fis-container' style='width:300px; margin:0 auto; font-family:Arial;'>" +
                          "<h4 style='text-align:center;'>ZEYTİN FABRİKASI</h4>" +
                          string.Format("<p style='text-align:center;'>SATIŞ FİŞİ: SAT-{0}</p>", satis.SatisID) +
                          string.Format("<p style='text-align:center;'>{0}</p>", satis.Tarih.ToString("dd.MM.yyyy HH:mm")) +
                          "<hr><table style='width:100%; border-collapse:collapse;'>" +
                          "<tr><th style='text-align:left;'>Ürün</th>" +
                          "<th style='text-align:right;'>Adet</th>" +
                          "<th style='text-align:right;'>Tutar</th></tr>";

            foreach (var detay in detaylar)
            {
                html += string.Format("<tr><td>{0}</td><td style='text-align:right;'>{1}</td><td style='text-align:right;'>{2:C}</td></tr>",
                    detay.Urunler.UrunAdi, detay.Miktar, detay.BirimFiyat * detay.Miktar);
            }

            html += "<tr style='border-top:1px solid #000;'>" +
                    "<td colspan='2' style='text-align:right; font-weight:bold;'>TOPLAM:</td>" +
                    string.Format("<td style='text-align:right; font-weight:bold;'>{0:C}</td></tr>", satis.ToplamTutar) +
                    "</table><hr>" +
                    "<p style='text-align:center;'>Teşekkür ederiz!</p></div>";

            return html;
        }

        private static void FisKaydet(int satisId, string fisHTML)
        {
            string dosyaYolu = HttpContext.Current.Server.MapPath(string.Format("~/Fisler/SAT-{0}.html", satisId));
            System.IO.File.WriteAllText(dosyaYolu, fisHTML);
        }

        public class SepetUrun
        {
            public int id { get; set; }
            public string ad { get; set; }
            public decimal fiyat { get; set; }
            public int adet { get; set; }
            public int PerakendeKdv { get; set; }
        }

        public class OdemeSonuc
        {
            public bool success { get; set; }
            public string message { get; set; }
            public string fisHTML { get; set; }
        }
    }
}