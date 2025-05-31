<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Musteriler_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">

        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:Button ID="btnYeniMusteri" runat="server" CssClass="btn btn-shadow btn-success btn-block" PostBackUrl="YeniMusteri.aspx" Text="+ Yeni Müşteri Ekle" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnExcelYukle" runat="server" CssClass="btn btn-shadow btn-warning btn-block" PostBackUrl="musteri_yukle_excel.aspx" Text="Excelden Müşteri Yükle" />
                </div>
                <div class="col-md-3"></div>
                <div class="col-md-3 text-right">
                    <span class="badge badge-success" style="font-size: 18px;">
                        <asp:Label ID="lblToplamMusteri" runat="server" Text="0"></asp:Label>

                        <i class="fa fa-users"></i>Toplam Müşteri
                    </span>
                </div>
            </div>
        </div>

    </section>
    <section class="panel">

        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDurum_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Aktif Müşteri" Value="1" />
                        <asp:ListItem Text="Pasif Müşteri" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlSinif" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSinif_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Tüm Kategoriler" Value="" />
                    </asp:DropDownList>
                </div>
                <%--       <div class="col-md-3">
                    <asp:DropDownList ID="ddlGoster" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Hepsini göster" Value="" />
                    </asp:DropDownList>
                </div>--%>
                <div class="col-md-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Arama... (en az 3 karakter)" AutoPostBack="True" OnTextChanged="btnAra_Click" />
                    </div>
                </div>
            </div>
    </section>
    <section class="panel">
        <%--  <header class="panel-heading">
            Müşteriler
   
        </header>--%>
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>İsim / Unvan</th>
                            <th>Kategori</th>
                            <th>Açık Bakiye</th>
                            <th>Çek/Senet Bakiyesi</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptMusteriler" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div style=" border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">
                                            <a href='MusteriDetay.aspx?id=<%# Eval("MusteriID") %>' style="  text-decoration: none; font-weight: bold;">
                                                <%# Eval("FirmaAdi") %>
                                            </a>
                                           <%-- <span style=  border-radius: 4px; padding: 2px 8px; margin-left: 10px; font-weight: bold;">
                                                <%# Eval("Telefon") %>
                                            </span>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <span style="  border-radius: 4px; padding: 4px 10px; font-size: 13px;">
                                            <%# Eval("KategoriAdi") %>
                                        </span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold;"><%# Eval("AcikBakiye", "{0:N2} TL") %></span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold;"><%# Eval("CekSenetBakiye", "{0:N2}") %></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

        </div>
    </section>
</asp:Content>

