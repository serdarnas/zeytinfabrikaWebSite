<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .dashboard-card {
            border-radius: 18px;
            box-shadow: 0 2px 16px rgba(0,0,0,0.07);
            border: none;
            margin-bottom: 24px;
        }
        .dashboard-card .card-title {
            font-size: 1.1rem;
            color: #6c757d;
            font-weight: 500;
        }
        .dashboard-card .card-value {
            font-size: 2.2rem;
            font-weight: bold;
        }
        .dashboard-badge {
            font-size: 0.9rem;
            border-radius: 12px;
            padding: 4px 12px;
        }
        .progress-circular {
            width: 90px;
            height: 90px;
            border-radius: 50%;
            background: conic-gradient(#4e73df 0% 85%, #e9ecef 85% 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 10px auto;
        }
        .progress-circular-content {
            text-align: center;
        }
        .progress-circular .progress-value {
            font-size: 1.5rem;
            font-weight: bold;
            color: #4e73df;
        }
        .stat-indicator {
            display: inline-block;
            width: 10px;
            height: 10px;
            border-radius: 50%;
            margin-right: 6px;
        }
        .stat-indicator.bg-primary { background: #4e73df; }
        .stat-indicator.bg-success { background: #1cc88a; }
        .stat-indicator.bg-warning { background: #f6c23e; }
        .stat-value { font-size: 0.95rem; font-weight: 500; }
        .placeholder-chart { height: 200px; background: #f8f9fa; border-radius: 12px; }
        .table thead th { background: #f8f9fa; }
    </style>
    <div class="container-fluid px-4">
        <!-- Üst Kartlar -->
        <div class="row mb-4">
            <div class="col-lg-3 col-md-6">
                <div class="card dashboard-card">
                    <div class="card-body d-flex justify-content-between align-items-center">
                        <div>
                            <div class="card-title">Zeytinyağı Firehane</div>
                            <div class="card-value">2,450</div>
                            <span class="text-success">+25% <i class="fa fa-arrow-up"></i></span>
                        </div>
                        <span class="dashboard-badge bg-success text-white">GÜN</span>
                    </div>
                    <div class="card-footer bg-white border-0">
                        <a href="#" class="text-primary">Detayları Görüntüle <i class="fa fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card dashboard-card">
                    <div class="card-body d-flex justify-content-between align-items-center">
                        <div>
                            <div class="card-title">Zeytin Fabrikası</div>
                            <div class="card-value">5,280</div>
                            <span class="text-success">+6% <i class="fa fa-arrow-up"></i></span>
                        </div>
                        <span class="dashboard-badge bg-primary text-white">GÜN</span>
                    </div>
                    <div class="card-footer bg-white border-0">
                        <a href="#" class="text-primary">Detayları Görüntüle <i class="fa fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card dashboard-card">
                    <div class="card-body d-flex justify-content-between align-items-center">
                        <div>
                            <div class="card-title">Satış Departmanı</div>
                            <div class="card-value">₺85,420</div>
                            <span class="text-success">+15% <i class="fa fa-arrow-up"></i></span>
                        </div>
                        <span class="dashboard-badge bg-info text-white">GÜN</span>
                    </div>
                    <div class="card-footer bg-white border-0">
                        <a href="#" class="text-primary">Detayları Görüntüle <i class="fa fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card dashboard-card">
                    <div class="card-body d-flex justify-content-between align-items-center">
                        <div>
                            <div class="card-title">Üretim Departmanı</div>
                            <div class="card-value">92%</div>
                            <span class="text-danger">-3% <i class="fa fa-arrow-down"></i></span>
                        </div>
                        <span class="dashboard-badge bg-warning text-white">AY</span>
                    </div>
                    <div class="card-footer bg-white border-0">
                        <a href="#" class="text-primary">Detayları Görüntüle <i class="fa fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Sekmeli Panel -->
        <ul class="nav nav-tabs mb-4" id="dashboardTabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="genel-tab" data-toggle="tab" href="#genel" role="tab">Genel Bakış</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="uretim-tab" data-toggle="tab" href="#uretim" role="tab">Üretim</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="envanter-tab" data-toggle="tab" href="#envanter" role="tab">Envanter</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="personel-tab" data-toggle="tab" href="#personel" role="tab">Personel</a>
            </li>
        </ul>
        <div class="tab-content" id="dashboardTabsContent">
            <!-- Genel Bakış Sekmesi -->
            <div class="tab-pane fade show active" id="genel" role="tabpanel">
                <div class="row">
                    <!-- Satış Performansı -->
                    <div class="col-lg-4 mb-4">
                        <div class="card dashboard-card h-100">
                            <div class="card-body text-center">
                                <h5 class="card-title">Satış Performansı</h5>
                                <div class="progress-circular mb-2">
                                    <div class="progress-circular-content">
                                        <span class="progress-value">85%</span>
                                        <div class="progress-text" style="font-size:0.95rem; color:#888;">Toplam</div>
                                    </div>
                                </div>
                                <div class="mt-3">
                                    <small class="text-muted">Satış Hedefi</small>
                                    <div class="font-weight-bold">5.480.432 / 6.500.000</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Ürün Satışları -->
                    <div class="col-lg-4 mb-4">
                        <div class="card dashboard-card h-100">
                            <div class="card-body">
                                <h5 class="card-title">Ürün Satışları</h5>
                                <div class="mb-3">
                                    <div class="d-flex justify-content-between mb-1">
                                        <span>Erken Hasat Zeytinyağı</span>
                                        <span class="badge bg-primary">1.250.000 ₺</span>
                                    </div>
                                    <div class="progress">
                                        <div class="progress-bar bg-primary" style="width: 65%"></div>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <div class="d-flex justify-content-between mb-1">
                                        <span>Naturel Sızma (0.5 L)</span>
                                        <span class="badge bg-info">825.000 ₺</span>
                                    </div>
                                    <div class="progress">
                                        <div class="progress-bar bg-info" style="width: 55%"></div>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <div class="d-flex justify-content-between mb-1">
                                        <span>Natürel Ham (5kg)</span>
                                        <span class="badge bg-warning">565.000 ₺</span>
                                    </div>
                                    <div class="progress">
                                        <div class="progress-bar bg-warning" style="width: 38%"></div>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <div class="d-flex justify-content-between mb-1">
                                        <span>Ham Zeytin (5kg)</span>
                                        <span class="badge bg-success">375.000 ₺</span>
                                    </div>
                                    <div class="progress">
                                        <div class="progress-bar bg-success" style="width: 25%"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Müşteri Dağılımı -->
                    <div class="col-lg-4 mb-4">
                        <div class="card dashboard-card h-100">
                            <div class="card-body text-center">
                                <h5 class="card-title">Müşteri Dağılımı</h5>
                                <div class="progress-circular mb-2" style="background:conic-gradient(#36b9cc 0% 100%, #e9ecef 0% 0%);">
                                    <div class="progress-circular-content">
                                        <span class="progress-value">124</span>
                                        <div class="progress-text" style="font-size:0.95rem; color:#888;">Aktif Müşteriler</div>
                                    </div>
                                </div>
                                <div class="customer-stats mt-3">
                                    <div class="row">
                                        <div class="col-4">
                                            <div class="stat-item">
                                                <span class="stat-indicator bg-primary"></span> Perakendeci
                                                <div class="stat-value">65 / %52</div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="stat-item">
                                                <span class="stat-indicator bg-success"></span> Toptancı
                                                <div class="stat-value">42 / %34</div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="stat-item">
                                                <span class="stat-indicator bg-warning"></span> İhracat
                                                <div class="stat-value">17 / %14</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Grafik ve İşlemler Satırı -->
                <div class="row">
                    <!-- Aylık Satışlar -->
                    <div class="col-lg-8 mb-4">
                        <div class="card dashboard-card h-100">
                            <div class="card-body">
                                <h5 class="card-title">Aylık Satışlar</h5>
                                <div class="placeholder-chart"></div>
                            </div>
                        </div>
                    </div>
                    <!-- Son Siparişler -->
                    <div class="col-lg-4 mb-4">
                        <div class="card dashboard-card h-100">
                            <div class="card-body">
                                <h5 class="card-title">Son Siparişler</h5>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>Mehmet Tirmak Market</strong>
                                            <div class="text-muted small">100 Şişe Naturel Sızma (1L)</div>
                                        </div>
                                        <span class="badge bg-success">Tamamlandı</span>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>Anadolu Gurme Ltd.</strong>
                                            <div class="text-muted small">200 kg Özel Zeytinyağı (5L)</div>
                                        </div>
                                        <span class="badge bg-warning">Hazırlanıyor</span>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>Anadolu Restoran</strong>
                                            <div class="text-muted small">150 Şişe Zeytinyağı (2L)</div>
                                        </div>
                                        <span class="badge bg-primary">Sipariş</span>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>Ege Marketleri A.Ş.</strong>
                                            <div class="text-muted small">300 kg Naturel Sızma</div>
                                        </div>
                                        <span class="badge bg-danger">Geri Bildirim</span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Diğer Sekmeler -->
            <div class="tab-pane fade" id="uretim" role="tabpanel">
                <div class="alert alert-info">Üretim Departmanı içeriği hazırlanıyor...</div>
            </div>
            <div class="tab-pane fade" id="envanter" role="tabpanel">
                <div class="alert alert-info">Envanter içeriği hazırlanıyor...</div>
            </div>
            <div class="tab-pane fade" id="personel" role="tabpanel">
                <div class="alert alert-info">Personel içeriği hazırlanıyor...</div>
            </div>
        </div>
        <!-- Son Aktiviteler -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card dashboard-card">
                    <div class="card-header bg-light">
                        <h5 class="mb-0">Son Aktiviteler</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Tarih</th>
                                        <th>Departman</th>
                                        <th>İşlem</th>
                                        <th>Kullanıcı</th>
                                        <th>Durum</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>10.05.2025 16:20</td>
                                        <td>Muhasebe</td>
                                        <td>Tahsilat Yönetimi</td>
                                        <td>Ahmet Yılmaz</td>
                                        <td><span class="badge bg-success">Tamamlandı</span></td>
                                    </tr>
                                    <tr>
                                        <td>10.05.2025 13:15</td>
                                        <td>Satış</td>
                                        <td>Yeni Sipariş Girişi</td>
                                        <td>Zeynep Kaya</td>
                                        <td><span class="badge bg-warning">İşleniyor</span></td>
                                    </tr>
                                    <tr>
                                        <td>10.05.2025 10:45</td>
                                        <td>Üretim</td>
                                        <td>Hammadde girişi</td>
                                        <td>Mehmet Demir</td>
                                        <td><span class="badge bg-success">Tamamlandı</span></td>
                                    </tr>
                                    <tr>
                                        <td>10.05.2025 09:20</td>
                                        <td>Satış</td>
                                        <td>Fatura düzenleme</td>
                                        <td>Zeynep Kaya</td>
                                        <td><span class="badge bg-success">Tamamlandı</span></td>
                                    </tr>
                                    <tr>
                                        <td>09.05.2025 16:00</td>
                                        <td>Üretim</td>
                                        <td>Üretim Raporu</td>
                                        <td>Ali Yıldız</td>
                                        <td><span class="badge bg-success">Tamamlandı</span></td>
                                    </tr>
                                    <tr>
                                        <td>09.05.2025 14:00</td>
                                        <td>Satış</td>
                                        <td>İhracat Dosyası</td>
                                        <td>Zeynep Kaya</td>
                                        <td><span class="badge bg-danger">Reddedildi</span></td>
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

