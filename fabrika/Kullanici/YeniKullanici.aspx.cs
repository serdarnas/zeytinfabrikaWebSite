using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

public partial class fabrika_Kullanici_YeniKullanici : System.Web.UI.Page
{
    private int _kullaniciID = 0;
    private bool _duzenlemeModu = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        // URL'den gelen KullaniciID parametresini al ve düzenleme modunu belirle
        if (Request.QueryString["KullaniciID"] != null)
        {
            if (int.TryParse(Request.QueryString["KullaniciID"], out _kullaniciID))
            {
                _duzenlemeModu = true;
                
                // Düzenleme modunda şifre zorunlu değil
                rfvSifre.Enabled = false;
            }
        }

        if (!IsPostBack)
        {
            // Sayfa ilk yüklendiğinde
            txtOlusturmaTarihi.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            
            // Menü ağacını doldur
            DoldurMenuYetkileri();

            // Düzenleme modunda kullanıcı bilgilerini yükle
            if (_duzenlemeModu)
            {
                KullaniciBilgileriniYukle();
                btnKaydet.Text = "Değişiklikleri Kaydet";
            }
        }
    }

    private void DoldurMenuYetkileri()
    {
        try
        {
            // PlaceHolder'ı temizle
            phMenuItems.Controls.Clear();
            
            // Kullanıcının yetkileriyle ilgili liste
            Dictionary<string, bool> kullaniciYetkiler = new Dictionary<string, bool>();
            
            // Düzenleme modundaysa mevcut yetkileri al
            if (_duzenlemeModu)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT YetkiKodu FROM [KullaniciYetki] WHERE KullaniciID = @KullaniciID AND Aktif = 1";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@KullaniciID", _kullaniciID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                kullaniciYetkiler[reader["YetkiKodu"].ToString()] = true;
                            }
                        }
                    }
                }
            }

            // Üst menüleri al ve işle
            Dictionary<int, string> ustMenuler = new Dictionary<int, string>();
            Dictionary<int, List<MenuItemInfo>> altMenuler = new Dictionary<int, List<MenuItemInfo>>();
            
            string connectionString2 = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString2))
            {
                conn.Open();
                
                // Üst menüleri al
                string sql = "SELECT MenuID, MenuAdi FROM Menu WHERE UstMenuID IS NULL ORDER BY Sira";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int menuID = Convert.ToInt32(reader["MenuID"]);
                            string menuAdi = reader["MenuAdi"].ToString();
                            ustMenuler.Add(menuID, menuAdi);
                            altMenuler[menuID] = new List<MenuItemInfo>();
                        }
                    }
                }
                
                // Alt menüleri al
                foreach (int ustMenuID in ustMenuler.Keys)
                {
                    string sql2 = "SELECT MenuID, MenuAdi, YetkiKodu FROM Menu WHERE UstMenuID = @UstMenuID ORDER BY Sira";
                    using (SqlCommand cmd = new SqlCommand(sql2, conn))
                    {
                        cmd.Parameters.AddWithValue("@UstMenuID", ustMenuID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string yetkiKodu = reader["YetkiKodu"].ToString();
                                bool isChecked = kullaniciYetkiler.ContainsKey(yetkiKodu);
                                
                                altMenuler[ustMenuID].Add(new MenuItemInfo
                                {
                                    MenuID = Convert.ToInt32(reader["MenuID"]),
                                    MenuAdi = reader["MenuAdi"].ToString(),
                                    YetkiKodu = yetkiKodu,
                                    IsChecked = isChecked
                                });
                            }
                        }
                    }
                }
            }
            
            // JavaScript ekle - tüm alt menü öğelerini seçme/kaldırma
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>
                function toggleCategoryItems(categoryId, checked) {
                    var items = document.querySelectorAll('[data-category=""' + categoryId + '""] input[type=""checkbox""]');
                    for (var i = 0; i < items.length; i++) {
                        items[i].checked = checked;
                    }
                }
            </script>");
            
            LiteralControl jsLiteral = new LiteralControl(sb.ToString());
            phMenuItems.Controls.Add(jsLiteral);
            
            // HTML oluştur
            int categoryCounter = 1;
            foreach (var ustMenu in ustMenuler)
            {
                int ustMenuID = ustMenu.Key;
                string ustMenuAdi = ustMenu.Value;
                string categoryId = "category_" + categoryCounter;
                
                // Üst menü başlığı ekle
                Panel kategoriPanel = new Panel();
                kategoriPanel.CssClass = "menu-category";
                
                // Kategori için checkbox ekle
                CheckBox chkCategory = new CheckBox();
                chkCategory.ID = "chkCategory_" + ustMenuID;
                chkCategory.CssClass = "category-checkbox";
                chkCategory.Text = ustMenuAdi;
                
                // Checkbox'a tıklandığında alt öğeleri işaretleme/kaldırma
                chkCategory.Attributes["onclick"] = "toggleCategoryItems('" + categoryId + "', this.checked)";
                
                // Tüm alt menü öğeleri işaretli ise kategori checkbox'ını da işaretle
                bool allChecked = true;
                if (altMenuler[ustMenuID].Count > 0)
                {
                    foreach (var altMenu in altMenuler[ustMenuID])
                    {
                        if (!altMenu.IsChecked)
                        {
                            allChecked = false;
                            break;
                        }
                    }
                    chkCategory.Checked = allChecked;
                }
                else
                {
                    chkCategory.Checked = false;
                }
                
                kategoriPanel.Controls.Add(chkCategory);
                phMenuItems.Controls.Add(kategoriPanel);
                
                // Alt menü items container
                Panel menuItemsPanel = new Panel();
                menuItemsPanel.CssClass = "menu-items";
                menuItemsPanel.Attributes["data-category"] = categoryId;
                
                // Alt menü öğelerini ekle
                foreach (var altMenu in altMenuler[ustMenuID])
                {
                    Panel menuItemPanel = new Panel();
                    menuItemPanel.CssClass = "menu-item";
                    
                    // Checkbox oluştur
                    CheckBox chkMenuItem = new CheckBox();
                    chkMenuItem.ID = "chk_" + altMenu.YetkiKodu;
                    chkMenuItem.Text = altMenu.MenuAdi;
                    chkMenuItem.Checked = altMenu.IsChecked;
                    
                    // Checkbox'ı value ile ilişkilendir
                    chkMenuItem.Attributes["data-yetki-kodu"] = altMenu.YetkiKodu;
                    
                    menuItemPanel.Controls.Add(chkMenuItem);
                    menuItemsPanel.Controls.Add(menuItemPanel);
                }
                
                phMenuItems.Controls.Add(menuItemsPanel);
                categoryCounter++;
            }
        }
        catch (Exception ex)
        {
            MessageHelper.ShowErrorMessage(this,"Yeni Kullanci", "Menü yetkileri yüklenirken bir hata oluştu: " + ex.Message);
            
        }
    }
    
    private class MenuItemInfo
    {
        public int MenuID { get; set; }
        public string MenuAdi { get; set; }
        public string YetkiKodu { get; set; }
        public bool IsChecked { get; set; }
    }

    private void KullaniciBilgileriniYukle()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Kullanıcı temel bilgilerini al
                string sql = "SELECT * FROM Kullanicilar WHERE KullaniciID = @KullaniciID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@KullaniciID", _kullaniciID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtAdSoyad.Text = reader["AdSoyad"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtTelefon.Text = reader["Telefon"].ToString();
                            ddlYetki.SelectedValue = reader["YetkiID"].ToString();
                            cbAktif.Checked = Convert.ToBoolean(reader["Aktif"]);
                            txtOlusturmaTarihi.Text = Convert.ToDateTime(reader["OlusturmaTarihi"]).ToString("dd.MM.yyyy HH:mm");
                            
                            if (reader["SonGirisTarihi"] != DBNull.Value)
                            {
                                txtSonGirisTarihi.Text = Convert.ToDateTime(reader["SonGirisTarihi"]).ToString("dd.MM.yyyy HH:mm");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { 
            MessageHelper.ShowErrorMessage(this, "Yeni Kullanci", "Kullanıcı bilgileri yüklenirken bir hata oluştu: " + ex.Message);
        }
    }
    
    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        // Validasyon kontrolü
        if (!Page.IsValid)
        {
            return;
        }
        
        try
        {
            int kullaniciID;
            
            // Düzenleme modu veya yeni kullanıcı oluşturma durumuna göre işlem yap
            if (_duzenlemeModu)
            {
                // Kullanıcı bilgilerini güncelle
                KullaniciBilgileriGuncelle();
                kullaniciID = _kullaniciID;
            }
            else
            {
                // Yeni kullanıcı bilgilerini kaydet
                kullaniciID = KullaniciKaydet();
            }
            
            // Kullanıcı için menü yetkilerini kaydet
            MenuYetkileriniKaydet(kullaniciID);
            
            // Başarılı mesajı göster
          
            MessageHelper.ShowSuccessMessage(this, "Yeni Kullanci", "Kullanıcı bilgileri başarıyla güncellendi. ");
        }
        catch (Exception ex)
        {
          
            MessageHelper.ShowErrorMessage(this, "Yeni Kullanci", "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message);
        }
    }
    
    private int KullaniciKaydet()
    {
        int kullaniciID = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"INSERT INTO Kullanicilar (AdSoyad, Email, Telefon, Sifre, YetkiID, Aktif, OlusturmaTarihi) 
                           VALUES (@AdSoyad, @Email, @Telefon, @Sifre, @YetkiID, @Aktif, @OlusturmaTarihi);
                           SELECT SCOPE_IDENTITY();";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                cmd.Parameters.AddWithValue("@YetkiID", ddlYetki.SelectedValue);
                cmd.Parameters.AddWithValue("@Aktif", cbAktif.Checked);
                cmd.Parameters.AddWithValue("@OlusturmaTarihi", DateTime.Now);
                
                // Kullanıcı ID'yi al
                kullaniciID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        
        return kullaniciID;
    }
    
    private void MenuYetkileriniKaydet(int kullaniciID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            // Kullanıcının mevcut yetkilerini temizle
            string deleteSql = "DELETE FROM [KullaniciYetki] WHERE KullaniciID = @KullaniciID";
            using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
            {
                cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
                cmd.ExecuteNonQuery();
            }
            
            // Sayfa üzerindeki tüm checkboxları kontrol et
            foreach (Control ctrl in phMenuItems.Controls)
            {
                if (ctrl is Panel)
                {
                    // Menü öğeleri paneli
                    foreach (Control itemsCtrl in ctrl.Controls)
                    {
                        if (itemsCtrl is Panel && ((Panel)itemsCtrl).CssClass == "menu-items")
                        {
                            foreach (Control itemCtrl in itemsCtrl.Controls)
                            {
                                if (itemCtrl is Panel && ((Panel)itemCtrl).CssClass == "menu-item")
                                {
                                    foreach (Control chkCtrl in itemCtrl.Controls)
                                    {
                                        if (chkCtrl is CheckBox)
                                        {
                                            CheckBox chk = (CheckBox)chkCtrl;
                                            
                                            if (chk.Checked)
                                            {
                                                // Yetki kodunu al
                                                string yetkiKodu = chk.Attributes["data-yetki-kodu"];
                                                
                                                // Yetkiyi kaydet
                                                string insertSql = "INSERT INTO [KullaniciYetki] (KullaniciID, YetkiKodu, Aktif) VALUES (@KullaniciID, @YetkiKodu, 1)";
                                                using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                                                {
                                                    cmd.Parameters.AddWithValue("@KullaniciID", kullaniciID);
                                                    cmd.Parameters.AddWithValue("@YetkiKodu", yetkiKodu);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    private void KullaniciBilgileriGuncelle()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"UPDATE Kullanicilar 
                           SET AdSoyad = @AdSoyad, 
                               Email = @Email, 
                               Telefon = @Telefon, 
                               YetkiID = @YetkiID, 
                               Aktif = @Aktif
                           WHERE KullaniciID = @KullaniciID";
            
            // Eğer şifre girilmişse, şifreyi de güncelle
            if (!string.IsNullOrEmpty(txtSifre.Text))
            {
                sql = @"UPDATE Kullanicilar 
                        SET AdSoyad = @AdSoyad, 
                            Email = @Email, 
                            Telefon = @Telefon, 
                            Sifre = @Sifre, 
                            YetkiID = @YetkiID, 
                            Aktif = @Aktif
                        WHERE KullaniciID = @KullaniciID";
            }
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@KullaniciID", _kullaniciID);
                cmd.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                cmd.Parameters.AddWithValue("@YetkiID", ddlYetki.SelectedValue);
                cmd.Parameters.AddWithValue("@Aktif", cbAktif.Checked);
                
                if (!string.IsNullOrEmpty(txtSifre.Text))
                {
                    cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                }
                
                cmd.ExecuteNonQuery();
            }
        }
    }
}