using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Pazarlama_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Pazarlama";
                master.SayfaAdi = "Pazarlama Listesi";
            }
        }
    }
}