<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Detay.aspx.cs" Inherits="fabrika_Tedarikciler_Detay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Tedarikci Detay Bilgileri</h3>
                </header>
            </section>
        </div>
    </div>

    <!-- Tedarikci Bilgileri Kartı -->
    <div class="row">
        <div class="col-lg-12">
            <div class="panel" style="background-color: #f8f4e8; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
                <div class="panel-body">
                    <div class="row">
                        <!-- Tedarikci Profil Resmi ve Temel Bilgiler -->
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Image ID="TedarikciResim" runat="server" Width="100px" />
                                    <%--<img src="../../App_Themes/serdarnas_admin_flat/img/avatar1_small.jpg" alt="Tedarikci Profil" class="img-circle" style="width: 80px; height: 80px;" />--%>
                                </div>
                                <div class="col-md-9">
                                    <h4>
                                        <asp:Label ID="lblTedarikciAdi" runat="server" Text="3em gıda sanayi ticaret ltd.şti"></asp:Label></h4>
                                    <p>
                                        <i class="icon-home"></i>
                                        <asp:Label ID="lblAdres" runat="server" Text="Cumhuriyet mah.Mehmet cad. No:4 Y/A Bahçelievler/İstanbul"></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-user"></i>
                                        <asp:Label ID="lblYetkili" runat="server" Text="Yetkili: 02169987650"></asp:Label>-<i class="icon-phone"></i>
                                        <asp:Label ID="lblTelefon" runat="server" Text="(0216) 494 7734"></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-phone"></i>
                                        <asp:Label ID="lblCepTelefonu" runat="server" Text="(0216) 494 7734"></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-mail-forward"></i>
                                        <asp:Label ID="lblmail" runat="server" Text="(0216) 494 7734"></asp:Label>
                                    </p>

                                    <p>
                                        <i class="icon-asterisk"></i>
                                        <asp:Label ID="lblVergiDairesi" runat="server" Text="(0216) 494 7734"></asp:Label>
                                    </p>
                                    <p>
                                        <i class="icon-mail-forward"></i>
                                        <asp:Label ID="lblVergiNo" runat="server" Text="(0216) 494 7734"></asp:Label>
                                    </p>

                                </div>
                            </div>
                        </div>
                        <!-- Fatura Bilgisi -->
                        <div class="col-md-6">
                            <div class="alert alert-success" style="background-color: #f9f9e5; border-color: #e0e0c7;">
                                <div class="message-box" style="padding: 10px; border-radius: 5px;">
                                    <p style="color: #777;">
                                        <asp:Label ID="lblNot" runat="server" Text="M012023000000040 no.lu e-fatura iptal edildi. (17.08.2023)"></asp:Label>
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
                        <asp:Label ID="lblAlacakBakiye" runat="server" Text="14.360,65 TL"></asp:Label></h4>
                    <p>Alacak Bakiyesi</p>
                </div>
            </section>
        </div>
        <div class="col-lg-3 col-sm-6">
            <section class="panel">
                <div class="symbol" style="background-color: #4ECDC4;">
                    <i class="icon-tags"></i>
                </div>
                <div class="value">
                    <h4>
                        <asp:Label ID="lblCekBakiye" runat="server" Text="20,00 TL"></asp:Label></h4>
                    <p>Çek Bakiyesi</p>
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
                        <asp:Label ID="lblSenetBakiye" runat="server" Text="50,00 TL"></asp:Label></h4>
                    <p>Senet Bakiyesi</p>
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
                        <asp:Label ID="lblCiroToplam" runat="server" Text="115.912,65 TL"></asp:Label></h4>
                    <p>Ciro</p>
                </div>
            </section>
        </div>
    </div>

    <!-- Hızlı İşlem Butonları -->
    <div class="row">
        <div class="col-lg-12">
            <div class="btn-group">
                <asp:HyperLink ID="hplinkAlisYap" runat="server" CssClass="btn btn-shadow btn-primary" Style="margin-right: 5px;">
                    <i class="icon-tag"></i> Aliş Yap
                </asp:HyperLink>
                <asp:HyperLink ID="btnTahsilatGir" runat="server" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                    <i class="icon-money"></i> Tahsilat Gir
                </asp:HyperLink>
                <asp:HyperLink ID="hplinkSatisYap" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">
                    <i class="icon-save"></i> Satiş Yap
                </asp:HyperLink>
                <asp:HyperLink ID="btnHesapEkstresi" runat="server" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                    <i class="icon-list"></i> Hesap Ekstresi
                </asp:HyperLink> 
                <asp:HyperLink ID="hplinkTedarikciGuncelle" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">
                    <i class="icon-edit"></i> Tedarikci Güncelle
                </asp:HyperLink>
                <asp:LinkButton ID="btnDigerIslemler" runat="server" CssClass="btn btn-shadow btn-default" Style="margin-right: 5px;">
                    <i class="icon-cog"></i> Ekstra İşlemler
                </asp:LinkButton>
        <%--        <div class="btn-group">
                    <button data-toggle="dropdown" class="btn btn-shadow btn-default dropdown-toggle" type="button"><i class="icon-file-text-alt"></i> Hesap Ekstresi <span class="caret"></span></button>
                    <ul role="menu" class="dropdown-menu">
                        <li><a href="#">Action</a></li>
                        <li><a href="#">Another action</a></li>
                        <li><a href="#">Something else here</a></li>
                        <li class="divider"></li>
                        <li><a href="#">Separated link</a></li>
                    </ul>
                </div>--%>
            </div>
        </div>
    </div>

    <!-- Önceki Satışlar ve Ödemeler Tabloları -->
    <div class="row" style="margin-top: 20px;">
        <!-- Önceki Satışlar Tablosu -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h4>ÖNCEKİ AliŞLAR <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>No</th>
                                    <th>Durum</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>20.10.2023</td>
                                    <td>M012023000000063</td>
                                    <td><span class="label label-success">Faturalandı</span></td>
                                    <td>8.585,00 TL</td>
                                </tr>
                                <tr>
                                    <td>12.10.2023</td>
                                    <td>M012023000000062</td>
                                    <td><span class="label label-success">Faturalandı</span></td>
                                    <td>2.575,50 TL</td>
                                </tr>
                                <tr>
                                    <td>06.10.2023</td>
                                    <td>M012023000000061</td>
                                    <td><span class="label label-success">Faturalandı</span></td>
                                    <td>17.599,25 TL</td>
                                </tr>
                                <tr>
                                    <td>02.08.2023</td>
                                    <td>M012023000000047</td>
                                    <td><span class="label label-success">Faturalandı</span></td>
                                    <td>60.600,00 TL</td>
                                </tr>
                                <tr>
                                    <td>07.06.2023</td>
                                    <td>M012023000000026</td>
                                    <td><span class="label label-success">Faturalandı</span></td>
                                    <td>26.552,90 TL</td>
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
                    <h4>ÖNCEKİ ÖDEMELERİ <i class="icon-angle-down pull-right"></i></h4>
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
                                    <td>29.09.2023</td>
                                    <td>25.000,00 TL</td>
                                    <td>Banka</td>
                                </tr>
                                <tr>
                                    <td>23.08.2023</td>
                                    <td>25.000,00 TL</td>
                                    <td>Banka</td>
                                </tr>
                                <tr>
                                    <td>23.08.2023</td>
                                    <td>25.000,00 TL</td>
                                    <td>Banka</td>
                                </tr>
                                <tr>
                                    <td>22.06.2023</td>
                                    <td>26.552,00 TL</td>
                                    <td>Nakit</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Alışlar / İadeler Tablosu -->
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h4>SATİŞLAR / İADELER <i class="icon-angle-down pull-right"></i></h4>
                </header>
                <div class="panel-body">
                    <div class="adv-table">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Tarih</th>
                                    <th>No</th>
                                    <th>Tür</th>
                                    <th>Durum</th>
                                    <th>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- Bu kısım boş bırakılabilir veya örnek veriler eklenebilir -->
                                <tr>
                                    <td colspan="5" class="text-center">Kayıt bulunamadı</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

