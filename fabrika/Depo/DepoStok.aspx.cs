using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Depo_DepoStok : System.Web.UI.Page
{
    private int depoID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            var master = this.Master as fabrika_FabrikaMasterPage;
            if (master != null)
            {
                master.KlasorAdi = "Depo";
                master.SayfaAdi = "Depo Stok Listesi";
            }

            // QueryString'den depo ID'sini al
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out depoID))
            {
                if (!IsPostBack)
                {
                    DepoAdiniBelirle();
                    DepoStokListele();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch
        {
            Response.Redirect("~/fabrika/Default.aspx");
        }
    }

    private void DepoAdiniBelirle()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoID && x.SirketID == sirketID);
            if (depo != null)
            {
                lblDepoAdi.Text = depo.DepoAdi + " - Stok Listesi";
            }
        }
        catch
        {
            lblDepoAdi.Text = "Depo Stok Listesi";
        }
    }

    private void DepoStokListele()
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Stok durumu filtresini al
            string stokDurumu = ddlStokDurumu.SelectedValue;

            // Arama metnini al
            string aramaMetni = txtArama.Text.Trim();

            // Sorguyu oluştur - önce temel sorgu
            var baseQuery = db.DepoStoks
                .Where(x => x.DepoID == depoID && x.SirketID == sirketID);

            // Stok durumu filtresi uygula
            if (!string.IsNullOrEmpty(stokDurumu))
            {
                switch (stokDurumu)
                {
                    case "1": // Stokta var
                        baseQuery = baseQuery.Where(x => x.Miktar > 0);
                        break;
                    case "0": // Stok yok
                        baseQuery = baseQuery.Where(x => x.Miktar <= 0);
                        break;
                    case "2": // Minimum stok altında
                        baseQuery = baseQuery.Where(x => x.Miktar <= (x.MinimumMiktar ?? 0) && (x.MinimumMiktar ?? 0) > 0);
                        break;
                }
            }

            // Arama filtresi varsa uygula
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                baseQuery = baseQuery.Where(x =>
                    x.Urunler.UrunAdi.Contains(aramaMetni) ||
                    (x.Urunler.UrunKodu != null && x.Urunler.UrunKodu.Contains(aramaMetni)));
            }

            // Son projection ve sıralama
            var stokListesi = baseQuery
                .Select(x => new
                {
                    x.DepoStokID,
                    x.UrunID,
                    UrunAdi = x.Urunler.UrunAdi,
                    UrunKodu = x.Urunler.UrunKodu ?? "",
                    BirimAdi = x.Urunler.Birimler != null ? x.Urunler.Birimler.BirimAdi : "",
                    x.Miktar,
                    x.MinimumMiktar,
                    x.SonGuncellemeTarihi
                })
                .OrderBy(x => x.UrunAdi)
                .ToList();

            rptDepoStok.DataSource = stokListesi;
            rptDepoStok.DataBind();
            lblToplamUrun.Text = stokListesi.Count.ToString();
        }
        catch (Exception ex)
        {
            rptDepoStok.DataSource = null;
            rptDepoStok.DataBind();
            lblToplamUrun.Text = "0";
        }
    }

    protected string GetRowClass(decimal miktar, decimal minimumMiktar)
    {
        if (miktar <= 0)
            return "table-danger"; // Stok yok
        else if (minimumMiktar > 0 && miktar <= minimumMiktar)
            return "table-warning"; // Minimum stok altında
        else
            return ""; // Normal
    }

    protected string GetStokDurumBadgeClass(decimal miktar, decimal minimumMiktar)
    {
        if (miktar <= 0)
            return "badge-danger";
        else if (minimumMiktar > 0 && miktar <= minimumMiktar)
            return "badge-warning";
        else
            return "badge-success";
    }

    protected string GetStokDurumText(decimal miktar, decimal minimumMiktar)
    {
        if (miktar <= 0)
            return "Stok Yok";
        else if (minimumMiktar > 0 && miktar <= minimumMiktar)
            return "Kritik Seviye";
        else
            return "Normal";
    }

    protected void ddlStokDurumu_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepoStokListele();
    }

    protected void txtArama_TextChanged(object sender, EventArgs e)
    {
        DepoStokListele();
    }

    protected void rptDepoStok_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Hareketler")
        {
            int urunID = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("StokHareketleri.aspx?depoID=" + depoID + "&urunID=" + urunID);
        }
        else if (e.CommandName == "Guncelle")
        {
            int depoStokID = Convert.ToInt32(e.CommandArgument);
            
            // Stok bilgilerini yükle
            try
            {
                int sirketID = SessionHelper.GetSirketID();
                FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                
                var depoStok = db.DepoStoks.FirstOrDefault(x => x.DepoStokID == depoStokID && x.SirketID == sirketID);
                if (depoStok != null)
                {
                    hdnDepoStokID.Value = depoStokID.ToString();
                    txtYeniMiktar.Text = depoStok.Miktar.ToString("0.##");
                    txtMinimumStok.Text = (depoStok.MinimumMiktar ?? 0).ToString("0.##");
                    txtAciklama.Text = "";
                    
                    // Modalı aç
                    ScriptManager.RegisterStartupScript(this, GetType(), "showModal", 
                        "$('#stokGuncelleModal').modal('show');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "alert('Stok bilgileri yüklenirken hata oluştu: " + ex.Message + "');", true);
            }
        }
    }

    protected void btnGeriDon_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepoDetay.aspx?id=" + depoID);
    }

    protected void btnStokEkle_Click(object sender, EventArgs e)
    {
        Response.Redirect("StokEkle.aspx?depoID=" + depoID);
    }

    protected void btnStokTransfer_Click(object sender, EventArgs e)
    {
        Response.Redirect("StokTransfer.aspx?depoID=" + depoID);
    }

    protected void btnStokGuncelle_Click(object sender, EventArgs e)
    {
        try
        {
            int depoStokID = Convert.ToInt32(hdnDepoStokID.Value);
            decimal yeniMiktar = Convert.ToDecimal(txtYeniMiktar.Text);
            decimal minimumStok = 0;
            
            if (!string.IsNullOrEmpty(txtMinimumStok.Text))
                minimumStok = Convert.ToDecimal(txtMinimumStok.Text);

            int sirketID = SessionHelper.GetSirketID();
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var depoStok = db.DepoStoks.FirstOrDefault(x => x.DepoStokID == depoStokID && x.SirketID == sirketID);
            
            if (depoStok != null)
            {
                decimal eskiMiktar = depoStok.Miktar;
                decimal fark = yeniMiktar - eskiMiktar;

                // Stok güncelle
                depoStok.Miktar = yeniMiktar;
                depoStok.MinimumMiktar = minimumStok;
                depoStok.SonGuncellemeTarihi = DateTime.Now;

                // Stok hareketi ekle
                var hareket = new StokHareketleri
                {
                    SirketID = sirketID,
                    IslemTarihi = DateTime.Now,
                    HareketTipi = fark > 0 ? "GIRIS" : "CIKIS",
                    DepoID = depoStok.DepoID,
                    UrunID = depoStok.UrunID,
                    Miktar = Math.Abs(fark),
                    ReferansNo = "MANUEL-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ReferansTipi = "MANUEL",
                    Aciklama = txtAciklama.Text.Trim()
                };

                db.StokHareketleris.InsertOnSubmit(hareket);

                // Depo dolu miktarını güncelle
                var depo = db.Depolars.FirstOrDefault(x => x.DepoID == depoStok.DepoID);
                if (depo != null)
                {
                    depo.DoluMiktar = db.DepoStoks.Where(x => x.DepoID == depoStok.DepoID).Sum(x => x.Miktar);
                }

                db.SubmitChanges();

                // Modal'ı temizle
                txtYeniMiktar.Text = "";
                txtMinimumStok.Text = "";
                txtAciklama.Text = "";

                // Listeyi yenile
                DepoStokListele();

                // Başarı mesajı
                ScriptManager.RegisterStartupScript(this, this.GetType(), "success", 
                    "alert('Stok başarıyla güncellendi.'); $('#stokGuncelleModal').modal('hide');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", 
                "alert('Stok güncellenirken hata oluştu: " + ex.Message + "');", true);
        }
    }
}