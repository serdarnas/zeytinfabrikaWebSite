using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class yonetim_KullaniciYetki : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü
            SessionHelper.KullaniciOturumKontrol();
            
            // Kullanıcıları yükle
            KullanicilariYukle();
        }
    }
    
    private void KullanicilariYukle()
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Kullanıcıları çek
            var kullanicilar = (from k in db.Kullanicilars
                               where k.SirketID == SessionHelper.GetSirketID()
                               orderby k.AdSoyad
                               select new
                               {
                                   k.KullaniciID,
                                   k.AdSoyad
                               }).ToList();
            
            // DropDown'ı doldur
            ddlKullanici.DataSource = kullanicilar;
            ddlKullanici.DataTextField = "AdSoyad";
            ddlKullanici.DataValueField = "KullaniciID";
            ddlKullanici.DataBind();
            
            // Boş seçenek ekle
            ddlKullanici.Items.Insert(0, new ListItem("-- Kullanıcı Seçin --", "0"));
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Kullanıcılar yüklenirken hata oluştu: " + ex.Message;
        }
    }
    
    protected void ddlKullanici_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlKullanici.SelectedIndex > 0)
        {
            int kullaniciID = Convert.ToInt32(ddlKullanici.SelectedValue);
            YetkileriYukle(kullaniciID);
        }
        else
        {
            // Seçim yapılmadıysa grid'i temizle
            gvYetkiler.DataSource = null;
            gvYetkiler.DataBind();
        }
    }
    
    private void YetkileriYukle(int kullaniciID)
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Tüm menüleri ve kullanıcının yetkilerini al
            var menuler = (from m in db.Menus
                          orderby m.UstMenuID, m.Sira
                          select new
                          {
                              m.MenuID,
                              m.MenuAdi,
                              m.YetkiKodu,
                              YetkiDurumu = (from ky in db.KullaniciYetkis
                                           where ky.KullaniciID == kullaniciID && ky.YetkiKodu == m.YetkiKodu
                                           select ky.Aktif).FirstOrDefault()
                          }).ToList();
            
            gvYetkiler.DataSource = menuler;
            gvYetkiler.DataBind();
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Yetkiler yüklenirken hata oluştu: " + ex.Message;
        }
    }
    
    protected void gvYetkiler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "YetkiVer")
        {
            try
            {
                int kullaniciID = Convert.ToInt32(ddlKullanici.SelectedValue);
                string yetkiKodu = e.CommandArgument.ToString();
                
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                
                // Yetki kaydını kontrol et
                KullaniciYetki yetki = db.KullaniciYetkis.FirstOrDefault(
                    ky => ky.KullaniciID == kullaniciID && ky.YetkiKodu == yetkiKodu);
                
                if (yetki != null)
                {
                    // Yetki durumunu tersine çevir
                    yetki.Aktif = !yetki.Aktif;
                }
                else
                {
                    // Yeni yetki kaydı oluştur
                    KullaniciYetki yeniYetki = new KullaniciYetki
                    {
                        KullaniciID = kullaniciID,
                        YetkiKodu = yetkiKodu,
                        Aktif = true
                    };
                    
                    db.KullaniciYetkis.InsertOnSubmit(yeniYetki);
                }
                
                db.SubmitChanges();
                
                // Yetkiler yeniden yüklenir
                YetkileriYukle(kullaniciID);
                
                pnlBasari.Visible = true;
                lblBasari.Text = "Kullanıcı yetkileri güncellendi.";
            }
            catch (Exception ex)
            {
                pnlHata.Visible = true;
                lblHata.Text = "Yetki güncellenirken hata oluştu: " + ex.Message;
            }
        }
    }
} 