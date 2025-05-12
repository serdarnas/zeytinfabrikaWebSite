using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web; 
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

public partial class iletisim : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Title = "İletişim - Zeytin Fabrika Yönetim Sistemi";
        }
    }

    protected void btnGonder_Click(object sender, EventArgs e)
    {
        try
        {
            // E-posta gönderme işlemi
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("info@zeytinfabrika.com");
            mail.To.Add("info@zeytinfabrika.com");
            mail.Subject = "İletişim Formu: " + txtKonu.Text;
            mail.Body = string.Format(
                "Gönderen: {0}<br/>" +
                "E-posta: {1}<br/>" +
                "Telefon: {2}<br/>" +
                "Konu: {3}<br/>" +
                "Mesaj: {4}",
                txtAdSoyad.Text,
                txtEmail.Text,
                txtTelefon.Text,
                txtKonu.Text,
                txtMesaj.Text
            );
            mail.IsBodyHtml = true;

            // SMTP ayarları
            SmtpClient smtp = new SmtpClient("smtp.zeytinfabrika.com", 587);
            smtp.Credentials = new NetworkCredential("info@zeytinfabrika.com", "sifre");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            // Başarılı mesajı
            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                "alert('Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.');", true);

            // Formu temizle
            txtAdSoyad.Text = "";
            txtEmail.Text = "";
            txtTelefon.Text = "";
            txtKonu.Text = "";
            txtMesaj.Text = "";
        }
        catch (Exception ex)
        {
            // Hata mesajı
            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                "alert('Mesaj gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');", true);
        }
    }
}