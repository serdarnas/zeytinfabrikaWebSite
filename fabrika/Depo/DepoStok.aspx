<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="DepoStok.aspx.cs" Inherits="fabrika_Depo_DepoStok" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default btn-block" Text="← Geri Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokEkle" runat="server" CssClass="btn btn-shadow btn-success btn-block" Text="+ Stok Ekle" OnClick="btnStokEkle_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnStokTransfer" runat="server" CssClass="btn btn-shadow btn-warning btn-block" Text="⇄ Stok Transfer" OnClick="btnStokTransfer_Click" />
                </div>
                <div class="col-md-3 text-right">
                    <span class="badge badge-info" style="font-size: 18px;">
                        <asp:Label ID="lblToplamUrun" runat="server" Text="0"></asp:Label>
                        <i class="fa fa-boxes"></i> Toplam Ürün
                    </span>
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-4">
                    <h4><strong><asp:Label ID="lblDepoAdi" runat="server" Text="Depo Stok Listesi"></asp:Label></strong></h4>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlStokDurumu" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlStokDurumu_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Tüm Ürünler" Value="" />
                        <asp:ListItem Text="Stokta Var" Value="1" />
                        <asp:ListItem Text="Stok Yok" Value="0" />
                        <asp:ListItem Text="Minimum Stok Altında" Value="2" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Ürün adı..." AutoPostBack="True" OnTextChanged="txtArama_TextChanged" />
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Ürün Adı</th>
                            <th>Ürün Kodu</th>
                            <th>Birim</th>
                            <th>Stok Miktarı</th>
                            <th>Minimum Stok</th>
                            <th>Stok Durumu</th>
                            <th>Son Güncelleme</th>
                            <th>İşlemler</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptDepoStok" runat="server" OnItemCommand="rptDepoStok_ItemCommand">
                            <ItemTemplate>
                                <tr class="<%# GetRowClass(Convert.ToDecimal(Eval("Miktar")), Convert.ToDecimal(Eval("MinimumMiktar") ?? 0)) %>">
                                    <td>
                                        <div style="border-radius: 6px; padding: 8px 12px;">
                                            <strong><%# Eval("UrunAdi") %></strong>
                                        </div>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold; color: #007bff;"><%# Eval("UrunKodu") %></span>
                                    </td>
                                    <td>
                                        <span class="badge badge-secondary"><%# Eval("BirimAdi") %></span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold; font-size: 14px;"><%# Eval("Miktar", "{0:N2}") %></span>
                                    </td>
                                    <td>
                                        <span style="color: #dc3545;"><%# Eval("MinimumMiktar", "{0:N2}") %></span>
                                    </td>
                                    <td>
                                        <span class="badge <%# GetStokDurumBadgeClass(Convert.ToDecimal(Eval("Miktar")), Convert.ToDecimal(Eval("MinimumMiktar") ?? 0)) %>">
                                            <%# GetStokDurumText(Convert.ToDecimal(Eval("Miktar")), Convert.ToDecimal(Eval("MinimumMiktar") ?? 0)) %>
                                        </span>
                                    </td>
                                    <td>
                                        <%# Eval("SonGuncelleme", "{0:dd.MM.yyyy}") %>
                                    </td>
                                    <td>
                                        <div class="btn-group btn-group-sm">
                                            <asp:LinkButton ID="btnStokHareketleri" runat="server" CssClass="btn btn-info btn-sm" 
                                                CommandName="Hareketler" CommandArgument='<%# Eval("UrunID") %>' ToolTip="Stok Hareketleri">
                                                <i class="fa fa-history"></i> Hareket
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnStokGuncelle" runat="server" CssClass="btn btn-warning btn-sm" 
                                                CommandName="Guncelle" CommandArgument='<%# Eval("DepoStokID") %>' ToolTip="Stok Güncelle">
                                                <i class="fa fa-edit"></i> Güncelle
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </section>

    <!-- Stok Güncelleme Modal -->
    <div class="modal fade" id="stokGuncelleModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Stok Güncelle</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnDepoStokID" runat="server" />
                    <div class="form-group">
                        <label>Yeni Miktar:</label>
                        <asp:TextBox ID="txtYeniMiktar" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                    </div>
                    <div class="form-group">
                        <label>Minimum Stok:</label>
                        <asp:TextBox ID="txtMinimumStok" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnStokGuncelle" runat="server" CssClass="btn btn-primary" Text="Güncelle" OnClick="btnStokGuncelle_Click" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">İptal</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showStokGuncelle(depoStokID) {
            document.getElementById('<%= hdnDepoStokID.ClientID %>').value = depoStokID;
            $('#stokGuncelleModal').modal('show');
        }

        function showStokHareketleri(urunID) {
            var depoID = '<%= Request.QueryString["id"] ?? "0" %>';
            window.open('StokHareketleri.aspx?urunID=' + urunID + '&depoID=' + depoID, '_blank');
        }
    </script>
</asp:Content>

