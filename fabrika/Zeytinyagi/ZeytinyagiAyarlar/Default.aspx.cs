using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Zeytinyagi_ZeytinyagiAyarlar_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Zeytinyağı";
            master.SayfaAdi = "Zeytinyağı Ayarlari";
        };
    }
}