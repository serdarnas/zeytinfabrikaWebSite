using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Urunler_YeniUrun : System.Web.UI.Page
{ 
    private const string ResimSessionKey = "YukluResimler";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Ürünler";
                master.SayfaAdi = "Yeni Ürün";
            }
            ResimleriYukleVeGoster();
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {

    }

    protected void btnGuncelle_Click(object sender, EventArgs e)
    {
        }

    protected void btnResimYukle_Click(object sender, EventArgs e)
    {
        string ResimKlasoru = "/fabrika/Depo/Sirket" + SessionHelper.GetSirketID().ToString() + "/Depo/UrunResimleri/";
        if (fuResimler.HasFiles)
        {
            List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
            foreach (HttpPostedFile uploadedFile in fuResimler.PostedFiles)
            {
                string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string sunucuYolu = Server.MapPath(ResimKlasoru + dosyaAdi);
                Directory.CreateDirectory(Server.MapPath(ResimKlasoru));
                uploadedFile.SaveAs(sunucuYolu);
                resimYollari.Add(ResimKlasoru + dosyaAdi);
            }
            Session[ResimSessionKey] = resimYollari;
            ResimleriYukleVeGoster();
        }
    }

    private void ResimleriYukleVeGoster()
    {
        List<string> resimYollari = Session[ResimSessionKey] as List<string> ?? new List<string>();
        var resimList = resimYollari.Select(x => new { ImageUrl = x }).ToList();
        rptResimler.DataSource = resimList;
        rptResimler.DataBind();
    }
}