using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class yonetim_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü
            SessionHelper.KullaniciOturumKontrol();
            
            // Kullanıcı bilgilerini göster
            KullaniciBilgileriniGoster();
            
            // İstatistikleri yükle
            IstatistikleriYukle();
        }
    }
    
    private void KullaniciBilgileriniGoster()
    {
        try
        {
            // Session'dan kullanıcı bilgilerini al
            ltKullaniciAdi.Text = HttpContext.Current.User.Identity.Name;
            
            // Null kontrolü yaparak Session değerlerini al
            if (Session["KullaniciAdSoyad"] != null)
            {
                ltKullaniciAdSoyad.Text = Session["KullaniciAdSoyad"].ToString();
            }
            else
            {
                ltKullaniciAdSoyad.Text = "Bilinmiyor";
            }
            
            ltAdSoyad.Text = ltKullaniciAdSoyad.Text;
            
            if (Session["SirketAdi"] != null)
            {
                ltSirketAdi.Text = Session["SirketAdi"].ToString();
            }
            else
            {
                ltSirketAdi.Text = "Bilinmiyor";
            }
            
            ltSonGirisTarihi.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
        catch (Exception ex)
        {
            // Hata durumunda log tutulabilir
            System.Diagnostics.Debug.WriteLine("Kullanıcı bilgileri gösterilirken hata: " + ex.Message);
        }
    }
    
    private void IstatistikleriYukle()
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Kullanıcı sayısı
            int sirketID =  SessionHelper.GetSirketID();
            int kullaniciSayisiDeger = db.Kullanicilars.Count(k => k.SirketID == sirketID);
            kullaniciSayisi.InnerText = kullaniciSayisiDeger.ToString();
            
            // Menü sayısı
            int menuSayisiDeger = db.Menus.Count();
            menuSayisi.InnerText = menuSayisiDeger.ToString();
            
            // Diğer istatistikler (varsayılan değerler bırakılmıştır)
            // Not: Bu kısım gerçek verilerle doldurulabilir
            siparisCount.InnerText = "0";
            ciroTutar.InnerText = "0₺";
        }
        catch (Exception ex)
        {
            // Hata durumunda log tutulabilir
            System.Diagnostics.Debug.WriteLine("İstatistikler yüklenirken hata: " + ex.Message);
        }
    }
}