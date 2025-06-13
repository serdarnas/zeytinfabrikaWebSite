using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Projelerim : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            LoadProjeler();
        }
    }

    private void LoadProjeler()
    {
        int _sirketID = SessionHelper.GetSirketID();
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        var tumProjeler = db.Projelers.Where(x => x.SirketID == _sirketID).ToList();

        rptProjeler.DataSource = tumProjeler;
        rptProjeler.DataBind();
    }
}