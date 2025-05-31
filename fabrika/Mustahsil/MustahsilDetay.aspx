<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MustahsilDetay.aspx.cs" Inherits="fabrika_Mustahsil_MustahsilDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Müstahsil Detay Bilgileri</h3>
                </header>
            </section>
        </div>
    </div>

    <!-- Müstahsil Bilgileri Kartı -->
    <div class="row">
        <div class="col-lg-12">
            <div class="panel" style="background-color: #f8f4e8; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
                <div class="panel-body">
                    <div class="row">
                        <!-- Müstahsil Profil Resmi ve Temel Bilgiler -->
                        <div class="col-md-8">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Image ID="MustahsilResim" runat="server" Width="100px" />
                                </div>
                                <div class="col-md-9">
                                    <h4>
                                        <asp:Label ID="lblMustahsilAdi" runat="server" Text=""></asp:Label></h4>
                                    <p>
                                        <i class="icon-home"></i>
                                        <asp:Label ID="lblAdres" runat="server" Text=""></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-phone"></i>
                                        <asp:Label ID="lblCepTelefonu" runat="server" Text=""></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-mail-forward"></i>
                                        <asp:Label ID="lblmail" runat="server" Text=""></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-user"></i>
                                        <asp:Label ID="lblTCKimlik" runat="server" Text=""></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                       
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bakiye Bilgileri -->
    <div class="row state-overview">
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #FF6B6B;">
                    <i class="icon-credit-card"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblAlacakBakiye" runat="server" Text=""></asp:Label></h4>
                    <p>Alacak Bakiyesi</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #4ECDC4;">
                    <i class="icon-leaf"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblUrunMiktari" runat="server" Text=""></asp:Label></h4>
                    <p>Toplam Ürün Miktarı</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #45B7D7;">
                    <i class="icon-money"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblOdemeMiktari" runat="server" Text=""></asp:Label></h4>
                    <p>Toplam Ödeme</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #5CB85C;">
                    <i class="icon-bar-chart"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblCiroToplam" runat="server" Text=""></asp:Label></h4>
                    <p>Ciro</p>
                </div>
            </section>
        </div>
    </div>

    <!-- Hızlı İşlem Butonları -->
    <div class="row">
        <div class="col-lg-12">
            <div class="btn-group">
                <asp:HyperLink ID="hplinkUrunAlimi" runat="server" CssClass="btn btn-shadow btn-primary" Style="margin-right: 5px;">
                    <i class="icon-leaf"></i> Ürün Alımı
                </asp:HyperLink>
                <asp:HyperLink ID="hplinkZeytinyagıicinUrunAlimi" runat="server"   CssClass="btn btn-shadow btn-primary" Style="margin-right: 5px;">
                    <i class="icon-leaf"></i> Zeytinyağı için Ürün Teslim
                </asp:HyperLink>
                <asp:HyperLink ID="btnOdemeYap" runat="server" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                    <i class="icon-money"></i> Ödeme Yap
                </asp:HyperLink>
                <asp:HyperLink ID="btnHesapEkstresi" runat="server" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                    <i class="icon-list"></i> Hesap Ekstresi
                </asp:HyperLink>
                <asp:HyperLink ID="hplinkMustahsilGuncelle" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">
                    <i class="icon-edit"></i> Müstahsil Güncelle
                </asp:HyperLink>
                <asp:LinkButton ID="btnDigerIslemler" runat="server" CssClass="btn btn-shadow btn-default" Style="margin-right: 5px;">
                    <i class="icon-cog"></i> Ekstra İşlemler
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <!-- Önceki Ürün Alımları ve Ödemeler Tabloları -->
    <div class="row" style="margin-top: 20px;">
        <!-- Önceki Ürün Alımları Tablosu -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ ÜRÜN ALIMLARI <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>No</th>
                                    <th>Ürün</th>
                                    <th>Miktar</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="5" class="text-center">Kayıt bulunamadı</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>

        <!-- Önceki Ödemeler Tablosu -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ ÖDEMELER <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>Tutar</th>
                                    <th>Şekli</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="3" class="text-center">Kayıt bulunamadı</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Müstahsil Makbuzu Bilgileri -->
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h4>MÜSTAHSİL MAKBUZLARI <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>Makbuz No</th>
                                    <th>Ürün</th>
                                    <th>Miktar</th>
                                    <th>Tutar</th>
                                    <th>Durum</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="6" class="text-center">Kayıt bulunamadı</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

