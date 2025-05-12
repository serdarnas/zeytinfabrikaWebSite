using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Web;

public static class EmailHelper
{
    private static readonly string SmtpHost = "mail.kurumsaleposta.com";
    private static readonly int SmtpPort = 587;
    private static readonly string SmtpUser = "destek@zeytinfabrika.com.tr";
    private static readonly string SmtpPass = "1Iz2o5._OeSgL6._";
    private static readonly string SenderName = "Zeytin Fabrika Yazilimi";
    private static readonly string SenderEmail = "destek@zeytinfabrika.com.tr";

    private static SmtpClient CreateSmtpClient()
    {
        var client = new SmtpClient(SmtpHost, SmtpPort)
        {
            EnableSsl = false,
            Credentials = new NetworkCredential(SmtpUser, SmtpPass),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 20000
        };
        return client;
    }

    private static MailMessage CreateMailMessage(string to, string subject, string body)
    {
        var mail = new MailMessage();
        mail.From = new MailAddress(SenderEmail, SenderName);
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        return mail;
    }

    public static void SendWelcomeMail(string to, string userName)
    {
        string templatePath = HttpContext.Current.Server.MapPath("~/fabrika/Musteriler/MailSablonlari/hosgeldin.html");
        string body = File.ReadAllText(templatePath);
        body = body.Replace("[Müşteri Adı]", userName);
        string subject = "Zeytin Fabrika'ya Hoş Geldiniz!";
        SendMail(to, subject, body);
    }

    public static void SendForgotPasswordMail(string to, string userName, string newPassword)
    {
        string subject = "Zeytin Fabrika - Şifre Sıfırlama";
        string body = "Merhaba " + userName + ",<br><br>Yeni şifreniz: <b>" + newPassword + " </b><br>Lütfen güvenliğiniz için giriş yaptıktan sonra şifrenizi değiştirin.";
        SendMail(to, subject, body);
    }

    public static void SendTrialEndedMail(string to, string userName)
    {
        string subject = "Zeytin Fabrika - Deneme Süresi Sona Erdi";
        string templatePath = HttpContext.Current.Server.MapPath("~/fabrika/Musteriler/MailSablonlari/demo_bitti.html");
        string body = File.ReadAllText(templatePath);
        body = body.Replace("[Müşteri Adı]", userName);
        SendMail(to, subject, body);
    }

    public static void SendMail(string to, string subject, string body)
    {
        try
        {
            using (var client = CreateSmtpClient())
            using (var mail = CreateMailMessage(to, subject, body))
            {
                client.Send(mail);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Mail gönderilemedi: " + ex.Message, ex);
        }
    }
}