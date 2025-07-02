<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="DepoDetay.aspx.cs" Inherits="fabrika_Depo_DepoDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .error-message {
            color: #dc3545;
            font-weight: bold;
            text-align: center;
            padding: 20px;
        }
        .loading-spinner {
            text-align: center;
            padding: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default btn-block" Text="← Depo Listesine Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnDuzenle" runat="server" CssClass="btn btn-shadow btn-warning btn-block" Text="✏ Depo Düzenle" OnClick="btnDuzenle_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokGoruntule" runat="server" CssClass="btn btn-shadow btn-info btn-block" Text="📦 Stok Görüntüle" OnClick="btnStokGoruntule_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokHareketleri" runat="server" CssClass="btn btn-shadow btn-success btn-block" Text="📊 Stok Hareketleri" OnClick="btnStokHareketleri_Click" />
                </div>
            </div>
        </div>
    </section>

    <div class="row">
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Depo Bilgileri</h3>
                </header>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Depo Adı:</label>
                                <div style="padding: 8px; font-weight: bold; font-size: 16px; color: #2c3e50;">
                                    <asp:Label ID="lblDepoAdi" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Depo Kodu:</label>
                                <div style="padding: 8px; font-weight: bold; color: #007bff;">
                                    <asp:Label ID="lblDepoKodu" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Durum:</label>
                                <div style="padding: 8px;">
                                    <asp:Label ID="lblDurum" runat="server" Text="" CssClass="badge"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Kapasite (Kg):</label>
                                <div style="padding: 8px; font-weight: bold; color: #28a745;">
                                    <asp:Label ID="lblKapasite" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Dolu Miktar (Kg):</label>
                                <div style="padding: 8px; font-weight: bold; color: #dc3545;">
                                    <asp:Label ID="lblDoluMiktar" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Doluluk Oranı:</label>
                                <div style="padding: 8px;">
                                    <div class="progress" style="height: 25px;">
                                        <div id="progressBar" runat="server" class="progress-bar" 
                                             role="progressbar" 
                                             style="height: 25px; line-height: 25px; font-weight: bold;">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Boş Kapasite (Kg):</label>
                                <div style="padding: 8px; font-weight: bold; color: #17a2b8;">
                                    <asp:Label ID="lblBosKapasite" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Ürün Çeşidi:</label>
                                <div style="padding: 8px; font-weight: bold; color: #6f42c1;">
                                    <asp:Label ID="lblUrunCesidi" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>

        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Stok Özeti</h3>
                </header>
                <div class="panel-body">
                    <div class="table-responsive">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Ürün Adı</th>
                                    <th>Miktar (Kg)</th>
                                    <th>Oran (%)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptStokOzeti" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("UrunAdi") %></td>
                                            <td><strong><%# Eval("Miktar", "{0:N2}") %></strong></td>
                                            <td>
                                                <span class="badge badge-info"><%# Eval("Oran", "{0:N1}") %>%</span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Tank Yönetimi Bölümü -->
    <section class="panel">
        <header class="panel-heading">
            <h3>Tank Yönetimi</h3>
            <div class="panel-actions">
                <asp:Button ID="btnYeniTank" runat="server" CssClass="btn btn-primary btn-sm" Text="+ Yeni Tank Ekle" OnClick="btnYeniTank_Click" />
            </div>
        </header>
        <div class="panel-body">
            <!-- Tank Ekleme/Düzenleme Formu -->
            <asp:Panel ID="pnlTankForm" runat="server" Visible="false" CssClass="well">
                <h4>Tank Bilgileri</h4>
                <asp:HiddenField ID="hfTankID" runat="server" />
                <div class="row">
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Tank Adı *</label>
                            <asp:TextBox ID="txtTankAdi" runat="server" CssClass="form-control" MaxLength="100" placeholder="Örn: Tank A1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTankAdi" runat="server" ControlToValidate="txtTankAdi" 
                                ErrorMessage="Tank adı zorunludur" CssClass="text-danger" ValidationGroup="TankForm" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Tank Kodu</label>
                            <asp:TextBox ID="txtTankKodu" runat="server" CssClass="form-control" MaxLength="50" placeholder="Örn: T001"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Kapasite (Litre) *</label>
                            <asp:TextBox ID="txtKapasite" runat="server" CssClass="form-control" TextMode="Number" placeholder="Örn: 5000"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvKapasite" runat="server" ControlToValidate="txtKapasite" 
                                ErrorMessage="Kapasite zorunludur" CssClass="text-danger" ValidationGroup="TankForm" Display="Dynamic" />
                            <asp:RangeValidator ID="rvKapasite" runat="server" ControlToValidate="txtKapasite" 
                                MinimumValue="1" MaximumValue="999999" Type="Double" 
                                ErrorMessage="Kapasite 1-999999 arasında olmalıdır" CssClass="text-danger" ValidationGroup="TankForm" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Dolu Miktar (Litre)</label>
                            <asp:TextBox ID="txtDoluMiktar" runat="server" CssClass="form-control" TextMode="Number" placeholder="Örn: 2500" Text="0"></asp:TextBox>
                            <asp:RangeValidator ID="rvDoluMiktar" runat="server" ControlToValidate="txtDoluMiktar" 
                                MinimumValue="0" MaximumValue="999999" Type="Double" 
                                ErrorMessage="Dolu miktar 0-999999 arasında olmalıdır" CssClass="text-danger" ValidationGroup="TankForm" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Durum</label>
                            <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-control">
                                <asp:ListItem Value="Aktif" Text="Aktif" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Pasif" Text="Pasif"></asp:ListItem>
                                <asp:ListItem Value="Bakımda" Text="Bakımda"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="form-group">
                            <label class="control-label">&nbsp;</label><br />
                            <asp:Button ID="btnTankKaydet" runat="server" CssClass="btn btn-success" Text="Kaydet" OnClick="btnTankKaydet_Click" ValidationGroup="TankForm" />
                            <asp:Button ID="btnTankIptal" runat="server" CssClass="btn btn-default" Text="İptal" OnClick="btnTankIptal_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Tank Listesi -->
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Tank Adı</th>
                            <th>Tank Kodu</th>
                            <th>Kapasite (L)</th>
                            <th>Dolu Miktar (L)</th>
                            <th>Doluluk Oranı</th>
                            <th>Durum</th>
                            <th>İşlemler</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptTanklar" runat="server" OnItemCommand="rptTanklar_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td><strong><%# Eval("TankAdi") %></strong></td>
                                    <td><%# Eval("TankKodu") %></td>
                                    <td><%# Eval("Kapasite", "{0:N0}") %></td>
                                    <td><%# Eval("DoluMiktar", "{0:N0}") %></td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar <%# GetDolulukBarClass(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>" 
                                                 style="width: <%# GetDolulukYuzdesi(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>%">
                                                <%# GetDolulukYuzdesi(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>%
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <span class="badge <%# GetDurumBadgeClass(GetDurumText(Eval("Durum"))) %>">
                                            <%# GetDurumText(Eval("Durum")) %>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnDuzenle" runat="server" CssClass="btn btn-warning btn-xs" 
                                            CommandName="Duzenle" CommandArgument='<%# Eval("TankID") %>' ToolTip="Düzenle">
                                            <i class="fa fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnSil" runat="server" CssClass="btn btn-danger btn-xs" 
                                            CommandName="Sil" CommandArgument='<%# Eval("TankID") %>' ToolTip="Sil"
                                            OnClientClick="return confirm('Bu tankı silmek istediğinizden emin misiniz?');">
                                            <i class="fa fa-trash"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="active">
                                    <td><strong><%# Eval("TankAdi") %></strong></td>
                                    <td><%# Eval("TankKodu") %></td>
                                    <td><%# Eval("Kapasite", "{0:N0}") %></td>
                                    <td><%# Eval("DoluMiktar", "{0:N0}") %></td>
                                    <td>
                                        <div class="progress" style="margin-bottom: 0;">
                                            <div class="progress-bar <%# GetDolulukBarClass(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>" 
                                                 style="width: <%# GetDolulukYuzdesi(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>%">
                                                <%# GetDolulukYuzdesi(Convert.ToDouble(Eval("DoluMiktar")), Convert.ToDouble(Eval("Kapasite"))) %>%
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <span class="badge <%# GetDurumBadgeClass(GetDurumText(Eval("Durum"))) %>">
                                            <%# GetDurumText(Eval("Durum")) %>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnDuzenle" runat="server" CssClass="btn btn-warning btn-xs" 
                                            CommandName="Duzenle" CommandArgument='<%# Eval("TankID") %>' ToolTip="Düzenle">
                                            <i class="fa fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnSil" runat="server" CssClass="btn btn-danger btn-xs" 
                                            CommandName="Sil" CommandArgument='<%# Eval("TankID") %>' ToolTip="Sil"
                                            OnClientClick="return confirm('Bu tankı silmek istediğinizden emin misiniz?');">
                                            <i class="fa fa-trash"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

            <!-- Tank bulunamadı mesajı -->
            <asp:Panel ID="pnlTankYok" runat="server" Visible="false" CssClass="alert alert-info text-center">
                <h4>Bu depoda henüz tank bulunmuyor.</h4>
                <p>Yeni tank eklemek için yukarıdaki "+ Yeni Tank Ekle" butonunu kullanabilirsiniz.</p>
            </asp:Panel>
        </div>
    </section>

    <section class="panel">
        <header class="panel-heading">
            <h3>Son Stok Hareketleri</h3>
        </header>
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Tarih</th>
                            <th>Hareket Tipi</th>
                            <th>Ürün</th>
                            <th>Miktar (Kg)</th>
                            <th>Referans</th>
                            <th>Açıklama</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptStokHareketleri" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("IslemTarihi", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td>
                                        <span class="badge <%# GetHareketTipiBadgeClass(Eval("HareketTipi").ToString()) %>">
                                            <%# Eval("HareketTipi") %>
                                        </span>
                                    </td>
                                    <td><%# Eval("UrunAdi") %></td>
                                    <td><strong><%# Eval("Miktar", "{0:N2}") %></strong></td>
                                    <td><%# Eval("ReferansNo") %></td>
                                    <td><%# Eval("Aciklama") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function confirmTankDelete(tankAdi) {
            return confirm('"' + tankAdi + '" adlı tankı silmek istediğinizden emin misiniz?\n\nBu işlem geri alınamaz!');
        }
    </script>
</asp:Content>

