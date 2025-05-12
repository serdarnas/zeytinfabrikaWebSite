using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_ProjeDetay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SirketID"] == null)
        {
            Response.Redirect("~/fabrika/Default.aspx");
        }
        if (!IsPostBack)
        {
            LoadProjeDetay();
        }
    }

    private void LoadProjeDetay()
    {
        int _sirketID = int.Parse(Session["SirketID"].ToString());
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        Projeler GelenProjeler = db.Projelers.FirstOrDefault(x => x.SirketID == _sirketID);
        lblProjeAdi.Text = GelenProjeler.Ad;
        lblProjeAciklama.Text = GelenProjeler.Detay;

    }
}