using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class yonetim_YonetimMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü
            SessionHelper.KullaniciOturumKontrol();
            
            // Kullanıcı adını göster
            if (Session["KullaniciAdSoyad"] != null)
            {
                lblKullaniciAdi.Text = Session["KullaniciAdSoyad"].ToString();
            }
            
            // Menüleri yükle - Artık sabit menü kullanacağız
            // MenuleriYukle();
        }
    }
    
    /* Eski dinamik menü kodu
    private void MenuleriYukle()
    {
        try
        {
            int kullaniciID = SessionHelper.GetKullaniciID();
            
            // Veritabanı bağlantısı
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Ana menüleri çek
            var anaMenuler = (from m in db.Menus
                             join ky in db.KullaniciYetkis on m.YetkiKodu equals ky.YetkiKodu
                             where m.UstMenuID == 0 
                                && ky.KullaniciID == kullaniciID 
                                && ky.Aktif == true
                             orderby m.Sira
                             select m).ToList();
            
            // Ana menüleri bağla
            rptMenu.DataSource = anaMenuler;
            rptMenu.DataBind();
        }
        catch (Exception ex)
        {
            // Hata oluştuğunda log tutulabilir
            System.Diagnostics.Debug.WriteLine("Menü yükleme hatası: " + ex.Message);
        }
    }
    
    protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Menu menu = (Menu)e.Item.DataItem;
            
            if (menu != null)
            {
                Repeater rptAltMenu = (Repeater)e.Item.FindControl("rptAltMenu");
                
                if (rptAltMenu != null)
                {
                    int kullaniciID = SessionHelper.GetKullaniciID();
                    
                    // Veritabanı bağlantısı
                    FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                    
                    // Alt menüleri çek
                    var altMenuler = (from m in db.Menus
                                    join ky in db.KullaniciYetkis on m.YetkiKodu equals ky.YetkiKodu
                                    where m.UstMenuID == menu.MenuID 
                                        && ky.KullaniciID == kullaniciID 
                                        && ky.Aktif == true
                                    orderby m.Sira
                                    select m).ToList();
                    
                    // Alt menüleri bağla
                    rptAltMenu.DataSource = altMenuler;
                    rptAltMenu.DataBind();
                }
            }
        }
    }
    */
}
