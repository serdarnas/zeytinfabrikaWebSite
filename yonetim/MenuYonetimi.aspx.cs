using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

public partial class yonetim_MenuYonetimi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Verileri yükle
            VerileriYukle();
            
            // Üst menü dropdown'ını doldur
            UstMenuleriDoldur();
        }
    }
    
    /// <summary>
    /// Menü verilerini yükler ve görünümlere doldurur
    /// </summary>
    private void VerileriYukle()
    {
        try
        {
            // Grid'i doldur
            GridiDoldur();
            
            // TreeView'ı doldur
            TreeViewDoldur();
        }
        catch (Exception ex)
        {
            // Hata mesajını göster
            pnlHata.Visible = true;
            lblHata.Text = "Veriler yüklenirken bir hata oluştu: " + ex.Message;
        }
    }
    
    /// <summary>
    /// GridView'a menü verilerini doldurur
    /// </summary>
    private void GridiDoldur()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"SELECT m.MenuID, m.MenuAdi, m.SayfaURL, m.Sira, m.YetkiKodu, m.Ikon, m.UstMenuID,
                          ISNULL(um.MenuAdi, 'Ana Menü') AS UstMenuAdi
                          FROM Menu m
                          LEFT JOIN Menu um ON m.UstMenuID = um.MenuID
                          ORDER BY ISNULL(m.UstMenuID, 0), m.Sira";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    gvMenuler.DataSource = dt;
                    gvMenuler.DataBind();
                }
            }
        }
    }
    
    /// <summary>
    /// TreeView'a menü ağacını doldurur
    /// </summary>
    private void TreeViewDoldur()
    {
        tvMenuler.Nodes.Clear();
        
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            // Önce ana menüleri getir
            string sql = @"SELECT MenuID, MenuAdi, SayfaURL, Ikon, YetkiKodu, Sira
                          FROM Menu
                          WHERE UstMenuID IS NULL OR UstMenuID = 0
                          ORDER BY Sira";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Ana menü düğümünü oluştur
                        TreeNode rootNode = new TreeNode();
                        int menuID = Convert.ToInt32(reader["MenuID"]);
                        rootNode.Text = reader["MenuAdi"].ToString();
                        rootNode.Value = menuID.ToString();
                        rootNode.ToolTip = reader["SayfaURL"].ToString();
                        
                        // İkon varsa göster
                        string ikon = reader["Ikon"].ToString();
                        if (!string.IsNullOrEmpty(ikon))
                        {
                            rootNode.Text = string.Format("<i class='fa {0}'></i> {1}", ikon, rootNode.Text);
                        }
                        
                        tvMenuler.Nodes.Add(rootNode);
                        
                        // Alt menüleri ekle
                        AltMenuleriEkle(rootNode, menuID);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Belirtilen üst menü altındaki alt menüleri recursive olarak ekler
    /// </summary>
    private void AltMenuleriEkle(TreeNode parentNode, int ustMenuID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"SELECT MenuID, MenuAdi, SayfaURL, Ikon, YetkiKodu, Sira
                          FROM Menu
                          WHERE UstMenuID = @UstMenuID
                          ORDER BY Sira";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UstMenuID", ustMenuID);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Alt menü düğümünü oluştur
                        TreeNode childNode = new TreeNode();
                        int menuID = Convert.ToInt32(reader["MenuID"]);
                        childNode.Text = reader["MenuAdi"].ToString();
                        childNode.Value = menuID.ToString();
                        childNode.ToolTip = reader["SayfaURL"].ToString();
                        
                        // İkon varsa göster
                        string ikon = reader["Ikon"].ToString();
                        if (!string.IsNullOrEmpty(ikon))
                        {
                            childNode.Text = string.Format("<i class='fa {0}'></i> {1}", ikon, childNode.Text);
                        }
                        
                        parentNode.ChildNodes.Add(childNode);
                        
                        // Daha alt menüler varsa onları da ekle (recursive)
                        AltMenuleriEkle(childNode, menuID);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Üst menü dropdown'ını doldurur
    /// </summary>
    private void UstMenuleriDoldur()
    {
        // Önce var olan öğeleri temizle (Ana Menü seçeneği hariç)
        ddlUstMenu.Items.Clear();
        ddlUstMenu.Items.Add(new ListItem("Ana Menü", "0"));
        
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"SELECT MenuID, MenuAdi, (CASE WHEN UstMenuID IS NULL THEN 0 ELSE UstMenuID END) AS UstMenuID
                          FROM Menu
                          ORDER BY ISNULL(UstMenuID, 0), MenuAdi";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Menü hiyerarşisini oluşturmak için önce tüm menüleri bir sözlüğe alalım
                    Dictionary<int, string> menuler = new Dictionary<int, string>();
                    Dictionary<int, int> ustMenuIDs = new Dictionary<int, int>();
                    
                    while (reader.Read())
                    {
                        int menuID = Convert.ToInt32(reader["MenuID"]);
                        string menuAdi = reader["MenuAdi"].ToString();
                        int ustMenuID = Convert.ToInt32(reader["UstMenuID"]);
                        
                        menuler.Add(menuID, menuAdi);
                        ustMenuIDs.Add(menuID, ustMenuID);
                    }
                    
                    // Hiyerarşik gösterim için her menünün tam adını oluştur
                    foreach (var menuID in menuler.Keys)
                    {
                        StringBuilder fullName = new StringBuilder();
                        int currentMenuID = menuID;
                        bool isFirst = true;
                        
                        // Menünün adını ve üst menü hiyerarşisini ekle
                        while (currentMenuID != 0 && ustMenuIDs.ContainsKey(currentMenuID))
                        {
                            if (isFirst)
                            {
                                fullName.Insert(0, menuler[currentMenuID]);
                                isFirst = false;
                            }
                            else
                            {
                                fullName.Insert(0, menuler[currentMenuID] + " > ");
                            }
                            
                            currentMenuID = ustMenuIDs[currentMenuID];
                            
                            // Döngüyü engelle
                            if (currentMenuID == menuID)
                            {
                                break;
                            }
                        }
                        
                        // Dropdown'a ekle
                        ddlUstMenu.Items.Add(new ListItem(fullName.ToString(), menuID.ToString()));
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// TreeView'da bir düğüm seçildiğinde
    /// </summary>
    protected void tvMenuler_SelectedNodeChanged(object sender, EventArgs e)
    {
        int menuID = Convert.ToInt32(tvMenuler.SelectedNode.Value);
        MenuDetayiGetir(menuID);
        
        // Silme butonunu görünür yap
        btnSilTreeView.Visible = true;
    }
    
    /// <summary>
    /// GridView'da bir satır komut çalıştırdığında
    /// </summary>
    protected void gvMenuler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Duzenle")
        {
            int menuID = Convert.ToInt32(e.CommandArgument);
            MenuDetayiGetir(menuID);
        }
    }
    
    /// <summary>
    /// GridView'da silme işlemi yapıldığında
    /// </summary>
    protected void gvMenuler_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int menuID = Convert.ToInt32(gvMenuler.DataKeys[e.RowIndex].Value);
        
        try
        {
            // Alt menüler varsa silme işlemi iptal
            if (AltMenuVarMi(menuID))
            {
                pnlHata.Visible = true;
                lblHata.Text = "Bu menüye ait alt menüler var. Önce alt menüleri silmelisiniz.";
                e.Cancel = true;
                return;
            }
            
            // Menüyü sil
            MenuSil(menuID);
            
            // Verileri yeniden yükle
            VerileriYukle();
            UstMenuleriDoldur();
            
            // Başarı mesajı
            pnlBasari.Visible = true;
            lblBasari.Text = "Menü başarıyla silindi.";
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
        }
    }
    
    /// <summary>
    /// GridView satır bağlanırken
    /// </summary>
    protected void gvMenuler_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Üst menü hiyerarşisini gösterme
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            
            // İkon sütununu ekleyerek göster
            if (rowView != null && rowView["Ikon"] != DBNull.Value)
            {
                string ikon = rowView["Ikon"].ToString();
                if (!string.IsNullOrEmpty(ikon))
                {
                    // Menü adı sütununa ikon ekle (1. sütun)
                    TableCell cell = e.Row.Cells[1];
                    cell.Text = string.Format("<i class='fa {0}'></i> {1}", ikon, cell.Text);
                }
            }
        }
    }
    
    /// <summary>
    /// Kaydet butonuna tıklandığında
    /// </summary>
    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            int menuID = Convert.ToInt32(hfMenuID.Value);
            
            // Çevrimsel üst menü kontrolü
            if (menuID > 0 && Convert.ToInt32(ddlUstMenu.SelectedValue) == menuID)
            {
                pnlHata.Visible = true;
                lblHata.Text = "Bir menü kendisini üst menü olarak seçemez.";
                return;
            }
            
            if (menuID > 0)
            {
                // Menü güncelleme
                MenuGuncelle(menuID);
                pnlBasari.Visible = true;
                lblBasari.Text = "Menü başarıyla güncellendi.";
            }
            else
            {
                // Yeni menü ekleme
                MenuEkle();
                pnlBasari.Visible = true;
                lblBasari.Text = "Yeni menü başarıyla eklendi.";
            }
            
            // Formu temizle
            FormuSifirla();
            
            // Verileri yeniden yükle
            VerileriYukle();
            UstMenuleriDoldur();
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Kaydetme işlemi sırasında bir hata oluştu: " + ex.Message;
        }
    }
    
    /// <summary>
    /// Vazgeç butonuna tıklandığında
    /// </summary>
    protected void btnVazgec_Click(object sender, EventArgs e)
    {
        FormuSifirla();
    }
    
    /// <summary>
    /// TreeView'daki silme butonuna tıklandığında çalışır
    /// </summary>
    protected void btnSilTreeView_Click(object sender, EventArgs e)
    {
        if (tvMenuler.SelectedNode != null)
        {
            int menuID = Convert.ToInt32(tvMenuler.SelectedNode.Value);
            
            try
            {
                // Alt menüler varsa silme işlemi iptal
                if (AltMenuVarMi(menuID))
                {
                    pnlHata.Visible = true;
                    lblHata.Text = "Bu menüye ait alt menüler var. Önce alt menüleri silmelisiniz.";
                    return;
                }
                
                // Menüyü sil
                MenuSil(menuID);
                
                // Verileri yeniden yükle
                VerileriYukle();
                UstMenuleriDoldur();
                
                // Başarı mesajı
                pnlBasari.Visible = true;
                lblBasari.Text = "Menü başarıyla silindi.";
                
                // Silme butonunu gizle
                btnSilTreeView.Visible = false;
                
                // Formu sıfırla
                FormuSifirla();
            }
            catch (Exception ex)
            {
                pnlHata.Visible = true;
                lblHata.Text = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
            }
        }
    }
    
    /// <summary>
    /// Belirtilen ID'deki menünün detaylarını getirir
    /// </summary>
    private void MenuDetayiGetir(int menuID)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                string sql = "SELECT * FROM Menu WHERE MenuID = @MenuID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MenuID", menuID);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Menü bilgilerini forma doldur
                            hfMenuID.Value = menuID.ToString();
                            txtMenuAdi.Text = reader["MenuAdi"].ToString();
                            txtIkon.Text = reader["Ikon"].ToString();
                            txtSayfaURL.Text = reader["SayfaURL"].ToString();
                            txtSira.Text = reader["Sira"].ToString();
                            txtYetkiKodu.Text = reader["YetkiKodu"].ToString();
                            
                            // Üst menü seçimini ayarla
                            if (reader["UstMenuID"] != DBNull.Value)
                            {
                                string ustMenuID = reader["UstMenuID"].ToString();
                                if (ddlUstMenu.Items.FindByValue(ustMenuID) != null)
                                {
                                    ddlUstMenu.SelectedValue = ustMenuID;
                                }
                            }
                            else
                            {
                                ddlUstMenu.SelectedValue = "0";
                            }
                            
                            // Başlığı güncelle
                            ltBaslik.Text = "Menü Düzenle: " + reader["MenuAdi"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Menü bilgileri yüklenirken bir hata oluştu: " + ex.Message;
        }
    }
    
    /// <summary>
    /// Formu varsayılan haline sıfırlar
    /// </summary>
    private void FormuSifirla()
    {
        hfMenuID.Value = "0";
        txtMenuAdi.Text = "";
        txtIkon.Text = "";
        txtSayfaURL.Text = "";
        txtSira.Text = "10";
        txtYetkiKodu.Text = "";
        ddlUstMenu.SelectedValue = "0";
        ltBaslik.Text = "Yeni Menü Ekle";
        
        // Hata ve başarı mesajlarını gizle
        pnlHata.Visible = false;
        pnlBasari.Visible = false;
    }
    
    /// <summary>
    /// Yeni menü ekler
    /// </summary>
    private void MenuEkle()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"INSERT INTO Menu (UstMenuID, MenuAdi, Ikon, SayfaURL, Sira, YetkiKodu)
                          VALUES (@UstMenuID, @MenuAdi, @Ikon, @SayfaURL, @Sira, @YetkiKodu)";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Üst menü seçilmişse ekle, ana menü seçilmişse NULL olarak ayarla
                if (ddlUstMenu.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@UstMenuID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UstMenuID", Convert.ToInt32(ddlUstMenu.SelectedValue));
                }
                
                cmd.Parameters.AddWithValue("@MenuAdi", txtMenuAdi.Text);
                cmd.Parameters.AddWithValue("@Ikon", string.IsNullOrEmpty(txtIkon.Text) ? DBNull.Value : (object)txtIkon.Text);
                cmd.Parameters.AddWithValue("@SayfaURL", string.IsNullOrEmpty(txtSayfaURL.Text) ? DBNull.Value : (object)txtSayfaURL.Text);
                cmd.Parameters.AddWithValue("@Sira", Convert.ToInt32(txtSira.Text));
                cmd.Parameters.AddWithValue("@YetkiKodu", txtYetkiKodu.Text);
                
                cmd.ExecuteNonQuery();
            }
        }
    }
    
    /// <summary>
    /// Belirtilen menüyü günceller
    /// </summary>
    private void MenuGuncelle(int menuID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = @"UPDATE Menu
                          SET UstMenuID = @UstMenuID,
                              MenuAdi = @MenuAdi,
                              Ikon = @Ikon,
                              SayfaURL = @SayfaURL,
                              Sira = @Sira,
                              YetkiKodu = @YetkiKodu
                          WHERE MenuID = @MenuID";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Üst menü seçilmişse ekle, ana menü seçilmişse NULL olarak ayarla
                if (ddlUstMenu.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@UstMenuID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UstMenuID", Convert.ToInt32(ddlUstMenu.SelectedValue));
                }
                
                cmd.Parameters.AddWithValue("@MenuID", menuID);
                cmd.Parameters.AddWithValue("@MenuAdi", txtMenuAdi.Text);
                cmd.Parameters.AddWithValue("@Ikon", string.IsNullOrEmpty(txtIkon.Text) ? DBNull.Value : (object)txtIkon.Text);
                cmd.Parameters.AddWithValue("@SayfaURL", string.IsNullOrEmpty(txtSayfaURL.Text) ? DBNull.Value : (object)txtSayfaURL.Text);
                cmd.Parameters.AddWithValue("@Sira", Convert.ToInt32(txtSira.Text));
                cmd.Parameters.AddWithValue("@YetkiKodu", txtYetkiKodu.Text);
                
                cmd.ExecuteNonQuery();
            }
        }
    }
    
    /// <summary>
    /// Belirtilen menüyü siler
    /// </summary>
    private void MenuSil(int menuID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = "DELETE FROM Menu WHERE MenuID = @MenuID";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MenuID", menuID);
                cmd.ExecuteNonQuery();
            }
        }
    }
    
    /// <summary>
    /// Belirtilen menünün alt menüleri var mı kontrol eder
    /// </summary>
    private bool AltMenuVarMi(int menuID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            string sql = "SELECT COUNT(*) FROM Menu WHERE UstMenuID = @MenuID";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MenuID", menuID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }

    protected void btnTemizleTekrarlar_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Tekrarlanan URL'leri tespit et
                string sqlFindDuplicates = @"
                    SELECT SayfaURL, COUNT(*) as Adet 
                    FROM Menu 
                    WHERE SayfaURL IS NOT NULL AND SayfaURL <> '' 
                    GROUP BY SayfaURL 
                    HAVING COUNT(*) > 1";
                
                Dictionary<string, List<int>> tekrarEdenURLler = new Dictionary<string, List<int>>();
                
                using (SqlCommand cmdFind = new SqlCommand(sqlFindDuplicates, conn))
                {
                    using (SqlDataReader reader = cmdFind.ExecuteReader())
                    {
                        // Tekrarlanan URL'ler varsa
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string url = reader["SayfaURL"].ToString();
                                tekrarEdenURLler.Add(url, new List<int>());
                            }
                        }
                        else
                        {
                            // Tekrarlanan URL bulunamadı
                            pnlBasari.Visible = true;
                            lblBasari.Text = "Tekrarlanan URL bulunamadı.";
                            return;
                        }
                    }
                }
                
                // Her tekrarlanan URL için en küçük MenuID'yi bulup, diğerlerini temizle
                int temizlenenURLSayisi = 0;
                foreach (string url in tekrarEdenURLler.Keys)
                {
                    // Önce bu URL'ye sahip tüm MenuID'leri al
                    List<int> allMenuIDs = new List<int>();
                    string sqlGetAllMenuIDs = "SELECT MenuID FROM Menu WHERE SayfaURL = @SayfaURL ORDER BY MenuID";
                    using (SqlCommand cmdGetAllIDs = new SqlCommand(sqlGetAllMenuIDs, conn))
                    {
                        cmdGetAllIDs.Parameters.AddWithValue("@SayfaURL", url);
                        using (SqlDataReader reader = cmdGetAllIDs.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allMenuIDs.Add(Convert.ToInt32(reader["MenuID"]));
                            }
                        }
                    }
                    
                    // En küçük MenuID'yi bul (kalacak olan)
                    int minMenuID = allMenuIDs.Min();
                    
                    // Diğer MenuID'leri temizle (en küçük hariç)
                    foreach (int menuID in allMenuIDs)
                    {
                        if (menuID != minMenuID)
                        {
                            string sqlClear = "UPDATE Menu SET SayfaURL = '' WHERE MenuID = @MenuID";
                            using (SqlCommand cmdClear = new SqlCommand(sqlClear, conn))
                            {
                                cmdClear.Parameters.AddWithValue("@MenuID", menuID);
                                cmdClear.ExecuteNonQuery();
                                temizlenenURLSayisi++;
                            }
                        }
                    }
                }
                
                // Başarı mesajı göster
                if (temizlenenURLSayisi > 0)
                {
                    pnlBasari.Visible = true;
                    lblBasari.Text = string.Format("{0} adet tekrarlanan URL temizlendi.", temizlenenURLSayisi);
                    
                    // Listeyi yenile
                    VerileriYukle();
                }
                else
                {
                    pnlBasari.Visible = true;
                    lblBasari.Text = "Tekrarlanan URL bulunamadı.";
                }
            }
        }
        catch (Exception ex)
        {
            pnlHata.Visible = true;
            lblHata.Text = "Tekrarlanan URL'ler temizlenirken bir hata oluştu: " + ex.Message;
        }
    }
}