using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mobil_MustahsilListesi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü - SessionHelper kullan
            try
            {
                int sirketID = SessionHelper.GetSirketID();
                // SirketID doğru alındıysa Session'a da kaydet
                Session["SirketID"] = sirketID;
            }
            catch
            {
                Response.Redirect("Login.aspx");
                return;
            }
            
            LoadMustahsiller();
        }
    }

    protected void txtAra_TextChanged(object sender, EventArgs e)
    {
        LoadMustahsiller();
    }

    protected void btnYenile_Click(object sender, EventArgs e)
    {
        txtAra.Text = "";
        LoadMustahsiller();
    }

    private void LoadMustahsiller()
    {
        try
        {
            // Session kontrolü
            int sirketID = 0;
            if (Session["SirketID"] != null)
            {
                sirketID = Convert.ToInt32(Session["SirketID"]);
            }
            else
            {
                // SessionHelper'dan tekrar deneyelim
                try
                {
                    sirketID = SessionHelper.GetSirketID();
                    Session["SirketID"] = sirketID;
                }
                catch
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
            }
            string aramaMetni = "";
            
            if (txtAra != null && !string.IsNullOrEmpty(txtAra.Text))
            {
                aramaMetni = txtAra.Text.Trim();
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                var mustahsiller = db.Mustahsillers.Where(m => m.SirketID == sirketID).ToList();

                // Arama varsa filtrele
                if (!string.IsNullOrEmpty(aramaMetni))
                {
                    mustahsiller = mustahsiller.Where(m => 
                        (m.Ad != null && m.Ad.ToLower().Contains(aramaMetni)) ||
                        (m.Soyad != null && m.Soyad.ToLower().Contains(aramaMetni)) ||
                        (m.Telefon != null && m.Telefon.Contains(aramaMetni)) ||
                        (m.Email != null && m.Email.ToLower().Contains(aramaMetni)) ||
                        (m.TCKimlikNo != null && m.TCKimlikNo.Contains(aramaMetni))
                    ).ToList();
                }

                // Sırala
                mustahsiller = mustahsiller.OrderByDescending(m => m.OlusturmaTarihi).ThenBy(m => m.Ad).ToList();

                if (mustahsiller.Count > 0)
                {
                    rptMustahsiller.DataSource = mustahsiller;
                    rptMustahsiller.DataBind();
                    rptMustahsiller.Visible = true;
                    pnlEmpty.Visible = false;
                    
                    if (string.IsNullOrEmpty(aramaMetni))
                    {
                        lblKayitSayisi.Text = mustahsiller.Count + " müstahsil";
                    }
                    else
                    {
                        lblKayitSayisi.Text = mustahsiller.Count + " sonuç";
                    }
                }
                else
                {
                    rptMustahsiller.Visible = false;
                    pnlEmpty.Visible = true;
                    
                    if (string.IsNullOrEmpty(aramaMetni))
                    {
                        lblKayitSayisi.Text = "0 müstahsil";
                        lblEmptyBaslik.Text = "Henüz müstahsil eklenmemiş";
                        lblEmptyAciklama.Text = "Yeni müstahsil eklemek için + butonuna tıklayın";
                    }
                    else
                    {
                        lblKayitSayisi.Text = "0 sonuç";
                        lblEmptyBaslik.Text = "Sonuç bulunamadı";
                        lblEmptyAciklama.Text = "Farklı kelimeler deneyin";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            rptMustahsiller.Visible = false;
            pnlEmpty.Visible = true;
            lblKayitSayisi.Text = "Hata oluştu";
            lblEmptyBaslik.Text = "Bağlantı Hatası";
            lblEmptyAciklama.Text = "Lütfen tekrar deneyin";
        }
    }
}