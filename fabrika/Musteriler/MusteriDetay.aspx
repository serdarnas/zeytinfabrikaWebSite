<%@ Page Title="Müşteri Detay" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MusteriDetay.aspx.cs" Inherits="fabrika_Musteriler_MusteriDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <div class="container-fluid py-4">
        <!-- Müşteri Bilgileri Kartı -->
        <div class="row mb-4">
            <div class="col-lg-6">
                <div class="card shadow-sm border-0">
                    <div class="card-body d-flex align-items-center">
                        <div class="me-4">
                            <asp:Image ID="MusteriResim" runat="server" CssClass="rounded-circle border" Width="100px" />
                        </div>
                        <div>
                            <h3 class="mb-1"><asp:Label ID="lblMusteriAdi" runat="server" /></h3>
                            <div class="mb-2 text-muted"><i class="bi bi-geo-alt"></i> <asp:Label ID="lblAdres" runat="server" /></div>
                            <div class="mb-1"><i class="bi bi-person"></i> <asp:Label ID="lblYetkili" runat="server" /></div>
                            <div class="mb-1"><i class="bi bi-telephone"></i> <asp:Label ID="lblTelefon" runat="server" /></div>
                            <div class="mb-1"><i class="bi bi-phone"></i> <asp:Label ID="lblCepTelefonu" runat="server" /></div>
            <div class="mb-1"><i class="bi bi-building"></i> <asp:Label ID="lblVergiDairesi" runat="server"></asp:Label></div>
            <div class="mb-1"><i class="bi bi-upc"></i> <asp:Label ID="lblVergiNo" runat="server" /></div>
            <div class="mt-3">
                <asp:HyperLink ID="hplinkMusteriGuncelleTop" runat="server" CssClass="btn btn-primary btn-sm me-2">
                    <i class="bi bi-pencil"></i> Müşteriyi Güncelle
                </asp:HyperLink>
                <asp:HyperLink ID="hplinkSatisYapTop" runat="server" CssClass="btn btn-success btn-sm">
                    <i class="bi bi-cart-plus"></i> Yeni Satış
                </asp:HyperLink>
            </div>
                            <div class="mb-1"><i class="bi bi-envelope"></i> <asp:Label ID="lblmail" runat="server" /></div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Bakiye Kartları -->
            <div class="col-lg-6">
                <div class="row g-3">
                    <div class="col-6 col-md-3">
                        <div class="card text-center border-0 shadow-sm">
                            <div class="card-body">
                                <div class="mb-2 text-danger fs-3"><i class="bi bi-credit-card"></i></div>
                                <h5 class="card-title"><asp:Label ID="lblAlacakBakiye" runat="server" /></h5>
                                <p class="card-text small text-muted">Alacak Bakiyesi</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card text-center border-0 shadow-sm">
                            <div class="card-body">
                                <div class="mb-2 text-info fs-3"><i class="bi bi-tags"></i></div>
                                <h5 class="card-title"><asp:Label ID="lblCekBakiye" runat="server" /></h5>
                                <p class="card-text small text-muted">Çek Bakiyesi</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card text-center border-0 shadow-sm">
                            <div class="card-body">
                                <div class="mb-2 text-primary fs-3"><i class="bi bi-cash"></i></div>
                                <h5 class="card-title"><asp:Label ID="lblSenetBakiye" runat="server" /></h5>
                                <p class="card-text small text-muted">Senet Bakiyesi</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card text-center border-0 shadow-sm">
                            <div class="card-body">
                                <div class="mb-2 text-success fs-3"><i class="bi bi-bar-chart"></i></div>
                                <h5 class="card-title"><asp:Label ID="lblCiroToplam" runat="server" /></h5>
                                <p class="card-text small text-muted">Ciro</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hızlı İşlem Butonları -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="d-flex flex-wrap gap-2">
                    <asp:HyperLink ID="hplinkSatisYap" runat="server" CssClass="btn btn-shadow btn-primary"><i class="bi bi-tag"></i> Satış Yap</asp:HyperLink>
                    <asp:LinkButton ID="btnTahsilatGir" runat="server" CssClass="btn btn-shadow btn-success"><i class="bi bi-cash"></i> Tahsilat Gir</asp:LinkButton>
                    <asp:LinkButton ID="btnFaturaOlustur" runat="server" CssClass="btn btn-shadow btn-info"><i class="bi bi-file-earmark-text"></i> Fatura Oluştur</asp:LinkButton>
                    <asp:LinkButton ID="btnHesapEkstresi" runat="server" CssClass="btn btn-shadow btn-warning"><i class="bi bi-list"></i> Hesap Ekstresi</asp:LinkButton>
                    <asp:HyperLink ID="hplinkMusteriGuncelle" runat="server" CssClass="btn btn-shadow btn-outline-info"><i class="bi bi-pencil"></i> Müşteri Güncelle</asp:HyperLink>
                    <asp:LinkButton ID="btnDigerIslemler" runat="server" CssClass="btn btn-shadow btn-outline-secondary"><i class="bi bi-gear"></i> Ekstra İşlemler</asp:LinkButton>
                </div>
            </div>
        </div>

        <!-- Önceki Satışlar ve Ödemeler -->
        <div class="row mb-4">
            <div class="col-lg-6 mb-3">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light">
                        <h5 class="mb-0">Önceki Satışlar</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Tarih</th>
                                        <th>No</th>
                                        <th>Durum</th>
                                        <th>Tutar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Satış verileri burada dinamik olarak doldurulacak -->
                                    <tr>
                                        <td>20.10.2023</td>
                                        <td>M012023000000063</td>
                                        <td><span class="badge bg-success">Faturalandı</span></td>
                                        <td>8.585,00 TL</td>
                                    </tr>
                                    <!-- ... -->
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 mb-3">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light">
                        <h5 class="mb-0">Önceki Ödemeler</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Tarih</th>
                                        <th>Tutar</th>
                                        <th>Şekli</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Ödeme verileri burada dinamik olarak doldurulacak -->
                                    <tr>
                                        <td>29.09.2023</td>
                                        <td>25.000,00 TL</td>
                                        <td>Banka</td>
                                    </tr>
                                    <!-- ... -->
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Alışlar / İadeler Tablosu -->
        <div class="row">
            <div class="col-12">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light">
                        <h5 class="mb-0">Alışlar / İadeler</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Tarih</th>
                                        <th>No</th>
                                        <th>Tür</th>
                                        <th>Durum</th>
                                        <th>Tutar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="5" class="text-center text-muted">Kayıt bulunamadı</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
