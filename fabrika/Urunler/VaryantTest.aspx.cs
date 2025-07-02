using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fabrika_Urunler_VaryantTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            VaryantTurleriniYukle();
        }
    }

    private void VaryantTurleriniYukle()
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                var varyantTurleri = from vt in db.VaryantTurleris
                                     where vt.SirketID == sirketID
                                     select new { vt.VaryantTurID, vt.TurAdi };

                ddlTestVaryantTuru.DataSource = varyantTurleri.ToList();
                ddlTestVaryantTuru.DataTextField = "TurAdi";
                ddlTestVaryantTuru.DataValueField = "VaryantTurID";
                ddlTestVaryantTuru.DataBind();
                ddlTestVaryantTuru.Items.Insert(0, new ListItem("Varyant Türü Seçin", "0"));
            }
        }
        catch (Exception ex)
        {
            litVaryantTurleri.Text = "<span style='color:red'>Hata: " + ex.Message + "</span>";
        }
    }

    protected void btnTestVaryantTurleri_Click(object sender, EventArgs e)
    {
        try
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                int sirketID = SessionHelper.GetSirketID();
                var varyantTurleri = from vt in db.VaryantTurleris
                                     where vt.SirketID == sirketID
                                     select vt;

                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>Şirket ID:</strong> " + sirketID + "<br/>");
                sb.Append("<strong>Toplam Varyant Türü:</strong> " + varyantTurleri.Count() + "<br/><br/>");

                if (varyantTurleri.Any())
                {
                    sb.Append("<table border='1' cellpadding='5'>");
                    sb.Append("<tr><th>VaryantTurID</th><th>TurAdi</th><th>SirketID</th></tr>");
                    
                    foreach (var vt in varyantTurleri)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + vt.VaryantTurID + "</td>");
                        sb.Append("<td>" + (vt.TurAdi ?? "NULL") + "</td>");
                        sb.Append("<td>" + vt.SirketID + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }
                else
                {
                    sb.Append("<span style='color:orange'>Bu şirket için varyant türü bulunamadı!</span>");
                }

                litVaryantTurleri.Text = sb.ToString();
            }
        }
        catch (Exception ex)
        {
            litVaryantTurleri.Text = "<span style='color:red'>Hata: " + ex.Message + "</span>";
        }
    }

    protected void ddlTestVaryantTuru_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTestVaryantTuru.SelectedValue != "0")
        {
            try
            {
                int varyantTurID = Convert.ToInt32(ddlTestVaryantTuru.SelectedValue);
                
                using (var db = new FabrikaDataClassesDataContext())
                {
                    int sirketID = SessionHelper.GetSirketID();
                    var varyantDegerleri = from vd in db.VaryantDegerleris
                                           where vd.SirketID == sirketID && vd.VaryantTurID == varyantTurID
                                           select vd;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<strong>Seçilen Varyant Tür ID:</strong> " + varyantTurID + "<br/>");
                    sb.Append("<strong>Toplam Varyant Değeri:</strong> " + varyantDegerleri.Count() + "<br/><br/>");

                    if (varyantDegerleri.Any())
                    {
                        sb.Append("<table border='1' cellpadding='5'>");
                        sb.Append("<tr><th>VaryantDegerID</th><th>DegerAdi</th><th>VaryantTurID</th><th>SirketID</th></tr>");
                        
                        foreach (var vd in varyantDegerleri)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + vd.VaryantDegerID + "</td>");
                            sb.Append("<td>" + (vd.DegerAdi ?? "NULL") + "</td>");
                            sb.Append("<td>" + vd.VaryantTurID + "</td>");
                            sb.Append("<td>" + vd.SirketID + "</td>");
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                    }
                    else
                    {
                        sb.Append("<span style='color:orange'>Bu varyant türü için değer bulunamadı!</span>");
                    }

                    litVaryantDegerleri.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                litVaryantDegerleri.Text = "<span style='color:red'>Hata: " + ex.Message + "</span>";
            }
        }
        else
        {
            litVaryantDegerleri.Text = "";
        }
    }

    protected void btnTestSirket_Click(object sender, EventArgs e)
    {
        try
        {
            int sirketID = SessionHelper.GetSirketID();
            
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Şirket bilgilerini kontrol et
                var sirket = db.Sirketlers.FirstOrDefault(s => s.SirketID == sirketID);
                
                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>Session'dan Alınan Şirket ID:</strong> " + sirketID + "<br/>");
                
                if (sirket != null)
                {
                    sb.Append("<strong>Şirket Adı:</strong> " + (sirket.SirketAdi ?? "NULL") + "<br/>");
                    sb.Append("<strong>Şirket ID:</strong> " + sirket.SirketID + "<br/>");
                    // Durum alanı mevcut değil, kaldırıldı
                }
                else
                {
                    sb.Append("<span style='color:red'>Bu ID'ye sahip şirket bulunamadı!</span><br/>");
                }
                
                // Varyant tabloları sayılarını kontrol et
                var varyantTurSayisi = db.VaryantTurleris.Count(vt => vt.SirketID == sirketID);
                var varyantDegerSayisi = db.VaryantDegerleris.Count(vd => vd.SirketID == sirketID);
                
                sb.Append("<br/><strong>Bu şirket için:</strong><br/>");
                sb.Append("- Varyant Türü Sayısı: " + varyantTurSayisi + "<br/>");
                sb.Append("- Varyant Değeri Sayısı: " + varyantDegerSayisi + "<br/>");
                
                litSirketBilgi.Text = sb.ToString();
            }
        }
        catch (Exception ex)
        {
            litSirketBilgi.Text = "<span style='color:red'>Hata: " + ex.Message + "</span>";
        }
    }
} 