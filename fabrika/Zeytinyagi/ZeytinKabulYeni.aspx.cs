using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class fabrika_Zeytinyagi_ZeytinKabulYeni : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Add server-side validation for the CustomValidator
        cvMiktar.ServerValidate += new ServerValidateEventHandler(cvMiktar_ServerValidate);

        // Master Page'e başlık bilgisini ayarla
        if (Master is fabrika_FabrikaMasterPage)
        {
            fabrika_FabrikaMasterPage master = (fabrika_FabrikaMasterPage)Master;
            master.KlasorAdi = "Zeytinyağı";
            master.SayfaAdi = "Zeytin Kabul Formu";
        }

        // İlk yüklemede veritabanı kolon yapısını kontrol et
        if (!IsPostBack)
        {
            try
            {
                EnsureGelisKgColumnAcceptsNull();
            }
            catch (Exception ex)
            {
                // Hata olursa sadece loglama yap, kullanıcıya gösterme
                System.Diagnostics.Debug.WriteLine("Veritabanı kontrolü sırasında hata: " + ex.Message);
            }
            
            // Yükleme işlemleri
            int selectedMustahsilID = 0;
            
            // MustahsilID parametresi kontrolü
            if (Request.QueryString["MustahsilID"] != null && int.TryParse(Request.QueryString["MustahsilID"], out selectedMustahsilID))
            {
                // MustahsilID parametresi varsa, seçili olarak getirmek için sakla
            }
            
            LoadMustahsiller(selectedMustahsilID);
            LoadUrunler();
            
            // Geliş tarihini şimdiki zamana ayarla
            txtGelisTarihi.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            
            // ID parametresi kontrolü
            int zeytinyagiUretimID = 0;
            bool hasID = false;

            // URL'den ID parametresi kontrolü
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out zeytinyagiUretimID))
            {
                hasID = true;
            }
            // Session'dan kontrol (URL'de ID yoksa)
            else if (Session["ZeytinyagiUretimID"] != null)
            {
                zeytinyagiUretimID = Convert.ToInt32(Session["ZeytinyagiUretimID"]);
                hasID = true;
                // Session değişkenlerini temizle
                Session.Remove("ZeytinyagiUretimID");
            }

            if (hasID)
            {
                // ID değerini hidden field'a aktar
                hfEditID.Value = zeytinyagiUretimID.ToString();
                hfEditMode.Value = "1";
                
                // Düzenleme veya salt okunur mod
                bool isReadOnly = Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().ToLower() == "readonly";
                
                if (isReadOnly)
                {
                    ltlPageTitle.Text = "Zeytin Kabul Detayları";
                    btnKaydet.Visible = false;
                    btnDeleteRecord.Visible = false;
                }
                else
                {
                    ltlPageTitle.Text = "Zeytin Kabul Kaydını Düzenle";
                    btnKaydet.Text = "Güncelle";
                    btnDeleteRecord.Visible = true;
                }
                
                LoadKayitForEdit(zeytinyagiUretimID, isReadOnly);
                
                // Box kasa yönetim panelini göster
                if (!isReadOnly)
                {
                    // ID parametresi olan her durumda Box Kasa panelini göster
                    ShowBoxKasaManagementPanel(zeytinyagiUretimID);
                    
                    // Session temizle (varsa)
                    if (Session["ShowBoxKasaManagement"] != null)
                    {
                        Session.Remove("ShowBoxKasaManagement");
                    }
                }
            }
            else
            {
                ltlPageTitle.Text = "Yeni Zeytin Kabul Kaydı";
                btnKaydet.Text = "Kaydet ve Kasa Ekle";
                btnDeleteRecord.Visible = false;
                
                // Yeni kayıt için örnek Parti No göster
                DateTime now = DateTime.Now;
                string year = now.Year.ToString();
                string month = now.Month.ToString().PadLeft(2, '0');
                
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // O aydaki kayıt sayısını sorgula
                        string query = @"SELECT COUNT(*) FROM ZeytinyagiUretimleri 
                                        WHERE YEAR(GelisTarihi) = @Year AND MONTH(GelisTarihi) = @Month";
                        
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Year", now.Year);
                            cmd.Parameters.AddWithValue("@Month", now.Month);
                            
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            int nextNumber = count + 1;
                            
                            // Örnek Parti No göster
                            string examplePartiNo = string.Format("P{0}{1}{2}", year, month, nextNumber.ToString().PadLeft(3, '0'));
                            txtPartiNo.Text = examplePartiNo + " (Taslak)";
                        }
                    }
                }
                catch
                {
                    // Hata durumunda basit bir örnek göster
                    txtPartiNo.Text = "P" + year + month + "001 (Taslak)";
                }
                
                // Box kasa yönetim panelini gizle
                pnlBoxKasaManagement.Visible = false;
                
                // Edit ve ID bilgilerini temizle
                hfEditID.Value = "0";
                hfEditMode.Value = "0";
            }
        }
    }

    // Server-side validation for the Miktar field
    protected void cvMiktar_ServerValidate(object source, ServerValidateEventArgs args)
    {
        // If empty, consider it valid (optional field)
        if (string.IsNullOrEmpty(args.Value))
        {
            args.IsValid = true;
            return;
        }
        
        // If not empty, check if it's a number within range
        decimal value;
        args.IsValid = decimal.TryParse(args.Value, out value) && value >= 0 && value <= 100000;
    }

    // Hata mesajı gösterme
    private void ShowError(string message)
    {
        //pnlHata.Visible = true;
        //ltlHata.Text = message;
        //pnlBasari.Visible = false;"İşlem sırasında bir hata oluştu: " + ex.Message
        MessageHelper.ShowErrorMessage(this, "Zeytin Kabul", "İşlem sırasında bir hata oluştu: " + message);

        // Bildirim ile hata mesajı gösterme
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ErrorNotification", 
            string.Format("showErrorMessage('Hata', '{0}');", message.Replace("'", "\\'")), true);
    }

    // Başarı mesajı gösterme
    private void ShowSuccess(string message)
    {
        //pnlBasari.Visible = true;
        //ltlBasari.Text = message;
        //pnlHata.Visible = false;
        MessageHelper.ShowSuccessMessage(this, "Zeytin Kabul", " " + message);

        // Bildirim ile başarı mesajı gösterme
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessNotification", 
            string.Format("showSuccessMessage('Başarılı', '{0}');", message.Replace("'", "\\'")), true);
    }

    // Form temizleme
    private void ClearForm()
    {
        hfEditMode.Value = "0";
        hfEditID.Value = "0";
        hfPartiNo.Value = "";
        txtPartiNo.Text = "";
        ddlMustahsil.SelectedIndex = 0;
        txtGelisTarihi.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        txtPlakaNo.Text = string.Empty;
        txtMiktar.Text = string.Empty;
        ddlUrun.SelectedIndex = 0;
        hfSelectedBoxKasalar.Value = "";
        btnKaydet.Text = "Kaydet ve Kasa Ekle";
        btnDeleteRecord.Visible = false;
        pnlBoxKasaManagement.Visible = false;
    }

    // Form kontrollerini salt okunur yapma
    private void SetFormReadOnly(bool isReadOnly)
    {
        ddlMustahsil.Enabled = !isReadOnly;
        txtGelisTarihi.ReadOnly = isReadOnly;
        txtPlakaNo.ReadOnly = isReadOnly;
        txtMiktar.ReadOnly = isReadOnly;
        ddlUrun.Enabled = !isReadOnly;
        btnKaydet.Visible = !isReadOnly;
        btnDeleteRecord.Visible = isReadOnly;
    }

    // Müstahsilleri yükleme
    private void LoadMustahsiller(int selectedMustahsilID = 0)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MustahsilID, (Ad + ' ' + Soyad) AS MustahsilAdi FROM Mustahsiller WHERE Durum = 1 ORDER BY Ad, Soyad";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlMustahsil.Items.Clear();
                    ddlMustahsil.DataSource = dt;
                    ddlMustahsil.DataTextField = "MustahsilAdi";
                    ddlMustahsil.DataValueField = "MustahsilID";
                    ddlMustahsil.DataBind();

                    // En üste "Seçiniz..." opsiyonu ekle
                    ddlMustahsil.Items.Insert(0, new ListItem("Seçiniz...", ""));
                    
                    // Eğer belirtilen bir ID varsa, o müstahsili seç
                    if (selectedMustahsilID > 0)
                    {
                        ListItem item = ddlMustahsil.Items.FindByValue(selectedMustahsilID.ToString());
                        if (item != null)
                            item.Selected = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ShowError("Müstahsil listesi yüklenirken bir hata oluştu: " + ex.Message);
            MessageHelper.ShowSuccessMessage(this, "Zeytin Kabul", "Müstahsil listesi yüklenirken bir hata oluştu:  " + ex.Message);
        }
    }

    // Yarı mamul ürünleri yükleme
    private void LoadUrunler()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UrunID, UrunAdi FROM Urunler WHERE YariManul = 1 AND Durum = 1 ORDER BY UrunAdi";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlUrun.DataSource = dt;
                    ddlUrun.DataTextField = "UrunAdi";
                    ddlUrun.DataValueField = "UrunID";
                    ddlUrun.DataBind();

                    // En üste "Seçiniz..." opsiyonu ekle
                    ddlUrun.Items.Insert(0, new ListItem("Seçiniz...", ""));
                }
            }
        }
        catch (Exception ex)
        {
            //ShowError("Ürün listesi yüklenirken bir hata oluştu: " + ex.Message);
            MessageHelper.ShowSuccessMessage(this, "Zeytin Kabul", "Ürün listesi yüklenirken bir hata oluştu:  " + ex.Message);
        }
    }

    // Kullanılmamış box kasaları yükleme
    private void LoadZeytinBoxKasalar()
    {
        // Bu metod artık kullanılmıyor çünkü lstZeytinBoxKasa kontrolü kaldırıldı
        /*
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SirketID al
                int sirketID = SessionHelper.GetSirketID();
                
                string query = @"SELECT ZeytinBoxKasaID, CONVERT(VARCHAR, ZeytinBoxNo) AS ZeytinBoxNo 
                                FROM ZeytinBoxKasalari 
                                WHERE KulananMustahsilID IS NULL AND Durumu = 1 AND SirketID = @SirketID
                                ORDER BY ZeytinBoxNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SirketID", sirketID);
                    
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // lstZeytinBoxKasa.DataSource = dt;
                    // lstZeytinBoxKasa.DataTextField = "ZeytinBoxNo";
                    // lstZeytinBoxKasa.DataValueField = "ZeytinBoxKasaID";
                    // lstZeytinBoxKasa.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Box kasa listesi yüklenirken bir hata oluştu: " + ex.Message);
        }
        */
    }

    // Düzenleme için kayıt verilerini yükleme
    private void LoadKayitForEdit(int zeytinyagiUretimID, bool isReadOnly = false)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                string query = @"SELECT z.ZeytinyagiUretimID, z.PartiNo, z.MustahsilID, z.PlakaNo, z.GelisTarihi, 
                                z.GelisKg, z.UrunID
                                FROM ZeytinyagiUretimleri z
                                WHERE z.ZeytinyagiUretimID = @ZeytinyagiUretimID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Düzenleme modunu ve ID'yi ayarla
                        hfEditMode.Value = "1";
                        hfEditID.Value = zeytinyagiUretimID.ToString();
                        
                        // Parti No'yu göster
                        if (reader["PartiNo"] != DBNull.Value)
                        {
                            string partiNo = reader["PartiNo"].ToString();
                            txtPartiNo.Text = partiNo;
                            hfPartiNo.Value = partiNo;
                        }
                        
                        // Form alanlarına verileri yerleştir
                        ddlMustahsil.SelectedValue = reader["MustahsilID"].ToString();
                        
                        // Geliş tarihi
                        if (reader["GelisTarihi"] != DBNull.Value)
                        {
                            DateTime gelisTarihi = Convert.ToDateTime(reader["GelisTarihi"]);
                            txtGelisTarihi.Text = gelisTarihi.ToString("yyyy-MM-ddTHH:mm");
                        }
                        
                        // Plaka No
                        if (reader["PlakaNo"] != DBNull.Value)
                            txtPlakaNo.Text = reader["PlakaNo"].ToString();
                        else
                            txtPlakaNo.Text = string.Empty;
                        
                        // Geliş miktarı
                        if (reader["GelisKg"] != DBNull.Value)
                            txtMiktar.Text = reader["GelisKg"].ToString();
                        else
                            txtMiktar.Text = string.Empty;
                            
                        // Ürün
                        if (reader["UrunID"] != DBNull.Value)
                            ddlUrun.SelectedValue = reader["UrunID"].ToString();
                        
                        reader.Close();
                        
                        // Salt okunur mod ayarlama
                        if (isReadOnly)
                        {
                            SetFormReadOnly(true);
                        }
                    }
                    else
                    {
                        reader.Close();
                        ShowError("Düzenlenecek kayıt bulunamadı.");
                        ClearForm();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Kayıt verileri yüklenirken bir hata oluştu: " + ex.Message);
        }
    }

    // Kaydet butonu işlemi
    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            // Form validasyon kontrolü
            Page.Validate("ZeytinKabulGroup");
            if (!Page.IsValid)
            {
                return;
            }

            // Veritabanı işlemleri
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Transaction başlat
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try 
                    {
                        // Düzenleme modu kontrolü
                        bool isEditMode = !string.IsNullOrEmpty(hfEditID.Value) && hfEditID.Value != "0";
                        int zeytinyagiUretimID = 0;
                        
                        if (isEditMode)
                        {
                            zeytinyagiUretimID = Convert.ToInt32(hfEditID.Value);
                            
                            // GÜNCELLEME işlemi
                            string updateQuery = @"UPDATE ZeytinyagiUretimleri SET 
                                                MustahsilID = @MustahsilID, 
                                                PlakaNo = @PlakaNo, 
                                                GelisTarihi = @GelisTarihi, 
                                                GelisKg = @GelisKg,
                                                UrunID = @UrunID
                                                WHERE ZeytinyagiUretimID = @ZeytinyagiUretimID";
                                                
                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                                cmd.Parameters.AddWithValue("@MustahsilID", Convert.ToInt32(ddlMustahsil.SelectedValue));
                                
                                if (string.IsNullOrEmpty(txtPlakaNo.Text))
                                    cmd.Parameters.AddWithValue("@PlakaNo", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@PlakaNo", txtPlakaNo.Text);
                                    
                                cmd.Parameters.AddWithValue("@GelisTarihi", Convert.ToDateTime(txtGelisTarihi.Text));
                                
                                // Miktar alanını opsiyonel hale getir
                                if (string.IsNullOrEmpty(txtMiktar.Text))
                                    cmd.Parameters.AddWithValue("@GelisKg", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@GelisKg", Convert.ToDecimal(txtMiktar.Text));
                                
                                if (string.IsNullOrEmpty(ddlUrun.SelectedValue))
                                    cmd.Parameters.AddWithValue("@UrunID", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@UrunID", Convert.ToInt32(ddlUrun.SelectedValue));
                                
                                cmd.ExecuteNonQuery();
                            }
                            
                            // Başarı mesajı
                            ShowSuccess("Zeytin kabul kaydı başarıyla güncellendi.");
                        }
                        else
                        {
                            // YENİ KAYIT oluşturma
                            string partiNo = GeneratePartiNo(conn, transaction);

                            string insertQuery = @"INSERT INTO ZeytinyagiUretimleri 
                                               (SirketID, MustahsilID, PartiNo, PlakaNo, GelisTarihi, GelisKg, 
                                                ZeytinyagiUretim_islemID, UrunID, Operator_KullaniciID) 
                                               VALUES 
                                               (@SirketID, @MustahsilID, @PartiNo, @PlakaNo, @GelisTarihi, @GelisKg, 
                                                @IslemID, @UrunID, @OperatorID); 
                                               SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction))
                            {
                                int sirketID = SessionHelper.GetSirketID();
                                cmd.Parameters.AddWithValue("@SirketID", sirketID);
                                cmd.Parameters.AddWithValue("@MustahsilID", Convert.ToInt32(ddlMustahsil.SelectedValue));
                                cmd.Parameters.AddWithValue("@PartiNo", partiNo);

                                if (string.IsNullOrEmpty(txtPlakaNo.Text))
                                    cmd.Parameters.AddWithValue("@PlakaNo", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@PlakaNo", txtPlakaNo.Text);

                                cmd.Parameters.AddWithValue("@GelisTarihi", Convert.ToDateTime(txtGelisTarihi.Text));
                                
                                // Miktar alanını opsiyonel hale getir
                                if (string.IsNullOrEmpty(txtMiktar.Text))
                                    cmd.Parameters.AddWithValue("@GelisKg", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@GelisKg", Convert.ToDecimal(txtMiktar.Text));

                                cmd.Parameters.AddWithValue("@IslemID", 1); // Sabit değer
                                
                                if (string.IsNullOrEmpty(ddlUrun.SelectedValue))
                                    cmd.Parameters.AddWithValue("@UrunID", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@UrunID", Convert.ToInt32(ddlUrun.SelectedValue));

                                cmd.Parameters.AddWithValue("@OperatorID", 1); // Örnek değer

                                // Yeni kaydın ID'sini al
                                var result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    zeytinyagiUretimID = Convert.ToInt32(result);
                                }
                            }
                            
                            // Başarı mesajı
                            ShowSuccess("Zeytin kabul kaydı başarıyla oluşturuldu. Şimdi box kasa ekleyebilirsiniz.");
                        }
                        
                        // İşlemi onayla
                        transaction.Commit();
                        
                        // Kayıt ID'sini sakla
                        hfEditID.Value = zeytinyagiUretimID.ToString();
                        hfEditMode.Value = "1";

                        // Session'a değişkenleri ekle
                        Session["ShowBoxKasaManagement"] = true;
                        Session["ZeytinyagiUretimID"] = zeytinyagiUretimID;

                        // Kasa yönetim panelini göstermek ve URL'i güncellemek için sayfayı yeniden yönlendir
                        // Bu yönlendirme ile sayfanın URL'si de güncellenecek ve postback sorunu çözülecek
                        Response.Redirect(string.Format("ZeytinKabulYeni.aspx?id={0}", zeytinyagiUretimID), false);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemi geri al
                        transaction.Rollback();
                        ShowError("İşlem sırasında hata oluştu: " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Kayıt işlemi sırasında hata oluştu: " + ex.Message);
        }
    }

    // Parti No oluşturma
    private string GeneratePartiNo(SqlConnection conn, SqlTransaction transaction = null)
    {
        try
        {
            // Şimdiki yıl ve ay bilgisini al
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString().PadLeft(2, '0');
            
            // O aydaki kayıt sayısını sorgula
            string query = @"SELECT COUNT(*) FROM ZeytinyagiUretimleri 
                            WHERE YEAR(GelisTarihi) = @Year AND MONTH(GelisTarihi) = @Month";
            
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Year", now.Year);
                cmd.Parameters.AddWithValue("@Month", now.Month);
                
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                int nextNumber = count + 1;
                
                // PartiNo oluştur: P + Yıl + Ay + Sıra numarası (en az 3 basamaklı)
                string partiNo = string.Format("P{0}{1}{2}", year, month, nextNumber.ToString().PadLeft(3, '0'));
                
                return partiNo;
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda varsayılan bir parti numarası döndür
            ShowError("Parti numarası oluşturulurken hata: " + ex.Message);
            return "P" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }

    // Box Kasa Yönetimi Panelini Göster
    private void ShowBoxKasaManagementPanel(int zeytinyagiUretimID)
    {
        // Ana form bileşenlerini gizle
        SetMainFormVisibility(false);
        
        // Box Kasa Yönetim panelini göster
        pnlBoxKasaManagement.Visible = true;
        
        // ID'yi saklıyoruz
        hfEditID.Value = zeytinyagiUretimID.ToString();
        hfEditMode.Value = "1";
        
        // Form başlığını güncelle
        ltlPageTitle.Text = "Zeytin Kabul Kaydını Düzenle - Box Kasa Yönetimi";
        
        // Arama kutusunu temizle
        txtSearchBoxKasa.Text = string.Empty;
        
        // Box Kasa listelerini yükle
        LoadAvailableBoxKasas();
        LoadAssignedBoxKasas(zeytinyagiUretimID);
        
        // UpdatePanel'i güncelle
        if (UpdatePanel1 != null)
        {
            UpdatePanel1.Update();
        }
    }
    
    // Ana Form Görünürlüğünü Ayarla
    private void SetMainFormVisibility(bool visible)
    {
        // Form alanlarını görünürlüğünü ayarla
        ddlMustahsil.Enabled = visible;
        txtGelisTarihi.Enabled = visible;
        txtPlakaNo.Enabled = visible;
        txtMiktar.Enabled = visible;
        ddlUrun.Enabled = visible;
        btnKaydet.Visible = visible;
    }
    
    // Kullanılabilir Box Kasaları Yükle
    private void LoadAvailableBoxKasas()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SirketID al
                int sirketID = SessionHelper.GetSirketID();
                
                // Arama filtresi ekle
                string searchFilter = string.Empty;
                string searchTerm = txtSearchBoxKasa.Text.Trim();
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchFilter = " AND CONVERT(VARCHAR, ZeytinBoxNo) LIKE @SearchTerm";
                }
                
                string query = string.Format(@"SELECT ZeytinBoxKasaID, CONVERT(VARCHAR, ZeytinBoxNo) AS ZeytinBoxNo 
                                FROM ZeytinBoxKasalari 
                                WHERE KulananMustahsilID IS NULL AND Durumu = 1 AND SirketID = @SirketID{0}
                                ORDER BY ZeytinBoxNo", searchFilter);

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SirketID", sirketID);
                    
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }
                    
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    lstAvailableBoxKasas.DataSource = dt;
                    lstAvailableBoxKasas.DataTextField = "ZeytinBoxNo";
                    lstAvailableBoxKasas.DataValueField = "ZeytinBoxKasaID";
                    lstAvailableBoxKasas.DataBind();
                    
                    // Bulunan kayıt sayısını göster
                    string countLabel = string.Format("{0} box kasa bulundu", dt.Rows.Count);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "UpdateCount", 
                        string.Format("$('#availableBoxCount').text('{0}');", countLabel), true);
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Kullanılabilir box kasa listesi yüklenirken bir hata oluştu: " + ex.Message);
        }
    }
    
    // Box Kasa Arama TextChanged Event
    protected void txtSearchBoxKasa_TextChanged(object sender, EventArgs e)
    {
        try
        {
            // Mevcut zeytin üretim ID'sini al
            int zeytinyagiUretimID = Convert.ToInt32(hfEditID.Value);
            
            // Box Kasa listeleri yeniden yükle
            LoadAvailableBoxKasas();
            
            // UpdatePanel'i güncelle
            UpdatePanel1.Update();
            
            // Odağı arama kutusuna getir (daha iyi UX için)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFocus", 
                "$('#" + txtSearchBoxKasa.ClientID + "').focus();", true);
        }
        catch (Exception ex)
        {
            ShowError("Arama yaparken bir hata oluştu: " + ex.Message);
        }
    }
    
    // Bu Üretime Atanmış Box Kasaları Yükle
    private void LoadAssignedBoxKasas(int zeytinyagiUretimID)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT zbk.ZeytinBoxKasaID, CONVERT(VARCHAR, zbk.ZeytinBoxNo) AS ZeytinBoxNo
                                FROM ZeytinBoxKasalari zbk
                                INNER JOIN ZeytinyagiUretimi_ZeytinBoxKasa_Map map 
                                ON zbk.ZeytinBoxKasaID = map.ZeytinBoxKasaID
                                WHERE map.ZeytinyagiUretimID = @ZeytinyagiUretimID
                                ORDER BY zbk.ZeytinBoxNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                    
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    lstAssignedBoxKasas.DataSource = dt;
                    lstAssignedBoxKasas.DataTextField = "ZeytinBoxNo";
                    lstAssignedBoxKasas.DataValueField = "ZeytinBoxKasaID";
                    lstAssignedBoxKasas.DataBind();
                    
                    // Bulunan kayıt sayısını göster
                    string countLabel = string.Format("{0} box kasa eklenmiş", dt.Rows.Count);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "UpdateAssignedCount", 
                        string.Format("$('#assignedBoxCount').text('{0}');", countLabel), true);
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Atanmış box kasa listesi yüklenirken bir hata oluştu: " + ex.Message);
        }
    }

    // Box Kasa Ekleme Butonu
    protected void btnAddBoxKasas_Click(object sender, EventArgs e)
    {
        try
        {
            // Zeytin Üretim ID'sini al
            int zeytinyagiUretimID = Convert.ToInt32(hfEditID.Value);
            int mustahsilID = Convert.ToInt32(ddlMustahsil.SelectedValue);
            
            // Veritabanı işlemleri
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Transaction başlat
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Seçilen box kasaları ekle
                        foreach (ListItem item in lstAvailableBoxKasas.Items)
                        {
                            if (item.Selected)
                            {
                                int boxKasaID = Convert.ToInt32(item.Value);
                                
                                // Box Kasa mapping tablosuna ilişki ekle
                                string insertBoxMapQuery = @"IF NOT EXISTS (SELECT 1 FROM ZeytinyagiUretimi_ZeytinBoxKasa_Map 
                                                        WHERE ZeytinyagiUretimID = @ZeytinyagiUretimID AND ZeytinBoxKasaID = @BoxKasaID)
                                                    INSERT INTO ZeytinyagiUretimi_ZeytinBoxKasa_Map 
                                                    (ZeytinyagiUretimID, ZeytinBoxKasaID) 
                                                    VALUES (@ZeytinyagiUretimID, @BoxKasaID)";
                                
                                using (SqlCommand cmd = new SqlCommand(insertBoxMapQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                                    cmd.Parameters.AddWithValue("@BoxKasaID", boxKasaID);
                                    cmd.ExecuteNonQuery();
                                }
                                
                                // Box Kasa durumunu güncelle
                                string updateBoxQuery = @"UPDATE ZeytinBoxKasalari 
                                                      SET KulananMustahsilID = @MustahsilID 
                                                      WHERE ZeytinBoxKasaID = @BoxKasaID";
                                
                                using (SqlCommand cmd = new SqlCommand(updateBoxQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@MustahsilID", mustahsilID);
                                    cmd.Parameters.AddWithValue("@BoxKasaID", boxKasaID);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        // İşlemi onayla
                        transaction.Commit();
                        
                        // Listeleri güncelle
                        LoadAvailableBoxKasas();
                        LoadAssignedBoxKasas(zeytinyagiUretimID);
                        
                        // UpdatePanel'i güncelle
                        UpdatePanel1.Update();
                        
                        // Başarılı mesajı göster (AJAX friendly)
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "SuccessNotification", 
                            string.Format("showSuccessMessage('Başarılı', '{0}');", 
                            "Seçilen box kasalar başarıyla eklendi.".Replace("'", "\\'")), true);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemi geri al
                        transaction.Rollback();
                        
                        // Hata mesajı göster (AJAX friendly)
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ErrorNotification", 
                            string.Format("showErrorMessage('Hata', '{0}');", 
                            ("Box kasa ekleme sırasında hata oluştu: " + ex.Message).Replace("'", "\\'")), true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata mesajı göster (AJAX friendly)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ErrorNotification", 
                string.Format("showErrorMessage('Hata', '{0}');", 
                ("İşlem sırasında bir hata oluştu: " + ex.Message).Replace("'", "\\'")), true);
        }
    }
    
    // Box Kasa Kaldırma Butonu
    protected void btnRemoveBoxKasas_Click(object sender, EventArgs e)
    {
        try
        {
            // Zeytin Üretim ID'sini al
            int zeytinyagiUretimID = Convert.ToInt32(hfEditID.Value);
            
            // Veritabanı işlemleri
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Transaction başlat
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Seçilen box kasaları kaldır
                        foreach (ListItem item in lstAssignedBoxKasas.Items)
                        {
                            if (item.Selected)
                            {
                                int boxKasaID = Convert.ToInt32(item.Value);
                                
                                // Box Kasa mapping tablosundan ilişkiyi kaldır
                                string deleteBoxMapQuery = @"DELETE FROM ZeytinyagiUretimi_ZeytinBoxKasa_Map 
                                                        WHERE ZeytinyagiUretimID = @ZeytinyagiUretimID 
                                                        AND ZeytinBoxKasaID = @BoxKasaID";
                                
                                using (SqlCommand cmd = new SqlCommand(deleteBoxMapQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                                    cmd.Parameters.AddWithValue("@BoxKasaID", boxKasaID);
                                    cmd.ExecuteNonQuery();
                                }
                                
                                // Box Kasa durumunu serbest bırak
                                string updateBoxQuery = @"UPDATE ZeytinBoxKasalari 
                                                      SET KulananMustahsilID = NULL 
                                                      WHERE ZeytinBoxKasaID = @BoxKasaID";
                                
                                using (SqlCommand cmd = new SqlCommand(updateBoxQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@BoxKasaID", boxKasaID);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        // İşlemi onayla
                        transaction.Commit();
                        
                        // Listeleri güncelle
                        LoadAvailableBoxKasas();
                        LoadAssignedBoxKasas(zeytinyagiUretimID);
                        
                        // UpdatePanel'i güncelle
                        UpdatePanel1.Update();
                        
                        // Başarılı mesajı göster (AJAX friendly)
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "SuccessNotification", 
                            string.Format("showSuccessMessage('Başarılı', '{0}');", 
                            "Seçilen box kasalar başarıyla kaldırıldı.".Replace("'", "\\'")), true);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemi geri al
                        transaction.Rollback();
                        
                        // Hata mesajı göster (AJAX friendly)
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ErrorNotification", 
                            string.Format("showErrorMessage('Hata', '{0}');", 
                            ("Box kasa kaldırma sırasında hata oluştu: " + ex.Message).Replace("'", "\\'")), true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata mesajı göster (AJAX friendly)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ErrorNotification", 
                string.Format("showErrorMessage('Hata', '{0}');", 
                ("İşlem sırasında bir hata oluştu: " + ex.Message).Replace("'", "\\'")), true);
        }
    }
    
    // İşlemi Tamamlama Butonu
    protected void btnCompleteProcess_Click(object sender, EventArgs e)
    {
        try 
        {
            // Başarı mesajı göster
            ShowSuccess("Box kasa işlemleri başarıyla tamamlandı.");
            
            // Kısa bir süre bekleyip listeye dön
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RedirectDelayed", 
                "setTimeout(function() { window.location.href = 'ZeytinKabul.aspx'; }, 1500);", true);
        }
        catch (Exception ex)
        {
            ShowError("İşlem sırasında bir hata oluştu: " + ex.Message);
        }
    }
    
    // Kayıt Silme Butonu
    protected void btnDeleteRecord_Click(object sender, EventArgs e)
    {
        try
        {
            // Silinecek kayıt ID'sini al
            int zeytinyagiUretimID = Convert.ToInt32(hfEditID.Value);
            
            // Veritabanı işlemleri
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Transaction başlat
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Önce bu üretim ile ilişkili box kasaları serbest bırak
                        string clearBoxQuery = @"UPDATE zbk
                                            SET zbk.KulananMustahsilID = NULL
                                            FROM ZeytinBoxKasalari zbk
                                            INNER JOIN ZeytinyagiUretimi_ZeytinBoxKasa_Map map ON zbk.ZeytinBoxKasaID = map.ZeytinBoxKasaID
                                            WHERE map.ZeytinyagiUretimID = @ZeytinyagiUretimID";
                        
                        using (SqlCommand cmd = new SqlCommand(clearBoxQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Mapping tablosundaki kayıtları sil (CASCADE olsa da güvenlik için)
                        string deleteMapQuery = @"DELETE FROM ZeytinyagiUretimi_ZeytinBoxKasa_Map
                                            WHERE ZeytinyagiUretimID = @ZeytinyagiUretimID";
                        
                        using (SqlCommand cmd = new SqlCommand(deleteMapQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Zeytin üretim kaydını sil
                        string deleteQuery = @"DELETE FROM ZeytinyagiUretimleri
                                          WHERE ZeytinyagiUretimID = @ZeytinyagiUretimID";
                        
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ZeytinyagiUretimID", zeytinyagiUretimID);
                            cmd.ExecuteNonQuery();
                        }
                        
                        // İşlemi onayla
                        transaction.Commit();
                        
                        // Başarılı mesajı göster ve listeye dön
                        ShowSuccess("Kayıt başarıyla silindi.");
                        
                        // Kısa bir süre bekleyip listeye dön
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "RedirectDelayed", 
                            "setTimeout(function() { window.location.href = 'ZeytinKabul.aspx'; }, 1500);", true);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemi geri al
                        transaction.Rollback();
                        ShowError("Kayıt silme sırasında hata oluştu: " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("İşlem sırasında bir hata oluştu: " + ex.Message);
        }
    }

    // GelisKg sütununun NULL değer kabul etmesini sağla
    private void EnsureGelisKgColumnAcceptsNull()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Önce sütunun NULL kabul edip etmediğini kontrol et
                string checkNullableQuery = @"SELECT IS_NULLABLE 
                                            FROM INFORMATION_SCHEMA.COLUMNS 
                                            WHERE TABLE_NAME = 'ZeytinyagiUretimleri' 
                                            AND COLUMN_NAME = 'GelisKg'";
                
                bool isNullable = false;
                using (SqlCommand cmd = new SqlCommand(checkNullableQuery, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        isNullable = string.Equals(result.ToString(), "YES", StringComparison.OrdinalIgnoreCase);
                    }
                }
                
                // Sütun NULL kabul etmiyorsa, ALTER TABLE ile değiştir
                if (!isNullable)
                {
                    string alterTableQuery = @"ALTER TABLE ZeytinyagiUretimleri 
                                            ALTER COLUMN GelisKg DECIMAL(18,2) NULL";
                    
                    using (SqlCommand cmd = new SqlCommand(alterTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata mesajını loglama, kullanıcıya gösterme
            System.Diagnostics.Debug.WriteLine("GelisKg sütununun NULL kabul etme kontrolü sırasında hata: " + ex.Message);
        }
    }
}
