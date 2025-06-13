using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Zeytinyagi_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
             var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müstahsiller";
                master.SayfaAdi = "Müstahsil Listesi";
            };

        // makina yoksa makina ekleme sayfasina git
        int ZeytinyagiMakiAdet = 0;
        int _SirkeID = SessionHelper.GetSirketID();
        ZeytinyagiMakiAdet = db.SirketZeytinyagiMakinalaris.Where(x=>x.SirketID==_SirkeID).Count(x => x.Durumu == true);

        if (ZeytinyagiMakiAdet == 0)
        {
            Response.Redirect("~/fabrika/Zeytinyagi/ZeytinyagiAyarlar/ZeytinyagiMakinalari.aspx");
        }

        // İlk kez sayfa yükleniyorsa veri getir
        if (!IsPostBack)
        {
            LoadDashboardData();
      
        }
    }
    
    /// <summary>
    /// Gösterge paneli için verileri yükler
    /// </summary>
    private void LoadDashboardData()
    {
        try
        {
            // Gerçek bir uygulamada burada veritabanından veriler alınıp gösterilecektir
            // Örnek: Günlük zeytin girişi, aktif üretim partisi, yağ stoğu, vs.
            
            // Bu örnekte statik veriler kullanıyoruz
            // Gerçek uygulamada veritabanı sorguları yapılacaktır
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama yap
            System.Diagnostics.Debug.WriteLine("Dashboard veri yükleme hatası: " + ex.Message);
        }
    }
}