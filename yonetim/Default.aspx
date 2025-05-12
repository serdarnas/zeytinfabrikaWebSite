<%@ Page Title="Yönetim Paneli" Language="C#" MasterPageFile="~/yonetim/YonetimMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="yonetim_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header"><i class="fa fa-dashboard"></i> Yönetim Paneli</h3>
        </div>
    </div>
    
    <div class="row state-overview">
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol terques">
                    <i class="fa fa-users"></i>
                </div>
                <div class="value">
                    <h1 id="kullaniciSayisi" runat="server">0</h1>
                    <p>Kullanıcı Sayısı</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol red">
                    <i class="fa fa-list"></i>
                </div>
                <div class="value">
                    <h1 id="menuSayisi" runat="server">0</h1>
                    <p>Menü Sayısı</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol yellow">
                    <i class="fa fa-shopping-cart"></i>
                </div>
                <div class="value">
                    <h1 id="siparisCount" runat="server">0</h1>
                    <p>Sipariş Sayısı</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol blue">
                    <i class="fa fa-bar-chart-o"></i>
                </div>
                <div class="value">
                    <h1 id="ciroTutar" runat="server">0₺</h1>
                    <p>Toplam Ciro</p>
                </div>
            </section>
        </div>
    </div>
    
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <i class="fa fa-user"></i> Hoş Geldiniz <asp:Literal ID="ltAdSoyad" runat="server"></asp:Literal>
                </header>
                <div class="panel-body">
                    <div class="alert alert-info fade in">
                        <strong>Zeytin Fabrika Yönetim Paneli'ne Hoş Geldiniz!</strong>
                        <p>
                            Bu yönetim panelinden, menü yönetimi, kullanıcı yetkilendirme ve sistem ayarlarını yapabilirsiniz. Sol menüden ilgili modüle geçiş yapabilirsiniz.
                        </p>
                    </div>
                    
                    <h4>Son Giriş Bilgileri</h4>
                    <div class="table-responsive">
                        <table class="table table-striped table-hover">
                            <tr>
                                <th width="200">Kullanıcı Adı:</th>
                                <td><asp:Literal ID="ltKullaniciAdi" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <th>Ad Soyad:</th>
                                <td><asp:Literal ID="ltKullaniciAdSoyad" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <th>Şirket:</th>
                                <td><asp:Literal ID="ltSirketAdi" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <th>Son Giriş Tarihi:</th>
                                <td><asp:Literal ID="ltSonGirisTarihi" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

