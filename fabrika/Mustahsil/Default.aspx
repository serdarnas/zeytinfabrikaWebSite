<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Mustahsil_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12"><section class="panel">
                <div class="panel-body">
                    <!-- Üst Menü Butonları -->
                    <div class="btn-group">
                        <asp:HyperLink ID="btnYeniEkle" runat="server" NavigateUrl="YeniMustahsil.aspx" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                            <i class="icon-plus"></i> Yeni Müstahsil Ekle
                        </asp:HyperLink>

                        <asp:HyperLink ID="btnExcelIndir" runat="server" NavigateUrl="Mustahsil_yukle_excel.aspx" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                            <i class="icon-file-excel"></i> Excel'den Müstahsil Yükle
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLinkCksileYuke" runat="server" NavigateUrl="Mustahsil_yukle_Cks_pdf.aspx" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                            <i class="icon-file-excel"></i> ÇKS'den Müstahsil Yükle veya Oluştur
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
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Ad Soyad</th>
                            <th>TC Kimlik No</th>
                            <th>Bakiye</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptMustahsiller" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div>
                                            <a href='MustahsilDetay.aspx?id=<%# Eval("MustahsilID") %>' style="color: black; text-decoration: none; font-weight: bold;">
                                                <%# Eval("Ad") %> <%# Eval("Soyad") %>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        <%# Eval("TCKimlikNo") %>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold;"><%# Eval("Bakiyesi", "{0:N2} TL") %></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <asp:HiddenField ID="HiddenFieldSirketID" runat="server" />
        </div>
    </section>
</asp:Content>

