using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Fabrika";
                master.SayfaAdi = "İşletme Paneli";
            }
            HesaplaStokDegeri();
        }
    }

    private void HesaplaStokDegeri()
    {
        using (var db = new FabrikaDataClassesDataContext())
        {
            //var stokDegeri = db.Urunlers
            //    .Where(u => u.StokMiktari.HasValue && u.AlisFiyati.HasValue)
            //    .Sum(u => (u.StokMiktari ?? 0) * (u.AlisFiyati ?? 0));
            //lblStokDegeri.Text = stokDegeri.ToString("N2") + " TL";
        }
    }
}