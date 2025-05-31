<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Tedarikciler_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">

                <div class="panel-body">
                    <!-- Üst Menü Butonları -->
                    <div class="btn-group">

                        <asp:HyperLink ID="btnYeni" runat="server" NavigateUrl="YeniTedarikci.aspx" CssClass="btn btn-shadow  btn-success" Style="margin-right: 5px;">
                        <i class="icon-plus"></i> Yeni Tedarikçi Ekle
                        </asp:HyperLink>
                        <asp:HyperLink ID="btnExcelIndir" runat="server" NavigateUrl="Tedarikci_yukle_excel.aspx" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                        <i class="icon-file-excel"></i> Excel'den Tedarikçi Yükle
                        </asp:HyperLink>

                        <div class="input-group">
                            <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Arama... (en az 3 karakter)" AutoPostBack="True" OnTextChanged="btnAra_Click" />
                        </div>

                    </div>
                </div>
            </section>
        </div>
    </div>
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
                            <th>Açık Bakiye</th>
                            <th>Çek/Senet Bakiyesi</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptTedarikciler" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div>
                                            <%--<div style="background: #4fc3f7; color: white; border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">--%>
                                            <a href='Detay.aspx?id=<%# Eval("TedarikciID") %>' style="color: black; text-decoration: none; font-weight: bold;">
                                                <%# Eval("FirmaAdi") %>
                                            </a>

                                        </div>
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

