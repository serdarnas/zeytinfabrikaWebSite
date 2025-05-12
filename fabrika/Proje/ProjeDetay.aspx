<%@ Page Title="Proje Detay" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ProjeDetay.aspx.cs" Inherits="fabrika_ProjeDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <div class="row">
                        <div class="col-md-8">
                            <h3>
                                <asp:Label ID="lblProjeAdi" runat="server" Text="16AAF564"></asp:Label>
                            </h3>
                            <small>
                                <asp:Label ID="lblProjeAciklama" runat="server" Text="2024 BAKIM"></asp:Label></small>
                        </div> 
                    </div>
                </header>
            </section>
        </div>
    </div>

    <!-- Finansal Özet Kartları -->
    <div class="row state-overview">
        <!-- Masraflar Kartı -->
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #FF6B6B;">
                    <i class="icon-credit-card"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblMasraflar" runat="server" Text="47.439,77 TL"></asp:Label></h4>
                    <p>Masraflar</p>
                </div>
            </section>
        </div>

        <!-- Alışlar Kartı -->
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #4ECDC4;">
                    <i class="icon-tags"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblAlislar" runat="server" Text="4.083,67 TL"></asp:Label></h4>
                    <p>Alışlar</p>
                </div>
            </section>
        </div>

        <!-- Satışlar Kartı -->
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #5CB85C;">
                    <i class="icon-shopping-cart"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblSatislar" runat="server" Text="4.083,67 TL"></asp:Label></h4>
                    <p>Satışlar</p>
                </div>
            </section>
        </div>

        <!-- Ödemeler Kartı -->
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #5B7FB6;">
                    <i class="icon-money"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblOdemeler" runat="server" Text="4.083,67 TL"></asp:Label></h4>
                    <p>Ödemeler</p>
                </div>
            </section>
        </div>
    </div>

    <!-- Hızlı İşlem Butonları -->
    <div class="row">
        <div class="col-lg-12">
            <div class="btn-group">
                <asp:LinkButton ID="btnGirisEkle" runat="server" CssClass="btn btn-shadow btn-primary" Style="margin-right: 5px;">
                    <i class="icon-plus-sign"></i> Giriş Ekle
                </asp:LinkButton>
                <asp:LinkButton ID="btnProjeKapat" runat="server" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                    <i class="icon-folder-close"></i> Proje Kapat
                </asp:LinkButton>
                <asp:LinkButton ID="btnProjeYazdir" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">
                    <i class="icon-print"></i> Proje Yazdır
                </asp:LinkButton>
                <asp:LinkButton ID="btnNotlar" runat="server" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                    <i class="icon-file-text"></i> Notlar
                </asp:LinkButton>
                <asp:LinkButton ID="btnDokumanlar" runat="server" CssClass="btn btn-shadow btn-danger" Style="margin-right: 5px;">
                    <i class="icon-folder-open"></i> Dökümanlar
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <!-- Önceki Masraflar Tablosu -->
    <div class="row" style="margin-top: 20px;">
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ MASRAFLAR <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>Hesap</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>12.07.2024</td>
                                    <td>Araç Güderici/TRAFİK SİGORTASI</td>
                                    <td>27.939,77 TL</td>
                                </tr>
                                <tr>
                                    <td>08.07.2024</td>
                                    <td>Araç Giderleri/AKARY LASTİK</td>
                                    <td>19.500,00 TL</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>

        <!-- Önceki Alışlar Tablosu -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ ALIŞLAR <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>Tedarikçi</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>03.07.2024</td>
                                    <td>TÜYTÜRK DİYARBAKIR TAŞIT MUAYENE İSTASYONLARI İŞL.A.Ş</td>
                                    <td>4.083,67 TL</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Önceki Cari Hareketler Tablosu -->
    <div class="row">
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ CARİ HAREKETLER <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>Tipi</th>
                                    <th>Hesap</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>12.07.2024</td>
                                    <td>Nakit</td>
                                    <td>TÜYTÜRK İSTANBUL TAŞIT MUAYENE İSTASYONLARI İŞL.A.Ş</td>
                                    <td>-4.083,67 TL</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    

    <!-- Önceki Satışlar Tablosu -->
    
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ SATIŞLAR <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <div class="alert alert-warning">
                            <p>Bu proje kapsamında hiç satış işlemi kaydedilmemiş.</p>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

