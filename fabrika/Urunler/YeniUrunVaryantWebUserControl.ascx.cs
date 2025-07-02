using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Urunler_YeniUrunVaryantWebUserControl : System.Web.UI.UserControl
{
    private int SirketID;
    private int UrunID;

    protected void Page_Load(object sender, EventArgs e)
    {
        SirketID = SessionHelper.GetSirketID();
    }

    public void SetUrunID(int urunID)
    {
        UrunID = urunID;
        ViewState["UrunID"] = urunID;
    }

    private int GetUrunID()
    {
        if (UrunID > 0) return UrunID;
        if (ViewState["UrunID"] != null)
        {
            int.TryParse(ViewState["UrunID"].ToString(), out UrunID);
        }
        return UrunID;
    }



    // Public metodlar - Ana sayfadan çağrılabilir
    public void ClearVaryantData()
    {
        // Varyant session verilerini temizle
        if (Session["UrunVaryantlari"] != null)
        {
            Session.Remove("UrunVaryantlari");
        }
    }


}