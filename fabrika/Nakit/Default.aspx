<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Nakit_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
    <style>
        .stats-card {
            background: #fff;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            border-left: 4px solid #007bff;
        }
        .stats-card.nakit {
            border-left-color: #28a745;
        }
        .stats-card.cek {
            border-left-color: #17a2b8;
        }
        .stats-card.senet {
            border-left-color: #ffc107;
        }
        .stats-number {
            font-size: 2em;
            font-weight: bold;
            margin: 10px 0;
        }
        .stats-title {
            color: #6c757d;
            font-size: 0.9em;
            text-transform: uppercase;
        }
        .quick-action-card {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            text-align: center;
            transition: transform 0.2s;
        }
        .quick-action-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .alert-card {
            border-radius: 8px;
            margin-bottom: 15px;
        }
        .chart-container {
            background: #fff;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-dashboard"></i> Finansal Yönetim Dashboard</h3>
                    <div class="pull-right">
                        <small class="text-muted">Son Güncelleme: <asp:Label ID="lblSonGuncelleme" runat="server"></asp:Label></small>
                    </div>
                </header>
                <div class="panel-body">
                    
                    <!-- Genel İstatistikler -->
                    <div class="row">
                        <div class="col-md-3">
                            <div class="stats-card nakit">
                                <div class="stats-title">Toplam Kasa Bakiyesi</div>
                                <div class="stats-number text-success">
                                    <asp:Label ID="lblToplamKasaBakiye" runat="server" Text="0,00"></asp:Label> TL
                                </div>
                                <small><asp:Label ID="lblKasaSayisi" runat="server" Text="0"></asp:Label> Aktif Kasa</small>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="stats-card cek">
                                <div class="stats-title">Portföyde Çek</div>
                                <div class="stats-number text-info">
                                    <asp:Label ID="lblPortfoydeCek" runat="server" Text="0,00"></asp:Label> TL
                                </div>
                                <small><asp:Label ID="lblCekAdet" runat="server" Text="0"></asp:Label> Adet Çek</small>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="stats-card senet">
                                <div class="stats-title">Portföyde Senet</div>
                                <div class="stats-number text-warning">
                                    <asp:Label ID="lblPortfoydeSenet" runat="server" Text="0,00"></asp:Label> TL
                                </div>
                                <small><asp:Label ID="lblSenetAdet" runat="server" Text="0"></asp:Label> Adet Senet</small>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="stats-card">
                                <div class="stats-title">Toplam Finansal Değer</div>
                                <div class="stats-number text-primary">
                                    <asp:Label ID="lblToplamDeger" runat="server" Text="0,00"></asp:Label> TL
                                </div>
                                <small>Nakit + Çek + Senet</small>
                            </div>
                        </div>
                    </div>

                    <!-- Hızlı Erişim Butonları -->
                    <div class="row">
                        <div class="col-md-12">
                            <h4><i class="fa fa-bolt"></i> Hızlı İşlemler</h4>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <a href="Kasalar.aspx" class="btn btn-success btn-lg" style="width: 100%;">
                                    <i class="fa fa-university fa-2x"></i><br/>
                                    Kasa Yönetimi
                                </a>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <a href="NakitIslemler.aspx" class="btn btn-primary btn-lg" style="width: 100%;">
                                    <i class="fa fa-money fa-2x"></i><br/>
                                    Nakit İşlemler
                                </a>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <a href="Cekler.aspx" class="btn btn-info btn-lg" style="width: 100%;">
                                    <i class="fa fa-file-text fa-2x"></i><br/>
                                    Çek Yönetimi
                                </a>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <a href="Senetler.aspx" class="btn btn-warning btn-lg" style="width: 100%;">
                                    <i class="fa fa-file-o fa-2x"></i><br/>
                                    Senet Yönetimi
                                </a>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <asp:LinkButton ID="btnBugunkuIslemler" runat="server" CssClass="btn btn-secondary btn-lg" style="width: 100%;" OnClick="btnBugunkuIslemler_Click">
                                    <i class="fa fa-calendar fa-2x"></i><br/>
                                    Bugünkü İşlemler
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <asp:LinkButton ID="btnRaporlar" runat="server" CssClass="btn btn-dark btn-lg" style="width: 100%;" OnClick="btnRaporlar_Click">
                                    <i class="fa fa-bar-chart fa-2x"></i><br/>
                                    Raporlar
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="quick-action-card">
                                <asp:LinkButton ID="btnYenile" runat="server" CssClass="btn btn-secondary btn-lg" style="width: 100%;" OnClick="btnYenile_Click">
                                    <i class="fa fa-refresh fa-2x"></i><br/>
                                    Yenile
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <!-- Uyarılar ve Önemli Bilgiler -->
                    <div class="row">
                        <div class="col-md-6">
                            <h4><i class="fa fa-exclamation-triangle text-warning"></i> Dikkat Edilmesi Gerekenler</h4>
                            <asp:Panel ID="pnlVadesiYaklasan" runat="server" CssClass="alert alert-warning alert-card" Visible="false">
                                <h5><i class="fa fa-clock-o"></i> Vadesi Yaklaşan Çek/Senetler</h5>
                                <asp:Literal ID="ltVadesiYaklasan" runat="server"></asp:Literal>
                            </asp:Panel>
                            
                            <asp:Panel ID="pnlVadesiGecen" runat="server" CssClass="alert alert-danger alert-card" Visible="false">
                                <h5><i class="fa fa-warning"></i> Vadesi Geçen Çek/Senetler</h5>
                                <asp:Literal ID="ltVadesiGecen" runat="server"></asp:Literal>
                            </asp:Panel>
                            
                            <asp:Panel ID="pnlDusukBakiye" runat="server" CssClass="alert alert-info alert-card" Visible="false">
                                <h5><i class="fa fa-info-circle"></i> Düşük Bakiyeli Kasalar</h5>
                                <asp:Literal ID="ltDusukBakiye" runat="server"></asp:Literal>
                            </asp:Panel>
                        </div>
                        
                        <div class="col-md-6">
                            <h4><i class="fa fa-list"></i> Son İşlemler</h4>
                            <div class="chart-container">
                                <asp:Repeater ID="rptSonIslemler" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Tarih</th>
                                                    <th>Tür</th>
                                                    <th>Açıklama</th>
                                                    <th>Tutar</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><small><%# String.Format("{0:dd.MM.yyyy}", Eval("Tarih")) %></small></td>
                                            <td><span class="label label-<%# GetIslemTipiClass(Eval("Tip").ToString()) %>"><%# Eval("Tip") %></span></td>
                                            <td><%# Eval("Aciklama") %></td>
                                            <td><strong><%# String.Format("{0:N2}", Eval("Tutar")) %> TL</strong></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                
                                <asp:Panel ID="pnlSonIslemYok" runat="server" Visible="false" CssClass="text-center text-muted">
                                    <i class="fa fa-info-circle fa-2x"></i>
                                    <p>Henüz işlem bulunmuyor</p>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <!-- Aylık Özet Grafik -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="chart-container">
                                <h4><i class="fa fa-line-chart"></i> Aylık Finansal Özet</h4>
                                <canvas id="aylikOzetGrafik" width="400" height="100"></canvas>
                            </div>
                        </div>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <script type="text/javascript">
        function showSuccessMessage(title, message) {
            Swal.fire({
                icon: 'success',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        function showErrorMessage(title, message) {
            Swal.fire({
                icon: 'error',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        function showInfoMessage(title, message) {
            Swal.fire({
                icon: 'info',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        // Grafik verilerini yükle
        window.onload = function() {
            var ctx = document.getElementById('aylikOzetGrafik').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran'],
                    datasets: [{
                        label: 'Nakit',
                        data: [12, 19, 3, 5, 2, 3],
                        borderColor: 'rgb(40, 167, 69)',
                        backgroundColor: 'rgba(40, 167, 69, 0.1)',
                        tension: 0.1
                    }, {
                        label: 'Çek',
                        data: [2, 3, 20, 5, 1, 4],
                        borderColor: 'rgb(23, 162, 184)',
                        backgroundColor: 'rgba(23, 162, 184, 0.1)',
                        tension: 0.1
                    }, {
                        label: 'Senet',
                        data: [7, 11, 5, 8, 3, 7],
                        borderColor: 'rgb(255, 193, 7)',
                        backgroundColor: 'rgba(255, 193, 7, 0.1)',
                        tension: 0.1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        };
    </script>
</asp:Content> 