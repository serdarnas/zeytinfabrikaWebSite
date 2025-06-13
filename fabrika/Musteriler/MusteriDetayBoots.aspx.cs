using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Musteriler_MusteriDetayBoots : System.Web.UI.Page
{
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
        try
        {
            if (!IsPostBack)
            {
                try
                {
                    if (string.IsNullOrEmpty(Request.QueryString["id"]))
                    {
                        Response.Redirect("~/fabrika/Musteriler/MusteriListesi.aspx");
                        return;
                    }

                    LoadMusteriData();
                }
                catch (Exception ex)
                {
                    // Hata mesajını görüntüle
                    string script = string.Format("alert('Hata oluştu: {0}')", ex.Message.Replace("'", "\\'"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorAlert", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            // Hata mesajını görüntüle
            string script = string.Format("alert('Hata oluştu: {0}')", ex.Message.Replace("'", "\\'"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorAlert", script, true);
        }
    }
    private void LoadMusteriData()
    {
        try
        {
            int musteriId;
            if (!int.TryParse(Request.QueryString["id"], out musteriId))
            {
                throw new Exception("Geçersiz müşteri ID");
            }
            // musteriId yukarıda parse edildi
            _MusteriID = musteriId;

            // Master page başlığını ayarla
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Müşteri";
                master.SayfaAdi = "Müşteri Detay";
            }

            using (var db = new FabrikaDataClassesDataContext())
            {
                // Müşteri bilgilerini getir
                var musteri = db.Musterilers.FirstOrDefault(x => x.MusteriID == musteriId && x.SirketID == SessionHelper.GetSirketID());
                if (musteri == null)
                {
                    Response.Redirect("~/fabrika/Musteriler/MusteriListesi.aspx");
                    return;
                }

                // Profil resmini ayarla
                MusteriResim.ImageUrl = string.IsNullOrEmpty(musteri.MusteriResim) 
                    ? "../../App_Themes/serdarnas_admin_flat/img/avatar1_small.jpg" 
                    : musteri.MusteriResim;

                // Müşteri bilgilerini doldur
                lblMusteriAdi.Text = musteri.FirmaAdi;
                lblTelefon.Text = musteri.Telefon;
                lblCepTelefonu.Text = musteri.CepTelefonu;
                lblAdres.Text = musteri.Adres;
                lblYetkili.Text = musteri.YetkiliAdi;
                lblmail.Text = musteri.Email;
                lblVergiDairesi.Text = musteri.VergiDairesi;
                lblVergiNo.Text = musteri.VergiNo;

                if (musteri != null)
                {
                    // Linkleri ayarla
                    hplinkMusteriGuncelleBoots.NavigateUrl = string.Format("YeniMusteri.aspx?id={0}", musteri.MusteriID);
                    hplinkSatisYapBoots.NavigateUrl = string.Format("MusteriSatis.aspx?id={0}", musteri.MusteriID);
                }
                else
                {
                    // Handle null case
                    //lblMessage.Text = "Müşteri bulunamadı";
                    //lblMessage.CssClass = "text-danger";
                }

                // İstatistikleri hesapla ve doldur
                var satislar = from s in db.Satislars
                              where s.MusteriID == musteriId && s.SirketID == SessionHelper.GetSirketID()
                              select s;

                decimal toplamSatis = satislar.AsEnumerable().Sum(s => db.SatisDetaylaris
                        .Where(sd => sd.SatisID == s.SatisID)
                        .Sum(sd => sd.BirimFiyat * sd.Miktar));
                decimal acikBakiye = 0; // Hesaplama mantığı eklenecek
                decimal cekBakiye = 0;  // Hesaplama mantığı eklenecek
                decimal senetBakiye = 0; // Hesaplama mantığı eklenecek

                lblToplamSatis.Text = string.Format("{0:C2}", toplamSatis);
                lblAcikBakiye.Text = string.Format("{0:C2}", acikBakiye);
                lblCekBakiye.Text = string.Format("{0:C2}", cekBakiye);
                lblSenetBakiye.Text = string.Format("{0:C2}", senetBakiye);

                // Satışları grid'e bind et
                var satisListesi = satislar.Select(s => new
                                 {
                                     s.SatisID,
                                     s.SatisTarihi,
                                     ToplamTutar = db.SatisDetaylaris
                                         .Where(sd => sd.SatisID == s.SatisID)
                                         .Sum(sd => sd.BirimFiyat * sd.Miktar),
                                     SatisDurumu = s.SatisTipi
                                 });

                gridSatislar.DataSource = satisListesi;
                gridSatislar.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("Müşteri bilgileri yüklenirken hata oluştu: {0}", ex.Message));
        }
    }

    protected void gridSatislar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ödeme durumuna göre badge rengini ayarla
                Label lblOdemeDurumu = e.Row.FindControl("lblOdemeDurumu") as Label;
                if (lblOdemeDurumu != null)
                {
                    string odemeDurumu = DataBinder.Eval(e.Row.DataItem, "OdemeDurumu").ToString();
                    lblOdemeDurumu.CssClass = odemeDurumu == "Ödendi" ? "badge bg-success" : "badge bg-warning";
                }

                // Satış durumuna göre hücre rengini ayarla
                string satisDurumu = DataBinder.Eval(e.Row.DataItem, "SatisDurumu").ToString();
                switch (satisDurumu.ToLower())
                {
                    case "tamamlandı":
                        e.Row.Cells[4].CssClass = "text-success";
                        break;
                    case "beklemede":
                        e.Row.Cells[4].CssClass = "text-warning";
                        break;
                    case "iptal":
                        e.Row.Cells[4].CssClass = "text-danger";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            string script = string.Format("console.error('Satır formatını ayarlarken hata: {0}')", ex.Message.Replace("'", "\\'")); 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "rowError", script, true);
        }
    }
}
