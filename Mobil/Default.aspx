<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Mobil/MobilMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Mobil_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .stats-row {
            margin-bottom: 25px;
        }
        
        .production-list {
            background: white;
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        }
        
        .list-header {
            background: linear-gradient(135deg, #4CAF50, #45a049);
            color: white;
            padding: 15px 20px;
            font-weight: 600;
        }
        
        .production-item {
            border-bottom: 1px solid #f0f0f0;
            padding: 15px 20px;
            transition: background-color 0.3s ease;
        }
        
        .production-item:last-child {
            border-bottom: none;
        }
        
        .production-item:hover {
            background-color: #f8f9fa;
        }
        
        .item-header {
            display: flex;
            justify-content: between;
            align-items: center;
            margin-bottom: 8px;
        }
        
        .parti-no {
            font-weight: 600;
            color: #333;
            font-size: 16px;
        }
        
        .status-badge {
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 11px;
            font-weight: 600;
        }
        
        .status-active { background: #e8f5e8; color: #2e7d32; }
        .status-completed { background: #e3f2fd; color: #1976d2; }
        .status-pending { background: #fff3e0; color: #f57c00; }
        
        .item-details {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 8px;
            font-size: 14px;
            color: #666;
        }
        
        .detail-item {
            display: flex;
            align-items: center;
        }
        
        .detail-item i {
            width: 16px;
            margin-right: 6px;
            color: #4CAF50;
        }
        
        .refresh-btn {
            position: fixed;
            bottom: 90px;
            right: 20px;
            width: 50px;
            height: 50px;
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 50%;
            box-shadow: 0 4px 15px rgba(76, 175, 80, 0.3);
            font-size: 18px;
            z-index: 999;
        }
        
        .empty-state {
            text-align: center;
            padding: 40px 20px;
            color: #666;
        }
        
        .empty-state i {
            font-size: 48px;
            color: #ddd;
            margin-bottom: 15px;
        }
        
        .search-container {
            padding: 15px 20px;
            background: #f8f9fa;
            border-bottom: 1px solid #e0e0e0;
        }
        
        .search-input {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #ddd;
            border-radius: 25px;
            font-size: 14px;
            outline: none;
        }
        
        .search-input:focus {
            border-color: #4CAF50;
            box-shadow: 0 0 5px rgba(76, 175, 80, 0.3);
        }
        
        .btn-update {
            background: #FF9800;
            color: white;
            border: none;
            border-radius: 6px;
            padding: 6px 12px;
            font-size: 12px;
            text-decoration: none;
            display: inline-block;
            margin-top: 8px;
            transition: all 0.3s ease;
        }
        
        .btn-update:hover {
            background: #F57C00;
            color: white;
            text-decoration: none;
            transform: translateY(-1px);
        }
        
        .mustahsil-info {
            color: #4CAF50;
            font-weight: 600;
            margin-bottom: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <!-- İstatistik Kartları -->
        <div class="row stats-row">
            <div class="col-6">
                <div class="dashboard-card">
                    <div class="card-icon icon-primary">
                        <i class="fas fa-industry"></i>
                    </div>
                    <div class="card-title">Bugünkü Üretim</div>
                    <div class="card-value">
                        <asp:Label ID="lblBugunkuUretim" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="card-subtitle">Adet</div>
                </div>
            </div>
            <div class="col-6">
                <div class="dashboard-card">
                    <div class="card-icon icon-info">
                        <i class="fas fa-weight-hanging"></i>
                    </div>
                    <div class="card-title">Toplam Kg</div>
                    <div class="card-value">
                        <asp:Label ID="lblToplamKg" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="card-subtitle">Bugün</div>
                </div>
            </div>
        </div>
        
        <div class="row stats-row">
            <div class="col-6">
                <div class="dashboard-card">
                    <div class="card-icon icon-warning">
                        <i class="fas fa-clock"></i>
                    </div>
                    <div class="card-title">Aktif Üretim</div>
                    <div class="card-value">
                        <asp:Label ID="lblAktifUretim" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="card-subtitle">Devam Eden</div>
                </div>
            </div>
            <div class="col-6">
                <div class="dashboard-card">
                    <div class="card-icon icon-success">
                        <i class="fas fa-chart-line"></i>
                    </div>
                    <div class="card-title">Aylık Toplam</div>
                    <div class="card-value">
                        <asp:Label ID="lblAylikToplam" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="card-subtitle">Bu Ay</div>
                </div>
            </div>
        </div>

        <!-- Üretim Listesi -->
        <div class="production-list">
            <div class="list-header">
                <i class="fas fa-list me-2"></i>
                Son Üretimler
                <asp:Label ID="lblSonucSayisi" runat="server" Text="" 
                    style="float: right; font-size: 12px; opacity: 0.8;"></asp:Label>
            </div>
            
            <!-- Arama Kutusu -->
            <div class="search-container">
                <div style="display: flex; gap: 10px;">
                    <asp:TextBox ID="txtArama" runat="server" CssClass="search-input" 
                        placeholder="Parti no, müştahsil adı, telefon, email, TC kimlik, plaka ile arama yapın..." 
                        OnTextChanged="txtArama_TextChanged" AutoPostBack="true" style="flex: 1;"></asp:TextBox>
                    <asp:Button ID="btnAra" runat="server" Text="Ara" OnClick="btnAra_Click" 
                        CssClass="btn" style="background: #4CAF50; color: white; border: none; padding: 10px 20px; border-radius: 8px;" />
                    <asp:Button ID="btnTemizle" runat="server" Text="Temizle" OnClick="btnTemizle_Click" 
                        CssClass="btn" style="background: #f44336; color: white; border: none; padding: 10px 20px; border-radius: 8px;" />
                </div>
            </div>
            
            <asp:Repeater ID="rptUretimler" runat="server">
                <ItemTemplate>
                    <div class="production-item">
                        <div class="item-header">
                            <span class="parti-no">Parti: <%# Eval("PartiNo") %></span>
                            <span class="status-badge <%# GetStatusClass(Eval("UretimBitisZamani")) %>">
                                <%# GetStatusText(Eval("UretimBitisZamani")) %>
                            </span>
                        </div>
                        
                        <!-- Müştahsil Bilgisi -->
                        <div class="mustahsil-info">
                            <i class="fas fa-user"></i>
                            <%# Eval("MustahsilAd") ?? "Müştahsil Bulunamadı" %>
                            <%# !string.IsNullOrEmpty((Eval("MustahsilTelefon") ?? "").ToString()) ? " - " + Eval("MustahsilTelefon") : "" %>
                            <%# !string.IsNullOrEmpty((Eval("MustahsilTCKimlikNo") ?? "").ToString()) ? " - TC: " + Eval("MustahsilTCKimlikNo") : "" %>
                        </div>
                        
                        <div class="item-details">
                            <div class="detail-item">
                                <i class="fas fa-truck"></i>
                                <span>Plaka: <%# Eval("PlakaNo") %></span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-calendar"></i>
                                <span><%# Eval("GelisTarihi", "{0:dd.MM.yyyy}") %></span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-weight"></i>
                                <span>Gelen: <%# Eval("GelisKg", "{0:N0}") %> kg - <%# Eval("GelisUrunAdi") %></span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-flask"></i>
                                <span>Asidite: <%# Eval("Asidite", "{0:F2}") %>%</span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-box"></i>
                                <span>Çıkan: <%# Eval("CikanUrunKg", "{0:N0}") %> kg - <%# Eval("CikanUrunAdi") %></span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-money-bill-wave"></i>
                                <span>Toplam Ücret: <%# Eval("TahsiliyeToplamUcreti", "{0:N2}") %> ₺</span>
                            </div>
                            <div class="detail-item">
                                <i class="fas fa-user-check"></i>
                                <span>Teslim Alan: <%# Eval("TesmilalanKullaniciAdi") %></span>
                            </div>
                        </div>
                        
                        <!-- Güncelleme Butonu -->
                        <a href="MalKabul.aspx?uretimID=<%# Eval("ZeytinyagiUretimID") %>" class="btn-update">
                            <i class="fas fa-edit"></i> Güncelle
                        </a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
                <asp:Literal ID="litEmptyMessage" runat="server" 
                    Text="<i class='fas fa-clipboard-list'></i><h5>Henüz üretim kaydı yok</h5><p>İlk üretim kaydınız oluşturulduğunda burada görünecek.</p>"></asp:Literal>
            </asp:Panel>
        </div>
    </div>

    <!-- Yenile Butonu -->
    <button type="button" class="refresh-btn" onclick="window.location.reload();">
        <i class="fas fa-sync-alt"></i>
    </button>
    
   
</asp:Content>

