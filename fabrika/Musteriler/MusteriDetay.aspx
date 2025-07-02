<%@ Page Title="Müşteri Detay" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MusteriDetay.aspx.cs" Inherits="fabrika_Musteriler_MusteriDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <style>
        .table-satislar tbody tr:hover,
        .table-odemeler tbody tr:hover,
        .table-senetler tbody tr:hover {
            background-color: #f8f9fa !important;
        }
        
        .card {
            transition: all 0.3s ease;
        }
        
        .card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.15) !important;
        }
        
        @media (max-width: 768px) {
            .container-fluid {
                padding-left: 10px;
                padding-right: 10px;
            }
            
            .card-body {
                padding: 1rem;
            }
            
            .btn-sm {
                font-size: 0.8rem;
                padding: 0.4rem 0.8rem;
            }
            
            .table-responsive {
                font-size: 0.85rem;
            }
            
            .dropdown-menu {
                font-size: 0.9rem;
            }
            
            h4 {
                font-size: 1.1rem;
            }
            
            h5 {
                font-size: 1rem;
            }
            
            h6 {
                font-size: 0.9rem;
            }
        }
        
        @media (max-width: 576px) {
            .py-4 {
                padding-top: 1rem !important;
                padding-bottom: 1rem !important;
            }
            
            .mb-4 {
                margin-bottom: 1.5rem !important;
            }
            
            .table th,
            .table td {
                padding: 0.5rem;
                font-size: 0.8rem;
            }
            
            .badge {
                font-size: 0.7rem;
            }
        }
        
        .loading-spinner {
            display: inline-block;
            width: 1rem;
            height: 1rem;
            border: 2px solid #f3f3f3;
            border-top: 2px solid #007bff;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
    <div class="container-fluid py-4">
        <!-- Müşteri Bilgileri Kartı -->
        <div class="row mb-4">
            <div class="col-lg-6 col-md-12 mb-3">
                <div class="card shadow-sm border-0 h-100">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-auto">
                                <asp:Image ID="MusteriResim" runat="server" CssClass="rounded-circle border" Width="80px" Height="80px" style="object-fit: cover;" />
                            </div>
                            <div class="col">
                                <h4 class="mb-1"><asp:Label ID="lblMusteriAdi" runat="server" /></h4>
                                <div class="mb-1 text-muted small"><i class="bi bi-geo-alt me-1"></i><asp:Label ID="lblAdres" runat="server" /></div>
                                <div class="row g-2 mt-2">
                                    <div class="col-sm-6">
                                        <div class="small"><i class="bi bi-person me-1"></i><asp:Label ID="lblYetkili" runat="server" /></div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="small"><i class="bi bi-telephone me-1"></i><asp:Label ID="lblTelefon" runat="server" /></div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="small"><i class="bi bi-phone me-1"></i><asp:Label ID="lblCepTelefonu" runat="server" /></div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="small"><i class="bi bi-envelope me-1"></i><asp:Label ID="lblmail" runat="server" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-12 mb-3">
                <div class="card shadow-sm border-0 h-100">
                    <div class="card-body">
                        <div class="row h-100">
                            <div class="col-md-6">
                                <h6 class="card-title mb-3">Vergi Bilgileri</h6>
                                <div class="mb-2 small"><strong>Vergi Dairesi:</strong><br><asp:Label ID="lblVergiDairesi" runat="server" /></div>
                                <div class="mb-2 small"><strong>Vergi No:</strong><br><asp:Label ID="lblVergiNo" runat="server" /></div>
                            </div>
                            <div class="col-md-6 d-flex align-items-center justify-content-center">
                                <div class="text-center">
                                    <h6 class="text-muted mb-2">Mevcut Bakiye</h6>
                                    <div id="bakiyeBilgisi">
                                        <div class="loading-spinner me-2"></div>
                                        <h4 class="text-muted">Yüklüyor...</h4>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hızlı İşlem Butonları -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="d-grid gap-2 d-md-flex flex-md-wrap">
                    <asp:HyperLink ID="hplinkSatisYap" runat="server" CssClass="btn btn-primary btn-sm"><i class="bi bi-tag"></i> Satış Yap</asp:HyperLink>
                    <div class="btn-group" role="group">
                        <button type="button" class="btn btn-success btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="bi bi-cash"></i> Tahsilat/Ödeme
                        </button>
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="TahsilatNakit.aspx"><i class="bi bi-cash"></i> Nakit</a></li>
                            <li><a class="dropdown-item" href="TahsilatCek.aspx"><i class="bi bi-credit-card-2-front"></i> Çek</a></li>
                            <li><a class="dropdown-item" href="TahsilatSenetAl.aspx"><i class="bi bi-file-text"></i> Müşteriden Senet Al</a></li>
                            <li><a class="dropdown-item" href="TahsilatSenetVer.aspx"><i class="bi bi-file-earmark-text"></i> Müşteriye Senet Ver</a></li>
                            <li><hr class="dropdown-divider"></li>
                            <li><a class="dropdown-item" href="BakiyeDuzelt.aspx"><i class="bi bi-arrow-left-right"></i> Bakiye Düzelt</a></li>
                        </ul>
                    </div>
                    <asp:LinkButton ID="btnFaturaOlustur" runat="server" CssClass="btn btn-info btn-sm"><i class="bi bi-file-earmark-text"></i> Fatura</asp:LinkButton>
                    <asp:LinkButton ID="btnHesapEkstresi" runat="server" CssClass="btn btn-warning btn-sm"><i class="bi bi-list"></i> Ekstre</asp:LinkButton>
                    <asp:HyperLink ID="hplinkMusteriGuncelle" runat="server" CssClass="btn btn-outline-info btn-sm"><i class="bi bi-pencil"></i> Güncelle</asp:HyperLink>
                    <asp:LinkButton ID="btnDigerIslemler" runat="server" CssClass="btn btn-outline-secondary btn-sm"><i class="bi bi-gear"></i> Diğer</asp:LinkButton>
                </div>
            </div>
        </div>

        <!-- Önceki Satışlar ve Ödemeler -->
        <div class="row mb-4">
            <div class="col-lg-6 mb-3">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">Önceki Satışlar</h5>
                        <asp:HyperLink ID="hplinkTumSatislar" runat="server" CssClass="btn btn-sm btn-outline-primary">Tümünü Gör</asp:HyperLink>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0 table-satislar">
                                <thead class="table-light">
                                    <tr>
                                        <th>Tarih</th>
                                        <th>No</th>
                                        <th>Durum</th>
                                        <th>Tutar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Satış verileri backend'den dinamik olarak yüklenecek -->
                                    <tr>
                                        <td colspan="4" class="text-center text-muted">
                                            <div class="loading-spinner me-2"></div>
                                            Satışlar yüklüyor...
                                        </td>
                                    </tr>
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
                            <table class="table table-hover align-middle mb-0 table-odemeler">
                                <thead class="table-light">
                                    <tr>
                                        <th>Tarih</th>
                                        <th>Tutar</th>
                                        <th>Şekli</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Ödeme verileri backend'den dinamik olarak yüklenecek -->
                                    <tr>
                                        <td colspan="3" class="text-center text-muted">
                                            <div class="loading-spinner me-2"></div>
                                            Ödemeler yüklüyor...
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Senetler Tablosu -->
        <div class="row">
            <div class="col-12">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light">
                        <h5 class="mb-0">Senetler</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0 table-senetler">
                                <thead class="table-light">
                                    <tr>
                                        <th>Vade Tarihi</th>
                                        <th>Seri No</th>
                                        <th>Tipi</th>
                                        <th>Durum</th>
                                        <th>Tutar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Senet verileri backend'den dinamik olarak yüklenecek -->
                                    <tr>
                                        <td colspan="5" class="text-center text-muted">
                                            <div class="loading-spinner me-2"></div>
                                            Senetler yüklüyor...
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function() {
            // URL'den MusteriID parametresini al
            const urlParams = new URLSearchParams(window.location.search);
            const musteriID = urlParams.get('id');
            
            if (musteriID) {
                // Dropdown menüdeki tüm linkleri güncelle
                const dropdownLinks = document.querySelectorAll('.dropdown-menu .dropdown-item');
                dropdownLinks.forEach(function(link) {
                    const href = link.getAttribute('href');
                    if (href && !href.includes('?')) {
                        link.setAttribute('href', href + '?musteriID=' + musteriID);
                    }
                });
            }
        });
    </script>
</asp:Content>

