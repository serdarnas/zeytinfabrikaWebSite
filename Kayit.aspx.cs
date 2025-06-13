using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Kayit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Eğer kullanıcı giriş yapmışsa kayıt sayfasına erişimi engelle
        if (Session["KullaniciID"] != null)
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void btnKayit_Click(object sender, EventArgs e)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        if (KayitliKulanicikotrolu(txtEmail.Text))
        {
            // kulanici kayitli
            lblEmailKayitli.Text = "Bu Mail Adresi ile daha önce kayit yapilmiş.";}
        else
        {
            //kullanici Kayitli Değil
            //yeni kullanici
            int _sirketID = 0;int _yetkiID = 0;

            //Şirket Kayıt Edilir
            Sirketler yeniSirketler =new Sirketler();
            yeniSirketler.SirketAdi = txtSirketAdi.Text;
            yeniSirketler.OlusturmaTarihi=DateTime.Now;
            yeniSirketler.Email = txtEmail.Text;
            yeniSirketler.Aktif = true;
            db.Sirketlers.InsertOnSubmit(yeniSirketler);
            db.SubmitChanges();
            _sirketID = yeniSirketler.SirketID;

            //yetkiler açilir
            Yetkiler yeniYetkiler=new Yetkiler();
            yeniYetkiler.SirketID = _sirketID;
            yeniYetkiler.YetkiAdi = "Admin";
            yeniYetkiler.Aciklama = "Şirket Yetkilisi";
            db.Yetkilers.InsertOnSubmit(yeniYetkiler);
            db.SubmitChanges();
            _yetkiID = yeniYetkiler.YetkiID;
            //Kullanici açilir


            Kullanicilar yeniKullanicilar = new Kullanicilar();
            yeniKullanicilar.SirketID = _sirketID;
            yeniKullanicilar.AdSoyad = txtAdSoyad.Text;
            yeniKullanicilar.Telefon = txtTelefon.Text;
            yeniKullanicilar.Email = txtEmail.Text;
            yeniKullanicilar.Sifre = txtSifre.Text;
            yeniKullanicilar.YetkiID = _yetkiID;
            yeniKullanicilar.Aktif = true;
            yeniKullanicilar.OlusturmaTarihi=DateTime.Now;
            db.Kullanicilars.InsertOnSubmit(yeniKullanicilar);
            db.SubmitChanges();

            // Yeni kullanıcıya tüm menü yetkilerini ver
            YetkiHelper.TumMenuYetkileriniVer(yeniKullanicilar.KullaniciID);

            EmailHelper.SendWelcomeMail(txtEmail.Text,txtAdSoyad.Text);
            Response.Redirect("giris.aspx");
        }

    }

    private bool KayitliKulanicikotrolu(string _Email)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        Kullanicilar istennenKullanicilar = db.Kullanicilars.FirstOrDefault(x => x.Email == _Email);
        if (istennenKullanicilar != null)
        {
            //kullanici Kayitli
            return true;
            
        }
        else
        {
            //kullanci Kayitli Değil
            return false;
        }
    }
}