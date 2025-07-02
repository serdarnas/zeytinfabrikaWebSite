using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetSirketID metodu içinde gerekli kontroller yapılacak
            int sirketID = SessionHelper.GetSirketID();
            
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Depo";
                master.SayfaAdi = "Depo Listesi";
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
            return;
        }
        
        if (!IsPostBack)
        {
            DepoListele();
        }
    }

    private void DepoListele()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();



            // Durum filtresini al
            bool? durum = null;
            if (!string.IsNullOrEmpty(ddlDurum.SelectedValue))
            {
                durum = ddlDurum.SelectedValue == "1";
            }

            // Kapasite filtresini al
            string kapasiteFiltresi = ddlKapasiteFiltresi.SelectedValue;

            // Doluluk oranı filtresini al
            string dolulukFiltresi = ddlDolulukOrani.SelectedValue;

            // Arama metnini al
            string aramaMetni = txtArama.Text.Trim();

            // Sorguyu oluştur
            var query = db.Depolars.Where(x => x.SirketID == sirketID);

            // Durum filtresi varsa uygula
            if (durum.HasValue)
            {
                query = query.Where(x => x.Durum == durum.Value);
            }

            // Kapasite filtresi uygula
            if (!string.IsNullOrEmpty(kapasiteFiltresi))
            {
                switch (kapasiteFiltresi)
                {
                    case "1": // Küçük (0-1000)
                        query = query.Where(x => x.Kapasite <= 1000);
                        break;
                    case "2": // Orta (1000-5000)
                        query = query.Where(x => x.Kapasite > 1000 && x.Kapasite <= 5000);
                        break;
                    case "3": // Büyük (5000+)
                        query = query.Where(x => x.Kapasite > 5000);
                        break;
                }
            }

            // Arama filtresi varsa uygula
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                query = query.Where(x =>
                    x.DepoAdi.Contains(aramaMetni) ||
                    x.DepoKodu.Contains(aramaMetni));
            }

            // Sonuçları al ve hesaplamaları yap
            var depolar = query.ToList().Select(x => new
            {
                DepoID = x.DepoID,
                DepoAdi = x.DepoAdi,
                DepoKodu = x.DepoKodu,
                Kapasite = x.Kapasite ?? 0,
                DoluMiktar = x.DoluMiktar ?? 0,
                Durum = x.Durum ?? true,
                DolulukOrani = x.Kapasite > 0 ? ((x.DoluMiktar ?? 0) / x.Kapasite) * 100 : 0,
                BosKapasite = (x.Kapasite ?? 0) - (x.DoluMiktar ?? 0)
            }).ToList();

            // Doluluk oranı filtresi uygula (hesaplama sonrası)
            if (!string.IsNullOrEmpty(dolulukFiltresi))
            {
                switch (dolulukFiltresi)
                {
                    case "1": // Boş (%0-25)
                        depolar = depolar.Where(x => x.DolulukOrani <= 25).ToList();
                        break;
                    case "2": // Az Dolu (%25-50)
                        depolar = depolar.Where(x => x.DolulukOrani > 25 && x.DolulukOrani <= 50).ToList();
                        break;
                    case "3": // Yarı Dolu (%50-75)
                        depolar = depolar.Where(x => x.DolulukOrani > 50 && x.DolulukOrani <= 75).ToList();
                        break;
                    case "4": // Dolu (%75-100)
                        depolar = depolar.Where(x => x.DolulukOrani > 75).ToList();
                        break;
                }
            }

            rptDepolar.DataSource = depolar.OrderBy(x => x.DepoAdi);
            rptDepolar.DataBind();
            lblToplamDepo.Text = depolar.Count.ToString();
        }
        catch (Exception ex)
        {
            // Hata durumunda boş liste göster
            rptDepolar.DataSource = null;
            rptDepolar.DataBind();
            lblToplamDepo.Text = "0";
        }
    }

    protected string GetProgressBarClass(decimal dolulukOrani)
    {
        if (dolulukOrani <= 25)
            return "progress-bar-success";
        else if (dolulukOrani <= 50)
            return "progress-bar-warning";
        else if (dolulukOrani <= 75)
            return "progress-bar-info";
        else
            return "progress-bar-danger";
    }

    protected void ddlDurum_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepoListele();
    }

    protected void ddlKapasiteFiltresi_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepoListele();
    }

    protected void ddlDolulukOrani_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepoListele();
    }

    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        DepoListele();
    }

    protected void btnYeniDepo_Click(object sender, EventArgs e)
    {
        Response.Redirect("YeniDepo.aspx");
    }

    protected void btnDepoRaporu_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoRaporlari.aspx");
    }
}