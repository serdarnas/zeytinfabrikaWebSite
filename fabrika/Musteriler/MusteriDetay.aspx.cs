using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_MusteriDetay : System.Web.UI.Page
{
    protected GridView gridSatislar;

    // Not: Şirket ID için artık SessionHelper.GetSirketID() kullanılıyor
    private int? _MusteriID
    {
        get
        {
            if (ViewState["MusteriID"] != null)
                return Convert.ToInt32(ViewState["MusteriID"]);
            return null;
        }
        set
        {
            ViewState["MusteriID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // QueryString kontrolü
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                throw new Exception("Müşteri ID belirtilmedi.");
            }

            int musteriId;
            if (!int.TryParse(Request.QueryString["id"], out musteriId))
            {
                throw new Exception("Geçersiz müşteri ID formatı.");
            }
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Müşteri";
            master.SayfaAdi = "Müşteri Detay";
        }

        using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
        {
            // Müşteri bilgilerini getir
            Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == musteriId && x.SirketID == SessionHelper.GetSirketID());
            if (gelenMusteriler == null)
            {
                throw new Exception("Müşteri bulunamadı.");
            }

            if (gelenMusteriler.MusteriResim == null)
        {
            MusteriResim.ImageUrl = "../../App_Themes/serdarnas_admin_flat/img/avatar1_small.jpg";
        }
        else
        {
            MusteriResim.ImageUrl = gelenMusteriler.MusteriResim;
        }

        lblMusteriAdi.Text = gelenMusteriler.FirmaAdi;
        lblTelefon.Text = gelenMusteriler.Telefon;
        lblCepTelefonu.Text = gelenMusteriler.CepTelefonu;
        lblAdres.Text = gelenMusteriler.Adres;
        lblYetkili.Text = gelenMusteriler.YetkiliAdi;
        lblmail.Text = gelenMusteriler.Email;
        lblVergiDairesi.Text = gelenMusteriler.VergiDairesi;
        lblVergiNo.Text = gelenMusteriler.VergiNo;
        
        hplinkMusteriGuncelle.NavigateUrl = "YeniMusteri.aspx?id=" + gelenMusteriler.MusteriID;
        hplinkSatisYap.NavigateUrl = "MusteriSatis.aspx?id=" + gelenMusteriler.MusteriID;

            // Müşteri satışlarını yükle
            var satislar = from s in db.Satislars
                          where s.MusteriID == musteriId && s.SirketID == SessionHelper.GetSirketID()
                          orderby s.SatisTarihi descending
                          select new
                          {
                              s.SatisID,
                              s.SatisTarihi,
                              ToplamTutar = s.SatisDetaylaris.Sum(sd => sd.BirimFiyat * sd.Miktar),
                              SatisDurumu = s.SatisTipi
                          };

                gridSatislar.DataSource = satislar.ToList();
            gridSatislar.DataBind();
        }
        }
        catch (Exception ex)
        {
            // Hata detaylarını logla
            string logPath = Server.MapPath("~/App_Data/Logs");
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            string logFile = Path.Combine(logPath, String.Format("error_{0}.log", DateTime.Now.ToString("yyyyMMdd")));
            string errorDetails = String.Format("\r\n[{0}] Hata: {1}\r\nStack Trace: {2}\r\nQueryString: {3}\r\nKullanıcı: {4}\r\n-------------------\r\n",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ex.Message,
                ex.StackTrace,
                Request.QueryString.ToString(),
                User.Identity.IsAuthenticated ? User.Identity.Name : "Anonim");

            File.AppendAllText(logFile, errorDetails);

            // Hata sayfasına yönlendir
            Response.Redirect(String.Format("~/fabrika/Fabrika_customErrors.aspx?error={0}", Server.UrlEncode(ex.Message)));
        }
    }
}