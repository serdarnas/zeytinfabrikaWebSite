using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Tedarikciler_Alis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Tedarikçi";
            master.SayfaAdi = "Tedarikçi Aliş";
        }
    }
}