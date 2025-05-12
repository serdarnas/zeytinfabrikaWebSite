<%@ Page Language="C#" AutoEventWireup="true" CodeFile="giris.aspx.cs" Inherits="giris" %>

<!DOCTYPE html>

<html lang="tr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="Mosaddek">
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina">
    <link rel="shortcut icon" href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/img/favicon.png") %>">
    <title>Zeytin Fabrika giriş Paneli</title>
    <!-- Bootstrap core CSS -->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css") %>" rel="stylesheet">
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/bootstrap-reset.css") %>" rel="stylesheet">

    <!--external css-->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/font-awesome.css") %>" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/style.css") %>" rel="stylesheet">
    <link href="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/css/style-responsive.css") %>" rel="stylesheet">

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
        <script src="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/html5shiv.js") %>"></script>
        <script src="<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/respond.min.js") %>"></script>
    <![endif]-->
</head>

<body class="login-body" style="background-image: url(App_Themes/serdarnas_admin_flat/img/fabrika_arka.png);background-repeat: no-repeat)" >

    <div class="container">
        <form class="form-signin" id="form1" runat="server">
            <h2 class="form-signin-heading">Giriş yapin</h2>
            <div class="login-wrap">
        <asp:Label ID="lblMesaj" runat="server" ></asp:Label>
                <asp:login id="Login1" runat="server" onauthenticate="Login1_Authenticate" CssClass="form-control" destinationpageurl="~/fabrika/Default.aspx" RememberMeSet="True">
                  <LayoutTemplate>
                    <div class="login-container" style="width: 260px">
                      <asp:TextBox ID="UserName" runat="server" placeholder="Email" CssClass="form-control"  autofocus ValidationGroup="girisGroup"/>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="UserName" ErrorMessage="E-posta gereklidir" ForeColor="Red" Display="Dynamic" ValidationGroup="girisGroup"></asp:RequiredFieldValidator>
                      <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control"  placeholder="Şifre" ValidationGroup="girisGroup"/>
                        <asp:RequiredFieldValidator ID="rfvSifre" runat="server" ControlToValidate="Password" ErrorMessage="Şifre gereklidir" ForeColor="Red" Display="Dynamic" ValidationGroup="girisGroup"></asp:RequiredFieldValidator>
                      <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Giriş Yap" CssClass="btn btn-lg btn-login btn-block" ValidationGroup="girisGroup"/>
                    </div>
                  </LayoutTemplate>
                </asp:login>
                <br/>
                <label class="checkbox">
                    <input type="checkbox" value="remember-me" checked="checked"> Beni Hatirla
                    <span class="pull-right">
                        <a data-toggle="modal" href="#myModal"> Şifremi Unutum</a>

                    </span>
                </label>
                <%--<a class="" href="Kayit.aspx">Hemen kayıt olun</a>--%>
            </div>



            <div class="registration">
                <p>  Hesabınız yok mu?
                 
                <a class="" href="Kayit.aspx">Hemen kayıt olun</a>
                </p><br/>
            </div>


            <!-- Modal -->
            <div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="myModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">Şifreyi Unutunuzmu ?</h4>
                        </div>
                        <div class="modal-body">
                            <p>Şifrenizi sıfırlamak için aşağıya e-posta adresinizi girin.</p>
                           <asp:TextBox ID="txtSifreSifirlaEmail" TextMode="Email" runat="server" CssClass="form-control placeholder-no-fix" ValidationGroup="sifreUnutumGroup"></asp:TextBox>
                            <%--<input type="text" name="email" placeholder="Email" autocomplete="off" class="form-control placeholder-no-fix">--%>
                        
                            <asp:Label ID="lblSifreSifirlaMesaj" runat="server" ></asp:Label>
                            </div>
                        <div class="modal-footer">
                            <button data-dismiss="modal" class="btn btn-default" type="button">iptal</button>
                           <asp:Button ID="btnSifreSifirla" runat="server" Text="Gönder" CssClass="btn btn-success" OnClick="btnSifreSifirla_Click" ValidationGroup="sifreUnutumGroup" />
                            <%--<button class="btn btn-success" type="button">Submit</button>--%>
                        </div>
                    </div>
                </div>
            </div>
            <!-- modal -->

        </form>

    </div>







    <!-- js placed at the end of the document so the pages load faster -->

    <script src='<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/jquery.js") %>'></script>

    <script src='<%= ResolveUrl("~/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js") %>'></script>


</body>
</html>
