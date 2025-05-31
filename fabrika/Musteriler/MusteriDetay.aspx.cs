using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_MusteriDetay : System.Web.UI.Page
{
    // Not: Şirket ID için artık SessionHelper.GetSirketID() kullanılıyor
    private int? _MusteriID
    {
        get
        {
            if (ViewState["MusteriID"] != null)
                return Convert.ToInt32(ViewState["MusteriID"]);
            return null;
        }
        set
        {
            ViewState["MusteriID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int _MusteriID = int.Parse(Request.QueryString["id"]);
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Müşteri";
            master.SayfaAdi = "Müşteri Detay";
        }

        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == _MusteriID);
        if (gelenMusteriler.MusteriResim == null)
        {
            MusteriResim.ImageUrl = "../../App_Themes/serdarnas_admin_flat/img/avatar1_small.jpg";
        }
        else
        {
            MusteriResim.ImageUrl = gelenMusteriler.MusteriResim;
        }

        lblMusteriAdi.Text = gelenMusteriler.FirmaAdi;
        lblTelefon.Text = gelenMusteriler.Telefon;
        lblCepTelefonu.Text = gelenMusteriler.CepTelefonu;
        lblAdres.Text = gelenMusteriler.Adres;
        lblYetkili.Text = gelenMusteriler.YetkiliAdi;
        lblmail.Text = gelenMusteriler.Email;
        lblVergiDairesi.Text = gelenMusteriler.VergiDairesi;
        lblVergiNo.Text = gelenMusteriler.VergiNo;
        
        hplinkMusteriGuncelle.NavigateUrl = "YeniMusteri.aspx?id=" + gelenMusteriler.MusteriID;
        hplinkSatisYap.NavigateUrl = "MusteriSatis.aspx?id=" + gelenMusteriler.MusteriID;
    }
}