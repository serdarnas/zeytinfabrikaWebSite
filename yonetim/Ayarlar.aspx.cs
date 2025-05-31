using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;

public partial class yonetim_Ayarlar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Session kontrolü
            SessionHelper.KullaniciOturumKontrol();
            
            // Ayarları yükle
            AyarlariYukle();
        }
    }
    
    private void AyarlariYukle()
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Şirket bilgilerini al
            Sirketler sirket = db.Sirketlers.FirstOrDefault(s => s.SirketID == SessionHelper.GetSirketID());
            
            if (sirket != null)
            {
                txtSirketAdi.Text = sirket.SirketAdi;
                txtEmail.Text = sirket.Email;
                txtTelefon.Text = sirket.Telefon;
                txtAdres.Text = sirket.Adres;
                
                // Logo gösterimi
                if (!string.IsNullOrEmpty(sirket.LogoURL))
                {
                    imgLogo.ImageUrl = sirket.LogoURL;
                    imgLogo.Visible = true;
                }
                else
                {
                    imgLogo.Visible = false;
                }
                
                // E-posta ayarları
                //txtSMTPServer.Text = sirket.SMTPServer;
                //txtSMTPPort.Text = sirket.SMTPPort.ToString();
                //txtSMTPEmail.Text = sirket.SMTPEmail;
                //txtSMTPPassword.Text = sirket.SMTPPassword;
                //chkSSL.Checked = sirket.SMTPSSL.GetValueOrDefault();
            }
        }
        catch (Exception ex)
        {
            //pnlHata.Visible = true;
            //lblHata.Text = "Ayarlar yüklenirken hata oluştu: " + ex.Message;
            MessageHelper.ShowErrorMessage(this, "Ayarla", "Ayarlar yüklenirken hata oluştu:" + ex.Message);
        }
    }
    
    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Şirket bilgilerini al
            Sirketler sirket = db.Sirketlers.FirstOrDefault(s => s.SirketID == SessionHelper.GetSirketID());
            
            if (sirket != null)
            {
                sirket.SirketAdi = txtSirketAdi.Text;
                sirket.Email = txtEmail.Text;
                sirket.Telefon = txtTelefon.Text;
                sirket.Adres = txtAdres.Text;
                
                // Logo yükleme
                if (fuLogo.HasFile)
                {
                    string dosyaUzantisi = Path.GetExtension(fuLogo.FileName).ToLower();
                    
                    // Sadece resim dosyaları
                    if (dosyaUzantisi == ".jpg" || dosyaUzantisi == ".jpeg" || dosyaUzantisi == ".png" || dosyaUzantisi == ".gif")
                    {
                        string yeniDosyaAdi = "sirket_logo_" + SessionHelper.GetSirketID() + dosyaUzantisi;
                        string dosyaYolu = Server.MapPath("~/img/logolar/") + yeniDosyaAdi;
                        
                        // Klasör yoksa oluştur
                        if (!Directory.Exists(Server.MapPath("~/img/logolar/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/img/logolar/"));
                        }
                        
                        // Dosyayı kaydet
                        fuLogo.SaveAs(dosyaYolu);
                        
                        // Veritabanına kaydet
                        sirket.LogoURL = "~/img/logolar/" + yeniDosyaAdi;
                        
                        // Logo gösterimi güncelle
                        imgLogo.ImageUrl = sirket.LogoURL;
                        imgLogo.Visible = true;
                    }
                    else
                    {
                        //pnlHata.Visible = true;
                        //lblHata.Text = "Sadece JPG, JPEG, PNG ve GIF formatında resim yükleyebilirsiniz.";
                        MessageHelper.ShowErrorMessage(this, "Ayarla", "Sadece JPG, JPEG, PNG ve GIF formatında resim yükleyebilirsiniz.");
                        return;
                    }
                }
                
                // Değişiklikleri kaydet
                db.SubmitChanges();
                
                // Session'daki şirket adını güncelle
                Session["SirketAdi"] = sirket.SirketAdi;
                
                //pnlBasari.Visible = true;
                //lblBasari.Text = "Ayarlar başarıyla güncellendi.";
                MessageHelper.ShowSuccessMessage(this, "Ayarla", "Ayarlar başarıyla güncellendi.");
            }
        }
        catch (Exception ex)
        {
            //pnlHata.Visible = true;
            //lblHata.Text = "Ayarlar kaydedilirken hata oluştu: " + ex.Message;
            MessageHelper.ShowSuccessMessage(this, "Ayarla", "Ayarlar kaydedilirken hata oluştu: " + ex.Message);
        }
    }
    
    protected void btnEmailKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext();
            
            // Şirket bilgilerini al
            Sirketler sirket = db.Sirketlers.FirstOrDefault(s => s.SirketID == SessionHelper.GetSirketID());
            
           
        }
        catch (Exception ex)
        {
            //pnlHata.Visible = true;
            //lblHata.Text = "E-posta ayarları kaydedilirken hata oluştu: " + ex.Message;
            MessageHelper.ShowErrorMessage(this, "Ayarla", "E-posta ayarları kaydedilirken hata oluştu: " + ex.Message);
        }
    }
    
    protected void btnEmailTest_Click(object sender, EventArgs e)
    {
        try
        {
            // SMTP bilgilerini al
            string smtpServer = txtSMTPServer.Text;
            int smtpPort = Convert.ToInt32(txtSMTPPort.Text);
            string smtpEmail = txtSMTPEmail.Text;
            string smtpPassword = txtSMTPPassword.Text;
            bool smtpSSL = chkSSL.Checked;
            
            // Mail gönderimi için gerekli bilgiler
            string kime = txtEmail.Text;
            string konu = "Test E-postası";
            string icerik = "Bu bir test e-postasıdır. E-posta ayarlarınız başarıyla çalışıyor.";
            
            // Mail gönder
            using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
            {
                smtp.EnableSsl = smtpSSL;
                smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpEmail, txtSirketAdi.Text);
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.Body = icerik;
                    mail.IsBodyHtml = true;
                    
                    smtp.Send(mail);
                }
            }
            
            //pnlBasari.Visible = true;
            //lblBasari.Text = "Test e-postası başarıyla gönderildi.";
            MessageHelper.ShowSuccessMessage(this, "Ayarla", "Test e-postası başarıyla gönderildi. ");
        }
        catch (Exception ex)
        {
            //pnlHata.Visible = true;
            //lblHata.Text = "E-posta gönderilirken hata oluştu: " + ex.Message;
            MessageHelper.ShowErrorMessage(this, "Ayarla", "E-posta gönderilirken hata oluştu:  " + ex.Message);
        }
    }
} 