using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Tedarikciler_Detay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Tedarikçi";
            master.SayfaAdi = "Tedarikçi Detay";
        }
        int _TedarikciID = int.Parse(Request.QueryString["id"]);
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        Tedarikciler gelenTedarikciler = db.Tedarikcilers.FirstOrDefault(x => x.TedarikciID == _TedarikciID);

        TedarikciResim.ImageUrl = "../../App_Themes/serdarnas_admin_flat/img/avatar1_small.jpg";


        lblTedarikciAdi.Text = gelenTedarikciler.FirmaAdi;
        lblTelefon.Text = gelenTedarikciler.Telefon;
        lblCepTelefonu.Text = gelenTedarikciler.CepTelefonu;
        lblAdres.Text = gelenTedarikciler.Adres;
        lblYetkili.Text = gelenTedarikciler.YetkiliAdi;
        lblmail.Text = gelenTedarikciler.Email;
        lblVergiDairesi.Text = gelenTedarikciler.VergiDairesi;
        lblVergiNo.Text = gelenTedarikciler.VergiNo;
        lblNot.Text = gelenTedarikciler.Notlar;
        hplinkTedarikciGuncelle.NavigateUrl = "Yeni.aspx?id=" + gelenTedarikciler.TedarikciID;
        hplinkSatisYap.NavigateUrl = "Satis.aspx?id=" + gelenTedarikciler.TedarikciID;
        hplinkAlisYap.NavigateUrl = "Alis.aspx?id=" + gelenTedarikciler.TedarikciID;
    }
}