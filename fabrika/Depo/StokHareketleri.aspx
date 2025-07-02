<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="StokHareketleri.aspx.cs" Inherits="fabrika_Depo_StokHareketleri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- Üst Butonlar -->
    <section class="panel">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default btn-block" 
                        Text="← Geri Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokGirisi" runat="server" CssClass="btn btn-shadow btn-success btn-block" 
                        Text="+ Stok Girişi" OnClick="btnStokGirisi_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokCikisi" runat="server" CssClass="btn btn-shadow btn-danger btn-block" 
                        Text="- Stok Çıkışı" OnClick="btnStokCikisi_Click" />
                </div>
                <div class="col-md-3 text-right">
                    <span class="badge badge-info" style="font-size: 14px;">
                        <asp:Label ID="lblToplamHareket" runat="server" Text="0"></asp:Label>
                        <i class="fa fa-list"></i> Toplam Hareket
                    </span>
                </div>
            </div>
        </div>
    </section>

    <!-- Filtreler -->
    <section class="panel">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <h4><strong><asp:Label ID="lblDepoAdi" runat="server" Text="Stok Hareketleri"></asp:Label></strong></h4>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlHareketTipi" runat="server" CssClass="form-control" 
                        OnSelectedIndexChanged="ddlHareketTipi_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Tüm Hareketler" Value="" />
                        <asp:ListItem Text="Giriş" Value="GIRIS" />
                        <asp:ListItem Text="Çıkış" Value="CIKIS" />
                        <asp:ListItem Text="Transfer" Value="TRANSFER" />
                        <asp:ListItem Text="Satış" Value="SATIS" />
                        <asp:ListItem Text="İade" Value="IADE" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control" 
                        TextMode="Date" OnTextChanged="txtTarih_TextChanged" AutoPostBack="true" />
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" 
                        TextMode="Date" OnTextChanged="txtTarih_TextChanged" AutoPostBack="true" />
                </div>
                <div class="col-md-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" 
                            placeholder="Ürün adı veya açıklama..." AutoPostBack="True" 
                            OnTextChanged="txtArama_TextChanged" />
                        <span class="input-group-btn">
                            <button class="btn btn-default" type="button">
                                <i class="fa fa-search"></i>
                            </button>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Stok Hareketleri Listesi -->
    <section class="panel">
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Tarih</th>
                            <th>Ürün</th>
                            <th>Hareket Tipi</th>
                            <th>Miktar</th>
                            <th>Birim</th>
                            <th>Açıklama</th>
                            <th>Kullanıcı</th>
                            <th>İşlemler</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptStokHareketleri" runat="server" OnItemCommand="rptStokHareketleri_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Tarih", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td><strong><%# Eval("UrunAdi") %></strong></td>
                                    <td>
                                        <span class="badge <%# GetHareketTipiBadgeClass(Eval("HareketTipi").ToString()) %>">
                                            <%# GetHareketTipiText(Eval("HareketTipi").ToString()) %>
                                        </span>
                                    </td>
                                    <td class="<%# Convert.ToDecimal(Eval("Miktar")) > 0 ? "text-success" : "text-danger" %>">
                                        <strong><%# Eval("Miktar", "{0:N2}") %></strong>
                                    </td>
                                    <td><%# Eval("Birim") %></td>
                                    <td><%# Eval("Aciklama") %></td>
                                    <td><%# Eval("KullaniciAdi") %></td>
                                    <td>
                                        <asp:LinkButton ID="btnDetay" runat="server" CssClass="btn btn-info btn-xs" 
                                            CommandName="Detay" CommandArgument='<%# Eval("HareketID") %>' ToolTip="Detay">
                                            <i class="fa fa-eye"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="active">
                                    <td><%# Eval("Tarih", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td><strong><%# Eval("UrunAdi") %></strong></td>
                                    <td>
                                        <span class="badge <%# GetHareketTipiBadgeClass(Eval("HareketTipi").ToString()) %>">
                                            <%# GetHareketTipiText(Eval("HareketTipi").ToString()) %>
                                        </span>
                                    </td>
                                    <td class="<%# Convert.ToDecimal(Eval("Miktar")) > 0 ? "text-success" : "text-danger" %>">
                                        <strong><%# Eval("Miktar", "{0:N2}") %></strong>
                                    </td>
                                    <td><%# Eval("Birim") %></td>
                                    <td><%# Eval("Aciklama") %></td>
                                    <td><%# Eval("KullaniciAdi") %></td>
                                    <td>
                                        <asp:LinkButton ID="btnDetay" runat="server" CssClass="btn btn-info btn-xs" 
                                            CommandName="Detay" CommandArgument='<%# Eval("HareketID") %>' ToolTip="Detay">
                                            <i class="fa fa-eye"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

            <!-- Hareket bulunamadı mesajı -->
            <asp:Panel ID="pnlHareketYok" runat="server" Visible="false" CssClass="alert alert-info text-center">
                <i class="fa fa-info-circle fa-2x"></i>
                <h4>Stok Hareketi Bulunamadı</h4>
                <p>Bu depo için henüz stok hareketi kaydı bulunmamaktadır.</p>
            </asp:Panel>
        </div>
    </section>

    <!-- Stok Hareket Formu Modal -->
    <div class="modal fade" id="stokHareketModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="lblModalBaslik" runat="server" Text="Stok Hareketi"></asp:Label>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="pnlStokHareketForm" runat="server" Visible="false">
                        <asp:HiddenField ID="hfHareketID" runat="server" />
                        
                        <div class="form-group">
                            <label>Ürün *</label>
                            <asp:DropDownList ID="ddlUrun" runat="server" CssClass="form-control" 
                                DataTextField="UrunAdi" DataValueField="UrunID">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUrun" runat="server" 
                                ControlToValidate="ddlUrun" ErrorMessage="Ürün seçimi zorunludur!" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="StokHareket" />
                        </div>
                        
                        <div class="form-group">
                            <label>Miktar *</label>
                            <asp:TextBox ID="txtMiktar" runat="server" CssClass="form-control" 
                                TextMode="Number" step="0.01" />
                            <asp:RequiredFieldValidator ID="rfvMiktar" runat="server" 
                                ControlToValidate="txtMiktar" ErrorMessage="Miktar zorunludur!" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="StokHareket" />
                            <asp:RangeValidator ID="rvMiktar" runat="server" 
                                ControlToValidate="txtMiktar" MinimumValue="0.01" MaximumValue="999999" 
                                Type="Double" ErrorMessage="Geçerli bir miktar girin!" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="StokHareket" />
                        </div>
                        
                        <div class="form-group">
                            <label>Açıklama</label>
                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" 
                                TextMode="MultiLine" Rows="3" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnHareketKaydet" runat="server" CssClass="btn btn-primary" 
                        Text="Kaydet" OnClick="btnHareketKaydet_Click" ValidationGroup="StokHareket" />
                    <asp:Button ID="btnHareketIptal" runat="server" CssClass="btn btn-default" 
                        Text="İptal" OnClick="btnHareketIptal_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
