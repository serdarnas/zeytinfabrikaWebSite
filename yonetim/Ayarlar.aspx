<%@ Page Title="Sistem Ayarları" Language="C#" MasterPageFile="~/yonetim/YonetimMasterPage.master" AutoEventWireup="true" CodeFile="Ayarlar.aspx.cs" Inherits="yonetim_Ayarlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header"><i class="fa fa-gear"></i> Sistem Ayarları</h3>
        </div>
    </div>
    
    <!-- Hata ve Başarı Mesajları -->
    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-block alert-danger fade in" Visible="false">
        <button data-dismiss="alert" class="close close-sm" type="button">
            <i class="fa fa-times"></i>
        </button>
        <h4><i class="fa fa-times-circle"></i> Hata!</h4>
        <asp:Label ID="lblHata" runat="server" Text=""></asp:Label>
    </asp:Panel>
    
    <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-block alert-success fade in" Visible="false">
        <button data-dismiss="alert" class="close close-sm" type="button">
            <i class="fa fa-times"></i>
        </button>
        <h4><i class="fa fa-check-circle"></i> Başarılı!</h4>
        <asp:Label ID="lblBasari" runat="server" Text=""></asp:Label>
    </asp:Panel>
    
    <div class="row">
        <div class="col-md-12">
            <section class="panel">
                <header class="panel-heading">
                    Genel Ayarlar
                </header>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Şirket Adı</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSirketAdi" runat="server" CssClass="form-control" placeholder="Şirket Adı"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">E-posta</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="ornek@sirket.com"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Telefon</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="0 (555) 555-5555"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Adres</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" placeholder="Adres" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Logo</label>
                            <div class="col-sm-9">
                                <asp:FileUpload ID="fuLogo" runat="server" CssClass="form-control" />
                                <div class="m-t-10">
                                    <asp:Image ID="imgLogo" runat="server" CssClass="img-thumbnail" Width="150" />
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="col-sm-offset-3 col-sm-9">
                                <asp:Button ID="btnKaydet" runat="server" Text="Ayarları Kaydet" CssClass="btn btn-success" OnClick="btnKaydet_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <section class="panel">
                <header class="panel-heading">
                    E-posta Ayarları
                </header>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">SMTP Sunucu</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSMTPServer" runat="server" CssClass="form-control" placeholder="smtp.example.com"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">SMTP Port</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSMTPPort" runat="server" CssClass="form-control" placeholder="587"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">E-posta Adresi</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSMTPEmail" runat="server" CssClass="form-control" placeholder="bilgi@sirket.com"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Şifre</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSMTPPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">SSL Kullan</label>
                            <div class="col-sm-9">
                                <asp:CheckBox ID="chkSSL" runat="server" />
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="col-sm-offset-3 col-sm-9">
                                <asp:Button ID="btnEmailTest" runat="server" Text="Test E-postası Gönder" CssClass="btn btn-info" OnClick="btnEmailTest_Click" />
                                <asp:Button ID="btnEmailKaydet" runat="server" Text="E-posta Ayarlarını Kaydet" CssClass="btn btn-success" OnClick="btnEmailKaydet_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content> 