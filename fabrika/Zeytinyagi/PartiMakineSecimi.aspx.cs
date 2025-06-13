using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Linq; // For NumberStyles

public partial class fabrika_Zeytinyagi_PartiMakineSecimi : System.Web.UI.Page
{
    int _sirketID = SessionHelper.GetSirketID();
    int _KullaniciID = SessionHelper.GetKullaniciID();

    FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

        //sirket zeytinyagi makinalari listesi
        ddlZeytinyagiMakinasi.DataSource = db.SirketZeytinyagiMakinalaris.Where(x => x.Durumu == true && x.SirketID == _sirketID).ToList();
        ddlZeytinyagiMakinasi.DataValueField = "SirketZeytinyagiMakinaID";
        //ddlZeytinyagiMakinasi.DataTextField = "Ad";
        ddlZeytinyagiMakinasi.DataBind();


    }

    protected void btnUretimiBaslat_Click(object sender, EventArgs e)
    {
    }

    protected void ddlZeytinyagiMakinasi_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _SirketZeytinyagiMakinaID = int.Parse(ddlZeytinyagiMakinasi.SelectedItem.Value);

        //sirket zeytiyağı makina malaksoyü listesi
        ddlMalaksorler.DataSource = db.SirketZeytinyagiMakinaMalaksorlers.Where(x => x.Durum == true && x.SirketID == _sirketID && x.SirketZeytinyagiMakinaID == _SirketZeytinyagiMakinaID).ToList();
        ddlMalaksorler.DataBind();
    }
}