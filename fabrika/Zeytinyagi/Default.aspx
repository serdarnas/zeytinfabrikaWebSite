<%@ Page Title="Zeytinyağı İşletme Paneli" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Zeytinyagi_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .card {
            border-radius: 4px;
            margin-bottom: 20px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
        }
        .card-header {
            font-weight: bold;
            padding: 10px 15px;
            background-color: #f8f9fa;
            border-bottom: 1px solid #ddd;
        }
        .card-body {
            padding: 15px;
        }
        .stat-card {
            border-left: 4px solid #ddd;
            padding: 15px;
            margin-bottom: 20px;
            background-color: #fff;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
        }
        .stat-card.success {
            border-left-color: #28a745;
        }
        .stat-card.primary {
            border-left-color: #007bff;
        }
        .stat-card.warning {
            border-left-color: #ffc107;
        }
        .stat-card.danger {
            border-left-color: #dc3545;
        }
        .stat-title {
            color: #6c757d;
            font-size: 14px;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 5px;
        }
        .stat-value {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .stat-icon {
            float: right;
            font-size: 38px;
            opacity: 0.2;
            margin-top: -30px;
        }
        .activity-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .activity-list li {
            padding: 10px 15px;
            border-bottom: 1px solid #f1f1f1;
        }
        .activity-list li:last-child {
            border-bottom: none;
        }
        .activity-list .time {
            color: #999;
            font-size: 12px;
            float: right;
        }
        .progress {
            height: 8px;
            margin-bottom: 10px;
            background-color: #f5f5f5;
            border-radius: 4px;
        }
        .page-title {
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        .table-responsive {
            margin-top: 15px;
        }
        .table th {
            background-color: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="page-title">
        <i class="fa fa-tint"></i> Zeytinyağı İşletme Paneli
    </h2>

    <!-- Özet Kartları -->
    <div class="row">
        <!-- Bugünkü Zeytin Girişi -->
        <div class="col-md-6 col-lg-3">
            <div class="stat-card success">
                <div class="stat-title">Bugünkü Zeytin Girişi</div>
                <div class="stat-value">12.5 Ton</div>
                <div class="stat-description">
                    <small><i class="fa fa-arrow-up text-success"></i> Düne göre %5 artış</small>
                </div>
                <div class="stat-icon">
                    <i class="fa fa-shopping-basket"></i>
                </div>
            </div>
        </div>

        <!-- Aktif Üretim Partisi -->
        <div class="col-md-6 col-lg-3">
            <div class="stat-card primary">
                <div class="stat-title">Aktif Üretim Partisi</div>
                <div class="stat-value">#P087</div>
                <div class="progress">
                    <div class="progress-bar bg-primary" role="progressbar" style="width: 75%;" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
                <div class="stat-description">
                    <small>Tamamlanma: %75</small>
                </div>
                <div class="stat-icon">
                    <i class="fa fa-refresh"></i>
                </div>
            </div>
        </div>

        <!-- Toplam Yağ Stoğu -->
        <div class="col-md-6 col-lg-3">
            <div class="stat-card warning">
                <div class="stat-title">Toplam Yağ Stoğu</div>
                <div class="stat-value">8.250 Lt</div>
                <div class="stat-description">
                    <small>Tank Doluluk: %65</small>
                </div>
                <div class="stat-icon">
                    <i class="fa fa-database"></i>
                </div>
            </div>
        </div>

        <!-- Kritik Kalite Uyarısı -->
        <div class="col-md-6 col-lg-3">
            <div class="stat-card danger">
                <div class="stat-title">Kritik Kalite Uyarısı</div>
                <div class="stat-value">2</div>
                <div class="stat-description">
                    <a href="#" class="text-danger"><small>Detayları Gör</small></a>
                </div>
                <div class="stat-icon">
                    <i class="fa fa-exclamation-triangle"></i>
                </div>
            </div>
        </div>
    </div>

    <!-- Ana İçerik Bölümü -->
    <div class="row">
        <!-- Sol Taraf - Tank Tablosu -->
        <div class="col-lg-8">
            <div class="card">
                <div class="card-header">
                    <i class="fa fa-table"></i> Aktif Tanklar ve Doluluk Oranları
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>Tank ID</th>
                                    <th>Yağ Tipi</th>
                                    <th>Kapasite</th>
                                    <th>Miktar</th>
                                    <th>Doluluk</th>
                                    <th>Durum</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>TNK-01</td>
                                    <td>Natürel Sızma - Erken Hasat</td>
                                    <td>5000 Lt</td>
                                    <td>4150 Lt</td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar bg-success" role="progressbar" style="width: 83%;" aria-valuenow="83" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                        <small>83%</small>
                                    </td>
                                    <td><span class="badge bg-success">Aktif</span></td>
                                </tr>
                                <tr>
                                    <td>TNK-02</td>
                                    <td>Natürel Sızma - Olgun Hasat</td>
                                    <td>5000 Lt</td>
                                    <td>3100 Lt</td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar bg-warning" role="progressbar" style="width: 62%;" aria-valuenow="62" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                        <small>62%</small>
                                    </td>
                                    <td><span class="badge bg-success">Aktif</span></td>
                                </tr>
                                <tr>
                                    <td>TNK-03</td>
                                    <td>Natürel Birinci</td>
                                    <td>10000 Lt</td>
                                    <td>1500 Lt</td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar bg-danger" role="progressbar" style="width: 15%;" aria-valuenow="15" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                        <small>15%</small>
                                    </td>
                                    <td><span class="badge bg-warning">Düşük Seviye</span></td>
                                </tr>
                                <tr>
                                    <td>TNK-04</td>
                                    <td>Natürel Sızma - Özel Rezerv</td>
                                    <td>2500 Lt</td>
                                    <td>2450 Lt</td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar bg-primary" role="progressbar" style="width: 98%;" aria-valuenow="98" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                        <small>98%</small>
                                    </td>
                                    <td><span class="badge bg-info">Neredeyse Dolu</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Sağ Taraf - Son Aktiviteler -->
        <div class="col-lg-4">
            <div class="card">
                <div class="card-header">
                    <i class="fa fa-tasks"></i> Son Aktiviteler
                </div>
                <div class="card-body" style="padding: 0;">
                    <ul class="activity-list">
                        <li>
                            <i class="fa fa-check-circle text-success mr-2"></i> 
                            Parti #P086 tamamlandı
                            <span class="time">1 saat önce</span>
                        </li>
                        <li>
                            <i class="fa fa-cube text-info mr-2"></i> 
                            Yeni ambalaj stoğu geldi
                            <span class="time">3 saat önce</span>
                        </li>
                        <li>
                            <i class="fa fa-filter text-warning mr-2"></i> 
                            Filtreleme makinesi bakıma alındı
                            <span class="time">Dün</span>
                        </li>
                        <li>
                            <i class="fa fa-truck text-primary mr-2"></i> 
                            Sipariş #S1052 sevk edildi
                            <span class="time">Dün</span>
                        </li>
                        <li>
                            <i class="fa fa-file-text text-danger mr-2"></i> 
                            Kalite test sonucu: Asidite sınırı aştı (Numune #N45)
                            <span class="time">2 gün önce</span>
                        </li>
                    </ul>
                </div>
                <div class="card-footer text-center">
                    <a href="#" class="text-muted"><small>Tüm Aktiviteleri Gör</small></a>
                </div>
            </div>

            <!-- Haftalık Üretim Özeti -->
            <div class="card">
                <div class="card-header">
                    <i class="fa fa-bar-chart"></i> Haftalık Üretim Özeti
                </div>
                <div class="card-body">
                    <table class="table table-sm">
                        <tbody>
                            <tr>
                                <td>Natürel Sızma</td>
                                <td>2.350 Lt</td>
                            </tr>
                            <tr>
                                <td>Natürel Birinci</td>
                                <td>1.150 Lt</td>
                            </tr>
                            <tr>
                                <td>Riviera</td>
                                <td>750 Lt</td>
                            </tr>
                            <tr>
                                <td><strong>Toplam</strong></td>
                                <td><strong>4.250 Lt</strong></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

