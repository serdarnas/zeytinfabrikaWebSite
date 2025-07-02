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
            // İlk yüklemede tüm müstahsilleri getir
            BindMustahsilList();
        }
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Master page'in breadcrumb ayarlarını yap
        var master = this.Master as fabrika_FabrikaMasterPage;
        if (master != null)
        {
            master.KlasorAdi = "Müstahsiller";
            master.SayfaAdi = "Müstahsil Listesi";
        }
    }
    
    private void BindMustahsilList(string aramaMetni = "")
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                
                var query = db.Mustahsillers.Where(m => m.SirketID == sirketID);
                
                // Arama kriteri varsa filtrele
                if (!string.IsNullOrEmpty(aramaMetni) && aramaMetni.Length >= 3)
                {
                    query = query.Where(m => 
                        m.Ad.Contains(aramaMetni) || 
                        m.Soyad.Contains(aramaMetni) || 
                        (m.TCKimlikNo != null && m.TCKimlikNo.Contains(aramaMetni)));
                }
                
                // Sıralama
                var mustahsiller = query.OrderBy(m => m.Ad).ThenBy(m => m.Soyad).ToList();
                
                rptMustahsiller.DataSource = mustahsiller;
                rptMustahsiller.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda kullanıcıya bilgi ver
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorMessage", 
                string.Format("alert('Müstahsil listesi yüklenirken hata oluştu: {0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }
    
    protected void btnAra_Click(object sender, EventArgs e)
    {
        string aramaMetni = txtArama.Text.Trim();
        BindMustahsilList(aramaMetni);
    }
}