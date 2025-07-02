<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="StokTransfer.aspx.cs" Inherits="fabrika_Depo_StokTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-6">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default" Text="← Geri Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-6 text-right">
                    <h4><strong>Stok Transfer İşlemi</strong></h4>
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Kaynak Depo</label>
                            <asp:Label ID="lblKaynakDepo" runat="server" CssClass="form-control-static" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Hedef Depo <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlHedefDepo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHedefDepo_SelectedIndexChanged">
                                <asp:ListItem Text="Hedef depo seçiniz..." Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvHedefDepo" runat="server" ControlToValidate="ddlHedefDepo" 
                                ErrorMessage="Hedef depo seçimi zorunludur" CssClass="text-danger" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Ürün <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlUrun" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUrun_SelectedIndexChanged">
                                <asp:ListItem Text="Ürün seçiniz..." Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUrun" runat="server" ControlToValidate="ddlUrun" 
                                ErrorMessage="Ürün seçimi zorunludur" CssClass="text-danger" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Mevcut Stok</label>
                            <asp:Label ID="lblMevcutStok" runat="server" CssClass="form-control-static text-info" Text="0"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Transfer Miktarı <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtMiktar" runat="server" CssClass="form-control" placeholder="0.00" />
                            <asp:RequiredFieldValidator ID="rfvMiktar" runat="server" ControlToValidate="txtMiktar" 
                                ErrorMessage="Miktar girişi zorunludur" CssClass="text-danger" Display="Dynamic" />
                            <asp:RangeValidator ID="rvMiktar" runat="server" ControlToValidate="txtMiktar" 
                                Type="Double" MinimumValue="0,01" MaximumValue="999999" 
                                ErrorMessage="Geçerli bir miktar giriniz" CssClass="text-danger" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Birim</label>
                            <asp:Label ID="lblBirim" runat="server" CssClass="form-control-static" Text=""></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Açıklama</label>
                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" 
                                placeholder="Transfer açıklaması..." />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 text-center">
                        <asp:Button ID="btnTransferYap" runat="server" CssClass="btn btn-success btn-lg" 
                            Text="Transfer Yap" OnClick="btnTransferYap_Click" />
                        <asp:Button ID="btnIptal" runat="server" CssClass="btn btn-default btn-lg" 
                            Text="İptal" OnClick="btnIptal_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Transfer Geçmişi -->
    <section class="panel">
        <header class="panel-heading">
            <h4>Son Transfer İşlemleri</h4>
        </header>
        <div class="panel-body">
            <asp:Panel ID="pnlTransferYok" runat="server" Visible="false" CssClass="alert alert-info">
                <i class="fa fa-info-circle"></i> Henüz transfer işlemi yapılmamış.
            </asp:Panel>
            
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Tarih</th>
                            <th>Ürün</th>
                            <th>Kaynak Depo</th>
                            <th>Hedef Depo</th>
                            <th>Miktar</th>
                            <th>Açıklama</th>
                            <th>Kullanıcı</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptTransferler" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("IslemTarihi", "{0:dd.MM.yyyy HH:mm}") %></td>
                                    <td><%# Eval("UrunAdi") %></td>
                                    <td><%# Eval("KaynakDepoAdi") %></td>
                                    <td><%# Eval("HedefDepoAdi") %></td>
                                    <td class="text-right">
                                        <span class="badge badge-info"><%# Eval("Miktar", "{0:N2}") %> <%# Eval("Birim") %></span>
                                    </td>
                                    <td><%# Eval("Aciklama") %></td>
                                    <td><%# Eval("KullaniciAdi") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </section>

</asp:Content>
