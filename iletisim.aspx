<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="iletisim.aspx.cs" Inherits="iletisim" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        :root {
            --primary-color: #2c3e50;
            --secondary-color: #18bc9c;
            --dark-color: #222;
            --light-color: #f8f9fa;
        }
        .contact-hero {
            background: linear-gradient(rgba(0,0,0,0.7), rgba(0,0,0,0.7)), url('/images/contact-bg.jpg');
            background-size: cover;
            background-position: center;
            padding: 150px 0 100px;
            color: white;
            text-align: center;
        }

        .contact-section {
            padding: 80px 0;
        }

        .contact-card {
            background: white;
            border-radius: 15px;
            padding: 30px;
            margin-bottom: 30px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
            height: auto;
        }

        .contact-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 15px 30px rgba(0,0,0,0.2);
        }

        .contact-icon {
            font-size: 2.5rem;
            color: var(--primary-color);
            margin-bottom: 20px;
        }

        .contact-title {
            font-size: 1.5rem;
            font-weight: 700;
            margin-bottom: 15px;
            color: var(--dark-color);
        }

        .contact-info {
            color: var(--secondary-color);
            line-height: 1.6;
        }

        .contact-form {
            background: white;
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }

        .form-control {
            border-radius: 10px;
            padding: 12px 15px;
            margin-bottom: 20px;
            border: 1px solid #ddd;
            transition: all 0.3s ease;
        }

        .form-control:focus {
            border-color: var(--primary-color);
            box-shadow: 0 0 0 0.2rem rgba(44, 62, 80, 0.25);
        }

        .submit-btn {
            background-color: var(--primary-color);
            color: white;
            padding: 12px 30px;
            border-radius: 30px;
            border: none;
            font-weight: 600;
            transition: all 0.3s ease;
        }

        .submit-btn:hover {
            background-color: var(--secondary-color);
            transform: translateY(-2px);
        }

        .map-container {
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            margin-top: 30px;
        }

        .map-container iframe {
            width: 100%;
            height: 400px;
            border: 0;
        }

        .social-links {
            margin-top: 20px;
        }

        .social-links a {
            display: inline-block;
            width: 40px;
            height: 40px;
            line-height: 40px;
            text-align: center;
            background: var(--light-color);
            color: var(--primary-color);
            border-radius: 50%;
            margin-right: 10px;
            transition: all 0.3s ease;
        }

        .social-links a:hover {
            background: var(--primary-color);
            color: white;
            transform: translateY(-3px);
        }

        @media (max-width: 768px) {
            .contact-hero {
                padding: 80px 0 50px;
                font-size: 1.2rem;
            }
            .contact-section {
                padding: 40px 0;
            }
            .map-container iframe {
                height: 250px;
            }
        }
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hero Section -->
    <section class="contact-hero">
        <div class="container">
            <h1 class="display-4 mb-4">İletişim</h1>
            <p class="lead">Bizimle iletişime geçin, size yardımcı olmaktan mutluluk duyarız</p>
        </div>
    </section>

    <!-- Contact Section -->
    <section class="contact-section">
        <div class="container">
            <div class="row">
                <!-- Contact Information -->
                <div class="col-lg-4">
                    <div class="contact-card">
                        <i class="fas fa-map-marker-alt contact-icon"></i>
                        <h3 class="contact-title">Adres</h3>
                        <p class="contact-info">
                            Zeytinli Mah. Şehit Hamdi Bey Cad. No:33<br>
                            Edremit/Balıkesir
                        </p>
                    </div>
                    <div class="contact-card">
                        <i class="fas fa-phone contact-icon"></i>
                        <h3 class="contact-title">Telefon</h3>
                        <p class="contact-info">
                            +90 542 523 41 02<br> 
                        </p>
                    </div>
                    <div class="contact-card">
                        <i class="fas fa-envelope contact-icon"></i>
                        <h3 class="contact-title">E-posta</h3>
                        <p class="contact-info">
                            info@zeytinfabrika.com.tr<br>
                            destek@zeytinfabrika.com.tr
                        </p>
                    </div>
                    <div class="contact-card">
                        <i class="fas fa-clock contact-icon"></i>
                        <h3 class="contact-title">Çalışma Saatleri</h3>
                        <p class="contact-info">
                            Pazartesi - Cuma: 09:00 - 18:00<br>
                            Cumartesi: 09:00 - 14:00<br>
                            Pazar: Kapalı
                        </p>
                    </div>
                </div>

                <!-- Contact Form -->
                <div class="col-lg-8">
                    <div class="contact-form">
                        <h3 class="mb-4">Bize Mesaj Gönderin</h3>
                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label for="txtAdSoyad">Adınız Soyadınız</label>
                                <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" placeholder="Adınız Soyadınız"></asp:TextBox>
                            </div>
                            <div class="col-md-6 form-group">
                                <label for="txtEmail">E-posta Adresiniz</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="E-posta Adresiniz" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label for="txtTelefon">Telefon Numaranız</label>
                                <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="Telefon Numaranız"></asp:TextBox>
                            </div>
                            <div class="col-md-6 form-group">
                                <label for="txtKonu">Konu</label>
                                <asp:TextBox ID="txtKonu" runat="server" CssClass="form-control" placeholder="Konu"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtMesaj">Mesajınız</label>
                            <asp:TextBox ID="txtMesaj" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Mesajınız"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnGonder" runat="server" Text="Gönder" CssClass="submit-btn" OnClick="btnGonder_Click" />
                    </div>

                    <!-- Map -->
                    <div class="map-container">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3059.123456789012!2d26.12345678901234!3d39.12345678901234!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14b1234567890123%3A0x1234567890123456!2sZeytinli%20Mahallesi%2C%20Edremit%2FBal%C4%B1kesir!5e0!3m2!1str!2str!4v1234567890123!5m2!1str!2str" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content> 

