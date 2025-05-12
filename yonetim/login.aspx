<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="yonetim_login" %>

<!DOCTYPE html>

<html lang="tr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Zeytin Fabrika Yönetim Paneli">
    <meta name="author" content="Zeytin Fabrika">
    <link rel="shortcut icon" href="/App_Themes/serdarnas_admin_flat/img/favicon.png">
    <title>Zeytin Fabrika Yönetim Paneli - Giriş</title>
    <!-- Bootstrap core CSS -->
    <link href="/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css" rel="stylesheet">
    <link href="/App_Themes/serdarnas_admin_flat/css/bootstrap-reset.css" rel="stylesheet">
    <!--external css-->
    <link href="/App_Themes/serdarnas_admin_flat/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="/App_Themes/serdarnas_admin_flat/css/style.css" rel="stylesheet">
    <link href="/App_Themes/serdarnas_admin_flat/css/style-responsive.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
    <script src="/App_Themes/serdarnas_admin_flat/js/html5shiv.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/respond.min.js"></script>
    <![endif]-->
</head>

<body class="login-body">
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-signin">
                <h2 class="form-signin-heading">Yönetim Paneli Girişi</h2>
                <div class="login-wrap">
                    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-danger" Visible="false">
                        <asp:Label ID="lblHata" runat="server"></asp:Label>
                    </asp:Panel>
                
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-user"></i></span>
                        <asp:TextBox ID="txtKullaniciAdi" runat="server" CssClass="form-control" placeholder="Kullanıcı Adı" autofocus></asp:TextBox>
                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                        <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" placeholder="Şifre" TextMode="Password"></asp:TextBox>
                    </div>
                    <label class="checkbox">
                        <asp:CheckBox ID="chkBeniHatirla" runat="server" Text=" Beni Hatırla" />
                        <span class="pull-right">
                            <a data-toggle="modal" href="#sifremiUnuttum">Şifremi Unuttum?</a>
                        </span>
                    </label>
                    <asp:Button ID="btnGiris" runat="server" Text="Giriş Yap" CssClass="btn btn-lg btn-login btn-block" OnClick="btnGiris_Click" />
                </div>
            </div>

            <!-- Şifremi Unuttum Modal -->
            <div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="sifremiUnuttum" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">Şifrenizi mi Unuttunuz?</h4>
                        </div>
                        <div class="modal-body">
                            <p>Şifrenizi sıfırlamak için e-posta adresinizi girin.</p>
                            <asp:TextBox ID="txtSifreSifirlaEmail" runat="server" CssClass="form-control" placeholder="E-posta" autocomplete="off"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblSifreSifirlaMesaj" runat="server"></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <button data-dismiss="modal" class="btn btn-default" type="button">İptal</button>
                            <asp:Button ID="btnSifreSifirla" runat="server" CssClass="btn btn-success" Text="Gönder" OnClick="btnSifreSifirla_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- modal -->
        </div>

        <!-- js placed at the end of the document so the pages load faster -->
        <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
        <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
