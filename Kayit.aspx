<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Kayit.aspx.cs" Inherits="Kayit" %>

<!DOCTYPE html>
<html lang="tr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="Mosaddek">
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina">
    <link rel="shortcut icon" href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/img/favicon.png") %>">
    <title>Zeytin Fabrika Kayit</title>
    <!-- Bootstrap core CSS -->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css") %>" rel="stylesheet">
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/bootstrap-reset.css") %>" rel="stylesheet">
    <!--external css-->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/font-awesome.css") %>" rel="stylesheet">
    <!-- Custom styles for this template -->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/style.css") %>" rel="stylesheet">
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/style-responsive.css") %>" rel="stylesheet">

    <style>
        body.login-body {
            background: url('<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/img/fabrika_arka.png") %>') no-repeat center center fixed;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
            position: relative;
        }

        body.login-body::before {
            content: '';
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background: rgba(0, 0, 0, 0.5);
            z-index: 0;
        }

        .container {
            position: relative;
            z-index: 1;
        }

        .form-signin {
            background: rgba(255, 255, 255, 0.95);
            border-radius: 10px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.3);
            padding: 30px;
            margin-top: 20px;
            position: relative;
        }

        .form-signin-heading {
            color: #333;
            margin-bottom: 25px;
            text-align: center;
        }

        .login-wrap {
            padding: 20px;
        }

        .form-control {
            margin-bottom: 15px;
            position: relative;
            z-index: 2;
        }

        .alert-info {
            background: rgba(255, 255, 255, 0.9);
            position: relative;
            z-index: 2;
        }

        /* Mobil cihazlar için ek düzenlemeler */
        @media (max-width: 768px) {
            .form-signin {
                margin-top: 50px;
                padding: 20px;
            }
        }
    </style>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
        <script src="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/html5shiv.js") %>"></script>
        <script src="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/respond.min.js") %>"></script>
    <![endif]-->
</head>
<body class="login-body">
    <div class="container">
        <form class="form-signin" id="form1" runat="server">
            <h2 class="form-signin-heading">Ücretsiz Hesap Oluşturun,
                30 Gün Boyunca Deneyin</h2>
            <div class="login-wrap">

                <p>Şirket Bilgileri</p>
                <asp:TextBox ID="txtSirketAdi" runat="server" CssClass="form-control" placeholder="Şirket Adı" />
                <asp:RequiredFieldValidator ID="rfvSirketAdi" runat="server"
                    ControlToValidate="txtSirketAdi" ErrorMessage="Şirket adı gereklidir"
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                <p>Kullanici Bilgileri</p>
                <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" placeholder="Ad Soyad"></asp:TextBox>
                <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="0 ile girin Telefon"></asp:TextBox>
                <asp:Label ID="lblEmailKayitli" runat="server"  ForeColor="red"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                    ErrorMessage="Geçerli bir email adresi giriniz"
                    ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                
                <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" TextMode="Password" placeholder="Şifre" />
                <asp:RequiredFieldValidator ID="rfvSifre" runat="server"
                    ControlToValidate="txtSifre"
                    ErrorMessage="Şifre gereklidir"
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revSifre" runat="server"
                    ControlToValidate="txtSifre"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"
                    ErrorMessage="Şifre en az 8 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir"
                    ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                <asp:TextBox ID="txtSifreTekrari" runat="server" CssClass="form-control" TextMode="Password" placeholder="Şifre Tekrarı" />
                <asp:RequiredFieldValidator ID="rfvSifreTekrari" runat="server"
                    ControlToValidate="txtSifreTekrari"
                    ErrorMessage="Şifre tekrarı gereklidir"
                    ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvSifre" runat="server"
                    ControlToValidate="txtSifreTekrari"
                    ControlToCompare="txtSifre"
                    ErrorMessage="Şifreler eşleşmiyor"
                    ForeColor="Red" Display="Dynamic"></asp:CompareValidator>

                <div class="alert alert-info mt-3">
                    <small>
                        <strong>Şifre gereksinimleri:</strong><br />
                        - En az 8 karakter uzunluğunda<br />
                        - En az bir büyük harf<br />
                        - En az bir küçük harf<br />
                        - En az bir rakam<br />
                        - En az bir özel karakter
                    </small>
                </div>

                <label class="checkbox">
                    <input type="checkbox" value="Bu koşulu kabul et">
                    Hizmet Şartları ve Gizlilik Politikası'nı kabul ediyorum. Hesabınızı oluşturarak <a href="ProgramSonKullaniciSozlesmesi.aspx">Kullanım Koşulları</a>'nı kabul etmiş olursunuz.
                </label>
                <asp:Button ID="btnKayit" runat="server" Text="Kayit" CssClass="btn btn-lg btn-login btn-block" OnClick="btnKayit_Click" />
                
                <div class="registration">
                    Zaten Kayıtlı iseniz.
                    <a class="" href="giris.aspx">Giriş yapin
                    </a>
                </div>
            </div>
        </form>
    </div>

</body>
</html>
