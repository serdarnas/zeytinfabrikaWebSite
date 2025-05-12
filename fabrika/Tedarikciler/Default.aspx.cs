using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Tedarikciler_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Tedarikçi";
                master.SayfaAdi = "Tedarikçi Listesi";
            }
            TedarikciListele();
        }
    }

    private void TedarikciListele()
    { 
        int sirketID = SessionHelper.GetSirketID();
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

        
        

        // Arama metnini al
        string aramaMetni = txtArama.Text.Trim();

        // Sorguyu oluştur
        var query = db.Tedarikcilers.Where(x => x.SirketID == sirketID && x.Durum == true);

      

        // Arama filtresi varsa uygula (en az 3 karakter)
        if (!string.IsNullOrEmpty(aramaMetni) && aramaMetni.Length >= 3)
        {
            query = query.Where(x =>
                x.FirmaAdi.Contains(aramaMetni) ||
                x.YetkiliAdi.Contains(aramaMetni) ||
                x.Telefon.Contains(aramaMetni) ||
                x.CepTelefonu.Contains(aramaMetni) ||
                x.Email.Contains(aramaMetni) ||
                x.VergiNo.Contains(aramaMetni) ||
                x.TedarikciKodu.Contains(aramaMetni));
        }

        // Sonuçları al
        var Tedarikciler = query.Select(x => new
        {
            TedarikciID = x.TedarikciID,
            x.FirmaAdi,
            x.Telefon,
            x.CepTelefonu,
            AcikBakiye = FabrikaTools.Tedarikci.Acikbakiye(x.TedarikciID),
            CekSenetBakiye = FabrikaTools.Tedarikci.CekSenetBakiye(x.TedarikciID)
        }).ToList();

        rptTedarikciler.DataSource = Tedarikciler;
        rptTedarikciler.DataBind();
       
    }

    protected void btnAra_Click(object sender, EventArgs e)
    {
        TedarikciListele();
    }
}

