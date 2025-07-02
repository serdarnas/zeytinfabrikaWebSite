using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Mobil_MalKabul : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Ürün listesini her zaman yükle (PostBack'lerde de gerekli)
        UrunleriYukle();
        
        if (!IsPostBack)
        {
            ZeytinBoxKontrolVeOlustur();
            
            // URL parametrelerini kontrol et
            URLParametreleriniKontrolEt();
        }
        else
        {
            // PostBack durumunda geçici form verilerini geri yükle
            FormVerileriniGeriYukle();
            
            // PostBack'de de mustahsilID kontrolü yap (form verisi korunması için)
            PostBackMustahsilIDKontrol();
        }
    }
    
    private void URLParametreleriniKontrolEt()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== URL PARAMETRELERİ KONTROL EDİLİYOR ===");
            
            // Öncelik sırası: uretimID > mustahsilID
            
            // 1. Düzenleme modu kontrolü (uretimID)
            string uretimIDParam = Request.QueryString["uretimID"];
            if (!string.IsNullOrEmpty(uretimIDParam))
            {
                System.Diagnostics.Debug.WriteLine("uretimID parametresi bulundu: " + uretimIDParam);
                int uretimID;
                if (int.TryParse(uretimIDParam, out uretimID))
                {
                    System.Diagnostics.Debug.WriteLine("Düzenleme moduna geçiliyor...");
                    DuzenlemeModunaGec(uretimID);
                    return; // Düzenleme modu aktifse diğer kontrolleri yapma
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("uretimID parse edilemedi: " + uretimIDParam);
                }
            }
            
            // 2. Müstahsil otomatik seçim kontrolü (mustahsilID)
            string mustahsilIDParam = Request.QueryString["mustahsilID"];
            if (!string.IsNullOrEmpty(mustahsilIDParam))
            {
                System.Diagnostics.Debug.WriteLine("mustahsilID parametresi bulundu: " + mustahsilIDParam);
                int mustahsilID;
                if (int.TryParse(mustahsilIDParam, out mustahsilID))
                {
                    System.Diagnostics.Debug.WriteLine("Müstahsil otomatik seçim yapılıyor...");
                    MustahsilOtomatikSecimYap(mustahsilID);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("mustahsilID parse edilemedi: " + mustahsilIDParam);
                }
            }
            
            if (string.IsNullOrEmpty(uretimIDParam) && string.IsNullOrEmpty(mustahsilIDParam))
            {
                System.Diagnostics.Debug.WriteLine("URL parametresi yok - normal yeni kayıt modu");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("URLParametreleriniKontrolEt hatası: " + ex.Message);
        }
    }
    
    private void MustahsilOtomatikSecimYap(int mustahsilID)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== MUSTAHSİL OTOMATİK SEÇİM BAŞLADI ===");
            System.Diagnostics.Debug.WriteLine("Seçilecek mustahsilID: " + mustahsilID);
            
            if (Session["SirketID"] == null)
            {
                System.Diagnostics.Debug.WriteLine("HATA: Session SirketID null");
                return;
            }

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Müstahsil bilgilerini al ve doğrula
            var mustahsil = db.Mustahsillers
                .Where(x => x.MustahsilID == mustahsilID && x.SirketID == sirketID)
                .FirstOrDefault();

            if (mustahsil != null)
            {
                System.Diagnostics.Debug.WriteLine("Müstahsil bulundu: " + mustahsil.Ad + " " + mustahsil.Soyad);
                
                // Hidden field'a müstahsil ID'sini set et
                hdnSelectedMustahsilID.Value = mustahsilID.ToString();
                
                // Session'a da kaydet (PostBack koruması için)
                Session["AutoSelectedMustahsilID"] = mustahsilID;
                Session["TempMustahsilID"] = mustahsilID.ToString(); // Form verileri için de kaydet
                Session["AutoSelectedMustahsilInfo"] = mustahsil.Ad + " " + mustahsil.Soyad + "|" + 
                                                      (mustahsil.Telefon ?? "Telefon yok") + "|" + 
                                                      (mustahsil.Email ?? "Email yok");

                // JavaScript ile müstahsil seçimini göster
                MustahsilOtomatikSecimJavaScript(mustahsil);
                
                System.Diagnostics.Debug.WriteLine("Müstahsil otomatik seçimi tamamlandı");
                
                // Formu göndermeden önce PartiNo oluştur
                if (string.IsNullOrEmpty(lblPartiNo.Text) || lblPartiNo.Text == "Müştahsil seçildikten sonra oluşturulacak")
                {
                    PartiNoOlustur();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("HATA: mustahsilID=" + mustahsilID + " bulunamadı veya farklı şirkete ait");
                MesajGoster("Belirtilen müştahsil bulunamadı!", false);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("MustahsilOtomatikSecimYap hatası: " + ex.Message);
            MesajGoster("Müştahsil seçimi sırasında hata: " + ex.Message, false);
        }
    }
    
    private void MustahsilOtomatikSecimJavaScript(Mustahsiller mustahsil)
    {
        try
        {
            string adSoyad = mustahsil.Ad + " " + mustahsil.Soyad;
            string telefon = mustahsil.Telefon ?? "Telefon yok";
            string email = mustahsil.Email ?? "Email yok";

            string script = string.Format(@"
                document.addEventListener('DOMContentLoaded', function() {{
                    console.log('Müştahsil otomatik seçim JavaScript çalışıyor...');
                    
                    // Müştahsil seçimini göster
                    var secilenBilgiDiv = document.getElementById('secilenMustahsilBilgi');
                    var secilenDiv = document.getElementById('secilenMustahsil');
                    
                    if (secilenBilgiDiv && secilenDiv) {{
                        secilenBilgiDiv.innerHTML = '<strong>{0}</strong><br><small>{1} | {2}</small>';
                        secilenDiv.style.display = 'block';
                        console.log('Müştahsil bilgisi gösterildi: {0}');
                    }}
                    
                    // Parti numarası oluştur
                    if (typeof PageMethods !== 'undefined' && typeof PageMethods.PartiNoOlustur !== 'undefined') {{
                        PageMethods.PartiNoOlustur(function(result) {{
                            var partiDiv = document.querySelector('.parti-display');
                            if (partiDiv) {{
                                partiDiv.innerHTML = result;
                                console.log('Parti numarası oluşturuldu: ' + result);
                            }}
                        }}, function(error) {{
                            console.error('Parti numarası oluşturma hatası:', error);
                        }});
                    }}
                    
                    // Bilgi mesajı göster
                    showAutoSelectMessage('{0}');
                }});
                
                function showAutoSelectMessage(adSoyad) {{
                    var mesajDiv = document.createElement('div');
                    mesajDiv.className = 'alert alert-success';
                    mesajDiv.style.marginBottom = '20px';
                    mesajDiv.innerHTML = '<i class=""fas fa-check-circle""></i> ' + adSoyad + ' otomatik olarak seçildi.';
                    
                    var container = document.querySelector('.container-fluid');
                    if (container && container.firstChild) {{
                        container.insertBefore(mesajDiv, container.firstChild);
                        
                        // 5 saniye sonra mesajı gizle
                        setTimeout(function() {{
                            if (mesajDiv.parentNode) {{
                                mesajDiv.parentNode.removeChild(mesajDiv);
                            }}
                        }}, 5000);
                    }}
                }}
            ", 
            adSoyad.Replace("'", "\\'"), 
            telefon.Replace("'", "\\'"), 
            email.Replace("'", "\\'"));

            ClientScript.RegisterStartupScript(this.GetType(), "AutoSelectMustahsil", script, true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("MustahsilOtomatikSecimJavaScript hatası: " + ex.Message);
        }
    }
    
    private void PostBackMustahsilIDKontrol()
    {
        try
        {
            // PostBack sırasında mustahsilID parametresi hala mevcut mu kontrol et
            string mustahsilIDParam = Request.QueryString["mustahsilID"];
            
            // 1. URL'den gelen mustahsilID varsa ve Session bilgileri doluysa
            if (!string.IsNullOrEmpty(mustahsilIDParam) && 
                Session["AutoSelectedMustahsilInfo"] != null)
            {
                System.Diagnostics.Debug.WriteLine("PostBack'de mustahsilID korunuyor (URL): " + mustahsilIDParam);
                
                // TempMustahsilID olarak da kaydet (sonraki form submit için)
                if (Session["TempMustahsilID"] == null)
                {
                    Session["TempMustahsilID"] = mustahsilIDParam;
                }
                
                // Hidden field'a da set et
                if (string.IsNullOrEmpty(hdnSelectedMustahsilID.Value))
                {
                    hdnSelectedMustahsilID.Value = mustahsilIDParam;
                }
                
                // Session'dan müştahsil bilgilerini al
                string[] bilgiler = Session["AutoSelectedMustahsilInfo"].ToString().Split('|');
                if (bilgiler.Length >= 3)
                {
                    string adSoyad = bilgiler[0];
                    string telefon = bilgiler[1];
                    string email = bilgiler[2];
                    
                    // JavaScript ile seçimi tekrar göster
                    string script = string.Format(@"
                        document.addEventListener('DOMContentLoaded', function() {{
                            var secilenBilgiDiv = document.getElementById('secilenMustahsilBilgi');
                            var secilenDiv = document.getElementById('secilenMustahsil');
                            
                            if (secilenBilgiDiv && secilenDiv) {{
                                secilenBilgiDiv.innerHTML = '<strong>{0}</strong><br><small>{1} | {2}</small>';
                                secilenDiv.style.display = 'block';
                            }}
                            
                            // Parti numarası oluştur
                            if (typeof PageMethods !== 'undefined' && typeof PageMethods.PartiNoOlustur !== 'undefined') {{
                                PageMethods.PartiNoOlustur(function(result) {{
                                    var partiDiv = document.querySelector('.parti-display');
                                    if (partiDiv && (!partiDiv.innerHTML || partiDiv.innerHTML === 'Müştahsil seçildikten sonra oluşturulacak')) {{
                                        partiDiv.innerHTML = result;
                                        console.log('Parti numarası oluşturuldu: ' + result);
                                    }}
                                }}, function(error) {{
                                    console.error('Parti numarası oluşturma hatası:', error);
                                }});
                            }}
                        }});
                    ", adSoyad.Replace("'", "\\'"), telefon.Replace("'", "\\'"), email.Replace("'", "\\'"));
                    
                    ClientScript.RegisterStartupScript(this.GetType(), "RestoreMustahsilPostBack", script, true);
                }
            }
            // 2. Session'da TempMustahsilID varsa (form submit sonrası)
            else if (Session["TempMustahsilID"] != null && string.IsNullOrEmpty(hdnSelectedMustahsilID.Value))
            {
                System.Diagnostics.Debug.WriteLine("PostBack'de Session'dan mustahsilID korunuyor");
                
                string tempMustahsilID = Session["TempMustahsilID"].ToString();
                hdnSelectedMustahsilID.Value = tempMustahsilID;
                
                int mustahsilID;
                if (int.TryParse(tempMustahsilID, out mustahsilID))
                {
                    // Mustahsil bilgilerini yeniden yükle
                    int sirketID = Convert.ToInt32(Session["SirketID"]);
                    FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
                    
                    var mustahsil = db.Mustahsillers
                        .Where(x => x.MustahsilID == mustahsilID && x.SirketID == sirketID)
                        .FirstOrDefault();
                        
                    if (mustahsil != null)
                    {
                        // JavaScript ile seçimi tekrar göster
                        string script = string.Format(@"
                            document.addEventListener('DOMContentLoaded', function() {{
                                var secilenBilgiDiv = document.getElementById('secilenMustahsilBilgi');
                                var secilenDiv = document.getElementById('secilenMustahsil');
                                
                                if (secilenBilgiDiv && secilenDiv) {{
                                    secilenBilgiDiv.innerHTML = '<strong>{0}</strong><br><small>{1} | {2}</small>';
                                    secilenDiv.style.display = 'block';
                                }}
                            }});
                        ", 
                        (mustahsil.Ad + " " + mustahsil.Soyad).Replace("'", "\\'"), 
                        (mustahsil.Telefon ?? "Telefon yok").Replace("'", "\\'"), 
                        (mustahsil.Email ?? "Email yok").Replace("'", "\\'"));
                        
                        ClientScript.RegisterStartupScript(this.GetType(), "RestoreMustahsilFromSession", script, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("PostBackMustahsilIDKontrol hatası: " + ex.Message);
        }
    }

    private void ZeytinBoxKontrolVeOlustur()
    {
        try
        {
            if (Session["SirketID"] == null) return;

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Bu şirketin ZeytinBox'ı var mı kontrol et
            var mevcutBoxlar = db.ZeytinBoxKasalaris
                .Where(x => x.SirketID == sirketID)
                .Count();

            System.Diagnostics.Debug.WriteLine("Mevcut ZeytinBox sayısı: " + mevcutBoxlar);

            // Eğer ZeytinBox yoksa, varsayılan olarak 1000 tane oluştur
            if (mevcutBoxlar == 0)
            {
                for (int i = 1; i <= 1000; i++)
                {
                    ZeytinBoxKasalari yeniBox = new ZeytinBoxKasalari
                    {
                        SirketID = sirketID,
                        ZeytinBoxNo = i, // 1, 2, 3, 4, ..., 1000
                        Durumu = (i % 10 == 0) ? true : false // Her 10. box'ı dolu yap (test için)
                    };

                    db.ZeytinBoxKasalaris.InsertOnSubmit(yeniBox);
                }

                db.SubmitChanges();
                System.Diagnostics.Debug.WriteLine("1000 adet ZeytinBox oluşturuldu (100 tanesi dolu)");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("ZeytinBox oluşturma hatası: " + ex.Message);
        }
    }

    private void DuzenlemeModunaGec(int uretimID)
    {
        try
        {
            if (Session["SirketID"] == null) return;

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Üretim bilgilerini al (müştahsil bilgileri ile birlikte)
            var uretim = (from u in db.ZeytinyagiUretimleris
                         join m in db.Mustahsillers on u.MustahsilID equals m.MustahsilID
                         where u.ZeytinyagiUretimID == uretimID && u.SirketID == sirketID
                         select new
                         {
                             u.ZeytinyagiUretimID,
                             u.MustahsilID,
                             u.PartiNo,
                             u.PlakaNo,
                             u.GelisKg,
                             u.UrunID,
                             MustahsilAd = m.Ad + " " + m.Soyad,
                             MustahsilTelefon = m.Telefon ?? "Telefon yok",
                             MustahsilEmail = m.Email ?? "Email yok"
                         }).FirstOrDefault();

            if (uretim != null)
            {
                // ViewState'e üretim ID'sini kaydet (güncelleme için)
                ViewState["EditingUretimID"] = uretimID;

                // Formu doldur
                hdnSelectedMustahsilID.Value = uretim.MustahsilID.ToString();
                txtPlaka.Text = uretim.PlakaNo ?? "";
                txtGelisKg.Text = uretim.GelisKg != null ? uretim.GelisKg.ToString() : "";
                
                if (uretim.UrunID != null)
                {
                    ddlUrun.SelectedValue = uretim.UrunID.ToString();
                }

                // ZeytinBox'ları yükle
                var zeytinBoxlar = db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps
                    .Where(x => x.ZeytinyagiUretimID == uretimID)
                    .Select(x => x.ZeytinBoxKasaID)
                    .ToList();

                if (zeytinBoxlar.Any())
                {
                    hdnSecilenZeytinBoxlar.Value = Newtonsoft.Json.JsonConvert.SerializeObject(zeytinBoxlar);
                }

                // JavaScript ile formu doldur
                string script = string.Format(@"
                    document.addEventListener('DOMContentLoaded', function() {{
                        // Müştahsil seçimini göster
                        document.getElementById('secilenMustahsilBilgi').innerHTML = '<strong>{0}</strong><br><small>{1} | {2}</small>';
                        document.getElementById('secilenMustahsil').style.display = 'block';
                        
                        // Parti numarasını göster
                        document.querySelector('.parti-display').innerHTML = '{3}';
                        
                        // ZeytinBox'ları yükle
                        if (typeof loadExistingZeytinBoxes !== 'undefined') {{
                            loadExistingZeytinBoxes();
                        }}
                        
                        // Düzenleme modunu göster
                        showEditModeMessage('{0}', '{3}');
                        
                        // Kaydet butonunu güncelle
                        var saveBtn = document.querySelector('.btn-save');
                        if (saveBtn) {{
                            saveBtn.innerHTML = '<i class=""fas fa-save""></i> Güncellemeyi Kaydet';
                        }}
                        
                        // Başlığı güncelle
                        var header = document.querySelector('.form-header h4');
                        if (header) {{
                            header.innerHTML = 'Mal Kabul Güncelleme';
                        }}
                    }});
                    
                    function showEditModeMessage(mustahsilAd, partiNo) {{
                        var mesajDiv = document.createElement('div');
                        mesajDiv.className = 'alert alert-info';
                        mesajDiv.style.marginBottom = '20px';
                        mesajDiv.innerHTML = '<i class=""fas fa-edit""></i> ' + mustahsilAd + ' - ' + partiNo + ' düzenleniyor.';
                        
                        var container = document.querySelector('.container-fluid');
                        if (container && container.firstChild) {{
                            container.insertBefore(mesajDiv, container.firstChild);
                        }}
                    }}
                    
                    function loadExistingZeytinBoxes() {{
                        try {{
                            var existingBoxes = {4};
                            if (existingBoxes && existingBoxes.length > 0) {{
                                // ZeytinBox listesini API'den al ve göster
                                Promise.all(existingBoxes.map(function(boxID) {{
                                    return new Promise(function(resolve) {{
                                        PageMethods.GetZeytinBoxInfo(boxID, function(result) {{
                                            resolve(result);
                                        }}, function(error) {{
                                            resolve(null);
                                        }});
                                    }});
                                }})).then(function(results) {{
                                    secilenZeytinBoxlar = [];
                                    results.forEach(function(box) {{
                                        if (box) {{
                                            secilenZeytinBoxlar.push({{
                                                ZeytinBoxKasaID: box.ZeytinBoxKasaID,
                                                ZeytinBoxNo: box.ZeytinBoxNo,
                                                Status: 'Dolu'
                                            }});
                                        }}
                                    }});
                                    updateZeytinBoxGrid();
                                    updateHiddenField();
                                }});
                            }}
                        }} catch(e) {{
                            console.error('ZeytinBox yükleme hatası:', e);
                        }}
                    }}
                ", 
                uretim.MustahsilAd.Replace("'", "\\'"), 
                uretim.MustahsilTelefon.Replace("'", "\\'"), 
                uretim.MustahsilEmail.Replace("'", "\\'"),
                uretim.PartiNo.Replace("'", "\\'"),
                Newtonsoft.Json.JsonConvert.SerializeObject(zeytinBoxlar));

                ClientScript.RegisterStartupScript(this.GetType(), "EditMode", script, true);

                System.Diagnostics.Debug.WriteLine("Düzenleme modu aktif: " + uretim.PartiNo + " (ID: " + uretimID + ")");
            }
            else
            {
                MesajGoster("Düzenlenecek üretim kaydı bulunamadı!", false);
            }
        }
        catch (Exception ex)
        {
            MesajGoster("Düzenleme modu yüklenirken hata: " + ex.Message, false);
            System.Diagnostics.Debug.WriteLine("Düzenleme modu hatası: " + ex.Message);
        }
    }



    [WebMethod]
    public static List<object> MustahsilAra(string aramaMetni)
    {
        List<object> sonuclar = new List<object>();

        try
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context.Session["SirketID"] == null)
            {
                return sonuclar;
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var mustahsiller = db.Mustahsillers
                .Where(x => x.SirketID == sirketID &&
                       (string.IsNullOrEmpty(aramaMetni) ||
                        x.Ad.Contains(aramaMetni) ||
                        x.Soyad.Contains(aramaMetni) ||
                        x.Telefon.Contains(aramaMetni) ||
                        x.Email.Contains(aramaMetni)))
                .Take(50) // İlk 50 sonucu getir (boş arama için daha fazla)
                .Select(x => new
                {
                    x.MustahsilID,
                    AdSoyad = x.Ad + " " + x.Soyad,
                    Telefon = x.Telefon ?? "Telefon yok",
                    Email = x.Email ?? "Email yok"
                })
                .ToList();

            foreach (var mustahsil in mustahsiller)
            {
                sonuclar.Add(new
                {
                    MustahsilID = mustahsil.MustahsilID,
                    AdSoyad = mustahsil.AdSoyad,
                    Telefon = mustahsil.Telefon,
                    Email = mustahsil.Email
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Müştahsil arama hatası: " + ex.Message);
        }

        return sonuclar;
    }

    private void UrunleriYukle()
    {
        try
        {
            if (Session["SirketID"] == null) return;

            // PostBack durumunda mevcut seçimi koru
            string mevcutSecim = ddlUrun.SelectedValue;
            
            int sirketID = Convert.ToInt32(Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // YariManul = True olan ürünleri al
            var urunler = db.Urunlers
                .Where(x => x.SirketID == sirketID && x.YariManul == true)
                .OrderBy(x => x.UrunAdi)
                .Select(x => new { x.UrunID, x.UrunAdi })
                .ToList();

            System.Diagnostics.Debug.WriteLine("UrunleriYukle: " + urunler.Count + " ürün bulundu (YariManul=true)");
            
            // Eğer YariManul=true ürün yoksa, tüm ürünleri al
            if (urunler.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("YariManul=true ürün yok, tüm ürünleri alıyorum...");
                urunler = db.Urunlers
                    .Where(x => x.SirketID == sirketID)
                    .OrderBy(x => x.UrunAdi)
                    .Select(x => new { x.UrunID, x.UrunAdi })
                    .ToList();
                System.Diagnostics.Debug.WriteLine("UrunleriYukle: " + urunler.Count + " ürün bulundu (tümü)");
            }

            ddlUrun.DataSource = urunler;
            ddlUrun.DataTextField = "UrunAdi";
            ddlUrun.DataValueField = "UrunID";
            ddlUrun.DataBind();

            ddlUrun.Items.Insert(0, new ListItem("-- Ürün Seçin --", ""));

            // PostBack durumunda seçimi geri yükle
            if (IsPostBack && !string.IsNullOrEmpty(mevcutSecim))
            {
                try
                {
                    ddlUrun.SelectedValue = mevcutSecim;
                    System.Diagnostics.Debug.WriteLine("UrunleriYukle: Mevcut seçim geri yüklendi: " + mevcutSecim);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine("UrunleriYukle: Seçim geri yükleme hatası: " + ex2.Message);
                }
            }
            
            // Session'dan da kontrol et
            if (IsPostBack && Session["TempUrunID"] != null && string.IsNullOrEmpty(ddlUrun.SelectedValue))
            {
                try
                {
                    string sessionUrunID = Session["TempUrunID"].ToString();
                    ddlUrun.SelectedValue = sessionUrunID;
                    System.Diagnostics.Debug.WriteLine("UrunleriYukle: Session'dan seçim geri yüklendi: " + sessionUrunID);
                }
                catch (Exception ex3)
                {
                    System.Diagnostics.Debug.WriteLine("UrunleriYukle: Session seçim geri yükleme hatası: " + ex3.Message);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("UrunleriYukle genel hatası: " + ex.Message);
            MesajGoster("Ürünler yüklenirken hata: " + ex.Message, false);
        }
    }

    [WebMethod]
    public static string PartiNoOlustur()
    {
        try
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context.Session["SirketID"] == null) return "Hata: Session bulunamadı";

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Son ZeytinyagiUretimID'yi al
            var sonUretim = db.ZeytinyagiUretimleris
                .Where(x => x.SirketID == sirketID)
                .OrderByDescending(x => x.ZeytinyagiUretimID)
                .FirstOrDefault();

            int sonID = sonUretim != null ? sonUretim.ZeytinyagiUretimID : 0;
            int yeniID = sonID + 1;

            // P + Yıl + Ay + Sıra numarası formatı
            DateTime simdi = DateTime.Now;
            string partiNo = string.Format("P{0}{1:D2}{2:D3}",
                simdi.Year,
                simdi.Month,
                yeniID);

            return partiNo;
        }
        catch (Exception ex)
        {
            return "Hata: " + ex.Message;
        }
    }

    [WebMethod]
    public static List<object> ZeytinBoxAra(string aramaMetni)
    {
        List<object> sonuclar = new List<object>();

        try
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            System.Diagnostics.Debug.WriteLine("ZeytinBoxAra başladı - Arama metni: " + aramaMetni);

            if (context.Session["SirketID"] == null)
            {
                System.Diagnostics.Debug.WriteLine("Session SirketID null");
                return sonuclar;
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);
            System.Diagnostics.Debug.WriteLine("SirketID: " + sirketID);

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Önce tüm ZeytinBox'ları kontrol et
            var tumBoxlar = db.ZeytinBoxKasalaris
                .Where(x => x.SirketID == sirketID)
                .ToList();

            System.Diagnostics.Debug.WriteLine("Toplam ZeytinBox sayısı: " + tumBoxlar.Count);

            // Müsait ZeytinBox'ları ara (Durumu true VE KulananMustahsilID null olanlar)
            var musaitBoxlar = tumBoxlar
                .Where(x => x.Durumu == true && x.KulananMustahsilID == null)
                .ToList();

            System.Diagnostics.Debug.WriteLine("Müsait ZeytinBox sayısı: " + musaitBoxlar.Count);

            // Arama yap
            var zeytinBoxlar = musaitBoxlar
                .Where(x =>
                {
                    string boxNo = x.ZeytinBoxNo.HasValue ? x.ZeytinBoxNo.Value.ToString() : "";
                    string boxID = x.ZeytinBoxKasaID.ToString();

                    bool eslesme = boxNo.Contains(aramaMetni) || boxID.Contains(aramaMetni);

                    if (eslesme)
                    {
                        System.Diagnostics.Debug.WriteLine("Eşleşen box: " + boxNo + " (ID: " + boxID + ")");
                    }

                    return eslesme;
                })
                .OrderBy(x => x.ZeytinBoxNo) // Numaraya göre sırala
                .Take(30) // İlk 30 sonuç (daha fazla göster)
                .ToList();

            System.Diagnostics.Debug.WriteLine("Arama sonucu: " + zeytinBoxlar.Count + " adet");

            foreach (var box in zeytinBoxlar)
            {
                string boxDisplayName = box.ZeytinBoxNo.HasValue ?
                    box.ZeytinBoxNo.Value.ToString() :
                    "Box-" + box.ZeytinBoxKasaID.ToString();

                sonuclar.Add(new
                {
                    ZeytinBoxKasaID = box.ZeytinBoxKasaID,
                    ZeytinBoxNo = boxDisplayName,
                    ZeytinBoxNoInt = box.ZeytinBoxNo ?? 0,
                    Durumu = box.Durumu ?? false,
                    KulananMustahsilID = box.KulananMustahsilID,
                    Status = (box.KulananMustahsilID == null) ? "Müsait" : "Dolu"
                });
            }

            System.Diagnostics.Debug.WriteLine("Sonuç listesi hazırlandı: " + sonuclar.Count + " adet");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("ZeytinBoxAra hatası: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
        }

        return sonuclar;
    }

    [WebMethod]
    public static object GetZeytinBoxInfo(int zeytinBoxID)
    {
        try
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context.Session["SirketID"] == null)
            {
                return null;
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var zeytinBox = db.ZeytinBoxKasalaris
                .Where(x => x.ZeytinBoxKasaID == zeytinBoxID && x.SirketID == sirketID)
                .FirstOrDefault();

            if (zeytinBox != null)
            {
                return new
                {
                    ZeytinBoxKasaID = zeytinBox.ZeytinBoxKasaID,
                    ZeytinBoxNo = zeytinBox.ZeytinBoxNo ?? 0
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GetZeytinBoxInfo hatası: " + ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static object ZeytinBoxDurum()
    {
        try
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context.Session["SirketID"] == null)
            {
                return new { Hata = "Session bulunamadı" };
            }

            int sirketID = Convert.ToInt32(context.Session["SirketID"]);
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            var boxSayilari = new
            {
                ToplamBox = db.ZeytinBoxKasalaris.Count(x => x.SirketID == sirketID),
                AktifBox = db.ZeytinBoxKasalaris.Count(x => x.SirketID == sirketID && x.Durumu == true),
                PasifBox = db.ZeytinBoxKasalaris.Count(x => x.SirketID == sirketID && (x.Durumu == false || x.Durumu == null)),
                MusaitBox = db.ZeytinBoxKasalaris.Count(x => x.SirketID == sirketID && x.Durumu == true && x.KulananMustahsilID == null),
                DoluBox = db.ZeytinBoxKasalaris.Count(x => x.SirketID == sirketID && x.Durumu == true && x.KulananMustahsilID != null),
                SirketID = sirketID
            };

            return boxSayilari;
        }
        catch (Exception ex)
        {
            return new { Hata = ex.Message };
        }
    }

    public string GetZeytinBoxOptionsJSON()
    {
        try
        {
            if (Session["SirketID"] == null)
            {
                System.Diagnostics.Debug.WriteLine("GetZeytinBoxOptionsJSON: Session SirketID null");
                return "";
            }

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            System.Diagnostics.Debug.WriteLine("GetZeytinBoxOptionsJSON: SirketID = " + sirketID);

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();

            // Tüm ZeytinBox'ları al (debug için)
            var tumZeytinBoxlar = db.ZeytinBoxKasalaris
                .Where(x => x.SirketID == sirketID)
                .ToList();

            System.Diagnostics.Debug.WriteLine("Toplam ZeytinBox sayısı: " + tumZeytinBoxlar.Count);

            // Müsait ZeytinBox'ları al (Durumu true VE KulananMustahsilID null olanlar)
            var zeytinBoxlar = db.ZeytinBoxKasalaris
                .Where(x => x.SirketID == sirketID && x.Durumu == true && x.KulananMustahsilID == null)
                .OrderBy(x => x.ZeytinBoxNo)
                .ToList();

            System.Diagnostics.Debug.WriteLine("Müsait ZeytinBox sayısı: " + zeytinBoxlar.Count);

            StringBuilder sb = new StringBuilder();
            foreach (var box in zeytinBoxlar)
            {
                string boxDisplayName = box.ZeytinBoxNo.HasValue ?
                    box.ZeytinBoxNo.Value.ToString() :
                    "Box-" + box.ZeytinBoxKasaID.ToString();

                sb.AppendFormat("<option value='{0}'>ZeytinBox {1} (ID: {2})</option>",
                    box.ZeytinBoxKasaID,
                    boxDisplayName,
                    box.ZeytinBoxKasaID);

                System.Diagnostics.Debug.WriteLine("ZeytinBox eklendi: " + box.ZeytinBoxNo + " - ID: " + box.ZeytinBoxKasaID);
            }

            string sonuc = sb.ToString();
            System.Diagnostics.Debug.WriteLine("GetZeytinBoxOptionsJSON sonuç: " + sonuc);
            return sonuc;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("GetZeytinBoxOptionsJSON hatası: " + ex.Message);
            return "";
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            // Düzenleme modu kontrolü
            bool duzenlemeModundaMi = ViewState["EditingUretimID"] != null;
            int editUretimID = 0;
            
            if (duzenlemeModundaMi)
            {
                editUretimID = Convert.ToInt32(ViewState["EditingUretimID"]);
            }

            
            if (string.IsNullOrEmpty(hdnSelectedMustahsilID.Value))
            {
                MesajGoster("Müştahsil seçimi zorunludur!", false);
                FormVerileriniKoru();
                return;
            }

            if (string.IsNullOrEmpty(ddlUrun.SelectedValue))
            {
                for (int i = 0; i < ddlUrun.Items.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine("  [" + i + "] Text: '" + ddlUrun.Items[i].Text + "', Value: '" + ddlUrun.Items[i].Value + "'");
                }
                MesajGoster("Ürün seçimi zorunludur!", false);
                FormVerileriniKoru();
                return;
            }
            
            System.Diagnostics.Debug.WriteLine("=== VALIDATION BAŞARILI ===");

            // Plaka artık opsiyonel - zorunlu değil

            // ZeytinBox seçimlerini kontrol et (JSON array'den)
            List<int> zeytinBoxlar = new List<int>();
            
            if (!string.IsNullOrEmpty(hdnSecilenZeytinBoxlar.Value))
            {
                try
                {
                    // JSON array'i parse et
                    var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(hdnSecilenZeytinBoxlar.Value);
                    if (jsonArray != null)
                    {
                        zeytinBoxlar.AddRange(jsonArray);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ZeytinBox JSON parse hatası: " + ex.Message);
                }
            }

            if (zeytinBoxlar.Count == 0)
            {
                MesajGoster("En az bir ZeytinBox seçimi yapmalısınız!", false);
                return;
            }

            if (Session["SirketID"] == null || Session["KullaniciID"] == null)
            {
                Response.Redirect("~/Mobil/Login.aspx");
                return;
            }

            int sirketID = Convert.ToInt32(Session["SirketID"]);
            int kullaniciID = Convert.ToInt32(Session["KullaniciID"]);
            int mustahsilID = Convert.ToInt32(hdnSelectedMustahsilID.Value);
            int urunID = Convert.ToInt32(ddlUrun.SelectedValue);

            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            TahsiliyeUcretleri tahsilUcret = db.TahsiliyeUcretleris.FirstOrDefault(x => x.SirketID == sirketID);

            if (duzenlemeModundaMi)
            {
                // GÜNCELLEME İŞLEMİ
                var mevcutUretim = db.ZeytinyagiUretimleris.FirstOrDefault(x => x.ZeytinyagiUretimID == editUretimID);
                if (mevcutUretim != null)
                {
                    // Temel bilgileri güncelle
                    mevcutUretim.PlakaNo = txtPlaka.Text.Trim();
                    mevcutUretim.UrunID = urunID;

                    // GelisKg güncelle
                    if (!string.IsNullOrEmpty(txtGelisKg.Text.Trim()))
                    {
                        int gelisKg;
                        if (int.TryParse(txtGelisKg.Text.Trim(), out gelisKg))
                        {
                            mevcutUretim.GelisKg = gelisKg;
                            if (tahsilUcret != null)
                            {
                                mevcutUretim.TahsiliyeKgUcreti = tahsilUcret.TahsiliyeUcreti;
                                mevcutUretim.TahsiliyeToplamUcreti = tahsilUcret.TahsiliyeUcreti * gelisKg;
                            }
                        }
                    }
                    else
                    {
                        mevcutUretim.GelisKg = null;
                        mevcutUretim.TahsiliyeToplamUcreti = null;
                    }

                    // Önce değişiklikleri kaydet
                    db.SubmitChanges();

                    // Eski ZeytinBox mapping'lerini al
                    var eskiMappings = db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps
                        .Where(x => x.ZeytinyagiUretimID == editUretimID).ToList();

                    var eskiBoxIDler = eskiMappings.Select(x => x.ZeytinBoxKasaID).ToList();
                    var yeniBoxIDler = zeytinBoxlar.ToList();

                    // Kaldırılacak box'lar (eski listede var, yeni listede yok)
                    var kaldirrilacakBoxlar = eskiBoxIDler.Except(yeniBoxIDler).ToList();
                    
                    // Eklenecek box'lar (yeni listede var, eski listede yok)
                    var eklenecekBoxlar = yeniBoxIDler.Except(eskiBoxIDler).ToList();

                    // Kaldırılacak box'ları temizle
                    foreach (int boxID in kaldirrilacakBoxlar)
                    {
                        var mapping = eskiMappings.FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);
                        if (mapping != null)
                        {
                            // ZeytinBox'ı boşalt
                            var zeytinBox = db.ZeytinBoxKasalaris.FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);
                            if (zeytinBox != null)
                            {
                                zeytinBox.KulananMustahsilID = null;
                                zeytinBox.AlimTarihi = null;
                            }
                            
                            db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps.DeleteOnSubmit(mapping);
                        }
                    }

                    // Değişiklikleri kaydet
                    db.SubmitChanges();

                    // Eklenecek box'ları kontrol et ve ekle
                    foreach (int boxID in eklenecekBoxlar)
                    {
                        // Bu box başka bir üretimde kullanılıyor mu kontrol et
                        var mevcutMapping = db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps
                            .FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);

                        if (mevcutMapping != null)
                        {
                            MesajGoster("ZeytinBox " + boxID + " başka bir üretimde kullanılıyor! Güncelleme tamamlanamadı.", false);
                            return;
                        }

                        // Box müsait mi kontrol et
                        var zeytinBox = db.ZeytinBoxKasalaris.FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);
                        if (zeytinBox == null)
                        {
                            MesajGoster("ZeytinBox " + boxID + " bulunamadı!", false);
                            return;
                        }

                        if (zeytinBox.KulananMustahsilID != null && zeytinBox.KulananMustahsilID != mustahsilID)
                        {
                            MesajGoster("ZeytinBox " + boxID + " başka bir müştahsil tarafından kullanılıyor!", false);
                            return;
                        }

                        // Yeni mapping ekle
                        ZeytinyagiUretimi_ZeytinBoxKasa_Map yeniMapping = new ZeytinyagiUretimi_ZeytinBoxKasa_Map
                        {
                            ZeytinyagiUretimID = editUretimID,
                            ZeytinBoxKasaID = boxID,
                            EklenmeTarihi = DateTime.Now
                        };

                        db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps.InsertOnSubmit(yeniMapping);

                        // ZeytinBox durumunu güncelle
                        zeytinBox.Durumu = true;
                        zeytinBox.KulananMustahsilID = mustahsilID;
                        zeytinBox.AlimTarihi = DateTime.Now;
                    }

                    db.SubmitChanges();
                    MesajGoster("Mal kabul güncelleme başarıyla kaydedildi! Parti No: " + mevcutUretim.PartiNo, true);
                }
                else
                {
                    MesajGoster("Güncellenecek üretim kaydı bulunamadı!", false);
                    return;
                }
            }
            else
            {
                // YENİ KAYIT İŞLEMİ
                string gercekPartiNo = OlusturPartiNo(sirketID, db);
                System.Diagnostics.Debug.WriteLine("Oluşturulan parti no: " + gercekPartiNo);
                
                // Yeni üretim kaydı oluştur
                ZeytinyagiUretimleri yeniUretim = new ZeytinyagiUretimleri
                {
                    SirketID = sirketID,
                    MustahsilID = mustahsilID,
                    PartiNo = gercekPartiNo,
                    PlakaNo = txtPlaka.Text.Trim(),
                    GelisTarihi = DateTime.Now,
                    UrunID = urunID,
                    Tesmilalan_KullaniciID = kullaniciID
                };

                // GelisKg değeri varsa ekle (int tipinde)
                if (!string.IsNullOrEmpty(txtGelisKg.Text.Trim()))
                {
                    int gelisKg;
                    if (int.TryParse(txtGelisKg.Text.Trim(), out gelisKg))
                    {
                        yeniUretim.GelisKg = gelisKg;
                        if (tahsilUcret != null)
                        {
                            yeniUretim.TahsiliyeKgUcreti = tahsilUcret.TahsiliyeUcreti;
                            yeniUretim.TahsiliyeToplamUcreti = tahsilUcret.TahsiliyeUcreti * gelisKg;
                        }
                    }
                }

                db.ZeytinyagiUretimleris.InsertOnSubmit(yeniUretim);
                db.SubmitChanges();

                // ZeytinBox mapping'lerini kaydet
                foreach (int boxID in zeytinBoxlar)
                {
                    // Bu box başka bir üretimde kullanılıyor mu kontrol et
                    var mevcutMapping = db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps
                        .FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);

                    if (mevcutMapping != null)
                    {
                        MesajGoster("ZeytinBox " + boxID + " başka bir üretimde kullanılıyor! Kayıt tamamlanamadı.", false);
                        return;
                    }

                    // Box müsait mi kontrol et
                    var zeytinBox = db.ZeytinBoxKasalaris.FirstOrDefault(x => x.ZeytinBoxKasaID == boxID);
                    if (zeytinBox == null)
                    {
                        MesajGoster("ZeytinBox " + boxID + " bulunamadı!", false);
                        return;
                    }

                    if (zeytinBox.KulananMustahsilID != null)
                    {
                        MesajGoster("ZeytinBox " + boxID + " zaten kullanımda!", false);
                        return;
                    }

                    ZeytinyagiUretimi_ZeytinBoxKasa_Map mapping = new ZeytinyagiUretimi_ZeytinBoxKasa_Map
                    {
                        ZeytinyagiUretimID = yeniUretim.ZeytinyagiUretimID,
                        ZeytinBoxKasaID = boxID,
                        EklenmeTarihi = DateTime.Now
                    };

                    db.ZeytinyagiUretimi_ZeytinBoxKasa_Maps.InsertOnSubmit(mapping);

                    // ZeytinBox durumunu güncelle
                    zeytinBox.Durumu = true;
                    zeytinBox.KulananMustahsilID = mustahsilID;
                    zeytinBox.AlimTarihi = DateTime.Now;
                }

                db.SubmitChanges();
                MesajGoster("Mal kabul işlemi başarıyla kaydedildi! Parti No: " + yeniUretim.PartiNo, true);
            }

            // Formu temizle
            FormTemizle();
        }
        catch (Exception ex)
        {
            MesajGoster("Kaydetme sırasında hata oluştu: " + ex.Message, false);
        }
    }

    private void FormTemizle()
    {
        hdnSelectedMustahsilID.Value = "";
        hdnSecilenZeytinBoxlar.Value = "";
        txtMustahsilAra.Text = "";
        ddlUrun.SelectedIndex = 0;
        txtPlaka.Text = "";
        txtGelisKg.Text = "";
        lblPartiNo.Text = "Müştahsil seçildikten sonra oluşturulacak";
        
        // ViewState'i temizle
        ViewState["EditingUretimID"] = null;

        // Geçici Session verilerini temizle
        Session.Remove("TempMustahsilID");
        Session.Remove("TempPlaka");
        Session.Remove("TempUrunID");
        Session.Remove("TempGelisKg");
        Session.Remove("TempMustahsilAra");
        Session.Remove("TempSecilenZeytinBoxlar");
        Session.Remove("TempEditingUretimID");

        // JavaScript ile seçimi temizle
        ClientScript.RegisterStartupScript(this.GetType(), "clearForm",
            "clearMustahsilSecimi(); secilenZeytinBoxlar = []; updateZeytinBoxGrid();", true);
    }

    private string OlusturPartiNo(int sirketID, FabrikaDataClassesDataContext db)
    {
        try
        {
            // Son ZeytinyagiUretimID'yi al
            var sonUretim = db.ZeytinyagiUretimleris
                .Where(x => x.SirketID == sirketID)
                .OrderByDescending(x => x.ZeytinyagiUretimID)
                .FirstOrDefault();

            int sonID = sonUretim != null ? sonUretim.ZeytinyagiUretimID : 0;
            int yeniID = sonID + 1;

            // P + Yıl + Ay + Sıra numarası formatı
            DateTime simdi = DateTime.Now;
            string partiNo = string.Format("P{0}{1:D2}{2:D3}", 
                simdi.Year, 
                simdi.Month, 
                yeniID);

            return partiNo;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("PartiNo oluşturma hatası: " + ex.Message);
            return "P" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.Ticks.ToString().Substring(10);
        }
    }

    private void MesajGoster(string mesaj, bool basarili)
    {
        lblMesaj.Text = mesaj;
        pnlMesaj.Visible = true;

        string cssClass = basarili ? "alert-success" : "alert-danger";
        ClientScript.RegisterStartupScript(this.GetType(), "mesaj",
            "document.getElementById('divMesaj').className = 'alert " + cssClass + "';", true);
    }

    private void FormVerileriniKoru()
    {
        try
        {
            // Form verilerini Session'da sakla
            Session["TempMustahsilID"] = hdnSelectedMustahsilID.Value;
            Session["TempPlaka"] = txtPlaka.Text;
            Session["TempUrunID"] = ddlUrun.SelectedValue;
            Session["TempGelisKg"] = txtGelisKg.Text;
            Session["TempMustahsilAra"] = txtMustahsilAra.Text;
            Session["TempSecilenZeytinBoxlar"] = hdnSecilenZeytinBoxlar.Value;
            
            // Edit modu bilgilerini de sakla
            if (ViewState["EditingUretimID"] != null)
            {
                Session["TempEditingUretimID"] = ViewState["EditingUretimID"];
            }
            
            // URL'den gelen mustahsilID parametresini de sakla
            string mustahsilIDParam = Request.QueryString["mustahsilID"];
            if (!string.IsNullOrEmpty(mustahsilIDParam))
            {
                Session["TempMustahsilID"] = mustahsilIDParam;
                System.Diagnostics.Debug.WriteLine("FormVerileriniKoru: URL mustahsilID korundu: " + mustahsilIDParam);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("FormVerileriniKoru hatası: " + ex.Message);
        }
    }
    private void FormVerileriniGeriYukle()
    {
        try
        {
            // Session'da saklanan verileri geri yükle
            bool hasData = false;
            
            // Öncelikle URL parametrelerine bakalım - mustahsilID parametresi varsa öncelikli
            string mustahsilIDParam = Request.QueryString["mustahsilID"];
            if (!string.IsNullOrEmpty(mustahsilIDParam))
            {
                System.Diagnostics.Debug.WriteLine("FormVerileriniGeriYukle: URL'den mustahsilID algılandı: " + mustahsilIDParam);
                int mustahsilID;
                if (int.TryParse(mustahsilIDParam, out mustahsilID))
                {
                    // Hidden field'a ID'yi set edelim
                    hdnSelectedMustahsilID.Value = mustahsilID.ToString();
                    
                    // Eğer Session'da TempMustahsilID yoksa ekleyelim
                    if (Session["TempMustahsilID"] == null)
                    {
                        Session["TempMustahsilID"] = mustahsilID.ToString();
                    }
                    
                    hasData = true;
                }
            }
            // URL parametresi yoksa Session'dan TempMustahsilID'yi kontrol et
            else if (Session["TempMustahsilID"] != null)
            {
                System.Diagnostics.Debug.WriteLine("FormVerileriniGeriYukle: Session'dan mustahsilID alınıyor");
                hdnSelectedMustahsilID.Value = Session["TempMustahsilID"].ToString();
                hasData = true;
            }
            
            // Diğer form verilerini de geri yükleyelim
            if (Session["TempPlaka"] != null)
            {
                txtPlaka.Text = Session["TempPlaka"].ToString();
                hasData = true;
            }
                
            if (Session["TempUrunID"] != null && ddlUrun.Items.Count > 0)
            {
                try
                {
                    ddlUrun.SelectedValue = Session["TempUrunID"].ToString();
                    hasData = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("TempUrunID seçerken hata: " + ex.Message);
                    // Hata durumunda sessizce devam et
                }
            }
                
            if (Session["TempGelisKg"] != null)
            {
                txtGelisKg.Text = Session["TempGelisKg"].ToString();
                hasData = true;
            }
                
            if (Session["TempMustahsilAra"] != null)
            {
                txtMustahsilAra.Text = Session["TempMustahsilAra"].ToString();
                hasData = true;
            }
            
            if (Session["TempSecilenZeytinBoxlar"] != null)
            {
                hdnSecilenZeytinBoxlar.Value = Session["TempSecilenZeytinBoxlar"].ToString();
                hasData = true;
            }
            
            // Edit modu bilgilerini geri yükle
            if (Session["TempEditingUretimID"] != null)
            {
                ViewState["EditingUretimID"] = Session["TempEditingUretimID"];
                hasData = true;
            }
            
            // Eğer herhangi bir veri varsa JavaScript ile geri yükleme yap
            if (hasData)
            {
                string script = @"
                    document.addEventListener('DOMContentLoaded', function() {
                        // Müştahsil seçimini geri yükle
                        if (document.getElementById('hdnSelectedMustahsilID').value) {
                            restoreMustahsilSelection();
                            
                            // Parti numarası oluştur
                            if (typeof PageMethods !== 'undefined' && typeof PageMethods.PartiNoOlustur !== 'undefined') {
                                PageMethods.PartiNoOlustur(function(result) {
                                    var partiDiv = document.querySelector('.parti-display');
                                    if (partiDiv && (!partiDiv.innerHTML || partiDiv.innerHTML === 'Müştahsil seçildikten sonra oluşturulacak')) {
                                        partiDiv.innerHTML = result;
                                        console.log('Parti numarası oluşturuldu: ' + result);
                                    }
                                }, function(error) {
                                    console.error('Parti numarası oluşturma hatası:', error);
                                });
                            }
                        }
                        
                        // ZeytinBox seçimlerini geri yükle
                        if (document.getElementById('hdnSecilenZeytinBoxlar').value) {
                            restoreZeytinBoxSelections();
                        }
                        
                        // Edit modu göstergelerini geri yükle
                        restoreEditMode();
                    });
                    
                    function restoreMustahsilSelection() {
                        var mustahsilID = document.getElementById('hdnSelectedMustahsilID').value;
                        if (mustahsilID) {
                            // PageMethods ile müştahsil bilgilerini al ve göster
                            if (typeof PageMethods !== 'undefined' && PageMethods.MustahsilAra) {
                                PageMethods.MustahsilAra('', function(result) {
                                    var mustahsil = result.find(m => m.MustahsilID == mustahsilID);
                                    if (mustahsil) {
                                        document.getElementById('secilenMustahsilBilgi').innerHTML = 
                                            '<strong>' + mustahsil.AdSoyad + '</strong><br><small>' + 
                                            mustahsil.Telefon + ' | ' + mustahsil.Email + '</small>';
                                        document.getElementById('secilenMustahsil').style.display = 'block';
                                    }
                                });
                            }
                        }
                    }
                    
                    function restoreZeytinBoxSelections() {
                        try {
                            var secilenBoxlarJSON = document.getElementById('hdnSecilenZeytinBoxlar').value;
                            if (secilenBoxlarJSON) {
                                secilenZeytinBoxlar = JSON.parse(secilenBoxlarJSON) || [];
                                updateZeytinBoxGrid();
                            }
                        } catch(e) {
                            console.error('ZeytinBox seçimleri geri yüklenirken hata:', e);
                        }
                    }
                    
                    function restoreEditMode() {
                        // Edit modu için URL'den uretimID kontrolü
                        var urlParams = new URLSearchParams(window.location.search);
                        var uretimID = urlParams.get('uretimID');
                        
                        if (uretimID) {
                            // Başlığı güncelle
                            var header = document.querySelector('.form-header h4');
                            if (header) {
                                header.innerHTML = 'Mal Kabul Güncelleme';
                            }
                            
                            // Kaydet butonunu güncelle
                            var saveBtn = document.querySelector('.btn-save');
                            if (saveBtn) {
                                saveBtn.innerHTML = '<i class=""fas fa-save""></i> Güncellemeyi Kaydet';
                            }
                            
                            // Edit modu mesajını göster
                            showEditModeMessage();
                        }
                    }
                    
                    function showEditModeMessage() {
                        var urlParams = new URLSearchParams(window.location.search);
                        var uretimID = urlParams.get('uretimID');
                        
                        if (uretimID) {
                            var mesajDiv = document.createElement('div');
                            mesajDiv.className = 'alert alert-info';
                            mesajDiv.style.marginBottom = '20px';
                            mesajDiv.innerHTML = '<i class=""fas fa-edit""></i> Üretim ID: ' + uretimID + ' düzenleniyor.';
                            
                            var container = document.querySelector('.container-fluid');
                            if (container && container.firstChild) {
                                container.insertBefore(mesajDiv, container.firstChild);
                            }
                        }
                    }
                ";
                
                ClientScript.RegisterStartupScript(this.GetType(), "restoreForm", script, true);
            }
            
            // Temp session verilerini temizle
            Session.Remove("TempMustahsilID");
            Session.Remove("TempPlaka");
            Session.Remove("TempUrunID");
            Session.Remove("TempGelisKg");
            Session.Remove("TempMustahsilAra");
            Session.Remove("TempSecilenZeytinBoxlar");
            Session.Remove("TempEditingUretimID");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("FormVerileriniGeriYukle hatası: " + ex.Message);
        }
    }
}