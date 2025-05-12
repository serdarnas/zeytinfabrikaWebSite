using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Mustahsil_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HiddenFieldSirketID.Value = SessionHelper.GetSirketID().ToString();
        Session.Add("SirketID", SessionHelper.GetSirketID());

        if (!IsPostBack)
        {

            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müstahsiller";
                master.SayfaAdi = "Müstahsil Listesi";
            }
        }
    }

    protected void btnYeniEkle_Click(object sender, EventArgs e)
    {
        ASPxGridViewMustahsil.AddNewRow();
    }
}