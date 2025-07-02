<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Mobil_Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Zeytin Fabrikası - Mobil Giriş</title>
    
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html, body {
            height: 100%;
            overflow-x: hidden;
        }

        body {
            background: linear-gradient(135deg, #667eea 0%, #56c468 100%);
            font-family: Arial, sans-serif;
            display: flex;
            align-items: center;
            justify-content: center;
            min-height: 100vh;
            padding: 20px;
        }

        .login-box {
            background: white;
            padding: 40px 30px;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
            width: 100%;
            max-width: 350px;
            margin: auto;
            position: relative;
        }

        .logo {
            text-align: center;
            margin-bottom: 30px;
        }

        .logo h1 {
            color: #4CAF50;
            font-size: 24px;
            margin: 10px 0 5px 0;
        }

        .logo p {
            color: #666;
            font-size: 14px;
            margin: 0;
        }

        .input-group {
            margin-bottom: 20px;
        }

        .input-group input {
            width: 100%;
            padding: 15px;
            border: 2px solid #ddd;
            border-radius: 8px;
            font-size: 16px;
            box-sizing: border-box;
        }

        .input-group input:focus {
            border-color: #4CAF50;
            outline: none;
        }

        .btn-login {
            width: 100%;
            padding: 15px;
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            margin-top: 10px;
        }

        .btn-login:hover {
            background: #45a049;
        }

        .checkbox-group {
            margin: 15px 0;
            font-size: 14px;
        }

        .error-msg {
            background: #ffebee;
            border-left: 4px solid #f44336;
            color: #c62828;
            padding: 10px;
            margin-bottom: 20px;
            border-radius: 4px;
        }

        .footer {
            text-align: center;
            color: #666;
            font-size: 12px;
            margin-top: 20px;
        }

        @media (max-width: 480px) {
            body {
                padding: 10px;
            }
            
            .login-box {
                padding: 30px 20px;
                margin: auto;
                max-width: 100%;
            }
        }

        /* Ekstra ortalama için */
        form {
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <!-- Logo -->
            <div class="logo">
                <div style="font-size: 50px; color: #4CAF50;">🌱</div>
                <h1>Zeytin Fabrikası</h1>
                <p>Mobil Yönetim Paneli</p>
            </div>

            <!-- Hata Mesajı -->
            <asp:Panel ID="pnlHata" runat="server" CssClass="error-msg" Visible="false">
                <asp:Label ID="lblHata" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Email -->
            <div class="input-group">
                <asp:TextBox ID="txtEmail" runat="server" 
                    placeholder="E-posta adresiniz" 
                    TextMode="Email" 
                    CssClass="form-control"></asp:TextBox>
            </div>

            <!-- Şifre -->
            <div class="input-group">
                <asp:TextBox ID="txtSifre" runat="server" 
                    TextMode="Password" 
                    placeholder="Şifreniz" 
                    CssClass="form-control"></asp:TextBox>
            </div>

            <!-- Beni Hatırla -->
            <div class="checkbox-group">
                <asp:CheckBox ID="chkBeniHatirla" runat="server" Text=" Beni Hatırla" />
            </div>

            <!-- Giriş Butonu -->
            <asp:Button ID="btnGiris" runat="server" 
                Text="Giriş Yap" 
                CssClass="btn-login" 
                OnClick="btnGiris_Click" />

            <!-- Footer -->
            <div class="footer">
                <p>&copy; 2024 Zeytin Fabrikası<br>Mobil Yönetim v1.0</p>
            </div>
        </div>
    </form>
</body>
</html>
