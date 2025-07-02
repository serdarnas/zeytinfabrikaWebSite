<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="DepoRaporlari.aspx.cs" Inherits="fabrika_Depo_DepoRaporlari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .report-card {
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            padding: 20px;
            margin-bottom: 20px;
            transition: transform 0.2s;
        }
        .report-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.15);
        }
        .report-icon {
            font-size: 48px;
            margin-bottom: 15px;
        }
        .stat-box {
            text-align: center;
            padding: 15px;
            border-radius: 5px;
            margin: 5px;
        }
        .stat-value {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .stat-label {
            font-size: 12px;
            color: #666;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-6">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default" Text="← Geri Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-6 text-right">
                    <h4><strong>Depo Raporları</strong></h4>
                </div>
            </div>
        </div>
    </section>

    <!-- Özet İstatistikler -->
    <section class="panel">
        <header class="panel-heading">
            <h4><i class="fa fa-chart-bar"></i> Genel İstatistikler</h4>
        </header>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="stat-box bg-primary text-white">
                        <div class="stat-value">
                            <asp:Label ID="lblToplamDepo" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="stat-label">Toplam Depo</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-box bg-success text-white">
                        <div class="stat-value">
                            <asp:Label ID="lblToplamUrun" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="stat-label">Toplam Ürün Çeşidi</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-box bg-info text-white">
                        <div class="stat-value">
                            <asp:Label ID="lblToplamStokDegeri" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="stat-label">Toplam Stok Değeri (₺)</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-box bg-warning text-white">
                        <div class="stat-value">
                            <asp:Label ID="lblMinimumStokAlti" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="stat-label">Minimum Stok Altı</div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Rapor Filtreleri -->
    <section class="panel">
        <header class="panel-heading">
            <h4><i class="fa fa-filter"></i> Rapor Filtreleri</h4>
        </header>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Depo</label>
                        <asp:DropDownList ID="ddlDepo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDepo_SelectedIndexChanged">
                            <asp:ListItem Text="Tüm Depolar" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Rapor Türü</label>
                        <asp:DropDownList ID="ddlRaporTuru" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRaporTuru_SelectedIndexChanged">
                            <asp:ListItem Text="Stok Durumu" Value="stok" Selected="True" />
                            <asp:ListItem Text="Stok Hareketleri" Value="hareket" />
                            <asp:ListItem Text="Minimum Stok Raporu" Value="minimum" />
                            <asp:ListItem Text="Stok Değer Raporu" Value="deger" />
                            <asp:ListItem Text="Transfer Raporu" Value="transfer" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Başlangıç Tarihi</label>
                        <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Bitiş Tarihi</label>
                        <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnRaporOlustur" runat="server" CssClass="btn btn-primary btn-lg" 
                        Text="Rapor Oluştur" OnClick="btnRaporOlustur_Click" />
                    <asp:Button ID="btnExcelAktar" runat="server" CssClass="btn btn-success btn-lg" 
                        Text="Excel'e Aktar" OnClick="btnExcelAktar_Click" />
                    <asp:Button ID="btnPDFAktar" runat="server" CssClass="btn btn-danger btn-lg" 
                        Text="PDF'e Aktar" OnClick="btnPDFAktar_Click" />
                </div>
            </div>
        </div>
    </section>

    <!-- Rapor Sonuçları -->
    <section class="panel">
        <header class="panel-heading">
            <h4><i class="fa fa-table"></i> <asp:Label ID="lblRaporBaslik" runat="server" Text="Stok Durumu Raporu"></asp:Label></h4>
        </header>
        <div class="panel-body">
            <asp:Panel ID="pnlVeriYok" runat="server" Visible="false" CssClass="alert alert-info">
                <i class="fa fa-info-circle"></i> Seçilen kriterlere uygun veri bulunamadı.
            </asp:Panel>
            
            <!-- Stok Durumu Raporu -->
            <asp:Panel ID="pnlStokRaporu" runat="server" Visible="true">
                <div class="table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead class="thead-dark">
                            <tr>
                                <th>Depo</th>
                                <th>Ürün</th>
                                <th>Mevcut Stok</th>
                                <th>Birim</th>
                                <th>Minimum Stok</th>
                                <th>Birim Fiyat</th>
                                <th>Toplam Değer</th>
                                <th>Durum</th>
                                <th>Son Güncelleme</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptStokRaporu" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("DepoAdi") %></td>
                                        <td><%# Eval("UrunAdi") %></td>
                                        <td class="text-right"><%# Eval("Miktar", "{0:N2}") %></td>
                                        <td><%# Eval("BirimAdi") %></td>
                                        <td class="text-right"><%# Eval("MinimumMiktar", "{0:N2}") %></td>
                                        <td class="text-right"><%# Eval("BirimFiyat", "{0:C}") %></td>
                                        <td class="text-right"><%# Eval("ToplamDeger", "{0:C}") %></td>
                                        <td>
                                            <span class="badge <%# GetStokDurumBadgeClass(Eval("Miktar"), Eval("MinimumMiktar")) %>">
                                                <%# GetStokDurumText(Eval("Miktar"), Eval("MinimumMiktar")) %>
                                            </span>
                                        </td>
                                        <td><%# Eval("SonGuncellemeTarihi", "{0:dd.MM.yyyy}") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>

            <!-- Stok Hareketleri Raporu -->
            <asp:Panel ID="pnlHareketRaporu" runat="server" Visible="false">
                <div class="table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead class="thead-dark">
                            <tr>
                                <th>Tarih</th>
                                <th>Depo</th>
                                <th>Ürün</th>
                                <th>Hareket Tipi</th>
                                <th>Miktar</th>
                                <th>Birim</th>
                                <th>Açıklama</th>
                                <th>Kullanıcı</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptHareketRaporu" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("IslemTarihi", "{0:dd.MM.yyyy HH:mm}") %></td>
                                        <td><%# Eval("DepoAdi") %></td>
                                        <td><%# Eval("UrunAdi") %></td>
                                        <td>
                                            <span class="badge <%# GetHareketTipiBadgeClass(Eval("HareketTipi").ToString()) %>">
                                                <%# GetHareketTipiText(Eval("HareketTipi").ToString()) %>
                                            </span>
                                        </td>
                                        <td class="text-right <%# Convert.ToDecimal(Eval("Miktar")) >= 0 ? "text-success" : "text-danger" %>">
                                            <%# Eval("Miktar", "{0:N2}") %>
                                        </td>
                                        <td><%# Eval("BirimAdi") %></td>
                                        <td><%# Eval("Aciklama") %></td>
                                        <td><%# Eval("KullaniciAdi") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>

            <!-- Minimum Stok Raporu -->
            <asp:Panel ID="pnlMinimumStokRaporu" runat="server" Visible="false">
                <div class="alert alert-warning">
                    <i class="fa fa-exclamation-triangle"></i> Minimum stok seviyesinin altındaki ürünler
                </div>
                <div class="table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead class="thead-dark">
                            <tr>
                                <th>Depo</th>
                                <th>Ürün</th>
                                <th>Mevcut Stok</th>
                                <th>Minimum Stok</th>
                                <th>Eksik Miktar</th>
                                <th>Birim</th>
                                <th>Durum</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptMinimumStokRaporu" runat="server">
                                <ItemTemplate>
                                    <tr class="table-warning">
                                        <td><%# Eval("DepoAdi") %></td>
                                        <td><%# Eval("UrunAdi") %></td>
                                        <td class="text-right text-danger"><%# Eval("Miktar", "{0:N2}") %></td>
                                        <td class="text-right"><%# Eval("MinimumMiktar", "{0:N2}") %></td>
                                        <td class="text-right text-danger"><%# Eval("EksikMiktar", "{0:N2}") %></td>
                                        <td><%# Eval("BirimAdi") %></td>
                                        <td>
                                            <span class="badge badge-danger">Kritik Seviye</span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </div>
    </section>

</asp:Content>
