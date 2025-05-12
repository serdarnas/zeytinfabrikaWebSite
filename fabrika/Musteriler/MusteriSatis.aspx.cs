using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_MusteriSatis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            // Geçersiz SirketID durumunda otomatik olarak giriş sayfasına yönlendirilecek
            int sirketID = SessionHelper.GetSirketID();
            // SirketID geçerli, devam et
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müşteri";
                master.SayfaAdi = "Müşteri Satiş";
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }
        
        if (!IsPostBack)
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            int sirketID = SessionHelper.GetSirketID();
            LoadProjeler(sirketID);
            LoadPazarlamacilar(sirketID);
            int _MusteriID = int.Parse(Request.QueryString["id"]);
            Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == _MusteriID);
            lblMusteriAd.Text = gelenMusteriler.FirmaAdi;


            hplbtnGeriDon.NavigateUrl = "MusteriDetay.aspx?id=" + _MusteriID;
        }
    }

    private void LoadPazarlamacilar(int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        var tumPazarlamacilar = db.Pazarlamacilars.Where(x => x.SirketID == sirketID && x.Durum == true).ToList();
        ddlPazarlama.DataSource = tumPazarlamacilar;
        ddlPazarlama.DataBind();
    }

    private void LoadProjeler(int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        var tumProjeler = db.Projelers.Where(x => x.SirketID == sirketID).ToList();
        ddlProje.DataSource = tumProjeler;
        ddlProje.DataBind();
    }
}