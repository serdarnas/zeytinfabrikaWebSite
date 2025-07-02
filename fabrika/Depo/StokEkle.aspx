<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="StokEkle.aspx.cs" Inherits="fabrika_Depo_StokEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-6">
                    <h4><strong>Stok Ekleme - <asp:Label ID="lblDepoAdi" runat="server" Text=""></asp:Label></strong></h4>
                </div>
                <div class="col-md-6 text-right">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default" Text="← Geri Dön" OnClick="btnGeriDon_Click" />
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">Ürün Seçimi</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlUrun" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlUrun_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Ürün Seçiniz..." Value="" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvUrun" runat="server" ControlToValidate="ddlUrun" 
                            ErrorMessage="Ürün seçimi zorunludur." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Mevcut Stok</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtMevcutStok" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <label class="col-sm-2 control-label">Birim</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtBirim" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Eklenecek Miktar</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtEklenecekMiktar" runat="server" CssClass="form-control" placeholder="0,00" />
                        <asp:RequiredFieldValidator ID="rfvMiktar" runat="server" ControlToValidate="txtEklenecekMiktar" 
                            ErrorMessage="Miktar girişi zorunludur." CssClass="text-danger" Display="Dynamic" />
                        <asp:RangeValidator ID="rvMiktar" runat="server" ControlToValidate="txtEklenecekMiktar" 
                            Type="Double" MinimumValue="0,01" MaximumValue="999999" 
                            ErrorMessage="Geçerli bir miktar giriniz (0,01 - 999999)." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Minimum Stok</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtMinimumStok" runat="server" CssClass="form-control" placeholder="0,00" />
                        <asp:RangeValidator ID="rvMinimumStok" runat="server" ControlToValidate="txtMinimumStok" 
                            Type="Double" MinimumValue="0" MaximumValue="999999" 
                            ErrorMessage="Geçerli bir minimum stok giriniz (0 - 999999)." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Açıklama</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" 
                            placeholder="Stok ekleme ile ilgili açıklama..." />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnStokEkle" runat="server" CssClass="btn btn-success btn-lg" Text="Stok Ekle" OnClick="btnStokEkle_Click" />
                        <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-default btn-lg" Text="Temizle" OnClick="btnTemizle_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Son Eklenen Stoklar -->
    <section class="panel">
        <header class="panel-heading">
            <h4>Son Eklenen Stoklar</h4>
        </header>
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Tarih</th>
                            <th>Ürün</th>
                            <th class="text-right">Miktar</th>
                            <th>Birim</th>
                            <th>Açıklama</th>
                            <th>Kullanıcı</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptSonStoklar" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("IslemTarihi", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td><%# Eval("UrunAdi") %></td>
                                    <td class="text-right"><%# Eval("Miktar", "{0:N2}") %></td>
                                    <td><%# Eval("BirimAdi") %></td>
                                    <td><%# Eval("Aciklama") %></td>
                                    <td><%# Eval("KullaniciAdi") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="active">
                                    <td><%# Eval("IslemTarihi", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td><%# Eval("UrunAdi") %></td>
                                    <td class="text-right"><%# Eval("Miktar", "{0:N2}") %></td>
                                    <td><%# Eval("BirimAdi") %></td>
                                    <td><%# Eval("Aciklama") %></td>
                                    <td><%# Eval("KullaniciAdi") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </section>



</asp:Content>
