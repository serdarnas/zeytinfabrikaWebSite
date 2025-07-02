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
        int sirketID = SessionHelper.GetSirketID();
        
        Musteriler gelenMusteriler = db.Musterilers.FirstOrDefault(x => x.MusteriID == _MusteriID && x.SirketID == sirketID);
        
        if (gelenMusteriler == null)
        {
            Response.Redirect("MusteriListesi.aspx");
            return;
        }
        
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
        hplinkTumSatislar.NavigateUrl = "MusteriSatisListesi.aspx?id=" + gelenMusteriler.MusteriID;
        
        // Verileri yükle
        LoadSatislar(_MusteriID, sirketID);
        LoadOdemeler(_MusteriID, sirketID);
        LoadSenetler(_MusteriID, sirketID);
        LoadBakiyeBilgisi(_MusteriID, sirketID);
    }
    
    private void LoadSatislar(int musteriID, int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        
        // Son 10 satışı getir
        var satislar = db.Satislars
            .Where(s => s.MusteriID == musteriID && s.SirketID == sirketID)
            .OrderByDescending(s => s.SatisTarihi)
            .Take(10)
            .Select(s => new
            {
                s.SatisTarihi,
                s.SatisBelgeNo,
                s.SatisTipi,
                s.GenelToplam,
                s.SatisID
            })
            .ToList();
            
        // HTML tablosunu oluştur
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        if (satislar.Any())
        {
            foreach (var satis in satislar)
            {
                string durum = "";
                string durumClass = "";
                
                switch (satis.SatisTipi)
                {
                    case "Fatura":
                        durum = "Faturalandı";
                        durumClass = "bg-success";
                        break;
                    case "İrsaliye":
                        durum = "İrsaliye";
                        durumClass = "bg-info";
                        break;
                    case "Proforma":
                        durum = "Proforma";
                        durumClass = "bg-warning";
                        break;
                    default:
                        durum = satis.SatisTipi ?? "Bilinmiyor";
                        durumClass = "bg-secondary";
                        break;
                }
                
                sb.AppendFormat(@"
                    <tr style=""cursor: pointer;"" onclick=""window.location.href='MusteriSatis.aspx?id={5}'"">
                        <td>{0}</td>
                        <td>{1}</td>
                        <td><span class=""badge {2}"">{3}</span></td>
                        <td>{4:C2}</td>
                    </tr>",
                    satis.SatisTarihi.ToString("dd.MM.yyyy"),
                    satis.SatisBelgeNo,
                    durumClass,
                    durum,
                    satis.GenelToplam,
                    musteriID
                );
            }
        }
        else
        {
            sb.Append(@"
                <tr>
                    <td colspan=""4"" class=""text-center text-muted"">Henüz satış yapılmamış</td>
                </tr>");
        }
        
        // JavaScript ile tabloyu güncelle
        string script = string.Format(@"
            document.addEventListener('DOMContentLoaded', function() {{
                var tbody = document.querySelector('.table-satislar tbody');
                if (tbody) {{
                    tbody.innerHTML = '{0}';
                }}
            }});", sb.ToString().Replace("'", "\\'").Replace("\r\n", ""));
        
        ClientScript.RegisterStartupScript(this.GetType(), "LoadSatislar", script, true);
    }
    
    private void LoadOdemeler(int musteriID, int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        
        // Son 10 ödemeyi getir (nakit + çek)
        var nakitOdemeler = db.NakitIslemlers
            .Where(n => n.MusteriID == musteriID && n.SirketID == sirketID && n.IslemTuru == 'T')
            .OrderByDescending(n => n.OlusturmaTarihi)
            .Take(5)
            .Select(n => new {
                Tarih = n.OlusturmaTarihi,
                Tutar = n.Tutar,
                Tip = "Nakit",
                Aciklama = n.Aciklama ?? ""
            });
            
        var cekOdemeler = db.Ceklers
            .Where(c => c.AlinanMusteriID == musteriID && c.SirketID == sirketID)
            .OrderByDescending(c => c.OlusturmaTarihi)
            .Take(5)
            .Select(c => new {
                Tarih = c.OlusturmaTarihi,
                Tutar = c.Tutar,
                Tip = "Çek",
                Aciklama = c.SeriNo ?? ""
            });
            
        var tumOdemeler = nakitOdemeler.Union(cekOdemeler)
            .OrderByDescending(o => o.Tarih)
            .Take(10)
            .ToList();
        
        // HTML tablosunu oluştur
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        if (tumOdemeler.Any())
        {
            foreach (var odeme in tumOdemeler)
            {
                sb.AppendFormat(@"
                    <tr>
                        <td>{0}</td>
                        <td>{1:C2}</td>
                        <td><span class=""badge {2}"">{3}</span></td>
                    </tr>",
                    odeme.Tarih != null ? odeme.Tarih.Value.ToString("dd.MM.yyyy") : "-",
                    odeme.Tutar,
                    odeme.Tip == "Nakit" ? "bg-success" : "bg-info",
                    odeme.Tip
                );
            }
        }
        else
        {
            sb.Append(@"
                <tr>
                    <td colspan=""3"" class=""text-center text-muted"">Henüz ödeme yapılmamış</td>
                </tr>");
        }
        
        // JavaScript ile tabloyu güncelle
        string script = string.Format(@"
            document.addEventListener('DOMContentLoaded', function() {{
                var tbody = document.querySelector('.table-odemeler tbody');
                if (tbody) {{
                    tbody.innerHTML = '{0}';
                }}
            }});", sb.ToString().Replace("'", "\\'").Replace("\r\n", ""));
        
        ClientScript.RegisterStartupScript(this.GetType(), "LoadOdemeler", script, true);
    }
    
    private void LoadSenetler(int musteriID, int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        
        // Son senetleri getir
        var senetler = db.Senetlers
            .Where(s => s.IlgiliMusteriID == musteriID && s.SirketID == sirketID)
            .OrderByDescending(s => s.OlusturmaTarihi)
            .Take(10)
            .Select(s => new {
                s.SeriNo,
                s.SenetTipi,
                s.Tutar,
                s.VadeTarihi,
                s.DurumID
            })
            .ToList();
        
        // HTML tablosunu oluştur
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        if (senetler.Any())
        {
            foreach (var senet in senetler)
            {
                string tip = senet.SenetTipi == 'A' ? "Alınan" : "Verilen";
                string tipClass = senet.SenetTipi == 'A' ? "bg-success" : "bg-warning";
                
                sb.AppendFormat(@"
                    <tr>
                        <td>{0}</td>
                        <td>{1}</td>
                        <td><span class=""badge {2}"">{3}</span></td>
                        <td>{4}</td>
                        <td>{5:C2}</td>
                    </tr>",
                    senet.VadeTarihi.ToString("dd.MM.yyyy"),
                    senet.SeriNo != null ? senet.SeriNo : "-",
                    tipClass,
                    tip,
                    senet.DurumID == 1 ? "Aktif" : "Pasif",
                    senet.Tutar
                );
            }
        }
        else
        {
            sb.Append(@"
                <tr>
                    <td colspan=""5"" class=""text-center text-muted"">Kayıt bulunamadı</td>
                </tr>");
        }
        
        // JavaScript ile tabloyu güncelle
        string script = string.Format(@"
            document.addEventListener('DOMContentLoaded', function() {{
                var tbody = document.querySelector('.table-senetler tbody');
                if (tbody) {{
                    tbody.innerHTML = '{0}';
                }}
            }});", sb.ToString().Replace("'", "\\'").Replace("\r\n", ""));
        
        ClientScript.RegisterStartupScript(this.GetType(), "LoadSenetler", script, true);
    }
    
    private void LoadBakiyeBilgisi(int musteriID, int sirketID)
    {
        FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
        
        try
        {
            // Satışlardan alacak
            var satisAlacakQuery = db.Satislars
                .Where(s => s.MusteriID == musteriID && s.SirketID == sirketID)
                .Sum(s => (decimal?)s.ToplamTutar);
            var satisAlacak = satisAlacakQuery.HasValue ? satisAlacakQuery.Value : 0;

            // Tahsilatlardan düşülecek
            var tahsilatlarQuery = db.NakitIslemlers
                .Where(n => n.MusteriID == musteriID && n.SirketID == sirketID && n.IslemTuru == 'T')
                .Sum(n => (decimal?)n.Tutar);
            var tahsilatlar = tahsilatlarQuery.HasValue ? tahsilatlarQuery.Value : 0;

            // Çeklerden düşülecek
            var ceklerQuery = db.Ceklers
                .Where(c => c.AlinanMusteriID == musteriID && c.SirketID == sirketID)
                .Sum(c => (decimal?)c.Tutar);
            var cekler = ceklerQuery.HasValue ? ceklerQuery.Value : 0;

            // Alınan senetlerden düşülecek
            var alinanSenetlerQuery = db.Senetlers
                .Where(s => s.IlgiliMusteriID == musteriID && s.SirketID == sirketID && s.SenetTipi == 'A')
                .Sum(s => (decimal?)s.Tutar);
            var alinanSenetler = alinanSenetlerQuery.HasValue ? alinanSenetlerQuery.Value : 0;

            // Verilen senetler borç olarak eklenir
            var verilenSenetlerQuery = db.Senetlers
                .Where(s => s.IlgiliMusteriID == musteriID && s.SirketID == sirketID && s.SenetTipi == 'V')
                .Sum(s => (decimal?)s.Tutar);
            var verilenSenetler = verilenSenetlerQuery.HasValue ? verilenSenetlerQuery.Value : 0;

            decimal bakiye = satisAlacak - tahsilatlar - cekler - alinanSenetler + verilenSenetler;
            
            // Bakiye bilgisini güncelle
            string bakiyeClass = bakiye >= 0 ? "text-success" : "text-danger";
            string bakiyeText = bakiye >= 0 ? "Alacak" : "Borç";
            
            string script = string.Format(@"
                document.addEventListener('DOMContentLoaded', function() {{
                    var bakiyeElement = document.querySelector('#bakiyeBilgisi');
                    if (bakiyeElement) {{
                        bakiyeElement.innerHTML = '<h4 class=""{0}"">{1:C2}</h4><small class=""text-muted"">{2}</small>';
                    }}
                }});", bakiyeClass, Math.Abs(bakiye), bakiyeText);
            
            ClientScript.RegisterStartupScript(this.GetType(), "LoadBakiye", script, true);
        }
        catch
        {
            // Hata durumunda varsayılan değer
            string script = @"
                document.addEventListener('DOMContentLoaded', function() {
                    var bakiyeElement = document.querySelector('#bakiyeBilgisi');
                    if (bakiyeElement) {
                        bakiyeElement.innerHTML = '<h4 class=""text-muted"">0,00 ₺</h4><small class=""text-muted"">Hesaplanamıyor</small>';
                    }
                });"; 
            ClientScript.RegisterStartupScript(this.GetType(), "LoadBakiyeError", script, true);
        }
    }
}